# Post-MVP Development Plan: Phases 2 & 3
## Vietnamese Lunar Calendar Mobile Application

**Version:** 2.0
**Date:** 2025-12-26
**Status:** Post-MVP Planning

---

## Overview

This document outlines the development roadmap for Phases 2 and 3, focusing on authentic Vietnamese/Chinese lunar calendar features that make the app culturally rich and attractive to users who value traditional calendar systems.

### Phase Prioritization Philosophy

**Phase 2** focuses on core astronomical and astrological features that are fundamental to lunar calendar users:
- Sexagenary cycle (Can Chi / 干支)
- Heavenly Stems and Earthly Branches
- Additional lunar calendar systems (Vietnamese, Chinese variations)
- Localization for Vietnamese and Chinese users
- Dynamic backgrounds aligned with zodiac animals

**Phase 3** adds social features like event management, which are useful but not essential to the core lunar calendar experience.

---

## Phase 2: Authentic Lunar Calendar Features

**Goal**: Transform the app from a basic calendar converter into a comprehensive Vietnamese/Chinese lunar calendar with authentic astronomical and astrological information.

**Duration**: 18 weeks (9 sprints × 2 weeks)

---

### Sprint 9: Sexagenary Cycle Foundation (2 weeks)

#### Backend Tasks
- [x] **T041**: Research and implement Sexagenary cycle (Can Chi / 干支) calculation algorithm ✅
- [x] **T042**: Create data models for:
  - [x] 10 Heavenly Stems (Thiên Can / 天干): Giáp, Ất, Bính, Đinh, Mậu, Kỷ, Canh, Tân, Nhâm, Quý ✅
  - [x] 12 Earthly Branches (Địa Chi / 地支): Tý, Sửu, Dần, Mão, Thìn, Tỵ, Ngọ, Mùi, Thân, Dậu, Tuất, Hợi ✅
  - [x] Five elements (Ngũ hành / 五行): Metal, Wood, Water, Fire, Earth ✅
- [x] **T043**: Implement calculation for:
  - [x] Year stem-branch (Năm Can Chi) ✅
  - [x] Month stem-branch (Tháng Can Chi) ✅
  - [x] Day stem-branch (Ngày Can Chi) - Empirically verified with JDN formula ✅
  - [x] Hour stem-branch (Giờ Can Chi) ✅
- [ ] **T044**: Create API endpoints:
  - [ ] `GET /api/calendar/sexagenary/{date}` - Get full sexagenary info for a date
  - [ ] `GET /api/calendar/year-info/{year}` - Get year's zodiac animal and element
- [x] **T045**: Add localization support for stem-branch names (Vietnamese, Chinese, English) ✅
- [ ] **T046**: Write comprehensive unit tests for cycle calculations (Backend)
- [ ] **T047**: API integration testing with mobile app

**Note**: Tasks T041-T043, T045 completed in Core library (offline-first architecture). API endpoints (T044) deferred to future sprint as mobile app operates offline with local calculations.

#### Mobile Tasks
- [x] **T048**: Create SexagenaryService for fetching cycle data ✅
- [x] **T049**: Implement day stem-branch calculation (Ngày Can Chi) ✅
- [x] **T050**: Implement year stem-branch calculation (Năm Can Chi) ✅
- [x] **T051**: Implement month stem-branch calculation (Tháng Can Chi) ✅
- [x] **T052**: Update CalendarViewModel with today's Can Chi ✅
- [x] **T053**: Design UI components for displaying:
  - [x] Daily stem-branch (Ngày Can Chi) on calendar cells
  - [x] Current day's full sexagenary info in header ✅
  - [x] Visual element indicators (五行 symbols or colors) ✅
- [x] **T054**: Add "Today's Information" section showing:
  - [x] Date in Gregorian, Lunar, and Sexagenary formats ✅
  - [x] Zodiac animal for the day ✅
  - [x] Five element association ✅
- [x] **T055**: Add language-specific formatting (Vietnamese, English, Chinese) ✅
- [ ] **T056**: Write unit tests for day stem-branch calculation
- [ ] **T057**: Write unit tests for year stem-branch calculation
- [ ] **T058**: Write unit tests for month stem-branch calculation
- [ ] **T059**: Write integration tests for sexagenary display
- [ ] **T060**: Ensure stem-branch consistency across holiday pages
  - [ ] Update HolidayDetailViewModel to show full year stem-branch (e.g., "Năm Ất Tỵ" instead of "Năm Tỵ")
  - [ ] Update upcoming holidays year display in CalendarViewModel
  - [ ] Update YearHolidaysViewModel to show full stem-branch format
  - [ ] Reuse existing FormatYearStemBranch() helper method
  - [ ] Test consistency across all three pages (Calendar, Holiday Detail, Year Holidays)

#### Deliverables
- ✅ Full sexagenary cycle calculation working for any date
- ✅ Year, month, day, and hour stem-branch displayed on calendar page
- ✅ Users can see traditional Chinese/Vietnamese date representation
- ✅ Language switching works correctly (Vietnamese, English, Chinese)
- ✅ iOS initialization bug fixed - stem-branch displays on app launch
- ⏳ Unit tests for calculation accuracy (T056-T059)
- ⏳ Stem-branch consistency across all holiday pages (T060)

---

### Sprint 10: Zodiac Animals & Year Characteristics (2 weeks)

#### Backend Tasks
- [ ] Create comprehensive database of 12 zodiac animals (12 Con Giáp / 生肖):
  - Rat (Tý/鼠), Ox (Sửu/牛), Tiger (Dần/虎), Rabbit (Mão/兔)
  - Dragon (Thìn/龍), Snake (Tỵ/蛇), Horse (Ngọ/馬), Goat (Mùi/羊)
  - Monkey (Thân/猴), Rooster (Dậu/雞), Dog (Tuất/狗), Pig (Hợi/豬)
- [ ] Add detailed information for each animal:
  - Personality traits and characteristics
  - Lucky numbers, colors, directions
  - Compatible and incompatible signs
  - Famous people born in that year
  - Cultural significance and folklore
- [ ] Create Zodiac entity and repository
- [ ] Implement API endpoints:
  - `GET /api/zodiac/{year}` - Get zodiac info for specific year
  - `GET /api/zodiac/animal/{animalName}` - Get detailed animal info
  - `GET /api/zodiac/compatibility/{animal1}/{animal2}` - Check compatibility
- [ ] Add support for calculating birth year zodiac from age
- [ ] Include element variations (e.g., Metal Rat, Water Tiger, etc.)

#### Mobile Tasks
- [ ] Create ZodiacService for animal data
- [ ] Design zodiac animal icon set (12 beautiful illustrations)
- [ ] Add year zodiac display in calendar header
- [ ] Create "Zodiac Information" page with:
  - Current year's animal and characteristics
  - User's birth year animal (if birth date provided in settings)
  - Personality traits and lucky elements
  - Compatibility checker feature
- [ ] Add zodiac animal indicator on calendar view
- [ ] Implement swipe/tap to view different zodiac animals
- [ ] Add zodiac compatibility calculator (fun feature)
- [ ] Cache zodiac data locally

#### Deliverables
- ✅ 12 zodiac animals with rich cultural information
- ✅ Year characteristics displayed prominently
- ✅ Users can learn about their birth year animal
- ✅ Compatibility checker adds engagement

---

### Sprint 11: Dynamic Backgrounds Based on Zodiac Year (2 weeks)

#### Backend Tasks
- [ ] Create image asset repository structure
- [ ] Implement CDN or cloud storage for background images
- [ ] Create API endpoint for serving optimized backgrounds:
  - `GET /api/assets/background/{year}` - Get background for specific year
  - `GET /api/assets/backgrounds` - List all available backgrounds
- [ ] Optimize images for mobile (multiple resolutions)
- [ ] Add image metadata (artist attribution, themes)

#### Mobile Tasks
- [ ] Commission or source 12 high-quality background images:
  - Each representing one zodiac animal
  - Traditional Vietnamese/Chinese artistic style
  - Optimized for both light and dark modes
  - Ensure text readability with overlay gradients
- [ ] Implement dynamic background system:
  - Automatically change based on current lunar year
  - Smooth transitions when year changes (Tết)
  - Option to manually select backgrounds in settings
- [ ] Add background opacity control (for readability)
- [ ] Implement background caching and preloading
- [ ] Add "Background Gallery" feature:
  - View all 12 zodiac backgrounds
  - Preview backgrounds before applying
  - Save favorites
- [ ] Ensure backgrounds work well with:
  - Calendar grid readability
  - Text contrast (accessibility)
  - Both iOS and Android
  - Different screen sizes (phones, tablets)
- [ ] Add option to disable backgrounds for minimal UI
- [ ] Test memory usage and performance

#### Deliverables
- ✅ 12 beautiful zodiac-themed backgrounds
- ✅ Automatic background change for lunar new year
- ✅ User control over background appearance
- ✅ Optimized performance and accessibility
- ✅ Gallery feature for browsing all backgrounds

---

### Sprint 12: Moon Phases & Lunar Day Information (2 weeks)

#### Backend Tasks
- [ ] Implement accurate moon phase calculation
- [ ] Calculate lunar day number (1st to 30th of lunar month)
- [ ] Add special lunar days information:
  - New Moon (Sóc / 朔)
  - Full Moon (Vọng / 望)
  - First Quarter (Thượng Huyền)
  - Last Quarter (Hạ Huyền)
- [ ] Create lunar day characteristics database:
  - Auspicious days (Ngày Tốt / 吉日)
  - Inauspicious days (Ngày Xấu / 凶日)
  - Traditional activities for each day
- [ ] Implement API endpoints:
  - `GET /api/lunar/moon-phase/{date}`
  - `GET /api/lunar/day-info/{lunarDay}`
- [ ] Add solar term (Tiết Khí / 节气) calculation (24 solar terms)

#### Mobile Tasks
- [ ] Design moon phase icons (8 phases: new, waxing crescent, first quarter, etc.)
- [ ] Display moon phase on calendar:
  - Small moon icon on each day
  - Current moon phase in header
- [ ] Create "Lunar Day Details" page showing:
  - Current lunar day number
  - Moon phase visualization
  - Day characteristics (auspicious/inauspicious)
  - Recommended activities
  - Solar term information if applicable
- [ ] Add moon phase widget/indicator
- [ ] Implement smooth moon phase animations
- [ ] Add educational content about lunar phases
- [ ] Test accuracy against astronomical data

#### Deliverables
- ✅ Accurate moon phase calculation and display
- ✅ Lunar day characteristics and recommendations
- ✅ 24 solar terms integrated
- ✅ Visual moon phase indicators on calendar

---

### Sprint 13: Additional Lunar Calendar Systems (2 weeks)

#### Backend Tasks
- [ ] Research Vietnamese lunar calendar variations:
  - Regional differences (Northern vs Southern Vietnam)
  - Historical calendar systems
  - Vietnam-specific adjustments vs Chinese calendar
- [ ] Implement Vietnamese-specific calculations:
  - Timezone adjustments (UTC+7)
  - Lunar new year determination (Vietnam method)
  - Vietnamese holiday calculations
- [ ] Add support for:
  - Chinese lunar calendar (current default)
  - Vietnamese lunar calendar
  - Option to show differences between systems
- [ ] Create calendar system configuration:
  - User preference storage
  - API to support multiple systems
- [ ] Document differences between calendar systems
- [ ] Add calendar system comparison endpoint:
  - `GET /api/calendar/compare/{date}`

#### Mobile Tasks
- [ ] Add calendar system selector in settings:
  - Chinese Lunar Calendar
  - Vietnamese Lunar Calendar
  - Show differences (educational feature)
- [ ] Update calendar display to respect selected system
- [ ] Add indicator showing which system is active
- [ ] Create "Calendar Systems" education page:
  - Explain differences between systems
  - Historical context
  - Why differences exist
- [ ] Ensure holidays align with selected calendar system
- [ ] Test with users familiar with both systems
- [ ] Add comparison view (optional advanced feature)

#### Deliverables
- ✅ Support for Vietnamese and Chinese lunar calendars
- ✅ Users can select preferred system
- ✅ Accurate calculations for both systems
- ✅ Educational content explaining differences

---

### Sprint 14: Vietnamese & Chinese Localization (2 weeks)

#### Backend Tasks
- [ ] Set up comprehensive localization infrastructure
- [ ] Create resource files for:
  - English (default)
  - Vietnamese (Tiếng Việt)
  - Simplified Chinese (简体中文)
  - Traditional Chinese (繁體中文)
- [ ] Translate all API error messages
- [ ] Translate zodiac descriptions
- [ ] Translate holiday names and descriptions
- [ ] Translate stem-branch names (Can Chi / 干支)
- [ ] Add language preference to user profile
- [ ] Implement content negotiation based on Accept-Language header

#### Mobile Tasks
- [ ] Extract all UI strings to resource files (.resx)
- [ ] Translate all screens to target languages:
  - Calendar view
  - Settings
  - Zodiac information
  - Holiday details
  - Help/About pages
- [ ] Implement language switching in settings
- [ ] Format dates and numbers according to locale:
  - Vietnamese: dd/mm/yyyy
  - Chinese: yyyy年mm月dd日
  - English: mm/dd/yyyy
- [ ] Add cultural-appropriate formatting:
  - Lunar dates in Vietnamese/Chinese format
  - Zodiac names in native languages
- [ ] Ensure layouts accommodate longer text (especially Vietnamese)
- [ ] Test all screens in all languages
- [ ] Add language-specific fonts if needed:
  - Vietnamese diacritics
  - Chinese characters (both simplified and traditional)
- [ ] Implement font scaling for accessibility
- [ ] Test right-to-left compatibility (future Arabic support)

#### Deliverables
- ✅ Full support for Vietnamese, English, and Chinese (simplified & traditional)
- ✅ Users can switch languages in-app
- ✅ All content professionally translated
- ✅ Cultural-appropriate formatting
- ✅ Layouts work with all languages

---

### Sprint 15: Auspicious Dates & Chinese Almanac (2 weeks)

#### Backend Tasks
- [ ] Research Chinese Almanac (Huangli / 黄历) system:
  - Tung Shing (通胜) principles
  - Vietnamese Lich Van Nien (Lịch Vạn Niên)
- [ ] Implement auspicious date calculation:
  - Good days for weddings (Ngày Tốt Cưới Hỏi)
  - Good days for business opening (Khai Trương)
  - Good days for moving house (Dọn Nhà)
  - Good days for travel (Xuất Hành)
  - Days to avoid for important events
- [ ] Create database of traditional activities and recommendations
- [ ] Implement calculation for:
  - 28 Lunar Mansions (28 Tú / 二十八宿)
  - 12 Day Officers (12 Trực / 十二建除)
  - Conflicting zodiac directions
- [ ] Create API endpoints:
  - `GET /api/almanac/{date}` - Get almanac info for specific date
  - `GET /api/almanac/find-auspicious` - Find good dates for specific activity
- [ ] Add traditional time systems (12 double-hours / Thập Nhị Thời)

#### Mobile Tasks
- [ ] Create "Auspicious Dates" feature:
  - Calendar marking showing good/bad days with color coding
  - Green for auspicious, red for inauspicious, yellow for neutral
- [ ] Design "Daily Almanac" page showing:
  - Today's luck rating
  - Recommended activities (宜 / Nên làm)
  - Activities to avoid (忌 / Kiêng)
  - Zodiac conflicts for the day
  - Lucky directions
  - Lucky colors and numbers
- [ ] Add "Find Good Day" tool:
  - Select activity type (wedding, business, travel, etc.)
  - Date range selection
  - Show ranked list of auspicious dates
- [ ] Implement visual indicators on calendar cells:
  - Stars or icons for very auspicious days
  - Warning icons for inauspicious days
- [ ] Add almanac notifications (optional):
  - Daily luck summary notification
  - Remind about upcoming auspicious days
- [ ] Create educational section explaining almanac system
- [ ] Test accuracy with traditional printed almanacs

#### Deliverables
- ✅ Chinese almanac (Huangli) integrated into calendar
- ✅ Auspicious date finder tool
- ✅ Daily recommendations and warnings
- ✅ Color-coded calendar showing luck ratings
- ✅ Cultural authenticity validated

---

### Sprint 16: Hour-based Zodiac & Time Selection (2 weeks)

#### Backend Tasks
- [ ] Implement hour-based zodiac calculation (Giờ Hoàng Đạo / 时辰)
- [ ] Calculate 12 traditional double-hours:
  - Tý (23:00-01:00), Sửu (01:00-03:00), Dần (03:00-05:00)
  - Mão (05:00-07:00), Thìn (07:00-09:00), Tỵ (09:00-11:00)
  - Ngọ (11:00-13:00), Mùi (13:00-15:00), Thân (15:00-17:00)
  - Dậu (17:00-19:00), Tuất (19:00-21:00), Hợi (21:00-23:00)
- [ ] Determine auspicious hours for each day
- [ ] Implement "Golden Hours" (Giờ Hoàng Đạo) calculation
- [ ] Create API endpoint:
  - `GET /api/almanac/hours/{date}` - Get hour-by-hour luck rating
- [ ] Add hour-specific activities and recommendations

#### Mobile Tasks
- [ ] Create "Auspicious Hours" view:
  - Timeline showing 12 double-hours
  - Color-coded luck rating for each hour
  - Current hour highlighted
- [ ] Add "Best Time Finder" tool:
  - Select desired date
  - View ranked hours (best to worst)
  - Tap hour to see detailed recommendations
- [ ] Implement hour selection for important activities:
  - Wedding ceremony start time
  - Business opening time
  - Meeting scheduling helper
- [ ] Add current hour's luck in app header or widget
- [ ] Create notifications for entering auspicious hours (optional)
- [ ] Add educational content about hour-based fortune
- [ ] Design beautiful timeline visualization

#### Deliverables
- ✅ Hour-based zodiac and auspicious time calculation
- ✅ Daily timeline showing luck by hour
- ✅ Best time finder for important activities
- ✅ Current hour luck indicator

---

### Sprint 17: Solar Terms & Agricultural Calendar (2 weeks)

#### Backend Tasks
- [ ] Implement complete 24 Solar Terms (24 Tiết Khí / 二十四节气):
  - **Spring**: Lập Xuân, Vũ Thủy, Kinh Trập, Xuân Phân, Thanh Minh, Cốc Vũ
  - **Summer**: Lập Hạ, Tiểu Mãn, Mang Chủng, Hạ Chí, Tiểu Thử, Đại Thử
  - **Autumn**: Lập Thu, Xử Thử, Bạch Lộ, Thu Phân, Hàn Lộ, Sương Giáng
  - **Winter**: Lập Đông, Tiểu Tuyết, Đại Tuyết, Đông Chí, Tiểu Hàn, Đại Hàn
- [ ] Calculate exact solar term dates for any year
- [ ] Create database of agricultural activities for each solar term
- [ ] Add traditional Vietnamese farming calendar integration
- [ ] Implement seasonal health and dietary recommendations
- [ ] Create API endpoints:
  - `GET /api/solar-terms/{year}` - Get all 24 solar terms for year
  - `GET /api/solar-terms/current` - Get current solar term info
  - `GET /api/solar-terms/next` - Get next upcoming solar term

#### Mobile Tasks
- [ ] Display solar term indicators on calendar:
  - Mark days when solar terms occur
  - Special styling for major terms (solstices, equinoxes)
- [ ] Create "Solar Terms" information page:
  - List all 24 terms with dates
  - Current term highlighted
  - Countdown to next term
- [ ] Add solar term detail view:
  - Seasonal meaning and significance
  - Traditional agricultural activities
  - Health and dietary recommendations
  - Weather expectations
  - Cultural practices and customs
- [ ] Implement notifications for solar term transitions (optional)
- [ ] Add seasonal background variations (aligned with solar terms)
- [ ] Create agricultural calendar view (for farming communities)
- [ ] Add educational content about solar term system

#### Deliverables
- ✅ Complete 24 solar terms calculation and display
- ✅ Agricultural calendar integration
- ✅ Seasonal recommendations
- ✅ Educational content about traditional farming wisdom

---

### Sprint 18: UI Polish & Cultural Refinement (2 weeks)

#### Backend Tasks
- [ ] Optimize API performance for all new features
- [ ] Add comprehensive caching for:
  - Sexagenary cycle data
  - Zodiac information
  - Solar terms
  - Almanac calculations
- [ ] Performance testing and optimization
- [ ] API response time improvements
- [ ] Database query optimization

#### Mobile Tasks
- [ ] Comprehensive UI/UX review and polish:
  - Consistent styling across all new features
  - Smooth animations and transitions
  - Loading states and skeleton screens
  - Error handling and user feedback
- [ ] Implement dark mode support for all new screens:
  - Zodiac backgrounds work in dark mode
  - Readable text with proper contrast
  - Cultural images adapt to theme
- [ ] Add haptic feedback for iOS
- [ ] Optimize app performance:
  - Reduce memory usage
  - Improve scroll performance
  - Optimize image loading
  - Reduce app size
- [ ] Accessibility improvements:
  - Screen reader support for all features
  - Voice-over descriptions for zodiac images
  - Adjustable text sizes
  - Color blind friendly color schemes
- [ ] Add comprehensive onboarding for new features:
  - Tour of sexagenary cycle
  - Explanation of auspicious dates
  - How to use almanac features
- [ ] Implement advanced settings:
  - Customize which features to show
  - Toggle background images
  - Notification preferences
  - Calendar display density

#### Deliverables
- ✅ Polished, cohesive user experience
- ✅ Dark mode support
- ✅ Excellent performance
- ✅ Accessibility compliant
- ✅ Comprehensive onboarding

---

## Phase 2 Summary

At the end of Phase 2, the app will be a **comprehensive, culturally authentic Vietnamese/Chinese lunar calendar** with:

✅ **Astronomical Features:**
- Complete sexagenary cycle (Can Chi / 干支)
- 12 zodiac animals with rich information
- Moon phases and lunar day characteristics
- 24 solar terms and agricultural calendar

✅ **Astrological Features:**
- Chinese almanac (Huangli / Lịch Vạn Niên)
- Auspicious date finder
- Hour-based luck ratings
- Daily recommendations and warnings

✅ **Cultural Features:**
- Dynamic zodiac backgrounds
- Vietnamese and Chinese calendar systems
- Full localization (Vietnamese, Chinese, English)
- Traditional farming wisdom

✅ **User Experience:**
- Beautiful, culturally appropriate design
- Dark mode support
- Excellent performance
- Accessibility features

---

## Phase 3: Social Features & Event Management

**Goal**: Add personal event management and social features to complement the rich cultural calendar foundation.

**Duration**: 16 weeks (8 sprints × 2 weeks)

---

### Sprint 19: Event Management - Create & View (2 weeks)

#### Backend Tasks
- [ ] Implement Event entity and repository
- [ ] Implement Category entity and repository
- [ ] Create event service with business logic
- [ ] Create events endpoints:
  - `POST /api/events` (create event)
  - `GET /api/events/{id}` (get single event)
  - `GET /api/events/range` (get events in date range)
  - `GET /api/events` (get all user events with pagination)
- [ ] Implement event validation (dates, required fields)
- [ ] Add authorization checks (users can only access own events)
- [ ] Write unit and integration tests
- [ ] Configure AutoMapper for DTOs
- [ ] Implement both Gregorian and Lunar date support for events

#### Mobile Tasks
- [ ] Add "Add Event" button to calendar view
- [ ] Implement guest mode check before event creation
- [ ] Show upgrade prompt when guest taps "Add Event"
- [ ] Create event creation form UI (for authenticated users)
- [ ] Create EventDetailViewModel
- [ ] Create CreateEventViewModel with validation
- [ ] Implement event form fields:
  - Title (required)
  - Description
  - Start date/time picker (Gregorian or Lunar)
  - End date/time picker
  - All-day toggle
  - Calendar type selector (Gregorian/Lunar/Both)
  - Color picker
  - Integration with auspicious hours (suggest best time)
- [ ] Create EventService for API communication
- [ ] Display events on calendar dates (visual indicators)
- [ ] Ensure events don't conflict with holiday/zodiac styling
- [ ] Create event detail page
- [ ] Cache events locally in SQLite for authenticated users
- [ ] Test upgrade prompt flow from guest to authenticated

#### Deliverables
- ✅ Authenticated users can create events
- ✅ Events support both Gregorian and Lunar dates
- ✅ Integration with auspicious time recommendations
- ✅ Events displayed alongside cultural information
- ✅ Guest users prompted to upgrade

---

### Sprint 20: Event Management - Edit, Delete & Categories (2 weeks)

#### Backend Tasks
- [ ] Create update event endpoint (`PUT /api/events/{id}`)
- [ ] Create delete event endpoint (`DELETE /api/events/{id}`)
- [ ] Implement soft delete for events (IsDeleted flag)
- [ ] Create category endpoints:
  - `GET /api/categories` (get all user categories)
  - `POST /api/categories` (create category)
  - `PUT /api/categories/{id}` (update category)
  - `DELETE /api/categories/{id}` (delete category)
- [ ] Implement default categories on user registration:
  - Personal, Work, Family, Health, etc.
- [ ] Add category filtering to events endpoint
- [ ] Write tests for all operations

#### Mobile Tasks
- [ ] Add edit button to event detail page
- [ ] Create edit event form (reuse create form)
- [ ] Implement EditEventViewModel
- [ ] Add delete button with confirmation dialog
- [ ] Update local cache on edit/delete
- [ ] Create category management UI
- [ ] Add category selector to event form
- [ ] Display event colors based on category
- [ ] Ensure event colors don't conflict with:
  - Holiday colors (red, gold, green)
  - Auspicious date indicators
  - Zodiac styling
- [ ] Add filter to show/hide event categories
- [ ] Test all operations offline and online

#### Deliverables
- ✅ Users can edit and delete events
- ✅ Category system for organizing events
- ✅ Color coordination with cultural features
- ✅ Offline support for event management

---

### Sprint 21: Search, Filtering & Event Intelligence (2 weeks)

#### Backend Tasks
- [ ] Create event search endpoint with full-text search
- [ ] Add holiday search capability
- [ ] Implement advanced filtering:
  - Date range
  - Category
  - Calendar type (Gregorian/Lunar)
  - Holidays vs personal events
  - Auspicious vs inauspicious dates
- [ ] Implement intelligent event suggestions:
  - Warn if scheduling on inauspicious day
  - Suggest alternative auspicious dates nearby
  - Recommend auspicious hours for events
- [ ] Optimize search queries with database indexing
- [ ] Add search result pagination

#### Mobile Tasks
- [ ] Create unified search UI:
  - Search events, holidays, zodiac info, solar terms
  - Filter options in bottom sheet
  - Real-time search with debouncing
- [ ] Implement SearchViewModel
- [ ] Add "Smart Scheduling" feature:
  - When creating event, show almanac rating for selected date
  - Suggest alternative dates if inauspicious
  - Highlight recommended hours
  - Show zodiac compatibility if multiple people involved
- [ ] Display search results with context:
  - Events: show date + category + almanac rating
  - Holidays: show type + description
  - Solar terms: show date + seasonal info
- [ ] Add search history
- [ ] Test search performance with large datasets

#### Deliverables
- ✅ Comprehensive search across all content
- ✅ Smart scheduling with almanac integration
- ✅ Date recommendations for important events
- ✅ Fast, responsive search experience

---

### Sprint 22: Event Reminders & Notifications (2 weeks)

#### Backend Tasks
- [ ] Implement push notification service integration:
  - Configure APNs for iOS
  - Configure FCM for Android
- [ ] Create notification scheduling service
- [ ] Create endpoints for device token registration
- [ ] Implement reminder notification logic
- [ ] Add background job for sending reminders
- [ ] Include cultural context in notifications:
  - "Your wedding tomorrow is on an auspicious day (Ngày Tốt)"
  - "Meeting at 2pm is during lucky hours (Giờ Hoàng Đạo)"
- [ ] Test notification delivery

#### Mobile Tasks
- [ ] Request notification permissions
- [ ] Register device token with backend
- [ ] Add reminder time picker to event form:
  - 15 min, 30 min, 1 hour, 1 day, 1 week before
  - Custom time selection
- [ ] Implement local notifications as fallback
- [ ] Handle notification taps (deep linking to event)
- [ ] Add notification settings page:
  - Daily almanac notifications
  - Event reminders
  - Solar term transitions
  - Auspicious day alerts
- [ ] Design culturally appropriate notification content:
  - Include zodiac context
  - Mention almanac ratings
  - Show lunar date
- [ ] Test notifications on iOS and Android

#### Deliverables
- ✅ Event reminders with cultural context
- ✅ Daily almanac notifications (optional)
- ✅ Solar term transition alerts
- ✅ Customizable notification preferences

---

### Sprint 23: Recurring Events (2 weeks)

#### Backend Tasks
- [ ] Design recurrence rule schema (iCalendar RRULE format)
- [ ] Implement recurrence calculation service
- [ ] Update event endpoints to support recurrence
- [ ] Handle both Gregorian and Lunar recurring events:
  - Lunar birthday (same lunar date each year)
  - Annual lunar holidays
  - Monthly lunar observances (1st, 15th of lunar month)
- [ ] Create endpoint to get single instance or series
- [ ] Implement logic for editing single vs. all instances
- [ ] Write tests for various recurrence patterns

#### Mobile Tasks
- [ ] Design recurring event UI
- [ ] Add recurrence pattern selector:
  - Daily, Weekly, Monthly (Gregorian)
  - Lunar Monthly (1st, 15th, or custom lunar day)
  - Lunar Annual (e.g., birthday on same lunar date)
  - Custom patterns
- [ ] Add recurrence end options:
  - Never
  - After N times
  - Until specific date
- [ ] Update event detail page to show recurrence info
- [ ] Implement edit options:
  - This event only
  - All events in series
  - This and future events
- [ ] Display recurring events correctly on calendar
- [ ] Add special handling for lunar recurring events:
  - Show both Gregorian and Lunar dates
  - Explain date variation (lunar dates shift in Gregorian calendar)
- [ ] Test various recurrence scenarios

#### Deliverables
- ✅ Support for recurring events (Gregorian and Lunar)
- ✅ Lunar birthday and annual observances
- ✅ Monthly lunar events (1st, 15th)
- ✅ Flexible editing options

---

### Sprint 24: Calendar Sharing & Family Features (2 weeks)

#### Backend Tasks
- [ ] Design shared calendar schema
- [ ] Implement calendar sharing service
- [ ] Create endpoints for inviting users:
  - `POST /api/calendars/share` (invite via email or share code)
  - `GET /api/calendars/shared` (get shared calendars)
  - `DELETE /api/calendars/share/{id}` (revoke sharing)
- [ ] Implement permissions (view-only, edit)
- [ ] Add shared events to event queries
- [ ] Implement notification for shared event updates
- [ ] Add family zodiac compatibility features:
  - Calculate compatibility between family members
  - Find auspicious dates good for all members

#### Mobile Tasks
- [ ] Create calendar sharing UI:
  - Share with specific users (email)
  - Generate share link/code
  - View who has access
  - Manage permissions
- [ ] Implement invite flow
- [ ] Display shared calendars separately or merged
- [ ] Show event ownership indicators
- [ ] Add "Family Calendar" feature:
  - Add family members with birth dates
  - View zodiac compatibility
  - Find dates auspicious for whole family
  - See conflicting zodiac directions for each member
- [ ] Create family compatibility report:
  - Best days for family gatherings
  - Compatible zodiac pairs
  - Harmonious arrangements
- [ ] Test collaborative scenarios

#### Deliverables
- ✅ Calendar sharing with family/friends
- ✅ Shared events sync across participants
- ✅ Family zodiac compatibility features
- ✅ Auspicious date finder for families

---

### Sprint 25: Export, Backup & Data Portability (2 weeks)

#### Backend Tasks
- [ ] Implement event export endpoint:
  - iCal format (for Google/Apple Calendar import)
  - CSV format
  - JSON format
- [ ] Create backup/restore functionality:
  - Full account backup
  - Events only backup
  - Settings and preferences backup
- [ ] Implement data export for privacy compliance (GDPR)
- [ ] Add import functionality:
  - Import from iCal files
  - Import from other calendar apps
- [ ] Create account deletion with data purge

#### Mobile Tasks
- [ ] Add export functionality:
  - Export single event (share as iCal)
  - Export month/year of events
  - Export all events
  - Share event as text or image (with lunar info)
- [ ] Create backup/restore UI:
  - Automatic cloud backup (if authenticated)
  - Manual backup to device
  - Restore from backup
- [ ] Implement import wizard:
  - Import iCal files
  - Map imported events to categories
  - Preview before importing
- [ ] Add event sharing features:
  - Share event as beautiful image card:
    - Include Gregorian and Lunar dates
    - Show zodiac context
    - Display almanac rating
    - Add zodiac background
  - Share to social media
  - Share via messaging apps
- [ ] Implement account deletion flow
- [ ] Add privacy settings and data management
- [ ] Test import/export with various calendar apps

#### Deliverables
- ✅ Export events to standard formats
- ✅ Backup and restore functionality
- ✅ Beautiful event sharing images
- ✅ Import from other calendar apps
- ✅ GDPR-compliant data portability

---

### Sprint 26: Home Screen Widgets & Quick Actions (2 weeks)

#### Backend Tasks
- [ ] Create optimized widget data endpoint:
  - Today's lunar date
  - Current zodiac info
  - Upcoming events
  - Almanac rating
  - Next solar term
- [ ] Implement caching for widget data
- [ ] Ensure API performance for frequent widget refreshes
- [ ] Add widget-specific data formatting

#### Mobile Tasks
- [ ] Design widget layouts for iOS:
  - **Small**: Today's lunar date + zodiac + almanac rating
  - **Medium**: Above + next 3 events + moon phase
  - **Large**: Full daily info + events + auspicious hours
- [ ] Design widget layouts for Android:
  - Similar functionality to iOS widgets
  - Material Design 3 styling
- [ ] Implement iOS widgets using WidgetKit:
  - Interactive widgets (iOS 17+)
  - Widget configuration options
  - Deep linking to app
- [ ] Implement Android widgets using Glance:
  - Widget customization
  - Tap actions
- [ ] Add widget customization options:
  - Show/hide elements
  - Color theme selection
  - Zodiac background toggle
- [ ] Implement iOS Quick Actions (3D Touch/Haptic Touch):
  - Create Event
  - View Today's Almanac
  - Find Auspicious Date
- [ ] Add Android App Shortcuts:
  - Same functionality as iOS Quick Actions
- [ ] Handle widget tap to open app at correct location
- [ ] Test widget updates and refresh timing
- [ ] Optimize widget performance and battery usage

#### Deliverables
- ✅ Home screen widgets (iOS and Android)
- ✅ Widgets display lunar date, events, almanac info
- ✅ Interactive widgets with customization
- ✅ Quick actions for common tasks
- ✅ Optimized battery usage

---

### Sprint 27: Performance, Analytics & Final Polish (2 weeks)

#### Backend Tasks
- [ ] Implement comprehensive analytics:
  - Track feature usage
  - Monitor API performance
  - Identify popular features
- [ ] Set up APM (Application Performance Monitoring)
- [ ] Optimize database queries:
  - Add indexes for common queries
  - Optimize almanac calculations
  - Cache frequently accessed data
- [ ] Implement Redis caching layer for:
  - Sexagenary calculations
  - Zodiac information
  - Solar terms
  - Almanac data
- [ ] Load testing with realistic scenarios:
  - Tet holiday peak usage
  - Multiple concurrent users
  - Widget refresh bursts
- [ ] Security audit for new features
- [ ] API rate limiting refinement

#### Mobile Tasks
- [ ] Implement app analytics:
  - Firebase Analytics or AppCenter
  - Track feature adoption:
    - How many users check auspicious dates?
    - Which zodiac features are most popular?
    - Calendar system preference distribution
  - User engagement metrics
- [ ] Performance optimization:
  - Optimize image loading and caching
  - Reduce app size (ProGuard, app bundles)
  - Optimize battery usage
  - Reduce memory footprint
  - Improve startup time
- [ ] Implement crash reporting and monitoring
- [ ] Final UI/UX polish:
  - Animation smoothness
  - Transition improvements
  - Loading state refinement
  - Error message improvements
- [ ] Accessibility audit:
  - Screen reader support for all features
  - Color contrast validation
  - Font scaling support
  - VoiceOver/TalkBack optimization
- [ ] Localization quality assurance:
  - Native speaker review
  - Cultural appropriateness check
  - Fix any translation issues
- [ ] Comprehensive testing:
  - End-to-end user flows
  - Edge cases
  - Different device sizes
  - OS version compatibility

#### Deliverables
- ✅ Analytics and monitoring in place
- ✅ Optimized performance (API and mobile)
- ✅ Crash reporting active
- ✅ Accessibility compliant
- ✅ Polished user experience

---

## Phase 3 Summary

At the end of Phase 3, the app will have:

✅ **Personal Event Management:**
- Create, edit, delete events
- Gregorian and Lunar date support
- Categories and organization
- Search and filtering

✅ **Smart Features:**
- Almanac-integrated scheduling
- Auspicious date recommendations
- Hour-based suggestions
- Zodiac compatibility checking

✅ **Social Features:**
- Calendar sharing
- Family compatibility
- Collaborative event planning
- Event export and sharing

✅ **Convenience:**
- Reminders and notifications
- Recurring events (Gregorian and Lunar)
- Home screen widgets
- Quick actions

✅ **Data Management:**
- Backup and restore
- Import/export (iCal, CSV, JSON)
- Beautiful event sharing
- Privacy controls

---

## Post-Phase 3: Future Enhancements

### Potential Phase 4 Features

**Advanced Astrology:**
- Birth chart (Tứ Trụ / 四柱 - Four Pillars of Destiny)
- Feng Shui direction calculator
- Personal lucky elements and colors
- Yearly fortune predictions (Tử Vi Hàng Năm)

**Platform Expansion:**
- iPad and tablet optimization
- Apple Watch app (quick almanac check)
- Android Wear app
- Web application (PWA)
- Desktop apps (macOS, Windows)

**Premium Features:**
- Professional astrology consultations (in-app booking)
- Detailed compatibility reports
- Advanced Feng Shui tools
- Personalized fortune readings
- Ad-free experience

**Integrations:**
- Google Calendar sync
- Apple Calendar sync
- Microsoft Outlook sync
- Wedding planning platforms
- Event venue booking services

**AI & Machine Learning:**
- Personalized event suggestions
- Smart date recommendations based on user patterns
- Voice input for event creation
- Natural language date parsing (e.g., "next auspicious day for moving")

**Community Features:**
- Share almanac interpretations
- Community-submitted auspicious activities
- Regional custom and traditions
- Expert Q&A forum

---

## Success Metrics

### Phase 2 Success Criteria
- All astronomical features working accurately
- Calendar system switching seamless
- Backgrounds enhance (not hinder) UX
- User retention > 50% after 30 days
- Average session time > 5 minutes
- 4.7+ star rating on app stores
- Cultural authenticity validated by native users

### Phase 3 Success Criteria
- Event management adoption > 60% of active users
- Calendar sharing used by > 30% of users
- Widget installation > 40% of iOS/Android users
- Almanac-based scheduling used > 50% of event creations
- App store rating maintained at 4.7+
- User retention > 60% after 30 days
- Monthly active users growing 15%+ month-over-month

---

## Risk Mitigation

### Cultural Risks
- **Risk**: Inaccurate cultural information offends users
- **Mitigation**: Consult with Vietnamese/Chinese cultural experts, beta test with native users

### Technical Risks
- **Risk**: Almanac calculations are complex and may have bugs
- **Mitigation**: Validate against traditional printed almanacs, extensive testing

### UX Risks
- **Risk**: Too many features overwhelm users
- **Mitigation**: Progressive disclosure, excellent onboarding, optional features

### Performance Risks
- **Risk**: Zodiac backgrounds slow down app
- **Mitigation**: Image optimization, lazy loading, option to disable

---

## Development Approach

### Agile Methodology
- 2-week sprints
- Daily standups
- Sprint retrospectives
- Continuous user feedback integration

### Testing Strategy
- Unit tests for all calculations (sexagenary, solar terms, almanac)
- Integration tests for API endpoints
- UI tests for critical user flows
- Cultural accuracy testing with native speakers
- Performance testing on low-end devices

### Quality Assurance
- Code reviews for all pull requests
- Cultural consultants review all content
- Beta testing with Vietnamese/Chinese user groups
- Accessibility audits each sprint
- Performance monitoring and optimization

---

**Document Version:** 2.0
**Last Updated:** 2025-12-26
**Status:** Ready for implementation

---

## Next Steps

1. **Review this plan** with stakeholders
2. **Prioritize features** if timeline needs compression
3. **Allocate resources** for cultural consultants
4. **Source or commission** 12 zodiac background images
5. **Begin Sprint 9** - Sexagenary Cycle Foundation

This roadmap transforms the Vietnamese Lunar Calendar app from a basic converter into a comprehensive cultural and astrological tool that respects and celebrates Vietnamese and Chinese traditions while providing modern, user-friendly features.
