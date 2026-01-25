# Phase 2 - Sprint 9: Sexagenary Cycle Foundation
## Quick Start Guide

**Status**: ✅ Specification Complete | ✅ Technical Plan Complete | ⏳ Ready for Task Breakdown

---

## 📋 What We've Created

### 1. **Feature Specification** 
📄 `.specify/features/001-sexagenary-cycle-foundation.md`

**Contains**:
- 5 prioritized user stories (P1-P3)
- 32 functional requirements
- 10 success criteria with measurable outcomes
- Edge cases and data model definitions

**Key User Value**: Users can see traditional Vietnamese/Chinese stem-branch dates (Can Chi / 干支) instead of just Gregorian/lunar, enabling authentic cultural calendar usage for event planning, astrology, and traditional practices.

### 2. **Technical Implementation Plan**
📄 `.specify/features/001-sexagenary-cycle-foundation-plan.md`

**Contains**:
- Full project structure showing all new and modified files
- 5 architecture decisions (on-device calculation, enums vs. database, caching strategy, etc.)
- 5 implementation phases with acceptance criteria
- Risk management and testing strategy
- Performance targets and rollout plan

**Key Technical Approach**: Implement pure calculation algorithms in LunarCalendar.Core library (offline-first), integrate into existing MAUI MVVM UI, validate against 1000+ historical dates.

---

## 🎯 Next Steps with Spec-Kit

### Step 1: Break Down into Tasks
Use the `/speckit.tasks` command to convert the implementation plan into actionable development tasks:

```
/speckit.tasks
```

This will create `tasks.md` with:
- Specific coding tasks for each phase
- Test requirements for each task
- Estimated complexity and dependencies
- Checklist format for tracking progress

### Step 2: Start Implementation
Use the `/speckit.implement` command to begin executing tasks:

```
/speckit.implement
```

This will:
- Guide you through implementing each task
- Create the actual code files
- Run tests as you go
- Track progress against the plan

---

## 📊 Project Overview

### What This Feature Adds

**New Capabilities**:
- Display today's stem-branch date in calendar header
- Show day stem-branch on every calendar cell
- View full sexagenary info (year/month/day/hour) in date details
- Educational reference for 10 Heavenly Stems and 12 Earthly Branches
- Support for Vietnamese, Chinese, and English localization

**Files to Create** (~15 new files):
```
Core Library:
- Models: SexagenaryInfo, HeavenlyStem, EarthlyBranch, FiveElement, ZodiacAnimal
- Services: SexagenaryService, SexagenaryCalculator

Mobile App:
- ViewModels: SexagenaryEducationViewModel
- Views: SexagenaryEducationPage.xaml
- Controls: StemBranchCell.xaml, ZodiacYearIndicator.xaml

Tests:
- SexagenaryServiceTests, SexagenaryCalculatorTests, UI tests
```

**Files to Modify** (~8 existing files):
```
Mobile App:
- ViewModels: MainViewModel, CalendarViewModel, DateDetailViewModel
- Views: MainPage.xaml, CalendarPage.xaml, DateDetailPage.xaml
- Services: LocalizationService

Resources:
- Strings.en.resx, Strings.vi.resx (add new strings)
```

### Success Criteria Highlights

**Performance**:
- ✅ < 50ms per date calculation
- ✅ > 55 FPS calendar scrolling
- ✅ < 2MB app size increase

**Accuracy**:
- ✅ 100% match with 1000+ historical validation dates
- ✅ Cultural expert validation

**Quality**:
- ✅ 95%+ unit test coverage
- ✅ Zero P0/P1 bugs at release

---

## 🏛️ Constitutional Alignment

This feature fully complies with the Vietnamese Lunar Calendar Constitution:

✅ **I. Offline-First**: All calculations on-device, no network required  
✅ **II. Cultural Accuracy**: Algorithm validated, SME reviewed  
✅ **III. Privacy**: No data collection, works in guest mode  
✅ **IV. Cross-Platform**: Shared Core library logic  
✅ **V. Performance**: < 50ms calculations, 60 FPS UI  
✅ **VI. Bilingual**: Vietnamese, Chinese, English support  
✅ **VII. Test Coverage**: 95%+ unit tests, historical validation

---

## 📈 Implementation Timeline

**Estimated Duration**: 2 weeks (Sprint 9)

| Phase | Duration | Deliverable |
|-------|----------|-------------|
| Phase 1: Core Library | 4 days | Calculation engine with passing tests |
| Phase 2: Mobile UI | 4 days | Stem-branch displays in calendar |
| Phase 3: Educational | 2 days | Reference page and tooltips |
| Phase 4: Backend API | 2 days | Optional API endpoints |
| Phase 5: Testing & Polish | 2 days | Production-ready quality |

**Parallelization Options**:
- Backend API (Phase 4) can run in parallel with UI work
- Educational features (Phase 3) can start after UI mockups ready

---

## 🔄 Using Spec-Kit Commands

### Available Commands

1. **`/speckit.clarify`** - Ask clarifying questions about underspecified areas
   - Use if you need more details on any requirement
   - Helps refine the specification before coding

2. **`/speckit.analyze`** - Cross-artifact consistency check
   - Verifies spec and plan are consistent
   - Identifies gaps or conflicts

3. **`/speckit.tasks`** - ⭐ **NEXT STEP** - Generate actionable task list
   - Breaks down plan into specific coding tasks
   - Creates task.md file

4. **`/speckit.implement`** - Execute tasks and build the feature
   - Guides you through implementation
   - Creates code files and runs tests

5. **`/speckit.checklist`** - Generate quality checklist
   - Custom validation checklist
   - "Unit tests for English" approach

---

## 📝 Quick Reference

### Key Files Created

```
.specify/features/
├── 001-sexagenary-cycle-foundation.md           ← Feature specification
├── 001-sexagenary-cycle-foundation-plan.md      ← This plan
└── [Coming Next] tasks.md                        ← Task breakdown
```

### Constitution Reference

📜 `.specify/memory/constitution.md` - Project principles and standards

### Development Roadmap

📖 `docs/development-roadmap.md` - Full Phase 2 & 3 plan

---

## 🚀 Ready to Start?

Run this command to create the task breakdown:

```
/speckit.tasks
```

Then review the tasks and start implementation with:

```
/speckit.implement
```

---

## ❓ Questions or Issues?

- **Unclear requirements?** → Use `/speckit.clarify` to ask questions
- **Inconsistencies?** → Use `/speckit.analyze` to check alignment
- **Need help implementing?** → Follow the phase-by-phase plan above
- **Stuck on a specific task?** → Consult the technical plan for architecture decisions

---

**Last Updated**: January 25, 2026  
**Next Sprint (10)**: Zodiac Animals & Year Characteristics (depends on this feature)
