# MVP Release Guide - Lunar Calendar App
**Version:** 1.0.1 (Build 2)
**Created:** January 4, 2026
**Target Platforms:** iOS App Store & Google Play Store

---

## Executive Summary

This guide provides **step-by-step instructions** to release your Lunar Calendar app to both app stores. You already have:
- ✅ Apple Developer Account (paid annually)
- ✅ App icon (1024x1024)
- ✅ 4 screenshots
- ✅ Physical device testing completed
- ✅ App metadata in project files

**Good News:** You can work on **both platforms simultaneously** - most preparation steps are independent!

**Timeline Estimate:**
- **Week 1 (Days 1-3):** Asset preparation & account setup
- **Week 2 (Days 4-7):** Build preparation & store listings
- **Week 3 (Days 8-10):** Submission
- **Week 4 (Days 11-21):** Review period

---

## Table of Contents

1. [Quick Checklist](#quick-checklist)
2. [Apple App Store Preparation](#apple-app-store-preparation)
3. [Google Play Store Preparation](#google-play-store-preparation)
4. [Assets & Documents Required](#assets--documents-required)
5. [Build Scripts & Automation](#build-scripts--automation)
6. [Submission Process](#submission-process)
7. [Post-Submission](#post-submission)

---

## Quick Checklist

### Prerequisites (Both Platforms)
- [x] Apple Developer Account active
- [ ] Google Play Developer Account ($25 one-time)
- [x] App icon 1024x1024 PNG
- [x] 4 screenshots captured
- [x] Physical device testing complete
- [ ] Privacy policy hosted online
- [ ] Support email/contact set up

### iOS Specific
- [ ] Distribution certificate created
- [ ] App Store provisioning profile created
- [ ] App Store Connect app listing created
- [ ] Release IPA built and signed
- [ ] TestFlight testing (optional but recommended)

### Android Specific
- [ ] Release keystore generated & backed up
- [ ] Google Play Console app created
- [ ] Feature graphic (1024x500) created
- [ ] Content rating completed
- [ ] Data safety form completed
- [ ] Release AAB built and signed

---

## Apple App Store Preparation

### Phase 1: Apple Developer Account Setup

Since you already have an active Apple Developer account, we need to set up **distribution certificates and provisioning profiles**.

#### 1.1 Create Distribution Certificate

```bash
# Navigate to Apple Developer Portal
# URL: https://developer.apple.com/account/resources/certificates

# Steps:
# 1. Click "Certificates" → "+" (Add new)
# 2. Select "Apple Distribution"
# 3. Generate CSR (Certificate Signing Request):
#    - Open "Keychain Access" on Mac
#    - Menu: Keychain Access → Certificate Assistant → Request Certificate from CA
#    - Email: your-apple-id@email.com
#    - Common Name: Lunar Calendar Distribution
#    - Saved to disk
# 4. Upload CSR file
# 5. Download certificate (.cer file)
# 6. Double-click to install in Keychain
```

#### 1.2 Create App ID (if not exists)

```bash
# Navigate to: https://developer.apple.com/account/resources/identifiers

# Create App ID:
# - Type: App IDs
# - Select: App
# - Description: Lunar Calendar
# - Bundle ID: com.huynguyen.lunarcalendar (Explicit)
# - Capabilities: None required (default is fine)
# - Register
```

#### 1.3 Create App Store Provisioning Profile

```bash
# Navigate to: https://developer.apple.com/account/resources/profiles

# Create Profile:
# 1. Click "+" to add new profile
# 2. Select "App Store" (under Distribution)
# 3. App ID: com.huynguyen.lunarcalendar
# 4. Select your Distribution Certificate
# 5. Profile Name: "Lunar Calendar App Store"
# 6. Generate and Download
# 7. Double-click .mobileprovision file to install
```

### Phase 2: Prepare iOS Assets

#### 2.1 App Icon ✅ (Already Have)
Your app icon is at: `src/LunarCalendar.MobileApp/Resources/AppIcon/appicon.png`

**Verify:**
```bash
# Check icon size
sips -g pixelWidth -g pixelHeight src/LunarCalendar.MobileApp/Resources/AppIcon/appicon.png

# Expected: 1024 x 1024
# Format: PNG, no transparency
```

#### 2.2 Screenshots ✅ (Already Have)
You mentioned having 4 screenshots. Let's verify and organize them.

**Required for App Store:**
- **iPhone 6.7" Display:** 1290 x 2796 pixels (Minimum 3 screenshots)
- **iPhone 6.5" Display:** 1242 x 2688 pixels (Optional but recommended)

**Action Items:**
1. Verify screenshot dimensions
2. Ensure they show key features:
   - Main calendar view
   - Lunar date display
   - Special days/holidays
   - Language switching
3. Rename for easy identification:
   - `ios-screenshot-1-main-calendar.png`
   - `ios-screenshot-2-special-days.png`
   - `ios-screenshot-3-day-details.png`
   - `ios-screenshot-4-bilingual.png`

#### 2.3 App Store Listing Copy

**App Name:** Lunar Calendar

**Subtitle (30 chars max):** Track Vietnamese Lunar Dates

**Keywords (100 chars max, comma-separated):**
```
lunar calendar,vietnamese,tet,holidays,moon,festival,zodiac,culture,am lich,viet
```

**Description (4000 chars max):**
```
LUNAR CALENDAR - Your Vietnamese Cultural Companion

Stay connected to Vietnamese traditions with this beautiful, easy-to-use lunar calendar app. Perfect for tracking Tết, traditional holidays, and special lunar days.

✨ KEY FEATURES
• 📅 Dual Calendar Display - View both Gregorian and lunar dates side by side
• 🎊 Vietnamese Holidays - Track all major festivals (Tết Nguyên Đán, Tết Trung Thu, and more)
• 🌙 Special Lunar Days - Never miss Mùng 1 and Rằm (1st and 15th of each month)
• 🌍 Bilingual Support - Seamlessly switch between Vietnamese and English
• 📱 Works Offline - No internet connection required
• 🎨 Beautiful Interface - Clean, modern design with traditional elements

🎉 HOLIDAYS INCLUDED
• Tết Nguyên Đán (Lunar New Year)
• Giỗ Tổ Hùng Vương (Hung Kings Festival)
• Tết Đoan Ngọ (Dragon Boat Festival)
• Tết Trung Thu (Mid-Autumn Festival)
• Tết Nguyên Tiêu (Lantern Festival)
• Vu Lan (Ullambana)
• And 40+ more traditional celebrations

👨‍👩‍👧‍👦 PERFECT FOR
• Vietnamese families maintaining cultural traditions
• Planning celebrations around lunar dates
• Teaching children about Vietnamese heritage
• Coordinating with family across the globe
• Learning about lunar calendar systems

🔒 PRIVACY FOCUSED
• No data collection
• No user accounts required
• No ads or tracking
• Completely self-contained
• All calculations done locally on your device

Made with ❤️ for the Vietnamese community worldwide.

Download now and stay connected to your cultural roots!
```

**What's New (Release Notes v1.0.1):**
```
🎉 Initial Release

Features:
• Complete Vietnamese lunar calendar
• Dual date display (Gregorian + Lunar)
• 45+ traditional holidays and festivals
• Bilingual interface (Vietnamese/English)
• 100% offline functionality
• Zero data collection

Chúc mừng năm mới! 🎊
```

**Promotional Text (170 chars, editable without review):**
```
🎊 Never miss Tết or important lunar days! Track Vietnamese holidays, lunar dates, and traditions. Bilingual. Offline. Privacy-focused. 100% free!
```

### Phase 3: App Store Connect Setup

#### 3.1 Create App Listing

```bash
# Go to: https://appstoreconnect.apple.com
# Sign in with Apple Developer account

# Steps:
# 1. Click "My Apps" → "+" → "New App"
# 2. Fill in:
#    - Platforms: iOS
#    - Name: Lunar Calendar
#    - Primary Language: English (U.S.)
#    - Bundle ID: com.huynguyen.lunarcalendar
#    - SKU: lunarcalendar-2026-v1
#    - User Access: Full Access
# 3. Click "Create"
```

#### 3.2 Complete App Information

**General Information:**
- Bundle ID: com.huynguyen.lunarcalendar
- App Store Icon: Upload 1024x1024 PNG
- Category:
  - Primary: Lifestyle
  - Secondary: Utilities

**Pricing and Availability:**
- Price: Free
- Availability: All territories

**App Privacy:**
- Data Collection: No
- Tracking: No
- Privacy Policy: (Optional but recommended - see Assets section)

**Age Rating:**
Complete questionnaire:
- All answers: "None" or "No"
- Result: **4+** (Everyone)

#### 3.3 Version Information

**Screenshots:**
- Upload your 4 iPhone screenshots
- Minimum 3 required for 6.7" display
- Add captions if desired

**App Description:**
- Copy from Section 2.3

**Keywords:**
- Copy from Section 2.3

**Support URL:**
- https://github.com/duchuy129/lunarcalendar (or your website)

**Marketing URL (optional):**
- Same as support URL

**Version:** 1.0.1

**Copyright:** 2026 Huy Nguyen (or your name)

#### 3.4 App Review Information

**Contact Information:**
- First Name: [Your First Name]
- Last Name: [Your Last Name]
- Phone: [Your Phone with country code]
- Email: [Your Email]

**Sign-In Information:**
- Not required (no login in app)

**Notes for Reviewer:**
```
Thank you for reviewing Lunar Calendar!

This is a simple, offline Vietnamese lunar calendar app. It displays lunar dates and traditional Vietnamese holidays.

TESTING INSTRUCTIONS:
1. App launches to current month with both Gregorian and lunar dates
2. Tap any date to view detailed lunar information
3. Use the month/year picker to navigate
4. Tap "Special Days" tab to view Vietnamese holidays
5. Tap settings icon (top right) to switch between Vietnamese and English
6. All features work offline - no internet required

No special credentials or setup needed. The app is completely self-contained with no backend services.

Thank you!
```

**Attachment (Optional):**
- Upload a demo video if you have one (highly recommended for faster approval)

---

## Google Play Store Preparation

### Phase 1: Google Play Developer Account

#### 1.1 Create Developer Account

```bash
# URL: https://play.google.com/console/signup

# Steps:
# 1. Sign in with Google Account
# 2. Accept Developer Distribution Agreement
# 3. Pay $25 registration fee (one-time)
# 4. Complete account details:
#    - Developer name: Huy Nguyen (or your preferred name)
#    - Email: your-email@example.com
#    - Website: https://github.com/duchuy129/lunarcalendar (optional)
# 5. Verify identity (may take 2-3 days)
```

**Important:** Keep your receipt/confirmation email. You'll need this for verification.

#### 1.2 Create Release Keystore

**This is CRITICAL - You CANNOT update your app without this keystore!**

```bash
# Create keystore directory
mkdir -p ~/.android/keystore

# Generate release keystore
keytool -genkey -v \
  -keystore ~/.android/keystore/lunarcalendar-release.keystore \
  -alias lunarcalendar \
  -keyalg RSA \
  -keysize 2048 \
  -validity 10000

# You will be prompted for:
# - Keystore password: [Choose strong password - SAVE THIS!]
# - Key password: [Same or different - SAVE THIS!]
# - First and Last Name: Huy Nguyen
# - Organizational Unit: Development
# - Organization: Lunar Calendar
# - City/Locality: [Your City]
# - State/Province: [Your State]
# - Country Code: VN (or your country)

# BACKUP THIS FILE IMMEDIATELY!
cp ~/.android/keystore/lunarcalendar-release.keystore ~/Desktop/BACKUP_lunarcalendar.keystore

# Store passwords securely in password manager or encrypted note
```

**⚠️ CRITICAL BACKUP STEPS:**
1. Copy keystore to external drive
2. Save passwords in password manager
3. Email encrypted copy to yourself
4. **You CANNOT recover this - if lost, you must create new app with new package name!**

#### 1.3 Configure Keystore in Project

Create keystore properties file:

```bash
# Create signing config file
cat > ~/.android/keystore/lunarcalendar.properties << 'EOF'
storeFile=/Users/huynguyen/.android/keystore/lunarcalendar-release.keystore
storePassword=YOUR_KEYSTORE_PASSWORD_HERE
keyAlias=lunarcalendar
keyPassword=YOUR_KEY_PASSWORD_HERE
EOF

# Secure the file
chmod 600 ~/.android/keystore/lunarcalendar.properties
```

### Phase 2: Prepare Android Assets

#### 2.1 App Icon ✅ (Already Have)
Your app icon will be generated from: `src/LunarCalendar.MobileApp/Resources/AppIcon/appicon.png`

**Also Required for Play Store:**
- **High-res icon:** 512 x 512 PNG (exported from your 1024x1024 icon)

```bash
# Generate 512x512 icon from existing 1024x1024
sips -z 512 512 src/LunarCalendar.MobileApp/Resources/AppIcon/appicon.png \
  --out ~/Desktop/android-icon-512.png
```

#### 2.2 Feature Graphic (REQUIRED)

**Dimensions:** 1024 x 500 pixels (PNG or JPEG)

This is the banner image shown at the top of your Play Store listing.

**Design Options:**

**Option 1: Simple Text + Icon**
```
[Background: Gradient from #FFF9F0 to #FFE5CC]
[Left: App Icon (200x200)]
[Center/Right: "Lunar Calendar" in large text]
[Bottom: "Track Vietnamese Holidays & Lunar Dates"]
```

**Option 2: Calendar Visual**
```
[Background: Traditional Vietnamese pattern/colors]
[Left: Calendar grid with lunar dates]
[Right: App name + tagline]
```

**Tools to Create:**
- Canva (easiest - has templates): https://canva.com
- Figma (free): https://figma.com
- Adobe Express: https://adobe.com/express

**Quick DIY with macOS Preview:**
1. Create 1024x500 blank canvas in Preview
2. Add app icon
3. Add text with app name
4. Export as PNG

#### 2.3 Screenshots ✅ (Already Have)

**Required for Play Store:**
- **Minimum:** 2 screenshots
- **Recommended:** 4-8 screenshots
- **Dimensions:** 1080 x 1920 pixels (or similar 9:16 aspect ratio)

**If your screenshots are iOS size, you can:**
1. Use them as-is (Play Store accepts various sizes)
2. Or resize to 1080x1920 for consistency

```bash
# Example: Resize iOS screenshots to Android standard
# (Only if needed - check your screenshot sizes first)
sips -z 1920 1080 ios_screenshot.png --out android_screenshot.png
```

#### 2.4 Play Store Listing Copy

**App Name:** Lunar Calendar

**Short Description (80 chars max):**
```
Track Vietnamese holidays & lunar dates. Bilingual. Offline. No ads.
```

**Full Description (4000 chars max):**
```
LUNAR CALENDAR - Never Miss Tết Again 🎊

Stay connected to Vietnamese culture with this beautiful, easy-to-use lunar calendar app!

✨ KEY FEATURES

📅 Dual Calendar Display
See both Gregorian and lunar dates side by side. Perfect for planning celebrations and coordinating with family.

🎊 Vietnamese Holidays
Track all major festivals including:
• Tết Nguyên Đán (Lunar New Year)
• Giỗ Tổ Hùng Vương (Hung Kings Festival)
• Tết Đoan Ngọ (Dragon Boat Festival)
• Tết Trung Thu (Mid-Autumn Festival)
• And 40+ more traditional celebrations

🌙 Special Lunar Days
Never miss Mùng 1 and Rằm (1st and 15th of each lunar month) - important days for Vietnamese traditions.

🌍 Bilingual Support
Seamlessly switch between Vietnamese and English. Perfect for families teaching children about their heritage.

📱 Works Completely Offline
No internet connection required. All calculations are done locally on your device.

🎨 Beautiful, Modern Interface
Clean design with traditional Vietnamese elements. Easy to use for all ages.

👨‍👩‍👧‍👦 PERFECT FOR

• Vietnamese families maintaining cultural traditions
• Planning celebrations around lunar dates
• Teaching children about Vietnamese heritage
• Coordinating with family in Vietnam or abroad
• Anyone interested in lunar calendar systems
• Learning about Vietnamese zodiac and traditions

🔒 YOUR PRIVACY MATTERS

• No data collection
• No user accounts required
• No advertisements
• No tracking or analytics
• Completely self-contained
• All calculations done on your device

💝 MADE WITH LOVE

Created for the Vietnamese community worldwide. Keep your cultural traditions alive and never miss another important lunar day!

🆓 100% FREE FOREVER

No in-app purchases, no subscriptions, no hidden costs. Just a useful, beautiful app to help you stay connected to Vietnamese culture.

---

Download now and start tracking Vietnamese holidays and lunar dates!

Keywords: lịch âm, lịch vạn niên, lunar calendar, vietnamese calendar, tet, festivals, holidays
```

**Category:**
- Primary: Lifestyle

**Tags (up to 5):**
- Lifestyle
- Culture
- Calendar
- Education
- Tools

### Phase 3: Google Play Console Setup

#### 3.1 Create App

```bash
# Go to: https://play.google.com/console
# Sign in with your Google Play Developer account

# Steps:
# 1. Click "Create app"
# 2. Fill in:
#    - App name: Lunar Calendar
#    - Default language: English (United States)
#    - App or game: App
#    - Free or paid: Free
# 3. Declarations:
#    ✓ I declare this app complies with Google Play policies
#    ✓ I declare this app complies with US export laws
# 4. Click "Create app"
```

#### 3.2 Complete Store Listing

Navigate to: **Store presence → Main store listing**

**App details:**
- App name: Lunar Calendar
- Short description: (from Section 2.4)
- Full description: (from Section 2.4)

**Graphics:**
- App icon: Upload 512x512 PNG
- Feature graphic: Upload 1024x500 PNG/JPEG
- Phone screenshots: Upload your 4 screenshots (minimum 2)

**Categorization:**
- App category: Lifestyle
- Tags: Select up to 5 tags

**Contact details:**
- Email: your-email@example.com
- Phone: (optional)
- Website: https://github.com/duchuy129/lunarcalendar

**Privacy Policy:**
- URL: (see Assets & Documents section)

#### 3.3 Complete Content Rating

Navigate to: **Policy → App content → Content ratings**

**Questionnaire answers:**
- Does your app contain violence? **No**
- Does your app contain sexual content? **No**
- Does your app contain profanity? **No**
- Does your app contain controlled substances? **No**
- Does your app contain user interaction features? **No**
- Does your app share user location? **No**
- Does your app share personal information? **No**

**Result:** EVERYONE / PEGI 3

#### 3.4 Complete Data Safety

Navigate to: **Policy → App content → Data safety**

**CRITICAL:** This appears prominently on your Play Store listing!

**Does your app collect or share user data?**
- Select: **No**

**Explanation:**
```
This app does not collect, store, or share any user data. All calendar calculations are performed locally on your device. No internet connection is required for app functionality. We do not use analytics, tracking, or any third-party services.
```

**Security practices:**
- Data encrypted in transit: N/A (no data transmitted)
- Users can request data deletion: N/A (no data collected)

#### 3.5 Complete Target Audience

Navigate to: **Policy → App content → Target audience and content**

- Target age groups: **18 and over** (general audience)
- Is your app designed for children? **No**

#### 3.6 Other Declarations

**News app:** No
**COVID-19 contact tracing:** No
**Government app:** No

---

## Assets & Documents Required

### Privacy Policy

**Required by:** Both App Store and Play Store (technically optional if no data collection, but highly recommended)

**What to include:**
```markdown
# Privacy Policy for Lunar Calendar

Last Updated: January 4, 2026

## Data Collection

We collect ZERO data.

Lunar Calendar:
- Does NOT collect any personal information
- Does NOT track your usage
- Does NOT use analytics services
- Does NOT use advertising networks
- Does NOT require an account
- Does NOT connect to the internet

## How the App Works

All calendar calculations are performed locally on your device. No data leaves your device.

## Data Storage

The only data stored on your device:
- Your app settings (language preference)
- Cached calendar calculations for performance

This data:
- Stays on your device only
- Is never transmitted anywhere
- Is deleted when you uninstall the app

## Third-Party Services

We do not use any third-party services, SDKs, or analytics tools.

## Contact

Email: your-email@example.com
GitHub: https://github.com/duchuy129/lunarcalendar

---

Summary: We don't collect any data. Period.
```

**Where to host:**

**Option 1: GitHub Pages (Recommended - Free)**
```bash
# Create docs folder
mkdir -p docs

# Create privacy policy
cat > docs/privacy-policy.md << 'EOF'
[Paste privacy policy content here]
EOF

# Commit and push
git add docs/privacy-policy.md
git commit -m "Add privacy policy"
git push

# Enable GitHub Pages:
# 1. Go to repo Settings → Pages
# 2. Source: main branch → /docs folder
# 3. Save
# 4. Your URL will be: https://duchuy129.github.io/lunarcalendar/privacy-policy
```

**Option 2: GitHub Gist**
- Create public gist at: https://gist.github.com
- Get shareable URL

**Option 3: Google Docs**
- Create document
- Share → Anyone with link can view
- Publish to web

### Support Contact

**Email:** Set up dedicated email or use personal with filter
- Suggested: lunarcalendar.support@gmail.com

**Auto-responder template:**
```
Thank you for contacting Lunar Calendar support!

I'll respond within 24-48 hours.

Common questions:
• Change language? → Settings tab → Language
• Works offline? → Yes, 100%!
• Data privacy? → We don't collect ANY data

For bug reports, please include:
- Device model
- OS version (iOS/Android)
- App version
- Steps to reproduce the issue

Best regards,
Huy Nguyen
Developer, Lunar Calendar
```

---

## Build Scripts & Automation

### iOS Release Build Script

Create: `build-ios-release.sh`

```bash
#!/bin/bash

echo "🍎 Building iOS Release for App Store..."

# Configuration
PROJECT_PATH="src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"
OUTPUT_DIR="builds/ios"
CONFIGURATION="Release"
FRAMEWORK="net10.0-ios"
RUNTIME="ios-arm64"

# Clean previous builds
echo "🧹 Cleaning previous builds..."
dotnet clean "$PROJECT_PATH" -c $CONFIGURATION -f $FRAMEWORK

# Build release
echo "🔨 Building release IPA..."
dotnet publish "$PROJECT_PATH" \
  -f $FRAMEWORK \
  -c $CONFIGURATION \
  -r $RUNTIME \
  -p:ArchiveOnBuild=true \
  -p:CodesignKey="Apple Distribution" \
  -p:CodesignProvision="Lunar Calendar App Store" \
  -p:EnableCodeSigning=true \
  -p:BuildIpa=true

# Check if build succeeded
if [ $? -eq 0 ]; then
  echo "✅ iOS Release build completed successfully!"
  echo "📦 IPA location: src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/publish/"
  ls -lh src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/publish/*.ipa 2>/dev/null || echo "⚠️  IPA file not found in expected location"
else
  echo "❌ iOS Release build failed!"
  exit 1
fi
```

**Make executable:**
```bash
chmod +x build-ios-release.sh
```

### Android Release Build Script

Create: `build-android-release.sh`

```bash
#!/bin/bash

echo "🤖 Building Android Release for Google Play..."

# Configuration
PROJECT_PATH="src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"
CONFIGURATION="Release"
FRAMEWORK="net10.0-android"

# Load keystore credentials
if [ -f ~/.android/keystore/lunarcalendar.properties ]; then
  source ~/.android/keystore/lunarcalendar.properties
else
  echo "❌ Keystore properties file not found!"
  echo "Expected: ~/.android/keystore/lunarcalendar.properties"
  exit 1
fi

# Clean previous builds
echo "🧹 Cleaning previous builds..."
dotnet clean "$PROJECT_PATH" -c $CONFIGURATION -f $FRAMEWORK

# Build release AAB
echo "🔨 Building release AAB..."
dotnet publish "$PROJECT_PATH" \
  -f $FRAMEWORK \
  -c $CONFIGURATION \
  -p:AndroidPackageFormat=aab \
  -p:AndroidKeyStore=true \
  -p:AndroidSigningKeyStore="$storeFile" \
  -p:AndroidSigningKeyAlias="$keyAlias" \
  -p:AndroidSigningKeyPass="$keyPassword" \
  -p:AndroidSigningStorePass="$storePassword"

# Check if build succeeded
if [ $? -eq 0 ]; then
  echo "✅ Android Release build completed successfully!"
  echo "📦 AAB location: src/LunarCalendar.MobileApp/bin/Release/net10.0-android/"
  ls -lh src/LunarCalendar.MobileApp/bin/Release/net10.0-android/*-Signed.aab 2>/dev/null || echo "⚠️  AAB file not found"
else
  echo "❌ Android Release build failed!"
  exit 1
fi
```

**Make executable:**
```bash
chmod +x build-android-release.sh
```

### Pre-Submission Verification Script

Create: `verify-release-builds.sh`

```bash
#!/bin/bash

echo "🔍 Verifying Release Builds..."

# Check iOS IPA
echo ""
echo "📱 iOS IPA:"
if [ -f "src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/publish/LunarCalendar.MobileApp.ipa" ]; then
  ls -lh src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/publish/*.ipa
  echo "✅ iOS IPA found"
else
  echo "❌ iOS IPA not found - run ./build-ios-release.sh first"
fi

# Check Android AAB
echo ""
echo "🤖 Android AAB:"
AAB_FILE=$(find src/LunarCalendar.MobileApp/bin/Release/net10.0-android -name "*-Signed.aab" 2>/dev/null | head -1)
if [ -n "$AAB_FILE" ]; then
  ls -lh "$AAB_FILE"
  echo "✅ Android AAB found and signed"

  # Verify signature
  echo "🔐 Verifying AAB signature..."
  jarsigner -verify -verbose -certs "$AAB_FILE" | grep "jar verified"
else
  echo "❌ Android AAB not found - run ./build-android-release.sh first"
fi

# Check assets
echo ""
echo "🎨 Assets Check:"
[ -f "src/LunarCalendar.MobileApp/Resources/AppIcon/appicon.png" ] && echo "✅ App icon found" || echo "❌ App icon missing"
[ $(ls *.png 2>/dev/null | wc -l) -ge 4 ] && echo "✅ Screenshots found ($(ls *.png 2>/dev/null | wc -l) images)" || echo "⚠️  Need at least 4 screenshots"

echo ""
echo "📋 Next Steps:"
echo "1. Upload iOS IPA to App Store Connect (use Transporter app)"
echo "2. Upload Android AAB to Google Play Console"
echo "3. Complete store listings with copy from MVP_RELEASE_GUIDE_2026.md"
echo "4. Submit for review!"
```

**Make executable:**
```bash
chmod +x verify-release-builds.sh
```

---

## Submission Process

### Can I Submit to Both Stores Simultaneously?

**YES! Most steps are independent and can be done in parallel.**

**Parallel Tasks (Do at same time):**
1. ✅ Create App Store Connect app listing
2. ✅ Create Google Play Console app listing
3. ✅ Upload screenshots and assets to both
4. ✅ Fill out metadata (descriptions, keywords, etc.)
5. ✅ Complete privacy/content questionnaires

**Sequential Tasks (Must be done in order per platform):**
- iOS: Certificate → Profile → Build → Upload → Submit
- Android: Keystore → Build → Upload → Submit

### iOS Submission Steps

#### Step 1: Build Release IPA

```bash
# Run build script
./build-ios-release.sh

# Verify build
./verify-release-builds.sh
```

#### Step 2: Upload to App Store Connect

**Method 1: Using Transporter App (Recommended)**
```bash
# Download Transporter from Mac App Store
# URL: https://apps.apple.com/app/transporter/id1450874784

# Steps:
# 1. Open Transporter
# 2. Sign in with Apple ID
# 3. Click "+" or drag and drop IPA file
# 4. Click "Deliver"
# 5. Wait for upload (5-20 minutes depending on file size)
```

**Method 2: Using Command Line**
```bash
# Install Transporter CLI (if not already installed)
# xcrun altool --upload-app -f <path_to_ipa> -t ios -u <apple_id> -p <app_specific_password>
```

#### Step 3: Complete App Store Connect Listing

1. Go to: https://appstoreconnect.apple.com
2. Select your app: Lunar Calendar
3. Select version: 1.0.1
4. Fill in all sections (use copy from this guide)
5. Upload screenshots
6. Select build (wait 10-30 min after upload for processing)
7. Complete App Privacy questionnaire
8. Answer Export Compliance: **No** (not using encryption beyond standard)

#### Step 4: Submit for Review

```bash
# Final checklist:
# - All sections show green checkmark (100% complete)
# - Build selected
# - Screenshots uploaded
# - Description filled
# - Privacy policy URL added (if using)
# - Export compliance answered

# Click "Submit for Review"
```

**Review Timeline:** 24-48 hours typically (can be faster)

### Android Submission Steps

#### Step 1: Build Release AAB

```bash
# Run build script
./build-android-release.sh

# Verify build
./verify-release-builds.sh
```

#### Step 2: Upload to Google Play Console

```bash
# Go to: https://play.google.com/console
# Select your app: Lunar Calendar

# Navigate to: Production → Releases
# Click: Create new release

# Upload AAB:
# 1. Drag and drop AAB file, or click "Upload"
# 2. Wait for processing (2-10 minutes)
# 3. Review any warnings (size, permissions, etc.)
```

#### Step 3: Complete Release Details

**Release name:** 1.0.1 (2)

**Release notes:**
```
Initial release of Lunar Calendar

Features:
• Complete Vietnamese lunar calendar
• Track 45+ traditional holidays and festivals
• Bilingual support (Vietnamese & English)
• 100% offline functionality
• Zero data collection
• No ads, no tracking

Chào mừng bạn! 🎊
```

**Countries/regions:**
- Select: All countries (recommended)
- Or: Vietnam, United States, Canada, Australia (target Vietnamese diaspora)

#### Step 4: Review and Publish

```bash
# Final checklist:
# - Store listing 100% complete
# - Content rating completed
# - Data safety completed
# - Target audience set
# - AAB uploaded and processed
# - Release notes added

# Click: "Review release"
# Then: "Start rollout to Production"
```

**Review Timeline:** 2-7 days (typically 3-5 days for first app)

---

## Post-Submission

### What Happens Next?

**iOS App Store:**
1. **"Waiting for Review"** - In queue (can take hours to 2 days)
2. **"In Review"** - Being tested by Apple (usually 12-24 hours)
3. **Possible Outcomes:**
   - **"Ready for Sale"** - Approved! 🎉
   - **"Metadata Rejected"** - Minor fixes needed (screenshots, description)
   - **"Rejected"** - Need to fix issues and resubmit

**Google Play Store:**
1. **"In review"** - Being tested (2-7 days)
2. **Possible Outcomes:**
   - **"Published"** - Live! 🎉
   - **"Changes requested"** - Need updates
   - **"Suspended"** - Policy violation (rare if you follow guidelines)

### If Rejected - Common Issues & Fixes

#### iOS Common Rejections

**Issue: App crashes during review**
- Fix: Test on real device, ensure no debug code
- Response: "Fixed crash, retested on physical device"

**Issue: Screenshots don't match app**
- Fix: Take fresh screenshots from latest build
- Response: "Updated screenshots to match current version"

**Issue: Privacy policy required**
- Fix: Create and host privacy policy, add URL
- Response: "Added privacy policy URL: [link]"

**Issue: Incomplete metadata**
- Fix: Fill all required fields in App Store Connect
- Response: "Completed all required information"

#### Android Common Rejections

**Issue: Missing feature graphic**
- Fix: Create 1024x500 feature graphic, upload
- Response: Automatic after resubmission

**Issue: Content rating incomplete**
- Fix: Complete content rating questionnaire
- Response: Automatic after completion

**Issue: Data safety form incomplete**
- Fix: Answer all data safety questions
- Response: Automatic after completion

**Issue: Screenshots too small**
- Fix: Ensure minimum 320px on short side
- Response: Upload higher resolution screenshots

### Monitoring After Approval

**First 24 Hours:**
- Test download from both stores
- Verify app appears in search
- Check all store listing displays correctly
- Monitor for crash reports

**First Week:**
- Respond to user reviews
- Ask friends/family to review (5 stars!)
- Monitor analytics (if you add them later)
- Fix any critical bugs

**First Month:**
- Collect user feedback
- Plan updates
- Build user base
- Consider localization to more languages

---

## Quick Reference

### Important URLs

**Apple:**
- Developer Portal: https://developer.apple.com/account
- App Store Connect: https://appstoreconnect.apple.com
- Review Guidelines: https://developer.apple.com/app-store/review/guidelines/

**Google:**
- Play Console: https://play.google.com/console
- Policy Center: https://play.google.com/about/developer-content-policy/

**Tools:**
- Transporter (iOS uploads): Mac App Store
- Android Studio: https://developer.android.com/studio

### App Metadata Summary

**App Name:** Lunar Calendar
**Package ID (iOS):** com.huynguyen.lunarcalendar
**Package ID (Android):** com.huynguyen.lunarcalendar
**Version:** 1.0.1 (Build 2)
**Category:** Lifestyle / Utilities
**Price:** Free
**Age Rating:** 4+ (iOS), Everyone (Android)

### Contact Information Template

**Developer Name:** Huy Nguyen
**Support Email:** [your-email@example.com]
**Support URL:** https://github.com/duchuy129/lunarcalendar
**Privacy Policy:** [your-privacy-policy-url]
**Phone:** [+country-code-phone] (optional)

---

## Final Checklist

### Pre-Submission (Complete Before Submitting)

**Accounts:**
- [x] Apple Developer Account active
- [ ] Google Play Developer Account created and verified

**Assets:**
- [x] App icon 1024x1024
- [x] 4+ screenshots
- [ ] Feature graphic 1024x500 (Android only)
- [ ] Privacy policy hosted and URL obtained

**Builds:**
- [ ] iOS release IPA built and signed
- [ ] Android release AAB built and signed
- [ ] Both builds tested on physical devices

**Store Listings:**
- [ ] App Store Connect app created
- [ ] Google Play Console app created
- [ ] All metadata filled in (descriptions, keywords, etc.)
- [ ] Screenshots uploaded to both stores
- [ ] Privacy questionnaires completed
- [ ] Content ratings completed

**Testing:**
- [x] Physical iOS device tested
- [x] Physical Android device tested
- [ ] All features work offline
- [ ] No crashes during extended use
- [ ] Language switching works
- [ ] All holidays display correctly

### Submission Day

**iOS:**
- [ ] Upload IPA via Transporter
- [ ] Wait for build processing (10-30 min)
- [ ] Select build in App Store Connect
- [ ] Answer export compliance
- [ ] Submit for review
- [ ] Receive confirmation email

**Android:**
- [ ] Upload AAB to Play Console
- [ ] Wait for processing (2-10 min)
- [ ] Add release notes
- [ ] Select countries/regions
- [ ] Review release
- [ ] Start rollout to production
- [ ] Receive confirmation email

### Post-Submission

**Monitor:**
- [ ] Check email for review status updates
- [ ] Monitor App Store Connect / Play Console dashboards
- [ ] Respond to any questions from reviewers within 24 hours

**After Approval:**
- [ ] Test download from both stores
- [ ] Verify store listings display correctly
- [ ] Share with friends and family
- [ ] Ask for initial reviews
- [ ] Celebrate! 🎉

---

## Estimated Timeline

**Week 1: Asset Preparation**
- Day 1-2: Set up Google Play account, create keystore, generate assets
- Day 3: Create privacy policy, set up support email

**Week 2: Build & Upload**
- Day 4: Build iOS release, create App Store Connect listing
- Day 5: Upload iOS build, complete App Store Connect
- Day 6: Build Android release, create Play Console listing
- Day 7: Upload Android build, complete Play Console

**Week 3: Submission**
- Day 8: Submit iOS for review
- Day 9: Submit Android for review
- Day 10: Monitor review status

**Week 4: Review & Launch**
- Day 11-12: iOS review completes (typically 24-48 hours)
- Day 11-17: Android review completes (typically 2-7 days)
- Day 18+: Both apps live! 🚀

**Total time to both stores live: 2-3 weeks**

---

## Support & Questions

If you have questions during the submission process:

1. **Apple App Store:**
   - Contact: https://developer.apple.com/contact/
   - Phone: 1-800-633-2152 (US)

2. **Google Play:**
   - Help Center: https://support.google.com/googleplay/android-developer
   - Contact: Through Play Console only

3. **Technical Issues:**
   - .NET MAUI Docs: https://learn.microsoft.com/dotnet/maui/
   - Community: https://stackoverflow.com/questions/tagged/maui

---

**Good luck with your MVP release! You've built a great app - now let's get it into users' hands! 🎉**

**Last Updated:** January 4, 2026
**Version:** 1.0 (Comprehensive Guide)
