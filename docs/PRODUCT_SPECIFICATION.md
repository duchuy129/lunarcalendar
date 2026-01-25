# Vietnamese Lunar Calendar Application
## Product Specification Document

**Version:** 2.0  
**Date:** January 3, 2026  
**Status:** MVP Complete - Phase 1 Deployed

---

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [Product Overview](#product-overview)
3. [Target Audience](#target-audience)
4. [Core Features](#core-features)
5. [User Flows](#user-flows)
6. [Business Logic](#business-logic)
7. [Data Requirements](#data-requirements)
8. [Non-Functional Requirements](#non-functional-requirements)
9. [Future Enhancements](#future-enhancements)

---

## 1. Executive Summary

The Vietnamese Lunar Calendar is a mobile application that provides comprehensive lunar calendar information with a focus on Vietnamese cultural traditions. The app bridges the gap between the Gregorian and lunar calendar systems, helping users track important holidays, traditional festivals, and lunar special days.

**Key Differentiators:**
- **Offline-First Architecture**: All calculations happen on-device, no internet required
- **Guest Mode**: Full functionality without requiring user registration
- **Cultural Focus**: Deep integration of Vietnamese holidays and traditions
- **Bilingual Support**: Full Vietnamese and English localization
- **Cross-Platform**: iOS, iPad, Android, and macOS support

---

## 2. Product Overview

### 2.1 Problem Statement

Vietnamese diaspora and cultural practitioners need a reliable tool to:
- Convert between Gregorian and lunar dates
- Track traditional Vietnamese holidays and festivals
- Plan cultural celebrations and ancestral commemorations
- Understand lunar calendar significance for agricultural and spiritual practices

### 2.2 Solution

A mobile calendar application that:
- Displays both Gregorian and lunar dates simultaneously
- Highlights Vietnamese holidays with cultural context
- Works completely offline for reliability
- Requires no registration for immediate use
- Provides rich cultural information for each holiday

### 2.3 Current Status

**Phase 1 (MVP) - ✅ COMPLETE**
- Core calendar functionality
- Holiday display and details
- Offline support
- Bilingual localization (Vietnamese/English)
- Guest mode (no authentication required)

**Phase 2 - PLANNED**
- Astronomical features (moon phases, solar terms)
- Astrological features (zodiac, horoscopes)
- Advanced cultural features

**Phase 3 - FUTURE**
- User authentication
- Personal event management
- Calendar sharing
- Social features

---

## 3. Target Audience

### 3.1 Primary Users

1. **Vietnamese Diaspora (Ages 25-65)**
   - Living abroad, maintaining cultural connections
   - Need to track traditional holidays
   - Want to teach children about cultural heritage

2. **Cultural Practitioners (Ages 30-70)**
   - Buddhist temples and practitioners
   - Traditional medicine practitioners
   - Feng Shui consultants
   - Need accurate lunar date calculations

3. **Family Planners (Ages 25-55)**
   - Planning weddings, ancestral commemorations
   - Scheduling family gatherings around holidays
   - Need to coordinate lunar and Gregorian dates

### 3.2 Secondary Users

4. **Students & Educators**
   - Learning about Vietnamese culture
   - Teaching cultural studies
   - Research purposes

5. **Business Owners**
   - Planning promotions around cultural holidays
   - Scheduling business events appropriately
   - Cultural sensitivity in operations

---

## 4. Core Features

### 4.1 Calendar Display

**Feature ID:** F-CAL-001  
**Priority:** P0 (Critical)  
**Status:** ✅ Implemented

#### Description
Monthly calendar view displaying both Gregorian and lunar dates for each day.

#### Functional Requirements
- Display current month by default on app launch
- Show Gregorian date prominently for each day
- Show corresponding lunar date below Gregorian date
- Highlight current day distinctly
- Support month navigation (previous/next)
- Display month and year header
- Show day of week labels (Sun-Sat)
- Handle month transitions correctly
- Display 5-6 week rows as needed

#### Display Format
- **Gregorian Date**: Arabic numerals (1-31)
- **Lunar Date**: Vietnamese format "1/1" or "Mùng 1/Tháng 1"
- **Current Day**: Highlighted with distinct background color
- **Weekend Days**: Optional visual distinction

#### Business Rules
- Lunar dates are calculated based on Vietnamese timezone (UTC+7)
- Leap months are indicated with special formatting
- Month starts on Sunday (configurable in settings)

---

### 4.2 Holiday Display

**Feature ID:** F-HOL-001  
**Priority:** P0 (Critical)  
**Status:** ✅ Implemented

#### Description
Visual indication of Vietnamese holidays on calendar dates with detailed information.

#### Functional Requirements
- Display colored dots or indicators on holiday dates
- Color-code holidays by type:
  - **Red (#DC143C)**: Major holidays (Tết, National Day)
  - **Gold (#FFD700)**: Traditional festivals
  - **Green (#32CD32)**: Seasonal celebrations
  - **Purple (#9370DB)**: Lunar special days (1st & 15th)
- Show holiday name when date is tapped
- Support multiple holidays on same date
- Display holiday details in dedicated view

#### Holiday Types

**Major Holidays (Public):**
- Tết Nguyên Đán (Lunar New Year) - Days 1-3
- Giỗ Tổ Hùng Vương (Hung Kings' Festival)
- Tết Dương Lịch (New Year's Day)
- Ngày Giải Phóng Miền Nam (Reunification Day)
- Ngày Quốc Tế Lao Động (International Labor Day)
- Quốc Khánh (National Day)

**Traditional Festivals:**
- Tết Nguyên Tiêu (Lantern Festival)
- Tết Hàn Thực (Cold Food Festival)
- Tết Đoan Ngọ (Dragon Boat Festival)
- Vu Lan (Ullambana Festival)
- Tết Trung Thu (Mid-Autumn Festival)
- Tết Trùng Cửu (Double Ninth Festival)
- Lễ Cúng Ông Công Ông Táo (Kitchen Gods Day)
- Tết Ông Bà (Vietnamese New Year's Eve)

**Lunar Special Days:**
- 1st day of each lunar month (Mùng 1)
- 15th day of each lunar month (Rằm)

---

### 4.3 Holiday Details

**Feature ID:** F-HOL-002  
**Priority:** P0 (Critical)  
**Status:** ✅ Implemented

#### Description
Detailed information page for each holiday with cultural significance and traditions.

#### Functional Requirements
- Display holiday name (localized)
- Show both Gregorian and lunar date
- Display detailed description (localized)
- Show animal zodiac sign (for Tết holidays)
- Display color-coded badge by holiday type
- Support scrolling for long descriptions
- Include "back" navigation
- Responsive layout for all screen sizes

#### Information Included
- **Holiday Name**: Full name in current language
- **Date Information**: 
  - Gregorian date (e.g., "February 10, 2025")
  - Lunar date (e.g., "Mùng 1 Tháng Giêng")
- **Animal Sign**: For Tết holidays (e.g., "Year of the Snake")
- **Description**: Cultural significance, traditions, customs
- **Holiday Type**: Visual indicator with color
- **Public Holiday Status**: Indicator if it's a public day off

---

### 4.4 Year Holidays View

**Feature ID:** F-HOL-003  
**Priority:** P1 (High)  
**Status:** ✅ Implemented

#### Description
Comprehensive list of all holidays for a selected year with filtering capabilities.

#### Functional Requirements
- Display all holidays for current year by default
- Group holidays by type with section headers
- Allow year selection (year picker)
- Filter holidays by type:
  - All Holidays
  - Major Holidays only
  - Traditional Festivals only
  - Lunar Special Days only
- Show holiday count per category
- Support scrolling through long lists
- Tap to view holiday details

#### Display Format
```
Major Holidays (8)
├─ Tết Dương Lịch - January 1, 2026
├─ Tết Nguyên Đán - February 10, 2026
└─ ...

Traditional Festivals (12)
├─ Tết Nguyên Tiêu - February 24, 2026
└─ ...

Lunar Special Days (24)
├─ Mùng 1 Tháng Giêng - February 10, 2026
└─ ...
```

#### Business Rules
- Year range: 1900-2100
- Default to current year
- Filter persists during session
- Holidays sorted chronologically within each category

---

### 4.5 Today's Information

**Feature ID:** F-CAL-002  
**Priority:** P1 (High)  
**Status:** ✅ Implemented

#### Description
Display comprehensive information about today's date at the top of calendar view.

#### Functional Requirements
- Show current Gregorian date (full format)
- Show current lunar date (Vietnamese format)
- Display any holidays occurring today
- Highlight lunar special days (1st & 15th)
- Update automatically at midnight
- Localized date formatting

#### Display Examples

**Vietnamese:**
```
Hôm Nay
Thứ Sáu, 03 Tháng 1, 2026
Mùng 4 Tháng Chạp
```

**English:**
```
Today
Friday, January 3, 2026
4th Day, 12th Month
```

---

### 4.6 Month/Year Navigation

**Feature ID:** F-CAL-003  
**Priority:** P1 (High)  
**Status:** ✅ Implemented

#### Description
Intuitive navigation to view different months and years.

#### Functional Requirements
- **Month Navigation**:
  - Previous/Next month buttons
  - Swipe gestures (left for next, right for previous)
  - Month/Year header tap to open picker
  - Smooth animations between months

- **Month/Year Picker**:
  - Month selection (1-12)
  - Year selection with scrollable list
  - "Today" button to return to current month
  - Haptic feedback on selection (iOS)
  - Modal presentation

#### Business Rules
- Support years: 1900-2100 (ChineseLunisolarCalendar limitation)
- Default to current month/year on app launch
- Remember last viewed month within session
- Reset to current month on app restart

---

### 4.7 Settings & Preferences

**Feature ID:** F-SET-001  
**Priority:** P1 (High)  
**Status:** ✅ Implemented

#### Description
User preferences and app configuration options.

#### Functional Requirements
- **Language Selection**:
  - Vietnamese (default)
  - English
  - Apply immediately without restart
  - Persist across sessions

- **Theme Options**:
  - Light mode
  - Dark mode (future)
  - System default (future)

- **Display Preferences**:
  - Week starts on: Sunday/Monday
  - Date format preferences (future)
  - Show/hide lunar special days indicators

- **About Section**:
  - App version
  - Privacy Policy link
  - Terms of Service link
  - Contact information

#### Business Rules
- Settings persist locally (no account required)
- Language changes affect all text immediately
- Settings stored in device preferences
- Default language: Vietnamese

---

### 4.8 Offline Support

**Feature ID:** F-OFF-001  
**Priority:** P0 (Critical)  
**Status:** ✅ Implemented

#### Description
Full application functionality without internet connection.

#### Functional Requirements
- **Local Calculations**:
  - All lunar date calculations performed on-device
  - No API calls required for basic functionality
  - Instant calculation results

- **Embedded Data**:
  - Holiday data bundled with app
  - Localization strings embedded
  - No external data dependencies

- **Local Storage**:
  - SQLite database for calculated dates (caching)
  - Preference storage in device storage
  - No cloud synchronization required

#### Business Rules
- App works 100% offline for all MVP features
- No "loading" states for calculations
- Internet connection optional (for future features)
- No error messages due to network unavailability

---

## 5. User Flows

### 5.1 First Launch Experience

```
1. User opens app
   ├─ Splash screen displays (1-2 seconds)
   └─ Navigate to Calendar Page

2. Calendar Page loads
   ├─ Show current month/year
   ├─ Highlight today's date
   ├─ Display "Today" section at top
   └─ Load holidays for visible month

3. User explores
   ├─ Swipe between months
   ├─ Tap dates to see details
   ├─ Tap holidays to learn more
   └─ Access settings to change language
```

**No registration or login required** - immediate value

---

### 5.2 Viewing Holiday Details

```
1. User sees holiday indicator on calendar
   └─ Colored dot under date

2. User taps on holiday date
   ├─ Navigate to Holiday Detail Page
   └─ Show loading state (minimal)

3. Holiday Detail Page displays
   ├─ Holiday name (large, prominent)
   ├─ Date information (Gregorian + Lunar)
   ├─ Animal sign (if applicable)
   ├─ Detailed description
   └─ Color-coded type indicator

4. User actions
   ├─ Read description (scroll if needed)
   ├─ Navigate back to calendar
   └─ Share holiday info (future)
```

---

### 5.3 Checking Upcoming Holidays

```
1. User taps "Year Holidays" tab
   └─ Navigate to Year Holidays Page

2. Page loads with current year
   ├─ Display all holidays grouped by type
   ├─ Show section headers with counts
   └─ Scroll to current date section

3. User interactions
   ├─ Change year using year picker
   ├─ Filter by holiday type
   ├─ Tap holiday to view details
   └─ Scroll through list

4. Filter holidays (optional)
   ├─ Tap filter button
   ├─ Select category
   └─ View filtered results
```

---

### 5.4 Changing Language

```
1. User taps "Settings" tab
   └─ Navigate to Settings Page

2. User taps "Language" option
   ├─ Show language picker
   └─ Current selection indicated

3. User selects new language
   ├─ Apply immediately
   ├─ Update all UI text
   ├─ Update holiday names/descriptions
   └─ Save preference

4. User returns to calendar
   └─ All content in new language
```

---

### 5.5 Finding Specific Date

```
1. User wants to find lunar date for specific Gregorian date
   
2. Method 1: Navigate by Month
   ├─ Swipe through months
   └─ Find target date

3. Method 2: Use Month/Year Picker
   ├─ Tap month/year header
   ├─ Select month and year
   ├─ Tap "Done"
   └─ Calendar jumps to selected month

4. View date information
   ├─ Gregorian date shown prominently
   ├─ Lunar date shown below
   └─ Any holidays indicated
```

---

## 6. Business Logic

### 6.1 Lunar Calendar Calculation

#### Algorithm
Uses .NET's built-in `ChineseLunisolarCalendar` class for accurate calculations.

#### Calculation Process
```
Input: Gregorian Date (DateTime)

1. Convert to Lunar Components
   ├─ Lunar Year (e.g., 2025)
   ├─ Lunar Month (1-12, or 13 for leap month)
   ├─ Lunar Day (1-30)
   └─ Is Leap Month (boolean)

2. Calculate Display Values
   ├─ Adjust month number if after leap month
   ├─ Generate Heavenly Stem (Thiên Can)
   ├─ Generate Earthly Branch (Địa Chi)
   ├─ Determine Animal Sign (12 zodiac animals)
   └─ Format names (Vietnamese traditional names)

Output: LunarDate object with all information
```

#### Heavenly Stems (Thiên Can)
Position calculated by: `(lunarYear - 4) % 10`
```
0: Giáp (甲) - Wood (+)
1: Ất (乙) - Wood (-)
2: Bính (丙) - Fire (+)
3: Đinh (丁) - Fire (-)
4: Mậu (戊) - Earth (+)
5: Kỷ (己) - Earth (-)
6: Canh (庚) - Metal (+)
7: Tân (辛) - Metal (-)
8: Nhâm (壬) - Water (+)
9: Quý (癸) - Water (-)
```

#### Earthly Branches (Địa Chi)
Position calculated by: `(lunarYear - 4) % 12`
```
0: Tý (子) - Rat
1: Sửu (丑) - Ox
2: Dần (寅) - Tiger
3: Mão (卯) - Rabbit
4: Thìn (辰) - Dragon
5: Tỵ (巳) - Snake
6: Ngọ (午) - Horse
7: Mùi (未) - Goat
8: Thân (申) - Monkey
9: Dậu (酉) - Rooster
10: Tuất (戌) - Dog
11: Hợi (亥) - Pig
```

#### Lunar Month Names (Vietnamese)
```
1: Tháng Giêng (正月)
2: Tháng Hai (二月)
3: Tháng Ba (三月)
4: Tháng Tư (四月)
5: Tháng Năm (五月)
6: Tháng Sáu (六月)
7: Tháng Bảy (七月)
8: Tháng Tám (八月)
9: Tháng Chín (九月)
10: Tháng Mười (十月)
11: Tháng Một (冬月)
12: Tháng Chạp (腊月)

Leap Month: "Tháng Nhuận" prefix
```

#### Lunar Day Names (Vietnamese)
```
1: Mùng 1 (初一)
2: Mùng 2 (初二)
...
10: Mùng 10 (初十)
11: Ngày 11 (十一)
...
15: Ngày 15 (十五) - "Rằm" (Full Moon)
...
30: Ngày 30 (三十) - "Tết" (Month End)
```

---

### 6.2 Holiday Calculation

#### Data Source
Holidays are defined in `HolidaySeeder.cs` with two types:

**1. Lunar-Based Holidays**
```csharp
LunarMonth: 1-12
LunarDay: 1-30
IsLeapMonth: true/false
```

**2. Gregorian-Based Holidays**
```csharp
GregorianMonth: 1-12
GregorianDay: 1-31
```

#### Calculation Process

**For Lunar Holidays:**
```
Input: Year (Gregorian), Holiday (Lunar Date)

1. Determine target lunar year
   ├─ Calculate lunar year for January 1st of Gregorian year
   └─ Also check lunar year for December 31st (may differ)

2. For each lunar year in range
   ├─ Convert lunar date to Gregorian date
   ├─ Check if Gregorian year matches target
   └─ If match, add to occurrences list

3. Handle leap months
   ├─ Skip if leap month doesn't exist in that year
   └─ Use correct month calculation

Output: List of HolidayOccurrence with Gregorian dates
```

**For Gregorian Holidays:**
```
Input: Year, Holiday (Gregorian Date)

1. Create DateTime for specified date
2. Calculate corresponding lunar date
3. Create HolidayOccurrence

Output: HolidayOccurrence with lunar date info
```

#### Lunar Special Days

Automatically generated for each month:
```
For each month (1-12) in lunar year:
  ├─ Create holiday for 1st day (Mùng 1)
  └─ Create holiday for 15th day (Rằm)
  
Convert to Gregorian dates for display
```

---

### 6.3 Date Formatting Logic

#### Vietnamese Format
```
Full Date: "Thứ [Dayofweek], [Day] Tháng [Month], [Year]"
Example: "Thứ Sáu, 03 Tháng 1, 2026"

Lunar Date: "[DayName] Tháng [MonthName]"
Example: "Mùng 4 Tháng Chạp"
```

#### English Format
```
Full Date: "[Dayofweek], [Month] [Day], [Year]"
Example: "Friday, January 3, 2026"

Lunar Date: "[Day]th Day, [Month]th Month"
Example: "4th Day, 12th Month"
```

#### Context-Aware Formatting
- **Calendar Cells**: Compact format "1/1" or "Mùng 1"
- **Today Section**: Full format with day of week
- **Holiday Details**: Full format with year
- **Year Holidays List**: Medium format without day of week

---

## 7. Data Requirements

### 7.1 Core Data Models

#### LunarDate
```
Properties:
- GregorianDate: DateTime
- LunarYear: int (1900-2100)
- LunarMonth: int (1-12)
- LunarDay: int (1-30)
- IsLeapMonth: boolean
- LunarYearName: string (e.g., "Giáp Thìn năm")
- LunarMonthName: string (e.g., "Tháng Giêng")
- LunarDayName: string (e.g., "Mùng 1")
- AnimalSign: string (e.g., "Dragon")

Calculated at Runtime: Yes
Stored in Database: Yes (for caching)
```

#### Holiday
```
Properties:
- Id: int (unique identifier)
- Name: string (default language)
- Description: string (default language)
- NameResourceKey: string (for localization)
- DescriptionResourceKey: string (for localization)
- LunarMonth: int (0 if Gregorian-based)
- LunarDay: int (0 if Gregorian-based)
- IsLeapMonth: boolean
- GregorianMonth: int? (null if lunar-based)
- GregorianDay: int? (null if lunar-based)
- Type: HolidayType enum
- ColorHex: string (e.g., "#DC143C")
- IsPublicHoliday: boolean
- Culture: string (e.g., "Vietnamese")

Data Source: Embedded in app (HolidaySeeder)
Updatable: No (requires app update)
```

#### HolidayOccurrence
```
Properties:
- Holiday: Holiday (reference)
- GregorianDate: DateTime (actual occurrence)
- AnimalSign: string (for Tết holidays)

Computed Properties:
- HolidayName: string
- HolidayDescription: string
- ColorHex: string
- IsPublicHoliday: boolean
- LunarDateDisplay: string
- AnimalSignDisplay: string

Calculated at Runtime: Yes
Purpose: Links holiday definitions to specific dates
```

#### HolidayType (Enum)
```
Values:
1. MajorHoliday - Public holidays, most important
2. TraditionalFestival - Cultural festivals
3. SeasonalCelebration - Seasonal events
4. LunarSpecialDay - 1st and 15th of lunar months
```

---

### 7.2 Local Storage

#### SQLite Database Schema

**Table: LunarDates**
```sql
CREATE TABLE LunarDates (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    GregorianDate TEXT NOT NULL UNIQUE,
    LunarYear INTEGER NOT NULL,
    LunarMonth INTEGER NOT NULL,
    LunarDay INTEGER NOT NULL,
    IsLeapMonth INTEGER NOT NULL,
    LunarYearName TEXT NOT NULL,
    LunarMonthName TEXT NOT NULL,
    LunarDayName TEXT NOT NULL,
    AnimalSign TEXT NOT NULL,
    CreatedAt TEXT DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_gregorian_date ON LunarDates(GregorianDate);
```

**Purpose:**
- Cache calculated lunar dates
- Improve performance for repeated queries
- Reduce calculation overhead

**Data Lifecycle:**
- Written on first calculation
- Read on subsequent access
- No expiration (lunar dates don't change)
- Database can be cleared to reclaim space

---

### 7.3 Embedded Resources

#### Localization Resources
```
Files:
- AppResources.resx (English - default)
- AppResources.vi.resx (Vietnamese)

Content:
- UI labels and buttons
- Holiday names and descriptions
- Section headers
- Error messages
- Settings options

Format: .NET Resource files (.resx)
Access: Type-safe generated classes
```

#### Holiday Data
```
File: HolidaySeeder.cs
Format: C# code (compiled into app)

Contains:
- 45+ Vietnamese holidays
- Lunar and Gregorian definitions
- Color codes
- Public holiday flags
- Localization keys

Updates: Requires app version update
```

#### Images & Icons
```
Assets:
- App icon (1024x1024)
- Splash screen
- Holiday type icons
- Tab bar icons
- Calendar cell indicators

Formats: PNG, SVG (platform-specific)
```

---

## 8. Non-Functional Requirements

### 8.1 Performance

**Response Times:**
- App launch: < 2 seconds (cold start)
- Calendar navigation: < 100ms
- Lunar calculation: < 10ms (cached: < 1ms)
- Holiday lookup: < 50ms
- Language switch: < 200ms

**Resource Usage:**
- App size: < 30 MB
- Memory: < 100 MB during normal use
- Battery: Minimal impact (no background processing)
- Storage: < 10 MB for cached data

---

### 8.2 Compatibility

**Platforms:**
- iOS: 15.0 or later
- Android: 8.0 (API 26) or later
- iPad: Full tablet support with adaptive layouts
- macOS: Catalyst support (future)

**Screen Sizes:**
- Phone: 320px - 428px width
- Tablet: 768px - 1024px width
- Orientation: Portrait and Landscape

**Languages:**
- Vietnamese (default)
- English
- Extensible for additional languages

---

### 8.3 Accessibility

**Requirements:**
- WCAG 2.1 Level AA compliance (target)
- Screen reader support (VoiceOver, TalkBack)
- Minimum touch target: 44x44 points
- Minimum color contrast: 4.5:1
- Dynamic text size support
- Reduced motion support (future)

**Implementation:**
- Semantic properties on all interactive elements
- Meaningful labels for images
- Logical reading order
- Focus indicators
- High contrast mode support (future)

---

### 8.4 Security

**Data Privacy:**
- No personal data collection (MVP)
- No analytics or tracking (MVP)
- Local storage only (no cloud)
- No third-party SDKs (except platform frameworks)

**App Security:**
- Code signing required
- SSL pinning (if API added)
- Secure storage for preferences
- No sensitive data exposure

---

### 8.5 Reliability

**Availability:**
- 100% offline availability
- No dependency on external services
- No single point of failure

**Error Handling:**
- Graceful degradation
- User-friendly error messages
- Automatic retry for transient errors (future)
- Crash reporting (disabled in MVP)

**Testing:**
- Unit tests for business logic
- Integration tests for services
- UI tests for critical flows
- Manual testing on physical devices

---

## 9. Future Enhancements

### 9.1 Phase 2 Features (Planned)

**Astronomical Features:**
- Moon phase display (new, waxing, full, waning)
- 24 Solar Terms (Tiết Khí)
- Sunrise/sunset times
- Lunar eclipse notifications

**Astrological Features:**
- Daily horoscope by animal sign
- Auspicious/inauspicious activities
- Color of the day
- Lucky numbers

**Advanced Cultural Features:**
- Ancestral commemoration calculator
- Buddhist calendar integration
- Regional holiday variations
- Custom holiday creation

---

### 9.2 Phase 3 Features (Future)

**User Accounts:**
- Optional registration
- Cloud sync of preferences
- Multi-device support

**Personal Events:**
- Create/edit personal events
- Set reminders
- Recurring events
- Event categories

**Social Features:**
- Share calendars with family
- Event invitations
- Family event coordination
- Community calendar (regional)

**Premium Features:**
- Advanced astrology readings
- Personalized horoscopes
- Historical date lookup
- Export calendar data

---

### 9.3 Technical Improvements

**Performance:**
- Pre-calculate common date ranges
- Implement virtual scrolling for large lists
- Optimize image loading
- Background calculation workers

**User Experience:**
- Widgets (home screen, lock screen)
- Shortcuts (Siri, App Shortcuts)
- Handoff support (between devices)
- Watch app companion

**Analytics & Monitoring:**
- Anonymous usage analytics
- Crash reporting
- Performance monitoring
- A/B testing framework

---

## Appendix A: Success Metrics

**MVP Success Criteria:**
- App launches successfully: 100%
- Calculations are accurate: 100%
- Offline functionality: 100%
- User can view holidays: 100%
- Language switch works: 100%

**User Engagement (Post-Launch):**
- Daily Active Users (DAU)
- Monthly Active Users (MAU)
- Session length
- Features used per session
- Retention rate (Day 1, Day 7, Day 30)

**Quality Metrics:**
- Crash-free rate: > 99.5%
- App Store rating: > 4.5 stars
- Response time: < 100ms for interactions
- Memory usage: < 100MB

---

## Appendix B: Glossary

**Lunar Calendar Terms:**
- **Âm Lịch**: Lunar calendar (Vietnamese)
- **Dương Lịch**: Solar/Gregorian calendar
- **Tết**: Vietnamese New Year
- **Mùng 1**: 1st day of lunar month
- **Rằm**: 15th day (full moon)
- **Thiên Can**: Heavenly Stems (10-year cycle)
- **Địa Chi**: Earthly Branches (12-year cycle)
- **Năm Nhuận**: Leap year
- **Tháng Nhuận**: Leap month

**Technical Terms:**
- **MVP**: Minimum Viable Product
- **Offline-First**: Design pattern prioritizing local functionality
- **Guest Mode**: Using app without registration
- **Localization**: Adapting app for different languages/cultures
- **SQLite**: Embedded relational database

---

**Document End**

This specification is technology-agnostic and can be used to implement the app in any framework (Flutter, React Native, Swift/Kotlin, etc.).
