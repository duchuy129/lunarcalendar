using System.Reflection;
using System.Text.Json;
using LunarCalendar.Core.Models;

namespace LunarCalendar.Core.Services;

/// <summary>
/// Loads and caches zodiac informational content from bundled JSON.
/// Sprint 10: EN + VI only; offline-first.
/// </summary>
public interface IZodiacDataRepository
{
    Task<ZodiacInfo?> GetByAnimalAsync(ZodiacAnimal animal);
    Task<IReadOnlyList<ZodiacInfo>> GetAllAsync();
}

public class ZodiacDataRepository : IZodiacDataRepository
{
    private List<ZodiacInfo>? _cache;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public async Task<ZodiacInfo?> GetByAnimalAsync(ZodiacAnimal animal)
    {
        await EnsureLoadedAsync();
        return _cache?.FirstOrDefault(z => z.Animal == animal);
    }

    public async Task<IReadOnlyList<ZodiacInfo>> GetAllAsync()
    {
        await EnsureLoadedAsync();
        return _cache ?? new List<ZodiacInfo>();
    }

    private async Task EnsureLoadedAsync()
    {
        if (_cache != null) return;

        await _lock.WaitAsync();
        try
        {
            if (_cache != null) return; // double-check

            var assembly = typeof(ZodiacDataRepository).Assembly;
            var resourceName = "LunarCalendar.Core.Data.ZodiacData.json";

            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };

            var data = await JsonSerializer.DeserializeAsync<List<ZodiacInfo>>(stream, options)
                ?? throw new InvalidOperationException("Failed to deserialize ZodiacData.json.");

            _cache = data;
        }
        finally
        {
            _lock.Release();
        }
    }
}
