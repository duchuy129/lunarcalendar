# Database SQLite LINQ Query Fix - January 7, 2026

## Issue Identified

From the diagnostic logs screenshot, the app was crashing with a LINQ query translation error:

```
...uery`1.<>c__DisplayClass2_0`1[[LunarCalendar.MobileApp.Data.Models.HolidayOccurrenceEntity...
```

This error occurs when SQLite-net cannot translate a LINQ expression to SQL, typically when:
1. Accessing properties of captured variables (closures) in WHERE clauses
2. Using complex expressions that can't be converted to SQL

## Root Cause

The issue was in two methods in `LunarCalendarDatabase.cs`:

### 1. SaveHolidayOccurrencesAsync (Line ~207)
```csharp
// PROBLEMATIC CODE
var existing = await _database!.Table<HolidayOccurrenceEntity>()
    .Where(ho => ho.GregorianDate == occurrence.GregorianDate && ho.Name == occurrence.Name)
    .FirstOrDefaultAsync();
```

### 2. SaveLunarDatesAsync (Line ~77)
```csharp
// PROBLEMATIC CODE
var existing = await _database!.Table<LunarDateEntity>()
    .Where(ld => ld.GregorianDate == lunarDate.GregorianDate)
    .FirstOrDefaultAsync();
```

**Problem**: SQLite-net couldn't translate `occurrence.GregorianDate` and `occurrence.Name` because they were captured from the loop variable `occurrence`, creating a closure that can't be converted to SQL.

## Solution Applied

Extract the values into local variables before the query:

### 1. Fixed SaveHolidayOccurrencesAsync
```csharp
foreach (var occurrence in occurrences)
{
    occurrence.LastSyncedAt = DateTime.UtcNow;
    
    // Store in local variables to avoid closure issues with SQLite-net
    var gregorianDate = occurrence.GregorianDate;
    var holidayName = occurrence.Name;
    
    var existing = await _database!.Table<HolidayOccurrenceEntity>()
        .Where(ho => ho.GregorianDate == gregorianDate && ho.Name == holidayName)
        .FirstOrDefaultAsync();
    // ...
}
```

### 2. Fixed SaveLunarDatesAsync
```csharp
foreach (var lunarDate in lunarDates)
{
    lunarDate.LastSyncedAt = DateTime.UtcNow;
    
    // Store in local variable to avoid closure issues with SQLite-net
    var gregorianDate = lunarDate.GregorianDate;
    
    var existing = await _database!.Table<LunarDateEntity>()
        .Where(ld => ld.GregorianDate == gregorianDate)
        .FirstOrDefaultAsync();
    // ...
}
```

## Why This Works

SQLite-net can translate simple local variables to SQL parameters, but struggles with property access on captured variables:

- ✅ **Works**: `var date = occurrence.Date; Where(x => x.Date == date)`
- ❌ **Fails**: `Where(x => x.Date == occurrence.Date)`

By extracting the values first, we give SQLite-net simple variables it can convert to SQL parameters.

## Additional Improvements

Also added comprehensive debug logging throughout the database operations:
- `[DB]` prefix for all database logs
- Entry/exit logging for all major operations
- Count of records being processed
- Detailed error messages with stack traces
- Individual record operation logging

## Testing

Build completed successfully:
```
LunarCalendar.MobileApp net10.0-ios ios-arm64 succeeded with 95 warning(s) (24.9s)
```

## Next Steps

1. Deploy to iOS simulator for testing
2. Deploy to iPhone device
3. Verify no more LINQ query errors in logs
4. Monitor `[DB]` logs to confirm database operations are working correctly

## Files Modified

- `src/LunarCalendar.MobileApp/Data/LunarCalendarDatabase.cs`
  - Fixed `SaveLunarDatesAsync` method
  - Fixed `SaveHolidayOccurrencesAsync` method
  - Added debug logging to all database methods

## Related Documentation

- [DEBUGGING_GUIDE.md](./DEBUGGING_GUIDE.md) - How to view and analyze the debug logs
