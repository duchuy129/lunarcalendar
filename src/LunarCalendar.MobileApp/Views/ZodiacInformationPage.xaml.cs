using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.ViewModels;
using Microsoft.Maui.Controls;

namespace LunarCalendar.MobileApp.Views;

[QueryProperty(nameof(AnimalParam), "animal")]
public partial class ZodiacInformationPage : ContentPage
{
    private readonly ZodiacInformationViewModel _viewModel;
    private string? _animalParam;
    private bool _isInitialized = false;

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
        _viewModel = viewModel;
        // CRITICAL: DO NOT set BindingContext here - causes iOS crash during navigation
        // Will be set in OnAppearing after navigation animation completes
        
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // CRITICAL: Set BindingContext AFTER navigation completes
        if (BindingContext == null)
        {
            BindingContext = _viewModel;
        }
        
        // CRITICAL: Prevent re-initialization if already loaded
        if (_isInitialized)
            return;
        
        // Initialize with the animal passed as query parameter
        ZodiacAnimal? initialAnimal = null;
        if (!string.IsNullOrEmpty(_animalParam) && Enum.TryParse<ZodiacAnimal>(_animalParam, out var animal))
        {
            initialAnimal = animal;
        }

        try
        {
            await _viewModel.InitializeAsync(initialAnimal);
            _isInitialized = true;
            
            // WORKAROUND: Instead of crashing CarouselView, show simple data view
            // TODO Sprint 10 Phase 3b: Implement alternative to CarouselView
            ShowSimpleZodiacView();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"CRITICAL ERROR loading zodiac data: {ex}");
            // Fallback: show error to user
            await DisplayAlert("Error", "Failed to load zodiac information. Please try again.", "OK");
        }
    }

    private void ShowSimpleZodiacView()
    {
        // Hide loading indicator
        LoadingIndicator.IsVisible = false;
        
        // Show simple text view with zodiac data
        var scrollView = new ScrollView
        {
            Padding = new Thickness(20)
        };

        var stackLayout = new VerticalStackLayout
        {
            Spacing = 20
        };

        // Add content for each zodiac animal
        foreach (var item in _viewModel.ZodiacAnimals)
        {
            var animalCard = new Frame
            {
                BorderColor = Colors.Gray,
                CornerRadius = 10,
                Padding = new Thickness(15),
                Margin = new Thickness(0, 0, 0, 15)
            };

            var cardContent = new VerticalStackLayout
            {
                Spacing = 10
            };

            cardContent.Add(new Label
            {
                Text = $"{item.Emoji} {item.Name}",
                FontSize = 24,
                FontAttributes = FontAttributes.Bold
            });

            cardContent.Add(new Label
            {
                Text = $"Traits: {item.Traits}",
                FontSize = 14
            });

            animalCard.Content = cardContent;
            stackLayout.Add(animalCard);
        }

        scrollView.Content = stackLayout;
        RootContainer.Add(scrollView);
    }
}
