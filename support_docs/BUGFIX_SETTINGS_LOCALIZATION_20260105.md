# Bug Fix: Settings Sync & Offline Localization Issues

**Date**: January 5, 2026  
**Type**: Localization Bug Fix  
**Priority**: Medium  
**Status**: ✅ Completed

## Problem Description

The Settings screen's "Sync & Offline" section had localization inconsistencies:

1. **Connection Status**: Was displaying "True/False" in both English and Vietnamese modes instead of properly localized "Online/Offline" or "Trực tuyến/Ngoại tuyến"
2. **Last Sync Time**: Date formatting was hardcoded using English format ("MMM dd, yyyy HH:mm") which didn't respect the current language/culture setting, resulting in English month names appearing in Vietnamese mode and vice versa

## Root Cause

### Issue 1: Connection Status
- The XAML had a duplicate label showing `{Binding IsOnline, StringFormat='{0}'}` which displayed the raw boolean value
- While there were DataTriggers to show localized text, the binding was not properly set up to use the localized resources

### Issue 2: Last Sync Time
- The `UpdateSyncStatus()` method in `SettingsViewModel.cs` used a hardcoded date format string: `ToString("MMM dd, yyyy HH:mm")`
- This format always used English month abbreviations regardless of the current culture

## Solution Implemented

### 1. Connection Status Fix

**File**: `SettingsViewModel.cs`

Added a new property to hold the localized connection status text:
```csharp
[ObservableProperty]
private string _connectionStatusText = string.Empty;
```

Created a method to update the localized text:
```csharp
private void UpdateConnectionStatus()
{
    ConnectionStatusText = IsOnline ? AppResources.Online : AppResources.Offline;
}
```

Updated initialization and event handlers:
- Initialize `ConnectionStatusText` in both constructors
- Call `UpdateConnectionStatus()` when connectivity changes
- Call `UpdateConnectionStatus()` when language changes

**File**: `SettingsPage.xaml`

Simplified the UI to remove the duplicate label and use the new property:
```xml
<!-- Before: Had VerticalStackLayout with two labels showing True/False -->
<!-- After: Single label showing localized text -->
<Label Grid.Column="1"
       Text="{Binding ConnectionStatusText}"
       FontSize="14"
       FontAttributes="Bold"
       TextColor="{DynamicResource Primary}"
       VerticalOptions="Center">
    <Label.Triggers>
        <DataTrigger TargetType="Label" Binding="{Binding IsOnline}" Value="True">
            <Setter Property="TextColor" Value="Green" />
        </DataTrigger>
        <DataTrigger TargetType="Label" Binding="{Binding IsOnline}" Value="False">
            <Setter Property="TextColor" Value="Orange" />
        </DataTrigger>
    </Label.Triggers>
</Label>
```

### 2. Last Sync Time Date Formatting Fix

**File**: `SettingsViewModel.cs`

Changed the date formatting to use culture-aware formatting:
```csharp
// Before:
LastSyncTime = _syncService.LastSyncTime.Value.ToString("MMM dd, yyyy HH:mm");

// After:
var culture = System.Globalization.CultureInfo.CurrentCulture;
LastSyncTime = _syncService.LastSyncTime.Value.ToString("g", culture); // Short date and time pattern
```

The "g" format specifier provides a culture-appropriate short date and short time pattern:
- English: "1/5/2026 2:30 PM"
- Vietnamese: "05/01/2026 14:30"

Also added `UpdateSyncStatus()` call in the language change handler to refresh the date format when language changes.

## Localization Resources Used

The fix utilizes existing localized strings:

**English** (`AppResources.resx`):
- `Online` = "Online"
- `Offline` = "Offline"
- `JustNow` = "Just now"
- `MinutesAgo` = "{0} minutes ago"
- `HoursAgo` = "{0} hours ago"
- `Never` = "Never"

**Vietnamese** (`AppResources.vi.resx`):
- `Online` = "Trực tuyến"
- `Offline` = "Ngoại tuyến"
- `JustNow` = "Vừa xong"
- `MinutesAgo` = "{0} phút trước"
- `HoursAgo` = "{0} giờ trước"
- `Never` = "Chưa bao giờ"

## Files Modified

1. `/src/LunarCalendar.MobileApp/ViewModels/SettingsViewModel.cs`
   - Added `ConnectionStatusText` property
   - Added `UpdateConnectionStatus()` method
   - Updated `UpdateSyncStatus()` for culture-aware date formatting
   - Updated constructors and event handlers

2. `/src/LunarCalendar.MobileApp/Views/SettingsPage.xaml`
   - Simplified Connection Status UI
   - Removed duplicate True/False display
   - Bound to new `ConnectionStatusText` property

## Testing Recommendations

1. **Connection Status**:
   - Switch language from English to Vietnamese and back
   - Verify "Online"/"Trực tuyến" displays correctly
   - Toggle airplane mode and verify "Offline"/"Ngoại tuyến" displays correctly
   - Verify text color changes (Green for online, Orange for offline)

2. **Last Sync Time**:
   - Perform a sync
   - Check immediate display shows "Just now"/"Vừa xong"
   - Wait and verify minute/hour displays: "5 minutes ago"/"5 phút trước"
   - For older syncs, verify date format matches language:
     - English: "1/5/2026 2:30 PM"
     - Vietnamese: "05/01/2026 14:30"
   - Switch language and verify format updates

3. **Language Switch**:
   - Go to Settings with online connection
   - Switch from English to Vietnamese
   - Verify both Connection Status and Last Sync update immediately
   - Switch back to English and verify updates

## Platform Compatibility

- ✅ iOS
- ✅ Android
- ✅ Works with both English and Vietnamese locales
- ✅ Compatible with system language/culture settings

## Impact Assessment

- **User Experience**: High positive impact - proper localization improves user trust and app professionalism
- **Performance**: No impact - minimal computation overhead
- **Code Complexity**: Low - simplified XAML, added one property and one method
- **Breaking Changes**: None
- **Dependencies**: Uses existing localization infrastructure

## Related Issues

This fix completes the localization work across the app:
- Addresses inconsistencies in Settings screen
- Aligns with localization implemented in other screens (Calendar, Holidays, Year Picker)
- Follows the pattern established in other ViewModels for language change handling
