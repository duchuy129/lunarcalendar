# Bug Fix: Lunar New Year's Eve Detail Page Display

## Date: January 26, 2026

## Issue Description
The holiday detail page for Lunar New Year's Eve (Giao Thừa) was displaying hardcoded "12/30" instead of the actual lunar date. In some years, the 12th lunar month only has 29 days, so Giao Thừa should display "12/29" instead of "12/30".

This issue was present in the holiday detail page, even though the holidays list page was already fixed to show the correct date using `ActualLunarDay` and `ActualLunarMonth` properties.

## Root Cause
In `HolidayDetailViewModel.cs`, when displaying lunar-based holidays, the code was using the hardcoded values from the `Holiday` definition:
```csharp
lunarDateText = DateFormatterHelper.FormatLunarDate(
    holidayOccurrence.Holiday.LunarDay,      // Always 30
    holidayOccurrence.Holiday.LunarMonth);   // Always 12
```

## Solution
Updated `HolidayDetailViewModel.cs` to use the actual lunar date values from the `HolidayOccurrence` object, which are calculated by `HolidayCalculationService`:

```csharp
// Use actual lunar date if available (fixes Giao Thừa 12/29 vs 12/30)
var lunarDay = holidayOccurrence.ActualLunarDay > 0 
    ? holidayOccurrence.ActualLunarDay 
    : holidayOccurrence.Holiday.LunarDay;
var lunarMonth = holidayOccurrence.ActualLunarMonth > 0 
    ? holidayOccurrence.ActualLunarMonth 
    : holidayOccurrence.Holiday.LunarMonth;

lunarDateText = DateFormatterHelper.FormatLunarDate(lunarDay, lunarMonth);
```

## Files Modified
- `/src/LunarCalendar.MobileApp/ViewModels/HolidayDetailViewModel.cs` (lines 104-110)

## Testing
The fix has been deployed and tested on:
- ✅ Android Simulator (emulator-5554)
- ✅ iOS iPad Simulator

## Impact
This fix ensures that:
1. The holiday detail page now correctly displays "12/29" for Giao Thừa in years where month 12 has 29 days
2. The display is consistent between the holidays list and the detail page
3. All other holidays continue to work as expected

## Related Previous Fix
This complements the earlier fix in `LocalizedHolidayOccurrence.cs` (lines 75-77) which fixed the same issue for the holidays list display.

## Notes
The `HolidayCalculationService` correctly calculates the actual last day of the 12th lunar month using the Vietnamese lunar calendar algorithm, and stores it in `ActualLunarDay` and `ActualLunarMonth` properties of the `HolidayOccurrence` object.
