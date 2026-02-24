using FluentAssertions;
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;
using Xunit;

namespace LunarCalendar.Core.Tests.Services;

/// <summary>
/// T070: 100-year zodiac calculation sweep.
/// Validates GetAnimalForLunarYear for every year 1924–2043 (five complete 12-year cycles × ~10).
/// The 12-animal cycle is anchored: 1924 = Rat (index 0), repeating every 12 years.
/// Also validates elemental-animal correctness on a representative sample, and
/// Gregorian-date boundary for years near the beginning of 2026's Lunar New Year.
/// </summary>
public class ZodiacServiceYearSweepTests
{
    private readonly IZodiacService _sut;

    /// <summary>
    /// The 12 animals in cycle order, starting at Rat (anchor year 1924).
    /// Index = (lunarYear - 1924) mod 12 → expected animal.
    /// </summary>
    private static readonly ZodiacAnimal[] CycleOrder =
    [
        ZodiacAnimal.Rat,     // 0
        ZodiacAnimal.Ox,      // 1
        ZodiacAnimal.Tiger,   // 2
        ZodiacAnimal.Rabbit,  // 3
        ZodiacAnimal.Dragon,  // 4
        ZodiacAnimal.Snake,   // 5
        ZodiacAnimal.Horse,   // 6
        ZodiacAnimal.Goat,    // 7
        ZodiacAnimal.Monkey,  // 8
        ZodiacAnimal.Rooster, // 9
        ZodiacAnimal.Dog,     // 10
        ZodiacAnimal.Pig,     // 11
    ];

    public ZodiacServiceYearSweepTests()
    {
        var lunar = new LunarCalculationService();
        var sexagenary = new SexagenaryService();
        _sut = new ZodiacService(lunar, sexagenary);
    }

    // ── 100-year sweep via MemberData to keep individual assertion messages clear ──

    public static IEnumerable<object[]> YearsFrom1924To2043()
    {
        for (int year = 1924; year <= 2043; year++)
        {
            var expected = CycleOrder[(year - 1924) % 12];
            yield return [year, expected];
        }
    }

    [Theory]
    [MemberData(nameof(YearsFrom1924To2043))]
    public void GetAnimalForLunarYear_100year_sweep(int lunarYear, ZodiacAnimal expected)
    {
        _sut.GetAnimalForLunarYear(lunarYear).Should().Be(expected,
            because: $"lunar year {lunarYear} is cycle position {(lunarYear - 1924) % 12}");
    }

    // ── Spot-check: well-known historical years ──

    [Theory]
    [InlineData(1936, ZodiacAnimal.Rat)]
    [InlineData(1949, ZodiacAnimal.Ox)]
    [InlineData(1950, ZodiacAnimal.Tiger)]
    [InlineData(1963, ZodiacAnimal.Rabbit)]
    [InlineData(1976, ZodiacAnimal.Dragon)]
    [InlineData(1989, ZodiacAnimal.Snake)]
    [InlineData(1990, ZodiacAnimal.Horse)]
    [InlineData(2000, ZodiacAnimal.Dragon)]
    [InlineData(2008, ZodiacAnimal.Rat)]
    [InlineData(2012, ZodiacAnimal.Dragon)]
    [InlineData(2020, ZodiacAnimal.Rat)]
    [InlineData(2023, ZodiacAnimal.Rabbit)]
    [InlineData(2024, ZodiacAnimal.Dragon)]
    [InlineData(2025, ZodiacAnimal.Snake)]
    [InlineData(2026, ZodiacAnimal.Horse)]
    [InlineData(2038, ZodiacAnimal.Horse)]  // 2026 + 12
    public void GetAnimalForLunarYear_wellKnown_years(int lunarYear, ZodiacAnimal expected)
    {
        _sut.GetAnimalForLunarYear(lunarYear).Should().Be(expected);
    }

    // ── Boundary: Gregorian date before / on Lunar New Year ──

    [Theory]
    // 2026: Lunar New Year is Feb 17, 2026 (Horse year begins)
    [InlineData("2026-01-01", ZodiacAnimal.Snake)]  // Jan 1 is still Snake year
    [InlineData("2026-02-16", ZodiacAnimal.Snake)]  // Eve of LNY 2026
    [InlineData("2026-02-17", ZodiacAnimal.Horse)]  // LNY 2026 — Horse begins
    [InlineData("2026-12-31", ZodiacAnimal.Horse)]  // Late in the Horse year
    // 2025: Lunar New Year is Jan 29, 2025 (Snake year begins)
    [InlineData("2025-01-28", ZodiacAnimal.Dragon)] // Eve of LNY 2025
    [InlineData("2025-01-29", ZodiacAnimal.Snake)]  // LNY 2025 — Snake begins
    public void GetAnimalForDate_respects_lunar_new_year_boundary(string isoDate, ZodiacAnimal expected)
    {
        var date = DateTime.Parse(isoDate);
        _sut.GetAnimalForDate(date).Should().Be(expected,
            because: $"{isoDate} falls in the {expected} lunar year");
    }

    // ── Elemental-animal: 10-year sample ──

    [Theory]
    [InlineData(2020, ZodiacAnimal.Rat)]
    [InlineData(2021, ZodiacAnimal.Ox)]
    [InlineData(2022, ZodiacAnimal.Tiger)]
    [InlineData(2023, ZodiacAnimal.Rabbit)]
    [InlineData(2024, ZodiacAnimal.Dragon)]
    [InlineData(2025, ZodiacAnimal.Snake)]
    [InlineData(2026, ZodiacAnimal.Horse)]
    [InlineData(2027, ZodiacAnimal.Goat)]
    [InlineData(2028, ZodiacAnimal.Monkey)]
    [InlineData(2029, ZodiacAnimal.Rooster)]
    public void GetElementalAnimalForLunarYear_animal_matches_sweep(int lunarYear, ZodiacAnimal expectedAnimal)
    {
        var result = _sut.GetElementalAnimalForLunarYear(lunarYear);

        result.Animal.Should().Be(expectedAnimal, because: $"elemental animal for {lunarYear} must match zodiac cycle");
        result.LunarYear.Should().Be(lunarYear);
        Enum.IsDefined(result.Element).Should().BeTrue(because: "element must be a valid FiveElement value");
        result.DisplayName.Should().NotBeNullOrWhiteSpace();
    }

    // ── Cycle integrity: consecutive years differ ──

    [Fact]
    public void GetAnimalForLunarYear_consecutive_years_cycle_through_12_animals()
    {
        var seen = new HashSet<ZodiacAnimal>();
        for (int year = 2020; year <= 2031; year++)
        {
            seen.Add(_sut.GetAnimalForLunarYear(year));
        }
        seen.Should().HaveCount(12, because: "12 consecutive years must produce all 12 distinct animals");
    }

    // ── Performance: entire sweep is fast ──

    [Fact]
    public void GetAnimalForLunarYear_1000_calls_complete_under_100ms()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        for (int year = 1924; year <= 2043; year++)
            _ = _sut.GetAnimalForLunarYear(year);
        // Run a second pass to include warm-up overhead
        for (int year = 1924; year <= 2043; year++)
            _ = _sut.GetAnimalForLunarYear(year);
        sw.Stop();

        sw.ElapsedMilliseconds.Should().BeLessThan(100,
            because: "zodiac calculation should be near-instant (<0.05ms per call)");
    }
}
