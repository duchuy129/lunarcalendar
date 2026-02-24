using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;
using LunarCalendar.MobileApp.Services;
using System.Collections.ObjectModel;

namespace LunarCalendar.MobileApp.ViewModels;

public partial class ZodiacInformationViewModel : BaseViewModel
{
    private readonly IZodiacDataRepository _zodiacDataRepository;
    private readonly ILocalizationService _localizationService;

    [ObservableProperty]
    private ObservableCollection<ZodiacInfoDisplayItem> _zodiacAnimals = new();

    [ObservableProperty]
    private int _selectedIndex;

    [ObservableProperty]
    private bool _isDataLoaded = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasSelectedAnimal))]
    private ZodiacInfoDisplayItem? _selectedAnimal;

    public bool HasSelectedAnimal => SelectedAnimal != null;

    public ZodiacInformationViewModel(
        IZodiacDataRepository zodiacDataRepository,
        ILocalizationService localizationService)
    {
        _zodiacDataRepository = zodiacDataRepository;
        _localizationService = localizationService;

        Title = "Zodiac Animals";
        
        // CRITICAL: Initialize empty collection to prevent CarouselView crash
        ZodiacAnimals = new ObservableCollection<ZodiacInfoDisplayItem>();
    }

    public async Task InitializeAsync(ZodiacAnimal? initialAnimal = null)
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var allAnimals = await _zodiacDataRepository.GetAllAsync();
            
            ZodiacAnimals.Clear();
            foreach (var animal in allAnimals.OrderBy(a => (int)a.Animal))
            {
                var displayItem = new ZodiacInfoDisplayItem(animal, _localizationService);
                ZodiacAnimals.Add(displayItem);
            }

            // Set initial selection
            if (initialAnimal.HasValue)
            {
                var index = ZodiacAnimals
                    .Select((item, idx) => new { item, idx })
                    .FirstOrDefault(x => x.item.Animal == initialAnimal.Value)?.idx ?? 0;
                SelectedIndex = index;
                SelectedAnimal = ZodiacAnimals[index];
            }
            else if (ZodiacAnimals.Any())
            {
                SelectedIndex = 0;
                SelectedAnimal = ZodiacAnimals[0];
            }

            // CRITICAL: Only show CarouselView AFTER data is fully loaded
            IsDataLoaded = true;
        }
        catch (Exception ex)
        {
            // TODO: Show error toast/alert
            System.Diagnostics.Debug.WriteLine($"Error loading zodiac data: {ex.Message}");
            throw; // Re-throw to be handled by caller
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSelectedIndexChanged(int value)
    {
        if (value >= 0 && value < ZodiacAnimals.Count)
        {
            SelectedAnimal = ZodiacAnimals[value];
        }
    }

    [RelayCommand]
    private void SelectAnimal(ZodiacInfoDisplayItem item)
    {
        SelectedAnimal = item;
        var idx = ZodiacAnimals.IndexOf(item);
        if (idx >= 0) SelectedIndex = idx;
    }

    [RelayCommand]
    private async Task NavigatePrevious()
    {
        if (SelectedIndex > 0)
        {
            SelectedIndex--;
        }
    }

    [RelayCommand]
    private async Task NavigateNext()
    {
        if (SelectedIndex < ZodiacAnimals.Count - 1)
        {
            SelectedIndex++;
        }
    }
}

/// <summary>
/// Display wrapper for ZodiacInfo with localized and formatted properties
/// </summary>
public class ZodiacInfoDisplayItem : ObservableObject
{
    private readonly ZodiacInfo _info;
    private readonly ILocalizationService _localizationService;

    public ZodiacInfoDisplayItem(ZodiacInfo info, ILocalizationService localizationService)
    {
        _info = info;
        _localizationService = localizationService;
    }

    public ZodiacAnimal Animal => _info.Animal;
    public string Emoji => ZodiacEmojiProvider.GetEmoji(_info.Animal);
    
    public string Name => _localizationService.CurrentLanguage == "vi" 
        ? _info.NameVi 
        : _info.NameEn;
    
    public string ChineseName => _info.ChineseName ?? string.Empty;
    
    public string Traits => _localizationService.CurrentLanguage == "vi"
        ? _info.TraitsVi
        : _info.TraitsEn;
    
    public string Personality => _localizationService.CurrentLanguage == "vi"
        ? _info.PersonalityVi
        : _info.PersonalityEn;
    
    public string Significance => _localizationService.CurrentLanguage == "vi"
        ? _info.SignificanceVi
        : _info.SignificanceEn;

    public string LuckyNumbersFormatted => string.Join(", ", _info.LuckyNumbers ?? Array.Empty<int>());
    public string LuckyColorsFormatted => string.Join(", ", _info.LuckyColors ?? Array.Empty<string>());
    public string LuckyDirectionsFormatted => string.Join(", ", _info.LuckyDirections ?? Array.Empty<string>());
    
    public string BestCompatibilityFormatted => string.Join(", ", _info.BestCompatibility ?? Array.Empty<string>());
    public string ChallengeCompatibilityFormatted => string.Join(", ", _info.ChallengeCompatibility ?? Array.Empty<string>());
    
    public string RecentYearsFormatted => string.Join(", ", _info.RecentYears ?? Array.Empty<int>());
}
