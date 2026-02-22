namespace LunarCalendar.Core.Models;

/// <summary>
/// A convenience model representing the combined Five Element + Zodiac animal
/// for a given lunar year (e.g., Fire Horse).
/// </summary>
public sealed record ElementalAnimal(
    int LunarYear,
    FiveElement Element,
    EarthlyBranch Branch,
    ZodiacAnimal Animal)
{
    public string DisplayName => $"{Element} {Animal}";
}
