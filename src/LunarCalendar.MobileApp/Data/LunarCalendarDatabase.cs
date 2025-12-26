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

        _database = new SQLiteAsyncConnection(_databasePath);

        await _database.CreateTableAsync<LunarDateEntity>();
        await _database.CreateTableAsync<HolidayEntity>();
        await _database.CreateTableAsync<HolidayOccurrenceEntity>();
        await _database.CreateTableAsync<SyncLogEntity>();
    }

    #region Lunar Dates

    public async Task<List<LunarDateEntity>> GetLunarDatesForMonthAsync(int year, int month)
    {
        await InitAsync();
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        return await _database!.Table<LunarDateEntity>()
            .Where(ld => ld.GregorianDate >= startDate && ld.GregorianDate <= endDate)
            .ToListAsync();
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
        await InitAsync();
        lunarDate.LastSyncedAt = DateTime.UtcNow;

        var existing = await GetLunarDateAsync(lunarDate.GregorianDate);
        if (existing != null)
        {
            lunarDate.Id = existing.Id;
            await _database!.UpdateAsync(lunarDate);
        }
        else
        {
            await _database!.InsertAsync(lunarDate);
        }
    }

    public async Task SaveLunarDatesAsync(List<LunarDateEntity> lunarDates)
    {
        await InitAsync();
        await _database!.RunInTransactionAsync(tran =>
        {
            foreach (var lunarDate in lunarDates)
            {
                lunarDate.LastSyncedAt = DateTime.UtcNow;
                var existing = tran.Table<LunarDateEntity>()
                    .Where(ld => ld.GregorianDate == lunarDate.GregorianDate)
                    .FirstOrDefault();

                if (existing != null)
                {
                    lunarDate.Id = existing.Id;
                    tran.Update(lunarDate);
                }
                else
                {
                    tran.Insert(lunarDate);
                }
            }
        });
    }

    #endregion

    #region Holiday Occurrences

    public async Task<List<HolidayOccurrenceEntity>> GetHolidaysForMonthAsync(int year, int month)
    {
        await InitAsync();
        return await _database!.Table<HolidayOccurrenceEntity>()
            .Where(ho => ho.Year == year && ho.Month == month)
            .ToListAsync();
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
        await InitAsync();
        await _database!.RunInTransactionAsync(tran =>
        {
            foreach (var occurrence in occurrences)
            {
                occurrence.LastSyncedAt = DateTime.UtcNow;
                var existing = tran.Table<HolidayOccurrenceEntity>()
                    .Where(ho => ho.GregorianDate == occurrence.GregorianDate && ho.Name == occurrence.Name)
                    .FirstOrDefault();

                if (existing != null)
                {
                    occurrence.Id = existing.Id;
                    tran.Update(occurrence);
                }
                else
                {
                    tran.Insert(occurrence);
                }
            }
        });
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
