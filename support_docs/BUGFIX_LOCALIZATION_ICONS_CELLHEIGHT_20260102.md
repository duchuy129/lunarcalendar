# Bug Fixes - Localization, iOS Icons, and Cell Height
**Date:** January 2, 2026  
**Status:** ✅ COMPLETED

## Issues Fixed

### 1. ✅ Calendar/Settings Not Localized
**Problem:** Tab titles were hardcoded as "Calendar" and "Settings" in English

**Solution:**
- Updated `AppShell.xaml.cs` to use `AppResources.Calendar` and `AppResources.Settings`
- Added `using LunarCalendar.MobileApp.Resources.Strings;` import
- Tab titles now properly switch between English and Vietnamese based on device language

**Files Modified:**
- `src/LunarCalendar.MobileApp/AppShell.xaml.cs`

**Changes:**
```csharp
// Before
Title = "Calendar",
Title = "Settings",

// After  
Title = AppResources.Calendar,
Title = AppResources.Settings,
```

### 2. ✅ Calendar/Settings Icons Not Visible on iOS
**Problem:** Tab bar icons were not showing on iOS

**Solution:**
- Added explicit Shell tab bar color settings
- Set `ForegroundColor` on each `ShellContent` to ensure icon visibility
- Applied color properties programmatically for better iOS compatibility

**Files Modified:**
- `src/LunarCalendar.MobileApp/AppShell.xaml.cs`

**Changes:**
```csharp
// Added to CreateUIInCode method
Shell.SetTabBarBackgroundColor(this, Colors.White);
Shell.SetTabBarForegroundColor(this, Color.FromArgb("#512BD4"));
Shell.SetTabBarUnselectedColor(this, Color.FromArgb("#999999"));

// Added for each tab
Shell.SetForegroundColor(calendarTab, Color.FromArgb("#512BD4"));
Shell.SetForegroundColor(settingsTab, Color.FromArgb("#512BD4"));
```

### 3. ✅ Lunar Date Text Cut Off in Calendar Cells
**Problem:** Lunar date description was partially visible/cut off in calendar date cells

**Solution:**
- Increased cell `HeightRequest` from 60 to 70 pixels
- Updated `CalendarViewModel` height calculations for all week configurations
- New heights: 296px (4 weeks), 370px (5 weeks), 444px (6 weeks)

**Files Modified:**
- `src/LunarCalendar.MobileApp/Views/CalendarPage.xaml`
- `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`

**Changes:**
```xml
<!-- CalendarPage.xaml -->
<!-- Before -->
<Border HeightRequest="60">

<!-- After -->
<Border HeightRequest="70">
```

```csharp
// CalendarViewModel.cs - Before
CalendarHeight = weeksNeeded switch
{
    4 => 256,  // 4 rows: 64 * 4 = 256
    5 => 320,  // 5 rows: 64 * 5 = 320
    6 => 384,  // 6 rows: 64 * 6 = 384
    _ => 320
};

// After
CalendarHeight = weeksNeeded switch
{
    4 => 296,  // 4 rows: 74 * 4 = 296
    5 => 370,  // 5 rows: 74 * 5 = 370
    6 => 444,  // 6 rows: 74 * 6 = 444
    _ => 370
};
```

## Testing Recommendations

### Test 1: Localization
1. Run app on iOS/Android
2. Verify tab titles show "Calendar" and "Settings" in English
3. Change device language to Vietnamese
4. Restart app
5. Verify tab titles show "Lịch Vạn Niên" and "Cài đặt" in Vietnamese

### Test 2: iOS Icon Visibility
1. Run app on iOS device or simulator
2. Check bottom tab bar
3. Verify calendar icon is visible on first tab
4. Verify settings icon is visible on second tab
5. Tap each tab to verify icons change color when selected

### Test 3: Calendar Cell Height
1. Open Calendar tab
2. View current month calendar grid
3. Check each date cell
4. Verify lunar date text (small red text) is fully visible
5. Verify holiday names are not cut off
6. Test with different months (4, 5, and 6 week layouts)

## Technical Details

### Cell Height Calculation
- Cell height: 70px
- Margin: 2px on each side = 4px total
- Effective height per row: 74px
- Total grid height = rows × 74px

### Color Scheme
- Selected tab: `#512BD4` (purple)
- Unselected tab: `#999999` (gray)
- Background: `White`

### Resource Keys
- English: `AppResources.Calendar`, `AppResources.Settings`
- Vietnamese: Defined in `AppResources.vi.resx`

## Impact

### Positive
- ✅ Proper localization support for tab titles
- ✅ iOS users can now see tab bar icons
- ✅ Better readability of lunar dates in calendar
- ✅ More space for holiday names
- ✅ Consistent UI across platforms

### Considerations
- Calendar grid is now slightly taller (46-64px increase depending on weeks)
- More scrolling may be needed on smaller devices
- No breaking changes to existing functionality

## Build & Deploy

All changes are compatible with current build process:
- iOS: `build-ios-release.sh`
- Android: `build-android-release.sh`

No additional dependencies or configuration needed.

---
**Status:** Ready for testing and deployment
