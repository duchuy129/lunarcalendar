# 📱 App Store Deployment Runbook
## iOS Release Process for Lunar Calendar App

**Document Version:** 1.0  
**Last Updated:** January 30, 2026  
**Target Release:** v1.1.0 (Sprint 9 - Sexagenary Cycle)

---

## 📋 Table of Contents
1. [Pre-Deployment Checklist](#pre-deployment-checklist)
2. [Version & Build Management](#version--build-management)
3. [Testing & Quality Assurance](#testing--quality-assurance)
4. [Build Process](#build-process)
5. [App Store Connect Submission](#app-store-connect-submission)
6. [Post-Submission](#post-submission)
7. [Rollback Procedures](#rollback-procedures)
8. [Sprint 9 Specific Checklist](#sprint-9-specific-checklist)

---

## 🎯 Pre-Deployment Checklist

### Phase 1: Code Freeze & Preparation (Day -7 to -3)

#### ✅ Code Quality
- [ ] All Sprint 9 features implemented and merged to main branch
- [ ] No compilation errors on iOS build
- [ ] No compilation warnings (or documented exceptions)
- [ ] All unit tests passing (currently: 108/108 ✅)
- [ ] Code review completed and approved
- [ ] Branch: `feature/001-sexagenary-cycle-complete` merged to `main`

#### ✅ Testing Completed
- [ ] All test cases from `TESTING_CHECKLIST_SPRINT9.md` passed
- [ ] Device testing on minimum iOS version (iOS 15.0)
- [ ] Device testing on latest iOS version (iOS 18.x)
- [ ] iPad testing (different screen sizes)
- [ ] Dark mode and light mode testing
- [ ] Localization testing (English, Vietnamese, Chinese)
- [ ] Memory leak testing (Xcode Instruments)
- [ ] Performance benchmarking (launch time, navigation)

#### ✅ Documentation
- [ ] `VERSION_HISTORY.md` updated with v1.1.0 details
- [ ] `CHANGELOG.md` created/updated
- [ ] Release notes prepared (English + Vietnamese)
- [ ] Screenshots prepared (see below)
- [ ] Privacy policy reviewed (if data collection changed)

---

## 📊 Version & Build Management

### Current Version Information
- **Version:** 1.1.0 (Sprint 9)
- **Build Number:** 6
- **Branch:** feature/001-sexagenary-cycle-complete
- **Target iOS:** 15.0+
- **Bundle ID:** com.huynguyen.lunarcalendar

### Step 1: Update Version Numbers

**File:** `src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj`

```xml
<!-- Update these values -->
<ApplicationDisplayVersion>1.1.0</ApplicationDisplayVersion>
<ApplicationVersion>6</ApplicationVersion>
```

**Versioning Rules (SemVer):**
- **MAJOR.MINOR.PATCH** format
- **MAJOR (1.x.x):** Breaking changes, major redesign
- **MINOR (x.1.x):** New features (Sprint 9 = 1.1.0)
- **PATCH (x.x.1):** Bug fixes only
- **Build Number:** Always increments (1, 2, 3...)

### Step 2: Update Info.plist (if needed)

**File:** `src/LunarCalendar.MobileApp/Platforms/iOS/Info.plist`

Verify these keys match:
```xml
<key>CFBundleShortVersionString</key>
<string>1.1.0</string>
<key>CFBundleVersion</key>
<string>6</string>
```

**Note:** .NET MAUI should sync these automatically from .csproj

### Step 3: Git Tag the Release

```bash
# Create annotated tag
git tag -a v1.1.0 -m "Release v1.1.0 - Sprint 9: Sexagenary Cycle"

# Push tag to remote
git push origin v1.1.0

# Verify tag
git describe --tags
```

---

## 🧪 Testing & Quality Assurance

### Pre-Build Testing (Day -3 to -1)

#### Unit Tests
```bash
# Navigate to test project
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Run all unit tests
dotnet test tests/LunarCalendar.Core.Tests/LunarCalendar.Core.Tests.csproj --logger "console;verbosity=detailed"

# Expected: 108 tests passed, 0 failed
```

#### Build Validation
```bash
# Clean build iOS Release
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -c Release -f net10.0-ios

# Build iOS Release (local verification)
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -c Release \
  -f net10.0-ios \
  -p:RuntimeIdentifier=ios-arm64

# Expected: 0 errors, 0 warnings
```

#### Simulator Testing
```bash
# Deploy to iPhone 16 Pro Max (largest screen)
bash scripts/deploy-iphone-simulator.sh

# Test checklist:
# ✅ App launches without crash
# ✅ Calendar displays correctly
# ✅ Tap date → shows Sexagenary info (if enabled)
# ✅ Settings → toggle "Show Sexagenary Cycle"
# ✅ Switch languages (EN/VI/ZH)
# ✅ Dark mode / Light mode
# ✅ Holiday list displays
# ✅ Year picker works
```

#### Device Testing (CRITICAL)
```bash
# Deploy to physical iPhone
bash scripts/deploy-ios-device.sh

# Test on actual hardware:
# ✅ Performance (no lag)
# ✅ Memory usage (check Xcode Instruments)
# ✅ Battery impact (background monitoring)
# ✅ Haptic feedback works
# ✅ Network (offline mode)
```

---

## 🏗️ Build Process

### Step 1: Pre-Build Preparation

#### Clean Workspace
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Remove all build artifacts
rm -rf src/LunarCalendar.MobileApp/bin/Release
rm -rf src/LunarCalendar.MobileApp/obj/Release

# Clean .NET build cache
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -c Release
```

#### Verify Certificates & Provisioning
```bash
# Check Apple Distribution certificate
security find-identity -v -p codesigning | grep "Apple Distribution"

# Expected output:
# 1) ABC123... "Apple Distribution: Huy Nguyen (TEAM_ID)"

# Verify provisioning profile exists
ls -lh releases/Lunar_Calendar_App_Store.mobileprovision

# Expected: File exists, not expired
```

#### Check Provisioning Profile Details
```bash
# Extract profile information
security cms -D -i releases/Lunar_Calendar_App_Store.mobileprovision | plutil -p -

# Verify:
# ✅ Bundle ID matches: com.huynguyen.lunarcalendar
# ✅ Team ID matches your Apple Developer Team
# ✅ Not expired (check CreationDate and ExpirationDate)
# ✅ Certificate matches your Distribution cert
```

### Step 2: Build App Store IPA

#### Automated Build Script
```bash
# Run build script
bash scripts/build-ios-appstore.sh
```

**What the script does:**
1. Verifies prerequisites (certs, profiles)
2. Installs provisioning profile
3. Cleans previous builds
4. Builds Release configuration for ios-arm64
5. Creates IPA with App Store code signing
6. Outputs IPA location and size

#### Expected Output
```
🍎 Building iOS Release for App Store Distribution
==================================================
✅ Apple Distribution certificate found
✅ Installed provisioning profile
🔨 Building App Store IPA...
✅ App Store IPA build completed successfully!

📦 IPA Details:
-rw-r--r--  1 user  staff   45M Jan 30 10:30 LunarCalendar.MobileApp.ipa

📍 IPA Location:
/Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar/src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/LunarCalendar.MobileApp.ipa

🚀 Ready to upload to App Store Connect!
```

#### Manual Build (if script fails)
```bash
dotnet publish src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net10.0-ios \
  -c Release \
  -r ios-arm64 \
  -p:ArchiveOnBuild=true \
  -p:BuildIpa=true \
  -p:CodesignKey="Apple Distribution" \
  -p:CodesignProvision="Lunar Calendar App Store" \
  -p:EnableCodeSigning=true
```

### Step 3: Validate IPA

#### Check IPA Contents
```bash
# Extract and inspect IPA
IPA_PATH="src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/LunarCalendar.MobileApp.ipa"

# Create temp directory
mkdir -p /tmp/ipa-validation
cd /tmp/ipa-validation

# Unzip IPA (IPA is just a zip file)
unzip "$IPA_PATH"

# Verify app structure
ls -R Payload/

# Check Info.plist
plutil -p Payload/*.app/Info.plist | grep -E "CFBundleVersion|CFBundleShortVersionString"

# Expected:
# "CFBundleShortVersionString" => "1.1.0"
# "CFBundleVersion" => "6"
```

#### Validate with Xcode (Recommended)
```bash
# Open Xcode Organizer
open -a "Xcode" ~/Library/Developer/Xcode/Archives

# OR use xcodebuild to validate
xcrun altool --validate-app \
  -f "$IPA_PATH" \
  -t ios \
  --apiKey "YOUR_API_KEY" \
  --apiIssuer "YOUR_ISSUER_ID"
```

---

## 🚀 App Store Connect Submission

### Step 1: Prepare App Store Connect

#### Login to App Store Connect
1. Go to: https://appstoreconnect.apple.com
2. Navigate to **My Apps**
3. Select **Lunar Calendar** (or create new app if first release)

#### Create New Version
1. Click **+ Version or Platform** → **iOS**
2. Enter version: **1.1.0**
3. Click **Create**

### Step 2: Upload IPA to App Store Connect

#### Method A: Using Xcode (Recommended)
```bash
# Find IPA
IPA_PATH="src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/LunarCalendar.MobileApp.ipa"

# Upload using Xcode's Transporter
xcrun altool --upload-app \
  -f "$IPA_PATH" \
  -t ios \
  --apiKey "YOUR_API_KEY" \
  --apiIssuer "YOUR_ISSUER_ID"
```

**Get API Key & Issuer:**
1. App Store Connect → Users and Access → Keys
2. Generate new key (select **App Manager** role)
3. Download `.p8` file and note Key ID + Issuer ID

#### Method B: Using Transporter App (GUI)
1. Open **Transporter** app (comes with Xcode)
2. Sign in with Apple ID
3. Click **+** and select IPA file
4. Click **Deliver**
5. Wait for upload (5-15 minutes depending on size/speed)

#### Method C: Using Xcode Organizer
1. Open Xcode
2. Window → Organizer
3. Archives tab
4. Select latest archive
5. Click **Distribute App**
6. Choose **App Store Connect**
7. Follow prompts and click **Upload**

### Step 3: Configure App Store Listing

#### App Information
- **Name:** Vietnamese Lunar Calendar
- **Subtitle (30 chars):** Traditional Calendar & Can Chi
- **Category:** Primary: **Utilities** | Secondary: **Reference**
- **Content Rights:** You own all rights

#### Version Information (v1.1.0)

##### What's New in This Version (English)
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

##### What's New (Vietnamese)
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

##### Promotional Text (Optional - can update anytime)
```
🆕 NEW: Traditional Sexagenary Cycle (Can Chi / 干支) with Five Elements color coding! Experience ancient Asian wisdom in a modern app.
```

#### Screenshots & App Preview

##### Required Screenshot Sizes
1. **6.7" Display (iPhone 14 Pro Max, 15 Pro Max, 16 Pro Max)**
   - Resolution: **1290 x 2796 pixels**
   - Minimum: 3-10 screenshots

2. **5.5" Display (iPhone 8 Plus, 7 Plus, 6s Plus)**
   - Resolution: **1242 x 2208 pixels**
   - Minimum: 3-10 screenshots

3. **iPad Pro (12.9-inch) (3rd gen+)**
   - Resolution: **2048 x 2732 pixels**
   - Minimum: 3-10 screenshots

##### Screenshot Capture Process
```bash
# Launch iPhone 16 Pro Max simulator
xcrun simctl boot "iPhone 16 Pro Max"
bash scripts/deploy-iphone-simulator.sh

# Take screenshots (Cmd+S in Simulator)
# OR use command line:
xcrun simctl io booted screenshot ~/Desktop/screenshot1.png

# Recommended screenshots:
# 1. Main calendar view with Sexagenary info visible
# 2. Calendar with date selected showing Can Chi
# 3. Settings page with "Show Sexagenary Cycle" toggle
# 4. Holiday list view
# 5. Year picker
# 6. Dark mode calendar view
```

##### Screenshot Checklist
- [ ] **Screenshot 1:** Main Calendar (light mode) - show Can Chi below dates with colors
- [ ] **Screenshot 2:** Calendar with today highlighted - zoom to show detail
- [ ] **Screenshot 3:** Settings page - show "Show Sexagenary Cycle" toggle ON
- [ ] **Screenshot 4:** Holiday list view - show Vietnamese holidays
- [ ] **Screenshot 5:** Year picker - show year selection UI
- [ ] **Screenshot 6:** Dark mode calendar - show Can Chi in dark theme
- [ ] All screenshots have correct dimensions (use Preview to resize if needed)
- [ ] No personal data visible in screenshots
- [ ] Status bar cleaned (use Xcode Simulator settings)

##### Optional: App Preview Video
- 15-30 seconds showing key features
- Portrait orientation
- Same resolutions as screenshots
- Show: Calendar → Tap date → Sexagenary info → Settings toggle

#### App Description

##### Short Description (English)
```
Traditional Vietnamese Lunar Calendar with Sexagenary Cycle (Can Chi / 干支). Track lunar dates, holidays, and connect with ancient Asian timekeeping. Fully offline, no ads.
```

##### Full Description (English)
```
🌙 Vietnamese Lunar Calendar - Ancient Wisdom, Modern Design

The most comprehensive Vietnamese Lunar Calendar app featuring traditional Sexagenary Cycle (Can Chi / 干支) with Five Elements color coding.

✨ KEY FEATURES

📅 LUNAR CALENDAR
• Accurate Vietnamese lunar date calculations
• Side-by-side solar and lunar dates
• Vietnamese traditional holidays
• Festival and special day markers

🎴 SEXAGENARY CYCLE (CAN CHI / 干支) - NEW!
• Traditional 60-year Chinese cycle
• Heavenly Stems (Thiên Can / 天干)
• Earthly Branches (Địa Chi / 地支)
• Zodiac animal for each year
• Five Elements (Ngũ Hành / 五行) color coding:
  🟢 Wood (Mộc) - Growth & Expansion
  🔴 Fire (Hỏa) - Energy & Passion
  🟤 Earth (Thổ) - Stability & Balance
  ⚪ Metal (Kim) - Strength & Structure
  🔵 Water (Thủy) - Flow & Adaptability

🌍 MULTI-LANGUAGE SUPPORT
• Vietnamese (Tiếng Việt)
• English
• Chinese (中文) characters
• Pinyin transliteration

⚙️ CUSTOMIZATION
• Toggle Sexagenary Cycle display on/off
• Dark mode and light mode
• Responsive design for all screen sizes

🔒 PRIVACY FOCUSED
• 100% offline - no internet required
• No ads, no tracking, no data collection
• Your calendar stays on your device

📱 OPTIMIZED FOR iOS
• Native iOS performance
• Support for iPhone SE to iPhone 16 Pro Max
• iPad optimization
• iOS 15.0 and later

Perfect for anyone interested in Vietnamese culture, Chinese astrology, traditional calendars, or connecting with ancestral timekeeping systems.

Download now and explore thousands of years of cultural wisdom! 🎉
```

##### Keywords (100 characters max)
```
lunar,calendar,vietnamese,can chi,zodiac,astrology,holiday,tradition,offline,五行,干支
```

#### App Privacy

Review and update privacy information:
- [ ] Data Types Collected: **None** (if truly offline)
- [ ] Data Used to Track You: **None**
- [ ] Privacy Policy URL: Update if needed

### Step 4: Submit for Review

#### Pre-Submission Checklist
- [ ] Build status shows **"Ready to Submit"** (wait 10-30 min after upload)
- [ ] All required screenshots uploaded (6.7", 5.5", iPad)
- [ ] What's New text filled in
- [ ] App description updated
- [ ] Keywords set
- [ ] Support URL set (GitHub or website)
- [ ] Marketing URL set (optional)
- [ ] Privacy policy reviewed
- [ ] Age rating set (4+)
- [ ] License agreement (standard is fine)

#### Submit
1. Select build: **v1.1.0 (6)**
2. Version Release: **Automatically release after approval** (or manual)
3. Phased Release: **Enabled** (gradual rollout 1-7 days)
4. Click **Add for Review**
5. Click **Submit to App Review**

#### Review Notes for Apple (Optional but Recommended)
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
4. Tap any date to view detailed information
5. Test language switching: EN, VI, ZH

Thank you for your review!

Contact: [Your email]
```

---

## 📬 Post-Submission

### Expected Timeline

| Stage | Duration | Actions |
|-------|----------|---------|
| Processing | 10-30 min | Build processes, no action needed |
| Waiting for Review | 1-3 days | Monitor status, respond quickly if needed |
| In Review | 1-4 hours | Apple tests app |
| Pending Developer Release | 0-24 hours | Release manually or auto-release |
| App Store | Immediate | Phased rollout over 7 days |

### Monitoring

#### Check Review Status
- App Store Connect → My Apps → Lunar Calendar → iOS App → Version 1.1.0
- Status options:
  - **Waiting for Review** ⏳
  - **In Review** 🔍
  - **Pending Developer Release** ✅
  - **Ready for Sale** 🚀
  - **Rejected** ❌ (see below)

#### If Rejected
1. Read rejection reason carefully
2. Common issues:
   - Incomplete information
   - Crashed during review (rare if tested well)
   - Misleading screenshots
   - Privacy issues
3. Fix issues and resubmit
4. Respond in Resolution Center if clarification needed

### Release Day Actions

#### Day 0: Approval Received
- [ ] Celebrate! 🎉
- [ ] Monitor crash reports (Xcode Organizer → Crashes)
- [ ] Check App Store reviews
- [ ] Announce release (social media, blog, etc.)

#### Day 1-7: Phased Rollout
- [ ] Monitor analytics (App Store Connect → Analytics)
- [ ] Check crash rate (should be <0.1%)
- [ ] Respond to user reviews (first 24 hours is critical)
- [ ] Monitor App Store ratings

#### Week 1: Post-Release
- [ ] Compile user feedback
- [ ] Create bug fix list (if any)
- [ ] Plan next sprint based on feedback
- [ ] Update internal roadmap

---

## 🔄 Rollback Procedures

### Scenario 1: Critical Bug Found Before Approval

**Action:** Remove from review
```
1. App Store Connect → My Apps → Lunar Calendar
2. Click "Remove from Review"
3. Fix bug
4. Increment build number (6 → 7)
5. Rebuild IPA
6. Upload new build
7. Submit again
```

### Scenario 2: Critical Bug Found After Release (Live on Store)

**Action:** Release hotfix immediately
```
1. Create hotfix branch: git checkout -b hotfix/v1.1.1
2. Fix critical bug
3. Update version to 1.1.1 (Build 7)
4. Run all tests
5. Build IPA with expedited review request
6. Submit to App Store Connect
7. Request Expedited Review:
   - App Store Connect → Version 1.1.1
   - Click "Request Expedited Review"
   - Explain: "Critical crash affecting X% of users"
8. Expected approval: 2-12 hours
```

**Expedited Review Criteria:**
- Critical crash/bug affecting many users
- Security vulnerability
- Essential bug fix for current release
- Note: Only use if truly critical (Apple limits expedited reviews)

### Scenario 3: User Reports Major Issue, But Not Critical

**Action:** Plan patch release
```
1. Triage issue severity
2. If affects <5% users and non-blocking: schedule for next sprint
3. If affects >5% or blocks major feature: plan 1.1.1 patch
4. Standard review process (1-3 days)
```

---

## ✅ Sprint 9 Specific Checklist

### Pre-Build Verification

#### Code Completeness
- [x] Sexagenary calculator implemented (`SexagenaryCalculator.cs`)
- [x] Sexagenary service with caching (`SexagenaryService.cs`)
- [x] Five Elements enum and colors (`FiveElement.cs`)
- [x] Heavenly Stems and Earthly Branches enums
- [x] Zodiac animal mapping
- [x] Settings toggle for "Show Sexagenary Cycle"
- [x] Calendar integration (display Can Chi below dates)
- [x] Multi-language support (EN/VI/ZH)
- [ ] Date detail page integration (DEFERRED to v1.2.0)

#### Testing Results
- [x] Unit tests: 108/108 passing ✅
- [x] Historical validation: 36/36 passing ✅
- [x] iOS build: 0 errors ✅
- [ ] Simulator testing completed
- [ ] Physical device testing completed
- [ ] iPad testing completed
- [ ] Dark mode testing completed
- [ ] Memory leak testing completed

#### Sprint 9 Features Showcase

**Screenshots Must Show:**
1. ✅ Calendar with Can Chi visible (main feature)
2. ✅ Five Elements color coding (Wood/Fire/Earth/Metal/Water)
3. ✅ Settings toggle "Show Sexagenary Cycle"
4. ✅ Multi-language support (show EN or VI or ZH)
5. ✅ Dark mode compatibility

#### Release Notes Verification

Ensure release notes mention:
- ✅ "Sexagenary Cycle (Can Chi / 干支)" as headline feature
- ✅ Five Elements color coding
- ✅ Multi-language support
- ✅ Settings toggle
- ✅ "Connect with ancient Asian timekeeping traditions"

---

## 📞 Support & Resources

### Key Contacts
- **Developer:** Huy Nguyen
- **Email:** [Your email]
- **GitHub:** https://github.com/duchuy129/lunarcalendar

### Useful Links
- **App Store Connect:** https://appstoreconnect.apple.com
- **Apple Developer Portal:** https://developer.apple.com/account
- **App Store Review Guidelines:** https://developer.apple.com/app-store/review/guidelines/
- **Human Interface Guidelines:** https://developer.apple.com/design/human-interface-guidelines/

### Common Issues & Solutions

#### Issue: "No provisioning profile found"
**Solution:**
```bash
# Re-install provisioning profile
security cms -D -i releases/Lunar_Calendar_App_Store.mobileprovision | plutil -p -
PROFILE_UUID=$(security cms -D -i releases/Lunar_Calendar_App_Store.mobileprovision | plutil -extract UUID raw -)
cp releases/Lunar_Calendar_App_Store.mobileprovision "$HOME/Library/MobileDevice/Provisioning Profiles/$PROFILE_UUID.mobileprovision"
```

#### Issue: "Certificate not trusted"
**Solution:**
```bash
# Check certificate validity
security find-identity -v -p codesigning

# If missing, download from Apple Developer Portal
# Install by double-clicking .cer file
```

#### Issue: "Build failed with code signing error"
**Solution:**
1. Clean build: `dotnet clean -c Release`
2. Remove DerivedData: `rm -rf ~/Library/Developer/Xcode/DerivedData`
3. Verify Bundle ID matches profile
4. Rebuild

#### Issue: "IPA upload rejected - ITMS-90xxx error"
**Solution:**
- Check error code: https://developer.apple.com/library/archive/documentation/iTunes/Conceptual/iTunesConnect_Guide/Appendices/AppStoreLimits.html
- Common fixes:
  - ITMS-90046: Invalid code signing
  - ITMS-90161: Invalid provisioning profile
  - ITMS-90171: Invalid bundle structure

---

## 📚 Appendix

### A. Version History Reference

| Version | Build | Date | Features |
|---------|-------|------|----------|
| 1.0.0 | 1 | 2025-12 | MVP Launch |
| 1.0.1 | 5 | 2026-01 | Bug fixes |
| **1.1.0** | **6** | **2026-01-30** | **Sprint 9: Sexagenary Cycle** |
| 1.2.0 | TBD | 2026-02 | Date Detail Page (planned) |

### B. Build Script Location

All deployment scripts located in: `/scripts/`

- `build-ios-appstore.sh` - App Store IPA build
- `build-ios-release.sh` - Ad-hoc release build
- `deploy-ios-device.sh` - Deploy to physical device
- `deploy-iphone-simulator.sh` - Deploy to simulator
- `capture-ios-crash-log.sh` - Capture crash logs

### C. Quality Metrics (Sprint 9)

| Metric | Target | Actual | Status |
|--------|--------|--------|--------|
| Unit Tests | >90% | 108/108 (100%) | ✅ |
| Historical Validation | 100% | 36/36 (100%) | ✅ |
| Code Coverage | >80% | 97% | ✅ |
| Build Errors | 0 | 0 | ✅ |
| Build Warnings | <10 | 0 | ✅ |
| Crash-Free Rate | >99.5% | TBD | ⏳ |
| App Store Rating | >4.0 | TBD | ⏳ |

### D. Emergency Contacts

**Critical Issues (Production Down):**
- Developer: [Your phone/email]
- Apple Support: https://developer.apple.com/contact/

**Non-Critical Issues:**
- GitHub Issues: https://github.com/duchuy129/lunarcalendar/issues
- Email: [Your email]

---

## ✍️ Sign-Off

### Deployment Approval

- [ ] **Developer:** Code complete, all tests passing
- [ ] **QA:** Testing complete, no critical bugs
- [ ] **Product Owner:** Features meet requirements
- [ ] **Release Manager:** Ready to deploy

**Approved by:** ________________  
**Date:** ______________________

---

**Document End**

*This runbook should be reviewed and updated after each release to incorporate lessons learned and process improvements.*
