using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Views;

[QueryProperty(nameof(AnimalParam), "animal")]
public partial class ZodiacInformationPage : ContentPage
{
    private readonly ZodiacInformationViewModel _viewModel;
    private string? _animalParam;
    private bool _initialized;

    public string? AnimalParam
    {
        get => _animalParam;
        set => _animalParam = value;
    }

    public ZodiacInformationPage(ZodiacInformationViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_initialized) return;
        _initialized = true;

        ZodiacAnimal? initial = null;
        if (!string.IsNullOrEmpty(_animalParam) && Enum.TryParse<ZodiacAnimal>(_animalParam, out var parsed))
            initial = parsed;

        try
        {
            await _viewModel.InitializeAsync(initial);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ZodiacInformationPage error: {ex.Message}");
            await DisplayAlert("Error", "Failed to load zodiac data.", "OK");
        }
    }
}
