# MVP Release Quick Start Guide
**Lunar Calendar App - Your Fast Track to App Stores**

**Date:** January 4, 2026
**Status:** Ready to Begin Release Preparation 🚀

---

## 📊 Current Status

Based on verification, here's where you are:

### ✅ What You Have
1. **Apple Developer Account** - Active and paid annually
2. **App Icon** - 1024x1024 PNG ready
3. **Physical Device Testing** - Completed on iOS and Android
4. **Privacy Policy** - Document created (PRIVACY_POLICY.md)
5. **Android Release Build** - AAB already built and signed (32MB)
6. **Some Screenshots** - 3 images captured

### 🎯 What You Need
1. **More Screenshots** - Need 1-5 more (total 4-8 recommended)
2. **Feature Graphic** - 1024x500 banner for Android Play Store
3. **iOS Release Build** - Need to build IPA
4. **Privacy Policy Online** - Host on GitHub Pages
5. **Support Email** - Set up dedicated or use existing
6. **Google Play Account** - $25 one-time registration

---

## 🚀 Can I Submit to Both Stores at Once?

**YES!** Most tasks can be done in parallel:

### Work in Parallel ✅
- Create App Store Connect listing
- Create Google Play Console listing
- Upload screenshots to both stores
- Fill out metadata (descriptions, keywords)
- Complete privacy/content questionnaires

### Must Do Separately
- **iOS**: Build IPA → Upload → Submit
- **Android**: Already have AAB → Just upload → Submit

**Recommendation:** Work on both simultaneously to save time!

---

## ⚡ Your 5-Day Launch Plan

### Day 1: Assets (Today!)
**Time:** 2-3 hours

**Tasks:**
1. Capture 3-4 more screenshots (see guide below)
2. Create Android feature graphic (1024x500)
3. Host privacy policy on GitHub Pages
4. Set up support email

**Output:**
- Total 6-8 high-quality screenshots
- Feature graphic ready
- Privacy policy URL
- Support email active

### Day 2: Google Play Setup
**Time:** 2-3 hours

**Tasks:**
1. Create Google Play Developer account ($25)
2. Wait for account verification (1-3 days, but can continue other tasks)
3. Create release keystore (if not done)
4. Verify Android AAB build (already done ✅)

**Output:**
- Google Play account ready
- Keystore backed up securely

### Day 3: iOS Build & Setup
**Time:** 2-4 hours

**Tasks:**
1. Create iOS distribution certificate
2. Create App Store provisioning profile
3. Build iOS release IPA
4. Create App Store Connect app listing
5. Upload screenshots and metadata

**Output:**
- iOS IPA built and ready
- App Store Connect 80% complete

### Day 4: Store Listings
**Time:** 2-3 hours

**Tasks:**
1. Complete App Store Connect listing
2. Complete Google Play Console listing
3. Upload iOS IPA to App Store Connect
4. Upload Android AAB to Play Console
5. Fill out all questionnaires

**Output:**
- Both stores 100% complete
- Ready to submit

### Day 5: Submit for Review
**Time:** 1 hour

**Tasks:**
1. Final review of both listings
2. Submit iOS for review
3. Submit Android for review
4. Monitor email for updates

**Output:**
- Both apps in review! 🎉

### Weeks 2-3: Review Period
- iOS: Typically 24-48 hours
- Android: Typically 2-7 days
- Respond to any reviewer questions
- Apps go live!

---

## 📸 Quick Screenshot Guide

You need **6-8 total screenshots** showing key features.

### Screenshots to Capture

1. **Main Calendar** - Today's date with lunar info
2. **Holiday View** - Calendar with Tết or major holiday highlighted
3. **Special Days Tab** - List of lunar special days
4. **Day Details** - Detailed view of a selected day
5. **Language Toggle** - Settings or before/after language switch
6. **Year Holidays** - Overview of all holidays (if you have this)

### How to Capture (Choose One)

**Option A: iOS Simulator** (Recommended - Highest Quality)
```bash
# 1. Boot iPhone 15 Pro Max simulator
xcrun simctl list devices | grep "iPhone 15 Pro Max"
xcrun simctl boot [DEVICE_ID_FROM_ABOVE]

# 2. Run your app
./deploy-multi-simulator.sh  # Or your existing deployment script

# 3. Clean status bar (optional but professional)
# Simulator menu: Features → Toggle Appearance → Light (for consistency)

# 4. Navigate to each screen and press Cmd+S
# Screenshots saved to Desktop

# 5. Rename files:
mv ~/Desktop/screenshot-1.png screenshot-01-main-calendar.png
mv ~/Desktop/screenshot-2.png screenshot-02-holiday-view.png
# etc.
```

**Option B: Physical iPhone** (Most Authentic)
```bash
# 1. Open app on iPhone
# 2. Press Side Button + Volume Up to screenshot
# 3. AirDrop to Mac
# 4. Resize if needed (iPhone 15 Pro Max is 1290x2796 - ideal!)
```

**For Android screenshots:** Use same screens but from Android emulator or device (1080x1920 ideal)

---

## 🎨 Feature Graphic in 10 Minutes

**What:** 1024 x 500 pixel banner for Android Play Store

**Quickest Method - Canva:**
1. Go to https://canva.com (free account)
2. Create design → Custom size: 1024 x 500 pixels
3. Add elements:
   - Background: Your app colors (#FFF9F0 or coral)
   - Your app icon (upload and place on left)
   - Text: "Lunar Calendar" (large, bold)
   - Subtitle: "Track Vietnamese Holidays"
4. Download as PNG
5. Save as `feature-graphic-1024x500.png`

**Total time:** 5-10 minutes

---

## 🌐 Host Privacy Policy (5 Minutes)

You already have PRIVACY_POLICY.md - just publish it!

```bash
# 1. Create docs folder
mkdir -p docs

# 2. Copy privacy policy
cp PRIVACY_POLICY.md docs/privacy-policy.md

# 3. Commit and push
git add docs/privacy-policy.md
git commit -m "Add privacy policy for app stores"
git push origin main

# 4. Enable GitHub Pages
# Go to: https://github.com/duchuy129/lunarcalendar/settings/pages
# Source: Deploy from a branch → main → /docs
# Click Save

# 5. Your URL (available in 1-2 minutes):
# https://duchuy129.github.io/lunarcalendar/privacy-policy

# 6. Test URL in browser before using in stores
```

**Total time:** 5 minutes

---

## 📧 Support Email (5 Minutes)

**Option 1: Create Dedicated Gmail** (Recommended)
```
Email: lunarcalendar.support@gmail.com
Password: [secure password]

Set auto-responder:
Gmail → Settings → "Vacation responder"
→ Paste template from ASSET_PREPARATION_CHECKLIST.md
```

**Option 2: Use Existing Email**
```
Use: your-existing@email.com
Create filter for app-related emails
Set up label: "App Support"
```

**Total time:** 5 minutes

---

## 🔨 Building Release Versions

### iOS Release Build

```bash
# Run the build script
./build-ios-release.sh

# This will:
# - Clean previous builds
# - Build signed IPA for App Store
# - Show location of IPA file

# Expected output location:
# src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/publish/*.ipa
```

**Note:** First time building for release, you may need to:
1. Create distribution certificate (see Day 3 tasks in MVP_RELEASE_GUIDE_2026.md)
2. Create provisioning profile

### Android Release Build

```bash
# Already done! ✅
# Your AAB is at:
# src/LunarCalendar.MobileApp/bin/Release/net10.0-android/com.huynguyen.lunarcalendar-Signed.aab

# To rebuild if needed:
./build-android-release.sh
```

---

## 📋 Pre-Submission Checklist

Run verification script:
```bash
./verify-release-builds.sh
```

Check that you have:

**Assets:**
- [x] App icon 1024x1024 PNG
- [ ] 6-8 screenshots (iOS: 1290x2796, Android: 1080x1920)
- [ ] Feature graphic 1024x500 (Android)

**Documents:**
- [x] Privacy policy document
- [ ] Privacy policy hosted online (URL)
- [ ] Support email set up

**Builds:**
- [ ] iOS IPA built and signed
- [x] Android AAB built and signed

**Accounts:**
- [x] Apple Developer account
- [ ] Google Play Developer account ($25)

**Store Metadata** (copy provided in MVP_RELEASE_GUIDE_2026.md):
- [ ] App descriptions
- [ ] Keywords/tags
- [ ] Release notes

---

## 📚 Documentation You Have

1. **[MVP_RELEASE_GUIDE_2026.md](MVP_RELEASE_GUIDE_2026.md)** - Comprehensive guide with all steps
2. **[ASSET_PREPARATION_CHECKLIST.md](ASSET_PREPARATION_CHECKLIST.md)** - Detailed asset requirements
3. **[RELEASE_QUICK_START.md](RELEASE_QUICK_START.md)** - This file (quick overview)
4. **[PRIVACY_POLICY.md](PRIVACY_POLICY.md)** - Privacy policy content
5. **[TERMS_OF_SERVICE.md](TERMS_OF_SERVICE.md)** - Terms of service

**Build Scripts:**
- `build-ios-release.sh` - Build iOS IPA
- `build-android-release.sh` - Build Android AAB
- `verify-release-builds.sh` - Check readiness

---

## 🎯 Your Next 3 Actions (Right Now!)

### Action 1: Capture Screenshots (30 minutes)
```bash
# Boot simulator and capture 4-6 screenshots
xcrun simctl list devices | grep "iPhone 15 Pro Max"
xcrun simctl boot [DEVICE_ID]
# Navigate app, press Cmd+S for each screen
```

### Action 2: Create Feature Graphic (10 minutes)
```bash
# Go to Canva.com
# Create 1024x500 design
# Export as PNG
```

### Action 3: Host Privacy Policy (5 minutes)
```bash
mkdir -p docs
cp PRIVACY_POLICY.md docs/privacy-policy.md
git add docs/
git commit -m "Add privacy policy"
git push
# Enable GitHub Pages in repo settings
```

**Total time for all 3: ~45 minutes**

After these 3 actions, you'll be 70% ready to submit!

---

## ⏰ Realistic Timeline

### Conservative Estimate
- **Asset Preparation:** 1-2 days
- **Account Setup:** 1-2 days (waiting for Google Play verification)
- **Build & Upload:** 1 day
- **Store Listings:** 1 day
- **Review Period:** 1-3 weeks
- **Total:** 3-4 weeks to live apps

### Aggressive Estimate (if you have focused time)
- **Asset Preparation:** 1 day (today!)
- **Account Setup:** 1 day (can work in parallel during verification)
- **Build & Upload:** 1 day
- **Store Listings:** 1 day
- **Review Period:** 1-3 weeks
- **Total:** 2-3 weeks to live apps

**You can definitely launch by end of January 2026!**

---

## 🆘 Common Questions

### Q: Do I need to finish iOS before starting Android?
**A:** No! Work on both simultaneously. Most steps are independent.

### Q: What if I don't have all screenshots perfect?
**A:** 4 good screenshots are better than 8 mediocre ones. Quality over quantity.

### Q: Can I use the same screenshots for both iOS and Android?
**A:** Yes! While the ideal sizes differ, both stores accept various sizes. Just ensure they're high resolution (1080p or better).

### Q: What if Google Play verification takes days?
**A:** Continue working on iOS submission and asset preparation while waiting. Google Play verification is usually 1-3 days.

### Q: Do I really need a privacy policy if I don't collect data?
**A:** Technically optional, but highly recommended. It builds trust and many regions prefer it.

### Q: What if my app gets rejected?
**A:** Common and normal! Read the rejection reason carefully, fix the issue, and resubmit. See "Common Rejections" section in MVP_RELEASE_GUIDE_2026.md.

---

## 🎉 Success Path

```
Day 1: Assets
  ↓
Day 2: Google Play Setup (wait for verification in background)
  ↓
Day 3: iOS Build & App Store Connect
  ↓
Day 4: Complete Both Store Listings
  ↓
Day 5: SUBMIT! 🚀
  ↓
Week 2: iOS Approved (24-48 hrs)
  ↓
Week 2-3: Android Approved (2-7 days)
  ↓
BOTH APPS LIVE! 🎊
```

---

## 📞 Need Help?

### For Technical Issues
- **iOS Build:** See "iOS Submission Steps" in MVP_RELEASE_GUIDE_2026.md
- **Android Build:** Already done! ✅
- **.NET MAUI:** https://learn.microsoft.com/dotnet/maui/

### For Store Policies
- **Apple:** https://developer.apple.com/app-store/review/guidelines/
- **Google:** https://play.google.com/about/developer-content-policy/

### For Design Help
- **Screenshots:** Look at top apps in Lifestyle category for inspiration
- **Feature Graphic:** Canva has templates, search "Google Play feature graphic"

---

## 🚀 You're Ready!

You have:
- ✅ A fully functional app
- ✅ Physical device testing complete
- ✅ Apple Developer account active
- ✅ App icon ready
- ✅ Android build ready
- ✅ Comprehensive documentation
- ✅ Build scripts ready to use

You just need:
- 📸 A few more screenshots (30 minutes)
- 🎨 Feature graphic (10 minutes)
- 🌐 Host privacy policy (5 minutes)
- 📧 Set up support email (5 minutes)

**That's less than 1 hour of work to be submission-ready!**

**Let's get your app into users' hands! 🎉**

---

**For detailed step-by-step instructions, see:**
- **[MVP_RELEASE_GUIDE_2026.md](MVP_RELEASE_GUIDE_2026.md)** - Complete guide
- **[ASSET_PREPARATION_CHECKLIST.md](ASSET_PREPARATION_CHECKLIST.md)** - Asset details

**Last Updated:** January 4, 2026
**Version:** 1.0 - Quick Start
