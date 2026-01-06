# Vietnamese Lunar Calendar - MVP Launch Checklist

**Target Launch Date:** January 20, 2026  
**Strategy:** Direct production release (no beta testing)  
**MVP Scope:** Core calendar + holidays only

---

## 🎯 Pre-Launch Philosophy

For a simple calendar app MVP:
- ✅ **Beta testing NOT required** - Calendar logic is deterministic (no user-generated content)
- ✅ **Launch fast, iterate faster** - Get real users immediately
- ✅ **Internal testing sufficient** - You + 2-3 friends/family can validate
- ⚠️ **Have rollback plan** - Can pull from stores if critical issues

---

## Week 1: Final Development (Jan 3-9)

### Day 1-2: Code Freeze & Feature Lock
- [ ] **Feature freeze** - No new features, bug fixes only
- [ ] Review all open TODOs in code (`grep -r "TODO" src/`)
- [ ] Remove all debug/console logging statements
- [ ] Remove test/dummy data
- [ ] Verify all placeholder text is localized
- [ ] Check all hardcoded strings moved to resources

**Critical Files to Review:**
```bash
# Remove debug code
grep -r "Console.WriteLine" src/
grep -r "Debug.WriteLine" src/
grep -r "// TODO" src/
grep -r "// FIXME" src/
```

### Day 3-4: Device Testing
- [ ] **iOS Testing:**
  - [ ] iPhone SE (smallest screen)
  - [ ] iPhone 14/15 (standard)
  - [ ] iPhone 15 Pro Max (largest)
  - [ ] iPad (if supported)
  - [ ] iOS 15, 16, 17, 18 compatibility

- [ ] **Android Testing:**
  - [ ] Small phone (5.5" screen)
  - [ ] Standard phone (6.1" - 6.5")
  - [ ] Large phone (6.7"+)
  - [ ] Tablet (if supported)
  - [ ] Android 10, 11, 12, 13, 14 compatibility

**Test Checklist (Each Device):**
```
✓ App launches successfully
✓ Today's date shows correct lunar date
✓ Calendar navigation (prev/next month)
✓ Year picker works
✓ Month picker works
✓ Holidays display with correct colors
✓ Holiday details open correctly
✓ Year holidays list loads
✓ Language switch (Vietnamese ↔ English)
✓ Settings persist after app restart
✓ No crashes after 5 minutes of use
✓ Rotation works (portrait/landscape)
✓ Dark mode works (if supported)
```

### Day 5: Edge Case Testing
- [ ] **Leap month handling** (test with dates that have leap months)
- [ ] **Year boundaries** (Dec 31 → Jan 1, lunar year transitions)
- [ ] **Tet holidays** (verify 2026 Tet dates: Feb 17-19)
- [ ] **Special dates:**
  - [ ] Jan 1, 2026 (New Year)
  - [ ] Feb 17, 2026 (Tet - Lunar New Year)
  - [ ] Apr 30, 2026 (Reunification Day)
  - [ ] Sep 2, 2026 (National Day)
- [ ] **Offline functionality** (airplane mode, no internet)
- [ ] **First launch experience** (clean install)
- [ ] **Memory usage** (should not exceed 100MB for calendar app)

### Day 6-7: Performance & Polish
- [ ] **Performance:**
  - [ ] Calendar loads in < 1 second
  - [ ] Month navigation is instant
  - [ ] Year picker scrolls smoothly
  - [ ] No lag when switching languages
  
- [ ] **UI Polish:**
  - [ ] All buttons have proper tap feedback
  - [ ] Icons are crisp on all screen densities
  - [ ] Colors match design (check COLOR_PALETTE_GUIDE.md)
  - [ ] Spacing/padding looks professional
  - [ ] No text cutoff or overflow

- [ ] **Final Code Review:**
  - [ ] Run code analyzer: `dotnet build --configuration Release`
  - [ ] Fix all warnings
  - [ ] Check app size (should be < 50MB iOS, < 30MB Android)

---

## Week 2: App Store Preparation (Jan 10-16)

### App Store Assets

#### iOS App Store (Required)
- [ ] **App Icon** (1024x1024px PNG, no transparency, no rounded corners)
- [ ] **Screenshots** (iPhone 6.7" - required, iPhone 5.5" - recommended):
  - [ ] 1. Calendar view showing today with lunar date
  - [ ] 2. Calendar with holidays highlighted
  - [ ] 3. Holiday details screen (Tet)
  - [ ] 4. Year holidays list
  - [ ] 5. Settings screen (optional)
  
- [ ] **App Preview Video** (optional, but recommended):
  - [ ] 15-30 seconds showing core navigation
  - [ ] Portrait orientation
  - [ ] No audio needed for calendar app

#### Google Play Store (Required)
- [ ] **App Icon** (512x512px PNG)
- [ ] **Feature Graphic** (1024x500px) - Banner for store listing
- [ ] **Screenshots** (Minimum 2, recommended 8):
  - [ ] Phone screenshots (1080x1920 or higher)
  - [ ] Tablet screenshots (1536x2048 or higher) - if supported
  - Same screens as iOS

- [ ] **Promotional Video** (optional):
  - [ ] YouTube link, 30s-2min

#### Both Stores
- [ ] **App Description** (write compelling copy):
  ```
  Title (30 chars): Vietnamese Lunar Calendar
  Subtitle (30 chars): Track Tet & Holidays
  
  Description:
  - What: Vietnamese lunar calendar with holidays
  - Why: Never miss Tet or important dates
  - How: Simple, offline, bilingual
  - Features: (bullet list)
  ```

### Store Listing Content

#### App Name & Description

**App Name:**
```
Vietnamese Lunar Calendar
```

**Subtitle/Short Description (iOS/Android):**
```
Track Vietnamese holidays and lunar dates
```

**Full Description (template):**
```markdown
Vietnamese Lunar Calendar - Never Miss Tet Again

Perfect for Vietnamese diaspora and anyone celebrating Vietnamese culture!

FEATURES:
• 📅 Dual Calendar Display - See both Gregorian and lunar dates
• 🎊 Vietnamese Holidays - All major holidays (Tet, Hung Kings, National Day)
• 🌙 Lunar Special Days - Track Mùng 1 and Rằm (1st and 15th of each month)
• 🐉 Zodiac Animals - See the year's zodiac animal for each date
• 🌍 Bilingual - Full support for Vietnamese and English
• ✈️ Works Offline - No internet required

HOLIDAYS INCLUDED:
• Tết Nguyên Đán (Lunar New Year)
• Giỗ Tổ Hùng Vương (Hung Kings Festival)
• Tết Đoan Ngọ (Dragon Boat Festival)
• Tết Trung Thu (Mid-Autumn Festival)
• And 40+ more traditional festivals

PERFECT FOR:
• Planning celebrations around lunar dates
• Teaching children about Vietnamese culture
• Coordinating with family in Vietnam
• Never forgetting important traditional days

100% FREE. NO ADS. NO DATA COLLECTION.

Made with ❤️ for the Vietnamese community worldwide.
```

**Keywords (iOS - 100 chars max):**
```
lunar calendar,vietnamese,tet,holidays,zodiac,moon,festival,culture
```

**Categories:**
- iOS: Lifestyle, Utilities
- Android: Lifestyle, Tools

### App Store Configuration

#### iOS App Store Connect
- [ ] **App Information:**
  - [ ] Bundle ID: `com.huynguyen.lunarcalendar` (verify matches project)
  - [ ] SKU: `lunarcalendar-ios-2026`
  - [ ] Primary Category: Lifestyle
  - [ ] Secondary Category: Utilities
  
- [ ] **Pricing:**
  - [ ] Free (recommended for MVP)
  - [ ] Available in all territories
  
- [ ] **Age Rating:**
  - [ ] 4+ (no objectionable content)
  
- [ ] **App Privacy:**
  - [ ] Does app collect data? **NO**
  - [ ] Privacy policy URL: Optional (not required if no data collection)
  
- [ ] **App Review Information:**
  - [ ] Contact email: [your-email]
  - [ ] Contact phone: [your-phone]
  - [ ] Demo account: Not needed
  - [ ] Notes: "Simple calendar app, no user accounts or data collection"

#### Google Play Console
- [ ] **App Information:**
  - [ ] Application ID: `com.huynguyen.lunarcalendar`
  - [ ] Category: Lifestyle
  
- [ ] **Pricing:**
  - [ ] Free
  - [ ] Distribute in all countries
  
- [ ] **Content Rating:**
  - [ ] Complete questionnaire (should get EVERYONE rating)
  
- [ ] **App Content:**
  - [ ] Does app have ads? **NO**
  - [ ] Does app have in-app purchases? **NO**
  - [ ] Target audience: All ages
  
- [ ] **Data Safety:**
  - [ ] Does app collect data? **NO**
  - [ ] Does app share data? **NO**
  - [ ] (This is huge selling point - emphasize in listing!)

### Legal & Compliance
- [ ] **Privacy Policy:** Not required if no data collection, but recommended
- [ ] **Terms of Service:** Optional for simple app
- [ ] **Copyright:** © 2026 Huy Nguyen (or your name)
- [ ] **Export Compliance:** Calendar app = No encryption = No export issues

---

## Week 3: Build & Submit (Jan 17-20)

### iOS Submission

#### Day 1: Final Build
```bash
# Clean build
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Update version
# Edit src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj
# Set: <ApplicationDisplayVersion>1.0.0</ApplicationDisplayVersion>
# Set: <ApplicationVersion>1</ApplicationVersion>

# Build iOS release
./build-ios-release.sh

# Verify IPA created
ls -lh src/LunarCalendar.MobileApp/bin/Release/net8.0-ios/ios-arm64/publish/*.ipa
```

**Pre-Submission Checklist:**
- [ ] Version: 1.0.0 (1)
- [ ] Bundle ID matches App Store Connect
- [ ] Provisioning profile is Distribution (not Development)
- [ ] Archive builds successfully
- [ ] IPA size is reasonable (< 100MB)

#### Day 2: Upload to App Store
```bash
# Option 1: Xcode (recommended)
# 1. Open .xcarchive in Xcode
# 2. Window > Organizer
# 3. Select archive > Distribute App
# 4. App Store Connect > Upload

# Option 2: Transporter app
# 1. Download Transporter from Mac App Store
# 2. Drag .ipa file
# 3. Deliver to App Store

# Option 3: Command line
xcrun altool --upload-app --type ios --file "path/to/app.ipa" \
  --username "your-apple-id" --password "@keychain:AC_PASSWORD"
```

**After Upload:**
- [ ] Wait 10-30 minutes for processing
- [ ] Refresh App Store Connect
- [ ] Select build for version
- [ ] Add What's New: "Initial release - Vietnamese Lunar Calendar with holidays"
- [ ] Click **Submit for Review**

**iOS Review Timeline:** 24-48 hours typically

---

### Android Submission

#### Day 1: Final Build
```bash
# Clean build
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Update version
# Edit src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj
# Set: <ApplicationDisplayVersion>1.0.0</ApplicationDisplayVersion>
# Set: <ApplicationVersion>1</ApplicationVersion>

# Build Android release
./build-android-release.sh

# Verify AAB/APK created
ls -lh src/LunarCalendar.MobileApp/bin/Release/net8.0-android/publish/*.aab
```

**Pre-Submission Checklist:**
- [ ] Version: 1.0.0 (versionCode: 1)
- [ ] Package name matches Play Console: `com.huynguyen.lunarcalendar`
- [ ] APK/AAB signed with release keystore
- [ ] File size reasonable (< 50MB)

#### Day 2: Upload to Google Play
1. **Go to Play Console** → Your App → Production
2. **Create New Release:**
   - [ ] Upload AAB (recommended) or APK
   - [ ] Release name: "1.0.0 - Initial Release"
   - [ ] Release notes:
     ```
     Vietnamese Lunar Calendar - Initial Release
     
     Features:
     • Dual calendar display (Gregorian + Lunar)
     • Vietnamese holidays and festivals
     • Bilingual (Vietnamese/English)
     • Works completely offline
     • No ads, no data collection
     
     Perfect for tracking Tet and traditional Vietnamese dates!
     ```
   
3. **Review and Rollout:**
   - [ ] Click "Review Release"
   - [ ] Fix any warnings (most can be ignored for MVP)
   - [ ] Click "Start rollout to Production"

**Android Review Timeline:** 1-7 days (usually 1-2 days for simple apps)

---

## Week 3 (Continued): Launch Day Prep

### Day 3-4: While Waiting for Review

#### Set Up Analytics (Optional but Recommended)
Even without user accounts, basic analytics help:

**Option A: Microsoft App Center (Free)**
```bash
# Add to project (optional)
dotnet add package Microsoft.AppCenter.Analytics
dotnet add package Microsoft.AppCenter.Crashes
```

Track:
- App launches
- Language preference
- Crashes (critical for MVP)

**Option B: Google Analytics for Firebase (Free)**
- More comprehensive but more setup

**Option C: No analytics** (valid for MVP)
- Launch without analytics
- Add in v1.1 based on feedback

#### Prepare Support Channels
- [ ] **Email:** Set up support email (e.g., support@yourdomain.com or personal email)
- [ ] **GitHub:** Enable Issues tab for bug reports
- [ ] **FAQ:** Create simple FAQ in README.md
  - How to change language?
  - What holidays are included?
  - Does it work offline? (YES)
  - Does it collect data? (NO)

#### Marketing Materials (Optional)
- [ ] **Landing page:** Simple one-pager (optional, can skip for MVP)
- [ ] **Social media:** Prepare announcement posts
  - Vietnamese: "Lịch âm dương Việt Nam - Ứng dụng miễn phí, không quảng cáo!"
  - English: "Vietnamese Lunar Calendar - Free, no ads, works offline!"
- [ ] **Reddit:** r/VietNam, r/Vietnamese (be genuine, not spammy)
- [ ] **Facebook groups:** Vietnamese diaspora groups (ask permission first)

---

## Launch Day & Week 1 Post-Launch

### Launch Day Tasks

#### When iOS Approved:
- [ ] Verify app appears in App Store
- [ ] Test download on real device (not TestFlight)
- [ ] Search works: "Vietnamese lunar calendar"
- [ ] Screenshots display correctly
- [ ] Description reads well
- [ ] Share App Store link:
  ```
  https://apps.apple.com/app/idXXXXXXXXX
  ```

#### When Android Approved:
- [ ] Verify app appears in Play Store
- [ ] Test download on real device
- [ ] Search works: "Vietnamese lunar calendar"
- [ ] Feature graphic displays
- [ ] Share Play Store link:
  ```
  https://play.google.com/store/apps/details?id=com.huynguyen.lunarcalendar
  ```

### Announcement Template

**For friends/family/social media:**
```
🎊 Launched my first app! 🎊

Vietnamese Lunar Calendar - Track Tet and traditional holidays

✨ Features:
• Dual calendar (Solar + Lunar)
• Vietnamese holidays
• Works offline
• Bilingual
• 100% FREE, no ads

Perfect for Vietnamese families worldwide! 🇻🇳

iOS: [link]
Android: [link]

Would love your feedback! 🙏
```

### Week 1 Monitoring (Critical!)

**Daily Checks (First 7 Days):**
- [ ] Check crash reports (App Store Connect / Play Console)
- [ ] Monitor ratings/reviews
- [ ] Respond to reviews within 24 hours
- [ ] Check download numbers
- [ ] Monitor support email

**Critical Metrics:**
- Downloads: Any downloads = success for MVP!
- Crashes: Should be < 1% crash rate
- Ratings: Aim for 4+ stars
- Reviews: Read ALL feedback carefully

**Common Issues to Watch:**
- ❌ Calendar dates off by one day → Time zone issue
- ❌ App crashes on specific date → Edge case in lunar calculation
- ❌ Holidays wrong → Data error
- ❌ Language doesn't switch → Localization bug
- ❌ UI elements overlap → Screen size issue

### Rapid Response Plan

**If Critical Bug Found (Crashes, Wrong Dates):**
1. **Confirm:** Reproduce on your device
2. **Fix:** Make minimal code change
3. **Test:** Verify fix on multiple devices
4. **Submit:** Rush update (can take 24-48 hours iOS, 1-2 hours Android)
5. **Communicate:** Update app description with "Bug fix in progress"

**Update Versioning:**
- Critical bug: 1.0.0 → 1.0.1 (hotfix)
- Minor bug: Wait for 1.1.0 (scheduled update)

---

## Success Criteria (MVP)

### Minimum Success (Week 1):
- [ ] 10+ downloads (iOS + Android combined)
- [ ] 0 crashes
- [ ] 1+ positive review or feedback
- [ ] No critical bugs reported

### Good Success (Week 1):
- [ ] 50+ downloads
- [ ] < 1% crash rate
- [ ] 4+ star rating
- [ ] Multiple positive reviews

### Great Success (Week 1):
- [ ] 100+ downloads
- [ ] 4.5+ star rating
- [ ] Featured in local community groups
- [ ] Organic sharing by users

**Remember:** ANY downloads = validation that people want this!

---

## Post-MVP Roadmap (Based on Feedback)

### Week 2-4: Collect Feedback
- Monitor reviews, emails, GitHub issues
- Identify top 3 requested features
- Track most common complaints

### Week 5-8: Version 1.1 Planning
Based on feedback, prioritize:
- [ ] Most requested feature (e.g., widgets, notifications)
- [ ] Critical bug fixes
- [ ] UI polish based on user screenshots

### Month 3+: Decision Point
Now you have REAL DATA to decide:
- Continue with MAUI + iterate?
- Migrate to Flutter? (with proven features)
- Build different app with Flutter?

---

## Quick Reference: Commands

### Build Commands
```bash
# iOS Release
./build-ios-release.sh

# Android Release  
./build-android-release.sh

# Check version
grep -A2 "ApplicationDisplayVersion" src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj
```

### Testing Commands
```bash
# Find debug code
grep -r "Console.WriteLine" src/
grep -r "TODO" src/

# Check app size
ls -lh src/LunarCalendar.MobileApp/bin/Release/net8.0-ios/ios-arm64/publish/*.ipa
ls -lh src/LunarCalendar.MobileApp/bin/Release/net8.0-android/publish/*.aab

# Run analyzer
dotnet build --configuration Release
```

### Deployment Commands
```bash
# iOS: Use Xcode Organizer or Transporter (GUI recommended)

# Android: Upload via Play Console (GUI recommended)
```

---

## Emergency Contacts & Resources

### App Store Support
- **iOS:** https://developer.apple.com/contact/
- **Android:** https://support.google.com/googleplay/android-developer

### Documentation
- App Store Review Guidelines: https://developer.apple.com/app-store/review/guidelines/
- Play Store Policy: https://play.google.com/about/developer-content-policy/

### Your Internal Docs
- `docs/PRODUCT_SPECIFICATION.md` - Feature reference
- `docs/TECHNICAL_ARCHITECTURE.md` - How it works
- `docs/DOTNET_MAUI_IMPLEMENTATION.md` - Code reference

---

## Final Thoughts

### Why Direct Production is OK:
1. ✅ **Deterministic app** - Calendar calculations are predictable
2. ✅ **No user data** - No risk of data loss/corruption
3. ✅ **Offline-first** - No server dependencies to fail
4. ✅ **Simple scope** - Calendar + holidays = well-defined
5. ✅ **Fast rollback** - Can remove from stores if major issue

### What Could Go Wrong (and it's OK):
- Few downloads first week → Normal for new app, give it time
- Negative review → Respond professionally, fix if valid
- Small bugs → Patch in v1.0.1
- Low ratings initially → Improve based on feedback

### Remember:
> **"Shipping is a feature."**  
> A simple app in users' hands beats a perfect app in development.

---

**Good luck with your launch! 🚀**

**Next Steps:**
1. Start Day 1 of checklist (code freeze)
2. Set target launch date: **January 20, 2026**
3. Check off items as you complete them
4. Ship it! 🎉
