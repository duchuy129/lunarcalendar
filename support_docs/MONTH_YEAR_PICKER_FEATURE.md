# Month/Year Picker Feature

## Overview
Added a month/year picker to the calendar view that allows users to quickly jump to any specific month and year.

## Location
The month/year picker is located in the calendar view, just below the month navigation header (Previous/Today/Next buttons).

## Components Added

### 1. **UI Components** (CalendarPage.xaml)
- **Row Layout**: Added a new row in the Grid (Row 1) for the picker section
- **Label**: "Jump to:" label
- **Month Picker**: Dropdown to select month (January - December)
- **Year Picker**: Dropdown to select year (±10 years from current year)
- **Go Button**: Button to navigate to the selected month/year

### 2. **View Model Properties** (CalendarViewModel.cs)
```csharp
// Month/Year picker properties
[ObservableProperty]
private int _selectedCalendarYear;

[ObservableProperty]
private int _selectedCalendarMonth;

[ObservableProperty]
private ObservableCollection<int> _availableCalendarYears = new();

[ObservableProperty]
private ObservableCollection<string> _availableMonths = new();
```

### 3. **Command** (CalendarViewModel.cs)
```csharp
[RelayCommand]
async Task JumpToMonthAsync()
{
    // Update current month based on selected year and month
    CurrentMonth = new DateTime(SelectedCalendarYear, SelectedCalendarMonth, 1);
    await LoadCalendarAsync();
}
```

### 4. **Value Converter** (MonthIndexConverter.cs)
Converts between 1-based month numbers (1-12) and 0-based picker indices (0-11).

### 5. **Event Handlers** (CalendarPage.xaml.cs)
```csharp
private void OnMonthPickerSelectedIndexChanged(object? sender, EventArgs e)
{
    if (sender is Picker picker && _viewModel != null)
    {
        // Update the selected month from picker index (0-based to 1-based)
        _viewModel.SelectedCalendarMonth = picker.SelectedIndex + 1;
    }
}
```

## How to Use

### For End Users:
1. Open the Lunar Calendar app
2. Navigate to the Calendar page (tap "Continue as Guest" from Welcome page)
3. Below the month navigation header, you'll see "Jump to:" section
4. Select desired **month** from the first dropdown
5. Select desired **year** from the second dropdown
6. Tap the **"Go"** button
7. The calendar will instantly jump to the selected month/year

### Features:
- **Month Selection**: All 12 months available (January - December)
- **Year Selection**: Current year ± 10 years (2015 - 2035 when current year is 2025)
- **Synchronized State**: When you use Previous/Next/Today buttons, the pickers automatically update to show the current displayed month
- **One-Click Navigation**: After selecting month/year, simply click "Go" to jump

## UI Design

```
┌─────────────────────────────────────────┐
│  ◀  December 2025        Today  ▶       │  <- Month Navigation Header
├─────────────────────────────────────────┤
│  Jump to: [December ▼] [2025 ▼] [Go]   │  <- NEW: Month/Year Picker
├─────────────────────────────────────────┤
│  Today: 6/11/2025                       │  <- Lunar Date Display
├─────────────────────────────────────────┤
│  Sun  Mon  Tue  Wed  Thu  Fri  Sat      │  <- Day Headers
├─────────────────────────────────────────┤
│  [Calendar Grid]                        │  <- Calendar Days
└─────────────────────────────────────────┘
```

## Testing Scenarios

### Test 1: Basic Navigation
1. Open calendar (should show current month: December 2025)
2. From month picker, select "March"
3. From year picker, select "2024"
4. Click "Go"
5. **Expected**: Calendar shows March 2024

### Test 2: Picker Synchronization
1. Calendar shows December 2025
2. Click "Previous Month" button (◀)
3. **Expected**: Month picker updates to "November", Year picker shows "2025"
4. Click "Next Month" button (▶) twice
5. **Expected**: Month picker updates to "January", Year picker shows "2026"

### Test 3: Today Button Integration
1. Use month/year picker to jump to March 2024
2. Click "Today" button in header
3. **Expected**:
   - Calendar returns to December 2025
   - Month picker shows "December"
   - Year picker shows "2025"

### Test 4: Edge Cases
1. Jump to January 2015 (earliest year)
2. **Expected**: Works correctly
3. Jump to December 2035 (latest year)
4. **Expected**: Works correctly
5. Rapidly change months using picker
6. **Expected**: Calendar updates smoothly without errors

## Files Modified

1. **CalendarViewModel.cs**
   - Added `SelectedCalendarYear`, `SelectedCalendarMonth` properties
   - Added `AvailableCalendarYears`, `AvailableMonths` collections
   - Added `JumpToMonthAsync()` command
   - Modified `LoadCalendarAsync()` to sync picker values

2. **CalendarPage.xaml**
   - Updated Grid RowDefinitions from 7 to 8 rows
   - Added new Grid row for month/year picker (Row 1)
   - Updated all subsequent Grid.Row values (+1)
   - Added Month and Year Picker controls
   - Added "Go" button
   - Registered MonthIndexConverter in resources

3. **CalendarPage.xaml.cs**
   - Added `OnMonthPickerSelectedIndexChanged()` event handler

4. **MonthIndexConverter.cs** (NEW)
   - Created new converter for month index conversion
   - Converts between 1-based months and 0-based picker indices

## Benefits

1. **Faster Navigation**: Users can jump directly to any month/year instead of clicking Previous/Next multiple times
2. **Better UX**: Especially useful for viewing holidays or dates far in the past/future
3. **Consistent State**: Pickers always reflect the currently displayed month
4. **Intuitive Design**: Familiar dropdown pattern that users understand
5. **Complements Existing Navigation**: Works alongside Previous/Next/Today buttons

## Future Enhancements (Optional)

1. **Wider Year Range**: Extend to ±20 or ±50 years if needed
2. **Quick Presets**: Add buttons like "6 months ago", "1 year from now"
3. **Keyboard Shortcuts**: Allow typing year directly instead of dropdown
4. **Month Abbreviations**: Use "Jan", "Feb" instead of full names on small screens
5. **Animation**: Add smooth transition when jumping to selected month

## Platform Support

- ✅ **iPhone** (iOS 18.2+)
- ✅ **iPad** (iOS 26.2+)
- ✅ **Android** (API 21+)

## Related Features

- Month navigation (Previous/Next/Today buttons)
- Vietnamese Holidays year picker (similar pattern in holidays section)
- Calendar day display
- Lunar date conversion

---

**Implementation Date**: December 25, 2024
**Feature Status**: ✅ Complete and Ready for Testing
