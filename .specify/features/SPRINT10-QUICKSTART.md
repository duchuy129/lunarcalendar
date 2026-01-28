# Sprint 10: Zodiac Animals & Year Characteristics
## Quick Reference

**Status**: ✅ Specification Complete | ⏳ Awaiting Technical Plan  
**Created**: January 26, 2026  
**Branch**: `feature/002-zodiac-animals` (to be created)  
**Duration**: 2 weeks  
**Dependencies**: Sprint 9 (Sexagenary Cycle Foundation) - COMPLETE ✅

---

## 🎯 Sprint Goal

Add comprehensive zodiac animal system with:
- Visual display of current year's zodiac animal
- Rich cultural content for all 12 animals
- Zodiac compatibility checker
- Elemental animal variations (60-year cycle)
- User zodiac profile

---

## 📋 What We're Building

### 5 User Stories (Prioritized)

1. **P1: View Current Year's Zodiac Animal** → Display zodiac icon in calendar header
2. **P1: Explore Zodiac Animal Characteristics** → Full zodiac information page with traits, lucky elements, folklore
3. **P2: Check Zodiac Compatibility** → Interactive compatibility checker with sharing
4. **P2: Understand Elemental Variations** → Fire Horse, Metal Rat, etc. (integrates Sprint 9)
5. **P3: Access from Multiple Entry Points** → Convenient navigation from header, details, settings

### Key Features

✅ **Visual Display**: Zodiac animal icons in calendar header  
✅ **Educational Content**: Personality traits, lucky numbers/colors/directions, cultural significance  
✅ **Compatibility System**: Check relationship compatibility between any 2 animals  
✅ **Personalization**: "My Zodiac Profile" based on user's birth date  
✅ **60-Year Cycle**: Element + Animal combinations (e.g., "2026: Fire Horse")  
✅ **Localization**: Vietnamese (12 Con Giáp), Chinese (生肖), English  

---

## 🔧 Technical Requirements

### New Components

**Services**:
- `ZodiacService` - Core zodiac calculations and data retrieval
- `ZodiacDataRepository` - Load/cache zodiac JSON data
- `ZodiacCompatibilityEngine` - Compatibility scoring algorithm

**UI Pages**:
- `ZodiacInformationPage` - Main zodiac browser
- `ZodiacCompatibilityPage` - Compatibility checker
- `ZodiacProfileView` - User's zodiac profile

**UI Components**:
- `ZodiacHeaderView` - Calendar header zodiac display
- `ZodiacCardView` - Condensed zodiac info card
- `ZodiacAnimalPicker` - Animal selection control

### Data Assets

- `ZodiacData.json` (~50-100 KB) - All 12 animals' info
- `ZodiacCompatibility.json` (~10 KB) - 144 compatibility pairings
- 12 zodiac animal images (512x512px, optimized, ~1.2 MB total)

### Integration Points with Sprint 9

```csharp
// Sprint 9 provides:
SexagenaryService.GetSexagenaryInfo(year) 
  → Returns EarthlyBranch (maps to ZodiacAnimal)
  → Returns HeavenlyStem + Element (e.g., Fire for Fire Horse)

// Sprint 10 uses:
ZodiacService.GetAnimalForYear(year)
  → Uses SexagenaryService internally
  → Returns ZodiacAnimal + Element
```

---

## 📊 Success Metrics

### Must Achieve
- ✅ 100% accuracy on zodiac calculations (100 test years)
- ✅ <10ms zodiac calculation time
- ✅ <500ms Zodiac Information page load time
- ✅ 80%+ users view zodiac animal in first session
- ✅ Cultural accuracy validated by 2+ Vietnamese SMEs

### Nice to Have
- 🎯 50%+ users explore 3+ zodiac animals
- 🎯 30%+ users check compatibility feature
- 🎯 15%+ increase in app virality via social shares

---

## 🎓 The 12 Zodiac Animals

| # | Vietnamese | Chinese | English | Years | Traits (Brief) |
|---|------------|---------|---------|-------|----------------|
| 1 | Tý - Chuột | 鼠 Shǔ | Rat | 2020, 2008, 1996 | Intelligent, adaptable, quick-witted |
| 2 | Sửu - Trâu | 牛 Niú | Ox | 2021, 2009, 1997 | Diligent, dependable, strong |
| 3 | Dần - Hổ | 虎 Hǔ | Tiger | 2022, 2010, 1998 | Brave, confident, competitive |
| 4 | Mão - Mèo/Thỏ | 兔 Tù | Rabbit | 2023, 2011, 1999 | Gentle, quiet, elegant |
| 5 | Thìn - Rồng | 龍 Lóng | Dragon | 2024, 2012, 2000 | Confident, intelligent, enthusiastic |
| 6 | Tỵ - Rắn | 蛇 Shé | Snake | 2025, 2013, 2001 | Wise, enigmatic, graceful |
| 7 | Ngọ - Ngựa | 馬 Mǎ | Horse | **2026**, 2014, 2002 | Energetic, independent, warm-hearted |
| 8 | Mùi - Dê | 羊 Yáng | Goat | 2027, 2015, 2003 | Calm, gentle, sympathetic |
| 9 | Thân - Khỉ | 猴 Hóu | Monkey | 2028, 2016, 2004 | Sharp, smart, curious |
| 10 | Dậu - Gà | 雞 Jī | Rooster | 2029, 2017, 2005 | Observant, hardworking, courageous |
| 11 | Tuất - Chó | 狗 Gǒu | Dog | 2030, 2018, 2006 | Lovely, honest, prudent |
| 12 | Hợi - Lợn | 豬 Zhū | Pig | 2031, 2019, 2007 | Compassionate, generous, diligent |

**Note**: Vietnamese culture uses "Mèo" (Cat) instead of "Thỏ" (Rabbit) for the 4th animal.

---

## 🧪 Key Test Scenarios

### Zodiac Calculation
```
Test Case: 2026 (current year)
Expected: Horse (Ngọ) + Fire Element = "Fire Horse"
Validation: (2026 - 4) % 12 = 2 → Index 6 → Horse ✅
```

### Lunar Year Boundary
```
Test Case: Lunar New Year 2026 (January 29)
Jan 28, 2026 → Snake (2025 lunar year)
Jan 29, 2026 → Horse (2026 lunar year)
```

### Compatibility Example
```
Horse + Dog = Great Match (90%)
Horse + Rat = Challenging (40%)
```

---

## 📁 File Structure (Planned)

```
src/LunarCalendar.Core/
├── Models/
│   └── ZodiacAnimal.cs         # Enum: 12 animals
├── Services/
│   └── ZodiacService.cs        # Core zodiac logic
├── Data/
│   ├── ZodiacData.json         # Comprehensive zodiac info
│   └── ZodiacCompatibility.json # Compatibility matrix

src/LunarCalendar.MobileApp/
├── ViewModels/
│   ├── ZodiacInformationViewModel.cs
│   ├── ZodiacCompatibilityViewModel.cs
│   └── ZodiacProfileViewModel.cs
├── Views/
│   ├── ZodiacInformationPage.xaml
│   ├── ZodiacCompatibilityPage.xaml
│   └── Components/
│       ├── ZodiacHeaderView.xaml
│       └── ZodiacCardView.xaml
├── Resources/
│   ├── Images/Zodiacs/
│   │   ├── rat.png (x12 animals)
│   │   └── rat_dark.png (dark mode variants)
│   └── Localization/
│       └── ZodiacStrings.resx

src/LunarCalendar.Core.Tests/
└── Services/
    └── ZodiacServiceTests.cs   # Unit tests
```

---

## 🚀 Implementation Phases

### Phase 0: Research (0.5 days)
- [x] Review Sprint 9 integration points
- [ ] Research Vietnamese zodiac content sources
- [ ] Collect zodiac images or commission artist
- [ ] Validate compatibility algorithm with SMEs

### Phase 1: Core Library (2 days)
- [ ] Create `ZodiacAnimal` enum
- [ ] Implement `ZodiacService.GetAnimalForYear(year)`
- [ ] Create `ZodiacData.json` with all 12 animals' info
- [ ] Implement `ZodiacDataRepository` for loading data
- [ ] Write unit tests (100% accuracy on 100 years)

### Phase 2: Compatibility System (1 day)
- [ ] Create `ZodiacCompatibility.json` with 144 pairings
- [ ] Implement `ZodiacCompatibilityEngine.Calculate(animal1, animal2)`
- [ ] Write compatibility unit tests

### Phase 3: UI Components (2 days)
- [ ] Add `ZodiacHeaderView` to calendar header
- [ ] Create `ZodiacInformationPage` with swipe navigation
- [ ] Implement `ZodiacCompatibilityPage`
- [ ] Add "My Zodiac Profile" in settings

### Phase 4: Integration (1 day)
- [ ] Integrate with Sprint 9's `SexagenaryService`
- [ ] Display elemental animals (Fire Horse, Metal Rat, etc.)
- [ ] Update year detail view to show both stem-branch AND zodiac
- [ ] Add zodiac info to date detail pages

### Phase 5: Localization (1 day)
- [ ] Add Vietnamese zodiac strings
- [ ] Add Chinese zodiac strings
- [ ] Validate all 12 animals have 3-language coverage

### Phase 6: Testing & Polish (2.5 days)
- [ ] Cultural SME review (external)
- [ ] Performance testing (<10ms, <500ms)
- [ ] Accessibility testing (screen readers, alt text)
- [ ] Device testing (iOS + Android)
- [ ] Bug fixes and polish

---

## 🔄 Spec-Kit Workflow

### ✅ Step 1: Specification (COMPLETE)
Created `002-zodiac-animals.md` with:
- 5 prioritized user stories
- 27 functional requirements
- 20 success criteria
- Edge cases and constraints

### 🎯 Step 2: Technical Plan (NEXT)
```
/speckit.plan
```
This will generate `002-zodiac-animals-plan.md` with:
- Technical architecture decisions
- API contracts and interfaces
- Database schema (if needed)
- Component design
- Testing strategy

### 📋 Step 3: Task Breakdown
```
/speckit.tasks
```
This will generate `002-zodiac-animals-tasks.md` with:
- Granular, actionable tasks (T001, T002, etc.)
- Time estimates
- Dependencies
- Priority ordering

### 🛠️ Step 4: Implementation
```
/speckit.implement
```
Guided implementation of tasks with:
- Code generation
- Test creation
- Real-time validation

---

## 💡 Key Design Decisions (to be made in Technical Plan)

### Image Strategy
- **Option A**: Vector graphics (SVG) - Scalable, small size, no resolution variants needed
- **Option B**: Raster images (PNG/WebP) - Better for complex illustrations, need 1x/2x/3x variants
- **Recommendation**: TBD in technical plan

### Data Source
- **Option A**: Embedded JSON in app bundle - No network needed, instant load
- **Option B**: Remote API with caching - Easier to update content without app release
- **Recommendation**: Option A for MVP (Sprint 10), Option B for future updates

### Compatibility Algorithm
- **Option A**: Simplified matrix (144 pre-defined scores) - Fast, consistent, easy to maintain
- **Option B**: Dynamic calculation (elements + animals) - More complex, harder to validate
- **Recommendation**: Option A (mainstream Vietnamese astrology)

---

## ⚠️ Risks & Mitigations

| Risk | Probability | Impact | Mitigation |
|------|-------------|--------|------------|
| Cultural inaccuracy in zodiac content | Medium | High | Engage 2+ Vietnamese SMEs for review |
| Image size bloat (>2 MB) | Low | Medium | Use optimized WebP or SVG graphics |
| Sprint 9 integration issues | Low | Medium | Sprint 9 complete and stable; thorough testing |
| Compatibility algorithm disputes | Medium | Low | Document sources; use mainstream consensus |

---

## 📝 Open Questions (to address in planning)

1. **Zodiac Start Date**: Confirm using Lunar New Year (not Solar Start of Spring) → **Answer**: Use Lunar New Year (traditional Vietnamese system)
2. **Rabbit vs Cat**: Which to use for the 4th animal? → **Answer**: Support both, locale-specific (Vietnamese uses Cat)
3. **Image Artist**: Commission custom art or use open-source? → **TBD**: Budget discussion
4. **Offline First**: Should all data be bundled, or allow remote updates? → **TBD**: Technical plan decision

---

## 🎯 Definition of Done

Sprint 10 is complete when:

- [ ] All 5 user stories implemented and acceptance scenarios pass
- [ ] All 27 functional requirements met and tested
- [ ] Zodiac calculation 100% accurate on 100 test years
- [ ] All 12 zodiac animals have complete data (traits, lucky elements, images)
- [ ] Compatibility checker works for all 144 pairings
- [ ] Zodiac year boundary transitions correctly at Lunar New Year
- [ ] Cultural content validated by 2+ Vietnamese SMEs
- [ ] Performance benchmarks met (<10ms calc, <500ms page load)
- [ ] Accessibility: WCAG 2.1 AA compliance
- [ ] Localization: 100% coverage in Vietnamese, Chinese, English
- [ ] Zero P0/P1 bugs in final testing
- [ ] Code reviewed and merged to `develop` branch
- [ ] Sprint 11 blockers removed (zodiac system ready for dynamic backgrounds)

---

## 📚 References

- **Sprint 9 Work**: `.specify/features/001-sexagenary-cycle-foundation.md`
- **Development Roadmap**: `docs/development-roadmap.md` (Sprint 10 section)
- **Phase 2 Plan**: `support_docs/PHASE2_PHASE3_PLAN.md` (lines 96-150)
- **Vietnamese Zodiac**: https://vi.wikipedia.org/wiki/12_con_giáp
- **Chinese Zodiac**: https://zh.wikipedia.org/wiki/生肖

---

## 🚀 Ready to Start?

1. **Review this specification**: Ensure stakeholders agree with scope and priorities
2. **Run technical planning**: `/speckit.plan` to generate architecture decisions
3. **Create feature branch**: `git checkout -b feature/002-zodiac-animals`
4. **Kick off Sprint 10**: Allocate 2 weeks starting February 2026

---

**Last Updated**: January 26, 2026  
**Status**: 🟢 Ready for Planning  
**Next Step**: `/speckit.plan` to generate technical implementation plan
