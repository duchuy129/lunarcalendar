# Settings Page Crash Fixes - January 1, 2026

## Issue Summary
The app was crashing when clicking any of the four action buttons in the Settings page:
1. **Sync Now** button
2. **Clear Cache** button  
3. **Reset All Settings** button
4. **About this app** button

## Root Cause
All four button command handlers were calling `Shell.Current.DisplayAlert()` directly without:
1. Ensuring execution on the main UI thread
2. Proper exception handling for iOS-specific threading requirements

On iOS, all UI operations (including displaying alerts) **must** be performed on the main thread. Calling UI methods from background threads causes immediate crashes.

## Solution Implemented

### Files Modified
- `src/LunarCalendar.MobileApp/ViewModels/SettingsViewModel.cs`

### Changes Made

#### 1. SyncDataAsync Command
**Before:**
```csharp
await Shell.Current.DisplayAlert(...);
```

**After:**
```csharp
await MainThread.InvokeOnMainThreadAsync(async () =>
{
    await Shell.Current.DisplayAlert(...);
});
```

**Changes:**
- Wrapped all 4 DisplayAlert calls in MainThread.InvokeOnMainThreadAsync
- Added comprehensive try-catch wrapper around entire method
- Added debug logging for troubleshooting
- Added nested try-catch for DisplayAlert calls in error handler

#### 2. ClearCacheAsync Command
**Before:**
```csharp
await Shell.Current.DisplayAlert(AppResources.Success, AppResources.CacheClearedMessage, AppResources.OK);
```

**After:**
```csharp
await MainThread.InvokeOnMainThreadAsync(async () =>
{
    await Shell.Current.DisplayAlert(AppResources.Success, AppResources.CacheClearedMessage, AppResources.OK);
});
```

**Changes:**
- Wrapped both success and error DisplayAlert calls in MainThread.InvokeOnMainThreadAsync
- Added comprehensive try-catch wrapper
- Added debug logging for error tracking
- Added nested try-catch for DisplayAlert in error handler

#### 3. AboutAsync Command
**Before:**
```csharp
async Task AboutAsync()
{
    await Shell.Current.DisplayAlert(
        AppResources.About,
        AppResources.AboutMessage,
        AppResources.OK);
}
```

**After:**
```csharp
async Task AboutAsync()
{
    try
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.DisplayAlert(
                AppResources.About,
                AppResources.AboutMessage,
                AppResources.OK);
        });
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"!!! AboutAsync error: {ex.Message} !!!");
    }
}
```

**Changes:**
- Wrapped DisplayAlert in MainThread.InvokeOnMainThreadAsync
- Added try-catch for error handling
- Added debug logging

#### 4. ResetSettingsAsync Command
**Before:**
```csharp
var confirmed = await Shell.Current.DisplayAlert(...);
if (confirmed)
{
    Preferences.Clear();
    LoadSettings();
    await Shell.Current.DisplayAlert(...);
}
```

**After:**
```csharp
var confirmed = await MainThread.InvokeOnMainThreadAsync(async () =>
{
    return await Shell.Current.DisplayAlert(...);
});

if (confirmed)
{
    Preferences.Clear();
    LoadSettings();
    
    await MainThread.InvokeOnMainThreadAsync(async () =>
    {
        await Shell.Current.DisplayAlert(...);
    });
}
```

**Changes:**
- Wrapped both DisplayAlert calls (confirmation dialog and success message) in MainThread.InvokeOnMainThreadAsync
- Added comprehensive try-catch wrapper
- Added debug logging for error tracking
- Added nested try-catch for DisplayAlert in error handler

## Technical Details

### MainThread.InvokeOnMainThreadAsync
This MAUI API ensures that code executes on the main UI thread, which is required for all UI operations on iOS:
- Safe for calling from any thread
- Returns a Task that can be awaited
- Automatically handles thread marshalling

### Error Handling Pattern
Each command now follows this pattern:
```csharp
try
{
    // Main logic here
    
    await MainThread.InvokeOnMainThreadAsync(async () =>
    {
        // All UI operations (DisplayAlert) here
    });
}
catch (Exception ex)
{
    System.Diagnostics.Debug.WriteLine($"!!! Error: {ex.Message} !!!");
    
    await MainThread.InvokeOnMainThreadAsync(async () =>
    {
        try
        {
            await Shell.Current.DisplayAlert(...);
        }
        catch (Exception displayEx)
        {
            System.Diagnostics.Debug.WriteLine($"!!! Failed to display error: {displayEx.Message} !!!");
        }
    });
}
```

## Testing Checklist
- [x] Build succeeds for iOS device (ios-arm64)
- [ ] Sync Now button - displays alert without crash
- [ ] Clear Cache button - clears cache and displays success message
- [ ] Reset All Settings button - shows confirmation, resets settings, displays success
- [ ] About this app button - displays about information

## Related Issues
This fix is part of a broader effort to ensure thread-safe UI operations across the app. Similar patterns should be applied to:
- CalendarViewModel.cs (already partially fixed in previous commits)
- Any other ViewModels that call Shell.Current.DisplayAlert()

## Deployment
- **Date:** January 1, 2026
- **Platform:** iOS 26.1
- **Device:** Huy's iPhone (00008110-001E38192E50401E)
- **Build Configuration:** Debug
- **Runtime Identifier:** ios-arm64

## Verification Steps
1. Launch app on iPhone
2. Navigate to Settings tab
3. Test each button in sequence:
   - Click "Sync Now" → Should show sync dialog
   - Click "Clear Cache" → Should show confirmation and success
   - Click "Reset All Settings" → Should show confirmation, reset settings, show success
   - Click "About this app" → Should show about information
4. Verify no crashes occur
5. Check device logs for any error messages

## Notes
- All DisplayAlert calls now use MainThread.InvokeOnMainThreadAsync
- Debug logging added to help diagnose future issues
- Comprehensive error handling prevents crashes even if DisplayAlert fails
- This pattern should be used consistently throughout the app
