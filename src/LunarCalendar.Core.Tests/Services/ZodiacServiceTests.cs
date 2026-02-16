using FluentAssertions;
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;
using Xunit;

namespace LunarCalendar.Core.Tests.Services;

public class ZodiacServiceTests
{
    private readonly IZodiacService _sut;

    public ZodiacServiceTests()
    {
        var lunar = new LunarCalculationService();
        var sexagenary = new SexagenaryService();
        _sut = new ZodiacService(lunar, sexagenary);
    }

    [Theory]
    // Project's ChineseLunisolarCalendar-based implementation validates Lunar New Year 2026 as Feb 17, 2026.
    // Keep this test aligned with the actual implementation (see LunarCalendar.Api.Tests).
    [InlineData("2026-02-16", ZodiacAnimal.Snake)]
    [InlineData("2026-02-17", ZodiacAnimal.Horse)]
    public void GetAnimalForDate_should_follow_lunar_year_boundary(string isoDate, ZodiacAnimal expected)
    {
        var date = DateTime.Parse(isoDate);

        var animal = _sut.GetAnimalForDate(date);

        animal.Should().Be(expected);
    }

    [Theory]
    [InlineData(2024, ZodiacAnimal.Dragon)]
    [InlineData(2025, ZodiacAnimal.Snake)]
    [InlineData(2026, ZodiacAnimal.Horse)]
    [InlineData(2027, ZodiacAnimal.Goat)]
    public void GetAnimalForLunarYear_should_match_existing_sexagenary_mapping(int lunarYear, ZodiacAnimal expected)
    {
        _sut.GetAnimalForLunarYear(lunarYear).Should().Be(expected);
    }

    [Fact]
    public void GetElementalAnimalForLunarYear_should_return_element_and_animal()
    {
        var result = _sut.GetElementalAnimalForLunarYear(2026);

        result.LunarYear.Should().Be(2026);
        result.Animal.Should().Be(ZodiacAnimal.Horse);
        result.Element.Should().BeOneOf(Enum.GetValues<FiveElement>());
        result.DisplayName.Should().Contain("Horse");
    }
}
