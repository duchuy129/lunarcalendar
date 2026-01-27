# T060 Testing Guide - Holiday Page Consistency

**Date**: January 26, 2026  
**Feature**: Full Stem-Branch Year Display  
**Branch**: `feature/001-sexagenary-cycle`  
**Build Status**: ✅ Compiled Successfully

---

## 🎯 Testing Objective

Verify that all holiday pages consistently display full stem-branch year format (e.g., "Năm Ất Tỵ") instead of just animal sign across all three languages.

---

## 📱 Quick Start - Deploy & Test

### Option 1: iOS Simulator (Recommended for Quick Testing)

```bash
# Deploy to iPhone 16 Pro simulator
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar
dotnet build -t:Run -f net10.0-ios -c Debug
```

**Or use your existing script**:
```bash
./scripts/deploy-ipad-simulator.sh
```

### Option 2: Android Emulator

```bash
# Start emulator first if not running
emulator -avd maui_avd &

# Build and deploy
dotnet build -t:Run -f net10.0-android -c Debug
```

**Or use your existing script**:
```bash
./scripts/deploy-android-sim.sh
```

---

## ✅ Test Cases Checklist

### Test 1: Holiday Detail Page (HolidayDetailViewModel)

**Navigation**: Calendar Page → Tap any lunar holiday (e.g., Tết, Mid-Autumn)

| Test Item | Expected Result | Pass/Fail | Notes |
|-----------|----------------|-----------|-------|
| **Vietnamese** | Shows "Năm Ất Tỵ" | ⬜ | Full stem-branch with prefix |
| **English** | Shows "Year Yi Si (Snake)" | ⬜ | Stem-branch + animal name |
| **Chinese** | Shows "年乙巳" | ⬜ | Chinese characters |
| Non-lunar holiday | No year display | ⬜ | e.g., New Year's Day |
| Language switch | Updates immediately | ⬜ | Change in Settings |

**Screenshots to capture**:
- [ ] Vietnamese format
- [ ] English format  
- [ ] Chinese format

---

### Test 2: Upcoming Holidays List (CalendarViewModel)

**Navigation**: Calendar Page → Scroll to "Upcoming Holidays" section

| Test Item | Expected Result | Pass/Fail | Notes |
|-----------|----------------|-----------|-------|
| **Vietnamese** | Each shows "Năm Ất Tỵ" | ⬜ | In holiday subtitle |
| **English** | Each shows "Year Yi Si (Snake)" | ⬜ | In holiday subtitle |
| **Chinese** | Each shows "年乙巳" | ⬜ | In holiday subtitle |
| Multiple holidays | All consistent | ⬜ | Check 3-4 holidays |
| Language switch | All update together | ⬜ | Dynamic refresh |

**Screenshots to capture**:
- [ ] List view with multiple holidays (Vietnamese)
- [ ] After language switch to English

---

### Test 3: Year Holidays Page (YearHolidaysViewModel)

**Navigation**: Bottom Tab → "Year Holidays" / "Lễ Trong Năm"

| Test Item | Expected Result | Pass/Fail | Notes |
|-----------|----------------|-----------|-------|
| **Vietnamese** | All show "Năm Ất Tỵ" | ⬜ | In each holiday card |
| **English** | All show "Year Yi Si (Snake)" | ⬜ | In each holiday card |
| **Chinese** | All show "年乙巳" | ⬜ | In each holiday card |
| Year navigation | Updates correctly | ⬜ | Try 2025, 2026, 2027 |
| Scroll performance | Smooth, no lag | ⬜ | Test with 20+ holidays |

**Screenshots to capture**:
- [ ] Year 2026 holidays (Vietnamese)
- [ ] Year 2025 holidays (to see different stem-branch)
- [ ] After language switch

---

### Test 4: Language Switching Integration

**Navigation**: Settings → Language → Switch between VI/EN/ZH

| Test Item | Expected Result | Pass/Fail | Notes |
|-----------|----------------|-----------|-------|
| Switch VI → EN | All pages update | ⬜ | Navigate to each page |
| Switch EN → ZH | All pages update | ⬜ | Check consistency |
| Switch ZH → VI | All pages update | ⬜ | Return to original |
| No blank displays | Always shows content | ⬜ | iOS initialization test |
| No crashes | App remains stable | ⬜ | Monitor for errors |

---

### Test 5: Calendar Page (Baseline - Already Working)

**Navigation**: Main Calendar Page → "Today" section

| Test Item | Expected Result | Pass/Fail | Notes |
|-----------|----------------|-----------|-------|
| Today's year | Shows "Năm Ất Tỵ" | ⬜ | Line 1 of Today section |
| Today's day | Shows "Ngày Kỷ Hợi" | ⬜ | Line 2 of Today section |
| Element color dot | Visible (10×10) | ⬜ | Color indicator |
| All languages | Proper formatting | ⬜ | VI/EN/ZH |

**Note**: This was completed in Phase 3, just verify it still works

---

## 🐛 Error Scenarios to Test

### Edge Cases

| Scenario | Expected Behavior | Pass/Fail | Notes |
|----------|------------------|-----------|-------|
| Gregorian holidays | No year stem-branch shown | ⬜ | e.g., Christmas, New Year |
| Year 2027 | Shows "Năm Đinh Mùi" | ⬜ | Different stem-branch |
| Year 2025 | Shows "Năm Giáp Thìn" | ⬜ | Previous year |
| Leap month holidays | Handles gracefully | ⬜ | If any exist |
| Network offline | Still calculates | ⬜ | Offline-first principle |

### Performance Tests

| Test | Target | Actual | Pass/Fail | Notes |
|------|--------|--------|-----------|-------|
| Holiday list load time | < 500ms | ___ms | ⬜ | 20+ holidays |
| Year switch time | < 300ms | ___ms | ⬜ | Year picker |
| Language switch time | < 500ms | ___ms | ⬜ | All pages update |
| Scroll FPS | 60 FPS | ___FPS | ⬜ | Smooth scrolling |
| Memory usage | < 10MB increase | ___MB | ⬜ | Monitor in Xcode |

---

## 📸 Screenshot Checklist

Capture screenshots for documentation:

### Required Screenshots

1. **Holiday Detail - Vietnamese**
   - [ ] Tết (Lunar New Year) showing "Năm Ất Tỵ"
   - [ ] Mid-Autumn Festival showing "Năm Ất Tỵ"

2. **Holiday Detail - English**
   - [ ] Same holiday showing "Year Yi Si (Snake)"

3. **Holiday Detail - Chinese**
   - [ ] Same holiday showing "年乙巳"

4. **Upcoming Holidays List**
   - [ ] List view with 3-4 holidays (Vietnamese)
   - [ ] Same view in English

5. **Year Holidays Page**
   - [ ] 2026 holidays (Vietnamese)
   - [ ] 2025 holidays (showing different year)

6. **Comparison: Before/After**
   - [ ] Old format: "Year of Snake" (if you have old screenshots)
   - [ ] New format: "Năm Ất Tỵ"

### Screenshot Location
Save to: `support_docs/screenshots/t060-testing/`

---

## 🔍 Manual Testing Steps

### Step-by-Step Test Flow

#### 1. Initial Launch Test (Vietnamese)

```
1. Launch app (ensure Vietnamese language)
2. Navigate to Calendar page
   ✓ Verify "Today" shows "Năm Ất Tỵ"
3. Scroll down to "Upcoming Holidays"
   ✓ Verify each holiday shows "Năm Ất Tỵ"
4. Tap on Tết holiday
   ✓ Verify Holiday Detail shows "Năm Ất Tỵ"
5. Go back, navigate to "Year Holidays" tab
   ✓ Verify all holidays show "Năm Ất Tỵ"
```

#### 2. Language Switch Test (English)

```
1. Open Settings
2. Change language to English
3. Return to Calendar
   ✓ Verify all pages now show "Year Yi Si (Snake)"
4. Navigate through all 3 holiday views
   ✓ Verify consistency across all pages
```

#### 3. Language Switch Test (Chinese)

```
1. Open Settings
2. Change language to 中文
3. Return to Calendar
   ✓ Verify all pages now show "年乙巳"
4. Check characters are correct (not garbled)
```

#### 4. Year Navigation Test

```
1. Go to "Year Holidays" page
2. Select Year 2025 from picker
   ✓ Verify shows "Năm Giáp Thìn" (Dragon year)
3. Select Year 2027
   ✓ Verify shows "Năm Đinh Mùi" (Goat year)
4. Select Year 2026 again
   ✓ Verify returns to "Năm Ất Tỵ" (Snake year)
```

#### 5. Performance Test

```
1. Open "Year Holidays" page
2. Scroll rapidly through all holidays
   ✓ Verify smooth scrolling (no lag)
3. Switch years multiple times quickly
   ✓ Verify no crashes or freezes
4. Switch languages multiple times
   ✓ Verify updates are fast and smooth
```

---

## 🚨 Known Issues to Watch For

### Potential Issues

1. **Blank Display on iOS Launch**
   - **Symptom**: Year stem-branch not shown until language switch
   - **Cause**: Initialization not called on startup
   - **Status**: Should be fixed by previous work, but verify

2. **Memory Leak**
   - **Symptom**: App memory grows with each language switch
   - **Cause**: Not properly cleaning up old instances
   - **How to test**: Monitor in Xcode Instruments

3. **Calculation Errors**
   - **Symptom**: Wrong stem-branch for certain dates
   - **Cause**: Edge case in year calculation
   - **How to test**: Test years 2024, 2025, 2026, 2027

4. **Missing Fallback**
   - **Symptom**: Crash or blank when calculation fails
   - **Cause**: No error handling
   - **Status**: Should be handled, but verify graceful degradation

---

## 📊 Test Results Summary

### Overall Status

| Component | Status | Notes |
|-----------|--------|-------|
| Holiday Detail | ⬜ Not Tested | |
| Upcoming Holidays | ⬜ Not Tested | |
| Year Holidays | ⬜ Not Tested | |
| Language Switching | ⬜ Not Tested | |
| Performance | ⬜ Not Tested | |

### Issues Found

| Issue # | Severity | Description | Status |
|---------|----------|-------------|--------|
| | | | |
| | | | |

**Legend**: ⬜ Not Tested | ✅ Pass | ❌ Fail | ⚠️ Warning

---

## 🎬 Next Steps After Testing

### If All Tests Pass ✅

1. Update STATUS.md with test results
2. Mark T060 as fully complete (including device testing)
3. Take final screenshots for documentation
4. Push changes to remote branch
5. Consider moving to Sprint 10 (T056-T059: Unit Tests)

### If Issues Found ❌

1. Document each issue in detail
2. Create bug report with:
   - Steps to reproduce
   - Expected vs actual behavior
   - Screenshots/logs
   - Device/OS version
3. Fix critical issues before proceeding
4. Re-test after fixes

---

## 📝 Test Log Template

```
=== T060 Testing Session ===
Date: _______________
Tester: _______________
Platform: iOS / Android (circle one)
Device: _______________
OS Version: _______________

Test Results:
- Holiday Detail: PASS / FAIL / N/A
- Upcoming Holidays: PASS / FAIL / N/A
- Year Holidays: PASS / FAIL / N/A
- Language Switching: PASS / FAIL / N/A
- Performance: PASS / FAIL / N/A

Issues Found: (list below)
1.
2.
3.

Screenshots Captured: YES / NO
Location: _______________

Overall Status: READY FOR MERGE / NEEDS FIXES

Notes:




Signature: _______________
```

---

## 🔧 Debugging Commands

### View Logs (iOS)

```bash
# Real-time logs
xcrun simctl spawn booted log stream --predicate 'process == "LunarCalendar"' --level debug

# Filter for sexagenary-related logs
xcrun simctl spawn booted log stream --predicate 'eventMessage contains "sexagenary"'
```

### View Logs (Android)

```bash
# Real-time logs
adb logcat | grep -i lunarcalendar

# Filter for errors
adb logcat *:E | grep -i lunarcalendar
```

### Memory Profiling (iOS)

```bash
# Open Instruments
instruments -t "Allocations" -D trace.trace -w "iPhone 16 Pro" LunarCalendar.app
```

---

## ✅ Quick Verification Commands

```bash
# Check if new helper file exists
ls -lh src/LunarCalendar.MobileApp/Helpers/SexagenaryFormatterHelper.cs

# Verify ViewModels updated
grep -n "ISexagenaryService" src/LunarCalendar.MobileApp/ViewModels/HolidayDetailViewModel.cs
grep -n "ISexagenaryService" src/LunarCalendar.MobileApp/ViewModels/YearHolidaysViewModel.cs

# Check build status
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -c Debug | grep -i "error\|succeeded"
```

---

**Ready to Test?** Choose your platform and follow the steps above! 🚀

**Questions?** Refer to:
- Main STATUS: `.specify/features/001-sexagenary-cycle/STATUS.md`
- Session Summary: `support_docs/SESSION_SUMMARY_20260126_T060.md`
- Original Spec: `.specify/features/001-sexagenary-cycle-foundation.md`
