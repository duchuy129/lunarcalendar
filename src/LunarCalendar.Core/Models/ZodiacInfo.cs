namespace LunarCalendar.Core.Models;

/// <summary>
/// Rich informational content for a zodiac animal.
/// Sprint 10: EN + VI only; Chinese name allowed as cultural reference.
/// </summary>
public class ZodiacInfo
{
    public required ZodiacAnimal Animal { get; init; }
    
    // Localized names
    public required string NameEn { get; init; }
    public required string NameVi { get; init; }
    public string? ChineseName { get; init; } // Cultural reference only
    
    // Characteristics
    public required string TraitsEn { get; init; }
    public required string TraitsVi { get; init; }
    
    public required string PersonalityEn { get; init; }
    public required string PersonalityVi { get; init; }
    
    // Lucky attributes
    public required int[] LuckyNumbers { get; init; }
    public required string[] LuckyColors { get; init; }
    public required string[] LuckyDirections { get; init; }
    
    // Cultural significance
    public required string SignificanceEn { get; init; }
    public required string SignificanceVi { get; init; }
    
    // Compatibility (animal names, not enum - for JSON simplicity)
    public required string[] BestCompatibility { get; init; }
    public required string[] ChallengeCompatibility { get; init; }
    
    // Years (next few occurrences for quick reference)
    public required int[] RecentYears { get; init; }
}
