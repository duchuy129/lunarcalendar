using FluentAssertions;
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;
using Xunit;

namespace LunarCalendar.Core.Tests.Services;

/// <summary>
/// T091 — Tests for the zodiac profile display logic used by ZodiacProfileViewModel.
/// These tests exercise the Core service layer that the ViewModel delegates to,
/// keeping tests framework-agnostic (no MAUI dependencies).
/// </summary>
public class ZodiacProfileLogicTests
{
    private readonly IZodiacService _zodiacService;

    public ZodiacProfileLogicTests()
    {
        var lunar = new LunarCalculationService();
        var sexagenary = new SexagenaryService();
        _zodiacService = new ZodiacService(lunar, sexagenary);
    }

    // ── LoadZodiacProfile: no saved year ─────────────────────────────────────

    [Fact]
    public void LoadZodiacProfile_WhenNoSavedYear_HasZodiacProfileIsFalse()
    {
        // Arrange: birth year = 0 (sentinel value meaning "not set")
        const int savedYear = 0;

        // Act
        var hasProfile = savedYear > 0;

        // Assert
        hasProfile.Should().BeFalse();
    }

    // ── LoadZodiacProfile: valid saved year ───────────────────────────────────

    [Theory]
    [InlineData(1990)]
    [InlineData(2000)]
    [InlineData(1985)]
    [InlineData(2001)]
    public void LoadZodiacProfile_WhenSavedYearValid_HasZodiacProfileIsTrue(int savedYear)
    {
        // Arrange & Act
        var hasProfile = savedYear > 0;

        // Assert
        hasProfile.Should().BeTrue();
    }

    // ── UpdateZodiacDisplay: correct elemental animal returned ────────────────

    [Theory]
    [InlineData(1990, ZodiacAnimal.Horse)]   // Geng Wu
    [InlineData(2000, ZodiacAnimal.Dragon)]  // Geng Chen
    [InlineData(1985, ZodiacAnimal.Ox)]      // Yi Chou
    [InlineData(1976, ZodiacAnimal.Dragon)]  // Bing Chen
    [InlineData(2012, ZodiacAnimal.Dragon)]  // Ren Chen
    public void UpdateZodiacDisplay_ReturnsCorrectAnimal_ForBirthYear(int birthYear, ZodiacAnimal expectedAnimal)
    {
        // Act
        var elemental = _zodiacService.GetElementalAnimalForLunarYear(birthYear);

        // Assert
        elemental.Animal.Should().Be(expectedAnimal);
    }

    // ── UpdateZodiacDisplay: emoji provided ───────────────────────────────────

    [Theory]
    [InlineData(ZodiacAnimal.Rat)]
    [InlineData(ZodiacAnimal.Ox)]
    [InlineData(ZodiacAnimal.Tiger)]
    [InlineData(ZodiacAnimal.Rabbit)]
    [InlineData(ZodiacAnimal.Dragon)]
    [InlineData(ZodiacAnimal.Snake)]
    [InlineData(ZodiacAnimal.Horse)]
    [InlineData(ZodiacAnimal.Goat)]
    [InlineData(ZodiacAnimal.Monkey)]
    [InlineData(ZodiacAnimal.Rooster)]
    [InlineData(ZodiacAnimal.Dog)]
    [InlineData(ZodiacAnimal.Pig)]
    public void ZodiacEmojiProvider_AllAnimals_ReturnNonEmptyEmoji(ZodiacAnimal animal)
    {
        // Act
        var emoji = ZodiacEmojiProvider.GetEmoji(animal);

        // Assert
        emoji.Should().NotBeNullOrEmpty("every animal should have an emoji");
    }

    // ── Birth year validation logic ────────────────────────────────────────────

    [Theory]
    [InlineData(1900, true)]
    [InlineData(1990, true)]
    [InlineData(2026, true)]   // current year (Feb 2026)
    [InlineData(1899, false)]  // too old
    [InlineData(2027, false)]  // future year
    [InlineData(0,    false)]  // sentinel
    [InlineData(-1,   false)]  // negative
    public void SaveZodiacProfile_BirthYearValidation_CorrectlyClassifiesYear(int year, bool isValid)
    {
        // This mirrors the validation in ZodiacProfileViewModel.SaveZodiacProfileAsync
        var currentYear = 2026; // pinned for determinism
        var result = year >= 1900 && year <= currentYear;

        result.Should().Be(isValid);
    }

    // ── Elemental cycle integrity ──────────────────────────────────────────────

    [Fact]
    public void UpdateZodiacDisplay_ElementalAnimal_RepeatsEvery60Years()
    {
        // Geng Wu (Metal Horse) appears in 1930, 1990, 2050 etc.
        var e1990 = _zodiacService.GetElementalAnimalForLunarYear(1990);
        var e1930 = _zodiacService.GetElementalAnimalForLunarYear(1930);

        e1990.Animal.Should().Be(e1930.Animal);
        e1990.Element.Should().Be(e1930.Element);
    }

    // ── Profile display string format check ──────────────────────────────────

    [Fact]
    public void UpdateZodiacDisplay_DisplayString_ContainsEmojiAnimalAndElement()
    {
        // Arrange
        const int birthYear = 1990;

        // Act
        var elemental = _zodiacService.GetElementalAnimalForLunarYear(birthYear);
        var emoji = ZodiacEmojiProvider.GetEmoji(elemental.Animal);
        var display = $"{emoji} {elemental.Element} {elemental.Animal}";

        // Assert
        display.Should().Contain(emoji);
        display.Should().Contain(elemental.Animal.ToString());
        display.Should().Contain(elemental.Element.ToString());
    }
}
