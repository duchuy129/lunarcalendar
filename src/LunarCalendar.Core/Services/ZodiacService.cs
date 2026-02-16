using LunarCalendar.Core.Models;

namespace LunarCalendar.Core.Services;

/// <summary>
/// Zodiac calculations based on the lunar year (not the Gregorian year).
/// Uses existing bundled/offline services:
/// - ILunarCalculationService for Gregorian -> Lunar conversion
/// - ISexagenaryService for stem/branch and zodiac mapping
/// </summary>
public sealed class ZodiacService : IZodiacService
{
    private readonly ILunarCalculationService _lunarCalculationService;
    private readonly ISexagenaryService _sexagenaryService;

    public ZodiacService(
        ILunarCalculationService lunarCalculationService,
        ISexagenaryService sexagenaryService)
    {
        _lunarCalculationService = lunarCalculationService;
        _sexagenaryService = sexagenaryService;
    }

    public ZodiacAnimal GetAnimalForDate(DateTime gregorianDate)
    {
        var lunar = _lunarCalculationService.ConvertToLunar(gregorianDate);
        return GetAnimalForLunarYear(lunar.LunarYear);
    }

    public ZodiacAnimal GetAnimalForLunarYear(int lunarYear)
    {
        // The existing Sexagenary implementation already maps EarthlyBranch -> ZodiacAnimal.
        var (_, _, zodiac) = _sexagenaryService.GetYearInfo(lunarYear);
        return zodiac;
    }

    public ElementalAnimal GetElementalAnimalForLunarYear(int lunarYear)
    {
        var (stem, branch, zodiac) = _sexagenaryService.GetYearInfo(lunarYear);
        return new ElementalAnimal(lunarYear, stem.GetElement(), branch, zodiac);
    }

    public ElementalAnimal GetElementalAnimalForDate(DateTime gregorianDate)
    {
        var lunar = _lunarCalculationService.ConvertToLunar(gregorianDate);
        return GetElementalAnimalForLunarYear(lunar.LunarYear);
    }
}
