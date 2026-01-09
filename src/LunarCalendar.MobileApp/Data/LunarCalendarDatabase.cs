using SQLite;
using LunarCalendar.MobileApp.Data.Models;

namespace LunarCalendar.MobileApp.Data;

public class LunarCalendarDatabase
{
    private SQLiteAsyncConnection? _database;
    private readonly string _databasePath;

    public LunarCalendarDatabase(string databasePath)
    {
        _databasePath = databasePath;
    }

    private async Task InitAsync()
    {
        if (_database is not null)
            return;

        Console.WriteLine($"[DB] InitAsync: Initializing database at path: {_databasePath}");
        try
        {
            _database = new SQLiteAsyncConnection(_databasePath);

            await _database.CreateTableAsync<LunarDateEntity>();
            await _database.CreateTableAsync<HolidayEntity>();
            await _database.CreateTableAsync<HolidayOccurrenceEntity>();
            await _database.CreateTableAsync<SyncLogEntity>();
            
            Console.WriteLine("[DB] Database initialized successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DB] ERROR initializing database: {ex.Message}");
            Console.WriteLine($"[DB] Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    #region Lunar Dates

    public async Task<List<LunarDateEntity>> GetLunarDatesForMonthAsync(int year, int month)
    {
        Console.WriteLine($"[DB] GetLunarDatesForMonthAsync: Fetching lunar dates for {year}-{month:D2}");
        await InitAsync();
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        try
        {
            var results = await _database!.Table<LunarDateEntity>()
                .Where(ld => ld.GregorianDate >= startDate && ld.GregorianDate <= endDate)
                .ToListAsync();
            Console.WriteLine($"[DB] Found {results.Count} lunar dates for {year}-{month:D2}");
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DB] ERROR fetching lunar dates: {ex.Message}");
            throw;
        }
    }

    public async Task<LunarDateEntity?> GetLunarDateAsync(DateTime date)
    {
        await InitAsync();
        var dateOnly = date.Date;
        return await _database!.Table<LunarDateEntity>()
            .Where(ld => ld.GregorianDate == dateOnly)
            .FirstOrDefaultAsync();
    }

    public async Task SaveLunarDateAsync(LunarDateEntity lunarDate)
    {
        Console.WriteLine($"[DB] SaveLunarDateAsync: Saving lunar date for {lunarDate.GregorianDate:yyyy-MM-dd}");
        await InitAsync();
        lunarDate.LastSyncedAt = DateTime.UtcNow;

        try
        {
            var existing = await GetLunarDateAsync(lunarDate.GregorianDate);
            if (existing != null)
            {
                lunarDate.Id = existing.Id;
                await _database!.UpdateAsync(lunarDate);
                Console.WriteLine($"[DB] Updated lunar date {lunarDate.GregorianDate:yyyy-MM-dd}");
            }
            else
            {
                await _database!.InsertAsync(lunarDate);
                Console.WriteLine($"[DB] Inserted new lunar date {lunarDate.GregorianDate:yyyy-MM-dd}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DB] ERROR saving lunar date: {ex.Message}");
            Console.WriteLine($"[DB] Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    public async Task SaveLunarDatesAsync(List<LunarDateEntity> lunarDates)
    {
        Console.WriteLine($"[DB] SaveLunarDatesAsync: Starting to save {lunarDates.Count} lunar dates");
        await InitAsync();

        try
        {
            // Use InsertOrReplace for simplicity and reliability
            foreach (var lunarDate in lunarDates)
            {
                lunarDate.LastSyncedAt = DateTime.UtcNow;
                await _database!.InsertOrReplaceAsync(lunarDate);
                Console.WriteLine($"[DB] Saved lunar date {lunarDate.GregorianDate:yyyy-MM-dd}");
            }
            Console.WriteLine($"[DB] Successfully saved {lunarDates.Count} lunar dates");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DB] ERROR saving lunar dates: {ex.Message}");
            Console.WriteLine($"[DB] Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    #endregion

    #region Holiday Occurrences

    public async Task<List<HolidayOccurrenceEntity>> GetHolidaysForMonthAsync(int year, int month)
    {
        Console.WriteLine($"[DB] GetHolidaysForMonthAsync: Fetching holidays for {year}-{month:D2}");
        await InitAsync();
        try
        {
            var results = await _database!.Table<HolidayOccurrenceEntity>()
                .Where(ho => ho.Year == year && ho.Month == month)
                .ToListAsync();
            Console.WriteLine($"[DB] Found {results.Count} holidays for {year}-{month:D2}");
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DB] ERROR fetching holidays: {ex.Message}");
            throw;
        }
    }

    public async Task<List<HolidayOccurrenceEntity>> GetHolidaysForYearAsync(int year)
    {
        await InitAsync();
        return await _database!.Table<HolidayOccurrenceEntity>()
            .Where(ho => ho.Year == year)
            .ToListAsync();
    }

    public async Task<HolidayOccurrenceEntity?> GetHolidayForDateAsync(DateTime date)
    {
        await InitAsync();
        var dateOnly = date.Date;
        return await _database!.Table<HolidayOccurrenceEntity>()
            .Where(ho => ho.GregorianDate == dateOnly)
            .FirstOrDefaultAsync();
    }

    public async Task SaveHolidayOccurrencesAsync(List<HolidayOccurrenceEntity> occurrences)
    {
        Console.WriteLine($"[DB] SaveHolidayOccurrencesAsync: Starting to save {occurrences.Count} holiday occurrences");
        await InitAsync();

        if (occurrences == null || occurrences.Count == 0)
        {
            Console.WriteLine("[DB] No holiday occurrences to save");
            return;
        }

        try
        {
            // Get min and max dates to limit the existing records query
            var minDate = occurrences.Min(o => o.GregorianDate);
            var maxDate = occurrences.Max(o => o.GregorianDate);

            // Load all existing records for the date range ONCE before processing
            Console.WriteLine($"[DB] Loading existing holidays from {minDate:yyyy-MM-dd} to {maxDate:yyyy-MM-dd}");
            var existingList = await _database!.Table<HolidayOccurrenceEntity>()
                .Where(ho => ho.GregorianDate >= minDate && ho.GregorianDate <= maxDate)
                .ToListAsync();

            // Create lookup dictionary for fast in-memory matching
            var existingDict = new Dictionary<string, HolidayOccurrenceEntity>();
            foreach (var existing in existingList)
            {
                var key = $"{existing.GregorianDate:yyyy-MM-dd}_{existing.Name}";
                existingDict[key] = existing;
            }
            Console.WriteLine($"[DB] Found {existingDict.Count} existing holiday records");

            // Prepare batches for update and insert
            var toUpdate = new List<HolidayOccurrenceEntity>();
            var toInsert = new List<HolidayOccurrenceEntity>();

            // Process all occurrences in memory (no async calls)
            foreach (var occurrence in occurrences)
            {
                occurrence.LastSyncedAt = DateTime.UtcNow;
                var key = $"{occurrence.GregorianDate:yyyy-MM-dd}_{occurrence.Name}";

                if (existingDict.TryGetValue(key, out var existing))
                {
                    occurrence.Id = existing.Id;
                    toUpdate.Add(occurrence);
                }
                else
                {
                    toInsert.Add(occurrence);
                }
            }

            Console.WriteLine($"[DB] Prepared {toUpdate.Count} updates and {toInsert.Count} inserts");

            // Execute batch operations
            foreach (var occurrence in toUpdate)
            {
                await _database!.UpdateAsync(occurrence);
                Console.WriteLine($"[DB] Updated holiday '{occurrence.Name}' on {occurrence.GregorianDate:yyyy-MM-dd}");
            }

            foreach (var occurrence in toInsert)
            {
                await _database!.InsertAsync(occurrence);
                Console.WriteLine($"[DB] Inserted holiday '{occurrence.Name}' on {occurrence.GregorianDate:yyyy-MM-dd}");
            }

            Console.WriteLine($"[DB] Successfully saved {occurrences.Count} holiday occurrences");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DB] ERROR saving holiday occurrences: {ex.Message}");
            Console.WriteLine($"[DB] Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    public async Task ClearHolidaysForYearAsync(int year)
    {
        await InitAsync();
        await _database!.ExecuteAsync("DELETE FROM HolidayOccurrences WHERE Year = ?", year);
    }

    #endregion

    #region Sync Log

    public async Task AddSyncLogAsync(string entityType, string result, string? errorMessage = null)
    {
        await InitAsync();
        var log = new SyncLogEntity
        {
            EntityType = entityType,
            SyncResult = result,
            SyncedAt = DateTime.UtcNow,
            ErrorMessage = errorMessage
        };
        await _database!.InsertAsync(log);
    }

    public async Task<List<SyncLogEntity>> GetRecentSyncLogsAsync(int count = 50)
    {
        await InitAsync();
        return await _database!.Table<SyncLogEntity>()
            .OrderByDescending(sl => sl.SyncedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<SyncLogEntity?> GetLastSuccessfulSyncAsync(string entityType)
    {
        await InitAsync();
        return await _database!.Table<SyncLogEntity>()
            .Where(sl => sl.EntityType == entityType && sl.SyncResult == "Success")
            .OrderByDescending(sl => sl.SyncedAt)
            .FirstOrDefaultAsync();
    }

    #endregion

    #region Database Management

    public async Task ClearAllDataAsync()
    {
        await InitAsync();
        await _database!.ExecuteAsync("DELETE FROM LunarDates");
        await _database!.ExecuteAsync("DELETE FROM Holidays");
        await _database!.ExecuteAsync("DELETE FROM HolidayOccurrences");
        await _database!.ExecuteAsync("DELETE FROM SyncLog");
    }

    public async Task CloseAsync()
    {
        if (_database != null)
        {
            await _database.CloseAsync();
            _database = null;
        }
    }

    #endregion
}
