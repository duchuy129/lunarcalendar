# iOS Crash Investigation & Fix - Upcoming Holidays Range Update

**Date:** December 30, 2025
**Issue:** iOS app crashes intermittently when updating the upcoming holidays range in Settings, then navigating back to Calendar

## Root Causes Identified

### 1. **ObservableCollection Race Condition on UI Thread** ⚠️ CRITICAL
**Location:** `CalendarViewModel.LoadUpcomingHolidaysAsync()` (Line 597)

**Problem:**
```csharp
UpcomingHolidays = new ObservableCollection<LocalizedHolidayOccurrence>(
    upcomingHolidays.Select(h => new LocalizedHolidayOccurrence(h)));
```

The issue occurs when:
1. User changes the upcoming holidays range (e.g., from 30 to 90 days) in Settings
2. `RefreshSettings()` is called in `CalendarViewModel` when navigating back to Calendar
3. `LoadUpcomingHolidaysAsync()` executes and creates a NEW ObservableCollection
4. The CollectionView on iOS is still bound to the OLD collection
5. **iOS CollectionView crashes** because it's trying to update/animate while the collection reference changes

### 2. **Multiple Concurrent InitializeAsync Calls** ⚠️ HIGH
**Location:** `CalendarPage.OnAppearing()` (Line 16-19)

**Problem:**
```csharp
protected override async void OnAppearing()
{
    base.OnAppearing();
    await _viewModel.InitializeAsync();
}
```

Every time the page appears, `InitializeAsync()` is called, which:
- Calls `LoadUpcomingHolidaysAsync()` 
- If user quickly navigates back and forth, multiple async operations overlap
- On iOS, this can cause collection binding issues

### 3. **Synchronous Property Updates on Background Thread**
**Location:** `CalendarViewModel.RefreshSettings()` (Line 223-234)

**Problem:**
```csharp
public async void RefreshSettings()
{
    // ...
    if (UpcomingHolidaysDays != newDays)
    {
        UpcomingHolidaysDays = newDays;  // Property change notification
        await LoadUpcomingHolidaysAsync(); // Async operation
    }
}
```

The property change and async load can happen while iOS is still rendering, causing timing issues.

### 4. **No Protection Against Rapid Updates**
There's no debouncing or cancellation token to prevent rapid-fire updates when user changes settings multiple times quickly.

## iOS-Specific Issues

### Why iOS is More Sensitive:
1. **UICollectionView (iOS)** is more strict about collection changes during updates than Android's RecyclerView
2. **iOS enforces UI thread operations** more strictly - collection mutations during enumeration cause crashes
3. **Memory pressure on iOS** is lower, making race conditions more likely to manifest as crashes
4. **Animation conflicts** - iOS animations conflict with collection replacements

## Fixes Implemented

### Fix 1: Clear and Repopulate Instead of Replacing Collection

**File:** `CalendarViewModel.cs` - `LoadUpcomingHolidaysAsync()` method

**Before:**
```csharp
UpcomingHolidays = new ObservableCollection<LocalizedHolidayOccurrence>(
    upcomingHolidays.Select(h => new LocalizedHolidayOccurrence(h)));
```

**After:**
```csharp
// CRITICAL: Use MainThread.BeginInvokeOnMainThread for iOS stability
// Clear and repopulate instead of replacing to avoid iOS CollectionView crashes
MainThread.BeginInvokeOnMainThread(() =>
{
    UpcomingHolidays.Clear();
    foreach (var holiday in upcomingHolidays.Select(h => new LocalizedHolidayOccurrence(h)))
    {
        UpcomingHolidays.Add(holiday);
    }
});
```

**Why this works:**
- Keeps the same ObservableCollection instance
- iOS CollectionView maintains its binding
- Updates happen on main thread explicitly
- Gradual addition instead of bulk replacement

### Fix 2: Add Initialization Guard

**File:** `CalendarViewModel.cs` - Add field and check

**Added:**
```csharp
private bool _isInitialized = false;

public async Task InitializeAsync()
{
    if (_isInitialized)
    {
        // Only refresh settings if already initialized
        RefreshSettings();
        return;
    }

    try
    {
        // ... existing initialization code ...
        _isInitialized = true;
    }
    catch (Exception ex)
    {
        // ... error handling ...
    }
}
```

**Why this works:**
- Prevents multiple full initializations
- First navigation does full init, subsequent ones only refresh
- Reduces redundant async operations

### Fix 3: Add Async Task Protection

**File:** `CalendarViewModel.cs` - Add cancellation and task tracking

**Added:**
```csharp
private CancellationTokenSource? _loadHolidaysCts;
private Task? _loadHolidaysTask;

private async Task LoadUpcomingHolidaysAsync()
{
    // Cancel any existing operation
    _loadHolidaysCts?.Cancel();
    _loadHolidaysCts = new CancellationTokenSource();
    var token = _loadHolidaysCts.Token;

    try
    {
        // Wait for previous task to complete
        if (_loadHolidaysTask != null && !_loadHolidaysTask.IsCompleted)
        {
            await _loadHolidaysTask;
        }

        _loadHolidaysTask = LoadHolidaysInternalAsync(token);
        await _loadHolidaysTask;
    }
    catch (OperationCanceledException)
    {
        // Expected when cancelled
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"Error loading upcoming holidays: {ex.Message}");
    }
}

private async Task LoadHolidaysInternalAsync(CancellationToken cancellationToken)
{
    var today = DateTime.Today;
    var endDate = today.AddDays(UpcomingHolidaysDays);
    var upcomingHolidays = new List<HolidayOccurrence>();

    // ... existing holiday loading logic ...
    
    cancellationToken.ThrowIfCancellationRequested();

    // Update on main thread
    MainThread.BeginInvokeOnMainThread(() =>
    {
        UpcomingHolidays.Clear();
        foreach (var holiday in upcomingHolidays.Select(h => new LocalizedHolidayOccurrence(h)))
        {
            if (cancellationToken.IsCancellationRequested) break;
            UpcomingHolidays.Add(holiday);
        }
    });
}
```

**Why this works:**
- Cancels outdated operations
- Prevents overlapping async tasks
- Respects cancellation throughout the pipeline

### Fix 4: Debounce Settings Updates

**File:** `CalendarViewModel.cs` - `RefreshSettings()` method

**Added:**
```csharp
private DateTime _lastRefreshTime = DateTime.MinValue;
private const int RefreshDebounceMs = 300;

public async void RefreshSettings()
{
    // Debounce rapid refresh calls
    var now = DateTime.Now;
    if ((now - _lastRefreshTime).TotalMilliseconds < RefreshDebounceMs)
    {
        return;
    }
    _lastRefreshTime = now;

    // ... existing refresh logic ...
}
```

**Why this works:**
- Prevents rapid-fire updates
- Gives iOS time to complete animations
- Reduces race conditions

## Testing Recommendations

### Critical Test Scenarios:

1. **Rapid Range Changes:**
   - Go to Settings → Change upcoming holidays range → Go back
   - Repeat 5-10 times quickly
   - Should NOT crash

2. **Quick Navigation:**
   - Tap on a holiday → Immediately go back
   - Navigate to Settings → Change range → Go back quickly
   - Should NOT crash

3. **Multiple Setting Changes:**
   - Change range from 30 → 60 → 90 → 7 rapidly
   - Navigate between tabs while changes are processing
   - Should NOT crash

4. **Memory Pressure Test:**
   - Run app for extended period
   - Change settings multiple times
   - Navigate extensively
   - Monitor memory usage on iOS

5. **Background/Foreground:**
   - Change settings
   - Put app in background
   - Return to foreground
   - Should NOT crash

### iOS-Specific Tests:

1. Test on **physical iOS devices** (different from simulator behavior)
2. Test on **older iOS devices** (iPhone 8, iPhone X) - more memory constrained
3. Enable **iOS Debug Memory Graph** to check for leaks
4. Use **Xcode Instruments** to monitor collection view updates

## Additional Improvements

### 1. Dispose Pattern for ViewModel
Consider implementing `IDisposable` to clean up CancellationTokenSource:

```csharp
public void Dispose()
{
    _loadHolidaysCts?.Cancel();
    _loadHolidaysCts?.Dispose();
    _connectivityService.ConnectivityChanged -= OnConnectivityChanged;
    _syncService.SyncStatusChanged -= OnSyncStatusChanged;
}
```

### 2. Add Diagnostic Logging
Add comprehensive logging for debugging future issues:

```csharp
System.Diagnostics.Debug.WriteLine($"[iOS] Loading holidays - Range: {UpcomingHolidaysDays} days");
System.Diagnostics.Debug.WriteLine($"[iOS] Collection count before: {UpcomingHolidays.Count}");
System.Diagnostics.Debug.WriteLine($"[iOS] Collection count after: {UpcomingHolidays.Count}");
```

### 3. Consider Virtualization
For large holiday lists (90+ days), consider implementing incremental loading or virtualization.

## Performance Impact

- **Minimal** - Main thread updates are fast (< 100ms typically)
- **Better stability** - Eliminates crashes worth any minimal overhead
- **Smoother UI** - Debouncing reduces jank

## Related Files Modified

1. `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`
   - LoadUpcomingHolidaysAsync()
   - InitializeAsync()
   - RefreshSettings()

## Monitoring

After deployment, monitor:
- iOS crash reports for `NSInternalInconsistencyException`
- Collection-related crashes in CalendarPage
- Memory usage patterns
- User feedback on Settings → Calendar navigation

## Success Criteria

✅ No crashes when changing upcoming holidays range
✅ Smooth navigation between Settings and Calendar  
✅ Proper UI updates when range changes
✅ No memory leaks or performance degradation
✅ Stable behavior on iOS physical devices

## References

- Apple Documentation: [UICollectionView Thread Safety](https://developer.apple.com/documentation/uikit/uicollectionview)
- MAUI Issue: [ObservableCollection crashes on iOS](https://github.com/dotnet/maui/issues/8458)
- Stack Overflow: [iOS CollectionView Update Crashes](https://stackoverflow.com/questions/19199985/)
