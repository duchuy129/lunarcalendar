# Implementation Plan: Zodiac Animals & Year Characteristics

**Branch**: `feature/002-zodiac-animals` | **Date**: January 26, 2026 | **Spec**: [002-zodiac-animals.md](./002-zodiac-animals.md)

---

## Summary

Implement comprehensive zodiac animal system that displays the current lunar year's zodiac animal, provides rich cultural educational content for all 12 animals, includes a compatibility checker, and shows elemental animal variations. This builds on Sprint 9's Sexagenary Cycle foundation by mapping Earthly Branches to zodiac animals and combining Heavenly Stems with animals to create the 60-year elemental cycle (e.g., Fire Horse, Metal Rat).

**Technical Approach**: Create a new `ZodiacService` in LunarCalendar.Core that leverages existing Sprint 9 enums (`EarthlyBranch`, `ZodiacAnimal`, `FiveElement`) and `SexagenaryService` for calculations. Store comprehensive zodiac data in embedded JSON files (ZodiacData.json, ZodiacCompatibility.json) for offline-first support. Add new UI pages for zodiac browsing and compatibility checking using MAUI MVVM patterns. Integrate zodiac display into existing calendar header and date detail views. Use SVG vector graphics for zodiac animal icons to keep bundle size under 1.5 MB while supporting dark mode and scalability.

---

## Technical Context

**Language/Version**: C# 12 with .NET 10.0  
**Framework**: .NET MAUI (Multi-platform App UI)  
**Primary Dependencies**: 
- Sprint 9: SexagenaryService, EarthlyBranch, ZodiacAnimal, FiveElement enums
- CommunityToolkit.Mvvm 8.2.2 (MVVM helpers)
- System.Text.Json (JSON parsing for zodiac data)
- SkiaSharp 2.88.6 (SVG rendering, already in project)

**Storage**: 
- Embedded JSON files in app bundle (ZodiacData.json ~80 KB, ZodiacCompatibility.json ~15 KB)
- User preferences (birth date) in existing Settings/Preferences system
- No database tables needed (all data is reference data, no user-generated content)

**Testing**: xUnit with FluentAssertions for unit tests, MAUI UI testing for integration  
**Target Platform**: iOS 15.0+, Android API 26+ (cross-platform mobile)  
**Project Type**: Mobile application with shared Core library  

**Performance Goals**: 
- < 10ms zodiac animal calculation (simple modulo operation)
- < 500ms Zodiac Information page load (with cached JSON data)
- 60 FPS scrolling on zodiac animal carousel
- < 1.5 MB total zodiac asset size (12 SVG images + JSON data)

**Constraints**: 
- **Offline-first**: All zodiac functionality must work without network
- **Cultural accuracy**: Content validated by 2+ Vietnamese cultural SMEs
- **Localization**: Vietnamese (12 Con Giáp) and English support (Chinese deferred to Sprint 14)
- **Image size**: Using Unicode emoji for Sprint 10 (zero bundle impact)
- **JSON size**: ZodiacData.json must be < 100 KB for fast parsing

**Scale/Scope**: 
- ~6 new classes in Core library (ZodiacService, ZodiacInfo, etc.)
- ~3 new ViewModels in MobileApp (ZodiacInformationViewModel, ZodiacCompatibilityViewModel, ZodiacProfileViewModel)
- ~3 new XAML pages (ZodiacInformationPage, ZodiacCompatibilityPage, profile views)
- ~2 new UI components (ZodiacHeaderView, ZodiacCardView)
- ~50+ unit tests for zodiac calculations and compatibility

---

## Constitution Check

*GATE: Must pass before implementation. Based on project constitution.*

✅ **I. Offline-First Architecture**: COMPLIANT  
All zodiac calculations and data are embedded in app bundle. No network dependency. JSON files loaded on first access and cached in memory.

✅ **II. Cultural Accuracy & Authenticity**: COMPLIANT  
Zodiac content will be sourced from authoritative Vietnamese/Chinese references. Cultural SME review scheduled before Sprint 10 completion. Rabbit vs Cat handled via locale-specific resource strings.

✅ **III. Privacy & Guest-First Design**: COMPLIANT  
No authentication required. Birth date is optional and stored locally in user preferences. Compatibility checker works without any personal data.

✅ **IV. Cross-Platform Consistency**: COMPLIANT  
All logic in LunarCalendar.Core. UI uses MAUI cross-platform controls. Unicode emoji render consistently on iOS and Android. Optional SVG graphics can be added later.

✅ **V. Performance & Responsiveness**: COMPLIANT  
Zodiac calculation is O(1). JSON parsing done once at startup. Images loaded lazily with caching. Target < 10ms calculation, < 500ms page load.

✅ **VI. Bilingual Support**: COMPLIANT  
All zodiac names, traits, and UI strings use .resx resource files for Vietnamese and English localization. Chinese localization deferred to Sprint 14.

✅ **VII. Test Coverage & Quality Assurance**: COMPLIANT  
Target 90%+ coverage for ZodiacService. Validation against 100 known years (2000-2099). Compatibility matrix tested for consistency.

**Gate Result**: ✅ PASS - All constitutional requirements met. Proceed with implementation.

---

## Project Structure

### Documentation (this feature)

```text
.specify/features/002-zodiac-animals/
├── 002-zodiac-animals.md                # Specification (already exists)
├── 002-zodiac-animals-plan.md           # This file
├── tasks.md                              # Task breakdown (from /speckit.tasks)
├── research/
│   ├── zodiac-content-sources.md        # Vietnamese/Chinese zodiac references
│   ├── compatibility-matrix.md          # Compatibility scoring research
│   └── image-optimization.md            # SVG vs PNG analysis
└── contracts/
    ├── IZodiacService.cs                # Service interface
    ├── ZodiacInfo.cs                    # Data model contracts
    └── ZodiacCompatibility.cs           # Compatibility model
```

### Source Code (repository root)

```text
src/
├── LunarCalendar.Core/                          # Shared business logic
│   ├── Models/
│   │   ├── ZodiacInfo.cs                       # NEW: Comprehensive zodiac data model
│   │   ├── ElementalAnimal.cs                  # NEW: Element + Animal combination
│   │   ├── ZodiacCompatibility.cs              # NEW: Compatibility data model
│   │   ├── CompatibilityRating.cs              # NEW: Great/Good/Fair/Poor enum
│   │   └── UserZodiacProfile.cs                # NEW: User's zodiac profile
│   │   
│   │   # Sprint 9 models (already exist, will be used):
│   │   ├── ZodiacAnimal.cs                     # EXISTS: 12 animals enum
│   │   ├── EarthlyBranch.cs                    # EXISTS: 12 branches enum
│   │   └── FiveElement.cs                      # EXISTS: 5 elements enum
│   │
│   ├── Services/
│   │   ├── ZodiacService.cs                    # NEW: Core zodiac service implementation
│   │   ├── IZodiacService.cs                   # NEW: Service interface
│   │   ├── ZodiacDataRepository.cs             # NEW: Loads/caches JSON data
│   │   ├── ZodiacCompatibilityEngine.cs        # NEW: Compatibility scoring logic
│   │   │
│   │   # Sprint 9 services (already exist, will be integrated):
│   │   ├── SexagenaryService.cs                # EXISTS: Used for element calculation
│   │   └── ISexagenaryService.cs               # EXISTS: Service interface
│   │
│   ├── Data/
│   │   ├── ZodiacData.json                     # NEW: All 12 animals' comprehensive info
│   │   └── ZodiacCompatibility.json            # NEW: 144 compatibility pairings (12x12)
│   │
│   └── Resources/
│       ├── Strings.en.resx                      # MODIFIED: Add zodiac UI strings
│       ├── Strings.vi.resx                      # MODIFIED: Add Vietnamese zodiac names
│       └── Strings.zh.resx                      # MODIFIED: Add Chinese zodiac names
│
├── LunarCalendar.MobileApp/                     # MAUI mobile app
│   ├── ViewModels/
│   │   ├── ZodiacInformationViewModel.cs       # NEW: Main zodiac browser
│   │   ├── ZodiacCompatibilityViewModel.cs     # NEW: Compatibility checker
│   │   ├── ZodiacProfileViewModel.cs           # NEW: User's zodiac profile
│   │   │
│   │   # Existing ViewModels to modify:
│   │   ├── CalendarViewModel.cs                # MODIFIED: Add current year zodiac
│   │   ├── HolidayDetailViewModel.cs           # MODIFIED: Add zodiac info section
│   │   └── SettingsViewModel.cs                # MODIFIED: Add birth date setting
│   │
│   ├── Views/
│   │   ├── ZodiacInformationPage.xaml          # NEW: Full zodiac browser with swipe
│   │   ├── ZodiacInformationPage.xaml.cs
│   │   ├── ZodiacCompatibilityPage.xaml        # NEW: Compatibility checker
│   │   ├── ZodiacCompatibilityPage.xaml.cs
│   │   │
│   │   # Existing Views to modify:
│   │   ├── CalendarPage.xaml                   # MODIFIED: Add zodiac header view
│   │   ├── HolidayDetailPage.xaml              # MODIFIED: Add zodiac card
│   │   └── SettingsPage.xaml                   # MODIFIED: Add Zodiac & Astrology section
│   │
│   ├── Controls/
│   │   ├── ZodiacHeaderView.xaml               # NEW: Calendar header zodiac display
│   │   ├── ZodiacHeaderView.xaml.cs
│   │   ├── ZodiacCardView.xaml                 # NEW: Condensed zodiac info card
│   │   ├── ZodiacCardView.xaml.cs
│   │   ├── ZodiacAnimalPicker.xaml             # NEW: Animal selection control
│   │   └── ZodiacAnimalPicker.xaml.cs
│   │
│   ├── Resources/
│   │   ├── Images/
│   │   │   └── Zodiac/                         # NEW: 12 zodiac animal SVG images
│   │   │       ├── rat.svg                     # ~100-150 KB each, optimized
│   │   │       ├── ox.svg
│   │   │       ├── tiger.svg
│   │   │       ├── rabbit.svg
│   │   │       ├── dragon.svg
│   │   │       ├── snake.svg
│   │   │       ├── horse.svg
│   │   │       ├── goat.svg
│   │   │       ├── monkey.svg
│   │   │       ├── rooster.svg
│   │   │       ├── dog.svg
│   │   │       └── pig.svg
│   │   │
│   │   └── Styles/
│   │       └── ZodiacStyles.xaml               # NEW: Zodiac-specific UI styles
│   │
│   └── Converters/
│       ├── ZodiacAnimalToImageConverter.cs     # NEW: Animal enum → image path
│       └── CompatibilityRatingToColorConverter.cs # NEW: Rating → color (green/yellow/red)
│
└── tests/
    ├── LunarCalendar.Core.Tests/
    │   ├── Services/
    │   │   ├── ZodiacServiceTests.cs           # NEW: Service tests (100 years)
    │   │   ├── ZodiacDataRepositoryTests.cs    # NEW: JSON loading tests
    │   │   └── ZodiacCompatibilityEngineTests.cs # NEW: Compatibility tests
    │   │
    │   └── Models/
    │       ├── ZodiacInfoTests.cs              # NEW: Model validation tests
    │       └── ElementalAnimalTests.cs         # NEW: Element + Animal combo tests
    │
    └── LunarCalendar.MobileApp.Tests/
        └── ViewModels/
            ├── ZodiacInformationViewModelTests.cs  # NEW: ViewModel tests
            └── ZodiacCompatibilityViewModelTests.cs # NEW: ViewModel tests
```

---

## Phase 0: Research & Preparation

### Research Tasks (Completed During Specification Phase)

1. **Zodiac Content Sources** ✅
   - Vietnamese: https://vi.wikipedia.org/wiki/12_con_giáp
   - Chinese: https://zh.wikipedia.org/wiki/生肖
   - Cultural references: Mainstream Vietnamese astrology books

2. **Compatibility Algorithm Research** 
   - Matrix-based approach (144 pre-defined scores)
   - Mainstream Vietnamese/Chinese compatibility sources
   - Document sources in `research/compatibility-matrix.md`

3. **Image Strategy Decision**
   - **Decision**: Use SVG vector graphics
   - **Rationale**: Scalable, dark mode support, small size (~100-150 KB each)
   - **Tool**: SkiaSharp (already in project) for SVG rendering
   - **Fallback**: Unicode emoji (🐭🐮🐯🐰🐲🐍🐴🐑🐵🐔🐶🐷) if SVG fails to load

4. **Sprint 9 Integration Points** ✅
   - `ZodiacAnimal` enum: Already exists, maps 1:1 with Earthly Branches
   - `EarthlyBranch` enum: Already exists, has `GetZodiacAnimal()` method
   - `FiveElement` enum: Already exists, used for elemental animals
   - `SexagenaryService`: Already exists, provides `GetYearInfo()` for element calculation

---

## Phase 1: Core Library Implementation

### 1.1 Data Models

#### ZodiacInfo.cs - Comprehensive zodiac data
```csharp
namespace LunarCalendar.Core.Models;

/// <summary>
/// Comprehensive information about a zodiac animal
/// </summary>
public class ZodiacInfo
{
    public ZodiacAnimal Animal { get; set; }
    
    // Multilingual names
    public string VietnameseName { get; set; } // e.g., "Tý - Chuột"
    public string ChineseName { get; set; }    // e.g., "鼠 - Shǔ"
    public string EnglishName { get; set; }    // e.g., "Rat"
    
    // Personality and characteristics
    public List<string> PersonalityTraits { get; set; } = new();
    public string CulturalSignificance { get; set; } // 200-300 words
    
    // Lucky elements
    public List<int> LuckyNumbers { get; set; } = new();
    public List<string> LuckyColors { get; set; } = new();
    public List<string> LuckyDirections { get; set; } = new();
    
    // Compatibility
    public List<ZodiacAnimal> CompatibleAnimals { get; set; } = new(); // Top 3
    public List<ZodiacAnimal> IncompatibleAnimals { get; set; } = new(); // Top 2
    
    // Cultural context
    public List<string> FamousPeople { get; set; } = new(); // Historical figures
    
    // Visual
    public string ImagePath { get; set; } // Path to SVG image
}
```

#### ElementalAnimal.cs - Element + Animal combination
```csharp
namespace LunarCalendar.Core.Models;

/// <summary>
/// Combination of Five Element + Zodiac Animal (60-year cycle)
/// </summary>
public class ElementalAnimal
{
    public int Year { get; set; }
    public FiveElement Element { get; set; }
    public ZodiacAnimal Animal { get; set; }
    
    /// <summary>
    /// Display name like "Fire Horse", "Metal Rat", "Water Tiger"
    /// </summary>
    public string DisplayName => $"{Element} {Animal}";
    
    /// <summary>
    /// How the element modifies the animal's traits
    /// </summary>
    public string ElementalTraits { get; set; }
}
```

#### ZodiacCompatibility.cs - Compatibility relationship
```csharp
namespace LunarCalendar.Core.Models;

/// <summary>
/// Compatibility relationship between two zodiac animals
/// </summary>
public class ZodiacCompatibility
{
    public ZodiacAnimal Animal1 { get; set; }
    public ZodiacAnimal Animal2 { get; set; }
    
    /// <summary>
    /// Compatibility score (0-100)
    /// </summary>
    public int Score { get; set; }
    
    /// <summary>
    /// Rating tier based on score
    /// </summary>
    public CompatibilityRating Rating => Score switch
    {
        >= 80 => CompatibilityRating.Great,
        >= 60 => CompatibilityRating.Good,
        >= 40 => CompatibilityRating.Fair,
        _ => CompatibilityRating.Poor
    };
    
    /// <summary>
    /// Explanation of the relationship dynamic (2-3 sentences)
    /// </summary>
    public string Explanation { get; set; }
}

/// <summary>
/// Compatibility rating tiers
/// </summary>
public enum CompatibilityRating
{
    Poor,    // 0-39%
    Fair,    // 40-59%
    Good,    // 60-79%
    Great    // 80-100%
}
```

#### UserZodiacProfile.cs - User's personal zodiac
```csharp
namespace LunarCalendar.Core.Models;

/// <summary>
/// User's personal zodiac profile based on birth date
/// </summary>
public class UserZodiacProfile
{
    public DateTime BirthDate { get; set; }
    public ZodiacAnimal ZodiacAnimal { get; set; }
    public FiveElement Element { get; set; }
    
    /// <summary>
    /// Elemental animal like "Metal Ox", "Fire Horse"
    /// </summary>
    public string ElementalAnimal => $"{Element} {ZodiacAnimal}";
    
    /// <summary>
    /// Personalized traits combining element and animal
    /// </summary>
    public string PersonalizedTraits { get; set; }
}
```

### 1.2 Service Interfaces

#### IZodiacService.cs
```csharp
namespace LunarCalendar.Core.Services;

/// <summary>
/// Service for zodiac animal calculations and data retrieval
/// </summary>
public interface IZodiacService
{
    /// <summary>
    /// Get zodiac animal for a Gregorian year
    /// Formula: (Year - 4) % 12
    /// </summary>
    ZodiacAnimal GetAnimalForYear(int gregorianYear);
    
    /// <summary>
    /// Get zodiac animal for a specific date (considers Lunar New Year boundary)
    /// </summary>
    ZodiacAnimal GetAnimalForDate(DateTime date);
    
    /// <summary>
    /// Get comprehensive information about a zodiac animal
    /// </summary>
    ZodiacInfo GetZodiacInfo(ZodiacAnimal animal);
    
    /// <summary>
    /// Get all 12 zodiac animals' information
    /// </summary>
    List<ZodiacInfo> GetAllZodiacInfo();
    
    /// <summary>
    /// Get elemental animal for a year (combines element + animal)
    /// Integrates with Sprint 9's SexagenaryService
    /// </summary>
    ElementalAnimal GetElementalAnimal(int gregorianYear);
    
    /// <summary>
    /// Get compatibility between two zodiac animals
    /// </summary>
    ZodiacCompatibility GetCompatibility(ZodiacAnimal animal1, ZodiacAnimal animal2);
    
    /// <summary>
    /// Get user's zodiac profile based on birth date
    /// </summary>
    UserZodiacProfile GetUserZodiacProfile(DateTime birthDate);
    
    /// <summary>
    /// Get current year's zodiac animal (considers Lunar New Year boundary)
    /// </summary>
    ZodiacAnimal GetCurrentYearAnimal();
}
```

### 1.3 Data Repository

#### ZodiacDataRepository.cs
```csharp
namespace LunarCalendar.Core.Services;

/// <summary>
/// Repository for loading and caching zodiac data from JSON files
/// </summary>
public class ZodiacDataRepository
{
    private readonly Dictionary<ZodiacAnimal, ZodiacInfo> _zodiacCache;
    private readonly Dictionary<(ZodiacAnimal, ZodiacAnimal), ZodiacCompatibility> _compatibilityCache;
    private readonly object _lock = new();
    
    public ZodiacDataRepository()
    {
        _zodiacCache = new Dictionary<ZodiacAnimal, ZodiacInfo>();
        _compatibilityCache = new Dictionary<(ZodiacAnimal, ZodiacAnimal), ZodiacCompatibility>();
    }
    
    /// <summary>
    /// Load zodiac data from embedded JSON file
    /// </summary>
    public async Task<Dictionary<ZodiacAnimal, ZodiacInfo>> LoadZodiacDataAsync()
    {
        if (_zodiacCache.Any())
            return _zodiacCache;
            
        lock (_lock)
        {
            // Double-check after acquiring lock
            if (_zodiacCache.Any())
                return _zodiacCache;
                
            // Load from Resources/Data/ZodiacData.json
            var assembly = typeof(ZodiacDataRepository).Assembly;
            using var stream = assembly.GetManifestResourceStream("LunarCalendar.Core.Data.ZodiacData.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            
            var zodiacList = JsonSerializer.Deserialize<List<ZodiacInfo>>(json);
            
            foreach (var zodiac in zodiacList)
            {
                _zodiacCache[zodiac.Animal] = zodiac;
            }
            
            return _zodiacCache;
        }
    }
    
    /// <summary>
    /// Load compatibility matrix from embedded JSON file
    /// </summary>
    public async Task<Dictionary<(ZodiacAnimal, ZodiacAnimal), ZodiacCompatibility>> LoadCompatibilityDataAsync()
    {
        if (_compatibilityCache.Any())
            return _compatibilityCache;
            
        lock (_lock)
        {
            // Double-check after acquiring lock
            if (_compatibilityCache.Any())
                return _compatibilityCache;
                
            // Load from Resources/Data/ZodiacCompatibility.json
            var assembly = typeof(ZodiacDataRepository).Assembly;
            using var stream = assembly.GetManifestResourceStream("LunarCalendar.Core.Data.ZodiacCompatibility.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            
            var compatibilityList = JsonSerializer.Deserialize<List<ZodiacCompatibility>>(json);
            
            foreach (var compatibility in compatibilityList)
            {
                _compatibilityCache[(compatibility.Animal1, compatibility.Animal2)] = compatibility;
                // Store both directions for easy lookup
                _compatibilityCache[(compatibility.Animal2, compatibility.Animal1)] = new ZodiacCompatibility
                {
                    Animal1 = compatibility.Animal2,
                    Animal2 = compatibility.Animal1,
                    Score = compatibility.Score,
                    Explanation = compatibility.Explanation
                };
            }
            
            return _compatibilityCache;
        }
    }
}
```

### 1.4 Core Service Implementation

#### ZodiacService.cs
```csharp
namespace LunarCalendar.Core.Services;

/// <summary>
/// Core zodiac service implementation
/// </summary>
public class ZodiacService : IZodiacService
{
    private readonly ZodiacDataRepository _dataRepository;
    private readonly ISexagenaryService _sexagenaryService;
    private readonly ICalendarService _calendarService;
    
    public ZodiacService(
        ZodiacDataRepository dataRepository,
        ISexagenaryService sexagenaryService,
        ICalendarService calendarService)
    {
        _dataRepository = dataRepository;
        _sexagenaryService = sexagenaryService;
        _calendarService = calendarService;
    }
    
    /// <inheritdoc />
    public ZodiacAnimal GetAnimalForYear(int gregorianYear)
    {
        // Formula: (Year - 4) % 12
        // Year 4 AD was Year of the Rat (index 0)
        var index = (gregorianYear - 4) % 12;
        if (index < 0) index += 12; // Handle negative years
        
        return (ZodiacAnimal)index;
    }
    
    /// <inheritdoc />
    public ZodiacAnimal GetAnimalForDate(DateTime date)
    {
        // Get lunar year for this date (considers Lunar New Year boundary)
        var lunarDate = _calendarService.GetLunarDate(date);
        var lunarYear = lunarDate.Year;
        
        return GetAnimalForYear(lunarYear);
    }
    
    /// <inheritdoc />
    public ZodiacInfo GetZodiacInfo(ZodiacAnimal animal)
    {
        var zodiacData = _dataRepository.LoadZodiacDataAsync().Result;
        return zodiacData[animal];
    }
    
    /// <inheritdoc />
    public List<ZodiacInfo> GetAllZodiacInfo()
    {
        var zodiacData = _dataRepository.LoadZodiacDataAsync().Result;
        return zodiacData.Values.OrderBy(z => (int)z.Animal).ToList();
    }
    
    /// <inheritdoc />
    public ElementalAnimal GetElementalAnimal(int gregorianYear)
    {
        // Use Sprint 9's SexagenaryService to get element
        var (stem, branch, zodiac) = _sexagenaryService.GetYearInfo(gregorianYear);
        var element = stem.GetElement(); // From Sprint 9: HeavenlyStem → FiveElement
        
        return new ElementalAnimal
        {
            Year = gregorianYear,
            Element = element,
            Animal = zodiac,
            ElementalTraits = $"{element} {zodiac} combines the {element} element's characteristics with the {zodiac}'s traits."
        };
    }
    
    /// <inheritdoc />
    public ZodiacCompatibility GetCompatibility(ZodiacAnimal animal1, ZodiacAnimal animal2)
    {
        var compatibilityData = _dataRepository.LoadCompatibilityDataAsync().Result;
        return compatibilityData[(animal1, animal2)];
    }
    
    /// <inheritdoc />
    public UserZodiacProfile GetUserZodiacProfile(DateTime birthDate)
    {
        var animal = GetAnimalForDate(birthDate);
        var elementalAnimal = GetElementalAnimal(birthDate.Year);
        var zodiacInfo = GetZodiacInfo(animal);
        
        return new UserZodiacProfile
        {
            BirthDate = birthDate,
            ZodiacAnimal = animal,
            Element = elementalAnimal.Element,
            PersonalizedTraits = string.Join(", ", zodiacInfo.PersonalityTraits.Take(5))
        };
    }
    
    /// <inheritdoc />
    public ZodiacAnimal GetCurrentYearAnimal()
    {
        return GetAnimalForDate(DateTime.Now);
    }
}
```

### 1.5 JSON Data Files

#### ZodiacData.json Structure
```json
[
  {
    "Animal": "Rat",
    "VietnameseName": "Tý - Chuột",
    "ChineseName": "鼠 - Shǔ",
    "EnglishName": "Rat",
    "PersonalityTraits": [
      "Intelligent and adaptable",
      "Quick-witted and resourceful",
      "Charming and sociable",
      "Creative and imaginative",
      "Observant and detail-oriented"
    ],
    "CulturalSignificance": "The Rat is the first animal in the zodiac cycle, symbolizing new beginnings and cleverness. In Vietnamese and Chinese culture, the Rat is admired for its intelligence and ability to thrive in any situation. Rats are known to be quick thinkers who can find solutions to problems others might overlook.",
    "LuckyNumbers": [2, 3],
    "LuckyColors": ["Blue", "Gold", "Green"],
    "LuckyDirections": ["Southeast", "Northeast"],
    "CompatibleAnimals": ["Dragon", "Monkey", "Ox"],
    "IncompatibleAnimals": ["Horse", "Rooster"],
    "FamousPeople": [
      "George Washington (1732)",
      "Wolfgang Amadeus Mozart (1756)",
      "Leo Tolstoy (1828)"
    ],
    "ImagePath": "Zodiac/rat.svg"
  },
  // ... 11 more animals
]
```

#### ZodiacCompatibility.json Structure
```json
[
  {
    "Animal1": "Rat",
    "Animal2": "Ox",
    "Score": 85,
    "Explanation": "Rat and Ox form a harmonious partnership. The Rat's cleverness complements the Ox's steadiness, creating a balanced and supportive relationship."
  },
  {
    "Animal1": "Rat",
    "Animal2": "Horse",
    "Score": 30,
    "Explanation": "Rat and Horse face challenges in understanding each other. Their different communication styles and priorities can lead to conflicts, though patience and compromise can help."
  },
  // ... 142 more pairings (12 x 12 = 144, minus 12 self-pairings = 132 unique, but we store 144 for easier lookup)
]
```

---

## Phase 2: UI Components

### 2.1 Reusable Controls

#### ZodiacHeaderView.xaml - Calendar header zodiac display
```xml
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:models="clr-namespace:LunarCalendar.Core.Models;assembly=LunarCalendar.Core"
             x:Class="LunarCalendar.MobileApp.Controls.ZodiacHeaderView">
    <Grid ColumnDefinitions="Auto,*" ColumnSpacing="10">
        <!-- Zodiac Animal Icon (SVG) -->
        <Image Grid.Column="0" 
               Source="{Binding ZodiacImagePath}"
               WidthRequest="40" 
               HeightRequest="40"
               Aspect="AspectFit">
            <Image.GestureRecognizers>
                <TapGestureRecognizer Command="{Binding OpenZodiacInfoCommand}" />
            </Image.GestureRecognizers>
        </Image>
        
        <!-- Year Name -->
        <VerticalStackLayout Grid.Column="1" VerticalOptions="Center">
            <Label Text="{Binding YearTitle}" 
                   FontSize="16" 
                   FontAttributes="Bold"
                   TextColor="{DynamicResource PrimaryTextColor}" />
            <Label Text="{Binding ZodiacName}" 
                   FontSize="14"
                   TextColor="{DynamicResource SecondaryTextColor}" />
        </VerticalStackLayout>
    </Grid>
</ContentView>
```

#### ZodiacCardView.xaml - Condensed zodiac info card
```xml
<Frame xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       x:Class="LunarCalendar.MobileApp.Controls.ZodiacCardView"
       CornerRadius="10"
       Padding="15"
       HasShadow="True">
    <Grid RowDefinitions="Auto,Auto,Auto" RowSpacing="10">
        <!-- Animal Image + Name -->
        <Grid Grid.Row="0" ColumnDefinitions="60,*" ColumnSpacing="10">
            <Image Grid.Column="0" 
                   Source="{Binding ZodiacInfo.ImagePath}"
                   WidthRequest="60" 
                   HeightRequest="60"
                   Aspect="AspectFit" />
            <VerticalStackLayout Grid.Column="1" VerticalOptions="Center">
                <Label Text="{Binding ZodiacInfo.EnglishName}" 
                       FontSize="18" 
                       FontAttributes="Bold" />
                <Label Text="{Binding ZodiacInfo.VietnameseName}" 
                       FontSize="14"
                       TextColor="{DynamicResource SecondaryTextColor}" />
            </VerticalStackLayout>
        </Grid>
        
        <!-- Quick Traits -->
        <Label Grid.Row="1" 
               Text="{Binding QuickTraits}"
               FontSize="14"
               LineBreakMode="TailTruncation"
               MaxLines="2" />
        
        <!-- Learn More Button -->
        <Button Grid.Row="2" 
                Text="Learn More"
                Command="{Binding LearnMoreCommand}"
                HorizontalOptions="End" />
    </Grid>
</Frame>
```

### 2.2 Main Pages

#### ZodiacInformationPage.xaml - Full zodiac browser
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:LunarCalendar.MobileApp.ViewModels"
             x:Class="LunarCalendar.MobileApp.Views.ZodiacInformationPage"
             Title="Zodiac Animals">
    
    <ContentPage.BindingContext>
        <vm:ZodiacInformationViewModel />
    </ContentPage.BindingContext>
    
    <Grid RowDefinitions="Auto,*,Auto">
        <!-- Navigation Indicator -->
        <HorizontalStackLayout Grid.Row="0" 
                               HorizontalOptions="Center"
                               Padding="10">
            <Label Text="{Binding CurrentAnimalIndex, StringFormat='{0} of 12'}" 
                   FontSize="14" />
        </HorizontalStackLayout>
        
        <!-- Swipeable Carousel -->
        <CarouselView Grid.Row="1" 
                      ItemsSource="{Binding AllZodiacInfo}"
                      CurrentItem="{Binding CurrentZodiacInfo}"
                      Position="{Binding CurrentAnimalIndex}"
                      Loop="True">
            <CarouselView.ItemTemplate>
                <DataTemplate>
                    <ScrollView>
                        <VerticalStackLayout Padding="20" Spacing="15">
                            <!-- Animal Image -->
                            <Image Source="{Binding ImagePath}"
                                   WidthRequest="200"
                                   HeightRequest="200"
                                   Aspect="AspectFit"
                                   HorizontalOptions="Center" />
                            
                            <!-- Names -->
                            <Label Text="{Binding EnglishName}" 
                                   FontSize="24" 
                                   FontAttributes="Bold"
                                   HorizontalOptions="Center" />
                            <Label Text="{Binding VietnameseName}" 
                                   FontSize="18"
                                   HorizontalOptions="Center" />
                            <Label Text="{Binding ChineseName}" 
                                   FontSize="18"
                                   HorizontalOptions="Center" />
                            
                            <!-- Personality Traits -->
                            <Label Text="Personality Traits" 
                                   FontSize="18" 
                                   FontAttributes="Bold" 
                                   Margin="0,10,0,5" />
                            <CollectionView ItemsSource="{Binding PersonalityTraits}">
                                <CollectionView.ItemTemplate>
                                    <DataTemplate>
                                        <Label Text="{Binding .}" Padding="5,2" />
                                    </DataTemplate>
                                </CollectionView.ItemTemplate>
                            </CollectionView>
                            
                            <!-- Lucky Elements -->
                            <Grid ColumnDefinitions="*,*,*" RowDefinitions="Auto,Auto" Margin="0,10,0,0">
                                <Label Grid.Row="0" Grid.Column="0" Text="Lucky Numbers" FontAttributes="Bold" />
                                <Label Grid.Row="1" Grid.Column="0" Text="{Binding LuckyNumbersString}" />
                                
                                <Label Grid.Row="0" Grid.Column="1" Text="Lucky Colors" FontAttributes="Bold" />
                                <Label Grid.Row="1" Grid.Column="1" Text="{Binding LuckyColorsString}" />
                                
                                <Label Grid.Row="0" Grid.Column="2" Text="Lucky Directions" FontAttributes="Bold" />
                                <Label Grid.Row="1" Grid.Column="2" Text="{Binding LuckyDirectionsString}" />
                            </Grid>
                            
                            <!-- Cultural Significance -->
                            <Label Text="Cultural Significance" 
                                   FontSize="18" 
                                   FontAttributes="Bold" 
                                   Margin="0,10,0,5" />
                            <Label Text="{Binding CulturalSignificance}" />
                            
                            <!-- Famous People -->
                            <Label Text="Famous People" 
                                   FontSize="18" 
                                   FontAttributes="Bold" 
                                   Margin="0,10,0,5" />
                            <CollectionView ItemsSource="{Binding FamousPeople}">
                                <CollectionView.ItemTemplate>
                                    <DataTemplate>
                                        <Label Text="{Binding .}" Padding="5,2" />
                                    </DataTemplate>
                                </CollectionView.ItemTemplate>
                            </CollectionView>
                        </VerticalStackLayout>
                    </ScrollView>
                </DataTemplate>
            </CarouselView.ItemTemplate>
        </CarouselView>
        
        <!-- Action Buttons -->
        <HorizontalStackLayout Grid.Row="2" 
                               HorizontalOptions="Center"
                               Padding="10"
                               Spacing="10">
            <Button Text="Check Compatibility" 
                    Command="{Binding CheckCompatibilityCommand}" />
            <Button Text="My Profile" 
                    Command="{Binding ViewMyProfileCommand}" />
        </HorizontalStackLayout>
    </Grid>
</ContentPage>
```

#### ZodiacCompatibilityPage.xaml - Compatibility checker
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:LunarCalendar.MobileApp.ViewModels"
             x:Class="LunarCalendar.MobileApp.Views.ZodiacCompatibilityPage"
             Title="Zodiac Compatibility">
    
    <ContentPage.BindingContext>
        <vm:ZodiacCompatibilityViewModel />
    </ContentPage.BindingContext>
    
    <ScrollView>
        <VerticalStackLayout Padding="20" Spacing="20">
            <!-- Animal 1 Picker -->
            <Frame>
                <VerticalStackLayout Spacing="10">
                    <Label Text="Select First Animal" FontSize="16" FontAttributes="Bold" />
                    <Picker ItemsSource="{Binding AllAnimals}"
                            SelectedItem="{Binding Animal1}"
                            Title="Choose animal..." />
                    <Image Source="{Binding Animal1ImagePath}"
                           WidthRequest="100"
                           HeightRequest="100"
                           Aspect="AspectFit"
                           HorizontalOptions="Center" />
                </VerticalStackLayout>
            </Frame>
            
            <!-- Hearts Icon (visual separator) -->
            <Label Text="💕" 
                   FontSize="40" 
                   HorizontalOptions="Center" />
            
            <!-- Animal 2 Picker -->
            <Frame>
                <VerticalStackLayout Spacing="10">
                    <Label Text="Select Second Animal" FontSize="16" FontAttributes="Bold" />
                    <Picker ItemsSource="{Binding AllAnimals}"
                            SelectedItem="{Binding Animal2}"
                            Title="Choose animal..." />
                    <Image Source="{Binding Animal2ImagePath}"
                           WidthRequest="100"
                           HeightRequest="100"
                           Aspect="AspectFit"
                           HorizontalOptions="Center" />
                </VerticalStackLayout>
            </Frame>
            
            <!-- Check Compatibility Button -->
            <Button Text="Check Compatibility" 
                    Command="{Binding CheckCompatibilityCommand}"
                    IsEnabled="{Binding BothAnimalsSelected}" />
            
            <!-- Compatibility Result -->
            <Frame IsVisible="{Binding HasResult}" 
                   BackgroundColor="{Binding ResultBackgroundColor}">
                <VerticalStackLayout Spacing="10">
                    <Label Text="{Binding CompatibilityTitle}" 
                           FontSize="24" 
                           FontAttributes="Bold"
                           HorizontalOptions="Center" />
                    <Label Text="{Binding CompatibilityScore, StringFormat='Score: {0}%'}" 
                           FontSize="20"
                           HorizontalOptions="Center" />
                    <Label Text="{Binding CompatibilityExplanation}" 
                           FontSize="16" />
                </VerticalStackLayout>
            </Frame>
            
            <!-- Share Button -->
            <Button Text="Share Result" 
                    Command="{Binding ShareCommand}"
                    IsVisible="{Binding HasResult}" />
        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

---

## Phase 3: ViewModels

### ZodiacInformationViewModel.cs
```csharp
namespace LunarCalendar.MobileApp.ViewModels;

public partial class ZodiacInformationViewModel : BaseViewModel
{
    private readonly IZodiacService _zodiacService;
    
    [ObservableProperty]
    private List<ZodiacInfo> _allZodiacInfo;
    
    [ObservableProperty]
    private ZodiacInfo _currentZodiacInfo;
    
    [ObservableProperty]
    private int _currentAnimalIndex;
    
    public ZodiacInformationViewModel(IZodiacService zodiacService)
    {
        _zodiacService = zodiacService;
        Title = "Zodiac Animals";
        LoadZodiacData();
    }
    
    private void LoadZodiacData()
    {
        AllZodiacInfo = _zodiacService.GetAllZodiacInfo();
        
        // Start with current year's animal
        var currentAnimal = _zodiacService.GetCurrentYearAnimal();
        CurrentAnimalIndex = (int)currentAnimal;
        CurrentZodiacInfo = AllZodiacInfo[CurrentAnimalIndex];
    }
    
    [RelayCommand]
    private async Task CheckCompatibility()
    {
        await Shell.Current.GoToAsync("///ZodiacCompatibilityPage");
    }
    
    [RelayCommand]
    private async Task ViewMyProfile()
    {
        // Navigate to My Zodiac Profile (in Settings)
        await Shell.Current.GoToAsync("///SettingsPage");
    }
}
```

### ZodiacCompatibilityViewModel.cs
```csharp
namespace LunarCalendar.MobileApp.ViewModels;

public partial class ZodiacCompatibilityViewModel : BaseViewModel
{
    private readonly IZodiacService _zodiacService;
    
    [ObservableProperty]
    private List<string> _allAnimals;
    
    [ObservableProperty]
    private string _animal1;
    
    [ObservableProperty]
    private string _animal2;
    
    [ObservableProperty]
    private bool _hasResult;
    
    [ObservableProperty]
    private string _compatibilityTitle;
    
    [ObservableProperty]
    private int _compatibilityScore;
    
    [ObservableProperty]
    private string _compatibilityExplanation;
    
    [ObservableProperty]
    private Color _resultBackgroundColor;
    
    public bool BothAnimalsSelected => !string.IsNullOrEmpty(Animal1) && !string.IsNullOrEmpty(Animal2);
    
    public ZodiacCompatibilityViewModel(IZodiacService zodiacService)
    {
        _zodiacService = zodiacService;
        Title = "Zodiac Compatibility";
        LoadAnimals();
    }
    
    private void LoadAnimals()
    {
        AllAnimals = Enum.GetNames(typeof(ZodiacAnimal)).ToList();
        
        // Pre-select user's zodiac animal if birth date is set
        // TODO: Get from user preferences
    }
    
    [RelayCommand]
    private void CheckCompatibility()
    {
        if (!BothAnimalsSelected) return;
        
        var animal1Enum = Enum.Parse<ZodiacAnimal>(Animal1);
        var animal2Enum = Enum.Parse<ZodiacAnimal>(Animal2);
        
        var compatibility = _zodiacService.GetCompatibility(animal1Enum, animal2Enum);
        
        CompatibilityScore = compatibility.Score;
        CompatibilityExplanation = compatibility.Explanation;
        CompatibilityTitle = $"{compatibility.Rating} Match";
        
        // Set background color based on rating
        ResultBackgroundColor = compatibility.Rating switch
        {
            CompatibilityRating.Great => Colors.LightGreen,
            CompatibilityRating.Good => Colors.LightBlue,
            CompatibilityRating.Fair => Colors.LightYellow,
            CompatibilityRating.Poor => Colors.LightCoral,
            _ => Colors.White
        };
        
        HasResult = true;
    }
    
    [RelayCommand]
    private async Task Share()
    {
        var text = $"Zodiac Compatibility: {Animal1} + {Animal2} = {CompatibilityTitle} ({CompatibilityScore}%)";
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Title = "Zodiac Compatibility",
            Text = text
        });
    }
}
```

---

## Phase 4: Integration with Existing UI

### Modify CalendarViewModel.cs
```csharp
// Add property for current year's zodiac animal
[ObservableProperty]
private ZodiacAnimal _currentYearZodiacAnimal;

[ObservableProperty]
private string _currentYearZodiacName;

// In constructor or initialization:
private void LoadCurrentYearZodiac()
{
    CurrentYearZodiacAnimal = _zodiacService.GetCurrentYearAnimal();
    CurrentYearZodiacName = $"Year of the {CurrentYearZodiacAnimal}";
}
```

### Modify CalendarPage.xaml
```xml
<!-- Add ZodiacHeaderView to calendar header -->
<controls:ZodiacHeaderView 
    ZodiacImagePath="{Binding CurrentYearZodiacImagePath}"
    YearTitle="{Binding CurrentYear}"
    ZodiacName="{Binding CurrentYearZodiacName}"
    OpenZodiacInfoCommand="{Binding OpenZodiacInfoCommand}" />
```

### Modify HolidayDetailViewModel.cs (Date Detail)
```csharp
// Add zodiac info for the displayed date
[ObservableProperty]
private ZodiacInfo _zodiacInfo;

[ObservableProperty]
private ElementalAnimal _elementalAnimal;

// In LoadDateDetails():
private void LoadZodiacForDate(DateTime date)
{
    var animal = _zodiacService.GetAnimalForDate(date);
    ZodiacInfo = _zodiacService.GetZodiacInfo(animal);
    ElementalAnimal = _zodiacService.GetElementalAnimal(date.Year);
}
```

### Modify SettingsPage.xaml
```xml
<!-- Add Zodiac & Astrology section -->
<Frame>
    <VerticalStackLayout Spacing="10">
        <Label Text="Zodiac & Astrology" FontSize="18" FontAttributes="Bold" />
        
        <!-- Birth Date -->
        <HorizontalStackLayout Spacing="10">
            <Label Text="Birth Date:" VerticalOptions="Center" />
            <DatePicker Date="{Binding BirthDate}" />
        </HorizontalStackLayout>
        
        <!-- My Zodiac Profile Button -->
        <Button Text="View My Zodiac Profile" 
                Command="{Binding ViewZodiacProfileCommand}" />
        
        <!-- Zodiac Animals Button -->
        <Button Text="Explore Zodiac Animals" 
                Command="{Binding ExploreZodiacCommand}" />
        
        <!-- Compatibility Checker Button -->
        <Button Text="Check Compatibility" 
                Command="{Binding CheckCompatibilityCommand}" />
    </VerticalStackLayout>
</Frame>
```

---

## Phase 5: Testing Strategy

### Unit Tests

#### ZodiacServiceTests.cs
```csharp
[Fact]
public void GetAnimalForYear_2026_ReturnsHorse()
{
    // Arrange
    var service = new ZodiacService(...);
    
    // Act
    var animal = service.GetAnimalForYear(2026);
    
    // Assert
    animal.Should().Be(ZodiacAnimal.Horse);
}

[Theory]
[InlineData(2020, ZodiacAnimal.Rat)]
[InlineData(2021, ZodiacAnimal.Ox)]
[InlineData(2022, ZodiacAnimal.Tiger)]
[InlineData(2023, ZodiacAnimal.Rabbit)]
[InlineData(2024, ZodiacAnimal.Dragon)]
[InlineData(2025, ZodiacAnimal.Snake)]
[InlineData(2026, ZodiacAnimal.Horse)]
[InlineData(2027, ZodiacAnimal.Goat)]
[InlineData(2028, ZodiacAnimal.Monkey)]
[InlineData(2029, ZodiacAnimal.Rooster)]
[InlineData(2030, ZodiacAnimal.Dog)]
[InlineData(2031, ZodiacAnimal.Pig)]
public void GetAnimalForYear_ValidYears_ReturnsCorrectAnimal(int year, ZodiacAnimal expected)
{
    // Arrange
    var service = new ZodiacService(...);
    
    // Act
    var animal = service.GetAnimalForYear(year);
    
    // Assert
    animal.Should().Be(expected);
}

[Fact]
public void GetAnimalForDate_LunarNewYearBoundary_ChangesAnimal()
{
    // Arrange
    var service = new ZodiacService(...);
    var beforeNewYear = new DateTime(2026, 1, 28); // Still Year of Snake
    var afterNewYear = new DateTime(2026, 1, 29);  // Year of Horse
    
    // Act
    var animalBefore = service.GetAnimalForDate(beforeNewYear);
    var animalAfter = service.GetAnimalForDate(afterNewYear);
    
    // Assert
    animalBefore.Should().Be(ZodiacAnimal.Snake);
    animalAfter.Should().Be(ZodiacAnimal.Horse);
}

[Fact]
public void GetElementalAnimal_2026_ReturnsFireHorse()
{
    // Arrange
    var service = new ZodiacService(...);
    
    // Act
    var elementalAnimal = service.GetElementalAnimal(2026);
    
    // Assert
    elementalAnimal.Animal.Should().Be(ZodiacAnimal.Horse);
    elementalAnimal.Element.Should().Be(FiveElement.Fire);
    elementalAnimal.DisplayName.Should().Be("Fire Horse");
}

[Theory]
[InlineData(ZodiacAnimal.Horse, ZodiacAnimal.Dog, CompatibilityRating.Great)]
[InlineData(ZodiacAnimal.Horse, ZodiacAnimal.Rat, CompatibilityRating.Poor)]
public void GetCompatibility_ValidPairs_ReturnsCorrectRating(
    ZodiacAnimal animal1, 
    ZodiacAnimal animal2, 
    CompatibilityRating expectedRating)
{
    // Arrange
    var service = new ZodiacService(...);
    
    // Act
    var compatibility = service.GetCompatibility(animal1, animal2);
    
    // Assert
    compatibility.Rating.Should().Be(expectedRating);
    compatibility.Explanation.Should().NotBeNullOrEmpty();
}
```

#### ZodiacDataRepositoryTests.cs
```csharp
[Fact]
public async Task LoadZodiacDataAsync_LoadsAll12Animals()
{
    // Arrange
    var repository = new ZodiacDataRepository();
    
    // Act
    var data = await repository.LoadZodiacDataAsync();
    
    // Assert
    data.Should().HaveCount(12);
    data.Keys.Should().Contain(ZodiacAnimal.Rat);
    data.Keys.Should().Contain(ZodiacAnimal.Horse);
}

[Fact]
public async Task LoadCompatibilityDataAsync_Loads144Pairings()
{
    // Arrange
    var repository = new ZodiacDataRepository();
    
    // Act
    var data = await repository.LoadCompatibilityDataAsync();
    
    // Assert
    data.Should().HaveCount(144); // 12 x 12
}
```

### Integration Tests

#### ZodiacIntegrationTests.cs
```csharp
[Fact]
public async Task ZodiacInformationPage_LoadsAndDisplaysCurrentYearAnimal()
{
    // Arrange
    var page = new ZodiacInformationPage();
    
    // Act
    await page.OnAppearing();
    
    // Assert
    var viewModel = page.BindingContext as ZodiacInformationViewModel;
    viewModel.CurrentZodiacInfo.Should().NotBeNull();
    viewModel.CurrentZodiacInfo.Animal.Should().Be(ZodiacAnimal.Horse); // 2026
}
```

### Performance Tests

```csharp
[Fact]
public void ZodiacService_GetAnimalForYear_CompletesUnder10ms()
{
    // Arrange
    var service = new ZodiacService(...);
    var stopwatch = Stopwatch.StartNew();
    
    // Act
    for (int i = 0; i < 1000; i++)
    {
        service.GetAnimalForYear(2000 + i);
    }
    stopwatch.Stop();
    
    // Assert
    var avgTime = stopwatch.ElapsedMilliseconds / 1000.0;
    avgTime.Should().BeLessThan(10); // < 10ms average
}

[Fact]
public async Task ZodiacInformationPage_LoadsUnder500ms()
{
    // Arrange
    var page = new ZodiacInformationPage();
    var stopwatch = Stopwatch.StartNew();
    
    // Act
    await page.OnAppearing();
    stopwatch.Stop();
    
    // Assert
    stopwatch.ElapsedMilliseconds.Should().BeLessThan(500);
}
```

---

## Phase 6: Localization

### Resource Strings (Strings.en.resx, Strings.vi.resx, Strings.zh.resx)

```xml
<!-- English (Strings.en.resx) -->
<data name="Zodiac_PageTitle" xml:space="preserve">
    <value>Zodiac Animals</value>
</data>
<data name="Zodiac_Compatibility" xml:space="preserve">
    <value>Check Compatibility</value>
</data>
<data name="Zodiac_MyProfile" xml:space="preserve">
    <value>My Zodiac Profile</value>
</data>

<!-- Vietnamese (Strings.vi.resx) -->
<data name="Zodiac_PageTitle" xml:space="preserve">
    <value>12 Con Giáp</value>
</data>
<data name="Zodiac_Compatibility" xml:space="preserve">
    <value>Kiểm Tra Hợp</value>
</data>
<data name="Zodiac_MyProfile" xml:space="preserve">
    <value>Hồ Sơ Con Giáp Của Tôi</value>
</data>

<!-- Chinese (Strings.zh.resx) -->
<data name="Zodiac_PageTitle" xml:space="preserve">
    <value>生肖</value>
</data>
<data name="Zodiac_Compatibility" xml:space="preserve">
    <value>查兼容性</value>
</data>
<data name="Zodiac_MyProfile" xml:space="preserve">
    <value>我的生肖档案</value>
</data>
```

---

## Phase 7: Asset Creation

### Zodiac Animal SVG Images

**Requirements**:
- 12 SVG files (one per animal)
- Each file ~100-150 KB (optimized)
- Designed for both light and dark modes
- Consistent art style across all animals
- Simple, recognizable silhouettes

**Naming Convention**:
- `rat.svg`, `ox.svg`, `tiger.svg`, etc.
- Stored in `Resources/Images/Zodiac/`

**Fallback Strategy**:
```csharp
// Unicode emoji as primary implementation for Sprint 10
private string GetZodiacEmoji(ZodiacAnimal animal)
{
    return animal switch
    {
        ZodiacAnimal.Rat => "🐭",
        ZodiacAnimal.Ox => "🐮",
        ZodiacAnimal.Tiger => "🐯",
        ZodiacAnimal.Rabbit => "🐰",
        ZodiacAnimal.Dragon => "🐲",
        ZodiacAnimal.Snake => "🐍",
        ZodiacAnimal.Horse => "🐴",
        ZodiacAnimal.Goat => "🐑",
        ZodiacAnimal.Monkey => "🐵",
        ZodiacAnimal.Rooster => "🐔",
        ZodiacAnimal.Dog => "🐶",
        ZodiacAnimal.Pig => "🐷",
        _ => "❓"
    };
}
```

---

## Implementation Timeline

### Sprint 10: 2 Weeks (10 Business Days)

| Phase | Days | Tasks | Dependencies |
|-------|------|-------|--------------|
| **Phase 0: Research** | 0.5 | Finalize zodiac content sources, compatibility matrix, image sourcing | None |
| **Phase 1: Core Library** | 2 | Models, interfaces, ZodiacService, ZodiacDataRepository, JSON files | Sprint 9 complete |
| **Phase 2: UI Components** | 2 | ZodiacHeaderView, ZodiacCardView, ZodiacAnimalPicker | Phase 1 |
| **Phase 3: Main Pages** | 1.5 | ZodiacInformationPage, ZodiacCompatibilityPage | Phase 2 |
| **Phase 4: ViewModels** | 1 | ZodiacInformationViewModel, ZodiacCompatibilityViewModel | Phase 3 |
| **Phase 5: Integration** | 1 | Modify CalendarViewModel, HolidayDetailViewModel, SettingsPage | Phase 4 |
| **Phase 6: Localization** | 0.5 | Add strings to .resx files (English + Vietnamese only) | Phase 5 |
| **Phase 7: Assets** | 0.1 | Use Unicode emoji (no asset creation needed) | Phase 1 |
| **Phase 8: Testing** | 1.5 | Unit tests, integration tests, performance tests | Phase 5 |
| **Phase 9: Cultural SME Review** | External | Send content for review (async) | Phase 6 |
| **Phase 10: Bug Fixes & Polish** | 0.5 | Fix issues from testing, polish UI | Phase 8 |

**Total**: 10 days (2 weeks)

**Notes**:
- Cultural SME review can happen in parallel (external dependency)
- Phase 7 simplified: Using emoji (0.1 days), no asset sourcing needed
- SVG artwork can be commissioned in parallel and added in Sprint 10.1 or 11
- Testing (Phase 8) should start early with TDD approach
- Chinese localization deferred to Sprint 14

---

## Risk Mitigation

### High Risk: Cultural Accuracy
**Risk**: Zodiac content may not be culturally authentic  
**Mitigation**:
- Source content from authoritative Vietnamese/Chinese references
- Engage 2+ Vietnamese cultural SMEs for content review
- Document all sources in `research/zodiac-content-sources.md`
- Be prepared to iterate on content based on SME feedback

### Low Risk: Image Quality (RESOLVED)
**Previous Risk**: Zodiac images may exceed 1.5 MB total  
**Resolution**: Using Unicode emoji for Sprint 10 MVP
- Zero bundle size impact
- Instant availability
- Good mobile support
- SVG artwork can be added later as polish (optional)

### Medium Risk: Compatibility Algorithm Subjectivity
**Risk**: Different sources have different compatibility ratings  
**Mitigation**:
- Use a simplified, well-documented matrix approach
- Document sources for all 144 pairings
- Focus on mainstream Vietnamese/Chinese consensus
- Include disclaimer: "Based on traditional Vietnamese astrology"

### Low Risk: Sprint 9 Integration
**Risk**: Changes to Sprint 9 code could break integration  
**Mitigation**:
- Sprint 9 is complete and stable
- Integration uses well-defined interfaces (ISexagenaryService)
- Unit tests cover integration points

---

## Success Metrics

### Functionality
- ✅ Zodiac animal calculation: 100% accuracy on 100 test years (2000-2099)
- ✅ Lunar New Year boundary: Correctly transitions zodiac in all test cases
- ✅ All 12 zodiac animals: Complete data (traits, lucky elements, folklore)
- ✅ Compatibility checker: All 144 pairings have scores and explanations

### Performance
- ✅ Zodiac calculation: < 10ms average (target: 5-8ms)
- ✅ ZodiacInformationPage load: < 500ms (target: 300-400ms)
- ✅ Scrolling FPS: 60 FPS on zodiac carousel
- ✅ Total asset size: < 1.5 MB (target: 1.0-1.2 MB)

### Quality
- ✅ Unit test coverage: > 90% for ZodiacService
- ✅ Zero P0/P1 bugs in final testing
- ✅ Cultural accuracy: Approved by 2+ Vietnamese SMEs
- ✅ Accessibility: WCAG 2.1 AA compliance

### User Experience
- ✅ Zodiac header visible on calendar launch (no extra taps)
- ✅ Swipe navigation works smoothly on zodiac browser
- ✅ Compatibility checker pre-selects user's animal (if birth date set)
- ✅ Share functionality works on both iOS and Android

---

## Dependencies

### Sprint 9 (CRITICAL - Already Complete ✅)
- `SexagenaryService.GetYearInfo(year)` → Returns (Stem, Branch, Zodiac)
- `EarthlyBranch` enum → Maps to `ZodiacAnimal` enum
- `FiveElement` enum → Used for elemental animal combinations
- `HeavenlyStem.GetElement()` → Returns element for elemental animals
- `CalendarService.GetLunarNewYear(year)` → Determines zodiac year boundaries

### External Dependencies
- **Cultural SME Review**: 2+ Vietnamese experts (external)
- **Zodiac Images**: Using emoji for Sprint 10 (no external dependency)
  - Optional: Commission SVG artwork in parallel for future sprint
- **Content Validation**: Authoritative zodiac references (research)

### Internal Dependencies
- **Localization Files**: Existing .resx infrastructure
- **MAUI UI Framework**: Existing XAML pages and view models
- **Settings/Preferences**: Existing user preferences system (for birth date)

---

## Deployment Notes

### Pre-Deployment Checklist
- [ ] All unit tests pass (90%+ coverage)
- [ ] Integration tests pass
- [ ] Performance benchmarks met (<10ms, <500ms)
- [ ] Cultural SME review complete and approved
- [ ] Emoji rendering tested on iOS and Android (fallback working)
- [ ] Localization complete (Vietnamese and English only for Sprint 10)
- [ ] Accessibility tested (screen readers, emoji alt text)
- [ ] iOS and Android device testing complete
- [ ] Zero P0/P1 bugs

### Optional Post-Sprint 10 Enhancements
- [ ] Commission SVG zodiac artwork (parallel work)
- [ ] Add Chinese localization (Sprint 14)
- [ ] Replace emoji with SVG images (Sprint 10.1 or 11)

### Post-Deployment Monitoring
- Monitor zodiac animal display accuracy (user reports)
- Track Zodiac Information page load times (analytics)
- Monitor compatibility checker usage (engagement metrics)
- Collect user feedback on cultural accuracy (in-app survey)

---

## Next Steps

1. **Review & Approve This Plan**: Stakeholders review technical approach, architecture, and timeline
2. **Create Feature Branch**: `git checkout -b feature/002-zodiac-animals`
3. **Run `/speckit.tasks`**: Generate granular task breakdown from this plan
4. **Phase 0 Execution**: Complete research tasks (zodiac content, compatibility matrix, image sourcing)
5. **Phase 1 Kickoff**: Begin Core Library implementation (models, service, data repository)
6. **Iterative Development**: Follow 10-day timeline, testing as you build
7. **Cultural SME Review**: Schedule external review during Phase 6-8
8. **Sprint 10 Completion**: Merge to `develop` branch after all acceptance criteria met

---

**Last Updated**: January 26, 2026  
**Status**: 🟢 Ready for Implementation  
**Next Command**: `/speckit.tasks` to generate task breakdown  
**Estimated Completion**: Mid-February 2026 (2 weeks from kickoff)  
**Prepared By**: GitHub Copilot (speckit.plan agent)
