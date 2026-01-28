# Sprint 9 Testing Checklist - Sexagenary Cycle

**Testing Date**: January 27, 2026  
**Branch**: `feature/001-sexagenary-cycle-complete`  
**Tester**: _____________

---

## 📱 Platform Testing

### iOS (iPad Simulator) ✅
- [ ] App launches successfully
- [ ] No crashes on startup
- [ ] Calendar page loads completely

### Android (Emulator) ⏳
- [ ] App launches successfully
- [ ] No crashes on startup
- [ ] Calendar page loads completely

---

## 🧪 Feature Testing

### 1. Calendar Page - Today's Stem-Branch Display

**Location**: Calendar header/today section

#### Test Cases:
- [ ] **TC001**: Today's date shows correct stem-branch
  - Expected: Can Chi (干支) displayed in selected language
  - Verify: Matches manual calculation or reference source
  
- [ ] **TC002**: Element color indicator displayed
  - Expected: Colored circle/indicator showing element
  - Colors: Wood=Green, Fire=Red, Earth=Yellow, Metal=White, Water=Blue
  
- [ ] **TC003**: Language switching works
  - [ ] Vietnamese: "Giáp Tý" format
  - [ ] English: "Jia Zi" format
  - [ ] Chinese: "甲子" format

---

### 2. Calendar Page - Date Cell Tap Navigation

**Feature**: Tap any date to view details

#### Test Cases:
- [ ] **TC004**: Tap on today's date
  - Expected: Navigates to DateDetailPage
  - Verify: Haptic feedback occurs
  
- [ ] **TC005**: Tap on past date
  - Expected: Navigates to DateDetailPage with past date info
  
- [ ] **TC006**: Tap on future date
  - Expected: Navigates to DateDetailPage with future date info
  
- [ ] **TC007**: Tap on lunar holiday date
  - Expected: DateDetailPage shows holiday card
  
- [ ] **TC008**: Multiple rapid taps
  - Expected: No duplicate navigation, no crash

---

### 3. DateDetailPage - Comprehensive Display

**Feature**: Detailed date information page

#### Section 1: Gregorian Date Header
- [ ] **TC009**: Date displays correctly
  - Format: "Friday, January 27, 2026"
  - Verify: Day of week matches calendar
  
- [ ] **TC010**: Large, readable text
  - Font size appropriate for quick scanning

#### Section 2: Lunar Date Card
- [ ] **TC011**: Lunar date displays correctly
  - Format: "Day 9, Month 12, Year Ất Tỵ"
  - Verify: Matches Chinese calendar conversion
  
- [ ] **TC012**: Leap month indicator (if applicable)
  - Test with leap month dates in 2025/2026
  - Expected: "Tháng nhuận" indicator shown

#### Section 3: Stem-Branch Card
- [ ] **TC013**: Day stem-branch correct
  - Expected: Matches SexagenaryCalculator output
  - Verify: Element color circle displayed
  
- [ ] **TC014**: Month stem-branch correct
  - Expected: Based on year stem + month number
  
- [ ] **TC015**: Year stem-branch correct
  - Expected: Matches current lunar year
  - Verify: Zodiac animal icon/name shown
  
- [ ] **TC016**: Element information
  - [ ] Element name displayed (Wood/Fire/Earth/Metal/Water)
  - [ ] Element description in selected language
  - [ ] Color indicator matches element
  
- [ ] **TC017**: Multi-language stem-branch
  - [ ] Vietnamese: "Giáp" format
  - [ ] English: "Jia" pinyin format
  - [ ] Chinese: "甲" character format

#### Section 4: Holiday Card (Conditional)
- [ ] **TC018**: Holiday card hidden for regular dates
  - Expected: Card not visible when no holiday
  
- [ ] **TC019**: Holiday card shown for holidays
  - Test dates: 
    - Lunar New Year Eve (2026-02-16)
    - Lunar New Year Day 1 (2026-02-17)
    - Mid-Autumn Festival
  - Expected: Holiday name and description displayed
  
- [ ] **TC020**: Holiday color coding
  - Major holidays: Red accent
  - Traditional festivals: Orange accent
  - Other celebrations: Blue accent

---

### 4. Navigation & UX

#### Back Navigation
- [ ] **TC021**: Back button returns to calendar
  - Expected: Smooth transition, no data loss
  
- [ ] **TC022**: Swipe back gesture (iOS)
  - Expected: Works correctly, no visual glitches

#### Page Initialization
- [ ] **TC023**: Page loads within 200ms
  - Use stopwatch or dev tools
  - Expected: All data visible within 200ms
  
- [ ] **TC024**: Parallel data loading
  - No visible delay between sections
  - All cards populate simultaneously

#### Responsive Design
- [ ] **TC025**: Portrait orientation
  - All cards visible without scrolling issues
  
- [ ] **TC026**: Landscape orientation (tablet)
  - Layout adapts appropriately
  
- [ ] **TC027**: Different screen sizes
  - [ ] iPhone SE (small)
  - [ ] iPhone 16 Pro (medium)
  - [ ] iPad Pro (large)

---

### 5. Historical Accuracy Validation

**Critical**: Verify against authoritative sources

#### Test Dates (Use online Vietnamese calendar tools):
- [ ] **TC028**: 2026-01-27 (Today)
  - Day: _____________
  - Month: _____________
  - Year: Ất Tỵ (乙巳)
  
- [ ] **TC029**: 2026-02-17 (Lunar New Year 2026)
  - Day: _____________
  - Month: First Month Day 1
  - Year: Bính Ngọ (丙午)
  - Should transition to new year stem-branch
  
- [ ] **TC030**: 2025-01-29 (Lunar New Year 2025)
  - Day: _____________
  - Year: Ất Tỵ (乙巳)
  
- [ ] **TC031**: 1984-02-02 (Known reference date)
  - Day: Giáp Tý (甲子) - Start of 60-cycle
  - Verify: Should be exactly 甲子

#### Reference Sources:
- [Vietnamese Calendar Online](https://www.informatik.uni-leipzig.de/~duc/amlich/)
- Traditional almanac books
- Chinese calendar conversion tools

---

### 6. Performance Testing

#### Calculation Speed
- [ ] **TC032**: Single date calculation
  - Open DateDetailPage for any date
  - Expected: < 10ms (imperceptible)
  
- [ ] **TC033**: Navigate between 10 dates rapidly
  - Expected: No lag, smooth transitions

#### Memory Usage
- [ ] **TC034**: Navigate to 20+ different dates
  - Expected: No memory warnings
  - Check: Caching works (fast subsequent loads)
  
- [ ] **TC035**: Leave app in background, resume
  - Expected: No crash, data retained

---

### 7. Edge Cases

#### Boundary Dates
- [ ] **TC036**: December 31, 2025
  - Gregorian year end
  - Lunar year mid-cycle
  
- [ ] **TC037**: January 1, 2026
  - Gregorian year start
  - Lunar year still Ất Tỵ
  
- [ ] **TC038**: February 16-17, 2026
  - Lunar New Year transition
  - Year stem-branch should change

#### Invalid Scenarios
- [ ] **TC039**: Airplane mode / no internet
  - Expected: Works offline (all local calculations)
  
- [ ] **TC040**: Date picker extremes
  - Test year 1900
  - Test year 2100
  - Expected: No crash, reasonable behavior

---

### 8. Localization Testing

#### Vietnamese (Tiếng Việt)
- [ ] **TC041**: All text in Vietnamese
  - Stem-branch names: Giáp, Ất, Bính...
  - Element names: Mộc, Hỏa, Thổ...
  - Zodiac: Tý, Sửu, Dần...
  
- [ ] **TC042**: Proper diacritics
  - Ất Tỵ (not At Ti)
  - Diacritics render correctly

#### English
- [ ] **TC043**: Pinyin transliteration
  - Jia, Yi, Bing...
  - Proper capitalization
  
- [ ] **TC044**: Element names in English
  - Wood, Fire, Earth, Metal, Water

#### Chinese (中文)
- [ ] **TC045**: Traditional characters
  - 甲子, 乙丑...
  - Characters render correctly
  - Font supports all characters

---

### 9. Dark Mode / Light Mode

- [ ] **TC046**: Light mode appearance
  - Colors are appropriate
  - Text readable
  - Element indicators visible
  
- [ ] **TC047**: Dark mode appearance
  - Colors invert correctly
  - Text still readable
  - Element indicators visible
  
- [ ] **TC048**: Dynamic switching
  - Change mode while on DateDetailPage
  - Expected: Smooth transition, no glitches

---

### 10. Regression Testing

**Ensure existing features still work**

- [ ] **TC049**: Calendar month navigation
  - Previous/next month buttons
  - Expected: No change from before
  
- [ ] **TC050**: Holiday display in calendar
  - Red dots for holidays
  - Expected: No change from before
  
- [ ] **TC051**: Today button
  - Returns to current date
  - Expected: No change from before
  
- [ ] **TC052**: Settings page
  - Language switching
  - Theme switching
  - Expected: No change from before

---

## 🐛 Bug Tracking

### Bugs Found During Testing

#### Bug #1
- **Severity**: [ ] Critical [ ] High [ ] Medium [ ] Low
- **Description**: _________________________________
- **Steps to Reproduce**: _________________________________
- **Expected**: _________________________________
- **Actual**: _________________________________
- **Screenshot**: _________________________________

#### Bug #2
- **Severity**: [ ] Critical [ ] High [ ] Medium [ ] Low
- **Description**: _________________________________
- **Steps to Reproduce**: _________________________________
- **Expected**: _________________________________
- **Actual**: _________________________________

_(Add more as needed)_

---

## ✅ Final Sign-Off

### Test Summary
- **Total Test Cases**: 52
- **Passed**: _______
- **Failed**: _______
- **Blocked**: _______
- **Pass Rate**: _______%

### Critical Criteria
- [ ] **Zero crashes** during testing
- [ ] **Historical accuracy** validated (100% match)
- [ ] **Performance** meets targets (< 200ms load)
- [ ] **All platforms** tested (iOS + Android)
- [ ] **Multi-language** works correctly
- [ ] **No regressions** in existing features

### Approval
- [ ] **APPROVED** - Ready for production
- [ ] **APPROVED with minor issues** - Document known issues
- [ ] **REJECTED** - Critical bugs must be fixed

**Tester Name**: _______________________  
**Tester Signature**: _______________________  
**Date**: _______________________

---

## 📋 Notes

### Positive Observations
- _________________________________
- _________________________________

### Areas for Improvement
- _________________________________
- _________________________________

### Recommendations for Next Sprint
- _________________________________
- _________________________________

---

**End of Testing Checklist**
