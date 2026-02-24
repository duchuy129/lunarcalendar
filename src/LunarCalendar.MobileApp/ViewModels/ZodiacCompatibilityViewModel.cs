using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;
using LunarCalendar.MobileApp.Resources.Strings;
using System.Globalization;

namespace LunarCalendar.MobileApp.ViewModels;

public partial class ZodiacCompatibilityViewModel : BaseViewModel
{
    private readonly IZodiacCompatibilityEngine _engine;

    // Picker source - all 12 animals
    public List<AnimalPickerItem> AllAnimals { get; } = Enum.GetValues<ZodiacAnimal>()
        .Select(a => new AnimalPickerItem(a))
        .ToList();

    [ObservableProperty]
    private AnimalPickerItem? _selectedAnimal1;

    [ObservableProperty]
    private AnimalPickerItem? _selectedAnimal2;

    // Result properties
    [ObservableProperty]
    private bool _hasResult;

    [ObservableProperty]
    private string _resultEmoji1 = string.Empty;

    [ObservableProperty]
    private string _resultEmoji2 = string.Empty;

    [ObservableProperty]
    private string _resultAnimalPair = string.Empty;

    [ObservableProperty]
    private int _resultScore;

    [ObservableProperty]
    private string _resultRating = string.Empty;

    [ObservableProperty]
    private string _resultRatingLocalized = string.Empty;

    [ObservableProperty]
    private string _resultDescription = string.Empty;

    [ObservableProperty]
    private string _resultRatingColor = "#4B5563";

    [ObservableProperty]
    private bool _isCalculating;

    public ZodiacCompatibilityViewModel(IZodiacCompatibilityEngine engine)
    {
        _engine = engine;
        Title = AppResources.ResourceManager.GetString("ZodiacCompatibility", CultureInfo.CurrentUICulture) ?? "Zodiac Compatibility";
        // Pre-select first two animals as defaults
        SelectedAnimal1 = AllAnimals[0]; // Rat
        SelectedAnimal2 = AllAnimals[3]; // Rabbit
    }

    [RelayCommand]
    private async Task CheckCompatibilityAsync()
    {
        if (SelectedAnimal1 == null || SelectedAnimal2 == null)
            return;

        IsCalculating = true;
        HasResult = false;

        try
        {
            var result = await _engine.CalculateAsync(SelectedAnimal1.Animal, SelectedAnimal2.Animal);

            ResultEmoji1 = ZodiacEmojiProvider.GetEmoji(SelectedAnimal1.Animal);
            ResultEmoji2 = ZodiacEmojiProvider.GetEmoji(SelectedAnimal2.Animal);
            ResultAnimalPair = $"{SelectedAnimal1.DisplayName} & {SelectedAnimal2.DisplayName}";
            ResultScore = result.Score;
            ResultRating = result.Rating;
            ResultRatingLocalized = GetLocalizedRating(result.Rating);
            ResultRatingColor = GetRatingColor(result.Rating);

            // Description: use VI if current culture is Vietnamese
            var isVi = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "vi";
            ResultDescription = isVi ? result.DescriptionVi : result.DescriptionEn;

            HasResult = true;
        }
        finally
        {
            IsCalculating = false;
        }
    }

    private static string GetLocalizedRating(string rating)
    {
        var key = rating switch
        {
            "Excellent"   => "RatingExcellent",
            "Good"        => "RatingGood",
            "Fair"        => "RatingFair",
            "Challenging" => "RatingChallenging",
            _ => null
        };
        return key != null
            ? AppResources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? rating
            : rating;
    }

    private static string GetRatingColor(string rating) => rating switch
    {
        "Excellent"   => "#16A34A",
        "Good"        => "#2563EB",
        "Fair"        => "#D97706",
        "Challenging" => "#DC2626",
        _             => "#4B5563"
    };
}

/// <summary>
/// Wraps ZodiacAnimal with a localized display name for use in Pickers.
/// </summary>
public class AnimalPickerItem
{
    public ZodiacAnimal Animal { get; }
    public string Emoji { get; }
    public string AnimalName { get; }
    public string DisplayName { get; }

    public AnimalPickerItem(ZodiacAnimal animal)
    {
        Animal = animal;
        Emoji = ZodiacEmojiProvider.GetEmoji(animal);
        AnimalName = GetLocalizedAnimalName(animal);
        DisplayName = $"{Emoji} {AnimalName}";
    }

    private static string GetLocalizedAnimalName(ZodiacAnimal animal)
    {
        var key = animal switch
        {
            ZodiacAnimal.Rat     => "EarthlyBranch_Zi",
            ZodiacAnimal.Ox      => "EarthlyBranch_Chou",
            ZodiacAnimal.Tiger   => "EarthlyBranch_Yin",
            ZodiacAnimal.Rabbit  => "EarthlyBranch_Mao",
            ZodiacAnimal.Dragon  => "EarthlyBranch_Chen",
            ZodiacAnimal.Snake   => "EarthlyBranch_Si",
            ZodiacAnimal.Horse   => "EarthlyBranch_Wu",
            ZodiacAnimal.Goat    => "EarthlyBranch_Wei",
            ZodiacAnimal.Monkey  => "EarthlyBranch_Shen",
            ZodiacAnimal.Rooster => "EarthlyBranch_You",
            ZodiacAnimal.Dog     => "EarthlyBranch_Xu",
            ZodiacAnimal.Pig     => "EarthlyBranch_Hai",
            _ => string.Empty
        };
        return string.IsNullOrEmpty(key) ? animal.ToString()
            : AppResources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? animal.ToString();
    }
}
