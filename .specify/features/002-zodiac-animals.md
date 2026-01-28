# Feature Specification: Zodiac Animals & Year Characteristics

**Feature Branch**: `feature/002-zodiac-animals`  
**Created**: January 26, 2026  
**Status**: Draft  
**Sprint**: 10 (Phase 2)  
**Dependencies**: Sprint 9 - Sexagenary Cycle Foundation (feature/001-sexagenary-cycle)  
**Input**: Sprint 10 requirements from development roadmap

---

## User Scenarios & Testing *(mandatory)*

### User Story 1 - View Current Year's Zodiac Animal (Priority: P1)

A user wants to quickly see which zodiac animal governs the current lunar year, displayed prominently in the calendar interface with a beautiful visual representation.

**Why this priority**: This is the most visible and commonly referenced aspect of the lunar calendar. Users immediately want to know "What animal year is it?" when opening the app.

**Independent Test**: Can be fully tested by opening the app and viewing the calendar header. The current year's zodiac animal icon and name should be displayed. This delivers immediate cultural value without requiring any other features.

**Acceptance Scenarios**:

1. **Given** I open the app during the Year of the Horse (Lunar Year 2026), **When** I view the main calendar, **Then** I see a Horse icon in the calendar header with the text "Year of the Horse" (or localized equivalent)
2. **Given** I am viewing any date within the current lunar year, **When** I look at the calendar header, **Then** the zodiac animal displayed matches the lunar year (not the Gregorian year)
3. **Given** Lunar New Year has just passed (e.g., January 29, 2026 - Snake to Horse), **When** I view dates before the new year, **Then** I see the Snake icon, and after the new year, I see the Horse icon

---

### User Story 2 - Explore Zodiac Animal Characteristics (Priority: P1)

A user wants to learn about their birth year's zodiac animal including personality traits, lucky elements (numbers, colors, directions), and cultural significance.

**Why this priority**: This is core cultural content that provides deep engagement and educational value. Users expect to learn about zodiac characteristics, not just see the animal name.

**Independent Test**: Can be fully tested by navigating to "Zodiac Information" page and selecting any zodiac animal. Should display comprehensive cultural information including traits, lucky elements, and folklore, delivering standalone educational value.

**Acceptance Scenarios**:

1. **Given** I tap on the zodiac animal icon in the calendar header, **When** the Zodiac Information page opens, **Then** I see detailed information about the current year's animal including: name in English/Vietnamese/Chinese, personality traits, lucky numbers, lucky colors, lucky directions, and cultural significance
2. **Given** I am on the Zodiac Information page, **When** I swipe left/right or tap navigation arrows, **Then** I can browse through all 12 zodiac animals (Rat, Ox, Tiger, Rabbit, Dragon, Snake, Horse, Goat, Monkey, Rooster, Dog, Pig)
3. **Given** I select the "Rat" zodiac animal, **When** the detail view loads, **Then** I see: Vietnamese name (Tý/Chuột), Chinese name (鼠), personality traits (intelligent, adaptable, quick-witted), lucky numbers (2, 3), lucky colors (blue, gold, green), lucky directions (southeast, northeast), and famous historical figures born in Rat years
4. **Given** I have set my birth date in app settings, **When** I open the Zodiac Information page, **Then** it automatically opens to my birth year's zodiac animal

---

### User Story 3 - Check Zodiac Compatibility (Priority: P2)

A user wants to check compatibility between two zodiac animals (e.g., for relationship planning, business partnerships, or family dynamics) based on traditional Chinese astrology.

**Why this priority**: This is a highly engaging feature that adds fun and social value. Users often share zodiac compatibility results, increasing app virality. However, it's not essential for basic calendar functionality.

**Independent Test**: Can be fully tested by selecting two zodiac animals in the compatibility checker and receiving a compatibility rating (Great, Good, Fair, Poor) with explanation. Delivers standalone entertainment and cultural insight value.

**Acceptance Scenarios**:

1. **Given** I navigate to the "Zodiac Compatibility" section, **When** I select "Horse" as the first animal and "Dog" as the second animal, **Then** I see a compatibility rating (e.g., "Great Match - 90%") with an explanation (e.g., "Horse and Dog share loyalty and passion. Both value honesty and enjoy active lifestyles.")
2. **Given** I am checking compatibility, **When** I select incompatible animals (e.g., Horse and Rat), **Then** I see a lower rating (e.g., "Challenging - 40%") with constructive guidance (e.g., "Horse and Rat may clash in communication styles, but can succeed with mutual understanding and patience.")
3. **Given** I have set my birth date in settings, **When** I open the compatibility checker, **Then** my zodiac animal is pre-selected as Animal 1
4. **Given** I am viewing a compatibility result, **When** I tap "Share", **Then** I can share the compatibility result via social media or messaging apps

---

### User Story 4 - Understand Elemental Variations (Priority: P2)

A user wants to see how the Five Elements (Wood, Fire, Earth, Metal, Water) combine with zodiac animals to create 60 unique year types (e.g., Metal Rat, Water Tiger, Fire Horse).

**Why this priority**: This connects Sprint 9's Sexagenary Cycle work with Sprint 10's Zodiac Animals, providing deeper astrological insight. Essential for users interested in detailed astrology, but not critical for casual users.

**Independent Test**: Can be fully tested by viewing any year's detailed information and seeing both the zodiac animal AND its element variation (e.g., "2026: Fire Horse"). Delivers advanced astrological knowledge value.

**Acceptance Scenarios**:

1. **Given** I view the Year 2026 details, **When** the year information dialog opens, **Then** I see "Fire Horse" (not just "Horse"), along with explanations of how the Fire element influences Horse characteristics
2. **Given** I am browsing zodiac animals, **When** I select an animal (e.g., Dragon), **Then** I can toggle to view all 5 elemental variations (Wood Dragon, Fire Dragon, Earth Dragon, Metal Dragon, Water Dragon) with descriptions of how each differs
3. **Given** I view my birth year, **When** I check my zodiac profile, **Then** I see my specific elemental animal (e.g., "You are a Metal Ox") with personality traits specific to that combination
4. **Given** I am viewing the 60-year cycle chart, **When** I tap on any year, **Then** I see both the stem-branch (from Sprint 9) and the Element + Animal name

---

### User Story 5 - Access Zodiac Information from Multiple Entry Points (Priority: P3)

A user wants convenient access to zodiac information from various parts of the app (calendar header, date details, settings, dedicated zodiac page) without navigating away from their current task.

**Why this priority**: This enhances discoverability and user experience but is not essential for MVP functionality. It's about polishing the user journey rather than adding core features.

**Independent Test**: Can be fully tested by accessing zodiac information from at least 3 different locations (header icon, date detail page, settings menu) and verifying all paths lead to the same rich zodiac content.

**Acceptance Scenarios**:

1. **Given** I tap on the zodiac icon in the calendar header, **When** the action menu appears, **Then** I see options: "View Current Year", "Explore All Animals", "Check Compatibility", "My Zodiac Profile"
2. **Given** I am viewing a specific date's details, **When** I scroll to the "Year Information" section, **Then** I see a condensed zodiac card with the year's animal, element, and a "Learn More" button
3. **Given** I navigate to Settings, **When** I go to "Zodiac & Astrology" section, **Then** I can access: My Zodiac Profile, Set Birth Date, Compatibility Checker, and About Zodiac System
4. **Given** I pull down on the calendar to refresh, **When** the refresh animation plays, **Then** I see a subtle animation of the current year's zodiac animal

---

### Edge Cases

- **What happens when viewing dates near Lunar New Year boundary?**  
  → Zodiac animal must change at exact Lunar New Year (not Gregorian Jan 1). Example: Jan 28, 2026 shows Snake; Jan 29, 2026 shows Horse.

- **How does system handle birth dates before 1900 or after 2100?**  
  → Zodiac calculation uses modulo 12, so it works for any year. Validate against known historical charts for accuracy.

- **What if user hasn't set their birth date?**  
  → "My Zodiac Profile" shows a prompt: "Set your birth date in Settings to see your zodiac animal" with a direct link to settings.

- **How to handle cultures that use different zodiac systems?**  
  → Display both Vietnamese (12 Con Giáp) and Chinese (生肖) names. Rabbit vs Cat for the 4th animal is handled by locale settings.

- **What about leap lunar months affecting year transitions?**  
  → Lunar New Year calculation is handled by `CalendarService` from Sprint 9. Zodiac Service queries that for year boundaries.

- **What if zodiac images fail to load?**  
  → Fallback to Unicode zodiac emoji (🐭🐮🐯🐰🐲🐍🐴🐑🐵🐔🐶🐷) with the text name. Cache images locally for offline access.

- **How to handle different zodiac start dates (solar vs lunar new year)?**  
  → This app uses **Lunar New Year** as the zodiac transition point (traditional Vietnamese/Chinese system). Document this clearly in the "About" section.

---

## Requirements *(mandatory)*

### Functional Requirements

#### Core Zodiac System
- **FR-001**: System MUST calculate zodiac animal for any given Gregorian or Lunar date using the formula: `(Year - 4) % 12` mapped to [Rat=0, Ox=1, Tiger=2, Rabbit=3, Dragon=4, Snake=5, Horse=6, Goat=7, Monkey=8, Rooster=9, Dog=10, Pig=11]
- **FR-002**: System MUST determine zodiac year boundaries based on Lunar New Year (not Gregorian January 1), using `CalendarService.GetLunarNewYear(year)`
- **FR-003**: System MUST support all 12 zodiac animals with Vietnamese names (12 Con Giáp), Chinese names (生肖), and English names
- **FR-004**: System MUST combine zodiac animals with Five Elements to produce 60 unique year types (e.g., Metal Rat, Water Tiger, Fire Horse)

#### Data & Content
- **FR-005**: System MUST store comprehensive zodiac data including: personality traits, lucky numbers (at least 2 per animal), lucky colors (at least 3 per animal), lucky directions (at least 2 per animal), compatible animals (top 3), incompatible animals (top 2), famous historical figures (at least 3 per animal), and cultural folklore
- **FR-006**: System MUST provide zodiac information in English, Vietnamese, and Chinese (Traditional Chinese characters for Chinese zodiac names)
- **FR-007**: System MUST include high-quality zodiac animal illustrations (12 images, optimized for mobile, at least 512x512px, with dark mode variants)

#### User Interface
- **FR-008**: System MUST display current year's zodiac animal icon in the main calendar header with animal name
- **FR-009**: System MUST provide a dedicated "Zodiac Information" page accessible from the calendar header icon
- **FR-010**: Users MUST be able to browse all 12 zodiac animals via swipe gestures or navigation buttons on the Zodiac Information page
- **FR-011**: System MUST show zodiac year transition correctly when viewing dates across Lunar New Year boundary
- **FR-012**: System MUST display condensed zodiac information on the date detail view for selected dates

#### Compatibility System
- **FR-013**: System MUST implement a zodiac compatibility checker allowing users to select two animals and receive a compatibility score (0-100%)
- **FR-014**: System MUST provide compatibility ratings in 4 tiers: Great (80-100%), Good (60-79%), Fair (40-59%), Poor (0-39%)
- **FR-015**: System MUST display compatibility explanation text (at least 2-3 sentences) describing the relationship dynamic between selected animals
- **FR-016**: System MUST support sharing compatibility results via native share sheet (iOS/Android)

#### Personalization
- **FR-017**: System MUST calculate user's birth year zodiac animal when birth date is provided in settings
- **FR-018**: System MUST create a "My Zodiac Profile" section showing user's animal, element, personality traits, and lucky elements
- **FR-019**: System MUST pre-select user's zodiac animal in the compatibility checker (Animal 1 position)
- **FR-020**: System MUST allow users to set/update birth date from Settings → Zodiac & Astrology section

#### Performance & Caching
- **FR-021**: System MUST cache zodiac images locally to support offline viewing
- **FR-022**: System MUST cache zodiac data (JSON) locally to avoid repeated network calls
- **FR-023**: Zodiac animal calculation MUST complete in under 10ms (average, on mid-range devices)
- **FR-024**: Zodiac Information page MUST load in under 500ms with cached data

#### Integration with Sprint 9
- **FR-025**: System MUST integrate with `SexagenaryService` to retrieve Heavenly Stem and Earthly Branch for combining Element + Animal
- **FR-026**: System MUST use `EarthlyBranch` enum to map to zodiac animals (Tý→Rat, Sửu→Ox, Dần→Tiger, etc.)
- **FR-027**: Year detail view MUST show both Sexagenary notation (e.g., "Bính Ngọ") AND Elemental Animal name (e.g., "Fire Horse")

### Key Entities *(include if feature involves data)*

- **ZodiacAnimal**: Enum representing 12 animals (Rat, Ox, Tiger, Rabbit, Dragon, Snake, Horse, Goat, Monkey, Rooster, Dog, Pig) mapped to Earthly Branches
  
- **ZodiacInfo**: Comprehensive data model containing:
  - `Animal` (enum)
  - `VietnameseName` (string, e.g., "Tý - Chuột")
  - `ChineseName` (string, e.g., "鼠 - Shǔ")
  - `EnglishName` (string, e.g., "Rat")
  - `PersonalityTraits` (list of strings)
  - `LuckyNumbers` (list of integers)
  - `LuckyColors` (list of strings)
  - `LuckyDirections` (list of strings)
  - `CompatibleAnimals` (list of ZodiacAnimal)
  - `IncompatibleAnimals` (list of ZodiacAnimal)
  - `FamousPeople` (list of strings)
  - `CulturalSignificance` (string, 200-300 words)
  - `ImagePath` (string)

- **ElementalAnimal**: Combination of Five Element + Zodiac Animal
  - `Year` (int)
  - `Element` (FiveElement enum from Sprint 9)
  - `Animal` (ZodiacAnimal)
  - `DisplayName` (string, e.g., "Fire Horse")
  - `ElementalTraits` (string, how element modifies animal traits)

- **ZodiacCompatibility**: Compatibility relationship between two animals
  - `Animal1` (ZodiacAnimal)
  - `Animal2` (ZodiacAnimal)
  - `Score` (int, 0-100)
  - `Rating` (enum: Great, Good, Fair, Poor)
  - `Explanation` (string)

- **UserZodiacProfile**: User's personal zodiac information
  - `BirthDate` (DateTime)
  - `ZodiacAnimal` (ZodiacAnimal)
  - `Element` (FiveElement)
  - `ElementalAnimal` (string, e.g., "Metal Ox")
  - `PersonalizedTraits` (string)

---

## Success Criteria *(mandatory)*

### Measurable Outcomes

#### User Engagement
- **SC-001**: 80%+ of users who open the app view the current year's zodiac animal within their first session
- **SC-002**: 50%+ of users who tap the zodiac icon explore at least 3 different zodiac animals
- **SC-003**: 30%+ of users who set their birth date return to view their zodiac profile at least twice
- **SC-004**: Users spend an average of 2+ minutes on the Zodiac Information page (indicating engagement with educational content)

#### Functionality
- **SC-005**: Zodiac animal calculation achieves 100% accuracy when tested against 100 known reference years (2000-2099)
- **SC-006**: Zodiac year transitions correctly at Lunar New Year boundary in 100% of test cases across 50 years (2000-2050)
- **SC-007**: All 12 zodiac animals load successfully with images on both iOS and Android with 99.9%+ reliability

#### Performance
- **SC-008**: Zodiac Information page loads in under 500ms for 95% of users (measured via analytics)
- **SC-009**: Zodiac animal calculation completes in under 10ms on 90% of devices (benchmark test)
- **SC-010**: Image caching reduces data usage by 80%+ on repeat views (compared to no caching)

#### Quality
- **SC-011**: Zero P0/P1 bugs related to incorrect zodiac animal display in production for 30 days post-launch
- **SC-012**: Zodiac content passes cultural accuracy review by at least 2 Vietnamese cultural subject matter experts (SMEs)
- **SC-013**: Compatibility checker produces consistent results (same input = same output) in 100% of test cases

#### Localization
- **SC-014**: All 12 zodiac animals have complete Vietnamese, Chinese, and English translations with 100% coverage
- **SC-015**: Users from Vietnam rate the cultural authenticity of zodiac content as 4.5/5 or higher (via in-app survey)

#### Accessibility
- **SC-016**: All zodiac animal images have descriptive alt text for screen readers (100% coverage)
- **SC-017**: Zodiac Information page passes WCAG 2.1 AA accessibility standards (automated + manual testing)

#### Business Impact
- **SC-018**: Feature completion enables Sprint 11 (Dynamic Backgrounds Based on Zodiac Year) to proceed without blockers
- **SC-019**: App store screenshots featuring zodiac animals receive 20%+ higher click-through rates than generic screenshots (A/B test)
- **SC-020**: Social shares of compatibility results increase app virality by 15%+ (measured via referral tracking)

---

## Dependencies & Integration Points

### Sprint 9 Dependencies (CRITICAL)
- **SexagenaryService**: Required for calculating Heavenly Stem + Earthly Branch → Element + Animal mapping
- **EarthlyBranch Enum**: Direct mapping to zodiac animals (Tý=Rat, Sửu=Ox, etc.)
- **FiveElement Enum**: Required for elemental animal variations (Metal Rat, Water Tiger, etc.)
- **CalendarService**: Required for determining Lunar New Year dates (zodiac year boundaries)

### New Services to Create
- **ZodiacService**: Core service for zodiac calculations, data retrieval, and compatibility logic
- **ZodiacDataRepository**: Loads and caches zodiac information from JSON files
- **ZodiacCompatibilityEngine**: Implements compatibility scoring algorithms

### UI Components
- **ZodiacHeaderView**: Displays current year's zodiac animal in calendar header
- **ZodiacInformationPage**: Full-page view for exploring zodiac animals
- **ZodiacCompatibilityPage**: Interactive compatibility checker
- **ZodiacProfileView**: User's personal zodiac profile

### Data Files
- **ZodiacData.json**: Comprehensive zodiac information for all 12 animals (~50-100 KB)
- **ZodiacCompatibility.json**: Compatibility matrix for all 144 pairings (12x12)
- **Zodiac Images**: 12 high-quality animal illustrations (512x512px, ~100 KB each, optimized)

---

## Out of Scope (Explicitly NOT in Sprint 10)

- **Dynamic zodiac-themed backgrounds** → Sprint 11
- **Moon phases and lunar day characteristics** → Sprint 12
- **Hour-based zodiac (Earthly Branch hours)** → Sprint 16
- **Auspicious date finder based on zodiac** → Sprint 15
- **Zodiac-based horoscopes or predictions** → Future consideration
- **User-generated zodiac content or social features** → Phase 3
- **Zodiac animal animations or games** → Future consideration
- **API endpoints for zodiac data** → Not needed (local data only for now)

---

## Technical Constraints

- **Image Size**: Total zodiac images must not exceed 1.5 MB to keep app bundle size reasonable
- **Data Size**: ZodiacData.json must not exceed 100 KB to ensure fast loading
- **Performance**: Zodiac calculation must be O(1) complexity (simple modulo operation, no loops)
- **Offline Support**: All zodiac core functionality (view, browse, calculate) must work offline once data is cached
- **Localization**: Strings must use .resx resource files for easy translation updates
- **Compatibility**: Must work on iOS 15+ and Android 8.0+ (consistent with app's minimum requirements)

---

## Risk Assessment

### High Risk
- **Cultural Accuracy**: Zodiac traits and compatibility could be culturally inaccurate → **Mitigation**: Engage 2+ Vietnamese cultural SMEs for content review

### Medium Risk
- **Image Quality vs Size**: Balancing high-quality zodiac images with app size constraints → **Mitigation**: Use vector graphics (SVG) or optimize PNG/WebP with multiple resolutions
- **Compatibility Algorithm Complexity**: Zodiac compatibility is subjective and varies by source → **Mitigation**: Use a simplified, well-documented compatibility matrix based on mainstream Vietnamese astrology

### Low Risk
- **Sprint 9 Integration**: Dependency on SexagenaryService → **Mitigation**: Sprint 9 is complete and stable; integration is straightforward via existing enums
- **Performance**: Zodiac calculations are computationally trivial → **Mitigation**: Simple modulo operations, no performance concern

---

## Acceptance Checklist

- [ ] All 5 user stories have acceptance scenarios written and reviewed
- [ ] All 27 functional requirements are testable and have acceptance criteria
- [ ] ZodiacService implements zodiac calculation with 100% accuracy on test dataset
- [ ] All 12 zodiac animals have complete data (traits, lucky elements, folklore, images)
- [ ] Zodiac Information page displays all 12 animals with navigation (swipe/buttons)
- [ ] Compatibility checker calculates and displays scores for all 144 pairings
- [ ] Zodiac animal changes correctly at Lunar New Year boundary in all test cases
- [ ] User's birth year zodiac displays correctly when birth date is set
- [ ] All zodiac images load successfully on iOS and Android with fallback emoji
- [ ] Zodiac content passes cultural accuracy review by 2+ Vietnamese SMEs
- [ ] Performance benchmarks met: <10ms calculation, <500ms page load, 60 FPS scrolling
- [ ] Accessibility: All images have alt text, page passes WCAG 2.1 AA
- [ ] Localization: All strings available in English, Vietnamese, and Chinese
- [ ] Offline functionality works: Cached data loads without network
- [ ] Integration with Sprint 9: Elemental animals display correctly (e.g., "Fire Horse")
- [ ] Zero P0/P1 bugs in final testing

---

## Next Steps

1. **Review & Approve**: Stakeholders review this specification for completeness and accuracy
2. **Run `/speckit.plan`**: Generate technical implementation plan with architecture decisions
3. **Run `/speckit.tasks`**: Break down implementation into specific, actionable tasks
4. **Cultural SME Engagement**: Schedule reviews with Vietnamese cultural experts for content validation
5. **Design Review**: Have UI/UX team review zodiac page mockups and approve visual direction
6. **Sprint Planning**: Allocate 2 weeks (10 business days) for implementation + testing
7. **Begin Implementation**: Use `/speckit.implement` to start building the feature

---

**Last Updated**: January 26, 2026  
**Next Review**: Before Sprint 10 kickoff (estimated February 2026)  
**Prepared By**: GitHub Copilot (Spec-Kit Agent)
