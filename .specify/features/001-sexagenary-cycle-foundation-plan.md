# Implementation Plan: Sexagenary Cycle Foundation

**Branch**: `feature/001-sexagenary-cycle` | **Date**: January 25, 2026 | **Spec**: [001-sexagenary-cycle-foundation.md](./001-sexagenary-cycle-foundation.md)

## Summary

Implement the traditional Chinese/Vietnamese Sexagenary Cycle (Can Chi / 干支) calculation and display system. This provides authentic lunar calendar functionality by showing dates in the traditional 60-year cycle format combining 10 Heavenly Stems and 12 Earthly Branches. The implementation will add stem-branch calculations for years, months, days, and hours, display them throughout the calendar UI, and provide educational context to help users understand this traditional dating system.

**Technical Approach**: Extend the existing offline-first LunarCalendar.Core library with a new SexagenaryService that implements the mathematical algorithms for calculating stem-branch positions. Use the existing ChineseLunisolarCalendar as the foundation for lunar date input, then apply stem-branch formulas. Display updates will integrate into the existing MAUI MVVM architecture with minimal UI changes to calendar cells and headers.

## Technical Context

**Language/Version**: C# 12 with .NET 10.0  
**Framework**: .NET MAUI (Multi-platform App UI)  
**Primary Dependencies**: 
- System.Globalization.ChineseLunisolarCalendar (built-in)
- CommunityToolkit.Mvvm 8.2.2 (MVVM helpers)
- sqlite-net-pcl 1.9.172 (local caching)

**Storage**: SQLite for caching localized string resources and educational content; calculation results are ephemeral (not persisted)  
**Testing**: xUnit with FluentAssertions for unit tests, MAUI UI testing for integration  
**Target Platform**: iOS 15.0+, Android API 26+ (cross-platform mobile)  
**Project Type**: Mobile application with shared Core library  
**Performance Goals**: 
- < 50ms per date calculation
- < 100ms to render monthly calendar view with stem-branches
- 60 FPS scrolling performance

**Constraints**: 
- Offline-first: All calculations must work without network
- Date range: 1900-2100 (ChineseLunisolarCalendar limitation)
- Cultural accuracy: Validated against historical references
- Bilingual: Vietnamese, Chinese, English support required

**Scale/Scope**: 
- ~10 new classes in Core library
- ~5 ViewModels updated in MobileApp
- ~8 XAML views modified
- ~15 new API endpoints in optional backend
- ~100+ unit tests for calculation accuracy

## Constitution Check

✅ **I. Offline-First Architecture**: COMPLIANT  
All stem-branch calculations execute on-device using mathematical algorithms. No network dependency for core functionality.

✅ **II. Cultural Accuracy & Authenticity**: COMPLIANT  
Algorithm will be validated against authoritative Vietnamese sources and historical dates. Educational content will be reviewed by cultural SMEs.

✅ **III. Privacy & Guest-First Design**: COMPLIANT  
No authentication required. Feature fully accessible to guest users. No data collection.

✅ **IV. Cross-Platform Consistency**: COMPLIANT  
Shared calculation logic in LunarCalendar.Core. Platform-specific UI adjustments isolated to Platforms/ folders.

✅ **V. Performance & Responsiveness**: COMPLIANT  
Calculation caching strategy ensures < 50ms per date. Pre-calculation during app init for current month.

✅ **VI. Bilingual Support**: COMPLIANT  
All stem/branch names will use .resx localization files (Vietnamese, Chinese, English).

✅ **VII. Test Coverage & Quality Assurance**: COMPLIANT  
Target 90%+ coverage for SexagenaryService. Historical validation dataset of 1000+ dates.

**Gate Result**: ✅ PASS - All constitutional requirements met. Proceed with implementation.

## Project Structure

### Documentation (this feature)

```text
.specify/features/001-sexagenary-cycle/
├── 001-sexagenary-cycle-foundation.md           # Specification
├── 001-sexagenary-cycle-foundation-plan.md      # This file
├── tasks.md                                      # Task breakdown (from /speckit.tasks)
└── research/
    ├── sexagenary-algorithm.md                  # Algorithm research notes
    ├── historical-validation-dataset.csv        # Test data
    └── cultural-references.md                   # Sources for accuracy
```

### Source Code (repository root)

```text
src/
├── LunarCalendar.Core/                          # Shared business logic
│   ├── Models/
│   │   ├── SexagenaryInfo.cs                   # NEW: Full stem-branch data
│   │   ├── HeavenlyStem.cs                     # NEW: 10 Heavenly Stems enum & data
│   │   ├── EarthlyBranch.cs                    # NEW: 12 Earthly Branches enum & data
│   │   ├── FiveElement.cs                      # NEW: Five elements enum
│   │   └── ZodiacAnimal.cs                     # NEW: 12 zodiac animals enum
│   │
│   ├── Services/
│   │   ├── SexagenaryService.cs                # NEW: Core calculation service
│   │   ├── SexagenaryCalculator.cs             # NEW: Pure calculation algorithms
│   │   └── ICalendarService.cs                 # MODIFIED: Add GetSexagenaryInfo()
│   │
│   ├── Data/
│   │   ├── HeavenlyStems.json                  # NEW: Stem data with elements
│   │   ├── EarthlyBranches.json                # NEW: Branch data with zodiacs
│   │   └── SexagenaryEducation.json            # NEW: Educational content
│   │
│   └── Resources/
│       ├── Strings.en.resx                      # MODIFIED: Add stem/branch names
│       ├── Strings.vi.resx                      # MODIFIED: Add stem/branch names
│       └── Strings.zh.resx                      # NEW: Chinese stem/branch names
│
├── LunarCalendar.MobileApp/                     # MAUI mobile app
│   ├── ViewModels/
│   │   ├── MainViewModel.cs                    # MODIFIED: Add TodayStemBranch
│   │   ├── CalendarViewModel.cs                # MODIFIED: Include stem-branch in dates
│   │   ├── DateDetailViewModel.cs              # MODIFIED: Show full sexagenary info
│   │   └── SexagenaryEducationViewModel.cs     # NEW: Educational reference view
│   │
│   ├── Views/
│   │   ├── MainPage.xaml                       # MODIFIED: Display today's stem-branch
│   │   ├── CalendarPage.xaml                   # MODIFIED: Show stem-branch on cells
│   │   ├── DateDetailPage.xaml                 # MODIFIED: Full stem-branch details
│   │   └── SexagenaryEducationPage.xaml        # NEW: Reference & education page
│   │
│   ├── Controls/
│   │   ├── StemBranchCell.xaml                 # NEW: Reusable stem-branch display
│   │   └── ZodiacYearIndicator.xaml            # NEW: Year zodiac display control
│   │
│   ├── Services/
│   │   └── LocalizationService.cs              # MODIFIED: Add stem-branch localization
│   │
│   └── Data/
│       └── LocalDatabase.cs                    # MODIFIED: Add caching tables
│
├── LunarCalendar.Api/                           # Optional backend API
│   ├── Controllers/
│   │   └── SexagenaryController.cs             # NEW: API endpoints for stem-branch
│   │
│   ├── Services/
│   │   └── SexagenaryApiService.cs             # NEW: Wraps Core service
│   │
│   └── Models/
│       └── SexagenaryResponse.cs               # NEW: API response DTOs
│
└── tests/
    ├── LunarCalendar.Core.Tests/
    │   ├── SexagenaryServiceTests.cs           # NEW: Service tests
    │   ├── SexagenaryCalculatorTests.cs        # NEW: Algorithm validation (1000+ dates)
    │   ├── HeavenlyStemTests.cs                # NEW: Stem model tests
    │   └── EarthlyBranchTests.cs               # NEW: Branch model tests
    │
    ├── LunarCalendar.MobileApp.Tests/
    │   ├── ViewModels/
    │   │   ├── MainViewModelTests.cs           # MODIFIED: Test stem-branch display
    │   │   └── CalendarViewModelTests.cs       # MODIFIED: Test stem-branch in calendar
    │   │
    │   └── Integration/
    │       └── SexagenaryUITests.cs            # NEW: UI integration tests
    │
    └── LunarCalendar.Api.Tests/
        └── SexagenaryControllerTests.cs        # NEW: API endpoint tests
```

**Structure Decision**: Using the existing hybrid mobile + optional API structure. The Core library approach aligns with constitutional requirement IV (Cross-Platform Consistency) by keeping all calculation logic in the shared LunarCalendar.Core project. Mobile app updates follow existing MVVM pattern. Backend API is supplementary and not required for core functionality (offline-first).

## Architecture Decisions

### AD-001: Calculation Strategy - On-Device vs. API

**Decision**: Implement all sexagenary calculations on-device in LunarCalendar.Core library.

**Rationale**:
- Constitutional requirement I (Offline-First) mandates network independence
- Calculations are deterministic and algorithmic (no dynamic data needed)
- Performance target of < 50ms is easily achievable with local calculation
- Eliminates API latency and dependency

**Alternative Considered**: API-based calculation with local caching  
**Rejected Because**: Violates offline-first principle; adds unnecessary complexity and failure points

### AD-002: Data Model - Enums vs. Database Tables

**Decision**: Use C# enums for Heavenly Stems and Earthly Branches with static data classes for metadata.

**Rationale**:
- Only 10 stems and 12 branches - small, fixed datasets
- Never changes (historical constants)
- Enums provide compile-time safety and better performance
- Metadata (names, elements) stored in static readonly dictionaries
- Localized names still use .resx for proper i18n

**Alternative Considered**: Database tables with rows for each stem/branch  
**Rejected Because**: Overkill for static data; adds query overhead; no dynamic requirements

### AD-003: Caching Strategy

**Decision**: In-memory caching for current month +/- 1 month with LRU eviction. SQLite only for localized educational content.

**Rationale**:
- Calculations are fast (< 50ms), caching is optimization not requirement
- Memory footprint is tiny (~30 dates × ~200 bytes = 6KB)
- Users primarily view current month
- SQLite caching adds disk I/O overhead without significant benefit

**Alternative Considered**: Aggressive SQLite caching of all calculated dates  
**Rejected Because**: Disk I/O slower than recalculation; violates YAGNI (You Aren't Gonna Need It)

### AD-004: Algorithm Source

**Decision**: Implement sexagenary algorithm from authoritative Chinese astronomical algorithms (Jean Meeus references + Vietnamese cultural sources).

**Rationale**:
- Proven accuracy over centuries
- Well-documented mathematical formulas
- Can be validated against historical records
- Respects cultural authenticity (Constitutional requirement II)

**Alternative Considered**: Use third-party library (e.g., lunar-calendar npm package)  
**Rejected Because**: Most libraries are JavaScript (not .NET); black-box implementation reduces cultural validation; adds dependency risk

### AD-005: UI Integration Approach

**Decision**: Augment existing calendar cells with stem-branch display rather than creating separate view.

**Rationale**:
- Users want stem-branch in context of their calendar usage
- Reduces navigation complexity
- Aligns with constitutional requirement V (Performance) by minimizing view switches
- Maintains consistency with existing UI patterns

**Alternative Considered**: Separate "Traditional View" toggle  
**Rejected Because**: Creates fragmentation; users shouldn't have to choose between modern and traditional

## Implementation Phases

### Phase 0: Research & Validation (Completed)
✅ Algorithm research completed  
✅ Historical validation dataset prepared (1000+ dates)  
✅ Cultural SME review scheduled  

### Phase 1: Core Library Implementation

**Milestone**: Calculation engine works and passes validation tests

**Tasks**:
1. Create data models (HeavenlyStem, EarthlyBranch, FiveElement, ZodiacAnimal, SexagenaryInfo)
2. Implement SexagenaryCalculator with pure calculation methods:
   - CalculateDayStemBranch(DateTime date)
   - CalculateMonthStemBranch(int lunarMonth, int yearStem)
   - CalculateYearStemBranch(int lunarYear)
   - CalculateHourStemBranch(TimeSpan time, int dayStem)
3. Implement SexagenaryService wrapping calculator with caching
4. Add localization resources for stem/branch names (vi, en, zh)
5. Write comprehensive unit tests with historical validation dataset
6. Validate against 1000+ historical dates
7. Performance benchmark (target: < 50ms per calculation)

**Acceptance Criteria**:
- All 1000+ validation dates pass with 100% accuracy
- Unit test coverage > 95%
- Performance benchmarks meet < 50ms target
- Code reviewed and approved

### Phase 2: Mobile UI Integration

**Milestone**: Stem-branch displays in calendar app on both iOS and Android

**Tasks**:
1. Update MainViewModel to display today's stem-branch in header
2. Modify CalendarViewModel to include stem-branch for each date cell
3. Create StemBranchCell.xaml control for reusable display
4. Update CalendarPage.xaml to show stem-branch on date cells
5. Modify DateDetailViewModel to show full sexagenary info (year/month/day/hour)
6. Update DateDetailPage.xaml with expanded stem-branch section
7. Create ZodiacYearIndicator.xaml control for year display
8. Add stem-branch to year selector/indicator
9. Implement responsive layout (abbreviate on small screens)
10. Add info icons (ⓘ) with tooltips throughout
11. Test on multiple device sizes (iPhone, iPad, Android phone, tablet)
12. Test in all three languages (vi, en, zh)

**Acceptance Criteria**:
- Today's stem-branch visible on main screen
- All calendar dates show day stem-branch
- Date details show full year/month/day/hour info
- UI remains responsive (> 55 FPS scrolling)
- Works correctly in all languages
- Manual testing passed on iOS and Android

### Phase 3: Educational Features

**Milestone**: Users can learn about stem-branch system

**Tasks**:
1. Create SexagenaryEducationViewModel
2. Create SexagenaryEducationPage.xaml with:
   - List of 10 Heavenly Stems with details
   - List of 12 Earthly Branches with zodiac info
   - Explanation of 60-year cycle
   - Visual cycle diagram
3. Implement tooltip system for inline education
4. Add "Learn More" navigation from calendar
5. Create onboarding flow for first-time feature usage
6. Add cultural context and sources in app

**Acceptance Criteria**:
- Educational page accessible from menu
- Tooltips display correct information
- Content culturally accurate (SME validated)
- First-time users see onboarding

### Phase 4: Backend API (Optional)

**Milestone**: API endpoints available for future web app or integrations

**Tasks**:
1. Create SexagenaryController with endpoints:
   - GET /api/calendar/sexagenary/{date}
   - GET /api/calendar/year-info/{year}
   - GET /api/calendar/stems (reference list)
   - GET /api/calendar/branches (reference list)
2. Implement SexagenaryApiService wrapping Core service
3. Add caching headers (ETags, Cache-Control)
4. Add Swagger documentation
5. Write API integration tests
6. Deploy to development environment

**Acceptance Criteria**:
- All endpoints return correct data
- Response times < 200ms (p95)
- API documentation complete
- Integration tests pass
- Deployed and accessible

### Phase 5: Testing & Polish

**Milestone**: Production-ready quality

**Tasks**:
1. Comprehensive cross-platform testing (iOS + Android)
2. Accessibility testing (VoiceOver, TalkBack)
3. Performance profiling and optimization
4. Bug fixes from testing
5. Cultural accuracy final review with SME
6. Localization quality review
7. Documentation updates (README, user guide)
8. Prepare release notes

**Acceptance Criteria**:
- Zero P0/P1 bugs
- Accessibility compliance
- Performance targets met
- Cultural accuracy validated
- All documentation updated
- Release notes approved

## Risk Management

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| **Algorithm accuracy errors** | High | Medium | Validate against 1000+ historical dates; SME review; compare with multiple reference sources |
| **ChineseLunisolarCalendar limitations (1900-2100)** | Medium | High | Document limitation clearly; graceful error handling for out-of-range dates |
| **Performance degradation on low-end devices** | Medium | Low | Benchmark on oldest supported devices; implement caching; optimize calculations |
| **Cultural inaccuracy in translations** | High | Medium | Native speaker review; SME validation; multiple authoritative sources |
| **UI clutter from additional information** | Medium | Medium | Careful UX design; responsive layouts; progressive disclosure; user testing |
| **Calculation differences vs. other calendar apps** | High | Medium | Document algorithm source; provide references; allow users to report discrepancies |
| **Localization errors** | Medium | Low | Native speaker review; automated localization testing; placeholder detection |

## Testing Strategy

### Unit Testing (Target: 95%+ coverage)
- **SexagenaryCalculator**: Test each calculation method with known values
- **Historical Validation**: 1000+ date dataset covering entire 1900-2100 range
- **Edge Cases**: Leap months, lunar new year boundaries, year transitions
- **Localization**: All three languages render correctly

### Integration Testing
- **ViewModel Integration**: Test data flow from service to UI
- **Calendar Display**: Verify stem-branch appears on all date cells
- **Detail Views**: Verify full sexagenary info displays correctly
- **Language Switching**: Test dynamic language change updates all displays

### UI Testing
- **Automated**: MAUI UI tests for critical flows
- **Manual**: Test on real devices (iPhone, iPad, Android phone, tablet)
- **Accessibility**: VoiceOver/TalkBack testing
- **Responsive**: Test on various screen sizes and orientations

### Performance Testing
- **Calculation Benchmark**: Measure single date calculation time
- **Bulk Calculation**: Measure time to calculate full month (30 dates)
- **Scroll Performance**: Verify 60 FPS maintained with stem-branch display
- **Memory Profiling**: Ensure no memory leaks from caching

### Cultural Accuracy Validation
- **SME Review**: Vietnamese cultural expert reviews all content
- **Reference Comparison**: Compare calculations with authoritative almanacs
- **User Testing**: Beta testers with traditional calendar knowledge verify accuracy

## Success Metrics

**Functionality**:
- ✅ All 5 user stories implemented and accepted
- ✅ All 32 functional requirements met
- ✅ 100% accuracy on 1000+ validation dates

**Performance**:
- ✅ < 50ms per date calculation (average)
- ✅ < 100ms to render monthly calendar
- ✅ > 55 FPS scrolling performance

**Quality**:
- ✅ > 95% unit test coverage
- ✅ Zero P0/P1 bugs at release
- ✅ Passes accessibility standards

**User Acceptance**:
- ✅ 80%+ beta testers rate feature as "useful" or higher
- ✅ Cultural accuracy validated by SMEs
- ✅ Positive feedback on educational content

## Dependencies

**Blockers** (must be completed first):
- None - This is the foundational feature of Phase 2

**Dependencies** (required for this feature):
- ✅ Phase 1 MVP completed (calendar infrastructure exists)
- ✅ ChineseLunisolarCalendar available (.NET built-in)
- ✅ Localization infrastructure in place

**Dependent Features** (blocked by this feature):
- Sprint 10: Zodiac Animals & Year Characteristics (needs SexagenaryService)
- Sprint 11: Dynamic Backgrounds (needs year zodiac from this feature)
- Sprint 15: Auspicious Dates (needs stem-branch calculations)

## Rollout Plan

### Development
1. Feature branch: `feature/001-sexagenary-cycle`
2. Regular commits with tests
3. Code review before merge to `main`

### Testing
1. Unit tests run on every commit (CI/CD)
2. Integration testing on staging build
3. Beta release to internal testers (50-100 users)
4. Collect feedback for 1 week
5. Bug fixes and polish

### Release
1. Merge to `main` after approval
2. Tag release: `v2.0.0-phase2-sprint9`
3. Build production release
4. Submit to TestFlight (iOS) and Internal Testing (Android)
5. Phased rollout: 10% → 25% → 50% → 100% over 2 weeks
6. Monitor crash reports and performance metrics

### Rollback Plan
If critical issues discovered:
- Revert feature flag (disable stem-branch display)
- Hotfix release with feature disabled
- Fix issues and re-enable in next release

---

**Next Steps**: Use `/speckit.tasks` to break down each implementation phase into specific, actionable development tasks.
