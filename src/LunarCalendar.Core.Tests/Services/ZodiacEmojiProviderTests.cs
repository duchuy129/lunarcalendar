using FluentAssertions;
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;
using Xunit;

namespace LunarCalendar.Core.Tests.Services;

public class ZodiacEmojiProviderTests
{
    [Theory]
    [InlineData(ZodiacAnimal.Rat, "🐭")]
    [InlineData(ZodiacAnimal.Ox, "🐮")]
    [InlineData(ZodiacAnimal.Tiger, "🐯")]
    [InlineData(ZodiacAnimal.Rabbit, "🐰")]
    [InlineData(ZodiacAnimal.Dragon, "🐲")]
    [InlineData(ZodiacAnimal.Snake, "🐍")]
    [InlineData(ZodiacAnimal.Horse, "🐴")]
    [InlineData(ZodiacAnimal.Goat, "🐑")]
    [InlineData(ZodiacAnimal.Monkey, "🐵")]
    [InlineData(ZodiacAnimal.Rooster, "🐔")]
    [InlineData(ZodiacAnimal.Dog, "🐶")]
    [InlineData(ZodiacAnimal.Pig, "🐷")]
    public void GetEmoji_ReturnsExpectedEmoji(ZodiacAnimal animal, string emoji)
    {
        ZodiacEmojiProvider.GetEmoji(animal).Should().Be(emoji);
    }
}
