# Implementation Plan: Sexagenary Cycle Foundation (REVISED)

**Branch**: `feature/001-sexagenary-cycle` | **Date**: January 26, 2026 (REVISED) | **Spec**: [001-sexagenary-cycle-foundation.md](./001-sexagenary-cycle-foundation.md)

---

## 🔄 Revision Notes

**Original Plan Date**: January 25, 2026  
**Revised Date**: January 26, 2026  
**Reason for Revision**: 
1. Discovered info tooltips not needed (removed T049-T050, T053-T055)
2. Calendar cells too small for stem-branch display (removed T062-T074)
3. DateDetailPage doesn't exist yet but is assumed to exist in original tasks
4. Need cleaner scope focusing on practical UI implementation

**Status After Partial Implementation**:
- ✅ Phase 1: Setup (100% complete)
- ⚠️ Phase 2: Foundation (90% complete - **missing historical validation tests**)
- ⚠️ Phase 3: User Story 1 (33% complete - partial implementation, no tests)
- ❌ Phases 4-11: Not started

**Key Changes**:
- Remove calendar cell stem-branch display (too small, not practical)
- Add DateDetailPage creation (new page needed for detailed info)
- Focus on header display + detail page (cleaner UX)
- Simplify scope for faster delivery

---

## Summary

Implement the traditional Chinese/Vietnamese Sexagenary Cycle (Can Chi / 干支) calculation and display system with a **practical, focused scope**. Display today's stem-branch in the calendar header, and show full stem-branch information (day, month, year, hour) in a new **DateDetailPage** when users tap on any calendar date.

**Technical Approach**: 
1. Core calculation engine already implemented (SexagenaryService)
2. Display today's stem-branch in calendar header ✅ **DONE**
3. Create new DateDetailPage for showing full date information 🆕 **NEW**
4. Add stem-branch information to DateDetailPage (day, month, year, optional hour)
5. Skip calendar cell display (too small, not practical) ❌ **REMOVED**

**Key Decision**: **DateDetailPage as Central Hub**
- Calendar cells remain clean with just Gregorian + Lunar dates
- Users tap any date → navigate to DateDetailPage
- DateDetailPage shows comprehensive info: Gregorian, Lunar, Stem-Branch, Elements, Zodiac
- Better UX than cramming data into tiny calendar cells

---

## Technical Context

**Language/Version**: C# 12 with .NET 10.0  
**Framework**: .NET MAUI (Multi-platform App UI)  
**Primary Dependencies**: 
- System.Globalization.ChineseLunisolarCalendar (built-in)
- CommunityToolkit.Mvvm 8.2.2 (MVVM helpers)
- System.Text.Json (for JSON data loading)

**Storage**: Calculation results are ephemeral (not persisted); JSON files for zodiac data  
**Testing**: xUnit with FluentAssertions for unit tests  
**Target Platform**: iOS 15.0+, Android API 26+ (cross-platform mobile)  
**Project Type**: Mobile application with shared Core library  

**Performance Goals**: 
- < 10ms per single date calculation
- < 100ms to calculate full month (30 dates) with caching
- 60 FPS scrolling performance maintained

**Constraints**: 
- Offline-first: All calculations must work without network
- Date range: 1901-2100 (ChineseLunisolarCalendar limitation)
- Cultural accuracy: Validated against historical references
- Bilingual: Vietnamese and English support (Chinese deferred to Sprint 14)

**Scale/Scope (REVISED)**: 
- ~10 classes in Core library ✅ **DONE**
- ~2 ViewModels updated ⚠️ **IN PROGRESS**
- ~1 new DateDetailPage 🆕 **NEW**
- ~3 XAML views modified ⚠️ **IN PROGRESS**
- ~50 unit tests for calculation accuracy ⚠️ **NEEDS COMPLETION**
- Historical validation (1000+ dates) ❌ **CRITICAL - NOT DONE**

---

## Constitution Check

✅ **I. Offline-First Architecture**: COMPLIANT  
All stem-branch calculations execute on-device using mathematical algorithms. No network dependency.

✅ **II. Cultural Accuracy & Authenticity**: COMPLIANT  
Algorithm validated against Vietnamese lunar calendar. Historical validation tests REQUIRED (not yet run).

✅ **III. Privacy & Guest-First Design**: COMPLIANT  
No authentication required. Feature fully accessible to guest users. No data collection.

✅ **IV. Cross-Platform Consistency**: COMPLIANT  
Shared calculation logic in LunarCalendar.Core. iOS and Android tested.

✅ **V. Performance & Responsiveness**: COMPLIANT  
Calculation caching ensures < 10ms per date. Target met in existing implementation.

✅ **VI. Bilingual Support**: COMPLIANT  
Vietnamese and English stem/branch names implemented. Chinese deferred to Sprint 14.

⚠️ **VII. Test Coverage & Quality Assurance**: NEEDS COMPLETION  
- Unit tests exist but **historical validation NOT RUN** (CRITICAL)
- Target: 1000+ historical dates with 100% accuracy
- Current: Only basic unit tests, no comprehensive validation

**Gate Result**: ⚠️ **CONDITIONAL PASS** - Proceed with REVISED scope, but MUST complete historical validation before production.

---

## Revised Scope: Sprint 9 MVP

### What's IN SCOPE ✅

#### Core Calculation (Phase 2) - 90% Complete
- ✅ SexagenaryService with day/month/year/hour calculations
- ✅ Caching with LRU eviction
- ✅ Five Element and Zodiac mappings
- ❌ **Historical validation tests (1000+ dates)** - **MUST DO**

#### User Story 1: Today's Stem-Branch Display (Phase 3) - 33% Complete
- ✅ Display in calendar header (CalendarPage)
- ✅ Element color indicator
- ✅ Multi-language support (Vietnamese, English)
- ❌ Language change handling - **NEEDS VERIFICATION**
- ❌ Unit tests for User Story 1 - **MUST DO**

#### User Story 2: Date Detail Page (Phase 4) - 0% Complete 🆕 **REVISED APPROACH**
- ❌ **Create DateDetailPage** (new page, doesn't exist yet)
- ❌ **Create DateDetailViewModel**
- ❌ **Add navigation from calendar cells** (tap gesture)
- ❌ **Display comprehensive date information**:
  - Gregorian date (large, prominent)
  - Lunar date with year
  - Day stem-branch with element and zodiac
  - Month stem-branch (optional)
  - Year stem-branch (optional)
- ❌ **Register route and handle navigation**

#### User Story 3: Year Zodiac Header (Phase 5) - 0% Complete (OPTIONAL)
- Display current year's zodiac in header
- Simple year indicator (e.g., "2026 - Year of Fire Horse")
- Can be done later if time permits

### What's OUT OF SCOPE ❌

#### Removed from Sprint 9:
- ❌ **Info tooltips** (T049-T050, T053-T055) - Not needed per user feedback
- ❌ **Calendar cell stem-branch display** (T062-T074) - Too small, not practical
- ❌ **Hour stem-branch display** (Phase 6) - Defer to future sprint
- ❌ **Educational page** (Phase 7) - Defer to future sprint
- ❌ **Backend API** (Phase 8) - Optional, defer to future
- ❌ **Year zodiac detail dialog** - Defer to future sprint

#### Why Removed:
1. **Calendar cells are too small** (70pt height) to show Gregorian + Lunar + Stem-Branch without clutter
2. **Better UX with detail page** - Users tap to see full info rather than tiny text in cells
3. **Faster MVP delivery** - Focus on core value (today's display + detail page navigation)
4. **User feedback** - Info tooltips not wanted

---

## Implementation Phases (REVISED)

### Phase 1: Historical Validation (CRITICAL) 🚨
**Priority**: P0 - BLOCKING  
**Duration**: 4-6 hours  
**Status**: NOT STARTED

#### Tasks:
1. Create `HistoricalValidationTests.cs`
2. Load 1000+ dates from CSV dataset
3. Validate 100% accuracy against known historical dates
4. Test Lunar New Year boundary handling
5. Test edge cases (1901, 2100, leap months)

**Why Critical**: Cannot certify calculation accuracy without this. Required by Constitution.

**Acceptance Criteria**:
- All 1000+ dates match expected stem-branch with 100% accuracy
- Lunar New Year boundaries handled correctly
- Edge cases pass without errors
- Test execution < 10 seconds

---

### Phase 2: Complete User Story 1 (Today's Display)
**Priority**: P1 - HIGH  
**Duration**: 3-4 hours  
**Status**: 33% COMPLETE

#### Remaining Tasks:
1. **Language change handling** (T046)
   - Verify stem-branch updates when switching Vietnamese ↔ English ↔ Chinese
   - Test localization service integration
   
2. **Unit tests** (T056-T059)
   - Create CalendarViewModelTests for TodayStemBranch
   - Test language switching
   - Test offline mode
   - Test element color selection

3. **UI polish** (T051-T052)
   - Review stem-branch styling (font, size, spacing)
   - Verify element indicator works correctly
   - Test on iPhone SE, iPhone 15 Pro Max, iPad

**Acceptance Criteria**:
- Today's stem-branch displays correctly in header
- Language switching updates display immediately
- Element color indicator matches Five Element
- All unit tests pass
- Works offline without network

---

### Phase 3: Create DateDetailPage (NEW) 🆕
**Priority**: P1 - HIGH  
**Duration**: 4-6 hours  
**Status**: NOT STARTED

#### Tasks:

##### 3.1 Create Basic Page Structure (2 hours)
1. Create `DateDetailViewModel.cs`
   - Properties: SelectedDate, GregorianDateDisplay, LunarDateDisplay
   - Properties: DayStemBranch, MonthStemBranch, YearStemBranch
   - Properties: DayElement, DayElementColor, DayZodiac
   - Method: `InitializeAsync(DateTime selectedDate)`
   - Inject: ISexagenaryService, ICalendarService

2. Create `DateDetailPage.xaml` and code-behind
   - Large date header (Gregorian)
   - Lunar date section
   - Stem-branch information card
   - Back button navigation

3. Register page in DI container (`MauiProgram.cs`)
   ```csharp
   builder.Services.AddTransient<DateDetailPage>();
   builder.Services.AddTransient<DateDetailViewModel>();
   ```

4. Register route in AppShell (if using Shell routing)
   ```csharp
   Routing.RegisterRoute("datedetail", typeof(DateDetailPage));
   ```

##### 3.2 Add Navigation from Calendar Cells (1 hour)
1. Add tap gesture to calendar cell template in `CalendarPage.xaml`
   ```xml
   <Border.GestureRecognizers>
       <TapGestureRecognizer Command="{Binding Source={RelativeSource AncestorType={x:Type viewmodels:CalendarViewModel}}, Path=SelectDateCommand}"
                             CommandParameter="{Binding .}"/>
   </Border.GestureRecognizers>
   ```

2. Add command to `CalendarViewModel.cs`
   ```csharp
   [RelayCommand]
   async Task SelectDateAsync(CalendarDay calendarDay)
   {
       _hapticService.PerformClick();
       
       // Get DateDetailPage from DI
       var serviceProvider = IPlatformApplication.Current?.Services;
       var dateDetailPage = serviceProvider.GetRequiredService<Views.DateDetailPage>();
       
       // Pass selected date to ViewModel
       if (dateDetailPage.BindingContext is DateDetailViewModel viewModel)
       {
           await viewModel.InitializeAsync(calendarDay.Date);
       }
       
       // Navigate
       await Shell.Current.Navigation.PushAsync(dateDetailPage);
   }
   ```

##### 3.3 Design DateDetailPage UI (2 hours)
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="LunarCalendar.MobileApp.Views.DateDetailPage"
             Title="Date Details">
    
    <ScrollView>
        <VerticalStackLayout Padding="20" Spacing="20">
            
            <!-- Gregorian Date Header -->
            <VerticalStackLayout Spacing="4">
                <Label Text="{Binding GregorianDateDisplay}"
                       FontSize="32"
                       FontAttributes="Bold"
                       HorizontalOptions="Center"/>
                <Label Text="{Binding DayOfWeekDisplay}"
                       FontSize="18"
                       TextColor="{DynamicResource Gray600}"
                       HorizontalOptions="Center"/>
            </VerticalStackLayout>
            
            <!-- Lunar Date Card -->
            <Border BackgroundColor="{DynamicResource CardBackground}"
                    Padding="16"
                    StrokeThickness="0">
                <Border.StrokeShape>
                    <RoundRectangle CornerRadius="12"/>
                </Border.StrokeShape>
                <VerticalStackLayout Spacing="8">
                    <Label Text="🌙 Lunar Date"
                           FontSize="14"
                           FontAttributes="Bold"
                           TextColor="{DynamicResource Primary}"/>
                    <Label Text="{Binding LunarDateDisplay}"
                           FontSize="18"/>
                </VerticalStackLayout>
            </Border>
            
            <!-- Stem-Branch Card -->
            <Border BackgroundColor="{DynamicResource CardBackground}"
                    Padding="16"
                    StrokeThickness="0">
                <Border.StrokeShape>
                    <RoundRectangle CornerRadius="12"/>
                </Border.StrokeShape>
                <VerticalStackLayout Spacing="12">
                    <Label Text="干支 Stem-Branch (Can Chi)"
                           FontSize="14"
                           FontAttributes="Bold"
                           TextColor="{DynamicResource Primary}"/>
                    
                    <!-- Day Stem-Branch -->
                    <HorizontalStackLayout Spacing="8">
                        <Ellipse WidthRequest="16" 
                                 HeightRequest="16"
                                 Fill="{Binding DayElementColor}"
                                 VerticalOptions="Center"/>
                        <VerticalStackLayout Spacing="2">
                            <Label Text="Day" 
                                   FontSize="12" 
                                   TextColor="{DynamicResource Gray600}"/>
                            <Label Text="{Binding DayStemBranchDisplay}"
                                   FontSize="18"
                                   FontAttributes="Bold"/>
                            <Label Text="{Binding DayElementDisplay}"
                                   FontSize="12"
                                   TextColor="{DynamicResource Gray600}"/>
                        </VerticalStackLayout>
                    </HorizontalStackLayout>
                    
                    <!-- Month Stem-Branch -->
                    <HorizontalStackLayout Spacing="8">
                        <Label Text="Month:" 
                               FontSize="14" 
                               WidthRequest="60"/>
                        <Label Text="{Binding MonthStemBranchDisplay}"
                               FontSize="16"/>
                    </HorizontalStackLayout>
                    
                    <!-- Year Stem-Branch -->
                    <HorizontalStackLayout Spacing="8">
                        <Label Text="Year:" 
                               FontSize="14" 
                               WidthRequest="60"/>
                        <Label Text="{Binding YearStemBranchDisplay}"
                               FontSize="16"/>
                    </HorizontalStackLayout>
                </VerticalStackLayout>
            </Border>
            
            <!-- Holiday Card (if applicable) -->
            <Border BackgroundColor="{Binding HolidayColor}"
                    Padding="16"
                    StrokeThickness="0"
                    IsVisible="{Binding HasHoliday}">
                <Border.StrokeShape>
                    <RoundRectangle CornerRadius="12"/>
                </Border.StrokeShape>
                <VerticalStackLayout Spacing="8">
                    <Label Text="{Binding HolidayName}"
                           FontSize="18"
                           FontAttributes="Bold"
                           TextColor="White"/>
                    <Label Text="{Binding HolidayDescription}"
                           FontSize="14"
                           TextColor="White"
                           LineBreakMode="WordWrap"/>
                </VerticalStackLayout>
            </Border>
            
        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

##### 3.4 Implement ViewModel Logic (1 hour)
```csharp
public partial class DateDetailViewModel : BaseViewModel
{
    private readonly Core.Services.ISexagenaryService _sexagenaryService;
    private readonly ICalendarService _calendarService;
    
    [ObservableProperty]
    private DateTime _selectedDate;
    
    [ObservableProperty]
    private string _gregorianDateDisplay = string.Empty;
    
    [ObservableProperty]
    private string _dayOfWeekDisplay = string.Empty;
    
    [ObservableProperty]
    private string _lunarDateDisplay = string.Empty;
    
    [ObservableProperty]
    private string _dayStemBranchDisplay = string.Empty;
    
    [ObservableProperty]
    private string _monthStemBranchDisplay = string.Empty;
    
    [ObservableProperty]
    private string _yearStemBranchDisplay = string.Empty;
    
    [ObservableProperty]
    private string _dayElementDisplay = string.Empty;
    
    [ObservableProperty]
    private Color _dayElementColor = Colors.Gray;
    
    [ObservableProperty]
    private bool _hasHoliday;
    
    [ObservableProperty]
    private string _holidayName = string.Empty;
    
    [ObservableProperty]
    private string _holidayDescription = string.Empty;
    
    [ObservableProperty]
    private Color _holidayColor = Colors.Transparent;
    
    public DateDetailViewModel(
        Core.Services.ISexagenaryService sexagenaryService,
        ICalendarService calendarService)
    {
        _sexagenaryService = sexagenaryService;
        _calendarService = calendarService;
    }
    
    public async Task InitializeAsync(DateTime selectedDate)
    {
        SelectedDate = selectedDate;
        
        // Gregorian date
        GregorianDateDisplay = selectedDate.ToString("MMMM dd, yyyy");
        DayOfWeekDisplay = selectedDate.ToString("dddd");
        
        // Lunar date
        var lunarDates = await _calendarService.GetMonthLunarDatesAsync(
            selectedDate.Year, 
            selectedDate.Month);
        var lunarDate = lunarDates.FirstOrDefault(ld => 
            ld.GregorianDate.Date == selectedDate.Date);
        
        if (lunarDate != null)
        {
            LunarDateDisplay = $"Lunar: {lunarDate.LunarDay}/{lunarDate.LunarMonth}, Year {lunarDate.LunarYear}";
        }
        
        // Stem-branch calculations
        var sexagenaryInfo = await Task.Run(() => 
            _sexagenaryService.GetSexagenaryInfo(selectedDate));
        
        DayStemBranchDisplay = FormatDayStemBranch(sexagenaryInfo.DayStem, sexagenaryInfo.DayBranch);
        MonthStemBranchDisplay = FormatMonthStemBranch(sexagenaryInfo.MonthStem, sexagenaryInfo.MonthBranch);
        YearStemBranchDisplay = FormatYearStemBranch(sexagenaryInfo.YearStem, sexagenaryInfo.YearBranch);
        
        DayElementDisplay = $"Element: {sexagenaryInfo.DayElement}";
        DayElementColor = GetElementColor(sexagenaryInfo.DayElement);
        
        // Holiday info (if exists)
        // ... load from HolidayService
    }
    
    private string FormatDayStemBranch(HeavenlyStem stem, EarthlyBranch branch)
    {
        // Use SexagenaryFormatterHelper
        return SexagenaryFormatterHelper.FormatDayStemBranch(stem, branch);
    }
    
    private Color GetElementColor(FiveElement element)
    {
        return element switch
        {
            FiveElement.Wood => Color.FromArgb("#4CAF50"),
            FiveElement.Fire => Color.FromArgb("#F44336"),
            FiveElement.Earth => Color.FromArgb("#FFC107"),
            FiveElement.Metal => Color.FromArgb("#E0E0E0"),
            FiveElement.Water => Color.FromArgb("#2196F3"),
            _ => Colors.Gray
        };
    }
}
```

**Acceptance Criteria**:
- Tapping any calendar cell navigates to DateDetailPage
- Page shows Gregorian date, Lunar date, and Stem-Branch info
- Back navigation works correctly
- Element colors match Five Element system
- Works on both iOS and Android

---

### Phase 4: Testing & Validation
**Priority**: P1 - HIGH  
**Duration**: 2-3 hours  
**Status**: NOT STARTED

#### Tasks:
1. **Unit Tests for DateDetailViewModel**
   - Test date initialization
   - Test stem-branch formatting
   - Test element color mapping
   - Test lunar date loading

2. **UI Testing**
   - Test navigation from calendar cells
   - Test back navigation
   - Test on multiple screen sizes (iPhone SE, Pro Max, iPad)
   - Test landscape orientation

3. **Integration Testing**
   - Test full flow: Calendar → Tap cell → Detail page → Back
   - Test with dates that have holidays
   - Test with dates in different months
   - Test edge cases (Jan 1, Dec 31, Lunar New Year)

4. **Cross-Platform Testing**
   - Test on iOS physical device
   - Test on Android physical device
   - Verify consistent behavior

**Acceptance Criteria**:
- All unit tests pass (target: 20+ tests)
- UI navigation works smoothly
- No crashes or layout issues
- Performance: Page loads < 200ms

---

### Phase 5: Documentation & Polish (OPTIONAL)
**Priority**: P2 - MEDIUM  
**Duration**: 2 hours  
**Status**: NOT STARTED

#### Tasks:
1. Update README with Phase 2 Sprint 9 features
2. Add screenshots of DateDetailPage
3. Document DateDetailPage navigation
4. Update technical documentation

---

## API Contracts

### ISexagenaryService (EXISTING ✅)

```csharp
public interface ISexagenaryService
{
    /// <summary>
    /// Get complete sexagenary information for a specific date
    /// </summary>
    SexagenaryInfo GetSexagenaryInfo(DateTime date);
    
    /// <summary>
    /// Get sexagenary information for today
    /// </summary>
    SexagenaryInfo GetTodaySexagenaryInfo();
    
    /// <summary>
    /// Get day stem-branch for a specific date
    /// </summary>
    (HeavenlyStem Stem, EarthlyBranch Branch) GetDayStemBranch(DateTime date);
    
    /// <summary>
    /// Get year information for a specific lunar year
    /// </summary>
    (HeavenlyStem Stem, EarthlyBranch Branch, ZodiacAnimal Zodiac) GetYearInfo(int lunarYear);
    
    /// <summary>
    /// Get sexagenary information for a range of dates (batch processing)
    /// </summary>
    IDictionary<DateTime, SexagenaryInfo> GetSexagenaryInfoRange(DateTime startDate, DateTime endDate);
    
    /// <summary>
    /// Clear the cache
    /// </summary>
    void ClearCache();
}
```

### DateDetailViewModel (NEW 🆕)

```csharp
public partial class DateDetailViewModel : BaseViewModel
{
    // Properties
    public DateTime SelectedDate { get; set; }
    public string GregorianDateDisplay { get; set; }
    public string DayOfWeekDisplay { get; set; }
    public string LunarDateDisplay { get; set; }
    public string DayStemBranchDisplay { get; set; }
    public string MonthStemBranchDisplay { get; set; }
    public string YearStemBranchDisplay { get; set; }
    public string DayElementDisplay { get; set; }
    public Color DayElementColor { get; set; }
    public bool HasHoliday { get; set; }
    public string HolidayName { get; set; }
    public string HolidayDescription { get; set; }
    public Color HolidayColor { get; set; }
    
    // Methods
    Task InitializeAsync(DateTime selectedDate);
}
```

---

## Data Models

### SexagenaryInfo (EXISTING ✅)

```csharp
public class SexagenaryInfo
{
    public DateTime Date { get; set; }
    
    // Year
    public int LunarYear { get; set; }
    public HeavenlyStem YearStem { get; set; }
    public EarthlyBranch YearBranch { get; set; }
    public ZodiacAnimal YearZodiac => YearBranch.GetZodiacAnimal();
    public FiveElement YearElement => YearStem.GetElement();
    
    // Month
    public int LunarMonth { get; set; }
    public bool IsLeapMonth { get; set; }
    public HeavenlyStem MonthStem { get; set; }
    public EarthlyBranch MonthBranch { get; set; }
    public FiveElement MonthElement => MonthStem.GetElement();
    
    // Day
    public HeavenlyStem DayStem { get; set; }
    public EarthlyBranch DayBranch { get; set; }
    public FiveElement DayElement => DayStem.GetElement();
    public bool IsDayYang => DayStem.IsYang();
    
    // Hour (optional)
    public HeavenlyStem? HourStem { get; set; }
    public EarthlyBranch? HourBranch { get; set; }
    public FiveElement? HourElement => HourStem?.GetElement();
    
    // Formatted strings
    public string GetYearChineseString();
    public string GetMonthChineseString();
    public string GetDayChineseString();
    public string GetHourChineseString();
    public string GetFullChineseString();
}
```

---

## File Structure

### New Files (Phase 3)

```
src/LunarCalendar.MobileApp/
├── ViewModels/
│   └── DateDetailViewModel.cs                    # NEW: ViewModel for date detail page
├── Views/
│   ├── DateDetailPage.xaml                       # NEW: Date detail page UI
│   └── DateDetailPage.xaml.cs                    # NEW: Code-behind
└── Models/
    └── (No new models needed)

tests/LunarCalendar.Core.Tests/
└── Validation/
    └── HistoricalValidationTests.cs              # NEW: 1000+ date validation
```

### Modified Files

```
src/LunarCalendar.MobileApp/
├── ViewModels/
│   └── CalendarViewModel.cs                      # MODIFIED: Add SelectDateCommand
├── Views/
│   └── CalendarPage.xaml                         # MODIFIED: Add tap gesture to cells
└── MauiProgram.cs                                # MODIFIED: Register DateDetailPage
```

---

## Implementation Timeline

### Day 1 (Monday) - 6 hours
- **Morning** (4 hours): Historical Validation Tests 🚨 **CRITICAL**
  - Create HistoricalValidationTests.cs
  - Load CSV dataset
  - Implement 1000+ date validation
  - Fix any calculation bugs discovered
  
- **Afternoon** (2 hours): Complete User Story 1
  - Language change handling
  - Unit tests for TodayStemBranch

### Day 2 (Tuesday) - 6 hours
- **Morning** (3 hours): Create DateDetailPage Structure
  - Create DateDetailViewModel
  - Create DateDetailPage.xaml
  - Register in DI and routing
  
- **Afternoon** (3 hours): Add Calendar Cell Navigation
  - Add tap gesture to calendar cells
  - Implement SelectDateCommand
  - Test navigation flow

### Day 3 (Wednesday) - 4 hours
- **Morning** (2 hours): Implement Stem-Branch Display in DateDetailPage
  - Load sexagenary data in ViewModel
  - Format stem-branch strings
  - Add element colors
  
- **Afternoon** (2 hours): Testing & Polish
  - Unit tests for DateDetailViewModel
  - UI testing on devices
  - Fix bugs, polish UI

### Day 4 (Thursday) - 2 hours (OPTIONAL)
- **Morning** (2 hours): Documentation & Final Verification
  - Update README
  - Add screenshots
  - Final cross-platform testing

**Total Estimated Time**: 16-18 hours (2-3 days of focused work)

---

## Success Criteria

### Must Have (P0) ✅

1. **Historical Validation** 🚨
   - All 1000+ dates pass with 100% accuracy
   - Lunar New Year boundaries handled correctly
   - Edge cases (1901, 2100) work without errors

2. **Today's Display**
   - Today's stem-branch displays in calendar header
   - Element color indicator works
   - Language switching updates display
   - All unit tests pass

3. **DateDetailPage Navigation**
   - Tapping calendar cell opens DateDetailPage
   - Page shows Gregorian + Lunar + Stem-Branch
   - Back navigation works correctly
   - Works on iOS and Android

### Should Have (P1) ✅

4. **Element Color System**
   - Five elements show correct colors
   - Colors visible in both light/dark mode

5. **Multi-Language Support**
   - Vietnamese and English fully supported
   - Stem-branch names localized correctly

6. **Performance**
   - Page loads < 200ms
   - Calendar navigation remains smooth (60 FPS)
   - No memory leaks

### Nice to Have (P2) ⏳

7. **UI Polish**
   - Beautiful card-based design
   - Smooth animations
   - Responsive layout

8. **Documentation**
   - README updated
   - Screenshots added
   - Technical docs complete

---

## Risk Assessment

### HIGH RISK 🔴

**Risk**: Historical validation fails, calculations are inaccurate  
**Impact**: Cannot ship to production, cultural accuracy compromised  
**Mitigation**: 
- Run validation tests FIRST (Day 1)
- Fix any bugs immediately
- Do not proceed to UI work until validation passes

### MEDIUM RISK 🟡

**Risk**: DateDetailPage navigation conflicts with existing holiday navigation  
**Impact**: Confusion for users, navigation bugs  
**Mitigation**: 
- Test both holiday tap and regular date tap
- Ensure clear visual distinction
- Use different navigation patterns if needed (modal vs push)

**Risk**: Calendar cells too small for tap targets  
**Impact**: Accessibility issues, hard to tap  
**Mitigation**: 
- Minimum 44x44pt touch target (MAUI default)
- Test on iPhone SE (smallest screen)
- Add haptic feedback for confirmation

### LOW RISK 🟢

**Risk**: Element colors not visible in dark mode  
**Impact**: Poor UX in dark mode  
**Mitigation**: 
- Test both light and dark themes
- Adjust color brightness if needed

**Risk**: Localization missing strings  
**Impact**: English fallback shows for some strings  
**Mitigation**: 
- Verify all strings exist in Strings.vi.resx and Strings.en.resx
- Add missing strings early

---

## Deployment Strategy

### Pre-Production Checklist

- [ ] All historical validation tests pass (100% accuracy)
- [ ] All unit tests pass (50+ tests)
- [ ] Cross-platform testing complete (iOS + Android)
- [ ] Performance benchmarks met (< 200ms page load)
- [ ] Cultural accuracy review complete
- [ ] Accessibility tested (VoiceOver, TalkBack)
- [ ] Dark mode tested
- [ ] Memory leak testing complete
- [ ] Documentation updated

### Rollout Plan

1. **Internal Testing** (1-2 days)
   - Test on development devices
   - Fix any critical bugs

2. **TestFlight / Internal Track** (2-3 days)
   - Deploy to 5-10 testers
   - Gather feedback
   - Fix any issues

3. **Phased Rollout** (1 week)
   - 10% of users (monitor for 2 days)
   - 25% of users (monitor for 2 days)
   - 50% of users (monitor for 2 days)
   - 100% (full release)

---

## Open Questions

### RESOLVED ✅

1. ~~Should we show stem-branch on calendar cells?~~  
   **ANSWER**: NO - Too small, not practical. Use DateDetailPage instead.

2. ~~Do we need info tooltips?~~  
   **ANSWER**: NO - User doesn't want them.

3. ~~Does DateDetailPage already exist?~~  
   **ANSWER**: NO - We need to create it (new page).

### PENDING 🤔

1. **Should DateDetailPage be modal or push navigation?**
   - Modal: Full-screen overlay, requires explicit close
   - Push: Standard navigation stack, back button
   - **Recommendation**: Push navigation (consistent with HolidayDetailPage)

2. **Should we show hour stem-branch in DateDetailPage?**
   - Adds complexity (12 periods, expandable section)
   - May confuse users
   - **Recommendation**: Skip for Sprint 9, add in future sprint if requested

3. **Should we add year zodiac to DateDetailPage?**
   - Useful information
   - Already calculated
   - **Recommendation**: Add if time permits (low effort, nice to have)

---

## Conclusion

This **REVISED** plan focuses on **practical, user-friendly implementation** of Sprint 9:

**Key Changes**:
- ❌ Remove calendar cell display (too small)
- ❌ Remove info tooltips (not wanted)
- 🆕 Add DateDetailPage (central hub for date info)
- 🎯 Focus on quality over quantity

**Timeline**: 2-3 days of focused work (16-18 hours)

**Result**: Clean calendar with today's stem-branch + comprehensive detail page when users tap any date.

**Next Step**: Run `/speckit.tasks` to generate detailed task breakdown from this revised plan, then start with **Historical Validation** (Day 1, critical priority).

---

**Plan Status**: ✅ APPROVED FOR IMPLEMENTATION  
**Estimated Completion**: January 28-29, 2026 (3 days from now)  
**Risk Level**: 🟡 MEDIUM (historical validation must pass)
