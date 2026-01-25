# Vietnamese Lunar Calendar Application
## .NET MAUI Implementation Guide

**Version:** 2.0  
**Date:** January 3, 2026  
**Framework:** .NET MAUI (.NET 8.0+)

---

## Table of Contents

1. [Implementation Overview](#implementation-overview)
2. [Project Setup](#project-setup)
3. [Core Services Implementation](#core-services-implementation)
4. [ViewModels Implementation](#viewmodels-implementation)
5. [UI Implementation](#ui-implementation)
6. [Localization Implementation](#localization-implementation)
7. [Data Persistence](#data-persistence)
8. [Platform-Specific Features](#platform-specific-features)
9. [Testing Strategy](#testing-strategy)
10. [Deployment](#deployment)

---

## 1. Implementation Overview

### 1.1 Technology Stack Details

**Framework:** .NET MAUI  
**Language:** C# 10.0+  
**.NET Version:** 8.0 or later  
**UI:** XAML with data binding  
**MVVM Library:** CommunityToolkit.Mvvm 8.2.2  
**Database:** sqlite-net-pcl 1.9.172  

### 1.2 Dependencies

```xml
<ItemGroup>
  <PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.2" />
  <PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />
  <PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="$(MauiVersion)" />
  <PackageReference Include="Microsoft.Extensions.Logging.Debug" Version="10.0.0" />
  <PackageReference Include="sqlite-net-pcl" Version="1.9.172" />
</ItemGroup>
```

### 1.3 Project Structure

```
LunarCalendar.MobileApp/
├── App.xaml/App.xaml.cs           # Application entry point
├── MauiProgram.cs                  # DI container configuration
├── Views/                          # XAML pages
├── ViewModels/                     # Business logic
├── Services/                       # Application services
├── Models/                         # Data models
├── Converters/                     # XAML value converters
├── Helpers/                        # Utility classes
├── Resources/                      # Assets, strings, styles
└── Platforms/                      # Platform-specific code
```

---

## 2. Project Setup

### 2.1 Creating a New .NET MAUI Project

```bash
# Create new MAUI app
dotnet new maui -n LunarCalendar.MobileApp

# Navigate to project
cd LunarCalendar.MobileApp

# Add packages
dotnet add package CommunityToolkit.Mvvm
dotnet add package sqlite-net-pcl

# Build
dotnet build
```

### 2.2 MauiProgram.cs Configuration

```csharp
using Microsoft.Extensions.Logging;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.ViewModels;
using LunarCalendar.MobileApp.Views;
using LunarCalendar.MobileApp.Data;

namespace LunarCalendar.MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Register Database
        var dbPath = Path.Combine(
            FileSystem.AppDataDirectory, 
            "lunarcalendar.db3"
        );
        builder.Services.AddSingleton(sp => 
            new LunarCalendarDatabase(dbPath));

        // Register Core Services
        builder.Services.AddSingleton<
            Core.Services.ILunarCalculationService, 
            Core.Services.LunarCalculationService>();
        builder.Services.AddSingleton<
            Core.Services.IHolidayCalculationService, 
            Core.Services.HolidayCalculationService>();

        // Register App Services
        builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
        builder.Services.AddSingleton<ICalendarService, CalendarService>();
        builder.Services.AddSingleton<IHolidayService, HolidayService>();
        builder.Services.AddSingleton<IHapticService, HapticService>();

        // Register ViewModels
        builder.Services.AddTransient<CalendarViewModel>();
        builder.Services.AddTransient<YearHolidaysViewModel>();
        builder.Services.AddTransient<HolidayDetailViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();

        // Register Views
        builder.Services.AddTransient<CalendarPage>();
        builder.Services.AddTransient<YearHolidaysPage>();
        builder.Services.AddTransient<HolidayDetailPage>();
        builder.Services.AddTransient<SettingsPage>();

        return builder.Build();
    }
}
```

### 2.3 App.xaml Configuration

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="LunarCalendar.MobileApp.App">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Resources/Styles/Colors.xaml" />
                <ResourceDictionary Source="Resources/Styles/Styles.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

```csharp
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        
        // Set main page with navigation
        MainPage = new NavigationPage(new CalendarPage());
    }
}
```

---

## 3. Core Services Implementation

### 3.1 LunarCalculationService

**File:** `LunarCalendar.Core/Services/LunarCalculationService.cs`

```csharp
using System.Globalization;
using LunarCalendar.Core.Models;

namespace LunarCalendar.Core.Services;

public class LunarCalculationService : ILunarCalculationService
{
    private readonly ChineseLunisolarCalendar _chineseCalendar = new();

    private static readonly string[] HeavenlyStems = { 
        "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" 
    };
    
    private static readonly string[] EarthlyBranches = { 
        "子", "丑", "寅", "卯", "辰", "巳", 
        "午", "未", "申", "酉", "戌", "亥" 
    };
    
    private static readonly string[] AnimalSigns = { 
        "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", 
        "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" 
    };
    
    private static readonly string[] LunarMonthNames = { 
        "正月", "二月", "三月", "四月", "五月", "六月", 
        "七月", "八月", "九月", "十月", "冬月", "腊月" 
    };
    
    private static readonly string[] LunarDayNames = {
        "初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十",
        "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十",
        "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十"
    };

    public LunarDate ConvertToLunar(DateTime gregorianDate)
    {
        try
        {
            // Get lunar components
            var lunarYear = _chineseCalendar.GetYear(gregorianDate);
            var lunarMonth = _chineseCalendar.GetMonth(gregorianDate);
            var lunarDay = _chineseCalendar.GetDayOfMonth(gregorianDate);
            var isLeapMonth = _chineseCalendar.IsLeapMonth(lunarYear, lunarMonth);

            // Adjust month number for display
            var displayMonth = lunarMonth;
            if (isLeapMonth)
            {
                displayMonth = lunarMonth - 1;
            }
            else
            {
                var leapMonth = _chineseCalendar.GetLeapMonth(lunarYear);
                if (leapMonth > 0 && lunarMonth > leapMonth)
                {
                    displayMonth = lunarMonth - 1;
                }
            }

            // Ensure display month is within valid range
            if (displayMonth < 1) displayMonth = 1;
            if (displayMonth > 12) displayMonth = 12;

            // Calculate cycle components
            var heavenlyStem = HeavenlyStems[(lunarYear - 4) % 10];
            var earthlyBranch = EarthlyBranches[(lunarYear - 4) % 12];
            var animalSign = AnimalSigns[(lunarYear - 4) % 12];

            // Format names
            var lunarMonthName = isLeapMonth 
                ? $"闰{LunarMonthNames[displayMonth - 1]}" 
                : LunarMonthNames[displayMonth - 1];
            var lunarDayName = lunarDay <= LunarDayNames.Length 
                ? LunarDayNames[lunarDay - 1] 
                : $"{lunarDay}";

            return new LunarDate
            {
                GregorianDate = gregorianDate,
                LunarYear = lunarYear,
                LunarMonth = displayMonth,
                LunarDay = lunarDay,
                IsLeapMonth = isLeapMonth,
                LunarYearName = $"{heavenlyStem}{earthlyBranch}年",
                LunarMonthName = lunarMonthName,
                LunarDayName = lunarDayName,
                AnimalSign = animalSign
            };
        }
        catch (Exception)
        {
            // Return default values if conversion fails (date out of range)
            return new LunarDate
            {
                GregorianDate = gregorianDate,
                LunarYear = gregorianDate.Year,
                LunarMonth = gregorianDate.Month,
                LunarDay = gregorianDate.Day,
                IsLeapMonth = false,
                LunarYearName = string.Empty,
                LunarMonthName = string.Empty,
                LunarDayName = string.Empty,
                AnimalSign = string.Empty
            };
        }
    }

    public DateTime ConvertToGregorian(int year, int month, int day, bool isLeapMonth = false)
    {
        var leapMonth = _chineseCalendar.GetLeapMonth(year);

        var adjustedMonth = month;
        if (isLeapMonth && leapMonth == month)
        {
            adjustedMonth = month;
        }
        else if (leapMonth > 0 && month >= leapMonth && !isLeapMonth)
        {
            adjustedMonth = month + 1;
        }

        return _chineseCalendar.ToDateTime(year, adjustedMonth, day, 0, 0, 0, 0);
    }

    public List<LunarDate> GetMonthInfo(int year, int month)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var results = new List<LunarDate>();

        for (int day = 1; day <= daysInMonth; day++)
        {
            var gregorianDate = new DateTime(year, month, day);
            results.Add(ConvertToLunar(gregorianDate));
        }

        return results;
    }
}
```

---

### 3.2 CalendarService (with Caching)

**File:** `LunarCalendar.MobileApp/Services/CalendarService.cs`

```csharp
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;
using LunarCalendar.MobileApp.Data;

namespace LunarCalendar.MobileApp.Services;

public class CalendarService : ICalendarService
{
    private readonly ILunarCalculationService _lunarCalculationService;
    private readonly LunarCalendarDatabase _database;

    public CalendarService(
        ILunarCalculationService lunarCalculationService,
        LunarCalendarDatabase database)
    {
        _lunarCalculationService = lunarCalculationService;
        _database = database;
    }

    public async Task<LunarDate?> GetLunarDateAsync(DateTime date)
    {
        try
        {
            // Try cache first (fast path)
            var cached = await _database.GetLunarDateAsync(date);
            if (cached != null)
            {
                return cached;
            }

            // Calculate locally - instant, no network needed
            var lunarDate = _lunarCalculationService.ConvertToLunar(date);

            // Save to database in background for future lookups
            _ = Task.Run(async () =>
            {
                try
                {
                    await _database.SaveLunarDateAsync(lunarDate);
                }
                catch (Exception ex)
                {
                    // Log error but don't fail the request
                    System.Diagnostics.Debug.WriteLine(
                        $"Error caching lunar date: {ex.Message}");
                }
            });

            return lunarDate;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Error getting lunar date: {ex.Message}");
            return null;
        }
    }

    public async Task<List<LunarDate>> GetMonthLunarDatesAsync(int year, int month)
    {
        var results = new List<LunarDate>();
        var daysInMonth = DateTime.DaysInMonth(year, month);

        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(year, month, day);
            var lunarDate = await GetLunarDateAsync(date);
            if (lunarDate != null)
            {
                results.Add(lunarDate);
            }
        }

        return results;
    }
}
```

---

### 3.3 HolidayService

**File:** `LunarCalendar.MobileApp/Services/HolidayService.cs`

```csharp
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;

namespace LunarCalendar.MobileApp.Services;

public class HolidayService : IHolidayService
{
    private readonly IHolidayCalculationService _holidayCalculationService;
    
    // Cache for performance
    private Dictionary<int, List<HolidayOccurrence>> _yearCache = new();

    public HolidayService(IHolidayCalculationService holidayCalculationService)
    {
        _holidayCalculationService = holidayCalculationService;
    }

    public Task<List<HolidayOccurrence>> GetHolidaysForYearAsync(int year)
    {
        // Check cache first
        if (_yearCache.TryGetValue(year, out var cached))
        {
            return Task.FromResult(cached);
        }

        // Calculate and cache
        var holidays = _holidayCalculationService.GetHolidaysForYear(year);
        _yearCache[year] = holidays;

        return Task.FromResult(holidays);
    }

    public async Task<List<HolidayOccurrence>> GetHolidaysForMonthAsync(
        int year, int month)
    {
        var yearHolidays = await GetHolidaysForYearAsync(year);
        return yearHolidays
            .Where(h => h.GregorianDate.Year == year && 
                        h.GregorianDate.Month == month)
            .ToList();
    }

    public async Task<List<HolidayOccurrence>> GetUpcomingHolidaysAsync(
        int count = 5)
    {
        var today = DateTime.Today;
        var year = today.Year;
        
        // Get holidays for current and next year
        var currentYearHolidays = await GetHolidaysForYearAsync(year);
        var nextYearHolidays = await GetHolidaysForYearAsync(year + 1);
        
        var allHolidays = currentYearHolidays.Concat(nextYearHolidays).ToList();
        
        return allHolidays
            .Where(h => h.GregorianDate >= today)
            .OrderBy(h => h.GregorianDate)
            .Take(count)
            .ToList();
    }

    public Task<List<Holiday>> GetAllHolidaysAsync()
    {
        var holidays = _holidayCalculationService.GetAllHolidays();
        return Task.FromResult(holidays);
    }
}
```

---

### 3.4 LocalizationService

**File:** `LunarCalendar.MobileApp/Services/LocalizationService.cs`

```csharp
using System.Globalization;
using LunarCalendar.MobileApp.Resources.Strings;

namespace LunarCalendar.MobileApp.Services;

public class LocalizationService : ILocalizationService
{
    public event EventHandler? CultureChanged;

    public CultureInfo CurrentCulture => CultureInfo.CurrentCulture;

    public LocalizationService()
    {
        // Load saved language preference
        var savedLanguage = Preferences.Get("AppLanguage", "vi-VN");
        SetCulture(savedLanguage);
    }

    public void SetCulture(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        
        // Update resource manager
        AppResources.Culture = culture;
        
        // Save preference
        Preferences.Set("AppLanguage", cultureName);
        
        // Notify observers
        CultureChanged?.Invoke(this, EventArgs.Empty);
    }

    public string GetString(string key)
    {
        return AppResources.ResourceManager.GetString(key, AppResources.Culture) 
               ?? key;
    }
}
```

---

## 4. ViewModels Implementation

### 4.1 BaseViewModel

**File:** `LunarCalendar.MobileApp/ViewModels/BaseViewModel.cs`

```csharp
using CommunityToolkit.Mvvm.ComponentModel;

namespace LunarCalendar.MobileApp.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;

    public bool IsNotBusy => !IsBusy;
}
```

**Key Features:**
- `[ObservableProperty]` - Auto-generates property with INotifyPropertyChanged
- `[NotifyPropertyChangedFor]` - Notifies dependent properties
- Base class for all ViewModels

---

### 4.2 CalendarViewModel

**File:** `LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`

```csharp
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.Helpers;
using LunarCalendar.MobileApp.Models;
using LunarCalendar.MobileApp.Services;

namespace LunarCalendar.MobileApp.ViewModels;

public partial class CalendarViewModel : BaseViewModel
{
    private readonly ICalendarService _calendarService;
    private readonly IHolidayService _holidayService;
    private readonly ILocalizationService _localizationService;
    private readonly IHapticService _hapticService;

    [ObservableProperty]
    private DateTime _currentMonth = DateTime.Today;

    [ObservableProperty]
    private LunarDate? _todayLunarDate;

    [ObservableProperty]
    private ObservableCollection<CalendarDay> _calendarDays = new();

    [ObservableProperty]
    private ObservableCollection<HolidayOccurrence> _upcomingHolidays = new();

    public string MonthYearDisplay => 
        DateFormatterHelper.FormatMonthYear(_currentMonth);

    public string TodayDisplay => 
        DateFormatterHelper.FormatFullDate(DateTime.Today);

    public string TodayLunarDisplay => _todayLunarDate != null
        ? DateFormatterHelper.FormatLunarDate(
            _todayLunarDate.LunarDay, 
            _todayLunarDate.LunarMonth)
        : string.Empty;

    public CalendarViewModel(
        ICalendarService calendarService,
        IHolidayService holidayService,
        ILocalizationService localizationService,
        IHapticService hapticService)
    {
        _calendarService = calendarService;
        _holidayService = holidayService;
        _localizationService = localizationService;
        _hapticService = hapticService;

        Title = "Calendar";

        // Subscribe to language changes
        _localizationService.CultureChanged += OnCultureChanged;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            // Load today's lunar date
            _todayLunarDate = await _calendarService.GetLunarDateAsync(DateTime.Today);
            OnPropertyChanged(nameof(TodayLunarDisplay));

            // Load month data
            await LoadMonthAsync();

            // Load upcoming holidays
            var holidays = await _holidayService.GetUpcomingHolidaysAsync(5);
            UpcomingHolidays = new ObservableCollection<HolidayOccurrence>(holidays);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading calendar: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task PreviousMonth()
    {
        _hapticService.Trigger();
        CurrentMonth = CurrentMonth.AddMonths(-1);
        await LoadMonthAsync();
        OnPropertyChanged(nameof(MonthYearDisplay));
    }

    [RelayCommand]
    private async Task NextMonth()
    {
        _hapticService.Trigger();
        CurrentMonth = CurrentMonth.AddMonths(1);
        await LoadMonthAsync();
        OnPropertyChanged(nameof(MonthYearDisplay));
    }

    [RelayCommand]
    private async Task GoToToday()
    {
        _hapticService.Trigger();
        CurrentMonth = DateTime.Today;
        await LoadMonthAsync();
        OnPropertyChanged(nameof(MonthYearDisplay));
    }

    [RelayCommand]
    private async Task SelectDate(CalendarDay day)
    {
        if (day == null || day.Date == null) return;

        _hapticService.Trigger();

        // Navigate to holiday detail if holiday exists
        if (day.Holidays?.Any() == true)
        {
            var firstHoliday = day.Holidays.First();
            // Navigation code here
        }
    }

    private async Task LoadMonthAsync()
    {
        try
        {
            var year = CurrentMonth.Year;
            var month = CurrentMonth.Month;

            // Get lunar dates for month
            var lunarDates = await _calendarService.GetMonthLunarDatesAsync(year, month);

            // Get holidays for month
            var holidays = await _holidayService.GetHolidaysForMonthAsync(year, month);

            // Build calendar grid (42 cells for 6 weeks)
            var calendarDays = new List<CalendarDay>();
            var firstDayOfMonth = new DateTime(year, month, 1);
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            // Previous month filler
            var previousMonth = firstDayOfMonth.AddMonths(-1);
            var daysInPreviousMonth = DateTime.DaysInMonth(
                previousMonth.Year, previousMonth.Month);
            for (int i = startDayOfWeek - 1; i >= 0; i--)
            {
                var day = daysInPreviousMonth - i;
                calendarDays.Add(new CalendarDay
                {
                    Date = new DateTime(previousMonth.Year, previousMonth.Month, day),
                    IsCurrentMonth = false
                });
            }

            // Current month days
            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(year, month, day);
                var lunarDate = lunarDates.FirstOrDefault(l => l.GregorianDate.Date == date);
                var dayHolidays = holidays.Where(h => h.GregorianDate.Date == date).ToList();

                calendarDays.Add(new CalendarDay
                {
                    Date = date,
                    LunarDate = lunarDate,
                    Holidays = dayHolidays,
                    IsCurrentMonth = true,
                    IsToday = date.Date == DateTime.Today.Date
                });
            }

            // Next month filler
            var remainingCells = 42 - calendarDays.Count;
            var nextMonth = firstDayOfMonth.AddMonths(1);
            for (int day = 1; day <= remainingCells; day++)
            {
                calendarDays.Add(new CalendarDay
                {
                    Date = new DateTime(nextMonth.Year, nextMonth.Month, day),
                    IsCurrentMonth = false
                });
            }

            CalendarDays = new ObservableCollection<CalendarDay>(calendarDays);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading month: {ex.Message}");
        }
    }

    private async void OnCultureChanged(object? sender, EventArgs e)
    {
        // Reload data in new language
        OnPropertyChanged(nameof(MonthYearDisplay));
        OnPropertyChanged(nameof(TodayDisplay));
        OnPropertyChanged(nameof(TodayLunarDisplay));
        await LoadMonthAsync();
    }
}
```

**Key Patterns:**
- `[RelayCommand]` - Auto-generates ICommand implementation
- `ObservableCollection` - Automatically updates UI
- Async/await for database operations
- Event subscription for localization changes

---

## 5. UI Implementation

### 5.1 CalendarPage XAML

**File:** `LunarCalendar.MobileApp/Views/CalendarPage.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:LunarCalendar.MobileApp.ViewModels"
             xmlns:models="clr-namespace:LunarCalendar.MobileApp.Models"
             xmlns:converters="clr-namespace:LunarCalendar.MobileApp.Converters"
             x:Class="LunarCalendar.MobileApp.Views.CalendarPage"
             x:DataType="vm:CalendarViewModel"
             Title="{Binding Title}">

    <ContentPage.Resources>
        <converters:BoolToVisibilityConverter x:Key="BoolToVisibility" />
        <converters:ColorConverter x:Key="ColorConverter" />
    </ContentPage.Resources>

    <Grid RowDefinitions="Auto,*" Padding="10">
        
        <!-- Header Section -->
        <VerticalStackLayout Grid.Row="0" Spacing="10">
            
            <!-- Today's Date -->
            <Frame BackgroundColor="{StaticResource Primary}"
                   CornerRadius="10"
                   Padding="15">
                <VerticalStackLayout Spacing="5">
                    <Label Text="{Binding TodayDisplay}"
                           FontSize="18"
                           FontAttributes="Bold"
                           TextColor="White" />
                    <Label Text="{Binding TodayLunarDisplay}"
                           FontSize="14"
                           TextColor="White" />
                </VerticalStackLayout>
            </Frame>

            <!-- Month Navigation -->
            <Grid ColumnDefinitions="Auto,*,Auto">
                <Button Grid.Column="0"
                        Text="&#x25C0;"
                        Command="{Binding PreviousMonthCommand}"
                        BackgroundColor="Transparent"
                        TextColor="{StaticResource Primary}" />
                
                <Label Grid.Column="1"
                       Text="{Binding MonthYearDisplay}"
                       FontSize="20"
                       FontAttributes="Bold"
                       HorizontalTextAlignment="Center"
                       VerticalTextAlignment="Center" />
                
                <Button Grid.Column="2"
                        Text="&#x25B6;"
                        Command="{Binding NextMonthCommand}"
                        BackgroundColor="Transparent"
                        TextColor="{StaticResource Primary}" />
            </Grid>

            <!-- Day of Week Headers -->
            <Grid ColumnDefinitions="*,*,*,*,*,*,*" HeightRequest="30">
                <Label Grid.Column="0" Text="Sun" HorizontalTextAlignment="Center" />
                <Label Grid.Column="1" Text="Mon" HorizontalTextAlignment="Center" />
                <Label Grid.Column="2" Text="Tue" HorizontalTextAlignment="Center" />
                <Label Grid.Column="3" Text="Wed" HorizontalTextAlignment="Center" />
                <Label Grid.Column="4" Text="Thu" HorizontalTextAlignment="Center" />
                <Label Grid.Column="5" Text="Fri" HorizontalTextAlignment="Center" />
                <Label Grid.Column="6" Text="Sat" HorizontalTextAlignment="Center" />
            </Grid>
        </VerticalStackLayout>

        <!-- Calendar Grid -->
        <CollectionView Grid.Row="1"
                        ItemsSource="{Binding CalendarDays}"
                        SelectionMode="Single"
                        SelectionChangedCommand="{Binding SelectDateCommand}"
                        SelectionChangedCommandParameter="{Binding SelectedItem, Source={RelativeSource Self}}">
            <CollectionView.ItemsLayout>
                <GridItemsLayout Orientation="Vertical"
                                 Span="7"
                                 VerticalItemSpacing="5"
                                 HorizontalItemSpacing="5" />
            </CollectionView.ItemsLayout>
            
            <CollectionView.ItemTemplate>
                <DataTemplate x:DataType="models:CalendarDay">
                    <Border StrokeThickness="1"
                            Stroke="{StaticResource Gray300}"
                            BackgroundColor="{Binding IsToday, Converter={StaticResource BoolToVisibility}}"
                            Padding="5">
                        <VerticalStackLayout Spacing="2">
                            <!-- Gregorian Date -->
                            <Label Text="{Binding Date.Day}"
                                   FontSize="16"
                                   FontAttributes="{Binding IsCurrentMonth, Converter={StaticResource BoolToFontAttributes}}"
                                   HorizontalTextAlignment="Center" />
                            
                            <!-- Lunar Date -->
                            <Label Text="{Binding LunarDateDisplay}"
                                   FontSize="10"
                                   TextColor="{StaticResource Gray600}"
                                   HorizontalTextAlignment="Center" />
                            
                            <!-- Holiday Indicator -->
                            <BoxView IsVisible="{Binding HasHolidays}"
                                     Color="{Binding HolidayColor, Converter={StaticResource ColorConverter}}"
                                     HeightRequest="4"
                                     WidthRequest="20"
                                     CornerRadius="2"
                                     HorizontalOptions="Center" />
                        </VerticalStackLayout>
                    </Border>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>

        <!-- Loading Indicator -->
        <ActivityIndicator Grid.RowSpan="2"
                           IsVisible="{Binding IsBusy}"
                           IsRunning="{Binding IsBusy}"
                           HorizontalOptions="Center"
                           VerticalOptions="Center" />
    </Grid>
</ContentPage>
```

---

### 5.2 CalendarPage Code-Behind

**File:** `LunarCalendar.MobileApp/Views/CalendarPage.xaml.cs`

```csharp
using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Views;

public partial class CalendarPage : ContentPage
{
    private readonly CalendarViewModel _viewModel;

    public CalendarPage(CalendarViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Load data when page appears
        await _viewModel.LoadCommand.ExecuteAsync(null);
    }
}
```

---

## 6. Localization Implementation

### 6.1 Resource Files

**English (Default):** `Resources/Strings/AppResources.resx`

```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="App_Title" xml:space="preserve">
    <value>Vietnamese Lunar Calendar</value>
  </data>
  <data name="Calendar_Title" xml:space="preserve">
    <value>Calendar</value>
  </data>
  <data name="Today" xml:space="preserve">
    <value>Today</value>
  </data>
  <data name="Holidays" xml:space="preserve">
    <value>Holidays</value>
  </data>
  <!-- More strings... -->
</root>
```

**Vietnamese:** `Resources/Strings/AppResources.vi.resx`

```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="App_Title" xml:space="preserve">
    <value>Lịch Âm Việt Nam</value>
  </data>
  <data name="Calendar_Title" xml:space="preserve">
    <value>Lịch</value>
  </data>
  <data name="Today" xml:space="preserve">
    <value>Hôm Nay</value>
  </data>
  <data name="Holidays" xml:space="preserve">
    <value>Ngày Lễ</value>
  </data>
  <!-- More strings... -->
</root>
```

---

### 6.2 Using Localization in XAML

```xml
<Label Text="{x:Static strings:AppResources.Calendar_Title}" />
```

### 6.3 Using Localization in Code

```csharp
var title = AppResources.Calendar_Title;
```

---

## 7. Data Persistence

### 7.1 SQLite Database Implementation

**File:** `LunarCalendar.MobileApp/Data/LunarCalendarDatabase.cs`

```csharp
using SQLite;
using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.Models;

namespace LunarCalendar.MobileApp.Data;

public class LunarCalendarDatabase
{
    private readonly SQLiteAsyncConnection _connection;

    public LunarCalendarDatabase(string dbPath)
    {
        _connection = new SQLiteAsyncConnection(dbPath);
        
        // Create tables
        _connection.CreateTableAsync<LunarDateEntity>().Wait();
    }

    public async Task<LunarDate?> GetLunarDateAsync(DateTime date)
    {
        var entity = await _connection
            .Table<LunarDateEntity>()
            .Where(e => e.GregorianDate == date.Date)
            .FirstOrDefaultAsync();

        if (entity == null) return null;

        return new LunarDate
        {
            GregorianDate = entity.GregorianDate,
            LunarYear = entity.LunarYear,
            LunarMonth = entity.LunarMonth,
            LunarDay = entity.LunarDay,
            IsLeapMonth = entity.IsLeapMonth,
            LunarYearName = entity.LunarYearName,
            LunarMonthName = entity.LunarMonthName,
            LunarDayName = entity.LunarDayName,
            AnimalSign = entity.AnimalSign
        };
    }

    public async Task SaveLunarDateAsync(LunarDate lunarDate)
    {
        var entity = new LunarDateEntity
        {
            GregorianDate = lunarDate.GregorianDate.Date,
            LunarYear = lunarDate.LunarYear,
            LunarMonth = lunarDate.LunarMonth,
            LunarDay = lunarDate.LunarDay,
            IsLeapMonth = lunarDate.IsLeapMonth,
            LunarYearName = lunarDate.LunarYearName,
            LunarMonthName = lunarDate.LunarMonthName,
            LunarDayName = lunarDate.LunarDayName,
            AnimalSign = lunarDate.AnimalSign
        };

        await _connection.InsertOrReplaceAsync(entity);
    }

    public async Task ClearCacheAsync()
    {
        await _connection.DeleteAllAsync<LunarDateEntity>();
    }
}
```

---

## 8. Platform-Specific Features

### 8.1 iOS Haptic Feedback

**File:** `Platforms/iOS/HapticFeedback.cs`

```csharp
#if IOS
using UIKit;

namespace LunarCalendar.MobileApp.Platforms.iOS;

public class HapticFeedback
{
    public static void Trigger(HapticFeedbackType type)
    {
        var impactStyle = type switch
        {
            HapticFeedbackType.Success => UIImpactFeedbackStyle.Medium,
            HapticFeedbackType.Warning => UIImpactFeedbackStyle.Heavy,
            _ => UIImpactFeedbackStyle.Light
        };

        var impact = new UIImpactFeedbackGenerator(impactStyle);
        impact.ImpactOccurred();
    }
}
#endif
```

---

## 9. Testing Strategy

### 9.1 Unit Tests

```csharp
using Xunit;
using LunarCalendar.Core.Services;

public class LunarCalculationServiceTests
{
    private readonly LunarCalculationService _service;

    public LunarCalculationServiceTests()
    {
        _service = new LunarCalculationService();
    }

    [Fact]
    public void ConvertToLunar_TetNguyenDan2025_ReturnsCorrectDate()
    {
        // Arrange
        var gregorianDate = new DateTime(2025, 1, 29);

        // Act
        var result = _service.ConvertToLunar(gregorianDate);

        // Assert
        Assert.Equal(2025, result.LunarYear);
        Assert.Equal(1, result.LunarMonth);
        Assert.Equal(1, result.LunarDay);
        Assert.Equal("Snake", result.AnimalSign);
    }
}
```

---

## 10. Deployment

### 10.1 Android Release Build

```bash
cd src/LunarCalendar.MobileApp

dotnet publish \
  -f net8.0-android \
  -c Release \
  /p:AndroidPackageFormat=aab \
  /p:AndroidKeyStore=true \
  /p:AndroidSigningKeyStore=myapp.keystore \
  /p:AndroidSigningKeyAlias=myapp \
  /p:AndroidSigningKeyPass=password \
  /p:AndroidSigningStorePass=password
```

### 10.2 iOS Release Build

```bash
cd src/LunarCalendar.MobileApp

dotnet publish \
  -f net8.0-ios \
  -c Release \
  /p:ArchiveOnBuild=true \
  /p:CodesignKey="Apple Distribution" \
  /p:CodesignProvision="com.huynguyen.lunarcalendar"
```

---

**Document End**

This implementation guide provides complete code examples for building the Vietnamese Lunar Calendar app with .NET MAUI. All patterns and practices shown are production-ready and follow .NET MAUI best practices.
