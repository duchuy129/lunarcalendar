# Feature 003 — Dedicated Zodiac Tab

**Branch:** `feature/003-zodiac-tab`  
**Based on:** `feature/002-zodiac-phase2` (commit `8b52013`)  
**Platforms:** iOS + Android  
**Locales:** EN + VI  

---

## Problem Statement

The "My Zodiac Profile" section currently lives inside `SettingsPage`, alongside language toggles, sync buttons, and cache controls. Zodiac content is **personal, primary-feature content** — not an app configuration option. Embedding it in Settings:

- Buries it behind a "⚙️ Settings" tap, reducing discoverability.
- Forces conceptually unrelated content (sync/cache/about) to share a scroll view with personal zodiac data.
- Is inconsistent with the existing `ZodiacInformationPage` and `ZodiacCompatibilityPage` which are already standalone, first-class pages reachable only via push-nav shortcuts.

**Decision:** Extract zodiac content into a dedicated **Zodiac tab** (🔯) in the bottom `TabBar`, positioned between `YearHolidays` and `Settings`. The tab hosts a new `ZodiacProfilePage` that combines the profile form and navigation shortcuts. Settings loses its zodiac section entirely.

---

## Architecture Overview

```
TabBar
├── 📅  CalendarTab        → CalendarPage           (existing, unchanged)
├── 🎉  YearHolidaysTab    → YearHolidaysPage        (existing, unchanged)
├── 🔯  ZodiacTab          → ZodiacProfilePage       ← NEW
└── ⚙️  SettingsTab        → SettingsPage            (existing, zodiac section removed)

Push nav (registered Shell routes, unchanged):
  zodiacinfo          → ZodiacInformationPage
  zodiaccompatibility → ZodiacCompatibilityPage
```

**Key constraints:**
- Shell is pure code-behind (`AppShell.xaml.cs`). The `.DISABLED` XAML file is not used.
- Existing routes `zodiacinfo` and `zodiaccompatibility` must remain registered and unbroken.
- `UpdateTabTitles()` must be extended to include the new `_zodiacTab` field.
- Birth year persistence key `"ZodiacProfileBirthYear"` stays in `Preferences` — only the ViewModel moves.
- `IZodiacService` is already `Singleton` in DI — no change required.
- All localization in `AppResources.resx` (EN) + `AppResources.vi.resx` (VI) only.

---

## Task Breakdown

### Phase 0 — Branch & Scaffolding

| ID   | Task | Files | Notes |
|------|------|-------|-------|
| T080 | ✅ Create branch `feature/003-zodiac-tab` | — | Done |
| T081 | Create `ZodiacProfileViewModel.cs` skeleton | `src/LunarCalendar.MobileApp/ViewModels/ZodiacProfileViewModel.cs` | Mirrors SettingsViewModel zodiac region; DI-constructible |
| T082 | Create `ZodiacProfilePage.xaml` + `.xaml.cs` skeleton | `src/LunarCalendar.MobileApp/Views/ZodiacProfilePage.xaml` | Empty `ContentPage` with correct namespace |
| T083 | Register `ZodiacProfileViewModel` + `ZodiacProfilePage` in DI | `MauiProgram.cs` | `AddTransient<>` for both |

---

### Phase 1 — ZodiacProfileViewModel

Extract the zodiac logic from `SettingsViewModel` into a new, focused ViewModel.

| ID   | Task | Files | Acceptance Criteria |
|------|------|-------|---------------------|
| T084 | Copy `BirthYearKey`, `BirthYearText`, `MyZodiacDisplay`, `HasZodiacProfile` from `SettingsViewModel` | `ZodiacProfileViewModel.cs` | `[ObservableProperty]` fields present |
| T085 | Copy constructor injection: `IZodiacService` (required) | `ZodiacProfileViewModel.cs` | `IZodiacService` injected, no nullability warnings |
| T086 | Copy `LoadZodiacProfile()`, `UpdateZodiacDisplay()`, `GetLocalizedAnimalName()` methods | `ZodiacProfileViewModel.cs` | Methods identical to `SettingsViewModel` counterparts |
| T087 | Copy `SaveZodiacProfileAsync()` as `[RelayCommand]` | `ZodiacProfileViewModel.cs` | `[RelayCommand]` attribute; validates year range 1900–current; saves to `Preferences`; calls `UpdateZodiacDisplay()` |
| T088 | Add `NavigateToZodiacInfoCommand` | `ZodiacProfileViewModel.cs` | `await Shell.Current.GoToAsync("zodiacinfo")` (no animal param — shows all 12) |
| T089 | Add `NavigateToCompatibilityCommand` | `ZodiacProfileViewModel.cs` | `await Shell.Current.GoToAsync("zodiaccompatibility")` |
| T090 | Subscribe to `LanguageChangedMessage` → reload display | `ZodiacProfileViewModel.cs` | `WeakReferenceMessenger` handler calls `LoadZodiacProfile()` |
| T091 | Write `ZodiacProfileViewModelTests.cs` | `src/LunarCalendar.Core.Tests/ViewModels/ZodiacProfileViewModelTests.cs` | SaveProfile_ValidYear_UpdatesDisplay; SaveProfile_InvalidYear_ShowsError; LoadProfile_NoSavedYear_HasZodiacProfileFalse; LoadProfile_SavedYear_HasZodiacProfileTrue |

---

### Phase 2 — ZodiacProfilePage UI

| ID   | Task | Files | Acceptance Criteria |
|------|------|-------|---------------------|
| T092 | Page header: emoji + title label "🔯 My Zodiac Profile" | `ZodiacProfilePage.xaml` | Uses `{ext:Translate ZodiacProfile}` string; styled consistent with other page headers |
| T093 | Birth year entry row: `Entry` bound to `BirthYearText`, numeric keyboard, save `Button` bound to `SaveZodiacProfileCommand` | `ZodiacProfilePage.xaml` | Same UX as existing Settings zodiac entry |
| T094 | Zodiac display label bound to `MyZodiacDisplay`; hidden when `!HasZodiacProfile` | `ZodiacProfilePage.xaml` | `IsVisible="{Binding HasZodiacProfile}"` on result section |
| T095 | "Browse Animals" card/button → `NavigateToZodiacInfoCommand` | `ZodiacProfilePage.xaml` | Navigates to `ZodiacInformationPage`; shows emoji 🐉 + translated label |
| T096 | "Check Compatibility" card/button → `NavigateToCompatibilityCommand` | `ZodiacProfilePage.xaml` | Navigates to `ZodiacCompatibilityPage`; shows emoji 💞 + translated label |
| T097 | `SemanticProperties` on all interactive elements | `ZodiacProfilePage.xaml` | `Description` on entry, buttons, and nav cards |
| T098 | Wire `BindingContext` in code-behind | `ZodiacProfilePage.xaml.cs` | DI constructor injects `ZodiacProfileViewModel`; sets `BindingContext` |
| T099 | Style: cards use `SubtleCardGradient` brush, consistent with `SettingsPage` / `ZodiacCompatibilityPage` border style | `ZodiacProfilePage.xaml` | Visual consistency audit passes |

---

### Phase 3 — Add Zodiac Tab to AppShell

| ID   | Task | Files | Acceptance Criteria |
|------|------|-------|---------------------|
| T100 | Add `_zodiacTab` private field | `AppShell.xaml.cs` | `private ShellContent? _zodiacTab;` after `_yearHolidaysTab` |
| T101 | Construct `_zodiacTab` in `CreateUIInCode()` with `ZodiacProfilePage` content template and `🔯` emoji icon | `AppShell.xaml.cs` | Same `FontImageSource` pattern as other tabs |
| T102 | Insert `tabBar.Items.Add(_zodiacTab)` between `_yearHolidaysTab` and `_settingsTab` | `AppShell.xaml.cs` | Tab order: 📅 Calendar, 🎉 Holidays, 🔯 Zodiac, ⚙️ Settings |
| T103 | Extend `UpdateTabTitles()` to set `_zodiacTab.Title = AppResources.ZodiacTab` | `AppShell.xaml.cs` | Title updates on language change |

---

### Phase 4 — Remove Zodiac Section from Settings

| ID   | Task | Files | Acceptance Criteria |
|------|------|-------|---------------------|
| T104 | Remove "My Zodiac Profile" `Border` block (lines ~63–180) from `SettingsPage.xaml` | `SettingsPage.xaml` | No zodiac section visible in Settings; no orphan `BoxView` dividers |
| T105 | Remove `OnZodiacCompatibilityClicked` event handler from `SettingsPage.xaml.cs` | `SettingsPage.xaml.cs` | No dead code; no compile errors |
| T106 | Remove zodiac fields from `SettingsViewModel`: `BirthYearKey`, `BirthYearText`, `MyZodiacDisplay`, `HasZodiacProfile`, `SaveZodiacProfileCommand` | `SettingsViewModel.cs` | Fields gone; `IZodiacService` dependency removed from constructor (if no longer used); no compile errors |
| T107 | Remove zodiac methods from `SettingsViewModel`: `LoadZodiacProfile()`, `UpdateZodiacDisplay()`, `GetLocalizedAnimalName()`, `SaveZodiacProfileAsync()` | `SettingsViewModel.cs` | Methods gone; no compile errors |
| T108 | Remove `IZodiacService` injection from `SettingsViewModel` constructor if no longer needed | `SettingsViewModel.cs` | DI constructor simplified; `MauiProgram.cs` unchanged (service still registered as Singleton) |

---

### Phase 5 — Localization

| ID   | Task | Files | Acceptance Criteria |
|------|------|-------|---------------------|
| T109 | Add `ZodiacTab` = `"Zodiac"` / `"Tử Vi"` | `AppResources.resx`, `AppResources.vi.resx` | Tab title correct in both languages after language switch |
| T110 | Add `ZodiacProfile` = `"My Zodiac Profile"` / `"Hồ Sơ Tử Vi"` | `AppResources.resx`, `AppResources.vi.resx` | Page header label translates |
| T111 | Add `BrowseAnimals` = `"Browse Animals"` / `"Xem Các Con Giáp"` | `AppResources.resx`, `AppResources.vi.resx` | Browse card label translates |
| T112 | Add `CheckCompatibility` (reuse or rename from existing) | `AppResources.resx`, `AppResources.vi.resx` | Compatibility card label translates; verify not a duplicate key |
| T113 | Verify `NotSet`, `InvalidBirthYear` strings already exist in both resx files (reused from `SettingsViewModel`) | `AppResources.resx`, `AppResources.vi.resx` | No new duplicates; `ZodiacProfileViewModel` references same keys |

---

### Phase 6 — Tests

| ID   | Task | Files | Acceptance Criteria |
|------|------|-------|---------------------|
| T091 | (see Phase 1) `ZodiacProfileViewModelTests.cs` — unit tests for ViewModel logic | `src/LunarCalendar.Core.Tests/ViewModels/ZodiacProfileViewModelTests.cs` | ≥ 6 tests; all green |
| T114 | Build verification: iOS `net10.0-ios` — 0 errors | CI / manual | `dotnet build -f net10.0-ios` succeeds |
| T115 | Build verification: Android `net10.0-android` — 0 errors | CI / manual | `dotnet build -f net10.0-android` succeeds |
| T116 | Full test suite: 313+ tests passing | `LunarCalendar.Core.Tests` | `dotnet test` green |

---

### Phase 7 — Polish & Docs

| ID   | Task | Files | Acceptance Criteria |
|------|------|-------|---------------------|
| T117 | Deploy to iOS simulator; tap Zodiac tab; verify profile entry, animal browse, compatibility navigate correctly | Manual | No crashes; navigation back-stack correct |
| T118 | Deploy to Android emulator; same smoke test | Manual | No crashes; tab icon renders |
| T119 | Verify Settings page no longer shows zodiac section | Manual | Settings shows: Language, Display, Interaction, Sync, Data, About only |
| T120 | Update `.specify/features/003-zodiac-tab-tasks.md` with ✅ / completion notes | This file | All tasks marked |

---

## Sequence / Dependencies

```
T081 → T084–T090 (ViewModel)
T082 → T092–T099 (Page)  
T083 (DI) — after T081 + T082
T081 + T082 + T083 → T100–T103 (AppShell tab)
T100–T103 → T104–T108 (remove from Settings) — do Settings cleanup AFTER tab is live
T109–T113 (localization) — parallel with Phase 1–3
T091, T114–T116 (tests + builds) — after Phase 1–4 complete
T117–T120 (deploy + polish) — last
```

---

## Localization String Reference

| Key | EN | VI |
|-----|----|----|
| `ZodiacTab` | `Zodiac` | `Tử Vi` |
| `ZodiacProfile` | `My Zodiac Profile` | `Hồ Sơ Tử Vi` |
| `BrowseAnimals` | `Browse Animals` | `Xem Các Con Giáp` |
| `CheckCompatibility` | `Check Compatibility` | `Kiểm Tra Hợp Tuổi` |

> `NotSet`, `InvalidBirthYear`, `ErrorTitle`, `OK`, `ZodiacCompatibilityTitle`, `ZodiacInformationTitle` — all pre-existing; reuse without modification.

---

## Files Impacted Summary

| File | Change |
|------|--------|
| `AppShell.xaml.cs` | Add `_zodiacTab` field, construct in `CreateUIInCode()`, add to `tabBar.Items`, extend `UpdateTabTitles()` |
| `MauiProgram.cs` | Register `ZodiacProfileViewModel` + `ZodiacProfilePage` (`AddTransient`) |
| `Views/ZodiacProfilePage.xaml` | **New** — full page UI |
| `Views/ZodiacProfilePage.xaml.cs` | **New** — DI constructor, BindingContext wiring |
| `ViewModels/ZodiacProfileViewModel.cs` | **New** — extracted zodiac logic + nav commands |
| `SettingsPage.xaml` | Remove zodiac `Border` section |
| `SettingsPage.xaml.cs` | Remove `OnZodiacCompatibilityClicked` handler |
| `SettingsViewModel.cs` | Remove all zodiac fields/methods/injection |
| `AppResources.resx` | Add 4 new strings (T109–T112) |
| `AppResources.vi.resx` | Add 4 new strings (T109–T112) |
| `ZodiacProfileViewModelTests.cs` | **New** — ≥ 6 unit tests |

**Files NOT changing:** `ZodiacInformationPage`, `ZodiacCompatibilityPage`, `CalendarPage`, `YearHolidaysPage`, any Core library files.

---

*Created: feature/003-zodiac-tab session kickoff*
