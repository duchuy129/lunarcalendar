# Vietnamese Lunar Calendar Application
## Technical Architecture Document

**Version:** 2.0  
**Date:** January 3, 2026  
**Status:** MVP Implementation Complete

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [System Components](#system-components)
3. [Core Algorithms](#core-algorithms)
4. [Data Architecture](#data-architecture)
5. [Service Layer](#service-layer)
6. [API Design](#api-design)
7. [Design Patterns](#design-patterns)
8. [Platform Architecture](#platform-architecture)

---

## 1. Architecture Overview

### 1.1 Architecture Style

**Hybrid Architecture: Bundled + Optional API**

```
┌─────────────────────────────────────────────────┐
│              Mobile Application                  │
│  ┌─────────────────────────────────────────┐   │
│  │         Presentation Layer              │   │
│  │  (Views, ViewModels, UI Components)     │   │
│  └──────────────┬──────────────────────────┘   │
│                 │                               │
│  ┌──────────────▼──────────────────────────┐   │
│  │         Service Layer                    │   │
│  │  (Calendar, Holiday, Localization)      │   │
│  └──────────────┬──────────────────────────┘   │
│                 │                               │
│  ┌──────────────▼──────────────────────────┐   │
│  │      Core Calculation Layer              │   │
│  │  (LunarCalculation, HolidayCalculation)  │   │
│  │         [BUNDLED IN APP]                 │   │
│  └──────────────┬──────────────────────────┘   │
│                 │                               │
│  ┌──────────────▼──────────────────────────┐   │
│  │        Local Data Layer                  │   │
│  │     (SQLite Cache, Preferences)          │   │
│  └──────────────────────────────────────────┘   │
└─────────────────────────────────────────────────┘
                  │
                  │ (Optional for Future Features)
                  ▼
    ┌──────────────────────────┐
    │    REST API (Optional)    │
    │  (.NET Core Web API)      │
    └──────────────┬─────────────┘
                  │
                  ▼
    ┌──────────────────────────┐
    │   PostgreSQL Database     │
    │  (User Data, Events)      │
    └──────────────────────────┘
```

### 1.2 Key Architectural Decisions

#### Decision 1: Bundled Core Logic
**Rationale:** Offline-first approach, instant calculations, no API dependency for MVP

**Benefits:**
- Zero network latency
- 100% offline functionality
- No server costs for MVP
- Simplified deployment
- Better user experience

**Trade-offs:**
- Updates require app release
- Cannot dynamically update holiday data
- Larger app bundle size

---

#### Decision 2: .NET MAUI Framework
**Rationale:** Cross-platform development with native performance

**Benefits:**
- Single codebase for iOS, Android, macOS
- Native UI controls
- .NET ecosystem integration
- Strong typing with C#
- Built-in MVVM support

**Trade-offs:**
- Larger app size vs native apps
- Framework updates required
- Platform-specific code still needed occasionally

---

#### Decision 3: MVVM Pattern
**Rationale:** Separation of concerns, testability, maintainability

**Components:**
- **Model**: Data entities (LunarDate, Holiday)
- **View**: XAML UI definitions
- **ViewModel**: Business logic, data binding, commands

**Benefits:**
- Clear separation of UI and logic
- Testable business logic
- Reusable ViewModels
- Data binding reduces boilerplate

---

#### Decision 4: ChineseLunisolarCalendar
**Rationale:** Built-in .NET calendar system, proven accuracy

**Benefits:**
- Well-tested algorithm
- Framework support
- No custom implementation needed
- Handles leap months correctly

**Limitations:**
- Date range: 1901-2100
- Fixed to Chinese lunar calendar (works for Vietnamese)
- No customization for regional variations

---

### 1.3 Technology Stack

**Mobile Frontend:**
- .NET MAUI (Multi-platform App UI)
- .NET 8.0 or later
- C# 10.0+
- XAML for UI definitions

**Core Libraries:**
- CommunityToolkit.Mvvm - MVVM helpers
- sqlite-net-pcl - Local database
- System.Globalization - Calendar calculations

**Backend API (Optional):**
- ASP.NET Core 8.0
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Swagger/OpenAPI

**Development Tools:**
- Visual Studio / VS Code
- Xcode (for iOS builds)
- Android SDK
- Git version control

---

## 2. System Components

### 2.1 Component Diagram

```
┌─────────────────────────────────────────────────────────┐
│                    Mobile Application                    │
├─────────────────────────────────────────────────────────┤
│                                                           │
│  ┌────────────────────────────────────────────────────┐ │
│  │              Presentation Layer                     │ │
│  ├────────────────────────────────────────────────────┤ │
│  │ Views:                                              │ │
│  │  • CalendarPage          • YearHolidaysPage        │ │
│  │  • HolidayDetailPage     • SettingsPage            │ │
│  │  • WelcomePage                                     │ │
│  ├────────────────────────────────────────────────────┤ │
│  │ ViewModels:                                         │ │
│  │  • CalendarViewModel     • YearHolidaysViewModel   │ │
│  │  • HolidayDetailViewModel • SettingsViewModel      │ │
│  │  • WelcomeViewModel      • BaseViewModel           │ │
│  └────────────────────────────────────────────────────┘ │
│                          │                               │
│  ┌────────────────────────────────────────────────────┐ │
│  │              Service Layer                          │ │
│  ├────────────────────────────────────────────────────┤ │
│  │ • ICalendarService       • IHolidayService         │ │
│  │ • ILocalizationService   • IUserModeService        │ │
│  │ • IHapticService         • IConnectivityService    │ │
│  │ • ISyncService                                     │ │
│  └────────────────────────────────────────────────────┘ │
│                          │                               │
│  ┌────────────────────────────────────────────────────┐ │
│  │            Core Calculation Layer                   │ │
│  ├────────────────────────────────────────────────────┤ │
│  │ • ILunarCalculationService                         │ │
│  │ • IHolidayCalculationService                       │ │
│  │ • HolidaySeeder (embedded data)                    │ │
│  └────────────────────────────────────────────────────┘ │
│                          │                               │
│  ┌────────────────────────────────────────────────────┐ │
│  │              Data Layer                             │ │
│  ├────────────────────────────────────────────────────┤ │
│  │ • LunarCalendarDatabase (SQLite)                   │ │
│  │ • Preferences (key-value storage)                  │ │
│  │ • AppResources (localization .resx)                │ │
│  └────────────────────────────────────────────────────┘ │
│                                                           │
│  ┌────────────────────────────────────────────────────┐ │
│  │            Helper/Utility Layer                     │ │
│  ├────────────────────────────────────────────────────┤ │
│  │ • DateFormatterHelper                              │ │
│  │ • ColorHelper                                      │ │
│  │ • Converters (XAML value converters)              │ │
│  └────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────┘
```

---

### 2.2 Project Structure

```
/lunarcalendar/
├── src/
│   ├── LunarCalendar.Core/              # Shared business logic
│   │   ├── Models/
│   │   │   ├── LunarDate.cs
│   │   │   └── Holiday.cs
│   │   ├── Services/
│   │   │   ├── ILunarCalculationService.cs
│   │   │   ├── LunarCalculationService.cs
│   │   │   ├── IHolidayCalculationService.cs
│   │   │   └── HolidayCalculationService.cs
│   │   └── Data/
│   │       └── HolidaySeeder.cs
│   │
│   ├── LunarCalendar.MobileApp/         # MAUI application
│   │   ├── App.xaml / App.xaml.cs       # App entry point
│   │   ├── MauiProgram.cs               # DI container setup
│   │   ├── Views/                       # XAML pages
│   │   │   ├── CalendarPage.xaml
│   │   │   ├── YearHolidaysPage.xaml
│   │   │   ├── HolidayDetailPage.xaml
│   │   │   ├── SettingsPage.xaml
│   │   │   └── WelcomePage.xaml
│   │   ├── ViewModels/                  # Business logic
│   │   │   ├── BaseViewModel.cs
│   │   │   ├── CalendarViewModel.cs
│   │   │   ├── YearHolidaysViewModel.cs
│   │   │   ├── HolidayDetailViewModel.cs
│   │   │   ├── SettingsViewModel.cs
│   │   │   └── WelcomeViewModel.cs
│   │   ├── Services/                    # App-level services
│   │   │   ├── ICalendarService.cs
│   │   │   ├── CalendarService.cs
│   │   │   ├── IHolidayService.cs
│   │   │   ├── HolidayService.cs
│   │   │   ├── ILocalizationService.cs
│   │   │   ├── LocalizationService.cs
│   │   │   ├── IUserModeService.cs
│   │   │   ├── UserModeService.cs
│   │   │   ├── IHapticService.cs
│   │   │   ├── HapticService.cs
│   │   │   ├── IConnectivityService.cs
│   │   │   ├── ConnectivityService.cs
│   │   │   ├── ISyncService.cs
│   │   │   └── SyncService.cs
│   │   ├── Models/                      # UI-specific models
│   │   │   ├── CalendarDay.cs
│   │   │   └── LunarDateEntity.cs
│   │   ├── Data/
│   │   │   └── LunarCalendarDatabase.cs # SQLite wrapper
│   │   ├── Helpers/
│   │   │   ├── DateFormatterHelper.cs
│   │   │   └── ColorHelper.cs
│   │   ├── Converters/                  # XAML converters
│   │   │   ├── BoolToVisibilityConverter.cs
│   │   │   ├── ColorConverter.cs
│   │   │   └── InverseBoolConverter.cs
│   │   ├── Extensions/
│   │   │   └── DateTimeExtensions.cs
│   │   ├── Resources/
│   │   │   ├── Strings/
│   │   │   │   ├── AppResources.resx    # English
│   │   │   │   └── AppResources.vi.resx # Vietnamese
│   │   │   ├── Styles/
│   │   │   │   ├── Colors.xaml
│   │   │   │   └── Styles.xaml
│   │   │   ├── Images/
│   │   │   ├── Fonts/
│   │   │   └── AppIcon/
│   │   ├── Platforms/                   # Platform-specific code
│   │   │   ├── Android/
│   │   │   ├── iOS/
│   │   │   ├── MacCatalyst/
│   │   │   └── Windows/
│   │   └── appsettings.json             # App configuration
│   │
│   └── LunarCalendar.Api/               # Optional backend API
│       ├── Controllers/
│       │   └── CalendarController.cs
│       ├── Services/
│       │   ├── ILunarCalendarService.cs
│       │   └── LunarCalendarService.cs
│       ├── Models/
│       ├── DTOs/
│       ├── Data/
│       ├── Middleware/
│       ├── Program.cs
│       └── appsettings.json
│
├── docs/                                # Documentation
│   ├── development-roadmap.md
│   ├── PRODUCT_SPECIFICATION.md
│   ├── TECHNICAL_ARCHITECTURE.md
│   └── ...
└── README.md
```

---

## 3. Core Algorithms

### 3.1 Lunar Date Conversion Algorithm

**Class:** `LunarCalculationService`  
**Location:** `LunarCalendar.Core/Services/LunarCalculationService.cs`

#### Algorithm Overview

```csharp
public LunarDate ConvertToLunar(DateTime gregorianDate)
{
    // 1. Initialize Chinese Lunisolar Calendar
    var chineseCalendar = new ChineseLunisolarCalendar();
    
    // 2. Extract lunar components
    var lunarYear = chineseCalendar.GetYear(gregorianDate);
    var lunarMonth = chineseCalendar.GetMonth(gregorianDate);
    var lunarDay = chineseCalendar.GetDayOfMonth(gregorianDate);
    var isLeapMonth = chineseCalendar.IsLeapMonth(lunarYear, lunarMonth);
    
    // 3. Adjust display month (leap month handling)
    var displayMonth = lunarMonth;
    if (isLeapMonth)
    {
        displayMonth = lunarMonth - 1;
    }
    else
    {
        var leapMonth = chineseCalendar.GetLeapMonth(lunarYear);
        if (leapMonth > 0 && lunarMonth > leapMonth)
        {
            displayMonth = lunarMonth - 1;
        }
    }
    
    // 4. Calculate sexagenary cycle (60-year cycle)
    var heavenlyStem = HeavenlyStems[(lunarYear - 4) % 10];
    var earthlyBranch = EarthlyBranches[(lunarYear - 4) % 12];
    var animalSign = AnimalSigns[(lunarYear - 4) % 12];
    
    // 5. Format names
    var lunarMonthName = isLeapMonth 
        ? $"Tháng Nhuận {LunarMonthNames[displayMonth - 1]}" 
        : LunarMonthNames[displayMonth - 1];
    var lunarDayName = LunarDayNames[lunarDay - 1];
    
    // 6. Return result
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
```

#### Key Concepts

**1. ChineseLunisolarCalendar**
- Built-in .NET Framework class
- Implements traditional Chinese lunar calendar
- Valid range: 1901-02-19 to 2101-01-28
- Handles leap months automatically

**2. Leap Month Handling**
```
Lunar Year Structure:
- Normal year: 12 months
- Leap year: 13 months (one month repeated)

Example Leap Year:
Month sequence: 1, 2, 3, 3*, 4, 5, 6, 7, 8, 9, 10, 11, 12
(3* is the leap month)

Display Logic:
- ChineseLunisolarCalendar returns month=4 for leap 3rd month
- We adjust: if IsLeapMonth, display = month - 1
- Result: Display "Tháng Nhuận Ba" (Leap 3rd Month)
```

**3. Sexagenary Cycle (60-Year Cycle)**
```
Combination of:
- 10 Heavenly Stems (Thiên Can)
- 12 Earthly Branches (Địa Chi)

Calculation:
Year 2024 → Index = (2024 - 4) = 2020
Stem = 2020 % 10 = 0 → "Giáp" (甲)
Branch = 2020 % 12 = 4 → "Thìn" (辰)
Result: "Giáp Thìn năm" (Year of the Dragon)

Offset -4: Aligns with traditional starting point
```

**4. Animal Signs**
```
12 animals repeating in 12-year cycles:
Index = (lunarYear - 4) % 12

0: Rat (Tý)      6: Horse (Ngọ)
1: Ox (Sửu)      7: Goat (Mùi)
2: Tiger (Dần)   8: Monkey (Thân)
3: Rabbit (Mão)  9: Rooster (Dậu)
4: Dragon (Thìn) 10: Dog (Tuất)
5: Snake (Tỵ)    11: Pig (Hợi)
```

---

### 3.2 Reverse Conversion (Lunar to Gregorian)

```csharp
public DateTime ConvertToGregorian(int year, int month, int day, bool isLeapMonth = false)
{
    var chineseCalendar = new ChineseLunisolarCalendar();
    var leapMonth = chineseCalendar.GetLeapMonth(year);
    
    // Adjust month index
    var adjustedMonth = month;
    if (isLeapMonth && leapMonth == month)
    {
        adjustedMonth = month; // Use actual leap month
    }
    else if (leapMonth > 0 && month >= leapMonth && !isLeapMonth)
    {
        adjustedMonth = month + 1; // Skip leap month
    }
    
    return chineseCalendar.ToDateTime(year, adjustedMonth, day, 0, 0, 0, 0);
}
```

**Complexity:**
- Leap month logic requires careful adjustment
- Must check if leap month exists for that year
- Month numbering differs between API and calendar

---

### 3.3 Holiday Calculation Algorithm

**Class:** `HolidayCalculationService`  
**Location:** `LunarCalendar.Core/Services/HolidayCalculationService.cs`

#### Get Holidays for Year

```csharp
public List<HolidayOccurrence> GetHolidaysForYear(int year)
{
    var occurrences = new List<HolidayOccurrence>();
    
    // Calculate lunar year range for this Gregorian year
    var startOfYear = new DateTime(year, 1, 1);
    var endOfYear = new DateTime(year, 12, 31);
    
    var startLunarYear = _lunarCalculationService.ConvertToLunar(startOfYear).LunarYear;
    var endLunarYear = _lunarCalculationService.ConvertToLunar(endOfYear).LunarYear;
    
    foreach (var holiday in _holidays)
    {
        if (holiday.HasLunarDate)
        {
            // Process lunar-based holiday
            // May need to check 2 lunar years (year transitions)
            for (int lunarYear = startLunarYear; lunarYear <= endLunarYear; lunarYear++)
            {
                TryAddLunarHoliday(occurrences, holiday, lunarYear, year);
            }
        }
        else
        {
            // Process Gregorian-based holiday
            var gregorianDate = new DateTime(year, 
                holiday.GregorianMonth!.Value, 
                holiday.GregorianDay!.Value);
            
            var lunarInfo = _lunarCalculationService.ConvertToLunar(gregorianDate);
            
            occurrences.Add(new HolidayOccurrence
            {
                Holiday = holiday,
                GregorianDate = gregorianDate,
                AnimalSign = lunarInfo.AnimalSign
            });
        }
    }
    
    return occurrences.OrderBy(h => h.GregorianDate).ToList();
}
```

#### Lunar Holiday Resolution

```csharp
private void TryAddLunarHoliday(
    List<HolidayOccurrence> occurrences, 
    Holiday holiday, 
    int lunarYear, 
    int targetGregorianYear)
{
    try
    {
        // Convert lunar date to Gregorian
        var gregorianDate = _lunarCalculationService.ConvertToGregorian(
            lunarYear, 
            holiday.LunarMonth, 
            holiday.LunarDay, 
            holiday.IsLeapMonth
        );
        
        // Only add if it falls within target Gregorian year
        if (gregorianDate.Year == targetGregorianYear)
        {
            var lunarInfo = _lunarCalculationService.ConvertToLunar(gregorianDate);
            
            occurrences.Add(new HolidayOccurrence
            {
                Holiday = holiday,
                GregorianDate = gregorianDate,
                AnimalSign = lunarInfo.AnimalSign
            });
        }
    }
    catch (Exception)
    {
        // Skip holidays that can't be converted
        // (e.g., leap month doesn't exist in this year)
    }
}
```

**Key Challenge:**
- Lunar New Year can fall in January or February
- A Gregorian year may span 2 lunar years
- Must check both lunar years to find all occurrences

**Example:**
```
Gregorian Year 2025:
- Jan 1, 2025 → Lunar Year 2024
- Jan 29, 2025 → Lunar Year 2024 (12th month)
- Jan 30, 2025 → Lunar Year 2025 (1st month - Tết!)
- Dec 31, 2025 → Lunar Year 2025 (11th month)

Solution: Check holidays for both Lunar 2024 and 2025
```

---

### 3.4 Lunar Special Days Generation

```csharp
public List<HolidayOccurrence> GetLunarSpecialDays(int year)
{
    var specialDays = new List<HolidayOccurrence>();
    
    // Get lunar year range
    var startLunarYear = ConvertToLunar(new DateTime(year, 1, 1)).LunarYear;
    var endLunarYear = ConvertToLunar(new DateTime(year, 12, 31)).LunarYear;
    
    for (int lunarYear = startLunarYear; lunarYear <= endLunarYear; lunarYear++)
    {
        // Generate for each month (1-12)
        for (int month = 1; month <= 12; month++)
        {
            // 1st day (Mùng 1)
            TryAddSpecialDay(specialDays, lunarYear, month, 1, year, "Mùng 1");
            
            // 15th day (Rằm - Full Moon)
            TryAddSpecialDay(specialDays, lunarYear, month, 15, year, "Rằm");
        }
    }
    
    return specialDays.OrderBy(s => s.GregorianDate).ToList();
}
```

**Purpose:**
- Highlight important lunar dates (new moon and full moon)
- Traditional days for temple visits, offerings
- 24 special days per lunar year

---

## 4. Data Architecture

### 4.1 Data Flow Diagram

```
User Action (e.g., "View January 2025")
    │
    ▼
┌─────────────────────────┐
│   CalendarViewModel      │ ← User interaction layer
└────────────┬────────────┘
             │ Request month data
             ▼
┌─────────────────────────┐
│   CalendarService        │ ← Orchestration layer
└────────────┬────────────┘
             │
             ├──→ Check SQLite Cache
             │    └─→ If cached, return immediately
             │
             ├──→ If not cached:
             │    └─→ Call LunarCalculationService
             │        ├─→ Calculate lunar dates
             │        └─→ Cache in SQLite
             │
             └──→ Call HolidayCalculationService
                  ├─→ Get holidays for dates
                  └─→ Return holiday occurrences
    │
    ▼
┌─────────────────────────┐
│   CalendarViewModel      │ ← Update ObservableCollection
└────────────┬────────────┘
             │
             ▼
┌─────────────────────────┐
│   CalendarPage (View)    │ ← UI renders data
└─────────────────────────┘
```

---

### 4.2 Database Schema

**SQLite Database: `lunarcalendar.db3`**

#### Table: LunarDateEntity

```sql
CREATE TABLE LunarDateEntity (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    GregorianDate TEXT NOT NULL UNIQUE,  -- ISO 8601 format
    LunarYear INTEGER NOT NULL,
    LunarMonth INTEGER NOT NULL,
    LunarDay INTEGER NOT NULL,
    IsLeapMonth INTEGER NOT NULL,        -- SQLite boolean (0/1)
    LunarYearName TEXT NOT NULL,
    LunarMonthName TEXT NOT NULL,
    LunarDayName TEXT NOT NULL,
    AnimalSign TEXT NOT NULL,
    CreatedAt TEXT DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_gregorian_date 
    ON LunarDateEntity(GregorianDate);
```

**C# Entity:**

```csharp
[Table("LunarDateEntity")]
public class LunarDateEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    [Unique]
    public DateTime GregorianDate { get; set; }
    
    public int LunarYear { get; set; }
    public int LunarMonth { get; set; }
    public int LunarDay { get; set; }
    public bool IsLeapMonth { get; set; }
    public string LunarYearName { get; set; } = string.Empty;
    public string LunarMonthName { get; set; } = string.Empty;
    public string LunarDayName { get; set; } = string.Empty;
    public string AnimalSign { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

**Usage Pattern:**

```csharp
// Write (cache)
public async Task SaveLunarDateAsync(LunarDate lunarDate)
{
    var entity = new LunarDateEntity
    {
        GregorianDate = lunarDate.GregorianDate.Date,
        LunarYear = lunarDate.LunarYear,
        // ... map all properties
    };
    
    await _connection.InsertOrReplaceAsync(entity);
}

// Read (retrieve from cache)
public async Task<LunarDate?> GetCachedLunarDateAsync(DateTime date)
{
    var entity = await _connection
        .Table<LunarDateEntity>()
        .Where(e => e.GregorianDate == date.Date)
        .FirstOrDefaultAsync();
    
    return entity != null ? MapToLunarDate(entity) : null;
}
```

---

### 4.3 In-Memory Data Structures

#### Holiday Data (Embedded)

```csharp
// Loaded once at startup, kept in memory
private static readonly List<Holiday> _holidays = HolidaySeeder.GetVietnameseLunarHolidays();

// 45+ holidays preloaded:
// - Lunar-based: Tết, Mid-Autumn, etc.
// - Gregorian-based: National holidays
// - Lunar special days: 1st & 15th generated dynamically
```

**Memory footprint:** ~10-15 KB (negligible)

---

#### ViewModel State

```csharp
public class CalendarViewModel : BaseViewModel
{
    // Observable collections for data binding
    public ObservableCollection<CalendarDay> CalendarDays { get; set; }
    public ObservableCollection<HolidayOccurrence> UpcomingHolidays { get; set; }
    
    // Current view state
    [ObservableProperty]
    private DateTime _currentMonth;
    
    [ObservableProperty]
    private LunarDate? _todayLunarDate;
    
    // Computed properties
    public string MonthYearDisplay => 
        _localizationService.FormatMonthYear(_currentMonth);
}
```

**Data Binding:**
- Properties decorated with `[ObservableProperty]` auto-generate `OnPropertyChanged`
- ObservableCollection automatically notifies UI of changes
- No manual UI update code needed

---

## 5. Service Layer

### 5.1 Service Responsibilities

#### Core Services (in LunarCalendar.Core)

**ILunarCalculationService**
```csharp
public interface ILunarCalculationService
{
    LunarDate ConvertToLunar(DateTime gregorianDate);
    DateTime ConvertToGregorian(int year, int month, int day, bool isLeapMonth = false);
    List<LunarDate> GetMonthInfo(int year, int month);
}
```
- **Responsibility**: Pure lunar calendar calculations
- **Dependencies**: None (uses System.Globalization)
- **Stateless**: No internal state
- **Thread-safe**: Yes

---

**IHolidayCalculationService**
```csharp
public interface IHolidayCalculationService
{
    List<Holiday> GetAllHolidays();
    List<HolidayOccurrence> GetHolidaysForYear(int year);
    List<HolidayOccurrence> GetHolidaysForMonth(int year, int month);
    Holiday? GetHolidayForDate(DateTime date);
}
```
- **Responsibility**: Holiday calculations and lookups
- **Dependencies**: ILunarCalculationService
- **Stateless**: Holiday data is static
- **Thread-safe**: Yes (read-only data)

---

#### App Services (in LunarCalendar.MobileApp)

**ICalendarService**
```csharp
public interface ICalendarService
{
    Task<LunarDate?> GetLunarDateAsync(DateTime date);
    Task<List<LunarDate>> GetMonthLunarDatesAsync(int year, int month);
}
```
- **Responsibility**: Calendar data with caching
- **Dependencies**: ILunarCalculationService, LunarCalendarDatabase
- **Caching**: Writes to SQLite asynchronously
- **Thread-safe**: Uses async/await

**Implementation:**
```csharp
public async Task<LunarDate?> GetLunarDateAsync(DateTime date)
{
    // 1. Try cache first (fast path)
    var cached = await _database.GetLunarDateAsync(date);
    if (cached != null) return cached;
    
    // 2. Calculate (instant, no network)
    var lunarDate = _lunarCalculationService.ConvertToLunar(date);
    
    // 3. Cache in background (fire-and-forget)
    _ = Task.Run(async () => await _database.SaveLunarDateAsync(lunarDate));
    
    return lunarDate;
}
```

---

**IHolidayService**
```csharp
public interface IHolidayService
{
    Task<List<HolidayOccurrence>> GetHolidaysForYearAsync(int year);
    Task<List<HolidayOccurrence>> GetHolidaysForMonthAsync(int year, int month);
    Task<List<HolidayOccurrence>> GetUpcomingHolidaysAsync(int count = 5);
    Task<List<Holiday>> GetAllHolidaysAsync();
}
```
- **Responsibility**: Holiday data for UI
- **Dependencies**: IHolidayCalculationService
- **Caching**: In-memory (holidays don't change)
- **Filtering**: By type, date range

---

**ILocalizationService**
```csharp
public interface ILocalizationService
{
    CultureInfo CurrentCulture { get; }
    void SetCulture(string cultureName);
    string GetString(string key);
    event EventHandler CultureChanged;
}
```
- **Responsibility**: Language/culture management
- **Dependencies**: AppResources.resx files
- **State**: Current culture stored in Preferences
- **Events**: Notifies ViewModels of language changes

**Implementation:**
```csharp
public void SetCulture(string cultureName)
{
    var culture = new CultureInfo(cultureName);
    CultureInfo.CurrentCulture = culture;
    CultureInfo.CurrentUICulture = culture;
    AppResources.Culture = culture;
    
    // Persist
    Preferences.Set("AppLanguage", cultureName);
    
    // Notify
    CultureChanged?.Invoke(this, EventArgs.Empty);
}
```

---

**IUserModeService**
```csharp
public interface IUserModeService
{
    UserMode CurrentMode { get; }
    bool IsGuestMode { get; }
    bool IsAuthenticatedMode { get; }
    void SetGuestMode();
    void SetAuthenticatedMode(string userId);
}
```
- **Responsibility**: Track user authentication state
- **Dependencies**: Preferences
- **State**: Persisted across app launches
- **Note**: MVP uses guest mode only

---

**IHapticService**
```csharp
public interface IHapticService
{
    void Trigger(HapticFeedbackType type = HapticFeedbackType.Click);
}

public enum HapticFeedbackType
{
    Click,      // Light tap
    Success,    // Success feedback
    Warning,    // Warning feedback
    Error       // Error feedback
}
```
- **Responsibility**: Tactile feedback
- **Platform-specific**: Uses MAUI HapticFeedback
- **iOS**: Haptic Engine
- **Android**: Vibration

---

### 5.2 Dependency Injection Setup

**File:** `MauiProgram.cs`

```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    
    // Register Database (Singleton - shared instance)
    var dbPath = Path.Combine(FileSystem.AppDataDirectory, "lunarcalendar.db3");
    builder.Services.AddSingleton(sp => new LunarCalendarDatabase(dbPath));
    
    // Register Core Services (Singleton - stateless, thread-safe)
    builder.Services.AddSingleton<ILunarCalculationService, LunarCalculationService>();
    builder.Services.AddSingleton<IHolidayCalculationService, HolidayCalculationService>();
    
    // Register App Services (Singleton - long-lived)
    builder.Services.AddSingleton<ICalendarService, CalendarService>();
    builder.Services.AddSingleton<IHolidayService, HolidayService>();
    builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
    builder.Services.AddSingleton<IUserModeService, UserModeService>();
    builder.Services.AddSingleton<IHapticService, HapticService>();
    
    // Register ViewModels (Transient - new instance per page)
    builder.Services.AddTransient<CalendarViewModel>();
    builder.Services.AddTransient<YearHolidaysViewModel>();
    builder.Services.AddTransient<HolidayDetailViewModel>();
    builder.Services.AddTransient<SettingsViewModel>();
    
    // Register Views (Transient - new instance per navigation)
    builder.Services.AddTransient<CalendarPage>();
    builder.Services.AddTransient<YearHolidaysPage>();
    builder.Services.AddTransient<HolidayDetailPage>();
    builder.Services.AddTransient<SettingsPage>();
    
    return builder.Build();
}
```

**Lifetime Strategies:**
- **Singleton**: Services, Database (one instance for app lifetime)
- **Transient**: ViewModels, Views (new instance each time)
- **Scoped**: Not used in this app (no per-request scope in mobile)

---

## 6. API Design (Optional Backend)

### 6.1 API Architecture

**Note:** API is optional for MVP. Mobile app works 100% offline.

**Framework:** ASP.NET Core 8.0  
**Pattern:** RESTful API with OpenAPI (Swagger)  
**Authentication:** JWT (for future user features)  
**Database:** PostgreSQL (for user data, events)

---

### 6.2 API Endpoints

#### Calendar Endpoints

**GET /api/v1/calendar/convert**
```
Description: Convert Gregorian date to lunar date
Query Parameters:
  - date: DateTime (optional, defaults to today)

Response: 200 OK
{
  "gregorianDate": "2026-01-03T00:00:00Z",
  "lunarYear": 2025,
  "lunarMonth": 12,
  "lunarDay": 4,
  "isLeapMonth": false,
  "lunarYearName": "Ất Tỵ năm",
  "lunarMonthName": "Tháng Chạp",
  "lunarDayName": "Mùng 4",
  "animalSign": "Snake",
  "heavenlyStem": "Ất",
  "earthlyBranch": "Tỵ"
}
```

---

**GET /api/v1/calendar/month/{year}/{month}**
```
Description: Get all dates for a month with lunar info
Path Parameters:
  - year: int (Gregorian year)
  - month: int (1-12)

Response: 200 OK
[
  {
    "gregorianDate": "2026-01-01T00:00:00Z",
    "lunarYear": 2025,
    "lunarMonth": 11,
    "lunarDay": 12,
    // ... full lunar info
  },
  // ... 28-31 dates
]
```

---

#### Holiday Endpoints

**GET /api/v1/holidays/year/{year}**
```
Description: Get all holidays for a year
Path Parameters:
  - year: int

Response: 200 OK
[
  {
    "holiday": {
      "id": 1,
      "name": "Tết Nguyên Đán",
      "description": "Lunar New Year...",
      "type": "MajorHoliday",
      "colorHex": "#DC143C",
      "isPublicHoliday": true
    },
    "gregorianDate": "2026-02-17T00:00:00Z",
    "animalSign": "Horse"
  },
  // ... all holidays
]
```

---

**GET /api/v1/holidays**
```
Description: Get holiday definitions (static data)

Response: 200 OK
[
  {
    "id": 1,
    "name": "Tết Nguyên Đán",
    "description": "Lunar New Year - The most important Vietnamese holiday",
    "lunarMonth": 1,
    "lunarDay": 1,
    "type": "MajorHoliday",
    "colorHex": "#DC143C",
    "isPublicHoliday": true
  },
  // ... all holiday definitions
]
```

---

### 6.3 API Response Caching

```csharp
[HttpGet("convert")]
[ResponseCache(Duration = 3600, VaryByQueryKeys = new[] { "date" })]
public ActionResult<LunarDateResponse> ConvertToLunar([FromQuery] DateTime? date = null)
{
    // Cache for 1 hour (lunar dates don't change)
    // ...
}

[HttpGet("month/{year}/{month}")]
[ResponseCache(Duration = 86400, VaryByQueryKeys = new[] { "year", "month" })]
public ActionResult<List<LunarDateInfo>> GetMonthInfo(int year, int month)
{
    // Cache for 24 hours
    // ...
}
```

---

### 6.4 API Versioning

```csharp
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CalendarController : ControllerBase
{
    // Version in URL: /api/v1/calendar/...
}
```

**Benefits:**
- Backward compatibility
- Gradual migration
- Clear versioning in URLs

---

## 7. Design Patterns

### 7.1 MVVM (Model-View-ViewModel)

**Pattern Overview:**
```
View (XAML)
  ↕ Data Binding
ViewModel (C#)
  ↕ Business Logic
Model (Data)
```

**Benefits:**
- Separation of concerns
- Testable business logic
- Reusable ViewModels
- No code-behind (or minimal)

**Example:**

```csharp
// ViewModel
public partial class CalendarViewModel : BaseViewModel
{
    [ObservableProperty]
    private DateTime _currentMonth = DateTime.Today;
    
    [ObservableProperty]
    private ObservableCollection<CalendarDay> _calendarDays = new();
    
    [RelayCommand]
    private async Task NextMonth()
    {
        CurrentMonth = CurrentMonth.AddMonths(1);
        await LoadMonthAsync();
    }
}
```

```xml
<!-- View (XAML) -->
<ContentPage>
    <Label Text="{Binding MonthYearDisplay}" />
    <CollectionView ItemsSource="{Binding CalendarDays}" />
    <Button Text="Next" Command="{Binding NextMonthCommand}" />
</ContentPage>
```

---

### 7.2 Repository Pattern (Simplified)

**Pattern:** Abstraction over data access

```csharp
// Interface
public interface ILunarCalendarDatabase
{
    Task<LunarDate?> GetLunarDateAsync(DateTime date);
    Task SaveLunarDateAsync(LunarDate lunarDate);
}

// Implementation
public class LunarCalendarDatabase : ILunarCalendarDatabase
{
    private readonly SQLiteAsyncConnection _connection;
    
    public async Task<LunarDate?> GetLunarDateAsync(DateTime date)
    {
        var entity = await _connection
            .Table<LunarDateEntity>()
            .FirstOrDefaultAsync(e => e.GregorianDate == date.Date);
        
        return entity != null ? MapToModel(entity) : null;
    }
}
```

**Benefits:**
- Swap implementations (SQLite, API, mock)
- Testable data access
- Centralized query logic

---

### 7.3 Service Locator (via DI)

**Pattern:** Centralized service access

```csharp
// In ViewModel constructor
public CalendarViewModel(
    ICalendarService calendarService,
    IHolidayService holidayService,
    ILocalizationService localizationService)
{
    _calendarService = calendarService;
    _holidayService = holidayService;
    _localizationService = localizationService;
}

// Services resolved automatically by DI container
```

**Alternative (discouraged):**
```csharp
// Manual service location (not used in this app)
var service = ServiceLocator.Current.GetService<ICalendarService>();
```

---

### 7.4 Observer Pattern (via Events)

**Pattern:** Notify observers of state changes

```csharp
// Publisher
public class LocalizationService : ILocalizationService
{
    public event EventHandler? CultureChanged;
    
    public void SetCulture(string cultureName)
    {
        // Change culture
        // ...
        
        // Notify observers
        CultureChanged?.Invoke(this, EventArgs.Empty);
    }
}

// Subscriber
public class CalendarViewModel : BaseViewModel
{
    public CalendarViewModel(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
        _localizationService.CultureChanged += OnCultureChanged;
    }
    
    private void OnCultureChanged(object? sender, EventArgs e)
    {
        // Reload data in new language
        await LoadMonthAsync();
    }
}
```

---

### 7.5 Strategy Pattern (for Date Formatting)

**Pattern:** Encapsulate algorithms

```csharp
public static class DateFormatterHelper
{
    public static string FormatDate(DateTime date, CultureInfo culture)
    {
        return culture.Name switch
        {
            "vi-VN" => FormatVietnamese(date),
            "en-US" => FormatEnglish(date),
            _ => date.ToString("D", culture)
        };
    }
    
    private static string FormatVietnamese(DateTime date)
    {
        return $"Thứ {GetVietnameseDayOfWeek(date)}, " +
               $"{date.Day:D2} Tháng {date.Month}, {date.Year}";
    }
    
    private static string FormatEnglish(DateTime date)
    {
        return date.ToString("dddd, MMMM d, yyyy", 
            CultureInfo.GetCultureInfo("en-US"));
    }
}
```

---

## 8. Platform Architecture

### 8.1 Cross-Platform Strategy

**.NET MAUI** provides:
1. **Single codebase** for multiple platforms
2. **Native UI controls** (not web view)
3. **Platform-specific code** when needed

**Code Sharing:**
```
┌─────────────────────────────────────┐
│     Shared Code (95%)                │
│  - ViewModels                        │
│  - Services                          │
│  - Models                            │
│  - Business Logic                    │
│  - XAML Views                        │
└─────────────────────────────────────┘
           │
           ├──→ ┌──────────────────┐
           │    │  iOS (5%)        │
           │    │  - Info.plist    │
           │    │  - Entitlements  │
           │    └──────────────────┘
           │
           ├──→ ┌──────────────────┐
           │    │  Android (5%)    │
           │    │  - Manifest      │
           │    │  - MainActivity  │
           │    └──────────────────┘
           │
           └──→ ┌──────────────────┐
                │  macOS (5%)      │
                │  - Info.plist    │
                └──────────────────┘
```

---

### 8.2 Platform-Specific Code

**Conditional Compilation:**

```csharp
public class HapticService : IHapticService
{
    public void Trigger(HapticFeedbackType type = HapticFeedbackType.Click)
    {
#if IOS
        // iOS-specific haptic feedback
        var impactStyle = type switch
        {
            HapticFeedbackType.Success => UIKit.UIImpactFeedbackStyle.Medium,
            HapticFeedbackType.Warning => UIKit.UIImpactFeedbackStyle.Heavy,
            _ => UIKit.UIImpactFeedbackStyle.Light
        };
        
        var impact = new UIKit.UIImpactFeedbackGenerator(impactStyle);
        impact.ImpactOccurred();
#elif ANDROID
        // Android vibration
        HapticFeedback.Perform(HapticFeedbackType.Click);
#endif
    }
}
```

---

**Platform Folders:**

```
Platforms/
├── Android/
│   ├── MainActivity.cs           # Android entry point
│   ├── AndroidManifest.xml       # Permissions, config
│   └── Resources/
│       └── values/
│           └── colors.xml        # Android colors
├── iOS/
│   ├── AppDelegate.cs            # iOS entry point
│   ├── Info.plist                # iOS config
│   └── Entitlements.plist        # Capabilities
├── MacCatalyst/
│   └── Info.plist                # macOS config
└── Windows/
    └── app.manifest               # Windows config
```

---

### 8.3 Resource Management

**Platform Assets:**

```
Resources/
├── AppIcon/
│   └── appicon.svg               # Vector icon (auto-resized)
├── Splash/
│   └── splash.svg                # Splash screen
├── Images/
│   ├── calendar_icon.png         # Rasterized images
│   └── holiday_icon.png
├── Fonts/
│   ├── OpenSans-Regular.ttf
│   └── OpenSans-Semibold.ttf
└── Strings/
    ├── AppResources.resx         # English
    └── AppResources.vi.resx      # Vietnamese
```

**MAUI Handles:**
- Automatic image resizing for different densities
- Font embedding across platforms
- Localization resource loading
- Platform-specific asset variants

---

### 8.4 Build Configuration

**Release Build Settings:**

```xml
<!-- Android Release -->
<PropertyGroup Condition="'$(Configuration)' == 'Release' AND '$(TargetFramework)' == 'net8.0-android'">
  <AndroidLinkMode>Full</AndroidLinkMode>         <!-- Aggressive linking -->
  <AndroidLinkTool>r8</AndroidLinkTool>           <!-- R8 optimizer -->
  <AndroidPackageFormat>aab</AndroidPackageFormat> <!-- App Bundle -->
  <EnableLLVM>true</EnableLLVM>                   <!-- LLVM optimization -->
</PropertyGroup>

<!-- iOS Release -->
<PropertyGroup Condition="'$(Configuration)' == 'Release' AND '$(TargetFramework)' == 'net8.0-ios'">
  <MtouchLink>Full</MtouchLink>                   <!-- Full linker -->
  <MtouchEnableSGenConc>true</MtouchEnableSGenConc> <!-- Concurrent GC -->
</PropertyGroup>
```

**Optimization:**
- Dead code elimination
- IL trimming
- AOT compilation (iOS)
- R8 optimization (Android)

---

## Conclusion

This architecture provides:
- **Offline-first** functionality (no API required for MVP)
- **Cross-platform** support with native performance
- **Maintainable** structure with clear separation of concerns
- **Testable** business logic through service abstraction
- **Extensible** design for future enhancements

**Key Strengths:**
1. Bundled calculation logic = instant, offline operation
2. MVVM pattern = testable, maintainable code
3. Service layer = clear responsibilities
4. ChineseLunisolarCalendar = accurate, proven calculations

**Technology-Agnostic Concepts:**
- All patterns (MVVM, Repository, etc.) work in Flutter, React Native, etc.
- Core algorithms can be ported to any language
- Data models are universal
- API design follows REST best practices

---

**Document End**
