using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LunarCalendar.MobileApp.Resources.Strings;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.Core.Services;
using LunarCalendar.Core.Models;
using System.Globalization;

namespace LunarCalendar.MobileApp.ViewModels;

public partial class ZodiacProfileViewModel : BaseViewModel
{
    private const string BirthYearKey = "ZodiacProfileBirthYear";

    private readonly IZodiacService _zodiacService;

    [ObservableProperty]
    private string _birthYearText = string.Empty;

    [ObservableProperty]
    private string _myZodiacDisplay = string.Empty;

    [ObservableProperty]
    private bool _hasZodiacProfile;

    public ZodiacProfileViewModel(IZodiacService zodiacService)
    {
        _zodiacService = zodiacService;
        Title = AppResources.ZodiacTab;
        LoadZodiacProfile();

        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
        {
            Title = AppResources.ZodiacTab;
            LoadZodiacProfile();
        });
    }

    [RelayCommand]
    private async Task SaveZodiacProfileAsync()
    {
        if (!int.TryParse(BirthYearText, out var year) || year < 1900 || year > DateTime.Now.Year)
        {
            var msg = AppResources.ResourceManager.GetString("InvalidBirthYear", CultureInfo.CurrentUICulture)
                      ?? "Please enter a valid birth year between 1900 and {0}.";
            await Application.Current!.MainPage!.DisplayAlert(
                AppResources.ErrorTitle,
                string.Format(msg, DateTime.Now.Year),
                AppResources.OK);
            return;
        }
        Preferences.Set(BirthYearKey, year);
        UpdateZodiacDisplay(year);
    }

    [RelayCommand]
    private async Task NavigateToZodiacInfoAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("zodiacinfo");
        }
        catch (Exception ex)
        {
            _ = ex;
        }
    }

    [RelayCommand]
    private async Task NavigateToCompatibilityAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("zodiaccompatibility");
        }
        catch (Exception ex)
        {
            _ = ex;
        }
    }

    internal void LoadZodiacProfile()
    {
        var savedYear = Preferences.Get(BirthYearKey, 0);
        if (savedYear > 0)
        {
            BirthYearText = savedYear.ToString();
            UpdateZodiacDisplay(savedYear);
        }
        else
        {
            MyZodiacDisplay = AppResources.ResourceManager.GetString("NotSet", CultureInfo.CurrentUICulture) ?? "Not set";
            HasZodiacProfile = false;
        }
    }

    private void UpdateZodiacDisplay(int birthYear)
    {
        try
        {
            var elemental = _zodiacService.GetElementalAnimalForLunarYear(birthYear);
            var elementName = AppResources.ResourceManager.GetString($"FiveElement_{elemental.Element}", CultureInfo.CurrentUICulture)
                              ?? elemental.Element.ToString();
            var animalName = GetLocalizedAnimalName(elemental.Animal);
            var emoji = ZodiacEmojiProvider.GetEmoji(elemental.Animal);
            MyZodiacDisplay = $"{emoji} {elementName} {animalName}";
            HasZodiacProfile = true;
        }
        catch
        {
            MyZodiacDisplay = AppResources.ResourceManager.GetString("NotSet", CultureInfo.CurrentUICulture) ?? "Not set";
            HasZodiacProfile = false;
        }
    }

    private string GetLocalizedAnimalName(ZodiacAnimal animal)
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
            _                    => string.Empty
        };
        return string.IsNullOrEmpty(key)
            ? animal.ToString()
            : AppResources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? animal.ToString();
    }
}
