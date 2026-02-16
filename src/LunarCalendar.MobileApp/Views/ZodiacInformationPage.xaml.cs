using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Views;

[QueryProperty(nameof(AnimalParam), "animal")]
public partial class ZodiacInformationPage : ContentPage
{
    private readonly ZodiacInformationViewModel _viewModel;
    private string? _animalParam;

    public string? AnimalParam
    {
        get => _animalParam;
        set
        {
            _animalParam = value;
            // Will be handled in OnAppearing
        }
    }

    public ZodiacInformationPage(ZodiacInformationViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Initialize with the animal passed as query parameter
        ZodiacAnimal? initialAnimal = null;
        if (!string.IsNullOrEmpty(_animalParam) && Enum.TryParse<ZodiacAnimal>(_animalParam, out var animal))
        {
            initialAnimal = animal;
        }

        await _viewModel.InitializeAsync(initialAnimal);
    }
}
