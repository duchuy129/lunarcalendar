# Android Crash - Troubleshooting & Rollback

## Current Status

Android app is crashing immediately on device launch after applying the 7 safe fixes.

## What Changed

The following changes were applied (all of which compiled successfully):

1. **IDisposable to CalendarViewModel & YearHolidaysViewModel** - Memory leak prevention
2. **Removed fire-and-forget Task.Run** in CalendarService & HolidayService
3. **Fixed Task.Run in OnSelectedYearChanged** - Changed to MainThread.BeginInvokeOnMainThread
4. **Simplified RefreshSettingsAsync** - Removed manual semaphore handling
5. **ConcurrentDictionary in HolidayService** - Thread-safe caching

## Potential Causes

### Theory 1: MainThread.BeginInvokeOnMainThread on Android
The change in `YearHolidaysViewModel.OnSelectedYearChanged` uses `MainThread.BeginInvokeOnMainThread`, which might behave differently on Android during early initialization.

**Location:** `ViewModels/YearHolidaysViewModel.cs:131`

### Theory 2: Collection Initialization Timing
The removal of fire-and-forget Task.Run might have changed timing of when collections are initialized, causing a race condition on Android.

### Theory 3: ConcurrentDictionary.GetOrAdd
The change to `ConcurrentDictionary.GetOrAdd` in HolidayService might be executing the factory lambda at a different time on Android.

**Location:** `Services/HolidayService.cs:44-48`

### Theory 4: RefreshSettingsAsync Change
The simplified RefreshSettingsAsync might not work correctly on Android if LoadUpcomingHolidaysAsync behaves differently.

## Quick Rollback

To rollback all changes:

```bash
git stash
```

This will undo all uncommitted changes and restore the app to the state before the 7 fixes were applied.

## Gradual Rollback (Recommended)

To identify the specific change causing the issue, rollback changes one at a time:

### Step 1: Rollback MainThread.BeginInvokeOnMainThread Change

**File:** `ViewModels/YearHolidaysViewModel.cs:129-141`

**Revert to:**
```csharp
partial void OnSelectedYearChanged(int value)
{
    if (_isLanguageChanging) return;
    _hapticService.PerformSelection();

    // Revert to Task.Run for Android compatibility
    Task.Run(async () =>
    {
        try
        {
            await LoadYearHolidaysAsync();
        }
        catch (Exception ex)
        {
            _logService.LogError("Failed to load holidays for selected year", ex, "YearHolidaysViewModel.OnSelectedYearChanged");
        }
    });
}
```

**Test:** Deploy to Android device

### Step 2: Rollback ConcurrentDictionary Change

**File:** `Services/HolidayService.cs`

**Revert to:**
```csharp
private readonly Dictionary<int, List<HolidayOccurrence>> _yearCache = new();
private readonly object _yearCacheLock = new();

public async Task<List<HolidayOccurrence>> GetHolidaysForYearAsync(int year)
{
    lock (_yearCacheLock)
    {
        if (_yearCache.TryGetValue(year, out var cached))
            return cached;
    }

    var holidays = _holidayCalculationService.GetHolidaysForYear(year);

    lock (_yearCacheLock)
    {
        _yearCache[year] = holidays;
    }

    return await Task.FromResult(holidays);
}
```

**Test:** Deploy to Android device

### Step 3: Rollback RefreshSettingsAsync Change

**File:** `ViewModels/CalendarViewModel.cs:315-343`

**Revert to:**
```csharp
public async Task RefreshSettingsAsync()
{
    try
    {
        ShowCulturalBackground = SettingsViewModel.GetShowCulturalBackground();
        ShowLunarDates = SettingsViewModel.GetShowLunarDates();
        var newDays = SettingsViewModel.GetUpcomingHolidaysDays();

        if (UpcomingHolidaysDays != newDays)
        {
            UpcomingHolidaysDays = newDays;

            if (_isUpdatingHolidays)
            {
                if (await _updateSemaphore.WaitAsync(5000))
                {
                    _updateSemaphore.Release();
                }
                else
                {
                    return;
                }
            }

            await LoadUpcomingHolidaysAsync();
        }
    }
    catch (Exception)
    {
        // Silent failure
    }
}
```

**Test:** Deploy to Android device

## Information Needed

To diagnose the issue, we need:

1. **Logcat output** from Android device showing the crash
2. **Stack trace** of the exception
3. **Confirmation**: Did the app work on Android BEFORE these changes?
4. **Android version** being tested on

## To Get Logcat Output

```bash
# Clear logcat buffer
adb logcat -c

# Deploy app to device
dotnet build -t:Run -f net10.0-android -c Debug

# Capture crash log
adb logcat -d > android-crash.log
```

Then search for "FATAL EXCEPTION" or "AndroidRuntime" in the android-crash.log file.

## Alternative: Use Git Bisect

If gradual rollback is too slow:

```bash
# Stash current changes
git stash

# Create a commit with all changes
git stash pop
git add .
git commit -m "temp: all 7 fixes"

# Rollback to before fixes
git reset --hard HEAD~1

# Test if app works now
# If yes, the problem is in one of the 7 fixes
```

---

**Next Step:** Get logcat output or try quick rollback with `git stash`
