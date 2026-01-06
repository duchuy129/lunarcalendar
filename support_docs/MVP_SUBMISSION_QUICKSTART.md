# MVP Submission Quick Start
**Vietnamese Lunar Calendar v1.0.1 - Fast Track to App Stores**

## 🎯 Overview
This is your fast-track guide to submitting the Vietnamese Lunar Calendar MVP to Apple App Store and Google Play Store. Follow these steps in order.

---

## ⏱️ Timeline: 2-3 Weeks Total

### Week 1: Preparation & Testing (Days 1-7)
- Physical device testing
- Create screenshots
- Set up developer accounts

### Week 2: Build & Submit (Days 8-14)
- Build production releases
- Complete store listings
- Submit for review

### Week 3: Review & Launch (Days 15-21)
- Address review feedback
- Apps go live! 🎉

---

## 📋 Step-by-Step Checklist

### Phase 1: Physical Device Testing (CRITICAL - Do First!)

#### iOS Testing
```bash
# 1. Connect iPhone via USB
# 2. Run test script
./test-ios-device.sh

# 3. Complete iOS test checklist
# See: PHYSICAL_DEVICE_TESTING_GUIDE.md
```

**Checklist:**
- [ ] App installs on real iPhone
- [ ] All features work correctly
- [ ] No crashes during 15-minute session
- [ ] Performance is acceptable
- [ ] UI looks good on device

#### Android Testing
```bash
# 1. Connect Android device via USB
# 2. Enable USB debugging
# 3. Run test script
./test-android-device.sh

# 4. Complete Android test checklist
# See: PHYSICAL_DEVICE_TESTING_GUIDE.md
```

**Checklist:**
- [ ] App installs on real Android device
- [ ] All features work correctly
- [ ] No ANR or crashes
- [ ] Performance is acceptable
- [ ] UI looks good on device

**⚠️ STOP HERE if tests fail. Fix issues before proceeding.**

---

### Phase 2: Create Store Assets

#### Take Screenshots
```bash
# iOS: iPhone 15 Pro Max simulator
xcrun simctl list devices | grep "iPhone 15 Pro Max"
xcrun simctl boot <DEVICE_ID>
# Run app, take 3-5 screenshots (Cmd+S)

# Android: Pixel 6 emulator
~/Library/Android/sdk/emulator/emulator -avd Pixel_6_API_34
# Run app, take 2-5 screenshots (Cmd+S)
```

**Checklist:**
- [ ] iOS screenshots: 3-5 images (1290 x 2796)
- [ ] Android screenshots: 2-5 images (1080 x 1920)
- [ ] Android feature graphic: 1 image (1024 x 500)

**See:** STORE_ASSETS_CHECKLIST.md for detailed requirements

#### Host Privacy Policy
```bash
# Quick GitHub Pages setup:
# 1. Push PRIVACY_POLICY.md to GitHub
git add PRIVACY_POLICY.md
git commit -m "Add privacy policy"
git push

# 2. Enable GitHub Pages in repo settings:
# Settings → Pages → Source: main branch → root
# Your URL: https://duchuy129.github.io/lunarcalendar/PRIVACY_POLICY
```

**Checklist:**
- [ ] Privacy policy hosted online
- [ ] URL accessible and works
- [ ] Save URL for store listings

---

### Phase 3: Set Up Developer Accounts

#### Apple Developer Account
1. **Enroll:** https://developer.apple.com/programs/
   - Cost: $99/year
   - Processing: 24-48 hours
2. **Create Distribution Certificate:**
   - See: APP_STORE_SUBMISSION_GUIDE.md → iOS → Phase 1
3. **Create Provisioning Profile:**
   - Name: "Vietnamese Lunar Calendar Distribution"
   - App ID: com.huynguyen.lunarcalendar

**Checklist:**
- [ ] Apple Developer Program enrolled
- [ ] Distribution certificate created
- [ ] Provisioning profile downloaded
- [ ] App Store Connect account accessible

#### Google Play Developer Account
1. **Register:** https://play.google.com/console/signup
   - Cost: $25 one-time
   - Verification: 2-3 days
2. **Create Release Keystore:**
```bash
# Generate keystore
keytool -genkey -v \
  -keystore ~/.android/keystore/lunarcalendar-release.keystore \
  -alias lunarcalendar \
  -keyalg RSA \
  -keysize 2048 \
  -validity 10000

# BACKUP THIS FILE! You cannot update app without it.
```

**Checklist:**
- [ ] Google Play Console account verified
- [ ] Release keystore created and backed up
- [ ] Keystore password saved securely

---

### Phase 4: Build Production Releases

#### Build iOS Release
```bash
# 1. Update .csproj with distribution settings
# See: APP_STORE_SUBMISSION_GUIDE.md → iOS → Phase 1.3

# 2. Build IPA
./build-ios-release.sh

# Output: src/LunarCalendar.MobileApp/bin/Release/net8.0-ios/ios-arm64/publish/*.ipa
```

**Verify:**
- [ ] Build completed successfully
- [ ] IPA file exists
- [ ] File size is reasonable (< 50MB)

#### Build Android Release
```bash
# 1. Set keystore passwords
export ANDROID_KEYSTORE_PASSWORD='your_password'
export ANDROID_KEY_PASSWORD='your_password'

# 2. Build AAB
./build-android-release.sh

# Output: src/LunarCalendar.MobileApp/bin/Release/net8.0-android/*-Signed.aab
```

**Verify:**
- [ ] Build completed successfully
- [ ] AAB file exists and is signed
- [ ] File size is reasonable (< 30MB)

---

### Phase 5: Create Store Listings

#### iOS: App Store Connect
1. **Log in:** https://appstoreconnect.apple.com
2. **Create App:**
   - Click "My Apps" → "+"
   - Platform: iOS
   - Name: Vietnamese Lunar Calendar
   - Bundle ID: com.huynguyen.lunarcalendar
   - SKU: com.huynguyen.lunarcalendar.001

3. **Fill Store Listing:**
   ```
   Category: Utilities / Lifestyle
   Privacy Policy: [Your GitHub Pages URL]
   Support URL: https://github.com/duchuy129/lunarcalendar
   Marketing URL: https://github.com/duchuy129/lunarcalendar
   ```

4. **Add Screenshots:**
   - Upload iPhone 6.7" screenshots
   - Upload iPhone 6.5" screenshots

5. **App Description:**
   - Copy from: APP_STORE_SUBMISSION_GUIDE.md → iOS → Phase 2.3

**Checklist:**
- [ ] App created in App Store Connect
- [ ] All required information filled
- [ ] Screenshots uploaded
- [ ] Privacy policy URL added

#### Android: Google Play Console
1. **Log in:** https://play.google.com/console
2. **Create App:**
   - App name: Vietnamese Lunar Calendar
   - Default language: English
   - App type: App
   - Free or paid: Free

3. **Complete Store Listing:**
   ```
   Category: Lifestyle
   Email: your-email@example.com
   Privacy Policy: [Your GitHub Pages URL]
   ```

4. **Add Assets:**
   - Upload app icon (512x512)
   - Upload feature graphic (1024x500)
   - Upload phone screenshots

5. **App Description:**
   - Copy from: APP_STORE_SUBMISSION_GUIDE.md → Android → Phase 2.3

6. **Complete Questionnaires:**
   - Content rating (Everyone/PEGI 3)
   - Data safety (No data collected)
   - Target audience

**Checklist:**
- [ ] App created in Play Console
- [ ] Store listing 100% complete
- [ ] All assets uploaded
- [ ] All questionnaires completed

---

### Phase 6: Submit for Review

#### Upload iOS Build
```bash
# Option 1: Using Transporter app (Recommended)
# 1. Download Transporter from Mac App Store
# 2. Open Transporter
# 3. Sign in with Apple ID
# 4. Drag and drop IPA file
# 5. Click "Deliver"

# Option 2: Using Xcode
# Window → Organizer → Archives → Distribute App
```

**Then in App Store Connect:**
1. Build → Select uploaded build (wait 10-30 min for processing)
2. Complete all sections (must be 100%)
3. Export Compliance: Select "No" (not using encryption beyond standard)
4. Submit for Review

**Checklist:**
- [ ] IPA uploaded successfully
- [ ] Build processed and available
- [ ] All sections green/complete
- [ ] Export compliance answered
- [ ] Submitted for review

#### Upload Android Build
1. **In Play Console:**
   - Production → Create new release
   - Upload AAB file
   - Release name: 1.0.1 (2)
   - Release notes:
     ```
     Initial release of Vietnamese Lunar Calendar
     
     Features:
     • Complete Vietnamese lunar calendar
     • Traditional festival tracking
     • Bilingual support (Vietnamese & English)
     • Offline functionality
     • Modern, intuitive interface
     ```

2. **Select Countries:**
   - Recommended: All countries
   - Or: Vietnam, USA, Canada, Australia

3. **Review and Rollout:**
   - Review everything (100% complete required)
   - Click "Send for review"

**Checklist:**
- [ ] AAB uploaded successfully
- [ ] Release notes added
- [ ] Countries selected
- [ ] All sections complete
- [ ] Submitted for review

---

## ⏰ Review Timeline

### Apple App Store
- **First Review:** 24-48 hours typically
- **Subsequent Reviews:** 12-24 hours
- **Status Check:** App Store Connect → My Apps → Version

### Google Play Store
- **First Review:** 2-7 days typically
- **Subsequent Reviews:** 1-3 days
- **Status Check:** Play Console → Dashboard

**During Review:**
- ✅ Check email for questions from reviewers
- ✅ Respond promptly if contacted
- ✅ Don't change anything in the app

---

## 🚨 Common Rejection Reasons & Solutions

### iOS Rejections

**"App crashes during review"**
- Solution: Did you test on physical device? Re-test thoroughly
- Fix crashes and resubmit

**"Missing/incomplete privacy policy"**
- Solution: Ensure privacy policy URL is accessible
- Add more details if needed

**"Misleading metadata"**
- Solution: Ensure screenshots match actual app
- Check descriptions are accurate

**"Performance issues"**
- Solution: Test on older devices (iPhone 11, iOS 15)
- Optimize if needed

### Android Rejections

**"Missing feature graphic"**
- Solution: Upload 1024x500 feature graphic
- Follow design guidelines

**"Content rating incomplete"**
- Solution: Complete content rating questionnaire
- Should be Everyone/PEGI 3

**"Data safety not completed"**
- Solution: Answer all data safety questions
- For this app: No data collected

**"App not working as described"**
- Solution: Test on physical Android device
- Ensure all features work

---

## 🎉 After Approval

### Day of Launch:
- [ ] Test download from App Store
- [ ] Test download from Google Play
- [ ] Verify app appears in search
- [ ] Check all store listing displays correctly

### First Week:
- [ ] Monitor crash reports (if any)
- [ ] Respond to user reviews
- [ ] Ask friends/family to review
- [ ] Share with Vietnamese community

### Ongoing:
- [ ] Plan updates based on feedback
- [ ] Monitor analytics
- [ ] Engage with users
- [ ] Build community

---

## 📊 Pre-Submission Final Check

**Before clicking "Submit":**

### iOS:
- ✅ Tested on physical iPhone
- ✅ IPA built and signed
- ✅ Screenshots uploaded (6.7" and 6.5")
- ✅ Privacy policy accessible
- ✅ Description complete
- ✅ Keywords added
- ✅ Support URL working
- ✅ Export compliance answered
- ✅ 100% complete in App Store Connect

### Android:
- ✅ Tested on physical Android device
- ✅ AAB built and signed with release keystore
- ✅ Screenshots uploaded
- ✅ Feature graphic uploaded
- ✅ Privacy policy accessible
- ✅ Description complete
- ✅ Content rating completed
- ✅ Data safety completed
- ✅ 100% complete in Play Console

---

## 📞 Quick Links

### Apple:
- App Store Connect: https://appstoreconnect.apple.com
- Developer Portal: https://developer.apple.com/account
- Review Guidelines: https://developer.apple.com/app-store/review/guidelines/

### Google:
- Play Console: https://play.google.com/console
- Policy Center: https://play.google.com/about/developer-content-policy/

### Your Resources:
- Detailed Guide: `APP_STORE_SUBMISSION_GUIDE.md`
- Testing Guide: `PHYSICAL_DEVICE_TESTING_GUIDE.md`
- Assets Checklist: `STORE_ASSETS_CHECKLIST.md`

---

## 🆘 Troubleshooting

### Build Issues:
```bash
# iOS build fails
./test-ios-device.sh  # Test first
./build-ios-release.sh  # Then build

# Android build fails
./test-android-device.sh  # Test first
./build-android-release.sh  # Then build
```

### Upload Issues:
- **iOS:** Use Transporter app, it's more reliable
- **Android:** Try different browser if upload fails

### Review Taking Too Long:
- **iOS:** Expect 24-48h, contact support after 72h
- **Android:** Expect 2-7 days, usually no action needed

---

## 🎯 Success Checklist

When you see these, you're done! 🎉

- [ ] iOS: "Waiting for Review" → "In Review" → "Ready for Sale"
- [ ] Android: "Pending publication" → "Published"
- [ ] App downloadable from both stores
- [ ] First 5-star reviews received
- [ ] Celebrating your MVP launch! 🥳

---

**You've got this! Follow the checklist and you'll have your app live in 2-3 weeks.**

**Last Updated:** December 30, 2024
**App Version:** 1.0.1 (Build 2)
**Status:** Ready for MVP Submission 🚀
