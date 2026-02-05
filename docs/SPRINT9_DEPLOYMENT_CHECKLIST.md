# 🚀 Sprint 9 Deployment Checklist
## v1.1.0 - Sexagenary Cycle Release

**Target Release Date:** February 2026  
**Version:** 1.1.0 (Build 6)  
**Branch:** feature/001-sexagenary-cycle-complete

---

## 📅 Timeline Overview

| Phase | Duration | Target Date | Status |
|-------|----------|-------------|--------|
| Code Freeze | Day -7 | Feb 1 | ⏳ Pending |
| Final Testing | Day -5 to -3 | Feb 3-5 | ⏳ Pending |
| Build & Package | Day -2 | Feb 6 | ⏳ Pending |
| Submit to Apple | Day -1 | Feb 7 | ⏳ Pending |
| Apple Review | Day 0-3 | Feb 8-11 | ⏳ Pending |
| Release to Production | Day 4 | Feb 12 | ⏳ Pending |

---

## ✅ PHASE 1: Code Freeze (Day -7)

### Code Completion
- [x] All Sprint 9 features implemented
- [x] Core calculation engine complete (`SexagenaryCalculator.cs`)
- [x] Service layer with caching (`SexagenaryService.cs`)
- [x] UI integration in calendar view
- [x] Settings toggle implemented
- [x] Multi-language support (EN/VI/ZH)
- [ ] **DEFERRED:** Date detail page (moved to v1.2.0)
- [ ] Merge `feature/001-sexagenary-cycle-complete` to `main`
- [ ] Create release branch `release/v1.1.0`

**Commands:**
```bash
# Verify current branch
git branch --show-current

# Merge to main (if approved)
git checkout main
git merge feature/001-sexagenary-cycle-complete --no-ff
git push origin main

# Create release branch
git checkout -b release/v1.1.0
git push origin release/v1.1.0
```

### Quality Assurance Sign-Off
- [x] Unit tests: 108/108 passing ✅
- [x] Historical validation: 36/36 passing ✅
- [x] Zero compilation errors ✅
- [ ] Code review completed
- [ ] Security review (if applicable)
- [ ] Performance benchmarks acceptable

---

## 🧪 PHASE 2: Final Testing (Day -5 to -3)

### Device Testing Matrix

#### iPhone Testing
- [ ] iPhone SE (2nd gen) - iOS 15.0 (minimum supported)
- [ ] iPhone 13 - iOS 17.x
- [ ] iPhone 14 Pro - iOS 17.x
- [ ] iPhone 15 Pro Max - iOS 18.x (latest)
- [ ] iPhone 16 Pro Max - iOS 18.x (largest screen)

#### iPad Testing
- [ ] iPad (9th gen) - iOS 15.0
- [ ] iPad Air (5th gen) - iOS 17.x
- [ ] iPad Pro 12.9" (6th gen) - iOS 18.x

### Test Scenarios (All Devices)

#### Core Functionality
- [ ] App launches without crash
- [ ] Calendar displays correctly
- [ ] Today's date highlighted
- [ ] Lunar dates accurate
- [ ] Holidays display correctly

#### Sprint 9 Features
- [ ] **Sexagenary Cycle displays** when enabled
- [ ] Can Chi (天干地支) shows below each date
- [ ] Five Elements colors render correctly:
  - [ ] 🟢 Wood (Mộc) - Forest Green
  - [ ] 🔴 Fire (Hỏa) - Crimson
  - [ ] 🟤 Earth (Thổ) - Saddle Brown
  - [ ] ⚪ Metal (Kim) - Silver
  - [ ] 🔵 Water (Thủy) - Deep Sky Blue
- [ ] Settings toggle "Show Sexagenary Cycle" works
- [ ] Toggle persists after app restart
- [ ] Tap date cell shows Can Chi (basic interaction)

#### Localization
- [ ] English (EN) - all texts display correctly
- [ ] Vietnamese (VI) - diacritics render properly
- [ ] Chinese (ZH) - characters display correctly
- [ ] Language switching works without app restart

#### Visual Modes
- [ ] Light mode - all colors visible
- [ ] Dark mode - all colors visible, proper contrast
- [ ] Automatic theme switching works

#### Performance
- [ ] App launch time <2 seconds
- [ ] Calendar navigation smooth (60 FPS)
- [ ] Date selection response <100ms
- [ ] Memory usage <100MB
- [ ] No memory leaks detected (Xcode Instruments)
- [ ] Battery impact: negligible

#### Edge Cases
- [ ] Year 1900 displays correctly
- [ ] Year 2100 displays correctly
- [ ] Leap years calculate correctly
- [ ] Lunar New Year dates accurate
- [ ] Tet holidays correct (2026, 2027, 2028)

### Regression Testing
- [ ] All v1.0.1 features still work
- [ ] Holiday list unchanged
- [ ] Year picker works
- [ ] Settings page loads
- [ ] About page displays
- [ ] No crashes from previous fixes

### Bug Log
**Track any bugs found:**

| ID | Description | Severity | Status | Fixed By |
|----|-------------|----------|--------|----------|
| - | - | - | - | - |

*If critical bugs found: STOP deployment, fix bugs, restart testing*

---

## 📦 PHASE 3: Build & Package (Day -2)

### Pre-Build Checklist

#### Version Numbers
- [ ] Update `LunarCalendar.MobileApp.csproj`:
  ```xml
  <ApplicationDisplayVersion>1.1.0</ApplicationDisplayVersion>
  <ApplicationVersion>6</ApplicationVersion>
  ```
- [ ] Verify `Info.plist` synced (auto by .NET MAUI)
- [ ] Update `VERSION_HISTORY.md` with release date

#### Git Tagging
```bash
# Create release tag
git tag -a v1.1.0 -m "Release v1.1.0 - Sprint 9: Sexagenary Cycle"
git push origin v1.1.0

# Verify tag
git tag -l
git show v1.1.0
```

#### Documentation Updates
- [ ] Update `README.md` with v1.1.0 features
- [ ] Update `CHANGELOG.md` (create if doesn't exist)
- [ ] Verify `docs/APP_STORE_DEPLOYMENT_RUNBOOK.md` current
- [ ] Archive Sprint 9 documentation

### Build Process

#### Step 1: Clean Workspace
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Remove build artifacts
rm -rf src/LunarCalendar.MobileApp/bin/Release
rm -rf src/LunarCalendar.MobileApp/obj/Release

# Clean .NET cache
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -c Release
```

- [ ] Workspace cleaned

#### Step 2: Verify Certificates & Provisioning
```bash
# Check Distribution certificate
security find-identity -v -p codesigning | grep "Apple Distribution"

# Verify provisioning profile
ls -lh releases/Lunar_Calendar_App_Store.mobileprovision

# Check profile details
security cms -D -i releases/Lunar_Calendar_App_Store.mobileprovision | grep -A5 "Name"
```

- [ ] Certificate found and valid
- [ ] Provisioning profile exists and not expired
- [ ] Bundle ID matches: `com.huynguyen.lunarcalendar`

#### Step 3: Build App Store IPA
```bash
# Run automated build script
bash scripts/build-ios-appstore.sh
```

- [ ] Build completed successfully (exit code 0)
- [ ] Zero errors
- [ ] Zero warnings (or documented exceptions)
- [ ] IPA file created

**IPA Location:**
```
src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/LunarCalendar.MobileApp.ipa
```

#### Step 4: Validate IPA
```bash
# Check IPA size (should be 40-60 MB)
ls -lh src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/*.ipa

# Extract and verify contents
mkdir -p /tmp/ipa-validation
cd /tmp/ipa-validation
unzip ~/path/to/LunarCalendar.MobileApp.ipa
plutil -p Payload/*.app/Info.plist | grep -E "CFBundleVersion|CFBundleShortVersionString"
```

- [ ] IPA size reasonable (40-60 MB)
- [ ] Info.plist shows version 1.1.0, build 6
- [ ] Bundle ID correct
- [ ] Code signature valid

#### Step 5: Archive Build Artifacts
```bash
# Create release archive
mkdir -p releases/v1.1.0
cp src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/*.ipa releases/v1.1.0/
cp -r docs releases/v1.1.0/
tar -czf releases/v1.1.0-sprint9.tar.gz releases/v1.1.0/

# Backup to external location (optional)
```

- [ ] IPA archived to `releases/v1.1.0/`
- [ ] Backup created
- [ ] Build artifacts saved

---

## 📸 PHASE 4: Screenshots & Assets (Day -2)

### Screenshot Preparation

#### Capture Screenshots
```bash
# Launch iPhone 16 Pro Max simulator (6.7" display)
xcrun simctl boot "iPhone 16 Pro Max"
bash scripts/deploy-iphone-simulator.sh

# Enable Sexagenary Cycle in Settings
# Navigate through app taking screenshots (Cmd+S)

# Take screenshots programmatically (optional)
xcrun simctl io booted screenshot ~/Desktop/screenshot1.png
```

#### Required Screenshots (6.7" Display - 1290 x 2796 px)
1. [ ] **Main Calendar** - Today highlighted, Can Chi visible, light mode
2. [ ] **Calendar with Can Chi** - Zoomed view showing Five Elements colors
3. [ ] **Settings Page** - "Show Sexagenary Cycle" toggle visible
4. [ ] **Holiday List** - Vietnamese holidays displayed
5. [ ] **Year Picker** - Year selection UI
6. [ ] **Dark Mode Calendar** - Same as #1 but dark theme

#### Screenshot Editing
- [ ] Crop to exact dimensions: 1290 x 2796 px
- [ ] Remove status bar or use clean status bar
- [ ] Ensure no personal data visible
- [ ] Save as PNG with high quality
- [ ] Name files descriptively: `01-main-calendar.png`

#### Additional Sizes (if targeting iPhone 8 Plus, iPad)
- [ ] 5.5" Display (1242 x 2208 px) - scale down from 6.7"
- [ ] iPad Pro 12.9" (2048 x 2732 px) - iPad simulator screenshots

### App Preview Video (Optional)
- [ ] 15-30 second video showing key features
- [ ] Portrait orientation
- [ ] Resolution: 1290 x 2796 px @ 30 FPS
- [ ] Show: Launch → Calendar → Tap date → Settings toggle
- [ ] Add background music (royalty-free)
- [ ] Export as .mp4 or .mov

---

## 🚀 PHASE 5: App Store Connect Submission (Day -1)

### App Store Connect Setup

#### Login & Navigate
1. [ ] Login to https://appstoreconnect.apple.com
2. [ ] Go to **My Apps**
3. [ ] Select **Vietnamese Lunar Calendar** (or create new)

#### Create New Version
1. [ ] Click **+ Version or Platform** → **iOS**
2. [ ] Enter version: **1.1.0**
3. [ ] Click **Create**

### Upload IPA

#### Method: Using Transporter (Recommended)
```bash
# Open Transporter app
open -a Transporter

# OR use command line (need API key)
xcrun altool --upload-app \
  -f src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/LunarCalendar.MobileApp.ipa \
  -t ios \
  --apiKey "YOUR_API_KEY" \
  --apiIssuer "YOUR_ISSUER_ID"
```

- [ ] IPA uploaded successfully
- [ ] Upload completed without errors
- [ ] Build shows "Processing" (wait 10-30 minutes)
- [ ] Build status changes to "Ready to Submit"

### Configure App Store Listing

#### What's New in This Version
- [ ] Copy release notes from below
- [ ] Paste into App Store Connect
- [ ] Character count under 4000

**Release Notes (English):**
```
🎉 New in Version 1.1.0

✨ SEXAGENARY CYCLE (CAN CHI / 干支)
• Traditional Chinese 60-year cycle display
• Shows Heavenly Stems (Thiên Can / 天干)
• Shows Earthly Branches (Địa Chi / 地支)
• Color-coded Five Elements (Ngũ Hành / 五行):
  🟢 Wood (Mộc) • 🔴 Fire (Hỏa) • 🟤 Earth (Thổ)
  ⚪ Metal (Kim) • 🔵 Water (Thủy)
• Multi-language support (Chinese, Vietnamese, English)
• Toggle display in Settings

Connect with ancient Asian timekeeping traditions!

📱 As always: Fully offline, no ads, no tracking.
```

**Release Notes (Vietnamese):**
```
🎉 Phiên bản mới 1.1.0

✨ CAN CHI (干支)
• Hiển thị chu kỳ Can Chi truyền thống
• Thiên Can (天干) và Địa Chi (地支)
• Màu sắc theo Ngũ Hành (五行):
  🟢 Mộc • 🔴 Hỏa • 🟤 Thổ • ⚪ Kim • 🔵 Thủy
• Hỗ trợ đa ngôn ngữ
• Bật/tắt trong Cài đặt

Kết nối với truyền thống phương Đông!

📱 Vẫn hoàn toàn offline, không quảng cáo.
```

#### Upload Screenshots
- [ ] Upload 6 screenshots for 6.7" display
- [ ] Upload 6 screenshots for 5.5" display (if available)
- [ ] Upload 6 screenshots for iPad (if available)
- [ ] Upload App Preview video (if created)
- [ ] Screenshots render correctly in preview

#### App Information
- [ ] **Name:** Vietnamese Lunar Calendar (no change)
- [ ] **Subtitle:** Traditional Calendar & Can Chi (updated)
- [ ] **Category:** Utilities / Reference (no change)
- [ ] **Keywords:** `lunar,calendar,vietnamese,can chi,zodiac,astrology,holiday,tradition,offline,五行,干支`

#### Support & Marketing URLs
- [ ] Support URL: GitHub repo or website
- [ ] Marketing URL (optional): Website or blog
- [ ] Privacy Policy URL: Current and accessible

#### Age Rating
- [ ] Review age rating (should be 4+)
- [ ] No changes needed

### Review Information

#### App Review Notes
```
Hello Apple Review Team,

This is version 1.1.0 of Vietnamese Lunar Calendar, adding a new feature: Sexagenary Cycle (Can Chi / 干支) display with Five Elements color coding.

KEY POINTS:
• This is a traditional Asian calendar feature, not related to fortune telling or gambling
• The app is 100% offline - no network requests, no ads, no tracking
• All data is calculated locally using historical algorithms
• Tested on iOS 15.0 - 18.x
• No in-app purchases or subscriptions

TEST INSTRUCTIONS:
1. Open app - calendar displays with lunar dates
2. Go to Settings → Enable "Show Sexagenary Cycle"
3. Return to calendar - see Can Chi (天干地支) below each date with color coding
4. Tap any date to view detailed information (basic interaction)
5. Test language switching: EN, VI, ZH

Thank you for your review!

Contact: [Your email]
```

- [ ] Review notes added
- [ ] Contact information provided
- [ ] Demo account (if applicable): N/A

#### Version Release
- [ ] **Release option:** Automatically release after approval
- [ ] **Phased release:** Enabled (7-day rollout)
- [ ] **Reset summary rating:** No (keep existing ratings)

### Submit for Review

#### Final Pre-Submission Check
- [ ] Build selected: v1.1.0 (6)
- [ ] Build status: "Ready to Submit" ✅
- [ ] Screenshots uploaded: 6+ images ✅
- [ ] What's New text: Filled ✅
- [ ] Review notes: Added ✅
- [ ] All required fields: Complete ✅

#### Submit
1. [ ] Click **Add for Review**
2. [ ] Review submission details
3. [ ] Click **Submit to App Review**
4. [ ] Confirmation received
5. [ ] Status changes to "Waiting for Review"

**Submission timestamp:** _________________

---

## ⏳ PHASE 6: Apple Review (Day 0-3)

### Monitor Status

#### Daily Checks
- [ ] Day 1: Check status morning/evening
- [ ] Day 2: Check status morning/evening
- [ ] Day 3: Check status morning/evening

**Current Status:** ___________________

#### Status Progression
- [⏳] **Waiting for Review** - Submitted, in queue
- [ ] **In Review** - Apple testing app (1-4 hours)
- [ ] **Pending Developer Release** - Approved! Ready to release
- [ ] **Ready for Sale** - Live on App Store
- [ ] **Rejected** - See rejection reasons

### If Rejected

#### Common Rejection Reasons
1. **Guideline 2.1 - App Completeness**
   - Fix: Ensure all features work as described
2. **Guideline 4.0 - Design**
   - Fix: Improve UI/UX, update screenshots
3. **Guideline 5.1.1 - Privacy**
   - Fix: Update privacy policy, data collection

#### Rejection Response Process
1. [ ] Read rejection reason carefully
2. [ ] Identify fix required
3. [ ] Make necessary changes
4. [ ] Increment build number (6 → 7)
5. [ ] Rebuild and resubmit
6. [ ] Respond in Resolution Center if needed

### If Approved
- [ ] 🎉 **APPROVED!**
- [ ] Status: "Pending Developer Release"
- [ ] Choose release timing:
  - [ ] Release immediately
  - [ ] Schedule release date/time
  - [ ] Manual release (recommended for monitoring)

---

## 🚀 PHASE 7: Production Release (Day 4)

### Release to Production

#### Manual Release (Recommended)
1. [ ] App Store Connect → Version 1.1.0
2. [ ] Click **Release this Version**
3. [ ] Confirm release
4. [ ] Status changes to "Ready for Sale"

**Release timestamp:** _________________

### Phased Rollout Schedule
- **Day 1:** 1% of users (monitor closely)
- **Day 2:** 2% of users
- **Day 3:** 5% of users
- **Day 4:** 10% of users
- **Day 5:** 20% of users
- **Day 6:** 50% of users
- **Day 7:** 100% of users

### Monitoring (Day 1-7)

#### Crash Monitoring
- [ ] Xcode → Window → Organizer → Crashes
- [ ] Check crash rate: **Target <0.1%**
- [ ] Current crash rate: ___________

#### App Analytics
- [ ] App Store Connect → Analytics → App Analytics
- [ ] Monitor:
  - [ ] Downloads: _________
  - [ ] App Store views: _________
  - [ ] Conversion rate: _________

#### User Reviews
- [ ] App Store Connect → My Apps → Ratings and Reviews
- [ ] Current rating: _____ stars
- [ ] Number of reviews: _____
- [ ] Respond to reviews (especially negative ones)

#### Performance Metrics
- [ ] Crash-free rate: _____ (target >99.5%)
- [ ] Average rating: _____ (target >4.0)
- [ ] User retention: _____ (target >80%)

### Issue Tracking

#### Bug Log (Post-Release)
| Date | Issue | Severity | Reports | Status | Action |
|------|-------|----------|---------|--------|--------|
| - | - | - | - | - | - |

**Severity Levels:**
- 🔴 **Critical:** Crashes, data loss, security issues → Hotfix immediately
- 🟠 **High:** Major feature broken → Patch in 1-3 days
- 🟡 **Medium:** Minor bug, workaround exists → Next release
- 🟢 **Low:** Cosmetic issue → Backlog

### Post-Release Actions

#### Day 1
- [ ] Monitor crash reports (check every 2 hours)
- [ ] Check first user reviews
- [ ] Respond to any support emails
- [ ] Verify phased rollout at 1%

#### Day 3
- [ ] Review crash rate (should be stable)
- [ ] Analyze user feedback
- [ ] Update internal roadmap based on feedback
- [ ] Continue phased rollout (5%)

#### Day 7
- [ ] Final crash rate analysis
- [ ] Compile user feedback report
- [ ] Plan hotfix if needed (v1.1.1)
- [ ] Plan next sprint features (v1.2.0)
- [ ] Complete rollout to 100%

#### Week 2
- [ ] Post-mortem meeting
- [ ] Update deployment runbook with lessons learned
- [ ] Archive Sprint 9 documentation
- [ ] Celebrate success! 🎉

---

## 📢 Marketing & Announcements

### Social Media
- [ ] Twitter/X announcement
- [ ] Facebook post
- [ ] LinkedIn update (if applicable)
- [ ] Reddit (r/iOSProgramming, r/Vietnamese, etc.)

**Sample Post:**
```
🎉 Vietnamese Lunar Calendar v1.1.0 is now LIVE on the App Store!

✨ NEW: Sexagenary Cycle (Can Chi / 干支) with Five Elements color coding
🌍 Multi-language support (EN/VI/ZH)
📱 100% offline, no ads, no tracking

Download: [App Store Link]

#iOS #LunarCalendar #CanChi #VietnameseCalendar #Astrology
```

### Website/Blog
- [ ] Update homepage with v1.1.0 announcement
- [ ] Write blog post about Sprint 9 features
- [ ] Update documentation
- [ ] Add v1.1.0 to changelog

### Community
- [ ] Notify beta testers
- [ ] Thank contributors
- [ ] Update GitHub README
- [ ] Close Sprint 9 milestone

---

## 🔄 Rollback Plan

### Scenario: Critical Bug Found

#### Immediate Actions (Within 1 hour)
1. [ ] Assess severity (is it critical?)
2. [ ] Document bug details
3. [ ] Notify team
4. [ ] Decide: Hotfix or rollback?

#### Option A: Hotfix (Recommended)
```bash
# Create hotfix branch
git checkout -b hotfix/v1.1.1 v1.1.0

# Fix bug
# ... make changes ...

# Update version to 1.1.1 (Build 7)
# Build, test, submit with Expedited Review
```

- [ ] Hotfix branch created
- [ ] Bug fixed
- [ ] Version updated to 1.1.1 (Build 7)
- [ ] Tests passing
- [ ] Expedited review requested
- [ ] Expected approval: 2-12 hours

#### Option B: Pause Rollout
1. [ ] App Store Connect → Version 1.1.0
2. [ ] Click **Pause Phased Release**
3. [ ] No new users get update
4. [ ] Fix bug and release v1.1.1

#### Option C: Remove from Sale (Last Resort)
1. [ ] App Store Connect → Pricing and Availability
2. [ ] Uncheck all territories
3. [ ] App removed from store
4. [ ] Fix bug and re-release
- **Note:** This affects all users, very disruptive

---

## ✅ Final Sign-Off

### Deployment Approval

**Developer Sign-Off:**
- [x] Code complete: Yes
- [x] Tests passing: 108/108 ✅
- [ ] Device testing complete
- [ ] Ready to deploy

**Signature:** ________________ **Date:** ________

**QA Sign-Off:**
- [ ] All test scenarios passed
- [ ] No critical bugs
- [ ] Regression testing complete
- [ ] Ready to deploy

**Signature:** ________________ **Date:** ________

**Product Owner Sign-Off:**
- [ ] Features meet requirements
- [ ] Release notes approved
- [ ] Screenshots approved
- [ ] Ready to deploy

**Signature:** ________________ **Date:** ________

---

## 📚 Reference Documents

- **Deployment Runbook:** `docs/APP_STORE_DEPLOYMENT_RUNBOOK.md`
- **Sprint 9 Summary:** `SPRINT9_IMPLEMENTATION_COMPLETE.md`
- **Version History:** `VERSION_HISTORY.md`
- **Testing Checklist:** `TESTING_CHECKLIST_SPRINT9.md`
- **Build Scripts:** `scripts/build-ios-appstore.sh`

---

## 📊 Success Metrics (Post-Release)

| Metric | Target | Actual | Status |
|--------|--------|--------|--------|
| Crash-free rate | >99.5% | _____ | ⏳ |
| App Store rating | >4.0 | _____ | ⏳ |
| Download count (Week 1) | 100+ | _____ | ⏳ |
| Positive reviews | >80% | _____ | ⏳ |
| Support tickets | <10 | _____ | ⏳ |
| Rollback needed | No | _____ | ⏳ |

---

**Document Status:** 🟢 Active  
**Last Updated:** January 30, 2026  
**Next Review:** After v1.1.0 deployment

**🚀 Good luck with the deployment!**
