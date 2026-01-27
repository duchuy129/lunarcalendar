# Feature Specification: Sexagenary Cycle Foundation (Can Chi / 干支)

**Feature Branch**: `feature/001-sexagenary-cycle`  
**Created**: January 25, 2026  
**Status**: Draft  
**Phase**: Phase 2 - Sprint 9  
**Input**: User description: "Transform the app from a basic calendar converter into a comprehensive Vietnamese/Chinese lunar calendar by implementing the foundational Sexagenary Cycle (Can Chi / 干支) system for authentic traditional date representation."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - View Today's Stem-Branch Date (Priority: P1)

A Vietnamese user opens the app and wants to know today's date in the traditional Can Chi (Stem-Branch) format, not just the Gregorian or simple lunar date. This is essential for cultural practices, astrology consultations, and traditional event planning.

**Why this priority**: This is the core value proposition of Phase 2 - enabling users to see authentic traditional calendar information. Without this, the feature has no value. All other stories depend on this foundational capability.

**Independent Test**: Can be fully tested by opening the app and verifying that today's date displays in Can Chi format (e.g., "Giáp Tý" for January 25, 2026) in the calendar header, and delivers immediate cultural value for users planning traditional activities.

**Acceptance Scenarios**:

1. **Given** I am on the main calendar view, **When** I look at the header section, **Then** I see today's date displayed in Can Chi format (e.g., "Ngày Giáp Tý") alongside the Gregorian and lunar dates
2. **Given** today is January 25, 2026 (lunar date: 26th day, 12th month, year of Snake), **When** I view the header, **Then** the stem-branch calculation shows the correct day stem-branch based on the traditional algorithm
3. **Given** I tap on the Can Chi date display, **When** the tooltip/info dialog appears, **Then** I see an explanation of what the stem (Thiên Can) and branch (Địa Chi) represent
4. **Given** I change the app language to English, **When** I view the header, **Then** the stem-branch names are displayed in English (e.g., "Day of Wood Rat")
5. **Given** I am offline with cached data, **When** I open the app, **Then** the Can Chi date still displays correctly without requiring network access

---

### User Story 2 - View Stem-Branch for Any Calendar Date (Priority: P2)

A user planning a wedding or important event wants to check the Can Chi (stem-branch) for specific future dates to determine auspicious timing according to traditional astrology.

**Why this priority**: This extends the P1 capability to any date, enabling practical planning use cases. Users need this for event planning, but it builds on the foundation of P1.

**Independent Test**: Can be fully tested by navigating to any month/year, selecting a date, and verifying the stem-branch appears on the calendar cell and in the date detail view, delivering value for event planning scenarios.

**Acceptance Scenarios**:

1. **Given** I am viewing the January 2026 calendar, **When** I look at each date cell, **Then** I see the day stem-branch displayed below the lunar date (e.g., "Giáp Tý" for day 1)
2. **Given** I navigate to March 2027, **When** I view any date, **Then** the stem-branch calculation adjusts correctly for that year and displays the accurate Can Chi
3. **Given** I tap on a specific date (e.g., February 14, 2026), **When** the date detail view opens, **Then** I see comprehensive stem-branch information including the day's element and zodiac animal
4. **Given** the calendar cell has limited space, **When** displaying stem-branch, **Then** the text is abbreviated or uses symbols while maintaining readability
5. **Given** I am viewing a date in the past (e.g., January 1, 2020), **When** I check its stem-branch, **Then** the calculation is accurate according to historical records

---

### User Story 3 - View Year's Stem-Branch and Zodiac Information (Priority: P2)

A user celebrating Lunar New Year wants to know what zodiac animal and element govern the current year according to the 60-year cycle, along with characteristics and predictions for the year.

**Why this priority**: This provides year-level context that enriches the daily stem-branch information. Users want to understand the broader astrological context of the year they're in.

**Independent Test**: Can be fully tested by viewing the year header or year info page and verifying the zodiac animal, element, and year stem-branch are displayed with cultural context, delivering value for annual planning and cultural understanding.

**Acceptance Scenarios**:

1. **Given** I am viewing the 2026 calendar, **When** I look at the year indicator, **Then** I see "Year of the Fire Horse" or "Bính Ngọ" (丙午) displayed prominently
2. **Given** I tap on the year indicator, **When** the year information dialog opens, **Then** I see detailed information including: year stem-branch, zodiac animal, five element, and brief characteristics
3. **Given** Lunar New Year has just passed (e.g., from Snake to Horse year), **When** I view dates before and after the new year, **Then** the zodiac animal changes correctly at the lunar new year boundary, not the Gregorian January 1
4. **Given** I select the year 2024, **When** I view year information, **Then** I see "Year of the Wood Dragon" (Giáp Thìn / 甲辰)
5. **Given** I am viewing year information in Vietnamese, **When** I switch to English, **Then** all zodiac and element names translate correctly

---

### User Story 4 - View Month and Hour Stem-Branch (Priority: P3)

A user consulting traditional almanac for auspicious activities wants to see not just the day's stem-branch, but also the month and hour stem-branch for precise timing according to traditional astrology.

**Why this priority**: This completes the full four-pillar system (Year-Month-Day-Hour) but is lower priority because most users primarily care about day-level information. Hour-level is primarily for advanced practitioners.

**Independent Test**: Can be fully tested by viewing the date detail view and verifying month stem-branch displays, and by selecting a time of day to see hour stem-branch, delivering value for advanced astrological consultations.

**Acceptance Scenarios**:

1. **Given** I tap on any date to open the detail view, **When** the view loads, **Then** I see both the month stem-branch and day stem-branch displayed (e.g., "Month: Canh Dần, Day: Giáp Tý")
2. **Given** I am viewing today's date details, **When** I look at the hour stem-branch section, **Then** I see the current hour's stem-branch (e.g., "Hour: Ất Sửu" for 1-3 AM period)
3. **Given** I want to check an auspicious hour for tomorrow, **When** I tap "View Hours" on tomorrow's date, **Then** I see a list of all 12 two-hour periods with their stem-branches
4. **Given** lunar months sometimes have leap months, **When** viewing a leap month, **Then** the month stem-branch calculation handles the leap month correctly
5. **Given** I am viewing hour stem-branches, **When** I tap on an hour, **Then** I see detailed information about that hour's zodiac animal and element

---

### User Story 5 - Educational Tooltips for Stem-Branch System (Priority: P3)

A new user unfamiliar with the Can Chi system wants to understand what these traditional terms mean and how they relate to their calendar usage.

**Why this priority**: Educational content improves user onboarding but is lower priority than actually displaying the information. Users can still use the feature without understanding the deep meaning initially.

**Independent Test**: Can be fully tested by tapping info icons throughout the app and verifying that clear, culturally-appropriate explanations appear, delivering educational value for users learning about the traditional calendar system.

**Acceptance Scenarios**:

1. **Given** I see a stem-branch date for the first time, **When** I tap the info icon (ⓘ) next to it, **Then** I see a simple explanation: "The stem-branch system (Can Chi) combines 10 Heavenly Stems and 12 Earthly Branches to create a 60-year cycle used for dating and astrology"
2. **Given** I am viewing the 10 Heavenly Stems list, **When** I tap on any stem (e.g., "Giáp"), **Then** I see its element association (Wood), yin/yang polarity, and cultural significance
3. **Given** I am viewing the 12 Earthly Branches list, **When** I tap on any branch (e.g., "Tý"), **Then** I see its zodiac animal (Rat), element, and time period association (11 PM - 1 AM)
4. **Given** I am a first-time user, **When** I first access the stem-branch feature, **Then** a brief onboarding tooltip appears explaining what I'm seeing
5. **Given** I tap "Learn More" on any stem-branch element, **When** the educational page loads, **Then** I see culturally accurate information sourced from reputable Vietnamese/Chinese references

---

### Edge Cases

- **What happens when viewing dates outside the 1900-2100 range?** The ChineseLunisolarCalendar has limits. The app should gracefully disable stem-branch display or show a "not available" message for dates outside the supported range, without crashing.

- **What happens when the user's device locale is set to a language other than Vietnamese, Chinese, or English?** The app should fall back to English for stem-branch names while maintaining the local number/date formatting for other UI elements.

- **How does the system handle the transition between lunar years on Lunar New Year?** The year stem-branch must change at the precise moment of Lunar New Year (not January 1), and the UI should clearly indicate which year's zodiac applies to dates near the transition.

- **What if the lunar month calculation results in a leap month?** The month stem-branch calculation should follow traditional rules where leap months don't get their own stem-branch but inherit from the month they duplicate.

- **How does the app perform when calculating stem-branches for an entire year (365 dates)?** Calculations must be optimized and cached to avoid UI lag when scrolling through months or viewing year-level views.

- **What happens if the server API for stem-branch data is unreachable?** The mobile app should calculate stem-branches locally using embedded algorithms and only sync for any additional cultural context data, ensuring offline functionality.

- **How are stem-branch names displayed on small phone screens vs. tablets?** The UI should adapt, using abbreviated forms on small screens (e.g., "甲子" instead of "Giáp Tý") while showing full names on tablets.

---

## Requirements *(mandatory)*

### Functional Requirements

#### Core Calculation Requirements

- **FR-001**: System MUST calculate the correct day stem-branch (Ngày Can Chi) for any date within the Gregorian range 1900-2100 using the traditional Sexagenary cycle algorithm
- **FR-002**: System MUST calculate the correct year stem-branch (Năm Can Chi) and associated zodiac animal for any lunar year within the supported range
- **FR-003**: System MUST calculate the correct month stem-branch (Tháng Can Chi) based on the lunar month and year stem
- **FR-004**: System MUST calculate the correct hour stem-branch (Giờ Can Chi) for each of the 12 two-hour periods in a day based on the day stem
- **FR-005**: System MUST correctly identify leap lunar months and handle their stem-branch calculation according to traditional rules

#### Display Requirements

- **FR-006**: System MUST display today's day stem-branch prominently in the calendar header alongside Gregorian and lunar dates
- **FR-007**: System MUST display day stem-branch on each calendar cell in the monthly view, positioned below the lunar date
- **FR-008**: System MUST display year stem-branch and zodiac animal in the year indicator/header
- **FR-009**: System MUST show month stem-branch in the date detail view when a user taps on any date
- **FR-010**: System MUST provide an expandable view showing all 12 hour stem-branches when viewing a specific date's details

#### Localization Requirements

- **FR-011**: System MUST support displaying stem names (Thiên Can / 天干) in Vietnamese, Chinese characters, and English
- **FR-012**: System MUST support displaying branch names (Địa Chi / 地支) in Vietnamese, Chinese characters, and English
- **FR-013**: System MUST support displaying zodiac animal names in all three languages (Vietnamese, Chinese, English)
- **FR-014**: System MUST support displaying Five Element names (Ngũ hành / 五行) in all three languages
- **FR-015**: System MUST allow dynamic language switching without requiring app restart, updating all stem-branch displays immediately

#### Educational & Context Requirements

- **FR-016**: System MUST provide info icons (ⓘ) that display explanatory tooltips for stem-branch terms throughout the UI
- **FR-017**: System MUST include a reference page listing all 10 Heavenly Stems with their elements, yin/yang properties, and meanings
- **FR-018**: System MUST include a reference page listing all 12 Earthly Branches with their zodiac animals, elements, and time periods
- **FR-019**: System MUST display the Five Element (Metal, Wood, Water, Fire, Earth) association for any given date when viewing details
- **FR-020**: System MUST show yin/yang polarity for stems when displaying detailed information

#### Performance & Offline Requirements

- **FR-021**: System MUST calculate stem-branch for any single date in under 50 milliseconds to maintain UI responsiveness
- **FR-022**: System MUST cache stem-branch calculations for the currently displayed month to avoid repeated calculations
- **FR-023**: System MUST work completely offline, performing all stem-branch calculations on-device without requiring API calls
- **FR-024**: System MUST pre-calculate and cache stem-branch data for the current year and adjacent years during app initialization

#### Data Integrity Requirements

- **FR-025**: System MUST validate stem-branch calculations against known historical dates to ensure accuracy (e.g., verify that January 1, 2000 = 庚辰年丙子月壬戌日)
- **FR-026**: System MUST handle the Lunar New Year boundary correctly, applying the new year's zodiac only to dates after the Lunar New Year
- **FR-027**: System MUST persist user's preferred stem-branch display format (Vietnamese, Chinese, English) in local storage

#### API Requirements (Backend)

- **FR-028**: API MUST expose endpoint `GET /api/calendar/sexagenary/{date}` returning full stem-branch information for a specific date
- **FR-029**: API MUST expose endpoint `GET /api/calendar/year-info/{year}` returning year zodiac, element, and characteristics
- **FR-030**: API MUST return stem-branch names in multiple languages based on `Accept-Language` header or query parameter
- **FR-031**: API MUST include caching headers (ETags, Cache-Control) for stem-branch responses since this data never changes
- **FR-032**: API MUST return 400 Bad Request for dates outside the 1900-2100 supported range with clear error message

### Key Entities *(include if feature involves data)*

- **SexagenaryInfo**: Represents complete stem-branch information for a date
  - DayStem (Thiên Can): One of 10 Heavenly Stems (Giáp through Quý)
  - DayBranch (Địa Chi): One of 12 Earthly Branches (Tý through Hợi)
  - DayElement: The Five Element associated with the day (Metal, Wood, Water, Fire, Earth)
  - DayYinYang: Yin or Yang polarity
  - MonthStem, MonthBranch: Stem-branch for the lunar month
  - YearStem, YearBranch: Stem-branch for the lunar year
  - YearZodiac: The zodiac animal for the year (Rat through Pig)
  - HourStemBranches: Collection of 12 hour periods with their stem-branches

- **HeavenlyStem**: The 10 Heavenly Stems (Thiên Can / 天干)
  - Index (0-9)
  - VietnameseName (Giáp, Ất, Bính, Đinh, Mậu, Kỷ, Canh, Tân, Nhâm, Quý)
  - ChineseName (甲, 乙, 丙, 丁, 戊, 己, 庚, 辛, 壬, 癸)
  - EnglishName (Wood, Wood, Fire, Fire, Earth, Earth, Metal, Metal, Water, Water)
  - Element: Five Element association
  - YinYang: Yin or Yang polarity

- **EarthlyBranch**: The 12 Earthly Branches (Địa Chi / 地支)
  - Index (0-11)
  - VietnameseName (Tý, Sửu, Dần, Mão, Thìn, Tỵ, Ngọ, Mùi, Thân, Dậu, Tuất, Hợi)
  - ChineseName (子, 丑, 寅, 卯, 辰, 巳, 午, 未, 申, 酉, 戌, 亥)
  - EnglishName (Rat, Ox, Tiger, Rabbit, Dragon, Snake, Horse, Goat, Monkey, Rooster, Dog, Pig)
  - ZodiacAnimal: Associated animal
  - Element: Five Element association
  - HourPeriod: Time range (e.g., 11 PM - 1 AM for Tý)

- **FiveElement** (Ngũ hành / 五行): The five elements
  - Metal (Kim / 金)
  - Wood (Mộc / 木)
  - Water (Thủy / 水)
  - Fire (Hỏa / 火)
  - Earth (Thổ / 土)
  - Relationships: Generating and overcoming cycles

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Users can immediately see today's stem-branch date on the main calendar screen within 1 second of app launch
- **SC-002**: Stem-branch calculations complete in under 50ms per date, verified through performance testing
- **SC-003**: 90% of users viewing the calendar can identify the current year's zodiac animal within 5 seconds of opening the app
- **SC-004**: All stem-branch calculations match validation dataset of 1000+ historically verified dates with 100% accuracy
- **SC-005**: Feature works completely offline with 0% dependency on network API calls for core calculations
- **SC-006**: Calendar scrolling performance remains above 55 FPS when displaying stem-branch data on all dates in monthly view
- **SC-007**: Users can switch between Vietnamese, Chinese, and English stem-branch names with language change taking effect within 500ms
- **SC-008**: At least 70% of beta testers successfully use the educational tooltips to understand basic stem-branch concepts
- **SC-009**: Zero crashes or errors when calculating stem-branches for any date within 1900-2100 range during stress testing
- **SC-010**: App size increase due to stem-branch feature is under 2MB to maintain offline-first principle without bloat

### User Acceptance

- **SC-011**: At least 80% of Phase 2 beta testers rate the stem-branch feature as "useful" or "very useful" for their cultural practices
- **SC-012**: Users planning traditional events report that having stem-branch information reduces time spent consulting external sources by at least 50%
- **SC-013**: Cultural accuracy validated by at least 2 subject matter experts (e.g., Vietnamese astrology practitioners or cultural historians)

### Technical Quality

- **SC-014**: Unit test coverage for SexagenaryService and calculation algorithms exceeds 95%
- **SC-015**: All API endpoints return responses within 200ms at p95 latency
- **SC-016**: Backend can handle 100 concurrent stem-branch calculation requests without degradation
