namespace LunarCalendar.Core.Models;

/// <summary>
/// Represents the compatibility relationship between two zodiac animals.
/// Score is 0-100 where higher means more compatible.
/// </summary>
public class ZodiacCompatibility
{
    public required ZodiacAnimal Animal1 { get; init; }
    public required ZodiacAnimal Animal2 { get; init; }
    
    /// <summary>Score from 0-100 indicating compatibility level.</summary>
    public required int Score { get; init; }
    
    /// <summary>Rating category: "Excellent", "Good", "Fair", "Challenging".</summary>
    public required string Rating { get; init; }
    
    /// <summary>English description of the compatibility.</summary>
    public required string DescriptionEn { get; init; }
    
    /// <summary>Vietnamese description of the compatibility.</summary>
    public required string DescriptionVi { get; init; }
}
