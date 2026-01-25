using System.Collections.Concurrent;
using LunarCalendar.Core.Models;

namespace LunarCalendar.Core.Services;

/// <summary>
/// Service implementation for Sexagenary Cycle calculations with caching support.
/// Caches calculations for current month +/- 1 month with LRU eviction.
/// </summary>
public class SexagenaryService : ISexagenaryService
{
    private readonly ConcurrentDictionary<DateTime, SexagenaryInfo> _cache;
    private readonly int _maxCacheSize;
    private readonly Queue<DateTime> _cacheAccessOrder;
    private readonly object _cacheLock = new object();
    
    public SexagenaryService(int maxCacheSize = 100)
    {
        _cache = new ConcurrentDictionary<DateTime, SexagenaryInfo>();
        _maxCacheSize = maxCacheSize;
        _cacheAccessOrder = new Queue<DateTime>();
    }
    
    /// <inheritdoc />
    public SexagenaryInfo GetSexagenaryInfo(DateTime date)
    {
        // Normalize date to midnight for cache key consistency
        var normalizedDate = date.Date;
        
        // Try to get from cache
        if (_cache.TryGetValue(normalizedDate, out var cachedInfo))
        {
            // Update access time if we have time component
            if (date.TimeOfDay.TotalSeconds > 0 && cachedInfo.HourStem == null)
            {
                // Recalculate with time component
                return CalculateAndCache(date);
            }
            return cachedInfo;
        }
        
        // Calculate and cache
        return CalculateAndCache(date);
    }
    
    /// <inheritdoc />
    public SexagenaryInfo GetTodaySexagenaryInfo()
    {
        return GetSexagenaryInfo(DateTime.Now);
    }
    
    /// <inheritdoc />
    public (HeavenlyStem Stem, EarthlyBranch Branch) GetDayStemBranch(DateTime date)
    {
        var info = GetSexagenaryInfo(date);
        return (info.DayStem, info.DayBranch);
    }
    
    /// <inheritdoc />
    public (HeavenlyStem Stem, EarthlyBranch Branch, ZodiacAnimal Zodiac) GetYearInfo(int lunarYear)
    {
        var (stem, branch) = SexagenaryCalculator.CalculateYearStemBranch(lunarYear);
        var zodiac = branch.GetZodiacAnimal();
        return (stem, branch, zodiac);
    }
    
    /// <inheritdoc />
    public HourStemBranchInfo[] GetAllHourStemBranches(DateTime date)
    {
        return SexagenaryCalculator.CalculateAllHourStemBranches(date);
    }
    
    /// <inheritdoc />
    public IDictionary<DateTime, SexagenaryInfo> GetSexagenaryInfoRange(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date must be before or equal to end date");
        
        var result = new Dictionary<DateTime, SexagenaryInfo>();
        var currentDate = startDate.Date;
        
        while (currentDate <= endDate.Date)
        {
            result[currentDate] = GetSexagenaryInfo(currentDate);
            currentDate = currentDate.AddDays(1);
        }
        
        return result;
    }
    
    /// <inheritdoc />
    public void ClearCache()
    {
        lock (_cacheLock)
        {
            _cache.Clear();
            _cacheAccessOrder.Clear();
        }
    }
    
    /// <summary>
    /// Pre-load cache for a specific month and adjacent months
    /// This is useful for calendar view initialization
    /// </summary>
    public void PreloadMonth(int year, int month)
    {
        // Calculate date range: current month +/- 1 month
        var startDate = new DateTime(year, month, 1).AddMonths(-1);
        var endDate = new DateTime(year, month, 1).AddMonths(2).AddDays(-1);
        
        // Pre-calculate all dates in range
        GetSexagenaryInfoRange(startDate, endDate);
    }
    
    /// <summary>
    /// Calculate sexagenary info and add to cache with LRU eviction
    /// </summary>
    private SexagenaryInfo CalculateAndCache(DateTime date)
    {
        var info = SexagenaryCalculator.CalculateSexagenaryInfo(date);
        var normalizedDate = date.Date;
        
        lock (_cacheLock)
        {
            // Add to cache
            _cache[normalizedDate] = info;
            _cacheAccessOrder.Enqueue(normalizedDate);
            
            // LRU eviction if cache is full
            while (_cache.Count > _maxCacheSize && _cacheAccessOrder.Count > 0)
            {
                var oldestDate = _cacheAccessOrder.Dequeue();
                _cache.TryRemove(oldestDate, out _);
            }
        }
        
        return info;
    }
    
    /// <summary>
    /// Get cache statistics for monitoring/debugging
    /// </summary>
    public (int CacheSize, int MaxSize, double HitRate) GetCacheStats()
    {
        // This is a simplified version - in production you'd track hits/misses
        return (_cache.Count, _maxCacheSize, 0.0);
    }
}
