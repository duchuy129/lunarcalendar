# Quick Testing Guide - Critical Bug Fixes

## Quick Start

The app is currently running on **iPhone 16 Pro Simulator (iOS 26.2)**.

## Test 1: Navigation Crash Fix (2-3 minutes)

### Steps:
1. **Launch the app** (already running)
2. **Scroll down** to the "Upcoming Holidays" section
3. **Tap on "Tết Dương Lịch"** (or any other holiday)
4. **View the holiday detail page**
5. **Tap the back button** to return to calendar
6. **Repeat steps 3-5** several times rapidly

### Expected Result:
- ✅ App navigates back smoothly without crashing
- ✅ No error alerts appear
- ✅ Calendar view reloads properly

### What to Check:
- App should NOT crash or freeze
- Collections should update smoothly
- No visual glitches

---

## Test 2: Year Holidays Display Fix (2-3 minutes)

### Steps:
1. **Scroll down** to the "Vietnamese Holidays" section
2. **Check the debug labels** at the top of the section:
   - Should show: "DEBUG: X holidays for year 2026"
   - Should show: "Selected Year: 2026"
3. **Tap the section header** if collapsed to expand it
4. **Verify holidays are displayed** in the list

### Expected Result:
- ✅ Debug label shows holiday count > 0
- ✅ Selected year shows 2026
- ✅ Multiple holidays are displayed with:
  - Date boxes (colored)
  - Holiday names in Vietnamese
  - Gregorian dates
  - Lunar dates (where applicable)

### What to Check:
- Holiday count matches number of items displayed
- All holidays have proper formatting
- Scrolling works smoothly

---

## Test 3: Year Navigation (1-2 minutes)

### Steps:
1. In the **Year Holidays section**, locate the navigation controls
2. **Tap "<" button** to go to 2025
3. **Verify debug label updates** to "Selected Year: 2025"
4. **Check that holidays for 2025 appear**
5. **Tap ">" button** to return to 2026
6. **Tap "Today" button** to ensure it stays on 2026

### Expected Result:
- ✅ Year changes correctly
- ✅ Holidays reload for selected year
- ✅ Debug label shows correct year
- ✅ Holiday count updates

---

## Test 4: Combined Stress Test (2-3 minutes)

### Steps:
1. **Navigate to holiday detail and back** (3 times)
2. **Change year to 2025**
3. **Navigate to a 2025 holiday detail and back**
4. **Change year back to 2026**
5. **Navigate to a 2026 holiday detail and back**
6. **Scroll rapidly** up and down

### Expected Result:
- ✅ No crashes occur
- ✅ UI remains responsive
- ✅ All data loads correctly

---

## Viewing Debug Output (Optional)

If you want to see the detailed debug logs:

1. **Open Xcode** (if not already open)
2. **Go to Window > Devices and Simulators**
3. **Select your running simulator**
4. **Click "Open Console"** button
5. **Filter by "LunarCalendar"**

### What to Look For in Logs:
- `=== LoadYearHolidaysAsync START for year XXXX ===`
- `=== Got X holidays from service ===`
- `=== YearHolidays collection updated with X items ===`
- No ERROR messages

---

## Quick Verification Checklist

After running all tests, verify:

- [ ] No crashes occurred during any test
- [ ] Upcoming Holidays section displays holidays
- [ ] Year Holidays section displays holidays
- [ ] Debug labels show correct counts and year
- [ ] Navigation back from detail works smoothly
- [ ] Year navigation updates holidays correctly
- [ ] App feels responsive and smooth

---

## If Issues Occur

### App Crashes:
1. Check Xcode console for error messages
2. Look for `=== ERROR` in the logs
3. Note the exact steps that caused the crash

### Year Holidays Not Showing:
1. Check the debug label - what count does it show?
2. Is the section expanded? (tap header)
3. Check Xcode console for `LoadYearHolidaysAsync` messages

### Empty View Appears:
If you see "No holidays found for this year":
1. Check the debug label for the selected year
2. Check Xcode console for any ERROR messages
3. This might indicate a filtering issue

---

## Removing Debug Elements (After Testing)

Once you've verified everything works:

1. Edit `CalendarPage.xaml`
2. Remove or comment out the debug labels:
   ```xml
   <!-- DEBUG: Remove these after testing -->
   <Label Text="{Binding YearHolidays.Count, StringFormat='DEBUG: {0} holidays for year {1}'}"/>
   <Label Text="{Binding SelectedYear, StringFormat='Selected Year: {0}'}"/>
   ```
3. Rebuild and redeploy

---

## Current Status

✅ App is deployed and running on iOS Simulator 26.2  
✅ Both critical bugs have been fixed  
✅ Ready for testing

**Estimated Testing Time:** 10-15 minutes for all tests

---

**Note:** The app should feel much more stable now with the error handling improvements. If you encounter any issues, check the debug output first!
