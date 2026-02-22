# Task Breakdown: Zodiac Animals & Year Characteristics

**Feature ID**: 002  
**Branch**: `feature/002-zodiac-animals`  
**Sprint**: 10 (Phase 2)  
**Inputs**: `002-zodiac-animals.md`, `002-zodiac-animals-plan.md`, `SPRINT10-DECISIONS.md`  
**Decisions baked in**: ✅ Emoji-only zodiac visuals (Sprint 10) • ✅ Vietnamese + English only (Sprint 10)

---

## Goals and guardrails

- Deliver **P1 stories first** (header + info page), then P2 (compatibility + elemental variations), then P3 (multi-entry UX polish).
- Keep **offline-first**: bundled JSON/resource data only.
- Ensure **Lunar New Year boundary** logic is correct (zodiac changes at Lunar New Year, not Jan 1).
- Implement **emoji-based display** (no SVG/PNG asset pipeline in Sprint 10).
- Localization for Sprint 10 is **English + Vietnamese only** (Chinese UI localization deferred to Sprint 14; Chinese characters allowed as cultural reference content in data).

---

## Phase 0 — Setup & alignment

### T001: Create feature branch and task scaffolding
- **Priority**: P1
- **Estimate**: 1h
- **Dependencies**: None
- **Scope**:
  - Included: Branch creation; add feature status tracking doc if used by repo conventions.
  - Excluded: Any code changes.
- **Acceptance criteria**:
  - Feature branch `feature/002-zodiac-animals` exists.
  - Task doc exists and is committed.
- **Files**:
  - (Optional) `.specify/features/002-zodiac-animals/STATUS.md`
- **Maps to**: Stories (all), FR (all) — scaffolding only

### T002: Confirm Sprint 10 decisions are reflected in spec/plan
- **Priority**: P1
- **Estimate**: 1h
- **Dependencies**: T001
- **Scope**:
  - Included: Ensure plan/spec don’t require image assets or Chinese UI localization for Sprint 10.
  - Excluded: Implementing tasks.
- **Acceptance criteria**:
  - Plan states emoji-only visuals.
  - Plan states en+vi only.
- **Files**:
  - `.specify/features/002-zodiac-animals-plan.md` (update if needed)
  - `.specify/features/002-zodiac-animals.md` (update if needed)
- **Maps to**: FR-006/FR-007/FR-021 (scope adjustment), Sprint 10 decisions

---

## Phase 1 — Core zodiac calculation (Core library)

### T010: Add/confirm `ZodiacAnimal` enum and EarthlyBranch mapping
- **Priority**: P1
- **Estimate**: 2h
- **Dependencies**: T001
- **Scope**:
  - Included: Ensure `ZodiacAnimal` exists and provides the 12 animals; add mapping from `EarthlyBranch` → `ZodiacAnimal`.
  - Excluded: UI.
- **Acceptance criteria**:
  - Mapping covers all 12 branches.
  - Unit tests cover each branch mapping.
- **Files** (expected):
  - `src/LunarCalendar.Core/Models/ZodiacAnimal.cs` (if not already present)
  - `src/LunarCalendar.Core/Services/ZodiacMappingExtensions.cs` (or similar)
  - `tests/LunarCalendar.Core.Tests/.../ZodiacMappingTests.cs`
- **Maps to**: FR-001, FR-026

### T011: Implement `ZodiacService.GetAnimalForDate(...)` using Lunar New Year boundary
- **Priority**: P1
- **Estimate**: 4h
- **Dependencies**: T010
- **Scope**:
  - Included: Determine zodiac animal for any **Gregorian date** by using Lunar New Year boundary via existing calendar service logic.
  - Excluded: Loading “info” content.
- **Acceptance criteria**:
  - For a date before Lunar New Year, uses previous lunar year’s animal.
  - For a date on/after Lunar New Year, uses that lunar year’s animal.
  - Boundary test includes Jan 28/29 2026 scenario (Snake → Horse).
- **Files** (expected):
  - `src/LunarCalendar.Core/Services/ZodiacService.cs`
  - `tests/LunarCalendar.Core.Tests/.../ZodiacServiceBoundaryTests.cs`
- **Maps to**: FR-002, FR-011, Story 1

### T012: Implement `ZodiacService.GetElementalAnimalForYear(year)` (Sprint 9 integration)
- **Priority**: P2
- **Estimate**: 4h
- **Dependencies**: T011
- **Scope**:
  - Included: Combine `SexagenaryService` (Heavenly Stem → element) + `EarthlyBranch` → animal.
  - Excluded: UI rendering.
- **Acceptance criteria**:
  - For 2026 returns Horse + correct element (per Sprint 9 sexagenary output).
  - Unit tests validate at least 10 sample years.
- **Files** (expected):
  - `src/LunarCalendar.Core/Models/ElementalAnimal.cs`
  - `src/LunarCalendar.Core/Services/ZodiacService.cs`
  - `tests/LunarCalendar.Core.Tests/.../ElementalAnimalTests.cs`
- **Maps to**: FR-004, FR-025, FR-027, Story 4

---

## Phase 2 — Zodiac content data (offline)

### T020: Define `ZodiacInfo` model + minimal content contract
- **Priority**: P1
- **Estimate**: 3h
- **Dependencies**: T010
- **Scope**:
  - Included: Model representing zodiac info needed by UI.
  - Excluded: Full content writing.
- **Acceptance criteria**:
  - Model supports: names (en/vi + optional ChineseName as reference), traits, lucky numbers/colors/directions, folklore/significance, compatibility lists.
- **Files** (expected):
  - `src/LunarCalendar.Core/Models/ZodiacInfo.cs`
- **Maps to**: FR-005, FR-003, Story 2

### T021: Create bundled zodiac data file (EN+VI; Chinese characters allowed as reference)
- **Priority**: P1
- **Estimate**: 1d
- **Dependencies**: T020
- **Scope**:
  - Included: `ZodiacData.json` with 12 animals.
  - Excluded: Chinese UI localization.
- **Acceptance criteria**:
  - All 12 animals present.
  - EN + VI fields are complete (no empty strings).
  - Chinese name field may exist as reference (not used for UI localization).
  - File size target: < 100 KB.
- **Files** (expected):
  - `src/LunarCalendar.Core/Data/ZodiacData.json` (or `src/LunarCalendar.MobileApp/Resources/Raw/...` depending on repo convention)
- **Maps to**: FR-005, FR-003, Sprint 10 decisions

### T022: Implement `ZodiacDataRepository` (load + cache)
- **Priority**: P1
- **Estimate**: 4h
- **Dependencies**: T021
- **Scope**:
  - Included: Load JSON once, cache in-memory, expose lookup by `ZodiacAnimal`.
  - Excluded: Network fetching.
- **Acceptance criteria**:
  - Repository loads successfully on both iOS/Android in principle (uses MAUI file embedding approach).
  - Unit tests validate parsing and lookup.
- **Files** (expected):
  - `src/LunarCalendar.Core/Services/ZodiacDataRepository.cs`
  - `tests/LunarCalendar.Core.Tests/.../ZodiacDataRepositoryTests.cs`
- **Maps to**: FR-022, FR-024, Story 2

---

## Phase 3 — Emoji rendering + UI components

### T030: Add `GetZodiacEmoji(ZodiacAnimal)` helper (single source)
- **Priority**: P1
- **Estimate**: 2h
- **Dependencies**: T010
- **Scope**:
  - Included: Centralized mapping animal → emoji + optional accessibility label.
  - Excluded: SVG/asset pipelines.
- **Acceptance criteria**:
  - Returns all 12 emoji.
  - Provides a safe fallback (e.g., `❓`).
- **Files** (expected):
  - `src/LunarCalendar.MobileApp/Services/ZodiacEmojiProvider.cs` (or Core if UI-agnostic)
  - Tests if placed in Core.
- **Maps to**: Sprint 10 decisions, FR-007 (implemented via emoji fallback), FR-008

### T031: Implement `ZodiacHeaderView` integration in calendar header
- **Priority**: P1
- **Estimate**: 1d
- **Dependencies**: T011, T030
- **Scope**:
  - Included: Visual display in header: emoji + localized “Year of …”.
  - Excluded: Deep navigation polish.
- **Acceptance criteria**:
  - In Year of the Horse (2026), shows 🐴 and correct text.
  - Updates when navigating across Lunar New Year boundary.
- **Files** (expected):
  - `src/LunarCalendar.MobileApp/Views/Components/ZodiacHeaderView.xaml`
  - `src/LunarCalendar.MobileApp/Views/Components/ZodiacHeaderView.xaml.cs`
  - Existing calendar page/view to host the header.
- **Maps to**: FR-008, FR-011, Story 1

---

## Phase 4 — Pages + ViewModels

### T040: Create `ZodiacInformationPage` + ViewModel (browse all 12)
- **Priority**: P1
- **Estimate**: 1d
- **Dependencies**: T022, T030
- **Scope**:
  - Included: List/carousel of 12 animals; detail panel for selected animal.
  - Excluded: Elemental variations toggle (separate task).
- **Acceptance criteria**:
  - User can browse 12 animals via swipe or buttons.
  - Displays EN+VI names and traits.
  - Opens to current year animal by default; opens to birth year animal if set.
- **Files** (expected):
  - `src/LunarCalendar.MobileApp/Views/ZodiacInformationPage.xaml`
  - `src/LunarCalendar.MobileApp/Views/ZodiacInformationPage.xaml.cs`
  - `src/LunarCalendar.MobileApp/ViewModels/ZodiacInformationViewModel.cs`
- **Maps to**: FR-009, FR-010, FR-017, Story 2

### T041: Wire header tap → open Zodiac Information page
- **Priority**: P1
- **Estimate**: 3h
- **Dependencies**: T031, T040
- **Scope**:
  - Included: Tap gesture / command.
  - Excluded: Multi-option menu (Story 5 is P3).
- **Acceptance criteria**:
  - Tapping header opens zodiac info page.
- **Files**:
  - `src/LunarCalendar.MobileApp/...` existing navigation shell/routes
- **Maps to**: Story 1, Story 2

### T042: Create `ZodiacCompatibilityPage` + ViewModel
- **Priority**: P2
- **Estimate**: 1d
- **Dependencies**: T022, T010
- **Scope**:
  - Included: Pick 2 animals, show score + rating + explanation.
  - Excluded: Sharing.
- **Acceptance criteria**:
  - Supports all 144 pairs.
  - Preselects user animal when birth date is set.
- **Files** (expected):
  - `src/LunarCalendar.Core/Data/ZodiacCompatibility.json`
  - `src/LunarCalendar.Core/Services/ZodiacCompatibilityEngine.cs`
  - `src/LunarCalendar.MobileApp/Views/ZodiacCompatibilityPage.xaml`
  - `src/LunarCalendar.MobileApp/ViewModels/ZodiacCompatibilityViewModel.cs`
- **Maps to**: FR-013..FR-015, FR-019, Story 3

### T043: Implement native share for compatibility result
- **Priority**: P2
- **Estimate**: 3h
- **Dependencies**: T042
- **Scope**:
  - Included: Share text via platform share sheet.
- **Acceptance criteria**:
  - Share button invokes native share on iOS and Android.
- **Files**:
  - `src/LunarCalendar.MobileApp/Services/ShareService.cs` (or use MAUI Essentials)
  - `src/LunarCalendar.MobileApp/ViewModels/ZodiacCompatibilityViewModel.cs`
- **Maps to**: FR-016, Story 3

---

## Phase 5 — Settings / profile / entry points

### T050: Add “My Zodiac Profile” (birth date + derived profile)
- **Priority**: P2
- **Estimate**: 1d
- **Dependencies**: T012, T022
- **Scope**:
  - Included: Store birth date locally; compute animal + elemental animal.
- **Acceptance criteria**:
  - If no birth date, shows prompt and navigation to set it.
  - If set, shows animal emoji + names + key traits.
- **Files**:
  - `src/LunarCalendar.MobileApp/Views/ZodiacProfilePage.xaml` (or embedded settings section)
  - `src/LunarCalendar.MobileApp/ViewModels/ZodiacProfileViewModel.cs`
  - Existing settings pages.
- **Maps to**: FR-017, FR-018, FR-020, Story 4

### T051: Add condensed zodiac card to date detail view
- **Priority**: P2
- **Estimate**: 4h
- **Dependencies**: T031, T011
- **Scope**:
  - Included: Display current selected date’s lunar year zodiac.
- **Acceptance criteria**:
  - Shows correct animal across boundary.
- **Files**:
  - Existing date detail view + viewmodel
  - `src/LunarCalendar.MobileApp/Views/Components/ZodiacCardView.xaml`
- **Maps to**: FR-012, Story 5

---

## Phase 6 — Localization (Sprint 10 scope: EN + VI)

### T060: Add/confirm zodiac-related UI strings in `Strings.en.resx` + `Strings.vi.resx`
- **Priority**: P1
- **Estimate**: 4h
- **Dependencies**: T031, T040
- **Scope**:
  - Included: UI labels (“Year of…”, “Compatibility”, “Share”, etc.).
  - Excluded: `Strings.zh.resx`.
- **Acceptance criteria**:
  - No missing resource keys at runtime.
  - All user-visible zodiac UI strings are localized in EN+VI.
- **Files** (expected):
  - `src/LunarCalendar.MobileApp/Resources/Strings/Strings.en.resx`
  - `src/LunarCalendar.MobileApp/Resources/Strings/Strings.vi.resx`
- **Maps to**: Sprint 10 decisions, FR-003 (UI part)

---

## Phase 7 — Testing, performance, accessibility

### T070: Unit tests for zodiac calculations (100-year sweep)
- **Priority**: P1
- **Estimate**: 4h
- **Dependencies**: T011
- **Scope**:
  - Included: Validate known year→animal mapping and boundary dates.
- **Acceptance criteria**:
  - Tests cover years 2000–2099 (or 1900–2100 if existing app range).
  - Average runtime not excessive (keep it fast).
- **Files**:
  - `tests/LunarCalendar.Core.Tests/.../ZodiacServiceYearSweepTests.cs`
- **Maps to**: FR-001..FR-003, FR-023

### T071: Accessibility labels for emoji-based UI
- **Priority**: P1
- **Estimate**: 2h
- **Dependencies**: T031, T040
- **Scope**:
  - Included: Screen reader labels like “Horse emoji representing Year of the Horse”.
- **Acceptance criteria**:
  - VoiceOver/TalkBack reads meaningful labels, not just “emoji”.
- **Files**:
  - XAML views updated with `SemanticProperties.Description`/`Hint`.
- **Maps to**: updated SC-016 in decisions doc

### T072: Performance smoke tests for page load times
- **Priority**: P2
- **Estimate**: 3h
- **Dependencies**: T022, T040
- **Scope**:
  - Included: Basic instrumentation/logging to ensure <500ms target with cached data.
- **Acceptance criteria**:
  - Documented measurements on at least one iOS simulator + one Android emulator/device.
- **Files**:
  - `tests/` and/or small debug-only instrumentation
- **Maps to**: FR-024

---

## Phase 8 — UX polish (P3)

### T080: Add multi-entry action menu (header options)
- **Priority**: P3
- **Estimate**: 1d
- **Dependencies**: T041, T042, T050
- **Scope**:
  - Included: Header tap shows options: View current year, Explore all, Compatibility, My Profile.
- **Acceptance criteria**:
  - Menu opens reliably and routes correctly.
- **Files**:
  - Calendar header host view + navigation
- **Maps to**: Story 5, FR-009

---

## Traceability checklist

- **Story 1**: Covered by T011, T031, T041
- **Story 2**: Covered by T020–T022, T040
- **Story 3**: Covered by T042–T043
- **Story 4**: Covered by T012, T050
- **Story 5**: Covered by T051, T080

---

## Definition of Done (task-level)

A task is “done” when:

- Acceptance criteria are met.
- Code compiles (target iOS + Android).
- Tests added/updated and passing.
- No new P0/P1 issues introduced.

