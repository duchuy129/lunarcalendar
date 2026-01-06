# Crash Investigation & Testing - December 29, 2025

## Issue Reported
User reported a crash on iPad when:
1. Selecting a different range in Settings
2. Navigating back to Calendar page

## Current Deployment Status

### ✅ iPhone 16 Pro
- **Device ID**: 928962C4-CCDB-40DF-92B8-8794CB867716
- **Status**: Running (Process ID: 31699)
- **App Version**: 1.0.0 with Upcoming Holidays Range Fix
- **Launch**: Successful

### ✅ iPad Pro 13-inch (M5) 16GB - iOS 26.2
- **Device ID**: D66062E4-3F8C-4709-B020-5F66809E3EDD
- **Status**: Running (Process ID: 31863)
- **App Version**: 1.0.0 with Upcoming Holidays Range Fix
- **Launch**: Successful

## Testing Instructions

Please test the following scenario on **both devices** (especially iPad):

### Test Case: Settings Range Change
1. ✅ Open the app
2. ✅ Navigate to Calendar page
3. ✅ Note the current "Upcoming Holidays" section title (e.g., "30 days")
4. ✅ Navigate to Settings
5. ⚠️ **Change the "Upcoming Holidays Range"** to a different value:
   - Try: 7 days
   - Try: 60 days
   - Try: 90 days
6. ⚠️ **Navigate back to Calendar page**
7. ✅ Verify the app doesn't crash
8. ✅ Verify the title updates correctly
9. ✅ Verify the holiday list updates correctly

### Additional Edge Case Tests

#### Test 1: Rapid Setting Changes
1. Change range to 7 days → Go back to Calendar
2. Immediately go to Settings again
3. Change range to 90 days → Go back to Calendar
4. Check for crashes or lag

#### Test 2: Language + Range Change
1. Change language (English ↔ Vietnamese)
2. Then change range
3. Navigate back to Calendar
4. Verify no crash and correct display

#### Test 3: Year Boundary Test
1. Set range to 90 days (to cross into next year)
2. Navigate back to Calendar
3. Verify holidays spanning into next year display correctly
4. Check for crashes when loading next year's data

## Potential Crash Causes to Monitor

Based on the code review, here are potential issues:

### 1. Async/Threading Issues
**Location**: `CalendarViewModel.RefreshSettings()`
```csharp
public async void RefreshSettings()
{
    var newDays = SettingsViewModel.GetUpcomingHolidaysDays();
    if (UpcomingHolidaysDays != newDays)
    {
        UpcomingHolidaysDays = newDays;
        await LoadUpcomingHolidaysAsync();
    }
}
```

**Concern**: `async void` can cause unhandled exceptions
**Recommended Fix**: Change to `async Task` and handle properly

### 2. Property Change Notification Race
**Location**: `OnUpcomingHolidaysDaysChanged`
```csharp
partial void OnUpcomingHolidaysDaysChanged(int value)
{
    OnPropertyChanged(nameof(UpcomingHolidaysTitle));
}
```

**Concern**: Property change might trigger before holidays are loaded
**Status**: Should be OK as title is just a computed property

### 3. Collection Update on Background Thread
**Location**: `LoadUpcomingHolidaysAsync()`
```csharp
UpcomingHolidays = new ObservableCollection<LocalizedHolidayOccurrence>(
    upcomingHolidays.Select(h => new LocalizedHolidayOccurrence(h)));
```

**Concern**: ObservableCollection updates must happen on UI thread
**Status**: Should be OK as method is async and awaited

## Console Log Monitoring

Currently monitoring both devices for errors. To check logs manually:

### iPad Logs:
```bash
xcrun simctl spawn D66062E4-3F8C-4709-B020-5F66809E3EDD log stream --predicate 'processImagePath contains "LunarCalendar"' --level=error
```

### iPhone Logs:
```bash
xcrun simctl spawn 928962C4-CCDB-40DF-92B8-8794CB867716 log stream --predicate 'processImagePath contains "LunarCalendar"' --level=error
```

## Next Steps

1. **User Testing**: Please test the scenario that caused the crash
2. **Report Findings**: Let us know:
   - Which device crashed (iPhone/iPad)?
   - What range values were selected?
   - Was there any error message?
   - Can you reproduce it consistently?

3. **If Crash Occurs**: We'll need to:
   - Review the stack trace
   - Add try-catch blocks with proper error handling
   - Potentially refactor `RefreshSettings()` to use `async Task`
   - Add MainThread.BeginInvokeOnMainThread for UI updates

## Status

🟢 **Both devices are running** - Ready for testing
⚠️ **Awaiting crash reproduction** - Please test and report findings
