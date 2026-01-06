# Physical Device Testing Checklist

**Vietnamese Lunar Calendar App - MVP Testing Guide**

---

## 📱 Device Requirements

Test on at least one device from each category:

### iOS Devices
- [ ] **iPhone** (iPhone 12 or newer recommended)
- [ ] **iPad** (iPad 9th gen or newer recommended)
- **Minimum iOS**: 15.0+

### Android Devices
- [ ] **Phone** (Android 8.0+ recommended)
- [ ] **Tablet** (Optional but recommended for tablet UI testing)
- **Minimum Android**: 8.0 (API 26)+

---

## 🔧 Pre-Testing Setup

### 1. iOS Setup (iPhone/iPad)

#### Install on Physical Device

**Option A: Via Xcode (Development Build)**
```bash
# Connect iPhone/iPad via USB
# In VS Code or terminal:
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Build and deploy to connected device
dotnet build -f net8.0-ios -c Release
# Then deploy via Xcode or Visual Studio for Mac
```

**Option B: TestFlight (Recommended for Beta Testing)**
1. Create App Store Connect account
2. Upload build to TestFlight
3. Invite yourself as tester
4. Install from TestFlight app on device

#### API Configuration for Physical Device
Update [HolidayService.cs](src/LunarCalendar.MobileApp/Services/HolidayService.cs) line 36:
```csharp
// BEFORE TESTING: Replace with your production API URL
_baseUrl = "https://your-app.ondigitalocean.app";
// OR use local network IP for testing: "http://YOUR_COMPUTER_IP:5090"
```

Do the same for [CalendarService.cs](src/LunarCalendar.MobileApp/Services/CalendarService.cs)

---

### 2. Android Setup

#### Install on Physical Device

```bash
# Enable Developer Mode on Android:
# Settings → About Phone → Tap "Build Number" 7 times

# Enable USB Debugging:
# Settings → Developer Options → USB Debugging

# Connect device via USB
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Build and install
dotnet build -f net8.0-android -c Release
dotnet build -f net8.0-android -c Release -t:Install

# Or manually install APK:
# Find APK in: src/LunarCalendar.MobileApp/bin/Release/net8.0-android/
# Transfer to device and install
```

#### API Configuration for Physical Device
Same as iOS - update both service files with production or local network IP.

---

## ✅ Testing Checklist

### 1. Installation & First Launch

#### iOS
- [ ] App installs without errors
- [ ] App icon appears correctly on home screen
- [ ] Launch screen displays properly
- [ ] No crashes on first launch
- [ ] App requests necessary permissions (if any)

#### Android
- [ ] APK installs successfully
- [ ] App icon appears in app drawer
- [ ] Splash screen shows correctly
- [ ] No crashes on first launch
- [ ] App requests necessary permissions (if any)

**Expected Behavior:**
- App opens to calendar view (no guest landing page)
- Current month and year displayed
- Today's date highlighted

**Screenshot Location:** Take screenshots for app store submission

---

### 2. Core Functionality - Calendar Display

#### Test Cases

**TC1: Calendar Month View**
- [ ] Current month displays correctly
- [ ] All days of month are visible
- [ ] Today's date is highlighted
- [ ] Weekend dates are colored appropriately
- [ ] Calendar grid layout is properly aligned

**TC2: Lunar Date Display**
- [ ] Each day shows lunar date below Gregorian date
- [ ] Lunar dates update when switching months
- [ ] First day of lunar month highlighted (if applicable)
- [ ] Lunar dates are readable (font size OK)

**TC3: Holiday Display**
- [ ] Holidays show with colored background
- [ ] Holiday names are displayed
- [ ] Multiple holidays on same day handled correctly
- [ ] Holiday colors match type (Major = red, Traditional = orange, etc.)

**Expected Behavior:**
- Smooth scrolling through calendar
- No UI glitches or overlapping text
- Dates align properly in grid

---

### 3. Navigation & Interaction

#### Month/Year Navigation

**TC4: Previous/Next Month Buttons**
- [ ] Tap "Previous Month" → goes to previous month
- [ ] Tap "Next Month" → goes to next month
- [ ] Month name and year update correctly
- [ ] Lunar dates update for new month
- [ ] Holidays update for new month

**TC5: Month/Year Picker**
- [ ] Tap month/year display → picker appears
- [ ] Can select different year (scroll through years)
- [ ] Can select different month
- [ ] Tap "Done" → calendar updates to selected month/year
- [ ] Tap "Cancel" → returns to current view without changes

**TC6: Jump to Today**
- [ ] Navigate to different month
- [ ] Tap "Today" button → returns to current month
- [ ] Today's date is highlighted

**Expected Behavior:**
- Smooth transitions between months
- No lag or freeze when changing dates
- Animations are fluid (if any)

---

### 4. Holiday Details

**TC7: Holiday Tap/Click**
- [ ] Tap on holiday date → detail popup appears
- [ ] Holiday name displayed correctly
- [ ] Description shown (if available)
- [ ] Holiday type indicated
- [ ] "Close" button works

**TC8: Holiday List View** (Settings → Holidays)
- [ ] List of all holidays displays
- [ ] Holidays sorted by date
- [ ] Scrolling works smoothly
- [ ] Each holiday shows date, name, type
- [ ] Public holidays marked/indicated

**Expected Behavior:**
- Readable text in detail popup
- Popup closes without issues
- List view performs well with many items

---

### 5. Settings & Preferences

**TC9: Settings Access**
- [ ] Tap Settings icon/menu → Settings page opens
- [ ] All settings options visible
- [ ] Back button returns to calendar

**TC10: Sync & Offline Status**
- [ ] Connection status shows "Online" when connected
- [ ] Connection status shows "Offline" when airplane mode enabled
- [ ] Last sync time displays correctly
- [ ] "Sync Now" button works when online
- [ ] "Sync Now" disabled/shows message when offline

**TC11: Clear Cache**
- [ ] Tap "Clear Cache" → confirmation prompt appears
- [ ] Confirm → cache cleared successfully
- [ ] App still works (re-fetches data when online)
- [ ] Offline data still available (from SQLite)

**Expected Behavior:**
- Settings save immediately
- No need to restart app for changes
- Clear feedback on actions (success/error messages)

---

### 6. Offline Functionality (CRITICAL)

**TC12: Offline Mode - First Time**
- [ ] Launch app with internet connection
- [ ] Browse calendar (loads data)
- [ ] Enable airplane mode
- [ ] Navigate to different months → previously viewed data still shows
- [ ] Navigate to new month → shows "No data" or loads from cache
- [ ] Status shows "Offline mode"

**TC13: Offline Mode - Pre-cached Data**
- [ ] With internet, browse several months (e.g., Dec 2024, Jan 2025, Feb 2025)
- [ ] Enable airplane mode
- [ ] Navigate back to those months → all data displays correctly
- [ ] Lunar dates show correctly
- [ ] Holidays display correctly
- [ ] No crashes or errors

**TC14: Return Online**
- [ ] Disable airplane mode (reconnect to internet)
- [ ] Status changes to "Online"
- [ ] Tap "Sync Now" → data syncs successfully
- [ ] Navigate to new month → fetches fresh data
- [ ] No duplicate data or corruption

**Expected Behavior:**
- Graceful offline experience
- Clear indication of offline vs online status
- Automatic sync when connection restored
- No data loss

---

### 7. Performance Testing

**TC15: App Launch Time**
- [ ] Cold start (app not in memory) < 3 seconds
- [ ] Warm start (app in background) < 1 second
- [ ] No prolonged white/black screen

**TC16: Navigation Speed**
- [ ] Switching months < 500ms
- [ ] Opening holiday details < 300ms
- [ ] Settings page load < 500ms

**TC17: Memory Usage**
- [ ] Browse 10+ months forward and backward
- [ ] No app slowdown
- [ ] No crashes due to memory
- [ ] Device doesn't heat up excessively

**TC18: Battery Consumption**
- [ ] Use app for 30 minutes actively
- [ ] Check battery drain: should be < 5% (approximate)
- [ ] No abnormal battery usage in settings

**Expected Behavior:**
- Smooth, responsive UI
- No lag or stuttering
- Reasonable battery usage

---

### 8. Network Conditions

**TC19: Slow Internet**
- [ ] Connect to slow WiFi (or use network throttling)
- [ ] Launch app → shows loading indicator
- [ ] Data eventually loads (with retry logic)
- [ ] No crashes on timeout

**TC20: No Internet on Launch**
- [ ] Airplane mode enabled
- [ ] Launch app → shows offline mode
- [ ] App doesn't crash
- [ ] Previous cached data displays (if any)

**TC21: Network Interruption Mid-Use**
- [ ] Using app with internet
- [ ] Suddenly disable WiFi/data
- [ ] App shows offline status
- [ ] Can still browse cached months
- [ ] No crashes

**Expected Behavior:**
- Robust error handling
- Clear user feedback
- Graceful degradation

---

### 9. UI/UX Testing

**TC22: Screen Rotation (iOS/Android)**
- [ ] Rotate device to landscape
- [ ] Calendar adjusts layout correctly
- [ ] No text cut off or overlapping
- [ ] Rotate back to portrait → restores properly

**TC23: Text Readability**
- [ ] All text is readable at arm's length
- [ ] Font sizes appropriate
- [ ] Sufficient contrast (text vs background)
- [ ] Vietnamese characters display correctly (if applicable)

**TC24: Touch Targets**
- [ ] All buttons easy to tap (min 44x44 pts iOS, 48x48 dp Android)
- [ ] No accidental taps on wrong elements
- [ ] Buttons have visual feedback (highlight on press)

**TC25: Dark Mode** (if implemented)
- [ ] Enable device dark mode
- [ ] App respects dark mode setting
- [ ] Colors appropriate for dark background
- [ ] Good contrast and readability

**Expected Behavior:**
- Intuitive navigation
- Professional appearance
- Accessible for all users

---

### 10. Edge Cases & Stress Testing

**TC26: Date Boundaries**
- [ ] Navigate to year 1900 → lunar dates still calculate correctly
- [ ] Navigate to year 2100 → lunar dates still calculate correctly
- [ ] Switch between years rapidly → no crashes

**TC27: Leap Months**
- [ ] Find a year with leap lunar month (e.g., 2023)
- [ ] Verify leap month indicated correctly
- [ ] Holidays in leap month display properly

**TC28: Rapid Interaction**
- [ ] Tap buttons rapidly and randomly
- [ ] Swipe back/forth quickly
- [ ] Open/close popups repeatedly
- [ ] App remains stable, no crashes

**TC29: App Background/Foreground**
- [ ] Use app, then press Home button (background)
- [ ] Wait 1 minute
- [ ] Re-open app → resumes where left off
- [ ] Wait 10+ minutes → may refresh, but no data loss
- [ ] App in background overnight → opens correctly next day

**TC30: Low Storage**
- [ ] Fill device storage to nearly full (< 500MB free)
- [ ] Launch app → still works
- [ ] Clear cache works
- [ ] No crashes

**Expected Behavior:**
- App handles extreme conditions gracefully
- Clear error messages when issues occur
- Data integrity maintained

---

### 11. Integration Testing

**TC31: API Connectivity**
- [ ] Verify API URL is correct (production or test)
- [ ] Lunar date endpoint works: `/api/v1/lunardate/{year}/{month}/{day}`
- [ ] Month info endpoint works: `/api/v1/lunardate/month/{year}/{month}`
- [ ] Holiday year endpoint works: `/api/v1/holiday/year/{year}`
- [ ] Holiday month endpoint works: `/api/v1/holiday/month/{year}/{month}`

**TC32: Authentication** (if login implemented)
- [ ] Login with valid credentials → success
- [ ] Login with invalid credentials → error message
- [ ] Logout → returns to login/welcome screen
- [ ] Stay logged in after app restart

**Expected Behavior:**
- API calls succeed with correct data
- Authentication flow smooth and secure

---

### 12. Crash & Error Handling

**TC33: Network Errors**
- [ ] Disconnect internet mid-request → shows error, doesn't crash
- [ ] Invalid API URL → shows error message
- [ ] API returns 500 error → app handles gracefully

**TC34: Data Corruption**
- [ ] Clear app data/cache
- [ ] Re-launch app → rebuilds cache correctly

**TC35: App Crash Recovery**
- [ ] Force quit app
- [ ] Re-open → starts fresh, no corruption
- [ ] Previous state recovered (if appropriate)

**Expected Behavior:**
- No unhandled exceptions
- User-friendly error messages
- App recovers gracefully

---

## 📊 Testing Results Template

### Test Session Information
- **Date**: _____________
- **Tester**: _____________
- **Device**: _____________ (e.g., iPhone 13, Samsung Galaxy S21)
- **OS Version**: _____________ (e.g., iOS 17.2, Android 13)
- **App Version**: _____________
- **API Environment**: _____________ (Production / Staging / Local)

### Summary
- **Tests Passed**: _____ / _____
- **Tests Failed**: _____
- **Crashes**: _____
- **Critical Issues**: _____
- **Minor Issues**: _____

### Issues Found

| # | Severity | Test Case | Description | Steps to Reproduce |
|---|----------|-----------|-------------|-------------------|
| 1 | Critical/High/Medium/Low | TC## | Description | 1. Step 1<br>2. Step 2 |
| 2 | | | | |

**Severity Definitions:**
- **Critical**: App crashes, data loss, core feature broken
- **High**: Major feature not working, significant UX issue
- **Medium**: Minor feature issue, cosmetic but noticeable
- **Low**: Cosmetic only, doesn't impact functionality

---

## 🐛 Bug Reporting Format

When reporting issues:

```
**Title**: [Device] Brief description

**Severity**: Critical/High/Medium/Low

**Device**: iPhone 13 (iOS 17.2) / Samsung Galaxy S21 (Android 13)

**Steps to Reproduce**:
1. Launch app
2. Navigate to December 2024
3. Tap on holiday date
4. Observe issue

**Expected**: Holiday detail popup should appear

**Actual**: App crashes

**Frequency**: Always / Sometimes / Rarely

**Screenshots**: [Attach if applicable]

**Logs**: [Attach crash logs if available]
```

---

## 📸 Screenshot Requirements for App Stores

While testing, capture high-quality screenshots for app store submissions:

### iOS Screenshots Needed
- **6.5" Display** (iPhone 14 Pro Max, 15 Pro Max): 1290 x 2796 px
  - [ ] Calendar main view (current month with today highlighted)
  - [ ] Calendar with holidays visible
  - [ ] Month/Year picker open
  - [ ] Holiday detail popup
  - [ ] Settings page

- **12.9" Display** (iPad Pro): 2048 x 2732 px
  - [ ] Same as above, but iPad layout

### Android Screenshots Needed
- **Phone**: 1080 x 1920 px minimum
  - [ ] Same views as iOS

- **7" Tablet**: 1200 x 1920 px
- **10" Tablet**: 1800 x 2560 px

**Tips for good screenshots:**
- Use December/January (has holidays)
- Show today's date highlighted
- Clean, realistic data
- Good lighting in screenshots
- Represent actual use case

---

## ✅ MVP Release Criteria

Before submitting to app stores, ensure:

### Functional Requirements
- [ ] All core features work on iOS and Android
- [ ] Offline mode works correctly
- [ ] No critical or high-severity bugs
- [ ] API connectivity stable

### Performance Requirements
- [ ] App launches in < 3 seconds
- [ ] No crashes during 1-hour testing session
- [ ] Smooth scrolling and navigation
- [ ] Acceptable battery usage

### Quality Requirements
- [ ] UI looks professional on all tested devices
- [ ] Text is readable and properly aligned
- [ ] Colors and styling consistent
- [ ] Screenshots captured for app stores

### Legal Requirements
- [ ] Privacy Policy created and linked
- [ ] Terms of Service created and linked
- [ ] App Store/Play Store guidelines followed
- [ ] Content appropriate (no offensive material)

---

## 🆘 Common Issues & Solutions

### Issue: "Could not connect to API"
**Solution**:
- Check API URL in HolidayService.cs and CalendarService.cs
- Ensure API is running (DigitalOcean or local)
- Verify device has internet connection
- Check firewall settings

### Issue: "No lunar dates showing"
**Solution**:
- Check API response in logs
- Verify CalendarService is fetching data
- Clear app cache and retry
- Check offline database has data

### Issue: "App crashes on startup"
**Solution**:
- Check device logs (Xcode Console / Android Logcat)
- Verify all dependencies installed
- Clean and rebuild project
- Check for null reference exceptions

### Issue: "Holidays not displaying"
**Solution**:
- Verify holiday data in API
- Check HolidayService endpoint URLs
- Inspect network requests in logs
- Clear cache and re-sync

---

## 📝 Post-Testing Checklist

After completing all tests:

- [ ] Document all bugs in issue tracker
- [ ] Prioritize bugs (Critical → High → Medium → Low)
- [ ] Fix critical and high-severity bugs
- [ ] Re-test fixed bugs
- [ ] Update app version number
- [ ] Capture app store screenshots
- [ ] Update CHANGELOG.md with fixes
- [ ] Ready for app store submission

---

## 🚀 Next Steps After Testing

1. **Fix Critical Issues**: Address all crashes and major bugs
2. **Optimize Performance**: Based on testing results
3. **Finalize Screenshots**: Edit and prepare for stores
4. **Update Documentation**: Reflect any changes
5. **Prepare App Store Listings**: Descriptions, keywords, categories
6. **Submit to App Stores**: iOS App Store and Google Play

---

**Good luck with testing! Report all issues promptly for quick resolution. 🎉**
