# Tasks: Sexagenary Cycle Foundation (Can Chi / 干支)

**Feature**: 001-sexagenary-cycle  
**Branch**: `feature/001-sexagenary-cycle`  
**Input**: `.specify/features/001-sexagenary-cycle-foundation.md` (spec), `001-sexagenary-cycle-foundation-plan.md` (plan)  
**Sprint**: Phase 2 - Sprint 9 (2 weeks)

---

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (US1-US5)
- File paths follow existing LunarCalendar project structure

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and feature branch setup

- [ ] T001 Create feature branch `feature/001-sexagenary-cycle` from main
- [ ] T002 Create research directory `.specify/features/001-sexagenary-cycle/research/`
- [ ] T003 [P] Add historical validation dataset to `.specify/features/001-sexagenary-cycle/research/historical-validation-dataset.csv` (1000+ dates)
- [ ] T004 [P] Document sexagenary algorithm in `.specify/features/001-sexagenary-cycle/research/sexagenary-algorithm.md`

**Checkpoint**: Feature branch and research materials ready

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core data models and calculation engine that ALL user stories depend on

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

### Core Data Models

- [ ] T005 [P] Create `HeavenlyStem` enum in `src/LunarCalendar.Core/Models/HeavenlyStem.cs` with 10 stems (Giáp through Quý)
- [ ] T006 [P] Create `EarthlyBranch` enum in `src/LunarCalendar.Core/Models/EarthlyBranch.cs` with 12 branches (Tý through Hợi)
- [ ] T007 [P] Create `FiveElement` enum in `src/LunarCalendar.Core/Models/FiveElement.cs` (Metal, Wood, Water, Fire, Earth)
- [ ] T008 [P] Create `ZodiacAnimal` enum in `src/LunarCalendar.Core/Models/ZodiacAnimal.cs` with 12 animals
- [ ] T009 Create `SexagenaryInfo` class in `src/LunarCalendar.Core/Models/SexagenaryInfo.cs` (composite model with day/month/year/hour data)

### Calculation Engine

- [ ] T010 Create `SexagenaryCalculator` class in `src/LunarCalendar.Core/Services/SexagenaryCalculator.cs`
- [ ] T011 Implement `CalculateDayStemBranch(DateTime date)` method using traditional algorithm
- [ ] T012 Implement `CalculateYearStemBranch(int lunarYear)` method
- [ ] T013 Implement `CalculateMonthStemBranch(int lunarMonth, int yearStem)` method
- [ ] T014 Implement `CalculateHourStemBranch(TimeSpan time, int dayStem)` method
- [ ] T015 Implement helper methods for stem-branch index calculations and 60-cycle math

### Service Layer

- [ ] T016 Create `ISexagenaryService` interface in `src/LunarCalendar.Core/Services/ISexagenaryService.cs`
- [ ] T017 Implement `SexagenaryService` class in `src/LunarCalendar.Core/Services/SexagenaryService.cs` (wraps calculator with caching)
- [ ] T018 Add caching logic for current month +/- 1 month with LRU eviction
- [ ] T019 Register `ISexagenaryService` in dependency injection (`MauiProgram.cs`)

### Localization Resources

- [ ] T020 [P] Add stem names (10 entries) to `src/LunarCalendar.Core/Resources/Strings.vi.resx` (Vietnamese)
- [ ] T021 [P] Add branch names (12 entries) to `src/LunarCalendar.Core/Resources/Strings.vi.resx` (Vietnamese)
- [ ] T022 [P] Add stem names (10 entries) to `src/LunarCalendar.Core/Resources/Strings.en.resx` (English)
- [ ] T023 [P] Add branch names (12 entries) to `src/LunarCalendar.Core/Resources/Strings.en.resx` (English)
- [ ] T024 [P] Create `src/LunarCalendar.Core/Resources/Strings.zh.resx` and add Chinese stem/branch names

### Static Reference Data

- [ ] T025 [P] Create `src/LunarCalendar.Core/Data/HeavenlyStems.json` with stem metadata (elements, yin/yang)
- [ ] T026 [P] Create `src/LunarCalendar.Core/Data/EarthlyBranches.json` with branch metadata (zodiacs, time periods)

### Unit Tests (Phase 2 - REQUIRED by Constitution)

- [ ] T027 [P] Create `tests/LunarCalendar.Core.Tests/Models/HeavenlyStemTests.cs` - test enum and metadata
- [ ] T028 [P] Create `tests/LunarCalendar.Core.Tests/Models/EarthlyBranchTests.cs` - test enum and metadata
- [ ] T029 Create `tests/LunarCalendar.Core.Tests/Services/SexagenaryCalculatorTests.cs`
- [ ] T030 Add unit tests for `CalculateDayStemBranch()` with 50+ known dates
- [ ] T031 Add unit tests for `CalculateYearStemBranch()` with 20+ known years
- [ ] T032 Add unit tests for `CalculateMonthStemBranch()` with leap month handling
- [ ] T033 Add unit tests for `CalculateHourStemBranch()` with all 12 hour periods
- [ ] T034 Create `tests/LunarCalendar.Core.Tests/Services/SexagenaryServiceTests.cs`
- [ ] T035 Add service-level tests for caching behavior

### Historical Validation Tests (CRITICAL)

- [ ] T036 Create `tests/LunarCalendar.Core.Tests/Validation/HistoricalValidationTests.cs`
- [ ] T037 Load 1000+ dates from historical-validation-dataset.csv
- [ ] T038 Run validation: all 1000+ dates must match expected stem-branch with 100% accuracy
- [ ] T039 Add test for Lunar New Year boundary handling (year stem-branch changes correctly)
- [ ] T040 Add edge case tests for date range limits (1900-2100)

**Checkpoint**: Foundation ready - all calculations work, all tests pass (95%+ coverage), user story implementation can now begin

---

## Phase 3: User Story 1 - View Today's Stem-Branch Date (Priority: P1) 🎯 MVP

**Goal**: Display today's stem-branch in the calendar header so users immediately see traditional date format

**Independent Test**: Open the app and verify "Ngày Giáp Tý" (or equivalent) displays in header alongside Gregorian/lunar dates

### ViewModels

- [ ] T041 Update `MainViewModel` in `src/LunarCalendar.MobileApp/ViewModels/MainViewModel.cs`
- [ ] T042 Inject `ISexagenaryService` into MainViewModel constructor
- [ ] T043 Add `TodayStemBranch` property (string, observable)
- [ ] T044 Add `TodayElementColor` property (Color, observable) for visual element indicator
- [ ] T045 Implement `LoadTodaySexagenaryInfo()` method called on initialization
- [ ] T046 Handle language change events to update stem-branch display

### UI Implementation

- [ ] T047 Update `MainPage.xaml` in `src/LunarCalendar.MobileApp/Views/MainPage.xaml`
- [ ] T048 Add today's stem-branch label to header section (below or beside lunar date)
- [ ] T049 Add info icon (ⓘ) next to stem-branch with tap gesture
- [ ] T050 Implement tooltip/dialog showing "What is Can Chi?" explanation
- [ ] T051 Style stem-branch text (font size, color, emphasis)
- [ ] T052 Add visual element indicator (small colored dot or symbol)

### Localization

- [ ] T053 [P] Add "TodayStemBranch" label text to Strings.vi.resx, Strings.en.resx, Strings.zh.resx
- [ ] T054 [P] Add "WhatIsCanChi" tooltip text to all language resources
- [ ] T055 Update `LocalizationService` in `src/LunarCalendar.MobileApp/Services/LocalizationService.cs` if needed

### Tests (User Story 1)

- [ ] T056 Create `tests/LunarCalendar.MobileApp.Tests/ViewModels/MainViewModelTests.cs`
- [ ] T057 Add test: TodayStemBranch property loads correctly on initialization
- [ ] T058 Add test: Language change updates stem-branch display
- [ ] T059 Add test: Offline mode still displays stem-branch (no network calls)

**Checkpoint**: User Story 1 complete - Today's stem-branch visible on main screen, works offline, supports all languages

---

## Phase 4: User Story 2 - View Stem-Branch for Any Calendar Date (Priority: P2)

**Goal**: Show day stem-branch on all calendar cells so users can check any date for event planning

**Independent Test**: Navigate to any month, see stem-branch on each date cell, tap a date to see details

### ViewModels

- [ ] T060 Update `CalendarViewModel` in `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`
- [ ] T061 Add `ISexagenaryService` injection if not already present
- [ ] T062 Extend `CalendarDay` model to include `StemBranch` property (string)
- [ ] T063 Update `LoadMonthDates()` method to calculate stem-branch for each date
- [ ] T064 Implement batch calculation for performance (calculate all dates in month at once)
- [ ] T065 Add caching to avoid recalculating when user navigates back to same month

### UI Components

- [ ] T066 Create reusable control `StemBranchCell.xaml` in `src/LunarCalendar.MobileApp/Controls/`
- [ ] T067 Create `StemBranchCell.xaml.cs` code-behind
- [ ] T068 Design cell layout: Gregorian date (top), lunar date (middle), stem-branch (bottom, smaller)
- [ ] T069 Implement abbreviated stem-branch display for small screens (e.g., "甲子" instead of "Giáp Tý")
- [ ] T070 Add responsive logic: full names on tablets, abbreviated on phones

### Calendar Integration

- [ ] T071 Update `CalendarPage.xaml` in `src/LunarCalendar.MobileApp/Views/CalendarPage.xaml`
- [ ] T072 Replace existing date cell template with new `StemBranchCell` control
- [ ] T073 Ensure stem-branch text doesn't cause layout overflow or clipping
- [ ] T074 Test scrolling performance with stem-branch displayed on all cells (target: 60 FPS)

### Date Detail View

- [ ] T075 Update `DateDetailViewModel` in `src/LunarCalendar.MobileApp/ViewModels/DateDetailViewModel.cs`
- [ ] T076 Add properties: `DayStemBranch`, `DayElement`, `DayZodiac`, `DayYinYang`
- [ ] T077 Load full sexagenary info when date is selected
- [ ] T078 Update `DateDetailPage.xaml` in `src/LunarCalendar.MobileApp/Views/DateDetailPage.xaml`
- [ ] T079 Add "Stem-Branch Information" section with day's full details
- [ ] T080 Add visual element indicator (icon or color based on Five Element)
- [ ] T081 Add "Learn More" button linking to educational page

### Tests (User Story 2)

- [ ] T082 Update `tests/LunarCalendar.MobileApp.Tests/ViewModels/CalendarViewModelTests.cs`
- [ ] T083 Add test: All dates in month load with stem-branch data
- [ ] T084 Add test: Stem-branch updates correctly when navigating between months
- [ ] T085 Add test: Past dates (2020) and future dates (2027) calculate correctly
- [ ] T086 Create `tests/LunarCalendar.MobileApp.Tests/Integration/SexagenaryUITests.cs`
- [ ] T087 Add UI test: Calendar cells display stem-branch without layout issues
- [ ] T088 Add UI test: Tapping date shows full stem-branch details

**Checkpoint**: User Story 2 complete - All calendar dates show stem-branch, detail view works, performance maintained

---

## Phase 5: User Story 3 - View Year's Stem-Branch and Zodiac Information (Priority: P2)

**Goal**: Display year's zodiac animal and element so users understand the broader astrological context

**Independent Test**: View year indicator, see "Year of Fire Horse", tap for full year information

### Year Display Component

- [ ] T089 Create `ZodiacYearIndicator.xaml` control in `src/LunarCalendar.MobileApp/Controls/`
- [ ] T090 Create `ZodiacYearIndicator.xaml.cs` code-behind
- [ ] T091 Design layout: Year number + zodiac name + element (e.g., "2026 - Bính Ngọ - Fire Horse")
- [ ] T092 Add zodiac animal icon/image
- [ ] T093 Add tap gesture to show full year information dialog

### ViewModels

- [ ] T094 Update `CalendarViewModel` to add `CurrentYearStemBranch` property
- [ ] T095 Add `CurrentYearZodiac` property (e.g., "Fire Horse")
- [ ] T096 Add `CurrentYearElement` property
- [ ] T097 Load year sexagenary info when month/year changes
- [ ] T098 Handle Lunar New Year boundary correctly (zodiac changes at lunar new year, not Jan 1)

### Year Information Dialog/Page

- [ ] T099 Create `YearInfoViewModel` in `src/LunarCalendar.MobileApp/ViewModels/YearInfoViewModel.cs`
- [ ] T100 Add properties: year number, stem-branch, zodiac animal, element, characteristics
- [ ] T101 Load year characteristics from static data (e.g., "Year of Horse: energetic, independent...")
- [ ] T102 Create `YearInfoDialog.xaml` or sheet in `src/LunarCalendar.MobileApp/Views/`
- [ ] T103 Display full year information with zodiac image, element color, and description
- [ ] T104 Add cultural context about the zodiac year

### UI Integration

- [ ] T105 Update `CalendarPage.xaml` to include `ZodiacYearIndicator` in header
- [ ] T106 Ensure year indicator visible but doesn't clutter calendar
- [ ] T107 Test year indicator on both portrait and landscape orientations

### Static Data

- [ ] T108 [P] Create `src/LunarCalendar.Core/Data/ZodiacCharacteristics.json` with data for all 12 animals
- [ ] T109 [P] Add zodiac animal images to `src/LunarCalendar.MobileApp/Resources/Images/Zodiacs/`

### Tests (User Story 3)

- [ ] T110 Add tests to CalendarViewModelTests for year stem-branch loading
- [ ] T111 Add test: Year zodiac changes correctly at Lunar New Year boundary
- [ ] T112 Add test: Years before 2020 and after 2027 show correct zodiac
- [ ] T113 Add UI test: Year indicator displays and taps show details

**Checkpoint**: User Story 3 complete - Year zodiac displayed, year info dialog works, Lunar New Year boundary handled correctly

---

## Phase 6: User Story 4 - View Month and Hour Stem-Branch (Priority: P3)

**Goal**: Show month and hour stem-branch for advanced users doing detailed almanac consultation

**Independent Test**: Open date details, see month stem-branch, tap "View Hours", see 12 hour periods with stem-branches

### Month Stem-Branch

- [ ] T114 Update `DateDetailViewModel` to add `MonthStemBranch` property
- [ ] T115 Calculate month stem-branch when loading date details
- [ ] T116 Handle leap month display (show note if leap month)
- [ ] T117 Update `DateDetailPage.xaml` to display month stem-branch
- [ ] T118 Add section: "Month: Canh Dần (庚寅)" with element indicator

### Hour Stem-Branch

- [ ] T119 Add `HourStemBranches` property to DateDetailViewModel (collection of 12 items)
- [ ] T120 Calculate all 12 hour periods (two-hour increments: 11PM-1AM, 1AM-3AM, etc.)
- [ ] T121 Create `HourStemBranchViewModel` model: HourPeriod (string), StemBranch (string), Element, IsCurrentHour (bool)
- [ ] T122 Highlight current hour if viewing today's date
- [ ] T123 Create expandable section in `DateDetailPage.xaml`: "Hour Stem-Branches"
- [ ] T124 Design hour list: show time range, stem-branch, element indicator
- [ ] T125 Add tap gesture on hour to show detailed hour information

### Hour Detail Dialog (optional enhancement)

- [ ] T126 Create `HourInfoDialog.xaml` showing detailed hour characteristics
- [ ] T127 Include zodiac animal for the hour, element, traditional meanings

### Tests (User Story 4)

- [ ] T128 Add tests to DateDetailViewModelTests for month stem-branch
- [ ] T129 Add test: Leap months handled correctly
- [ ] T130 Add test: All 12 hour periods calculated correctly
- [ ] T131 Add test: Current hour highlighted correctly when viewing today
- [ ] T132 Add UI test: Hour list expands and displays all periods

**Checkpoint**: User Story 4 complete - Month and hour stem-branches display correctly in date details

---

## Phase 7: User Story 5 - Educational Tooltips (Priority: P3)

**Goal**: Help users understand the stem-branch system through contextual education

**Independent Test**: Tap info icons throughout app, see clear explanations, access reference page

### Educational Content

- [ ] T133 [P] Create `src/LunarCalendar.Core/Data/SexagenaryEducation.json` with educational content
- [ ] T134 [P] Include explanations for: sexagenary cycle, heavenly stems, earthly branches, five elements, zodiac animals
- [ ] T135 [P] Add content in all three languages (vi, en, zh)

### Reference Page

- [ ] T136 Create `SexagenaryEducationViewModel` in `src/LunarCalendar.MobileApp/ViewModels/`
- [ ] T137 Load educational content from JSON
- [ ] T138 Organize content into sections: Overview, Stems, Branches, Elements, Zodiacs
- [ ] T139 Create `SexagenaryEducationPage.xaml` in `src/LunarCalendar.MobileApp/Views/`
- [ ] T140 Design tabbed or scrollable layout with sections
- [ ] T141 Display all 10 Heavenly Stems with details (element, yin/yang, meaning)
- [ ] T142 Display all 12 Earthly Branches with details (zodiac, time period, element)
- [ ] T143 Add visual cycle diagram showing 60-year cycle
- [ ] T144 Include cultural references and sources

### Tooltips and Info Icons

- [ ] T145 Create reusable `InfoTooltip` control in `src/LunarCalendar.MobileApp/Controls/`
- [ ] T146 Add info icons (ⓘ) to: MainPage header, CalendarPage year indicator, DateDetailPage sections
- [ ] T147 Implement tooltip display on tap (show brief explanation)
- [ ] T148 Add "Learn More" link in tooltips navigating to education page

### Onboarding

- [ ] T149 Create first-time user onboarding flow for sexagenary feature
- [ ] T150 Show brief introduction: "We've added traditional stem-branch dates (Can Chi)"
- [ ] T151 Add "Learn More" button to education page
- [ ] T152 Store onboarding completion in local preferences to show only once

### Navigation

- [ ] T153 Add "About Stem-Branch System" menu item to app settings or help menu
- [ ] T154 Ensure education page accessible from multiple entry points

### Tests (User Story 5)

- [ ] T155 Create tests for SexagenaryEducationViewModel
- [ ] T156 Add test: Educational content loads correctly
- [ ] T157 Add test: All languages have complete education content
- [ ] T158 Add UI test: Info icons display tooltips
- [ ] T159 Add UI test: Navigation to education page works

**Checkpoint**: User Story 5 complete - Educational content accessible, tooltips work, onboarding shown to new users

---

## Phase 8: Backend API (Optional - Non-Blocking)

**Goal**: Provide API endpoints for potential future web app or third-party integrations

**Note**: This can run in parallel with any mobile UI work after Phase 2 foundation is complete

### API Controllers

- [ ] T160 [P] Create `SexagenaryController` in `src/LunarCalendar.Api/Controllers/SexagenaryController.cs`
- [ ] T161 [P] Implement `GET /api/calendar/sexagenary/{date}` endpoint
- [ ] T162 [P] Implement `GET /api/calendar/year-info/{year}` endpoint
- [ ] T163 [P] Implement `GET /api/calendar/stems` (reference list) endpoint
- [ ] T164 [P] Implement `GET /api/calendar/branches` (reference list) endpoint

### API Services

- [ ] T165 Create `SexagenaryApiService` in `src/LunarCalendar.Api/Services/` (wraps Core service)
- [ ] T166 Add request validation (date range 1900-2100)
- [ ] T167 Add response caching headers (ETags, Cache-Control: max-age=31536000)
- [ ] T168 Implement localization based on Accept-Language header

### API Models

- [ ] T169 [P] Create `SexagenaryResponse` DTO in `src/LunarCalendar.Api/Models/`
- [ ] T170 [P] Create `YearInfoResponse` DTO
- [ ] T171 [P] Create `StemResponse` and `BranchResponse` DTOs

### API Documentation

- [ ] T172 Add XML comments to all API endpoints
- [ ] T173 Update Swagger configuration to include sexagenary endpoints
- [ ] T174 Add example requests and responses to Swagger docs

### API Tests

- [ ] T175 Create `tests/LunarCalendar.Api.Tests/Controllers/SexagenaryControllerTests.cs`
- [ ] T176 Add test: GET sexagenary endpoint returns correct data
- [ ] T177 Add test: Invalid date returns 400 Bad Request
- [ ] T178 Add test: Out-of-range date returns 400 with clear error
- [ ] T179 Add test: Caching headers present in response
- [ ] T180 Add integration test: Full API request/response cycle

**Checkpoint**: API endpoints functional, documented, and tested

---

## Phase 9: Performance Optimization & Polish

**Purpose**: Ensure performance targets met and UI polished

### Performance Testing

- [ ] T181 Run performance benchmarks: single date calculation must be < 50ms
- [ ] T182 Benchmark full month calculation (30 dates) must be < 100ms total
- [ ] T183 Test calendar scrolling performance with profiler (target: > 55 FPS)
- [ ] T184 Profile memory usage - ensure caching doesn't cause memory bloat
- [ ] T185 Test on low-end devices (oldest supported iOS 15 and Android API 26 devices)

### Performance Optimization

- [ ] T186 Optimize calculation algorithms if benchmarks not met
- [ ] T187 Implement lazy loading for educational content
- [ ] T188 Optimize image assets (zodiac images) for size
- [ ] T189 Review and optimize XAML layouts for render performance

### UI Polish

- [ ] T190 Review all stem-branch displays for consistent styling
- [ ] T191 Ensure text contrast meets accessibility standards (WCAG AA)
- [ ] T192 Test all animations and transitions are smooth
- [ ] T193 Verify no layout clipping or overflow on small screens
- [ ] T194 Test on both portrait and landscape orientations
- [ ] T195 Polish spacing, alignment, colors throughout feature

### Cross-Platform Testing

- [ ] T196 Test on iPhone (small screen, large screen, SE size)
- [ ] T197 Test on iPad (all orientations)
- [ ] T198 Test on Android phone (various screen sizes)
- [ ] T199 Test on Android tablet
- [ ] T200 Test on iOS 15, iOS 16, iOS 17, iOS 18
- [ ] T201 Test on Android API 26, API 30, API 33, API 36

### Accessibility

- [ ] T202 Test with VoiceOver (iOS) - all stem-branch elements announced correctly
- [ ] T203 Test with TalkBack (Android) - all elements accessible
- [ ] T204 Add accessibility labels to all visual-only elements
- [ ] T205 Ensure minimum touch target sizes (44x44 pt)
- [ ] T206 Test with dynamic text sizes (accessibility settings)

**Checkpoint**: Performance targets met, UI polished, accessibility verified

---

## Phase 10: Documentation & Release Preparation

**Purpose**: Prepare for production release

### Code Documentation

- [ ] T207 [P] Add XML documentation comments to all public classes in Core library
- [ ] T208 [P] Add inline comments explaining complex calculation logic
- [ ] T209 [P] Document algorithm sources and references in code

### User Documentation

- [ ] T210 Update `README.md` with new Phase 2 features
- [ ] T211 Create user guide for stem-branch feature in `docs/user-guide-sexagenary.md`
- [ ] T212 Add screenshots showing stem-branch displays
- [ ] T213 Document how to read stem-branch dates
- [ ] T214 Explain Lunar New Year boundary behavior

### Technical Documentation

- [ ] T215 Update `docs/TECHNICAL_ARCHITECTURE.md` with sexagenary service architecture
- [ ] T216 Document calculation algorithms and sources
- [ ] T217 Update API documentation with new endpoints
- [ ] T218 Create migration guide for existing users (new feature announcement)

### Release Notes

- [ ] T219 Write release notes for v2.0.0 Phase 2 Sprint 9
- [ ] T220 Highlight: "Traditional stem-branch dates (Can Chi / 干支) now displayed"
- [ ] T221 Include screenshots and examples
- [ ] T222 Explain value to users (authentic cultural calendar)

### App Store Updates

- [ ] T223 Update App Store description to mention traditional features
- [ ] T224 Create new screenshots showing stem-branch displays
- [ ] T225 Update Google Play Store description
- [ ] T226 Prepare promotional graphics highlighting new feature

### Final Validation

- [ ] T227 Run full test suite (all 100+ tests must pass)
- [ ] T228 Verify all 1000+ historical validation dates pass
- [ ] T229 Confirm test coverage meets 95%+ target for Core library
- [ ] T230 Cultural accuracy review with Vietnamese SME
- [ ] T231 Final code review with team
- [ ] T232 Security review (no sensitive data exposed)

**Checkpoint**: Documentation complete, release materials ready

---

## Phase 11: Deployment & Monitoring

**Purpose**: Deploy to production and monitor

### Build & Deploy

- [ ] T233 Merge feature branch to main after all approvals
- [ ] T234 Tag release: `v2.0.0-phase2-sprint9`
- [ ] T235 Build production release (iOS and Android)
- [ ] T236 Submit iOS build to TestFlight
- [ ] T237 Submit Android build to Internal Testing track

### Phased Rollout

- [ ] T238 Release to 10% of users (monitor for 2 days)
- [ ] T239 If stable, increase to 25% (monitor for 2 days)
- [ ] T240 If stable, increase to 50% (monitor for 2 days)
- [ ] T241 If stable, increase to 100% (full release)

### Monitoring

- [ ] T242 Set up alerts for crash rate increase
- [ ] T243 Monitor performance metrics (app startup time, FPS)
- [ ] T244 Monitor API endpoints (if deployed)
- [ ] T245 Track user engagement with new feature (analytics events)
- [ ] T246 Monitor app reviews for feedback

### Rollback Plan

- [ ] T247 Document rollback procedure if critical issues found
- [ ] T248 Prepare hotfix branch if needed
- [ ] T249 Have feature flag ready to disable stem-branch display remotely (if possible)

**Checkpoint**: Feature deployed to production, monitoring active

---

## Dependencies & Execution Order

### Phase Dependencies

1. **Setup (Phase 1)**: No dependencies - start immediately ✅
2. **Foundational (Phase 2)**: Depends on Setup - **BLOCKS all user stories** ⚠️
3. **User Story 1 (Phase 3)**: Can start after Phase 2 complete
4. **User Story 2 (Phase 4)**: Can start after Phase 2 complete (may integrate US1)
5. **User Story 3 (Phase 5)**: Can start after Phase 2 complete (may integrate US1)
6. **User Story 4 (Phase 6)**: Can start after Phase 2 complete (depends on US2 DateDetailPage)
7. **User Story 5 (Phase 7)**: Can start after Phase 2 complete (integrates with all UIs)
8. **Backend API (Phase 8)**: Can start after Phase 2 complete - runs in parallel ⚡
9. **Performance & Polish (Phase 9)**: After all desired user stories complete
10. **Documentation (Phase 10)**: After implementation complete
11. **Deployment (Phase 11)**: After documentation and final validation

### Critical Path (Sequential)

```
Phase 1 Setup → Phase 2 Foundation → Phase 3 US1 → Phase 4 US2 → Phase 9 Polish → Phase 10 Docs → Phase 11 Deploy
```

### Parallel Opportunities

**Within Phase 2 (Foundation)**:
- T005-T009 (all model enums) can run in parallel
- T020-T026 (all localization/data files) can run in parallel
- T027-T028 (model tests) can run in parallel after models done

**After Phase 2 Complete**:
- Phase 3 (US1), Phase 4 (US2), Phase 5 (US3) can run in parallel with different developers
- Phase 8 (Backend API) can run completely in parallel with mobile UI work

**Within Phases**:
- Most [P] marked tasks can run in parallel within their phase

---

## Implementation Strategy Recommendations

### MVP-First (2 weeks, Minimum Viable):

1. ✅ Phase 1: Setup (day 1)
2. ✅ Phase 2: Foundation (days 2-5)
3. ✅ Phase 3: User Story 1 only (days 6-8)
4. ✅ Phase 9: Performance & Polish (days 9-10)
5. ✅ Phase 10: Documentation (days 11-12)
6. ✅ Phase 11: Deploy (days 13-14)

**Result**: Users can see today's stem-branch date (core value delivered)

### Full Feature (2 weeks, All User Stories):

**Week 1**:
- Days 1-2: Phase 1 + Phase 2 (Foundation)
- Days 3-4: Phase 3 (US1) + Phase 4 (US2)
- Day 5: Phase 5 (US3)

**Week 2**:
- Days 6-7: Phase 6 (US4) + Phase 7 (US5)
- Days 8-9: Phase 9 (Performance & Polish)
- Days 10-11: Phase 10 (Documentation)
- Days 12-14: Phase 11 (Deploy & Monitor)

Backend API (Phase 8) runs in parallel throughout if backend developer available.

### Parallel Team (1.5 weeks, Fastest):

If you have 3 developers:

**Week 1**:
- All: Phase 1 + Phase 2 together (days 1-3)
- Dev A: Phase 3 (US1) (days 4-5)
- Dev B: Phase 4 (US2) (days 4-5)
- Dev C: Phase 8 (Backend API) (days 4-5)

**Week 2**:
- Dev A: Phase 5 (US3) (days 6-7)
- Dev B: Phase 6 (US4) (days 6-7)
- Dev C: Phase 7 (US5) (days 6-7)
- All: Phase 9 (Performance) (days 8-9)
- All: Phase 10 + 11 (Documentation & Deploy) (days 10-11)

---

## Success Metrics

**Completion Criteria**:
- ✅ All tasks in selected phases complete
- ✅ All 100+ unit tests pass
- ✅ 1000+ historical validation dates pass (100% accuracy)
- ✅ Performance benchmarks met (< 50ms, > 55 FPS)
- ✅ Test coverage > 95% for Core library
- ✅ Code reviewed and approved
- ✅ Documentation complete
- ✅ Cultural accuracy validated by SME

**Ready for Production**:
- ✅ Zero P0/P1 bugs
- ✅ Accessibility compliance verified
- ✅ Cross-platform testing complete (iOS + Android)
- ✅ Release notes and app store materials ready
- ✅ Monitoring and rollback plan in place

---

## Notes

- All [P] tasks can run in parallel (different files)
- [Story] tag (US1-US5) enables independent story tracking
- Phase 2 Foundation is **CRITICAL** - nothing else can proceed until complete
- Tests are **REQUIRED** by Constitution (not optional)
- Historical validation with 1000+ dates is **NON-NEGOTIABLE** for cultural accuracy
- Performance targets are **REQUIRED** (< 50ms, > 55 FPS)
- Each user story should be independently testable after its phase completes
- Backend API (Phase 8) is optional and non-blocking

---

**Total Tasks**: 249  
**Estimated Duration**: 2 weeks (Sprint 9)  
**Next Step**: Start with Phase 1 (Setup), complete Phase 2 (Foundation), then implement user stories in priority order
