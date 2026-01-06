# Asset Preparation Checklist
**For:** Lunar Calendar App MVP Release
**Date:** January 4, 2026

---

## Current Status

Based on verification, here's what you have:

### ✅ Complete
- [x] **App Icon**: 1024x1024 PNG at [src/LunarCalendar.MobileApp/Resources/AppIcon/appicon.png](src/LunarCalendar.MobileApp/Resources/AppIcon/appicon.png)
- [x] **Screenshots**: 2 screenshots captured (android_coral_fixed.png, ios_coral_fixed.png)
- [x] **Physical Device Testing**: Completed on both iOS and Android

### ⚠️ Needs Attention
- [ ] **More Screenshots**: Need 2-6 more screenshots (total minimum: 4 for good presentation)
- [ ] **Feature Graphic** (Android only): 1024 x 500 pixels
- [ ] **Privacy Policy**: Needs to be hosted online
- [ ] **Support Email**: Needs to be set up

---

## Screenshot Requirements & Planning

### iOS App Store Requirements
- **Minimum:** 3 screenshots for 6.7" display
- **Recommended:** 4-6 screenshots
- **Size:** 1290 x 2796 pixels (iPhone 15 Pro Max, 14 Pro Max)

### Google Play Store Requirements
- **Minimum:** 2 screenshots
- **Recommended:** 4-8 screenshots
- **Size:** 1080 x 1920 pixels (or similar 9:16 aspect ratio)

### Your Current Screenshots

**android_coral_fixed.png**: 450 x 800 (too small for stores)
**ios_coral_fixed.png**: 369 x 800 (too small for stores)

**⚠️ ACTION REQUIRED**: You need to retake screenshots at higher resolution!

### Recommended Screenshots to Capture

1. **Main Calendar View** - Today's date with lunar date visible
2. **Holiday Highlighted** - Calendar showing a major holiday (Tết) in red
3. **Special Days Tab** - List of lunar special days/holidays
4. **Day Detail View** - Detailed information for a selected day
5. **Language Switch** - Side-by-side or settings showing bilingual support
6. **Year View** - Full year holidays overview (if you have this feature)

---

## How to Capture High-Quality Screenshots

### Option 1: iOS Simulator (for both iOS and Android screenshots)

```bash
# 1. Find and boot iPhone 15 Pro Max simulator (highest resolution)
xcrun simctl list devices | grep "iPhone 15 Pro Max"
xcrun simctl boot [DEVICE_ID]

# 2. Build and run your app
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-ios -c Debug

# Deploy to simulator (adjust device ID)
xcrun simctl install [DEVICE_ID] src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app
xcrun simctl launch [DEVICE_ID] com.huynguyen.lunarcalendar

# 3. Clean up status bar (optional but professional)
# In Simulator menu: I/O → Status Bar → "Clean Status Bar"

# 4. Capture screenshots
# Press: Cmd + S (saves to Desktop)

# 5. Rename files descriptively:
# Desktop/screenshot-1.png → ios-01-main-calendar.png
# Desktop/screenshot-2.png → ios-02-holiday-view.png
# etc.
```

### Option 2: Android Emulator

```bash
# 1. List available emulators
~/Library/Android/sdk/emulator/emulator -list-avds

# 2. Start Pixel 6 or similar (1080x1920 resolution)
~/Library/Android/sdk/emulator/emulator -avd Pixel_6_API_34 &

# 3. Build and install app
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-android -c Debug
~/Library/Android/sdk/platform-tools/adb install -r src/LunarCalendar.MobileApp/bin/Debug/net10.0-android/*-Signed.apk

# 4. Capture screenshots
# Click camera icon in emulator toolbar, or press Cmd+S

# 5. Rename files:
# android-01-main-calendar.png
# android-02-holiday-view.png
# etc.
```

### Option 3: Physical Device (Most Authentic!)

**iOS:**
```bash
# 1. Connect iPhone
# 2. Take screenshot on device (Side button + Volume Up)
# 3. AirDrop to Mac, or access via Photos app
# 4. Resize if needed (iPhone 15 Pro Max = 1290x2796 is ideal)
```

**Android:**
```bash
# 1. Connect Android device via USB
# 2. Capture screenshot
~/Library/Android/sdk/platform-tools/adb exec-out screencap -p > android-screenshot.png

# 3. Or use device's screenshot function and transfer to computer
```

---

## Feature Graphic (Android Play Store)

**Required:** 1024 x 500 pixels PNG or JPEG

### Quick Creation Options

#### Option 1: Canva (Easiest - Recommended)
1. Go to: https://canva.com
2. Create account (free)
3. Search templates: "Google Play Feature Graphic" or create custom 1024x500
4. Design elements:
   - Background: Use your app's colors (#FFF9F0, coral, traditional Vietnamese colors)
   - Left side: Your app icon (export at 200x200)
   - Center/Right: "Lunar Calendar" text (large, readable)
   - Bottom: Tagline "Track Vietnamese Holidays & Lunar Dates"
5. Download as PNG

#### Option 2: Figma (Free, Professional)
1. Go to: https://figma.com
2. Create new file
3. Create frame: 1024 x 500 pixels
4. Design banner:
   ```
   Layout suggestion:
   [App Icon 200x200]  |  Lunar Calendar
                       |  Track Vietnamese Holidays
                       |  [Small calendar visual]
   ```
5. Export as PNG

#### Option 3: macOS Preview (Quick & Dirty)
```bash
# 1. Create blank 1024x500 canvas
sips -z 500 1024 lunar_bg.png --out feature-graphic-temp.png

# 2. Open in Preview
open feature-graphic-temp.png

# 3. Add text and app icon using Preview tools
# 4. Save as: feature-graphic-1024x500.png
```

#### Option 4: Professional Designer
- Fiverr: $5-20 for custom feature graphic
- Search: "Google Play feature graphic design"

### Feature Graphic Design Tips
- **Keep it simple**: Users see this at small sizes
- **High contrast**: Ensure text is readable
- **Brand colors**: Use your app's color scheme
- **Clear message**: What does the app do?
- **No clutter**: Avoid too much text or complex imagery

---

## Privacy Policy Setup

You mentioned you already have [PRIVACY_POLICY.md](PRIVACY_POLICY.md) in your project.

### Step 1: Host on GitHub Pages (Free & Easy)

```bash
# 1. Create docs folder (if doesn't exist)
mkdir -p docs

# 2. Copy privacy policy
cp PRIVACY_POLICY.md docs/privacy-policy.md

# Optional: Also add terms of service
cp TERMS_OF_SERVICE.md docs/terms-of-service.md

# 3. Commit and push
git add docs/
git commit -m "Add privacy policy for app stores"
git push origin main

# 4. Enable GitHub Pages
# Go to: https://github.com/duchuy129/lunarcalendar/settings/pages
# Source: main branch → /docs folder
# Click Save

# 5. Your URL will be:
# https://duchuy129.github.io/lunarcalendar/privacy-policy
# (may take 1-2 minutes to go live)

# 6. Test the URL in browser before submitting to stores
```

### Step 2: Alternative Hosting Options

**GitHub Gist (Even simpler):**
```bash
# 1. Go to: https://gist.github.com
# 2. Paste PRIVACY_POLICY.md content
# 3. Create public gist
# 4. Click "Raw" button to get direct URL
# 5. Use that URL in app stores
```

**Google Docs (Non-technical option):**
```bash
# 1. Create new Google Doc
# 2. Paste privacy policy content
# 3. File → Share → Anyone with link can view
# 4. File → Publish to web
# 5. Get published URL
# 6. Use in app stores
```

---

## Support Email Setup

### Option 1: Dedicated Gmail (Recommended)

```bash
# 1. Create: lunarcalendar.support@gmail.com
# 2. Set up auto-responder (Gmail → Settings → Vacation responder)

Auto-response template:
---
Subject: Re: Lunar Calendar Support

Thank you for contacting Lunar Calendar support!

I'll respond within 24-48 hours.

Common questions:
• How do I change language? → Settings tab (top right) → Language
• Does it work offline? → Yes, 100% offline!
• Is my data safe? → We don't collect ANY data

For bug reports, please include:
- Device model
- OS version (iOS/Android)
- App version
- Steps to reproduce the issue

Best regards,
Huy Nguyen
Developer, Lunar Calendar
---

# 3. Add this email to:
#    - App Store Connect: App Information → Support URL
#    - Google Play Console: Store listing → Contact details
```

### Option 2: Use Existing Email with Filter

```bash
# If using existing email (e.g., your-name@gmail.com):
# 1. Create filter: from:(*@apple.com OR *@google.com OR subject:"Lunar Calendar")
# 2. Apply label: "App Support"
# 3. Set up notifications for this label
```

---

## Final Pre-Submission Checklist

Run this script to verify everything:

```bash
./verify-release-builds.sh
```

### Manual Verification

**Assets:**
- [ ] App icon: 1024x1024 PNG, no transparency
- [ ] iOS screenshots: Minimum 3, size 1290x2796 (recommended: 4-6)
- [ ] Android screenshots: Minimum 2, size 1080x1920 (recommended: 4-8)
- [ ] Feature graphic: 1024x500 for Android

**Documents:**
- [ ] Privacy policy hosted online with accessible URL
- [ ] Support email set up and monitoring
- [ ] App descriptions written (see MVP_RELEASE_GUIDE_2026.md)
- [ ] Keywords/tags prepared

**Builds:**
- [ ] iOS Release IPA built and ready
- [ ] Android Release AAB built and signed
- [ ] Both builds tested on physical devices

**Store Accounts:**
- [ ] Apple Developer account active
- [ ] Google Play Developer account created ($25 paid)
- [ ] iOS distribution certificate created
- [ ] iOS provisioning profile created
- [ ] Android release keystore created and backed up

---

## Quick Commands Reference

### Capture iOS Screenshots (Simulator)
```bash
# Boot simulator
xcrun simctl boot [DEVICE_ID]

# Take screenshot
# Press Cmd+S in simulator
# Saves to ~/Desktop
```

### Capture Android Screenshots (Emulator)
```bash
# Start emulator
~/Library/Android/sdk/emulator/emulator -avd Pixel_6_API_34 &

# Take screenshot
# Click camera icon in emulator, or Cmd+S
```

### Resize Images (if needed)
```bash
# Resize to iOS size (1290x2796)
sips -z 2796 1290 input.png --out ios-screenshot.png

# Resize to Android size (1080x1920)
sips -z 1920 1080 input.png --out android-screenshot.png

# Create 512x512 Android icon from 1024x1024
sips -z 512 512 src/LunarCalendar.MobileApp/Resources/AppIcon/appicon.png --out android-icon-512.png
```

### Verify Screenshot Sizes
```bash
# Check dimensions
sips -g pixelWidth -g pixelHeight screenshot.png

# Check all screenshots
for img in *.png; do
  echo "$img:"
  sips -g pixelWidth -g pixelHeight "$img" | grep pixel
  echo ""
done
```

---

## Recommended File Organization

Create a folder structure like this:

```
lunarcalendar/
├── store-assets/
│   ├── ios/
│   │   ├── screenshots/
│   │   │   ├── ios-01-main-calendar.png (1290x2796)
│   │   │   ├── ios-02-special-days.png (1290x2796)
│   │   │   ├── ios-03-day-detail.png (1290x2796)
│   │   │   └── ios-04-bilingual.png (1290x2796)
│   │   └── icon/
│   │       └── app-icon-1024.png (1024x1024)
│   │
│   ├── android/
│   │   ├── screenshots/
│   │   │   ├── android-01-main-calendar.png (1080x1920)
│   │   │   ├── android-02-special-days.png (1080x1920)
│   │   │   ├── android-03-day-detail.png (1080x1920)
│   │   │   └── android-04-bilingual.png (1080x1920)
│   │   ├── icon/
│   │   │   └── app-icon-512.png (512x512)
│   │   └── feature-graphic/
│   │       └── feature-graphic-1024x500.png (1024x500)
│   │
│   └── copy/
│       ├── app-description-ios.txt
│       ├── app-description-android.txt
│       ├── keywords.txt
│       └── release-notes.txt
│
├── builds/
│   ├── ios/
│   │   └── LunarCalendar.ipa
│   └── android/
│       └── com.huynguyen.lunarcalendar-Signed.aab
│
└── docs/
    ├── privacy-policy.md (for GitHub Pages)
    └── terms-of-service.md
```

To create this structure:

```bash
mkdir -p store-assets/{ios/{screenshots,icon},android/{screenshots,icon,feature-graphic},copy}
mkdir -p builds/{ios,android}
```

---

## Timeline to Completion

**Today (Day 1):**
- [ ] Capture 4-6 high-quality screenshots
- [ ] Create feature graphic for Android
- [ ] Host privacy policy on GitHub Pages
- [ ] Set up support email

**Tomorrow (Day 2):**
- [ ] Create Google Play Developer account (if not done)
- [ ] Generate Android release keystore
- [ ] Build iOS release IPA
- [ ] Build Android release AAB

**Day 3:**
- [ ] Create App Store Connect listing
- [ ] Upload iOS build
- [ ] Complete all App Store Connect forms

**Day 4:**
- [ ] Create Google Play Console listing
- [ ] Upload Android build
- [ ] Complete all Play Console forms

**Day 5:**
- [ ] Final review of both store listings
- [ ] Submit iOS for review
- [ ] Submit Android for review

**Week 2-3:**
- [ ] Monitor review status
- [ ] Respond to any reviewer questions
- [ ] Apps go live! 🎉

---

## Need Help?

**For screenshot composition:**
- Look at similar apps in App Store / Play Store
- Google: "app store screenshot examples"
- Keep it simple: show the app in action

**For feature graphic:**
- Canva has templates: search "Google Play feature graphic"
- Fiverr designers: $5-20 for custom design
- Keep text large and readable

**For privacy policy:**
- You already have PRIVACY_POLICY.md - just host it!
- GitHub Pages is free and professional

**For general questions:**
- Refer to [MVP_RELEASE_GUIDE_2026.md](MVP_RELEASE_GUIDE_2026.md)
- Apple: https://developer.apple.com/contact/
- Google: https://support.google.com/googleplay/android-developer

---

**You're very close to submission! Focus on screenshots and feature graphic, and you'll be ready to go! 🚀**

**Last Updated:** January 4, 2026
