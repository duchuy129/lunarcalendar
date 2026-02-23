using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;

namespace LunarCalendar.Core.Tests.Services;

public class ZodiacCompatibilityEngineTests
{
    private readonly ZodiacCompatibilityEngine _engine = new();

    [Fact]
    public async Task CalculateAsync_RatAndHorse_ReturnsChallenging()
    {
        var result = await _engine.CalculateAsync(ZodiacAnimal.Rat, ZodiacAnimal.Horse);

        Assert.Equal(ZodiacAnimal.Rat, result.Animal1);
        Assert.Equal(ZodiacAnimal.Horse, result.Animal2);
        Assert.Equal("Challenging", result.Rating);
        Assert.True(result.Score < 50, "Rat-Horse should be a low score");
    }

    [Fact]
    public async Task CalculateAsync_RatAndDragon_ReturnsExcellent()
    {
        var result = await _engine.CalculateAsync(ZodiacAnimal.Rat, ZodiacAnimal.Dragon);

        Assert.Equal("Excellent", result.Rating);
        Assert.True(result.Score >= 85, "Rat-Dragon should be a high score");
    }

    [Fact]
    public async Task CalculateAsync_IsSymmetric()
    {
        var forward = await _engine.CalculateAsync(ZodiacAnimal.Tiger, ZodiacAnimal.Dog);
        var reverse = await _engine.CalculateAsync(ZodiacAnimal.Dog, ZodiacAnimal.Tiger);

        Assert.Equal(forward.Score, reverse.Score);
        Assert.Equal(forward.Rating, reverse.Rating);
    }

    [Fact]
    public async Task CalculateAsync_SameAnimal_ReturnsResult()
    {
        var result = await _engine.CalculateAsync(ZodiacAnimal.Horse, ZodiacAnimal.Horse);

        Assert.True(result.Score > 0, "Self-compatibility should have a positive score");
        Assert.False(string.IsNullOrWhiteSpace(result.DescriptionEn));
        Assert.False(string.IsNullOrWhiteSpace(result.DescriptionVi));
    }

    [Fact]
    public async Task GetAllForAnimalAsync_Returns12Entries()
    {
        var results = await _engine.GetAllForAnimalAsync(ZodiacAnimal.Horse);

        // Horse should appear in exactly 12 pairings (Horse-Rat through Horse-Pig + Horse-Horse)
        Assert.Equal(12, results.Count);
    }

    [Fact]
    public async Task GetAllAsync_Returns78Entries()
    {
        var results = await _engine.GetAllAsync();

        // Upper-triangle including diagonal: 12 + 11 + 10 + ... + 1 = 78
        Assert.Equal(78, results.Count);
    }

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
    public async Task CalculateAsync_AllAnimalsHaveSelfCompatibility(ZodiacAnimal animal)
    {
        var result = await _engine.CalculateAsync(animal, animal);

        Assert.NotNull(result);
        Assert.True(result.Score > 0 && result.Score <= 100);
    }

    [Fact]
    public async Task GetAllAsync_AllEntriesHaveValidScores()
    {
        var results = await _engine.GetAllAsync();

        foreach (var entry in results)
        {
            Assert.True(entry.Score >= 0 && entry.Score <= 100,
                $"{entry.Animal1}-{entry.Animal2} score {entry.Score} out of range");
            Assert.False(string.IsNullOrWhiteSpace(entry.Rating),
                $"{entry.Animal1}-{entry.Animal2} missing rating");
            Assert.False(string.IsNullOrWhiteSpace(entry.DescriptionEn),
                $"{entry.Animal1}-{entry.Animal2} missing English description");
            Assert.False(string.IsNullOrWhiteSpace(entry.DescriptionVi),
                $"{entry.Animal1}-{entry.Animal2} missing Vietnamese description");
        }
    }

    [Fact]
    public async Task GetAllAsync_AllEntriesHaveValidRating()
    {
        var validRatings = new[] { "Excellent", "Good", "Fair", "Challenging" };
        var results = await _engine.GetAllAsync();

        foreach (var entry in results)
        {
            Assert.Contains(entry.Rating, validRatings);
        }
    }

    [Theory]
    [InlineData(ZodiacAnimal.Tiger, ZodiacAnimal.Horse, "Excellent")]
    [InlineData(ZodiacAnimal.Tiger, ZodiacAnimal.Dog, "Excellent")]
    [InlineData(ZodiacAnimal.Ox, ZodiacAnimal.Snake, "Excellent")]
    [InlineData(ZodiacAnimal.Ox, ZodiacAnimal.Rooster, "Excellent")]
    [InlineData(ZodiacAnimal.Dragon, ZodiacAnimal.Dog, "Challenging")]
    [InlineData(ZodiacAnimal.Rabbit, ZodiacAnimal.Rooster, "Challenging")]
    public async Task CalculateAsync_TraditionalPairings_MatchExpected(
        ZodiacAnimal a, ZodiacAnimal b, string expectedRating)
    {
        var result = await _engine.CalculateAsync(a, b);
        Assert.Equal(expectedRating, result.Rating);
    }
}
