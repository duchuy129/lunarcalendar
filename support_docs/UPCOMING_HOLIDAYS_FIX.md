# Upcoming Holidays Range Fix

## Issue Description
When updating the "Upcoming Holidays Range" setting from the Settings page, the calendar page displayed inconsistent data:
1. The section title (e.g., "Upcoming Holidays (30 days)") would show the old value
2. The holiday list itself would load correctly with the new range
3. This caused a visual mismatch where the label didn't match the actual data displayed

## Root Cause
The issue was a missing property change notification in `CalendarViewModel.cs`. 

The `UpcomingHolidaysTitle` is a computed property that depends on `UpcomingHolidaysDays`:
```csharp
public string UpcomingHolidaysTitle => string.Format(AppResources.UpcomingHolidaysFormat, UpcomingHolidaysDays);
```

When `UpcomingHolidaysDays` was updated (via the `[ObservableProperty]` attribute), the change was saved and the holiday list was reloaded correctly. However, the UI binding for `UpcomingHolidaysTitle` was not notified to refresh because there was no property change notification for the computed property.

## Solution
Added a property change handler that notifies the UI when `UpcomingHolidaysDays` changes:

```csharp
partial void OnUpcomingHolidaysDaysChanged(int value)
{
    OnPropertyChanged(nameof(UpcomingHolidaysTitle));
}
```

This ensures that:
1. When `UpcomingHolidaysDays` is updated in `RefreshSettings()`
2. The `OnUpcomingHolidaysDaysChanged` handler is called automatically
3. The UI is notified that `UpcomingHolidaysTitle` has changed
4. The label updates to show the correct value
5. The holiday list loads with the same range value

## Testing
To verify the fix:
1. Open the app and note the current "Upcoming Holidays" section title
2. Navigate to Settings
3. Change the "Upcoming Holidays Range" value (e.g., from 30 to 60 days)
4. Navigate back to the Calendar page
5. Verify that:
   - The section title updates to show the new value (e.g., "Upcoming Holidays (60 days)")
   - The holiday list displays holidays within the new range
   - Both values are consistent

## Files Modified
- `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`
  - Added `OnUpcomingHolidaysDaysChanged()` method to notify UI of title changes

## Technical Details
This fix follows the MVVM pattern's property change notification mechanism:
- `[ObservableProperty]` generates the property and change notification for `UpcomingHolidaysDays`
- The partial method `OnUpcomingHolidaysDaysChanged` is called automatically when the value changes
- We manually notify dependent computed properties using `OnPropertyChanged()`
- This ensures all UI bindings stay synchronized

The fix is not a cache issue but rather a UI synchronization issue where the computed property wasn't being notified of changes to its dependency.
