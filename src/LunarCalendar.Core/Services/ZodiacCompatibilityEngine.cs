using System.Reflection;
using System.Text.Json;
using LunarCalendar.Core.Models;

namespace LunarCalendar.Core.Services;

/// <summary>
/// Interface for the zodiac compatibility engine.
/// </summary>
public interface IZodiacCompatibilityEngine
{
    /// <summary>
    /// Calculate compatibility between two zodiac animals.
    /// Order of animals does not matter — (Rat, Horse) == (Horse, Rat).
    /// </summary>
    Task<ZodiacCompatibility> CalculateAsync(ZodiacAnimal animal1, ZodiacAnimal animal2);

    /// <summary>
    /// Get all compatibility entries for a given animal.
    /// Returns 12 entries (including self-compatibility).
    /// </summary>
    Task<IReadOnlyList<ZodiacCompatibility>> GetAllForAnimalAsync(ZodiacAnimal animal);

    /// <summary>
    /// Get all 78 unique pairings (including 12 self-pairings = 78 total stored).
    /// </summary>
    Task<IReadOnlyList<ZodiacCompatibility>> GetAllAsync();
}

/// <summary>
/// Loads zodiac compatibility data from bundled JSON and provides lookup.
/// Data is stored as upper-triangle (Animal1 &lt;= Animal2 by enum value)
/// so each pair is stored once; lookups normalise order automatically.
/// </summary>
public class ZodiacCompatibilityEngine : IZodiacCompatibilityEngine
{
    private Dictionary<(ZodiacAnimal, ZodiacAnimal), ZodiacCompatibility>? _cache;
    private List<ZodiacCompatibility>? _all;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public async Task<ZodiacCompatibility> CalculateAsync(ZodiacAnimal animal1, ZodiacAnimal animal2)
    {
        await EnsureLoadedAsync();
        var key = Normalise(animal1, animal2);
        return _cache!.TryGetValue(key, out var result)
            ? result
            : CreateFallback(animal1, animal2);
    }

    public async Task<IReadOnlyList<ZodiacCompatibility>> GetAllForAnimalAsync(ZodiacAnimal animal)
    {
        await EnsureLoadedAsync();
        return _all!
            .Where(c => c.Animal1 == animal || c.Animal2 == animal)
            .ToList();
    }

    public async Task<IReadOnlyList<ZodiacCompatibility>> GetAllAsync()
    {
        await EnsureLoadedAsync();
        return _all!;
    }

    // --- private helpers ---

    private static (ZodiacAnimal, ZodiacAnimal) Normalise(ZodiacAnimal a, ZodiacAnimal b)
        => a <= b ? (a, b) : (b, a);

    private static ZodiacCompatibility CreateFallback(ZodiacAnimal a, ZodiacAnimal b)
        => new()
        {
            Animal1 = a,
            Animal2 = b,
            Score = 50,
            Rating = "Fair",
            DescriptionEn = "Compatibility information is not available.",
            DescriptionVi = "Thông tin tương hợp chưa có sẵn."
        };

    private async Task EnsureLoadedAsync()
    {
        if (_cache != null) return;

        await _lock.WaitAsync();
        try
        {
            if (_cache != null) return;

            var assembly = typeof(ZodiacCompatibilityEngine).Assembly;
            var resourceName = "LunarCalendar.Core.Data.ZodiacCompatibility.json";

            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };

            var data = await JsonSerializer.DeserializeAsync<List<ZodiacCompatibility>>(stream, options)
                ?? throw new InvalidOperationException("Failed to deserialize ZodiacCompatibility.json.");

            var dict = new Dictionary<(ZodiacAnimal, ZodiacAnimal), ZodiacCompatibility>(data.Count);
            foreach (var entry in data)
            {
                var key = Normalise(entry.Animal1, entry.Animal2);
                dict[key] = entry;
            }

            _all = data;
            _cache = dict;
        }
        finally
        {
            _lock.Release();
        }
    }
}
