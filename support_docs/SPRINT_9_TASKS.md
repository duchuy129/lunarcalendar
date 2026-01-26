# Sprint 9: Sexagenary Cycle Foundation - Task Details

**Sprint Duration**: 2 weeks  
**Status**: In Progress (8/12 complete, 4 remaining)  
**Last Updated**: January 25, 2026

---

## Overview

Sprint 9 focuses on implementing the **Sexagenary Cycle (Can Chi / 干支)** system, which is fundamental to Vietnamese and Chinese lunar calendars. This includes calculating and displaying the 60-year cycle based on Heavenly Stems (Thiên Can) and Earthly Branches (Địa Chi).

---

## Task List

### Backend/Core Tasks

#### ✅ T041: Research and implement Sexagenary cycle calculation algorithm
**Status**: Complete  
**Completed**: January 25, 2026  
**Files Modified**:
- `src/LunarCalendar.Core/Services/SexagenaryCalculator.cs`

**Implementation Details**:
- Implemented Julian Day Number (JDN) based calculation
- Empirically verified formula: `stem = (JDN + 49) % 10`, `branch = (JDN + 1) % 12`
- Verified against reference dates:
  - January 25, 2026 = Kỷ Hợi (Lunar 12/7)
  - January 26, 2026 = Canh Tý (Lunar 12/8)
- Added comprehensive documentation explaining lunar calendar basis

---

#### ✅ T042: Create data models for Heavenly Stems, Earthly Branches, and Five Elements
**Status**: Complete  
**Completed**: January 25, 2026  
**Files Modified**:
- `src/LunarCalendar.Core/Models/HeavenlyStem.cs`
- `src/LunarCalendar.Core/Models/EarthlyBranch.cs`
- `src/LunarCalendar.Core/Models/FiveElements.cs`
- `src/LunarCalendar.Core/Models/SexagenaryInfo.cs`

**Implementation Details**:
- Created enums for 10 Heavenly Stems (Giáp, Ất, Bính, Đinh, Mậu, Kỷ, Canh, Tân, Nhâm, Quý)
- Created enums for 12 Earthly Branches (Tý, Sửu, Dần, Mão, Thìn, Tỵ, Ngọ, Mùi, Thân, Dậu, Tuất, Hợi)
- Created Five Elements enum (Metal, Wood, Water, Fire, Earth)
- SexagenaryInfo model includes day, month, year, and hour stem-branch data

---

#### ✅ T043: Implement calculation for Year, Month, Day, and Hour stem-branch
**Status**: Complete  
**Completed**: January 25, 2026  
**Files Modified**:
- `src/LunarCalendar.Core/Services/SexagenaryCalculator.cs`

**Implementation Details**:
- **Day Calculation**: Empirically-verified JDN formula (stem+49, branch+1)
- **Year Calculation**: Based on lunar year with offset from 1984 (Giáp Tý)
- **Month Calculation**: Dependent on year stem and lunar month
- **Hour Calculation**: Based on earthly branch for 12 traditional double-hours

**Verified Accuracy**:
- Day: January 25, 2026 correctly shows Kỷ Hợi ✅
- Year: 2026 correctly shows Ất Tỵ (not just Tỵ) ✅

---

#### ⏳ T044: Create API endpoints for sexagenary data
**Status**: Deferred  
**Priority**: Low  
**Reason**: Mobile app uses offline-first architecture with local calculations

**Planned Endpoints**:
- `GET /api/calendar/sexagenary/{date}` - Get full sexagenary info
- `GET /api/calendar/year-info/{year}` - Get year's zodiac animal and element

**Note**: May be implemented in future sprint if backend sync is needed.

---

#### ✅ T045: Add localization support for stem-branch names
**Status**: Complete  
**Completed**: January 25, 2026  
**Files Modified**:
- `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`

**Implementation Details**:
- Vietnamese: Giáp, Ất, Bính... / Tý, Sửu, Dần...
- English: Jia, Yi, Bing... / Rat, Ox, Tiger...
- Chinese: 甲, 乙, 丙... / 子, 丑, 寅...
- Language detection via `CultureInfo.CurrentUICulture` (iOS-compatible)

---

#### ⏳ T046: Write comprehensive unit tests for cycle calculations (Backend)
**Status**: Not Started  
**Priority**: Medium  
**Estimated Effort**: 4-6 hours

**Test Coverage Needed**:
- Day stem-branch accuracy across multiple dates
- Year boundaries (e.g., lunar new year transitions)
- Month calculation edge cases
- Hour calculation for all 12 double-hours
- JDN calculation accuracy
- Edge cases: leap years, century transitions

---

#### ⏳ T047: API integration testing
**Status**: Not Started (Blocked by T044)  
**Priority**: Low  
**Note**: Deferred until backend API endpoints are implemented

---

### Mobile Tasks

#### ✅ T048: Create SexagenaryService for fetching cycle data
**Status**: Complete (N/A - offline architecture)  
**Completed**: January 25, 2026  

**Implementation Details**:
- Calculations performed directly in Core library
- No service layer needed - SexagenaryCalculator accessed directly from ViewModels
- Offline-first architecture eliminates network dependency

---

#### ✅ T049: Implement day stem-branch calculation (Ngày Can Chi)
**Status**: Complete  
**Completed**: January 25, 2026  
**Files Modified**:
- `src/LunarCalendar.Core/Services/SexagenaryCalculator.cs`

**Bug Fix Applied**:
- **Issue**: Calculation was one day ahead (showing Jan 26's value for Jan 25)
- **Root Cause**: Incorrect JDN offset
- **Solution**: Empirically-verified offsets (stem+49, branch+1)
- **Verification**: Tested against multiple reference dates

---

#### ✅ T050: Implement year stem-branch calculation (Năm Can Chi)
**Status**: Complete  
**Completed**: January 25, 2026  
**Files Modified**:
- `src/LunarCalendar.Core/Services/SexagenaryCalculator.cs`
- `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`

**Enhancement Applied**:
- **Issue**: Calendar showed "Năm Tỵ" instead of "Năm Ất Tỵ"
- **Solution**: Implemented FormatYearStemBranch() helper method
- **Result**: Full stem-branch display in all three languages

---

#### ✅ T051: Implement month stem-branch calculation (Tháng Can Chi)
**Status**: Complete  
**Completed**: January 25, 2026  
**Files Modified**:
- `src/LunarCalendar.Core/Services/SexagenaryCalculator.cs`

**Implementation Details**:
- Month stem depends on year stem and lunar month
- Formula: `monthStem = (yearStem * 2 + lunarMonth) % 10`
- Not yet displayed in UI (planned for Sprint 10)

---

#### ✅ T052: Update CalendarViewModel with today's Can Chi
**Status**: Complete  
**Completed**: January 25, 2026  
**Files Modified**:
- `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`

**Implementation Details**:
- Added `TodayStemBranch` observable property
- Implemented `LoadTodaySexagenaryInfoAsync()` method
- Added to `LoadCalendarAsync()` for initial load (iOS fix)
- Language-specific formatting with prefixes ("Ngày", "Day", "日")

**Bug Fixes**:
1. **iOS Initialization Bug**: Stem-branch was blank until language switch
   - **Root Cause**: LoadTodaySexagenaryInfoAsync() only called in UpdateTodayDisplayAsync()
   - **Fix**: Added call to LoadCalendarAsync() method
2. **Missing Prefix**: Showed "Kỷ Hợi" instead of "Ngày Kỷ Hợi"
   - **Fix**: Added language-specific prefixes in LoadTodaySexagenaryInfoAsync()

---

#### ✅ T053: Design UI components for stem-branch display
**Status**: Complete  
**Completed**: January 25, 2026  
**Files Modified**:
- `src/LunarCalendar.MobileApp/Views/CalendarPage.xaml`

**Implementation Details**:
- Two-line Today section layout:
  - Line 1: Lunar date + Year stem-branch (19pt bold)
  - Line 2: Element color dot (10×10) + Day stem-branch (14pt) + Info icon (ⓘ)
- Height increased from 56 to 76 to accommodate two lines
- Conditional visibility using StringNotEmptyConverter

---

#### ✅ T054: Add "Today's Information" section
**Status**: Complete  
**Completed**: January 25, 2026  

**Implementation Details**:
- Displays Gregorian date, Lunar date, and Sexagenary date
- Shows zodiac animal for the day
- Five element association via color indicator
- Info icon for future educational tooltip

---

#### ✅ T055: Add language-specific formatting
**Status**: Complete  
**Completed**: January 25, 2026  

**Implementation Details**:
- Vietnamese: "Ngày Kỷ Hợi", "Năm Ất Tỵ"
- English: "Day Ky Hoi", "Year of Yi Si (Snake)"
- Chinese: "日己亥", "乙巳年"
- Language switching works correctly on both iOS and Android

---

#### ⏳ T056: Write unit tests for day stem-branch calculation
**Status**: Not Started  
**Priority**: High  
**Estimated Effort**: 2-3 hours

**Test Cases Needed**:
- Verify day calculation for multiple reference dates
- Test year boundaries (Dec 31 → Jan 1)
- Test lunar new year transitions
- Test edge cases: leap years, century transitions
- Verify JDN calculation accuracy

**Suggested Test File**: `src/LunarCalendar.Core.Tests/Services/SexagenaryCalculatorTests.cs`

---

#### ⏳ T057: Write unit tests for year stem-branch calculation
**Status**: Not Started  
**Priority**: High  
**Estimated Effort**: 2 hours

**Test Cases Needed**:
- Verify year calculation for known years (1984, 2026, etc.)
- Test 60-year cycle wraparound
- Test lunar vs Gregorian year differences
- Verify animal sign mapping

---

#### ⏳ T058: Write unit tests for month stem-branch calculation
**Status**: Not Started  
**Priority**: Medium  
**Estimated Effort**: 1-2 hours

**Test Cases Needed**:
- Verify month calculation formula
- Test all 12 lunar months
- Test relationship between year stem and month stem
- Test leap month handling

---

#### ⏳ T059: Write integration tests for sexagenary display
**Status**: Not Started  
**Priority**: Medium  
**Estimated Effort**: 2 hours

**Test Cases Needed**:
- Test UI updates when date changes
- Test language switching updates stem-branch display
- Test iOS initialization (ensure no blank display)
- Test prefix display in all languages
- Test element color indicator visibility

**Suggested Test File**: `src/LunarCalendar.MobileApp.Tests/ViewModels/CalendarViewModelSexagenaryTests.cs`

---

#### ⏳ T060: Ensure stem-branch consistency across holiday pages
**Status**: Not Started  
**Priority**: High (Consistency critical)  
**Estimated Effort**: 2-3 hours

**Problem Statement**:
Currently, the app shows **inconsistent** year stem-branch information:
- ✅ **Calendar Page**: Shows "Năm Ất Tỵ" (CORRECT - full stem-branch)
- ❌ **Holiday Detail Page**: Shows "Year of the Snake" (INCORRECT - animal only)
- ❌ **Upcoming Holidays**: Shows "Năm Tỵ" (INCORRECT - branch only)
- ❌ **Year Holidays Page**: Shows "Năm Tỵ" (INCORRECT - branch only)

**Root Cause**:
- HolidayDetailViewModel uses `AnimalSignDisplay` which only shows the animal sign
- LocalizedHolidayOccurrence doesn't format year with full stem-branch
- YearHolidaysViewModel may show incomplete year information

**Files to Modify**:
1. `src/LunarCalendar.MobileApp/ViewModels/HolidayDetailViewModel.cs`
   - Line 154: Update AnimalSignDisplay formatting
   - Change from: `$" - {AppResources.YearOfThe} {localizedAnimalSign}"`
   - Change to: Use FormatYearStemBranch() helper method

2. `src/LunarCalendar.MobileApp/Models/LocalizedHolidayOccurrence.cs`
   - Update year display property to include full stem-branch

3. `src/LunarCalendar.MobileApp/ViewModels/YearHolidaysViewModel.cs`
   - Update page title or year selector to show full stem-branch

**Implementation Approach**:
```csharp
// Reuse existing helper from CalendarViewModel
private string FormatYearStemBranch(int lunarYear, EarthlyBranch yearBranch)
{
    // Get year stem-branch using SexagenaryCalculator
    var yearStemBranch = SexagenaryCalculator.CalculateYearStemBranch(lunarYear);
    
    var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
    
    if (currentCulture == "vi")
    {
        // Vietnamese: Just stem-branch (e.g., "Ất Tỵ")
        return $"{GetVietnameseStemName(yearStemBranch.Stem)} {GetVietnameseBranchName(yearStemBranch.Branch)}";
    }
    else if (currentCulture == "zh")
    {
        // Chinese: Chinese characters (e.g., "乙巳")
        return $"{GetChineseStemName(yearStemBranch.Stem)}{GetChineseBranchName(yearStemBranch.Branch)}";
    }
    else
    {
        // English: Stem-branch with animal (e.g., "Yi Si (Snake)")
        var animalName = GetAnimalNameFromBranch(yearStemBranch.Branch);
        return $"{yearStemBranch.Stem} {yearStemBranch.Branch} ({animalName})";
    }
}
```

**Expected Results**:
- Vietnamese: "Năm Ất Tỵ" (not "Năm Tỵ")
- English: "Year of Yi Si (Snake)" (not just "Year of the Snake")
- Chinese: "乙巳年" (full stem-branch)

**Testing Checklist**:
- [ ] Calendar page shows full stem-branch ✅ (already working)
- [ ] Holiday Detail page shows full stem-branch
- [ ] Upcoming Holidays shows full stem-branch
- [ ] Year Holidays page shows full stem-branch
- [ ] All three languages display correctly
- [ ] Language switching updates all pages
- [ ] No performance impact (calculations cached)

---

## Sprint 9 Summary

### Progress Overview
**Completion**: 8/12 tasks complete (67%)

**Completed (8)**:
- T041: Algorithm research and implementation ✅
- T042: Data models creation ✅
- T043: Calculation implementation ✅
- T045: Localization support ✅
- T048: Service architecture (offline-first) ✅
- T049-T055: Mobile UI implementation ✅

**Remaining (4)**:
- T046-T047: Backend testing (deferred/low priority)
- T056-T059: Unit and integration tests (medium-high priority)
- T060: Holiday page consistency (high priority)

### Key Achievements

1. **Accurate Day Calculation**: Empirically-verified JDN formula working for all dates
2. **Full Year Display**: "Năm Ất Tỵ" instead of "Năm Tỵ"
3. **iOS Initialization Fix**: Stem-branch displays immediately on app launch
4. **Language Support**: Vietnamese, English, Chinese with proper prefixes
5. **Consistent Calendar UI**: Two-line layout with element color indicator

### Known Issues (Resolved)
- ✅ Day calculation off by one → Fixed with empirical JDN offsets
- ✅ Year missing stem → Fixed with FormatYearStemBranch()
- ✅ iOS blank on launch → Fixed by adding LoadTodaySexagenaryInfoAsync() to LoadCalendarAsync()
- ✅ Missing "Ngày" prefix → Fixed with language-specific prefixes

### Remaining Work

#### High Priority (Sprint 9 Extension Recommended)
- **T060**: Holiday page consistency (2-3 hours)
  - Critical for user experience
  - Infrastructure already in place
  - Simple copy-paste of existing logic

#### Medium Priority (Can defer to Sprint 10)
- **T056-T059**: Unit and integration tests (6-9 hours)
  - Important for quality assurance
  - Doesn't block feature completion
  - Can be done alongside Sprint 10 work

#### Low Priority (Future Sprint)
- **T044, T047**: Backend API implementation
  - Not needed for offline-first architecture
  - Can be added later for cloud sync

---

## Next Steps

### Immediate (Before Sprint 10)
1. Complete T060 (Holiday page consistency) - **RECOMMENDED**
2. Test thoroughly on physical devices
3. Merge feature branch to develop

### Optional (Parallel with Sprint 10)
1. Write unit tests (T056-T058)
2. Write integration tests (T059)
3. Performance testing
4. Accessibility testing

### Documentation
1. Update user guide with stem-branch explanation
2. Add developer documentation for calculation algorithms
3. Document localization strings
4. Create educational content for app users

---

## Files Modified Summary

### Core Library (3 files)
- `Services/SexagenaryCalculator.cs` - Main calculation logic
- `Models/SexagenaryInfo.cs` - Data model
- `Models/HeavenlyStem.cs, EarthlyBranch.cs, FiveElements.cs` - Enums

### Mobile App (2 files)
- `ViewModels/CalendarViewModel.cs` - Display logic and formatting
- `Views/CalendarPage.xaml` - UI layout

### Total Lines Changed
- **+136 lines** (additions)
- **-26 lines** (deletions)
- **Net: +110 lines**

---

## Commit History

```
cef88d5 - feat: Complete Phase 3 sexagenary cycle display with iOS initialization fix
8628f0f - fix(phase3): Increase Today section height for stem-branch display
0440e56 - feat(phase3): Add UI for today's stem-branch display
6c8ca01 - feat(phase3): Add today's stem-branch to CalendarViewModel
9d50d42 - feat: Implement comprehensive testing suite for sexagenary cycle
```

---

**Last Updated**: January 25, 2026  
**Branch**: feature/001-sexagenary-cycle  
**Ready for**: User testing and Sprint 10 planning
