using FluentAssertions;
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;
using Xunit;

namespace LunarCalendar.Core.Tests.Services;

public class ZodiacDataRepositoryTests
{
    private readonly IZodiacDataRepository _repository = new ZodiacDataRepository();

    [Fact]
    public async Task GetAllAsync_ReturnsAll12Animals()
    {
        var all = await _repository.GetAllAsync();
        
        all.Should().HaveCount(12);
        all.Select(z => z.Animal).Should().BeEquivalentTo(Enum.GetValues<ZodiacAnimal>());
    }

    [Theory]
    [InlineData(ZodiacAnimal.Rat, "Rat", "Chuột")]
    [InlineData(ZodiacAnimal.Horse, "Horse", "Ngựa")]
    [InlineData(ZodiacAnimal.Dragon, "Dragon", "Rồng")]
    public async Task GetByAnimalAsync_ReturnsCorrectInfo(ZodiacAnimal animal, string expectedEn, string expectedVi)
    {
        var info = await _repository.GetByAnimalAsync(animal);
        
        info.Should().NotBeNull();
        info!.Animal.Should().Be(animal);
        info.NameEn.Should().Be(expectedEn);
        info.NameVi.Should().Be(expectedVi);
    }

    [Fact]
    public async Task GetByAnimalAsync_ReturnsCompleteData()
    {
        var info = await _repository.GetByAnimalAsync(ZodiacAnimal.Tiger);
        
        info.Should().NotBeNull();
        info!.TraitsEn.Should().NotBeNullOrEmpty();
        info.TraitsVi.Should().NotBeNullOrEmpty();
        info.PersonalityEn.Should().NotBeNullOrEmpty();
        info.PersonalityVi.Should().NotBeNullOrEmpty();
        info.LuckyNumbers.Should().NotBeEmpty();
        info.LuckyColors.Should().NotBeEmpty();
        info.LuckyDirections.Should().NotBeEmpty();
        info.SignificanceEn.Should().NotBeNullOrEmpty();
        info.SignificanceVi.Should().NotBeNullOrEmpty();
        info.BestCompatibility.Should().NotBeEmpty();
        info.ChallengeCompatibility.Should().NotBeEmpty();
        info.RecentYears.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_CachesResults()
    {
        var first = await _repository.GetAllAsync();
        var second = await _repository.GetAllAsync();
        
        // Same instance = cached
        first.Should().BeSameAs(second);
    }
}
