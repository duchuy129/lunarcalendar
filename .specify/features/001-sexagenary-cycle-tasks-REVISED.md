# Task Breakdown: Sexagenary Cycle Foundation (REVISED)

**Feature**: 001 - Sexagenary Cycle Foundation  
**Sprint**: Sprint 9 (REVISED)  
**Date**: January 26, 2026 (REVISED)  
**Branch**: `feature/001-sexagenary-cycle`  
**Plan**: [001-sexagenary-cycle-foundation-plan-REVISED.md](./001-sexagenary-cycle-foundation-plan-REVISED.md)

---

## 🔄 Revision Summary

**Original Tasks**: 249 tasks across 11 phases  
**Revised Tasks**: ~120 tasks across 5 phases (51% reduction)  
**Reason**: Scope refinement based on practical UX decisions

**Removed from Original**:
- ❌ Info tooltips (T049-T050, T053-T055) - Not needed
- ❌ Calendar cell stem-branch display (T062-T074) - Too small, not practical
- ❌ Educational page (Phase 7) - Defer to future sprint
- ❌ Hour stem-branch display (Phase 6) - Defer to future sprint
- ❌ Backend API (Phase 8) - Optional, defer

**Added to Revised**:
- 🆕 DateDetailPage creation (Phase 4) - 15 new tasks
- 🆕 Calendar cell navigation (Phase 4) - 5 new tasks

**Status After Partial Implementation**:
- ✅ Phase 1: Setup - 100% complete (4/4 tasks)
- ⚠️ Phase 2: Foundation - 90% complete (31/36 tasks) - missing historical validation
- ⚠️ Phase 3: User Story 1 - 33% complete (2/15 tasks) - partial implementation
- ❌ Phase 4: DateDetailPage - 0% complete (0/20 tasks) - NEW
- ❌ Phase 5: Testing & Deployment - 0% complete (0/10 tasks)

---

## Phase 1: Setup (4 tasks) ✅ COMPLETE

**Goal**: Establish branch, research foundation, validation dataset  
**Duration**: 1 hour  
**Status**: ✅ 100% COMPLETE (4/4 tasks)  
**Priority**: P0 - CRITICAL

### T001: Create Feature Branch ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 5 min  
**Description**: Create and checkout `feature/001-sexagenary-cycle` branch  
**Acceptance Criteria**:
- Branch created from `main`
- Branch pushed to remote
- Git workflow established

**Verification**: ✅ Branch exists and is active

---

### T002: Create Research Directory ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 5 min  
**Description**: Create `research/sexagenary-cycle/` directory for documentation  
**Acceptance Criteria**:
- Directory created: `research/sexagenary-cycle/`
- README.md created with research overview
- Gitkeep or initial file committed

**Files Created**:
- ✅ `research/sexagenary-cycle/README.md`

**Verification**: ✅ Directory exists with documentation

---

### T003: Create Validation Dataset ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 30 min  
**Description**: Prepare CSV dataset with 1000+ known historical dates for validation  
**Acceptance Criteria**:
- CSV file: `research/sexagenary-cycle/validation-dates.csv`
- Minimum 1000 dates from 1901-2100
- Columns: Date, DayStem, DayBranch, MonthStem, MonthBranch, YearStem, YearBranch
- Include edge cases: Jan 1, Dec 31, Lunar New Year boundaries
- Source references documented

**Files Created**:
- ✅ `research/sexagenary-cycle/validation-dates.csv`

**Verification**: ✅ CSV exists with 1000+ dates

---

### T004: Document Algorithm Research ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Document sexagenary calculation algorithms and formulas  
**Acceptance Criteria**:
- Document: `research/sexagenary-cycle/algorithm.md`
- Day stem-branch calculation formula documented
- Month stem-branch calculation documented
- Year stem-branch calculation documented
- Hour stem-branch calculation documented
- References to authoritative sources

**Files Created**:
- ✅ `research/sexagenary-cycle/algorithm.md`

**Verification**: ✅ Algorithm documentation complete

---

## Phase 2: Foundation Layer (36 tasks) ⚠️ 90% COMPLETE

**Goal**: Implement core calculation engine, models, services, and validation  
**Duration**: 8-10 hours  
**Status**: ⚠️ 90% COMPLETE (31/36 tasks) - **MISSING HISTORICAL VALIDATION**  
**Priority**: P0 - CRITICAL

### 2.1 Core Models (5 tasks) ✅ COMPLETE

#### T005: Create HeavenlyStem Enum ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 10 min  
**Description**: Define enum for 10 Heavenly Stems with localized names  
**Acceptance Criteria**:
- Enum: `LunarCalendar.Core.Models.HeavenlyStem`
- 10 values: Jia, Yi, Bing, Ding, Wu, Ji, Geng, Xin, Ren, Gui
- Extension methods: `GetElement()`, `IsYang()`, `GetVietnameseName()`, `GetEnglishName()`
- XML documentation

**Files Created**:
- ✅ `src/LunarCalendar.Core/Models/HeavenlyStem.cs`

**Verification**: ✅ Enum exists and compiles

---

#### T006: Create EarthlyBranch Enum ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 10 min  
**Description**: Define enum for 12 Earthly Branches with zodiac mapping  
**Acceptance Criteria**:
- Enum: `LunarCalendar.Core.Models.EarthlyBranch`
- 12 values: Zi, Chou, Yin, Mao, Chen, Si, Wu, Wei, Shen, You, Xu, Hai
- Extension methods: `GetZodiacAnimal()`, `IsYang()`, `GetVietnameseName()`, `GetEnglishName()`
- XML documentation

**Files Created**:
- ✅ `src/LunarCalendar.Core/Models/EarthlyBranch.cs`

**Verification**: ✅ Enum exists and compiles

---

#### T007: Create FiveElement Enum ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 5 min  
**Description**: Define enum for Five Elements (Wu Xing)  
**Acceptance Criteria**:
- Enum: `LunarCalendar.Core.Models.FiveElement`
- 5 values: Wood, Fire, Earth, Metal, Water
- Extension methods: `GetVietnameseName()`, `GetEnglishName()`, `GetColor()`
- XML documentation

**Files Created**:
- ✅ `src/LunarCalendar.Core/Models/FiveElement.cs`

**Verification**: ✅ Enum exists and compiles

---

#### T008: Create ZodiacAnimal Enum ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 5 min  
**Description**: Define enum for 12 Zodiac Animals  
**Acceptance Criteria**:
- Enum: `LunarCalendar.Core.Models.ZodiacAnimal`
- 12 values: Rat, Ox, Tiger, Rabbit, Dragon, Snake, Horse, Goat, Monkey, Rooster, Dog, Pig
- Extension methods: `GetVietnameseName()`, `GetEnglishName()`, `GetEmoji()`
- XML documentation

**Files Created**:
- ✅ `src/LunarCalendar.Core/Models/ZodiacAnimal.cs`

**Verification**: ✅ Enum exists and compiles

---

#### T009: Create SexagenaryInfo Model ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Create comprehensive model for sexagenary information  
**Acceptance Criteria**:
- Class: `LunarCalendar.Core.Models.SexagenaryInfo`
- Properties: Date, LunarYear, LunarMonth, IsLeapMonth
- Properties: YearStem, YearBranch, MonthStem, MonthBranch, DayStem, DayBranch
- Properties: HourStem?, HourBranch? (nullable)
- Computed properties: YearZodiac, YearElement, MonthElement, DayElement, HourElement
- Methods: GetYearChineseString(), GetMonthChineseString(), GetDayChineseString(), GetHourChineseString(), GetFullChineseString()
- XML documentation

**Files Created**:
- ✅ `src/LunarCalendar.Core/Models/SexagenaryInfo.cs`

**Verification**: ✅ Model exists and compiles

---

### 2.2 Calculation Engine (6 tasks) ✅ COMPLETE

#### T010: Create SexagenaryCalculator Class ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 30 min  
**Description**: Implement core calculation algorithms  
**Acceptance Criteria**:
- Class: `LunarCalendar.Core.Services.SexagenaryCalculator`
- Static methods for day/month/year/hour calculations
- Pure functions (no side effects)
- XML documentation

**Files Created**:
- ✅ `src/LunarCalendar.Core/Services/SexagenaryCalculator.cs`

**Verification**: ✅ Calculator exists and compiles

---

#### T011: Implement Day Stem-Branch Calculation ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 45 min  
**Description**: Calculate day stem-branch using Julian Day Number algorithm  
**Acceptance Criteria**:
- Method: `CalculateDayStemBranch(DateTime date)`
- Returns: `(HeavenlyStem, EarthlyBranch)`
- Algorithm: JDN-based calculation with modulo 60
- Handles dates 1901-2100
- Matches known historical dates

**Implementation**: ✅ Algorithm implemented

**Test Cases**:
- ✅ Jan 25, 2026 → Kỷ Hợi (Ji-Hai)
- ✅ Jan 26, 2026 → Canh Tý (Geng-Zi)
- ✅ Jan 1, 1901 → Known reference
- ✅ Dec 31, 2100 → Known reference

**Verification**: ✅ Passes empirical validation

---

#### T012: Implement Month Stem-Branch Calculation ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 30 min  
**Description**: Calculate month stem-branch using lunar month and year stem  
**Acceptance Criteria**:
- Method: `CalculateMonthStemBranch(int lunarMonth, HeavenlyStem yearStem)`
- Returns: `(HeavenlyStem, EarthlyBranch)`
- Algorithm: Formula based on lunar month and year stem
- Handles leap months
- Matches historical references

**Implementation**: ✅ Algorithm implemented

**Verification**: ✅ Passes unit tests

---

#### T013: Implement Year Stem-Branch Calculation ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 30 min  
**Description**: Calculate year stem-branch using lunar year  
**Acceptance Criteria**:
- Method: `CalculateYearStemBranch(int lunarYear)`
- Returns: `(HeavenlyStem, EarthlyBranch, ZodiacAnimal)`
- Algorithm: (year - 4) mod 60
- Handles years 1901-2100
- Matches known zodiac animals

**Implementation**: ✅ Algorithm implemented

**Test Cases**:
- ✅ 2026 → Fire Horse (Bính Ngọ)
- ✅ 2025 → Wood Snake (Ất Tỵ)
- ✅ 1984 → Wood Rat (Giáp Tý)

**Verification**: ✅ Passes unit tests

---

#### T014: Implement Hour Stem-Branch Calculation ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Calculate hour stem-branch using time and day stem  
**Acceptance Criteria**:
- Method: `CalculateHourStemBranch(DateTime dateTime, HeavenlyStem dayStem)`
- Returns: `(HeavenlyStem, EarthlyBranch)`
- Algorithm: Hour branch from time (12 periods), hour stem from day stem
- Handles 24-hour format
- Matches traditional time periods

**Implementation**: ✅ Algorithm implemented

**Verification**: ✅ Passes unit tests

---

#### T015: Implement Element and Zodiac Mappings ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Map stems to elements and branches to zodiac animals  
**Acceptance Criteria**:
- Method: `GetElementFromStem(HeavenlyStem stem)`
- Method: `GetZodiacFromBranch(EarthlyBranch branch)`
- Correct mappings per traditional system
- XML documentation

**Implementation**: ✅ Mappings implemented

**Verification**: ✅ Passes unit tests

---

### 2.3 Service Layer (4 tasks) ✅ COMPLETE

#### T016: Create ISexagenaryService Interface ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Define service interface for sexagenary calculations  
**Acceptance Criteria**:
- Interface: `LunarCalendar.Core.Services.ISexagenaryService`
- Methods: GetSexagenaryInfo(), GetTodaySexagenaryInfo(), GetDayStemBranch(), GetYearInfo(), GetSexagenaryInfoRange(), ClearCache()
- XML documentation

**Files Created**:
- ✅ `src/LunarCalendar.Core/Services/ISexagenaryService.cs`

**Verification**: ✅ Interface exists and compiles

---

#### T017: Implement SexagenaryService ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 1 hour  
**Description**: Implement service with caching and lunar calendar integration  
**Acceptance Criteria**:
- Class: `LunarCalendar.Core.Services.SexagenaryService`
- Implements ISexagenaryService
- LRU caching with max 100 entries
- Integrates with ChineseLunisolarCalendar
- Dependency injection ready

**Files Created**:
- ✅ `src/LunarCalendar.Core/Services/SexagenaryService.cs`

**Implementation**:
- ✅ LRU cache with eviction
- ✅ Lunar calendar integration
- ✅ All interface methods implemented

**Verification**: ✅ Service works correctly

---

#### T018: Register SexagenaryService in DI ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 5 min  
**Description**: Register service in dependency injection container  
**Acceptance Criteria**:
- Modified: `src/LunarCalendar.Core/CoreServiceExtensions.cs`
- Service registered as singleton
- Interface mapping configured

**Files Modified**:
- ✅ `src/LunarCalendar.Core/CoreServiceExtensions.cs`

**Verification**: ✅ DI registration works

---

#### T019: Create Service Unit Tests Skeleton ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 10 min  
**Description**: Set up test class for SexagenaryService  
**Acceptance Criteria**:
- Class: `tests/LunarCalendar.Core.Tests/Services/SexagenaryServiceTests.cs`
- Test setup with mock dependencies
- Test cleanup

**Files Created**:
- ✅ `tests/LunarCalendar.Core.Tests/Services/SexagenaryServiceTests.cs`

**Verification**: ✅ Test class exists

---

### 2.4 Localization (5 tasks) ⚠️ 40% COMPLETE

#### T020: Add Vietnamese Stem Names ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Add Vietnamese names for 10 Heavenly Stems  
**Acceptance Criteria**:
- File: `src/LunarCalendar.Core/Resources/Strings.vi.resx`
- Keys: Stem_Jia, Stem_Yi, ..., Stem_Gui (10 keys)
- Values: Giáp, Ất, Bính, Đinh, Mậu, Kỷ, Canh, Tân, Nhâm, Quý
- Proper diacritics

**Files Modified**:
- ✅ `src/LunarCalendar.Core/Resources/Strings.vi.resx`

**Verification**: ✅ Strings exist and display correctly

---

#### T021: Add Vietnamese Branch Names ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Add Vietnamese names for 12 Earthly Branches  
**Acceptance Criteria**:
- File: `src/LunarCalendar.Core/Resources/Strings.vi.resx`
- Keys: Branch_Zi, Branch_Chou, ..., Branch_Hai (12 keys)
- Values: Tý, Sửu, Dần, Mão, Thìn, Tỵ, Ngọ, Mùi, Thân, Dậu, Tuất, Hợi
- Proper diacritics

**Files Modified**:
- ✅ `src/LunarCalendar.Core/Resources/Strings.vi.resx`

**Verification**: ✅ Strings exist and display correctly

---

#### T022: Add English Stem Names ⏳
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 10 min  
**Description**: Add English names for 10 Heavenly Stems  
**Acceptance Criteria**:
- File: `src/LunarCalendar.Core/Resources/Strings.en.resx`
- Keys: Stem_Jia, Stem_Yi, ..., Stem_Gui (10 keys)
- Values: Jia, Yi, Bing, Ding, Wu, Ji, Geng, Xin, Ren, Gui
- Pinyin transliteration

**Files to Modify**:
- `src/LunarCalendar.Core/Resources/Strings.en.resx`

---

#### T023: Add English Branch Names ⏳
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 10 min  
**Description**: Add English names for 12 Earthly Branches  
**Acceptance Criteria**:
- File: `src/LunarCalendar.Core/Resources/Strings.en.resx`
- Keys: Branch_Zi, Branch_Chou, ..., Branch_Hai (12 keys)
- Values: Zi, Chou, Yin, Mao, Chen, Si, Wu, Wei, Shen, You, Xu, Hai
- Pinyin transliteration

**Files to Modify**:
- `src/LunarCalendar.Core/Resources/Strings.en.resx`

---

#### T024: Add Element and Zodiac Localization ⏳
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Add localized names for Five Elements and Zodiac Animals  
**Acceptance Criteria**:
- Files: Strings.vi.resx, Strings.en.resx
- Element keys: Element_Wood, Element_Fire, Element_Earth, Element_Metal, Element_Water
- Vietnamese: Mộc, Hỏa, Thổ, Kim, Thủy
- English: Wood, Fire, Earth, Metal, Water
- Zodiac keys: Zodiac_Rat, Zodiac_Ox, ..., Zodiac_Pig (12 keys)
- Vietnamese: Tý, Sửu, Dần, Mão, Thìn, Tỵ, Ngọ, Mùi, Thân, Dậu, Tuất, Hợi
- English: Rat, Ox, Tiger, Rabbit, Dragon, Snake, Horse, Goat, Monkey, Rooster, Dog, Pig

**Files to Modify**:
- `src/LunarCalendar.Core/Resources/Strings.vi.resx`
- `src/LunarCalendar.Core/Resources/Strings.en.resx`

---

### 2.5 Unit Tests (9 tasks) ✅ COMPLETE

#### T025: Test Day Stem-Branch Calculation ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 30 min  
**Description**: Test day calculation with known dates  
**Acceptance Criteria**:
- Test: `CalculateDayStemBranch_KnownDates_ReturnsCorrectStemBranch()`
- Test data: Jan 25, 2026 → Ji-Hai, Jan 26, 2026 → Geng-Zi
- Test edge cases: Jan 1 1901, Dec 31 2100
- All tests pass

**Files Created**:
- ✅ `tests/LunarCalendar.Core.Tests/Services/SexagenaryCalculatorTests.cs`

**Verification**: ✅ Tests pass

---

#### T026: Test Month Stem-Branch Calculation ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Test month calculation with known months  
**Acceptance Criteria**:
- Test: `CalculateMonthStemBranch_KnownMonths_ReturnsCorrectStemBranch()`
- Test multiple year stems
- Test all 12 months
- All tests pass

**Verification**: ✅ Tests pass

---

#### T027: Test Year Stem-Branch Calculation ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Test year calculation with known years  
**Acceptance Criteria**:
- Test: `CalculateYearStemBranch_KnownYears_ReturnsCorrectStemBranchAndZodiac()`
- Test data: 2026 → Fire Horse, 2025 → Wood Snake, 1984 → Wood Rat
- Test zodiac mapping
- All tests pass

**Verification**: ✅ Tests pass

---

#### T028: Test Hour Stem-Branch Calculation ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Test hour calculation with known times  
**Acceptance Criteria**:
- Test: `CalculateHourStemBranch_KnownTimes_ReturnsCorrectStemBranch()`
- Test all 12 time periods
- Test different day stems
- All tests pass

**Verification**: ✅ Tests pass

---

#### T029: Test Element Mappings ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Test stem to element mappings  
**Acceptance Criteria**:
- Test: `GetElementFromStem_AllStems_ReturnsCorrectElement()`
- Test all 10 stems
- Verify Wood, Fire, Earth, Metal, Water mapping
- All tests pass

**Verification**: ✅ Tests pass

---

#### T030: Test Zodiac Mappings ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Test branch to zodiac animal mappings  
**Acceptance Criteria**:
- Test: `GetZodiacFromBranch_AllBranches_ReturnsCorrectZodiac()`
- Test all 12 branches
- Verify correct zodiac animals
- All tests pass

**Verification**: ✅ Tests pass

---

#### T031: Test SexagenaryService GetSexagenaryInfo ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 30 min  
**Description**: Test service method returns complete info  
**Acceptance Criteria**:
- Test: `GetSexagenaryInfo_ValidDate_ReturnsCompleteInfo()`
- Test date, day, month, year stem-branch
- Test lunar year and month
- Test element and zodiac properties
- All tests pass

**Verification**: ✅ Tests pass

---

#### T032: Test SexagenaryService Caching ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Test LRU cache behavior  
**Acceptance Criteria**:
- Test: `GetSexagenaryInfo_SameDate_ReturnsCachedResult()`
- Test: `GetSexagenaryInfo_CacheEviction_EvictsOldestEntry()`
- Verify cache size limit (100 entries)
- Verify performance improvement
- All tests pass

**Verification**: ✅ Tests pass

---

#### T033: Test SexagenaryService Range Query ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Test batch calculation for date ranges  
**Acceptance Criteria**:
- Test: `GetSexagenaryInfoRange_30Days_ReturnsAllDates()`
- Test date range query (30 days)
- Verify all dates calculated correctly
- Verify performance < 100ms for 30 dates
- All tests pass

**Verification**: ✅ Tests pass

---

### 2.6 Historical Validation (5 tasks) ❌ 0% COMPLETE - 🚨 CRITICAL

#### T034: Create Historical Validation Test Class ❌
**Status**: TODO (CRITICAL)  
**Assignee**: Developer  
**Estimate**: 15 min  
**Priority**: P0 - BLOCKING  
**Description**: Set up test class for historical validation  
**Acceptance Criteria**:
- Class: `tests/LunarCalendar.Core.Tests/Validation/HistoricalValidationTests.cs`
- Test setup with CSV loader
- Performance timing instrumentation

**Files to Create**:
- `tests/LunarCalendar.Core.Tests/Validation/HistoricalValidationTests.cs`

**Why Critical**: Required by Constitution (Cultural Accuracy requirement)

---

#### T035: Load Validation Dataset ❌
**Status**: TODO (CRITICAL)  
**Assignee**: Developer  
**Estimate**: 20 min  
**Priority**: P0 - BLOCKING  
**Description**: Implement CSV loader for 1000+ validation dates  
**Acceptance Criteria**:
- Method: `LoadValidationDataset()`
- Returns: `List<ValidationRecord>` with 1000+ entries
- Parses CSV: Date, DayStem, DayBranch, MonthStem, MonthBranch, YearStem, YearBranch
- Handles parsing errors gracefully

**Dependencies**: T034

---

#### T036: Validate 1000+ Historical Dates ❌
**Status**: TODO (CRITICAL)  
**Assignee**: Developer  
**Estimate**: 30 min  
**Priority**: P0 - BLOCKING  
**Description**: Run calculation against all 1000+ known dates  
**Acceptance Criteria**:
- Test: `HistoricalValidation_1000Dates_100PercentAccuracy()`
- Loads all validation dates
- Calculates stem-branch for each date
- Compares with expected values
- **100% accuracy required** (Constitution requirement)
- Test execution < 10 seconds
- Detailed error report if any mismatch

**Dependencies**: T035

**Why Critical**: Cannot certify algorithm accuracy without this

---

#### T037: Test Lunar New Year Boundaries ❌
**Status**: TODO (CRITICAL)  
**Assignee**: Developer  
**Estimate**: 30 min  
**Priority**: P0 - BLOCKING  
**Description**: Validate stem-branch around Lunar New Year transitions  
**Acceptance Criteria**:
- Test: `LunarNewYear_Boundaries_CorrectYearStemBranch()`
- Test dates: 3 days before/after Lunar New Year for years 2020-2030
- Verify year stem-branch changes on correct date
- Verify month calculation transitions correctly
- All tests pass

**Dependencies**: T035

**Test Cases**:
- Jan 25, 2026 (eve of Lunar New Year) → Fire Horse year
- Jan 26, 2026 (Lunar New Year) → Fire Goat year starts
- Similar for 2020, 2021, 2022, 2023, 2024, 2025, 2027, 2028, 2029, 2030

---

#### T038: Test Edge Cases (1901, 2100) ❌
**Status**: TODO (CRITICAL)  
**Assignee**: Developer  
**Estimate**: 20 min  
**Priority**: P0 - BLOCKING  
**Description**: Validate calculations at ChineseLunisolarCalendar limits  
**Acceptance Criteria**:
- Test: `EdgeCases_1901And2100_NoErrors()`
- Test: Jan 1, 1901 - earliest supported date
- Test: Dec 31, 2100 - latest supported date
- Verify no exceptions thrown
- Verify results match expected values
- All tests pass

**Dependencies**: T035

---

## Phase 3: User Story 1 - Today's Stem-Branch Display (15 tasks) ⚠️ 33% COMPLETE

**Goal**: Display today's stem-branch in calendar header with element color  
**Duration**: 3-4 hours  
**Status**: ⚠️ 33% COMPLETE (5/15 tasks)  
**Priority**: P1 - HIGH

### 3.1 ViewModel Integration (5 tasks) ⚠️ 60% COMPLETE

#### T039: Inject ISexagenaryService into CalendarViewModel ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 10 min  
**Description**: Add dependency injection for ISexagenaryService  
**Acceptance Criteria**:
- File: `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`
- Constructor parameter: `ISexagenaryService sexagenaryService`
- Field: `private readonly ISexagenaryService _sexagenaryService;`
- No compilation errors

**Files Modified**:
- ✅ `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`

**Verification**: ✅ Service injected successfully

---

#### T040: Add TodayStemBranch Property ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 10 min  
**Description**: Add observable property for today's stem-branch display  
**Acceptance Criteria**:
- Property: `[ObservableProperty] private string _todayStemBranch = string.Empty;`
- Property: `[ObservableProperty] private Color _todayElementColor = Colors.Gray;`
- XML documentation

**Files Modified**:
- ✅ `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`

**Verification**: ✅ Properties exist

---

#### T041: Implement LoadTodaySexagenaryInfoAsync ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Load and format today's stem-branch information  
**Acceptance Criteria**:
- Method: `private async Task LoadTodaySexagenaryInfoAsync()`
- Calls: `_sexagenaryService.GetTodaySexagenaryInfo()`
- Formats: Using SexagenaryFormatterHelper
- Sets: TodayStemBranch and TodayElementColor
- Handles exceptions gracefully

**Files Modified**:
- ✅ `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`

**Implementation**:
```csharp
private async Task LoadTodaySexagenaryInfoAsync()
{
    try
    {
        var info = await Task.Run(() => _sexagenaryService.GetTodaySexagenaryInfo());
        TodayStemBranch = SexagenaryFormatterHelper.FormatDayStemBranch(info.DayStem, info.DayBranch);
        TodayElementColor = GetElementColor(info.DayElement);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to load today's sexagenary info");
        TodayStemBranch = string.Empty;
    }
}
```

**Verification**: ✅ Method works correctly

---

#### T042: Call LoadTodaySexagenaryInfoAsync on Initialization ⏳
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 5 min  
**Description**: Call method when calendar loads  
**Acceptance Criteria**:
- Modified: `LoadCalendarAsync()` or `OnAppearing()`
- Call: `await LoadTodaySexagenaryInfoAsync();`
- Parallel loading with calendar data (don't block UI)

**Files to Modify**:
- `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`

**Dependencies**: T041

---

#### T043: Implement Element Color Helper ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 10 min  
**Description**: Create helper method to map FiveElement to Color  
**Acceptance Criteria**:
- Method: `private Color GetElementColor(FiveElement element)`
- Returns:
  - Wood → #4CAF50 (green)
  - Fire → #F44336 (red)
  - Earth → #FFC107 (amber)
  - Metal → #E0E0E0 (gray)
  - Water → #2196F3 (blue)

**Files Modified**:
- ✅ `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`

**Verification**: ✅ Method exists and works

---

### 3.2 UI Implementation (5 tasks) ⚠️ 20% COMPLETE

#### T044: Add Stem-Branch Label to Calendar Header ✅
**Status**: DONE  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Add UI label for today's stem-branch in CalendarPage header  
**Acceptance Criteria**:
- File: `src/LunarCalendar.MobileApp/Views/CalendarPage.xaml`
- Label text binding: `{Binding TodayStemBranch}`
- Font: 14pt, SemiBold
- Layout: Below month/year selector, above calendar grid
- Styling: Consistent with app theme

**Files Modified**:
- ✅ `src/LunarCalendar.MobileApp/Views/CalendarPage.xaml`

**Verification**: ✅ Label displays in header

---

#### T045: Add Element Color Indicator ⏳
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Add visual indicator for element color  
**Acceptance Criteria**:
- UI element: Small circle (16x16pt) or border
- Color binding: `{Binding TodayElementColor}`
- Position: Next to stem-branch label
- Accessible: Color + text description

**Files to Modify**:
- `src/LunarCalendar.MobileApp/Views/CalendarPage.xaml`

**Dependencies**: T044

**Example XAML**:
```xml
<HorizontalStackLayout Spacing="8" HorizontalOptions="Center">
    <Ellipse WidthRequest="16" HeightRequest="16" 
             Fill="{Binding TodayElementColor}"
             VerticalOptions="Center"/>
    <Label Text="{Binding TodayStemBranch}" 
           FontSize="14" FontAttributes="Bold"/>
</HorizontalStackLayout>
```

---

#### T046: Test Language Switching ⏳
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Verify stem-branch updates when changing language  
**Acceptance Criteria**:
- Switch language: Vietnamese → English → Vietnamese
- Verify TodayStemBranch updates immediately
- Verify element color remains consistent
- No UI glitches or delays

**Dependencies**: T042, T044

**Test Steps**:
1. Launch app (Vietnamese)
2. Note stem-branch text (e.g., "Kỷ Hợi")
3. Switch to English in settings
4. Verify stem-branch text updates (e.g., "Ji-Hai")
5. Switch back to Vietnamese
6. Verify text reverts to "Kỷ Hợi"

---

#### T047: Test on Multiple Screen Sizes ⏳
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Test UI on iPhone SE, iPhone 15 Pro Max, iPad  
**Acceptance Criteria**:
- iPhone SE (375pt width): Label visible, not truncated
- iPhone 15 Pro Max (430pt width): Proper spacing
- iPad (1024pt width): Centered layout, not stretched
- Landscape orientation: Layout adapts correctly

**Dependencies**: T045

---

#### T048: Polish UI Styling ⏳
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Final UI polish and styling  
**Acceptance Criteria**:
- Font sizing consistent with app
- Spacing harmonious with header
- Dark mode: Colors visible
- Light mode: Colors visible
- Accessibility: Contrast ratio > 4.5:1

**Dependencies**: T045

---

### 3.3 Unit Tests (5 tasks) ❌ 0% COMPLETE

#### T049: Create CalendarViewModelTests for Stem-Branch ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Set up unit tests for CalendarViewModel stem-branch features  
**Acceptance Criteria**:
- Class: `tests/LunarCalendar.MobileApp.Tests/ViewModels/CalendarViewModelTests.cs`
- Mock ISexagenaryService
- Test setup and teardown

**Files to Create**:
- `tests/LunarCalendar.MobileApp.Tests/ViewModels/CalendarViewModelTests.cs` (may need to extend existing)

---

#### T050: Test LoadTodaySexagenaryInfoAsync ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 30 min  
**Description**: Test today's stem-branch loading  
**Acceptance Criteria**:
- Test: `LoadTodaySexagenaryInfoAsync_ValidDate_SetsTodayStemBranch()`
- Mock service returns known SexagenaryInfo
- Verify TodayStemBranch property set correctly
- Verify TodayElementColor set correctly
- All tests pass

**Dependencies**: T049

---

#### T051: Test Element Color Mapping ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Test GetElementColor method  
**Acceptance Criteria**:
- Test: `GetElementColor_AllElements_ReturnsCorrectColor()`
- Test all 5 elements
- Verify color values match specification
- All tests pass

**Dependencies**: T049

---

#### T052: Test Language Change Handling ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Test localization updates stem-branch display  
**Acceptance Criteria**:
- Test: `LanguageChanged_ReloadsStemBranch()`
- Mock language change event
- Verify LoadTodaySexagenaryInfoAsync called
- Verify TodayStemBranch updated with new language
- All tests pass

**Dependencies**: T049

---

#### T053: Test Offline Mode ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Verify stem-branch works without network  
**Acceptance Criteria**:
- Test: `LoadTodaySexagenaryInfoAsync_Offline_Works()`
- No network dependency
- All calculations succeed
- All tests pass

**Dependencies**: T049

---

## Phase 4: User Story 2 - DateDetailPage (20 tasks) ❌ 0% COMPLETE - 🆕 NEW

**Goal**: Create detail page showing comprehensive date information with stem-branch  
**Duration**: 4-6 hours  
**Status**: ❌ 0% COMPLETE (0/20 tasks)  
**Priority**: P1 - HIGH

### 4.1 ViewModel Creation (5 tasks) ❌ 0% COMPLETE

#### T054: Create DateDetailViewModel Class ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Create ViewModel for DateDetailPage  
**Acceptance Criteria**:
- Class: `src/LunarCalendar.MobileApp/ViewModels/DateDetailViewModel.cs`
- Inherits: BaseViewModel
- Constructor: Inject ISexagenaryService, ICalendarService, IHolidayService
- Observable properties defined (see below)
- XML documentation

**Files to Create**:
- `src/LunarCalendar.MobileApp/ViewModels/DateDetailViewModel.cs`

**Properties**:
```csharp
[ObservableProperty] private DateTime _selectedDate;
[ObservableProperty] private string _gregorianDateDisplay = string.Empty;
[ObservableProperty] private string _dayOfWeekDisplay = string.Empty;
[ObservableProperty] private string _lunarDateDisplay = string.Empty;
[ObservableProperty] private string _dayStemBranchDisplay = string.Empty;
[ObservableProperty] private string _monthStemBranchDisplay = string.Empty;
[ObservableProperty] private string _yearStemBranchDisplay = string.Empty;
[ObservableProperty] private string _dayElementDisplay = string.Empty;
[ObservableProperty] private Color _dayElementColor = Colors.Gray;
[ObservableProperty] private bool _hasHoliday;
[ObservableProperty] private string _holidayName = string.Empty;
[ObservableProperty] private string _holidayDescription = string.Empty;
[ObservableProperty] private Color _holidayColor = Colors.Transparent;
```

---

#### T055: Implement InitializeAsync Method ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 30 min  
**Description**: Load all date information for selected date  
**Acceptance Criteria**:
- Method: `public async Task InitializeAsync(DateTime selectedDate)`
- Loads: Gregorian date, lunar date, stem-branch info, holiday info
- Formats: All display strings
- Sets: All observable properties
- Handles errors gracefully

**Dependencies**: T054

**Implementation Outline**:
```csharp
public async Task InitializeAsync(DateTime selectedDate)
{
    SelectedDate = selectedDate;
    
    // Gregorian
    GregorianDateDisplay = selectedDate.ToString("MMMM dd, yyyy");
    DayOfWeekDisplay = selectedDate.ToString("dddd");
    
    // Lunar
    var lunarDates = await _calendarService.GetMonthLunarDatesAsync(
        selectedDate.Year, selectedDate.Month);
    var lunarDate = lunarDates.FirstOrDefault(ld => 
        ld.GregorianDate.Date == selectedDate.Date);
    if (lunarDate != null)
    {
        LunarDateDisplay = $"Lunar: {lunarDate.LunarDay}/{lunarDate.LunarMonth}, Year {lunarDate.LunarYear}";
    }
    
    // Stem-branch
    var sexagenaryInfo = await Task.Run(() => 
        _sexagenaryService.GetSexagenaryInfo(selectedDate));
    DayStemBranchDisplay = FormatDayStemBranch(sexagenaryInfo.DayStem, sexagenaryInfo.DayBranch);
    MonthStemBranchDisplay = FormatMonthStemBranch(sexagenaryInfo.MonthStem, sexagenaryInfo.MonthBranch);
    YearStemBranchDisplay = FormatYearStemBranch(sexagenaryInfo.YearStem, sexagenaryInfo.YearBranch);
    DayElementDisplay = $"Element: {sexagenaryInfo.DayElement}";
    DayElementColor = GetElementColor(sexagenaryInfo.DayElement);
    
    // Holiday
    var holidays = await _holidayService.GetHolidaysForDateAsync(selectedDate);
    if (holidays.Any())
    {
        HasHoliday = true;
        var holiday = holidays.First();
        HolidayName = holiday.Name;
        HolidayDescription = holiday.Description;
        HolidayColor = GetHolidayColor(holiday.Type);
    }
}
```

---

#### T056: Implement Stem-Branch Formatting Methods ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Create helper methods to format stem-branch strings  
**Acceptance Criteria**:
- Method: `private string FormatDayStemBranch(HeavenlyStem stem, EarthlyBranch branch)`
- Method: `private string FormatMonthStemBranch(HeavenlyStem stem, EarthlyBranch branch)`
- Method: `private string FormatYearStemBranch(HeavenlyStem stem, EarthlyBranch branch)`
- Uses: SexagenaryFormatterHelper
- Returns: Localized strings

**Dependencies**: T054

---

#### T057: Implement Element Color Helper ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 10 min  
**Description**: Copy element color mapping from CalendarViewModel  
**Acceptance Criteria**:
- Method: `private Color GetElementColor(FiveElement element)`
- Returns same colors as CalendarViewModel
- Wood → #4CAF50, Fire → #F44336, Earth → #FFC107, Metal → #E0E0E0, Water → #2196F3

**Dependencies**: T054

---

#### T058: Register DateDetailViewModel in DI ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 5 min  
**Description**: Register ViewModel in dependency injection  
**Acceptance Criteria**:
- File: `src/LunarCalendar.MobileApp/MauiProgram.cs`
- Registration: `builder.Services.AddTransient<DateDetailViewModel>();`
- No compilation errors

**Dependencies**: T054

---

### 4.2 Page Creation (5 tasks) ❌ 0% COMPLETE

#### T059: Create DateDetailPage XAML ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 45 min  
**Description**: Create UI for DateDetailPage  
**Acceptance Criteria**:
- File: `src/LunarCalendar.MobileApp/Views/DateDetailPage.xaml`
- Layout: ScrollView with VerticalStackLayout
- Sections: Gregorian header, Lunar card, Stem-branch card, Holiday card (conditional)
- Styling: Card-based design with rounded corners
- Bindings: All ViewModel properties bound
- Responsive: Works on all screen sizes

**Files to Create**:
- `src/LunarCalendar.MobileApp/Views/DateDetailPage.xaml`

**UI Structure**:
- Large Gregorian date header (32pt bold)
- Day of week subtitle (18pt)
- Lunar date card with moon emoji
- Stem-branch card with:
  - Day stem-branch with element indicator
  - Month stem-branch
  - Year stem-branch
- Holiday card (if applicable)

---

#### T060: Create DateDetailPage Code-Behind ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Create code-behind for DateDetailPage  
**Acceptance Criteria**:
- File: `src/LunarCalendar.MobileApp/Views/DateDetailPage.xaml.cs`
- Constructor: Inject DateDetailViewModel
- Set BindingContext
- OnAppearing: No action needed (ViewModel initialized before navigation)

**Files to Create**:
- `src/LunarCalendar.MobileApp/Views/DateDetailPage.xaml.cs`

**Dependencies**: T059

**Implementation**:
```csharp
public partial class DateDetailPage : ContentPage
{
    public DateDetailPage(DateDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
```

---

#### T061: Register DateDetailPage in DI ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 5 min  
**Description**: Register page in dependency injection  
**Acceptance Criteria**:
- File: `src/LunarCalendar.MobileApp/MauiProgram.cs`
- Registration: `builder.Services.AddTransient<DateDetailPage>();`
- No compilation errors

**Dependencies**: T060

---

#### T062: Register Route in AppShell ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 5 min  
**Description**: Register navigation route (if using Shell routing)  
**Acceptance Criteria**:
- File: `src/LunarCalendar.MobileApp/AppShell.xaml.cs`
- Route: `Routing.RegisterRoute("datedetail", typeof(DateDetailPage));`
- Alternative: Use navigation stack (Shell.Current.Navigation.PushAsync)

**Dependencies**: T061

---

#### T063: Style DateDetailPage with App Theme ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Apply consistent styling with app theme  
**Acceptance Criteria**:
- Uses DynamicResource for colors
- Dark mode: All text and cards visible
- Light mode: All text and cards visible
- Card backgrounds: Subtle contrast with page background
- Corner radius: 12pt (consistent with app)
- Padding: 20pt page, 16pt cards
- Spacing: 20pt between cards

**Dependencies**: T059

---

### 4.3 Navigation Implementation (5 tasks) ❌ 0% COMPLETE

#### T064: Add SelectDateCommand to CalendarViewModel ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Add command to handle calendar cell taps  
**Acceptance Criteria**:
- File: `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`
- Command: `[RelayCommand] async Task SelectDateAsync(CalendarDay calendarDay)`
- Implementation: Get DateDetailPage from DI, initialize ViewModel, navigate
- Haptic feedback on tap

**Files to Modify**:
- `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`

**Implementation**:
```csharp
[RelayCommand]
async Task SelectDateAsync(CalendarDay calendarDay)
{
    _hapticService.PerformClick();
    
    // Get DateDetailPage from DI
    var serviceProvider = IPlatformApplication.Current?.Services;
    var dateDetailPage = serviceProvider.GetRequiredService<Views.DateDetailPage>();
    
    // Initialize ViewModel with selected date
    if (dateDetailPage.BindingContext is DateDetailViewModel viewModel)
    {
        await viewModel.InitializeAsync(calendarDay.Date);
    }
    
    // Navigate
    await Shell.Current.Navigation.PushAsync(dateDetailPage);
}
```

---

#### T065: Add TapGesture to Calendar Cells ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Add tap gesture recognizer to calendar cell template  
**Acceptance Criteria**:
- File: `src/LunarCalendar.MobileApp/Views/CalendarPage.xaml`
- Find: Calendar cell Border template (around line 533)
- Add: TapGestureRecognizer with command binding
- Command binding: `{Binding Source={RelativeSource AncestorType={x:Type viewmodels:CalendarViewModel}}, Path=SelectDateCommand}`
- Command parameter: `{Binding .}` (CalendarDay)

**Files to Modify**:
- `src/LunarCalendar.MobileApp/Views/CalendarPage.xaml`

**Dependencies**: T064

**XAML Addition**:
```xml
<Border ...>
    <Border.GestureRecognizers>
        <TapGestureRecognizer 
            Command="{Binding Source={RelativeSource AncestorType={x:Type viewmodels:CalendarViewModel}}, Path=SelectDateCommand}"
            CommandParameter="{Binding .}"/>
    </Border.GestureRecognizers>
    <!-- existing cell content -->
</Border>
```

---

#### T066: Test Calendar Cell Tap Navigation ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Verify tapping calendar cell navigates to detail page  
**Acceptance Criteria**:
- Tap any calendar cell
- DateDetailPage opens
- Selected date displayed correctly
- Back button navigates back to calendar
- Haptic feedback felt on tap

**Dependencies**: T065

**Test Steps**:
1. Open calendar
2. Tap cell for Jan 26, 2026
3. Verify DateDetailPage opens
4. Verify "January 26, 2026" displayed
5. Verify back button works
6. Repeat for different dates

---

#### T067: Test Touch Target Size ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 10 min  
**Description**: Verify calendar cells have adequate touch targets  
**Acceptance Criteria**:
- Minimum touch target: 44x44pt (iOS HIG, Android Material)
- Test on iPhone SE (smallest screen)
- Easy to tap without precision
- No accidental taps on adjacent cells

**Dependencies**: T065

---

#### T068: Handle Holiday Cell Navigation Conflict ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Ensure holiday cell tap doesn't conflict with date detail navigation  
**Acceptance Criteria**:
- If cell has holiday: Tapping opens DateDetailPage (not HolidayDetailPage)
- DateDetailPage shows holiday info within page
- Or: Tapping holiday name opens HolidayDetailPage, tapping cell elsewhere opens DateDetailPage
- User intent clear and predictable

**Dependencies**: T066

**Decision**: Open DateDetailPage for all cell taps (holiday info shown within DateDetailPage)

---

### 4.4 Content Population (3 tasks) ❌ 0% COMPLETE

#### T069: Display Gregorian Date Information ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 10 min  
**Description**: Verify Gregorian date displays correctly  
**Acceptance Criteria**:
- Large date header (e.g., "January 26, 2026")
- Day of week subtitle (e.g., "Monday")
- Formatting consistent with locale
- All dates display correctly

**Dependencies**: T055, T059

**Test Cases**:
- Jan 1, 2026 (New Year's Day)
- Jan 26, 2026 (Lunar New Year)
- Dec 31, 2026 (Year end)

---

#### T070: Display Lunar Date Information ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 10 min  
**Description**: Verify lunar date displays correctly  
**Acceptance Criteria**:
- Lunar date format: "Lunar: DD/MM, Year YYYY"
- Moon emoji indicator
- Leap month indicator (if applicable)
- All dates display correctly

**Dependencies**: T055, T059

**Test Cases**:
- Jan 26, 2026 → "Lunar: 1/1, Year 4723" (Lunar New Year)
- Regular dates
- Leap month dates (if any in 2026)

---

#### T071: Display Stem-Branch Information ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Verify stem-branch displays correctly with element colors  
**Acceptance Criteria**:
- Day stem-branch: Large, bold, with element indicator
- Month stem-branch: Readable size
- Year stem-branch: Readable size
- Element color: Correct color for day element
- Element name: Displayed below day stem-branch
- All information accurate

**Dependencies**: T055, T059

**Test Cases**:
- Jan 25, 2026 → Day: Kỷ Hợi (Ji-Hai), Element: Earth (yellow)
- Jan 26, 2026 → Day: Canh Tý (Geng-Zi), Element: Metal (gray)
- Different elements across multiple dates

---

### 4.5 Testing & Polish (2 tasks) ❌ 0% COMPLETE

#### T072: Test DateDetailPage on Multiple Screen Sizes ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Test UI on iPhone SE, iPhone 15 Pro Max, iPad  
**Acceptance Criteria**:
- iPhone SE: All content visible, no truncation
- iPhone 15 Pro Max: Proper spacing, not stretched
- iPad: Centered layout, cards not too wide
- Landscape: Layout adapts, all content visible

**Dependencies**: T071

**Devices to Test**:
- iPhone SE (375pt width)
- iPhone 15 Pro Max (430pt width)
- iPad Pro 12.9" (1024pt width)
- Landscape orientation on all

---

#### T073: Create Unit Tests for DateDetailViewModel ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 45 min  
**Description**: Write unit tests for DateDetailViewModel  
**Acceptance Criteria**:
- Class: `tests/LunarCalendar.MobileApp.Tests/ViewModels/DateDetailViewModelTests.cs`
- Test: InitializeAsync sets all properties correctly
- Test: Element color mapping works
- Test: Holiday info loaded when present
- Test: Formatting methods work correctly
- All tests pass

**Files to Create**:
- `tests/LunarCalendar.MobileApp.Tests/ViewModels/DateDetailViewModelTests.cs`

**Dependencies**: T055

**Test Cases**:
- InitializeAsync_ValidDate_SetsAllProperties()
- InitializeAsync_DateWithHoliday_SetsHolidayInfo()
- InitializeAsync_DateWithoutHoliday_HasHolidayFalse()
- FormatDayStemBranch_KnownDate_ReturnsCorrectFormat()
- GetElementColor_AllElements_ReturnsCorrectColor()

---

## Phase 5: Testing, Documentation & Deployment (10 tasks) ❌ 0% COMPLETE

**Goal**: Comprehensive testing, documentation, and production readiness  
**Duration**: 3-4 hours  
**Status**: ❌ 0% COMPLETE (0/10 tasks)  
**Priority**: P2 - MEDIUM

### 5.1 Integration Testing (3 tasks) ❌ 0% COMPLETE

#### T074: End-to-End Flow Testing ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 30 min  
**Description**: Test complete user flow from calendar to detail page  
**Acceptance Criteria**:
- Flow: Launch app → View calendar → Tap cell → View detail → Back
- Test multiple dates (10+ dates)
- Test dates with holidays
- Test dates without holidays
- Test edge cases (month boundaries, year boundaries)
- All flows work smoothly

**Test Scenarios**:
1. Regular date (no holiday)
2. Date with holiday (Lunar New Year)
3. First day of month
4. Last day of month
5. Lunar New Year Eve
6. Different months
7. Language switching during flow

---

#### T075: Cross-Platform Testing ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 45 min  
**Description**: Test on iOS and Android physical devices  
**Acceptance Criteria**:
- iOS device: All features work, no crashes
- Android device: All features work, no crashes
- Performance: Smooth navigation, < 200ms page load
- Consistency: Same behavior on both platforms
- No platform-specific bugs

**Devices to Test**:
- iOS: iPhone 13 or newer
- Android: Pixel 6 or Samsung Galaxy S21+

---

#### T076: Performance Testing ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 30 min  
**Description**: Measure and verify performance benchmarks  
**Acceptance Criteria**:
- Single date calculation: < 10ms
- DateDetailPage initialization: < 200ms
- Calendar navigation: Smooth 60 FPS
- Memory: No leaks after 50 navigations
- Cache: Improves performance on repeated loads

**Metrics to Measure**:
- SexagenaryService.GetSexagenaryInfo() execution time
- DateDetailViewModel.InitializeAsync() execution time
- Memory usage before/after 50 navigations
- Cache hit rate after 10 navigations

---

### 5.2 Documentation (4 tasks) ❌ 0% COMPLETE

#### T077: Update README with Sprint 9 Features ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Document new sexagenary cycle features in README  
**Acceptance Criteria**:
- File: `README.md`
- Section: Added "Sexagenary Cycle (Can Chi / 干支)" under Features
- Description: Today's stem-branch display, DateDetailPage navigation
- Screenshots: Today's display, DateDetailPage
- Usage: How to view stem-branch information

**Files to Modify**:
- `README.md`

---

#### T078: Add Screenshots ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 20 min  
**Description**: Capture and add screenshots of new features  
**Acceptance Criteria**:
- Screenshot 1: Calendar with today's stem-branch in header
- Screenshot 2: DateDetailPage showing full date information
- Screenshot 3: DateDetailPage with holiday
- Images: PNG format, 1x scale, iPhone 15 Pro
- Location: `docs/screenshots/` or similar

**Files to Create**:
- `docs/screenshots/calendar-with-stem-branch.png`
- `docs/screenshots/date-detail-page.png`
- `docs/screenshots/date-detail-with-holiday.png`

**Dependencies**: T074

---

#### T079: Update Technical Documentation ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 30 min  
**Description**: Document technical implementation details  
**Acceptance Criteria**:
- File: `docs/TECHNICAL_ARCHITECTURE.md` or similar
- Section: "Sexagenary Cycle Implementation"
- Content: Algorithm overview, service architecture, caching strategy
- API: ISexagenaryService interface documentation
- Models: SexagenaryInfo model documentation

**Files to Modify**:
- `docs/TECHNICAL_ARCHITECTURE.md`

---

#### T080: Create Migration Guide (if applicable) ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Document any breaking changes or migration steps  
**Acceptance Criteria**:
- File: `docs/MIGRATION_GUIDE.md` or in README
- Breaking changes: None expected (new feature)
- Migration: Not applicable (new feature, no data migration)
- Deployment: No special steps required

**Note**: Likely not needed for Sprint 9 (new feature, non-breaking)

---

### 5.3 Deployment (3 tasks) ❌ 0% COMPLETE

#### T081: Update Version Number ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 5 min  
**Description**: Bump app version for Sprint 9 release  
**Acceptance Criteria**:
- File: `src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj`
- Version: Increment minor or patch (e.g., 1.2.0 → 1.3.0)
- Both iOS and Android version codes updated

**Files to Modify**:
- `src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj`

---

#### T082: Create Release Notes ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 15 min  
**Description**: Write release notes for Sprint 9  
**Acceptance Criteria**:
- File: `CHANGELOG.md` or `releases/v1.3.0.md`
- Section: "Version 1.3.0 - Sexagenary Cycle Foundation"
- New features: Today's stem-branch display, DateDetailPage
- Bug fixes: None (new feature)
- Known issues: None expected

**Content**:
```markdown
## Version 1.3.0 - Sexagenary Cycle Foundation (Sprint 9)

**Release Date**: January 29, 2026

### New Features
- 🎋 Today's Stem-Branch Display: View today's sexagenary cycle (Can Chi / 干支) in calendar header
- 📄 Date Detail Page: Tap any date to view comprehensive information including Gregorian, Lunar, and Stem-Branch details
- 🎨 Element Color Indicators: Visual indicators for Five Elements (Wood, Fire, Earth, Metal, Water)
- 🌏 Multi-Language Support: Vietnamese and English stem-branch names

### Technical Improvements
- Core calculation engine with LRU caching
- Historical validation: 1000+ dates with 100% accuracy
- Performance: < 10ms per calculation, < 200ms page load
- Cross-platform: iOS and Android support

### Known Issues
- None

### Coming Soon (Sprint 10+)
- Chinese language support
- Educational resources about sexagenary cycle
- Hour stem-branch display
```

---

#### T083: Merge Feature Branch ❌
**Status**: TODO  
**Assignee**: Developer  
**Estimate**: 10 min  
**Description**: Merge feature branch to main after all tests pass  
**Acceptance Criteria**:
- All unit tests pass (100%)
- All integration tests pass
- Historical validation passes (100% accuracy)
- Code review approved (if applicable)
- Merge: `feature/001-sexagenary-cycle` → `main`
- Tag: `v1.3.0` or similar

**Commands**:
```bash
git checkout main
git pull origin main
git merge feature/001-sexagenary-cycle
git push origin main
git tag v1.3.0
git push origin v1.3.0
```

**Dependencies**: All tasks T001-T082 complete

---

## Summary

**Total Tasks (REVISED)**: ~120 tasks  
**Original Tasks**: 249 tasks  
**Reduction**: 51% (129 tasks removed or deferred)

### Status Overview

| Phase | Tasks | Complete | In Progress | Todo | % Complete |
|-------|-------|----------|-------------|------|------------|
| Phase 1: Setup | 4 | 4 | 0 | 0 | 100% ✅ |
| Phase 2: Foundation | 36 | 31 | 0 | 5 | 86% ⚠️ |
| Phase 3: User Story 1 | 15 | 5 | 0 | 10 | 33% ⚠️ |
| Phase 4: DateDetailPage | 20 | 0 | 0 | 20 | 0% ❌ |
| Phase 5: Testing & Deployment | 10 | 0 | 0 | 10 | 0% ❌ |
| **TOTAL** | **85** | **40** | **0** | **45** | **47%** |

**Note**: 35 tasks from original plan were removed (tooltips, calendar cells, educational page, hour display)

### Critical Path

**BLOCKING** (must complete first):
1. Historical Validation (T034-T038) - 🚨 **P0 CRITICAL**
2. Complete User Story 1 (T042-T053) - P1 HIGH
3. Create DateDetailPage (T054-T073) - P1 HIGH
4. Testing & Deployment (T074-T083) - P2 MEDIUM

### Time Estimates

- **Phase 2 Completion** (Historical Validation): 2-3 hours
- **Phase 3 Completion** (User Story 1): 2-3 hours
- **Phase 4** (DateDetailPage): 4-6 hours
- **Phase 5** (Testing & Deployment): 3-4 hours

**Total Remaining**: 11-16 hours (1.5-2 days)

### Removed Features (for reference)

❌ **Removed from Sprint 9**:
- Info tooltips (T049-T050, T053-T055 from original) - 7 tasks
- Calendar cell stem-branch display (T062-T074 from original) - 13 tasks
- Educational page (Phase 7 from original) - 25 tasks
- Hour stem-branch display (Phase 6 from original) - 20 tasks
- Backend API (Phase 8 from original) - 30 tasks
- Additional polish tasks - 34 tasks

**Total Removed**: 129 tasks (deferred to future sprints)

---

## Next Steps

1. **IMMEDIATE** (P0): Complete historical validation tests (T034-T038)
   - Cannot certify accuracy without this
   - Required by Constitution (Cultural Accuracy)
   
2. **HIGH** (P1): Complete User Story 1 (T042-T053)
   - Finish today's stem-branch display
   - Add unit tests
   
3. **HIGH** (P1): Build DateDetailPage (T054-T073)
   - Core feature of revised scope
   - Better UX than calendar cell display
   
4. **MEDIUM** (P2): Testing & deployment (T074-T083)
   - Ensure quality before production
   - Update documentation

**Estimated Completion**: January 28-29, 2026 (2-3 days from now)

---

**Task Breakdown Status**: ✅ APPROVED FOR IMPLEMENTATION  
**Last Updated**: January 26, 2026  
**Version**: 2.0 (REVISED)
