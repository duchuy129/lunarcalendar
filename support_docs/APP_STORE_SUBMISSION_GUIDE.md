# App Store Submission Guide
**Vietnamese Lunar Calendar - MVP Release v1.0.1**

## 📋 Overview
This guide covers all steps and artifacts needed to submit the Vietnamese Lunar Calendar app to both Apple App Store and Google Play Store.

**Current App Details:**
- **App Name:** Vietnamese Lunar Calendar
- **Bundle ID:** com.huynguyen.lunarcalendar
- **Version:** 1.0.1 (Build 2)
- **Platforms:** iOS 15.0+, Android 8.0+ (API 26)

---

## 🍎 Apple App Store Submission

### Phase 1: Prerequisites & Account Setup

#### 1.1 Apple Developer Account
- [ ] Enroll in Apple Developer Program ($99/year)
  - Visit: https://developer.apple.com/programs/
  - Complete enrollment (takes 24-48 hours for approval)
- [ ] Verify account is active in App Store Connect
  - Visit: https://appstoreconnect.apple.com

#### 1.2 Certificates & Provisioning Profiles

**Development Certificate (for testing):**
```bash
# Already configured in your project for simulator testing
# Using "Apple Development" certificate with Automatic provisioning
```

**Distribution Certificate (for App Store):**
1. Log in to [Apple Developer Portal](https://developer.apple.com/account)
2. Navigate to **Certificates, Identifiers & Profiles**
3. Create **iOS Distribution Certificate**:
   - Click "+" to add new certificate
   - Select "Apple Distribution"
   - Follow CSR (Certificate Signing Request) generation steps
   - Download and install certificate in Keychain Access

4. Create **App ID**:
   - Navigate to "Identifiers"
   - Click "+" to register new App ID
   - **App ID:** com.huynguyen.lunarcalendar
   - **Description:** Vietnamese Lunar Calendar
   - **Capabilities:** Default (no special permissions needed)

5. Create **App Store Provisioning Profile**:
   - Navigate to "Profiles"
   - Click "+" to create new profile
   - Select "App Store" distribution
   - Choose your App ID: com.huynguyen.lunarcalendar
   - Select your Distribution Certificate
   - Name it: "Vietnamese Lunar Calendar Distribution"
   - Download the provisioning profile

6. Install Provisioning Profile:
   ```bash
   # Download and double-click the .mobileprovision file
   # Or copy to: ~/Library/MobileDevice/Provisioning Profiles/
   ```

#### 1.3 Update Project for Production

Update `LunarCalendar.MobileApp.csproj` for App Store distribution:

```xml
<!-- iOS Distribution Settings -->
<PropertyGroup Condition="'$(Configuration)' == 'Release' AND '$(TargetFramework)' == 'net8.0-ios'">
    <CodesignKey>Apple Distribution</CodesignKey>
    <CodesignProvision>Vietnamese Lunar Calendar Distribution</CodesignProvision>
    <CodesignEntitlements>Platforms/iOS/Entitlements.plist</CodesignEntitlements>
    <RuntimeIdentifier>ios-arm64</RuntimeIdentifier>
    <ArchiveOnBuild>true</ArchiveOnBuild>
    <TcpPort>58181</TcpPort>
</PropertyGroup>
```

### Phase 2: Required Assets & Information

#### 2.1 App Icons (Already Created ✓)
Your app icon is located at: `Resources/AppIcon/appicon.png` (1024x1024)

Verify icon requirements:
- [ ] 1024x1024 PNG format
- [ ] No transparency
- [ ] No rounded corners (iOS adds them automatically)
- [ ] Represents your lunar calendar app

#### 2.2 Screenshots (Required)

**iPhone Screenshots** (Mandatory):
- **6.7" Display (iPhone 15 Pro Max, 14 Pro Max, etc.):**
  - Resolution: 1290 x 2796 pixels
  - Minimum: 3 screenshots, Maximum: 10

- **6.5" Display (iPhone 11 Pro Max, XS Max):**
  - Resolution: 1284 x 2778 pixels
  - Minimum: 3 screenshots, Maximum: 10

**iPad Screenshots** (If supporting iPad):
- **12.9" Display (iPad Pro):**
  - Resolution: 2048 x 2732 pixels
  - Minimum: 3 screenshots, Maximum: 10

**What to Capture:**
1. Main calendar view with lunar dates
2. Month/Year picker in action
3. Special lunar days list
4. Settings/Language switch
5. Day details view

**How to Capture on Simulator:**
```bash
# 1. Boot iPhone 15 Pro Max simulator
xcrun simctl list devices | grep "iPhone 15 Pro Max"
xcrun simctl boot <DEVICE_ID>

# 2. Run your app
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-ios -c Release
xcrun simctl install <DEVICE_ID> src/LunarCalendar.MobileApp/bin/Release/net8.0-ios/ios-arm64/LunarCalendar.MobileApp.app

# 3. Take screenshots (Cmd + S in Simulator)
# Screenshots saved to: ~/Desktop

# 4. Repeat for iPad Pro 12.9" if supporting iPad
```

#### 2.3 App Store Listing Information

**App Information:**
```yaml
App Name: Vietnamese Lunar Calendar
Subtitle: Traditional Lunar Calendar (max 30 characters)
Primary Language: English
Category: 
  Primary: Utilities
  Secondary: Lifestyle

Privacy Policy URL: 
  https://yourdomain.com/privacy-policy
  # Or host PRIVACY_POLICY.md on GitHub Pages

Support URL:
  https://github.com/duchuy129/lunarcalendar
  # Or your support email

Marketing URL (Optional):
  https://github.com/duchuy129/lunarcalendar
```

**Description (Max 4000 characters):**
```
Vietnamese Lunar Calendar - Your Traditional Calendar Companion

Discover the beauty and significance of the Vietnamese Lunar Calendar with this intuitive mobile app. Perfect for tracking traditional holidays, festivals, and special lunar days.

KEY FEATURES:
• 📅 Dual Calendar Display - View both solar and lunar dates side by side
• 🌙 Lunar Date Conversion - Instantly convert between solar and lunar dates
• 🎊 Special Days Tracker - Never miss important lunar celebrations and festivals
• 🌏 Vietnamese & English - Fully bilingual interface with seamless language switching
• 🎯 Month & Year Navigation - Easily browse any month and year
• 🎨 Beautiful, Clean Interface - Modern design with traditional elements
• 📱 Offline Functionality - Works without internet connection
• 🔒 Privacy First - No data collection, completely self-contained

PERFECT FOR:
✓ Vietnamese families maintaining cultural traditions
✓ Anyone interested in lunar calendar systems
✓ Planning traditional celebrations and ceremonies
✓ Understanding lunar phases and their significance

SPECIAL LUNAR DAYS INCLUDED:
• Tết Nguyên Đán (Lunar New Year)
• Tết Đoan Ngọ (Double Fifth Festival)
• Tết Trung Thu (Mid-Autumn Festival)
• Rằm tháng Giêng (First Full Moon)
• And many more traditional observances

This app is completely self-contained with no external dependencies, ensuring your privacy and providing reliable offline access to all features.

Download now and stay connected to your cultural heritage!
```

**Keywords (Max 100 characters, comma-separated):**
```
lunar calendar,vietnamese,moon calendar,traditional,tet,festival,holiday,culture,converter
```

**Promotional Text (Max 170 characters - Optional):**
```
Track Vietnamese lunar dates, traditional festivals, and special days. Bilingual, offline, and privacy-focused. Perfect for cultural celebrations!
```

**What's New in This Version (Release Notes):**
```
Initial release of Vietnamese Lunar Calendar!

✨ Features:
• Complete lunar calendar with solar date conversion
• Vietnamese and English language support
• Special lunar days and festivals
• Month and year picker for easy navigation
• Beautiful, intuitive interface
• Fully offline - no internet required

We hope you enjoy connecting with traditional Vietnamese culture through this app!
```

#### 2.4 App Privacy Details

Since your app is self-contained with no API or database:

**Data Collection:** None
- [ ] Select "No, we do not collect data from this app"

**Privacy Policy:** Required even if not collecting data
- [ ] Host your PRIVACY_POLICY.md online
- [ ] Suggested: GitHub Pages, Netlify, or simple static hosting

```bash
# Quick setup with GitHub Pages:
# 1. Create docs folder in your repo
mkdir -p docs
cp PRIVACY_POLICY.md docs/privacy-policy.md
cp TERMS_OF_SERVICE.md docs/terms-of-service.md

# 2. Enable GitHub Pages in repo settings
# Settings → Pages → Source: main branch /docs folder

# 3. Your privacy policy will be at:
# https://duchuy129.github.io/lunarcalendar/privacy-policy
```

#### 2.5 App Review Information

**Contact Information:**
```yaml
First Name: [Your First Name]
Last Name: [Your Last Name]
Phone Number: [Your Phone with country code]
Email: [Your Email]
```

**Demo Account (Not needed for your app):**
- [ ] No demo account required (app has no login)

**Notes for Reviewer:**
```
Thank you for reviewing Vietnamese Lunar Calendar.

This app is a self-contained utility for viewing and converting Vietnamese lunar calendar dates. It requires no internet connection and collects no user data.

TESTING INSTRUCTIONS:
1. App launches to current month with both solar and lunar dates
2. Tap any date to view detailed lunar information
3. Use month/year picker at top to navigate different time periods
4. Tap "Special Days" to view traditional Vietnamese holidays
5. Tap settings icon to switch between Vietnamese and English
6. All features work offline

The app displays traditional Vietnamese festivals and lunar calendar information. All content is cultural and educational in nature.

No special credentials or setup required for testing.
```

**Attachment (Optional but Recommended):**
- [ ] Screen recording video showing app flow (Max 500MB)

### Phase 3: Build & Submit

#### 3.1 Physical Device Testing (REQUIRED BEFORE SUBMISSION)

**Test on Real iPhone:**
```bash
# 1. Connect iPhone via USB
# 2. Trust computer on iPhone
# 3. Get device UDID
xcrun xctrace list devices

# 4. Add UDID to provisioning profile in Developer Portal
# Navigate to: developer.apple.com → Certificates → Provisioning Profiles
# Edit your development profile and add the device UDID

# 5. Build and deploy to device
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net8.0-ios \
  -c Debug \
  -p:RuntimeIdentifier=ios-arm64 \
  -p:CodesignKey="Apple Development" \
  -p:CodesignProvision="Automatic"

# 6. Install on device
# Use Xcode or: xcrun devicectl device install app --device <DEVICE_ID> <APP_BUNDLE>
```

**Critical Tests on Physical Device:**
- [ ] App launches successfully
- [ ] Calendar displays correctly
- [ ] Date selection works
- [ ] Month/Year picker functions
- [ ] Special days list loads
- [ ] Language switching works
- [ ] Settings page accessible
- [ ] App performs smoothly (no lag)
- [ ] Memory usage acceptable
- [ ] No crashes during 15-minute usage session
- [ ] Screen rotations work properly
- [ ] App can be backgrounded and resumed

#### 3.2 Build Release IPA

```bash
# Navigate to project root
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Clean previous builds
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -c Release

# Build for App Store (creates .ipa file)
dotnet publish src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net8.0-ios \
  -c Release \
  -p:ArchiveOnBuild=true \
  -p:RuntimeIdentifier=ios-arm64 \
  -p:CodesignKey="Apple Distribution" \
  -p:CodesignProvision="Vietnamese Lunar Calendar Distribution" \
  -p:EnableCodeSigning=true

# IPA location:
# src/LunarCalendar.MobileApp/bin/Release/net8.0-ios/ios-arm64/publish/LunarCalendar.MobileApp.ipa
```

**Alternative: Build with Xcode (Recommended for first submission):**

```bash
# 1. Generate Xcode project
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-ios -t:Run

# 2. Open in Xcode
open src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/ios-arm64/LunarCalendar.MobileApp.app

# 3. In Xcode:
#    - Select "Any iOS Device (arm64)" as target
#    - Product → Archive
#    - Click "Distribute App" → "App Store Connect"
#    - Follow wizard to upload
```

#### 3.3 Create App in App Store Connect

1. Log in to [App Store Connect](https://appstoreconnect.apple.com)
2. Click **"My Apps"** → **"+"** → **"New App"**
3. Fill in details:
   - **Platform:** iOS
   - **Name:** Vietnamese Lunar Calendar
   - **Primary Language:** English (U.S.)
   - **Bundle ID:** com.huynguyen.lunarcalendar
   - **SKU:** com.huynguyen.lunarcalendar.001
   - **User Access:** Full Access

4. **App Information** Tab:
   - Add all information from Section 2.3
   - Upload screenshots from Section 2.2
   - Set pricing to **Free**
   - Select **Availability:** All countries

5. **Pricing and Availability:**
   - Price: Free
   - Availability: Make available in all territories

6. **App Privacy:**
   - Complete privacy questionnaire (No data collected)
   - Add privacy policy URL

7. **Build** Tab:
   - Upload IPA using Transporter app or Xcode
   - Wait for build processing (10-30 minutes)
   - Select processed build

8. **Submit for Review:**
   - Add review notes from Section 2.5
   - Check export compliance (select No if not using encryption)
   - Click **"Submit for Review"**

**Review Timeline:** Typically 24-48 hours

---

## 🤖 Google Play Store Submission

### Phase 1: Prerequisites & Account Setup

#### 1.1 Google Play Developer Account
- [ ] Create Google Play Console account ($25 one-time fee)
  - Visit: https://play.google.com/console/signup
  - Complete registration and payment
  - Verify email and phone number
  - Complete identity verification (may take 2-3 days)

#### 1.2 Create App Signing Key

**Generate Upload Key:**
```bash
# Create keystore directory
mkdir -p ~/.android/keystore

# Generate release keystore
keytool -genkey -v \
  -keystore ~/.android/keystore/lunarcalendar-release.keystore \
  -alias lunarcalendar \
  -keyalg RSA \
  -keysize 2048 \
  -validity 10000 \
  -storepass YOUR_STRONG_PASSWORD \
  -keypass YOUR_STRONG_PASSWORD \
  -dname "CN=Huy Nguyen, OU=Development, O=Lunar Calendar, L=City, ST=State, C=VN"

# IMPORTANT: Backup this keystore file safely!
# You cannot update your app without it.
```

**Create Key Properties File:**
```bash
# Create signing configuration
cat > ~/keystore.properties << EOF
storeFile=/Users/huynguyen/.android/keystore/lunarcalendar-release.keystore
storePassword=YOUR_STRONG_PASSWORD
keyAlias=lunarcalendar
keyPassword=YOUR_STRONG_PASSWORD
EOF

# Secure the file
chmod 600 ~/keystore.properties
```

**⚠️ CRITICAL: Backup Your Keystore**
```bash
# Backup to secure location (external drive, password manager, etc.)
cp ~/.android/keystore/lunarcalendar-release.keystore /path/to/secure/backup/

# Never commit keystore to git!
# Already in .gitignore: *.keystore
```

#### 1.3 Update Android Project Configuration

Add to your `.csproj` file for Android Release signing:

```xml
<!-- Android Release Signing -->
<PropertyGroup Condition="'$(Configuration)' == 'Release' AND '$(TargetFramework)' == 'net8.0-android'">
    <AndroidKeyStore>True</AndroidKeyStore>
    <AndroidSigningKeyStore>$(HOME)/.android/keystore/lunarcalendar-release.keystore</AndroidSigningKeyStore>
    <AndroidSigningKeyAlias>lunarcalendar</AndroidSigningKeyAlias>
    <AndroidSigningKeyPass>YOUR_STRONG_PASSWORD</AndroidSigningKeyPass>
    <AndroidSigningStorePass>YOUR_STRONG_PASSWORD</AndroidSigningStorePass>
</PropertyGroup>
```

**Better: Use environment variables:**
```bash
# Add to ~/.zshrc or ~/.bash_profile
export ANDROID_KEYSTORE_PASSWORD="YOUR_STRONG_PASSWORD"
export ANDROID_KEY_PASSWORD="YOUR_STRONG_PASSWORD"
```

Then in `.csproj`:
```xml
<AndroidSigningKeyPass>$(ANDROID_KEY_PASSWORD)</AndroidSigningKeyPass>
<AndroidSigningStorePass>$(ANDROID_KEYSTORE_PASSWORD)</AndroidSigningStorePass>
```

### Phase 2: Required Assets & Information

#### 2.1 App Icons (Already Created ✓)
Your Android icon is generated from: `Resources/AppIcon/appicon.png`

Android generates multiple densities automatically:
- mdpi (48x48)
- hdpi (72x72)
- xhdpi (96x96)
- xxhdpi (144x144)
- xxxhdpi (192x192)

Feature Graphic (Required for Store Listing):
- **Size:** 1024 x 500 pixels
- **Format:** PNG or JPEG
- **Purpose:** Banner image for Play Store

#### 2.2 Screenshots (Required)

**Phone Screenshots** (Mandatory):
- **Resolution:** Minimum 320px, Maximum 3840px
- **Aspect Ratio:** 16:9 or 9:16
- **Recommended:** 1080 x 1920 pixels (Portrait)
- **Quantity:** Minimum 2, Maximum 8

**Tablet Screenshots** (Recommended if supporting tablets):
- **Resolution:** 1200 x 1920 pixels or similar
- **Quantity:** Minimum 2, Maximum 8

**How to Capture on Emulator:**
```bash
# 1. List available emulators
~/Library/Android/sdk/emulator/emulator -list-avds

# 2. Start Pixel 6 or similar emulator
~/Library/Android/sdk/emulator/emulator -avd Pixel_6_API_34 &

# 3. Build and install app
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-android -c Release
~/Library/Android/sdk/platform-tools/adb install -r \
  src/LunarCalendar.MobileApp/bin/Release/net8.0-android/com.huynguyen.lunarcalendar-Signed.aab

# 4. Take screenshots
# In emulator: Click camera icon or Ctrl+S (Cmd+S on Mac)
# Screenshots saved to: ~/Desktop or emulator's screenshot folder

# 5. For tablets, repeat with tablet emulator
~/Library/Android/sdk/emulator/emulator -avd Pixel_Tablet_API_34 &
```

**What to Capture:**
1. Main calendar with lunar dates
2. Month/Year selection
3. Special lunar days
4. Day details
5. Settings/Language options

#### 2.3 Play Store Listing Information

**App Details:**
```yaml
App Name: Vietnamese Lunar Calendar
Short Description (max 80 characters):
  Traditional lunar calendar with festivals and date conversion

Full Description (max 4000 characters):
```
Vietnamese Lunar Calendar - Your Traditional Calendar Companion

Experience the richness of Vietnamese culture with this comprehensive lunar calendar app. Track traditional holidays, festivals, and important lunar dates with ease.

🌟 KEY FEATURES
📅 Dual Calendar View
  • See both solar and lunar dates at a glance
  • Seamless date conversion between systems
  • Intuitive month and year navigation

🌙 Lunar Date Tracking
  • Accurate lunar date calculations
  • Traditional Vietnamese calendar system
  • Works completely offline

🎊 Traditional Celebrations
  • Tết Nguyên Đán (Lunar New Year)
  • Tết Đoan Ngọ (Double Fifth Festival)
  • Tết Trung Thu (Mid-Autumn Festival)
  • Rằm tháng Giêng (First Full Moon)
  • Many more special lunar days

🌏 Bilingual Support
  • Full Vietnamese language support
  • English translation available
  • One-tap language switching

🎨 Modern Design
  • Clean, beautiful interface
  • Traditional elements with modern aesthetics
  • Optimized for both phones and tablets

🔒 Privacy Focused
  • No data collection
  • No internet required
  • Completely self-contained
  • No ads or tracking

✨ PERFECT FOR
• Vietnamese families maintaining traditions
• Cultural education and learning
• Planning traditional ceremonies
• Understanding lunar calendar systems
• Anyone interested in Vietnamese culture

📱 WORKS OFFLINE
All features work without an internet connection. Your privacy is completely protected as no data ever leaves your device.

🌸 CULTURAL HERITAGE
Stay connected to Vietnamese traditions and never miss important lunar celebrations. Whether you're planning Tết festivities or simply want to understand the lunar calendar, this app is your perfect companion.

Download now and bring Vietnamese cultural traditions to your fingertips!
```

**Category:**
```
Primary: Lifestyle
Secondary: Tools
```

**Tags/Keywords:**
```
lunar calendar, vietnamese, traditional calendar, moon calendar, tet, festival, 
lunar new year, vietnamese culture, date converter, holiday tracker, âm lịch, 
lịch việt nam, tết nguyên đán
```

**Contact Details:**
```yaml
Email: your-email@example.com
Website: https://github.com/duchuy129/lunarcalendar
Phone: +84-XXX-XXX-XXX (Optional)
```

**Content Rating:**
- Complete questionnaire: Everyone / PEGI 3
- No violence, mature content, or in-app purchases

**Privacy Policy:**
```
Required: Host PRIVACY_POLICY.md and provide URL
Same as iOS - use GitHub Pages or similar hosting
```

#### 2.4 Store Listing Assets

Create a feature graphic (1024 x 500):
```
Design should include:
- App name: "Vietnamese Lunar Calendar"
- Visual: Moon phases or traditional calendar imagery
- Colors: Match your app theme (lunar/traditional colors)
- Text: Minimal, readable at small sizes
```

Tools to create graphics:
- Figma (free): https://figma.com
- Canva (free): https://canva.com
- Adobe Express (free tier): https://adobe.com/express

### Phase 3: Build & Submit

#### 3.1 Physical Device Testing (REQUIRED)

**Test on Real Android Device:**
```bash
# 1. Enable Developer Mode on Android device
#    Settings → About Phone → Tap "Build Number" 7 times
#    Settings → Developer Options → Enable "USB Debugging"

# 2. Connect device via USB
# 3. Verify connection
~/Library/Android/sdk/platform-tools/adb devices

# 4. Build debug version first
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net8.0-android \
  -c Debug

# 5. Install on device
~/Library/Android/sdk/platform-tools/adb install -r \
  src/LunarCalendar.MobileApp/bin/Debug/net8.0-android/com.huynguyen.lunarcalendar-Signed.apk

# 6. Launch app on device
~/Library/Android/sdk/platform-tools/adb shell am start \
  -n com.huynguyen.lunarcalendar/.MainActivity
```

**Critical Tests on Physical Device:**
- [ ] App installs successfully
- [ ] App launches without crashes
- [ ] Main calendar displays correctly
- [ ] Date navigation works smoothly
- [ ] Month/Year picker functions
- [ ] Special days list loads
- [ ] Language switching works
- [ ] Settings accessible
- [ ] No performance issues
- [ ] Back button navigation works
- [ ] App survives background/foreground
- [ ] Different screen sizes (if available)
- [ ] Rotation works properly
- [ ] No memory leaks during extended use

#### 3.2 Build Release AAB (Android App Bundle)

```bash
# Navigate to project root
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Clean previous builds
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -c Release

# Build signed AAB for Play Store
dotnet publish src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net8.0-android \
  -c Release \
  -p:AndroidPackageFormat=aab \
  -p:AndroidKeyStore=true \
  -p:AndroidSigningKeyStore=$HOME/.android/keystore/lunarcalendar-release.keystore \
  -p:AndroidSigningKeyAlias=lunarcalendar \
  -p:AndroidSigningKeyPass=$ANDROID_KEY_PASSWORD \
  -p:AndroidSigningStorePass=$ANDROID_KEYSTORE_PASSWORD

# AAB location:
# src/LunarCalendar.MobileApp/bin/Release/net8.0-android/com.huynguyen.lunarcalendar-Signed.aab

# Verify AAB is signed
~/Library/Android/sdk/build-tools/34.0.0/aapt dump badging \
  src/LunarCalendar.MobileApp/bin/Release/net8.0-android/com.huynguyen.lunarcalendar-Signed.aab
```

**Verify AAB Contents:**
```bash
# Extract and inspect AAB
unzip -l src/LunarCalendar.MobileApp/bin/Release/net8.0-android/com.huynguyen.lunarcalendar-Signed.aab

# Check size (should be optimized with R8)
ls -lh src/LunarCalendar.MobileApp/bin/Release/net8.0-android/com.huynguyen.lunarcalendar-Signed.aab
```

#### 3.3 Create App in Play Console

1. Log in to [Google Play Console](https://play.google.com/console)
2. Click **"Create app"**
3. Fill in app details:
   - **App name:** Vietnamese Lunar Calendar
   - **Default language:** English (United States)
   - **App or game:** App
   - **Free or paid:** Free
   - **Declarations:** Check all boxes (privacy policy, content guidelines, US export laws)

4. **Dashboard Tasks** - Complete all required sections:

   **a) Store Settings:**
   - **App category:** Lifestyle
   - **Tags:** Add relevant tags
   - **Store listing contact:** Your email

   **b) Store Listing:**
   - App name: Vietnamese Lunar Calendar
   - Short description: (from Section 2.3)
   - Full description: (from Section 2.3)
   - App icon: Upload 512x512 PNG
   - Feature graphic: Upload 1024x500 PNG/JPEG
   - Phone screenshots: Upload 2-8 screenshots
   - Tablet screenshots (optional): Upload 2-8 screenshots

   **c) Privacy Policy:**
   - Add your privacy policy URL
   - Answer data safety questions (select "No" for all data collection)

   **d) App Access:**
   - Select "All functionality is available without restrictions"

   **e) Ads:**
   - Select "No, my app does not contain ads"

   **f) Content Rating:**
   - Complete questionnaire
   - Should result in: Everyone / PEGI 3

   **g) Target Audience:**
   - Select: 18 and over (general audience)
   - Or: 13-17, 18 and over (if targeting younger users for education)

   **h) News App:**
   - Select: No

   **i) COVID-19 Contact Tracing:**
   - Select: No

   **j) Data Safety:**
   - Data collection: No data collected
   - Data sharing: No data shared
   - Security practices: Data encrypted in transit (if applicable)

5. **Production Track:**
   - Navigate to: Production → Create new release
   - **App bundles:** Upload your signed AAB file
   - **Release name:** 1.0.1 (2)
   - **Release notes:**
     ```
     Initial release of Vietnamese Lunar Calendar
     
     Features:
     • Complete Vietnamese lunar calendar
     • Traditional festival tracking
     • Bilingual support (Vietnamese & English)
     • Offline functionality
     • Modern, intuitive interface
     
     Chào mừng bạn đến với Lịch Âm Việt Nam!
     ```

6. **App Signing:**
   - Select "Google Play App Signing"
   - Upload your upload key certificate
   - Google will manage your app signing key

7. **Countries/Regions:**
   - Select "All countries" or specific regions
   - Recommended: Vietnam, United States, Canada, Australia (large Vietnamese populations)

8. **Review and Rollout:**
   - Review all sections (must be 100% complete)
   - Click **"Send for review"**

**Review Timeline:** Typically 2-7 days for first app

---

## 📱 Physical Device Testing Checklist

### iOS Device Testing

#### Setup:
- [ ] iPhone running iOS 15.0 or later
- [ ] Device connected via USB to Mac
- [ ] Device UDID added to provisioning profile
- [ ] App installed successfully on device

#### Functional Tests:
- [ ] App launches without crash
- [ ] Calendar displays current month
- [ ] Lunar dates display correctly
- [ ] Tap date shows details
- [ ] Month/Year picker works
- [ ] Can navigate to past/future months
- [ ] Special days list loads
- [ ] Special days are accurate
- [ ] Settings page opens
- [ ] Language switch works (Vietnamese ↔ English)
- [ ] All text displays correctly in both languages
- [ ] Back navigation works

#### Performance Tests:
- [ ] App launches in < 3 seconds
- [ ] Date selection is responsive
- [ ] No lag when switching months
- [ ] Smooth scrolling
- [ ] No memory warnings
- [ ] Can run for 15+ minutes without issues

#### UI/UX Tests:
- [ ] Text is readable
- [ ] Colors are consistent
- [ ] Buttons are tappable
- [ ] No UI elements overlap
- [ ] Safe area respected (notch/dynamic island)
- [ ] Status bar displays correctly
- [ ] Splash screen shows properly

#### Stress Tests:
- [ ] Background → Foreground works
- [ ] App survives low memory conditions
- [ ] Rapid date selection doesn't crash
- [ ] Quick language switching works
- [ ] Screen rotation works (if supported)

### Android Device Testing

#### Setup:
- [ ] Android device running API 26+ (Android 8.0+)
- [ ] USB debugging enabled
- [ ] Device connected and authorized
- [ ] App installed successfully

#### Functional Tests:
- [ ] App launches without crash
- [ ] Calendar displays current month
- [ ] Lunar dates display correctly
- [ ] Tap date shows details
- [ ] Month/Year picker works
- [ ] Can navigate to past/future months
- [ ] Special days list loads
- [ ] Special days are accurate
- [ ] Settings accessible
- [ ] Language switch works (Vietnamese ↔ English)
- [ ] All text displays correctly in both languages
- [ ] Back button navigation works
- [ ] App drawer icon displays correctly

#### Performance Tests:
- [ ] App launches in < 3 seconds
- [ ] Date selection is responsive
- [ ] No lag when switching months
- [ ] Smooth scrolling
- [ ] No ANR (App Not Responding) errors
- [ ] Can run for 15+ minutes without issues

#### UI/UX Tests:
- [ ] Text is readable on device
- [ ] Colors display correctly
- [ ] Buttons have proper touch targets
- [ ] No UI elements cut off
- [ ] Navigation bar works
- [ ] Splash screen shows properly
- [ ] Status bar displays correctly

#### Android-Specific Tests:
- [ ] Back button closes app properly
- [ ] Recent apps preview shows correct info
- [ ] Notification shade doesn't break UI
- [ ] Split screen mode works (if supported)
- [ ] Different screen densities look good
- [ ] Dark mode respected (if implemented)

#### Stress Tests:
- [ ] Background → Foreground works
- [ ] Low memory doesn't crash app
- [ ] Rapid interaction doesn't cause ANR
- [ ] Battery usage is reasonable
- [ ] No wakelocks or battery drain

---

## 🎯 Pre-Submission Final Checklist

### Both Platforms:
- [ ] All physical device tests passed
- [ ] Privacy policy hosted and accessible
- [ ] Terms of service available (if needed)
- [ ] Support email/website ready to respond
- [ ] Screenshots captured for all required sizes
- [ ] App descriptions proofread and finalized
- [ ] Version numbers match: 1.0.1 (Build 2)
- [ ] App icons verified (no transparency, correct sizes)
- [ ] All text in app and store listings checked for typos

### iOS Specific:
- [ ] Apple Developer account active
- [ ] Distribution certificate created
- [ ] Provisioning profile downloaded
- [ ] IPA built and signed correctly
- [ ] TestFlight testing completed (optional but recommended)
- [ ] Export compliance answered (No for this app)
- [ ] Age rating appropriate (4+)

### Android Specific:
- [ ] Google Play Console account verified
- [ ] Keystore file backed up securely
- [ ] AAB signed with release keystore
- [ ] App signing by Google enabled
- [ ] Content rating completed (Everyone/PEGI 3)
- [ ] Feature graphic created and uploaded
- [ ] Data safety form completed

---

## 🚀 Submission Timeline

### Week 1: Preparation
- **Day 1-2:** Physical device testing
- **Day 3:** Create screenshots and feature graphics
- **Day 4:** Write store listings and descriptions
- **Day 5:** Set up developer accounts (if not done)

### Week 2: Builds & Assets
- **Day 6:** Build and sign iOS IPA
- **Day 7:** Build and sign Android AAB
- **Day 8-9:** Create App Store Connect and Play Console listings
- **Day 10:** Upload builds

### Week 3: Review
- **Day 11-13:** Apple review (typically 24-48 hours)
- **Day 11-17:** Google review (typically 2-7 days)
- **Day 18+:** Address any review feedback

### Week 4: Launch
- **Day 21:** Apps approved and live! 🎉

---

## 📞 Support & Resources

### Apple Resources:
- App Store Connect: https://appstoreconnect.apple.com
- Developer Portal: https://developer.apple.com/account
- App Store Review Guidelines: https://developer.apple.com/app-store/review/guidelines/
- Human Interface Guidelines: https://developer.apple.com/design/human-interface-guidelines/

### Google Resources:
- Play Console: https://play.google.com/console
- Developer Policy Center: https://play.google.com/about/developer-content-policy/
- Material Design: https://material.io/design
- Android App Bundle: https://developer.android.com/guide/app-bundle

### Common Rejection Reasons to Avoid:
1. **Missing/incomplete privacy policy**
2. **Screenshots don't match actual app**
3. **App crashes during review**
4. **Misleading app description**
5. **Poor app performance**
6. **Copyright/trademark issues**
7. **Incomplete metadata**

### If Rejected:
1. Read rejection reason carefully
2. Fix the specific issue mentioned
3. Test thoroughly
4. Resubmit with clear notes about changes
5. Respond politely to reviewer questions

---

## 🎊 Post-Launch Checklist

After apps are approved and live:

- [ ] Test download from App Store
- [ ] Test download from Play Store
- [ ] Verify app appears in search results
- [ ] Check all screenshots display correctly
- [ ] Monitor crash reports (if any)
- [ ] Respond to user reviews
- [ ] Share app link with friends/family for initial reviews
- [ ] Consider soft launch in Vietnam first
- [ ] Monitor analytics and user feedback
- [ ] Plan for future updates based on feedback

**Congratulations on launching your app! 🎉**

---

## 📝 Quick Reference Commands

### iOS Release Build:
```bash
dotnet publish src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net8.0-ios -c Release -p:ArchiveOnBuild=true \
  -p:RuntimeIdentifier=ios-arm64
```

### Android Release Build:
```bash
dotnet publish src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net8.0-android -c Release -p:AndroidPackageFormat=aab
```

### Install on Physical iOS Device:
```bash
xcrun devicectl device install app \
  --device <DEVICE_ID> \
  <PATH_TO_APP_BUNDLE>
```

### Install on Physical Android Device:
```bash
~/Library/Android/sdk/platform-tools/adb install -r <PATH_TO_APK>
```

---

**Last Updated:** December 30, 2024
**App Version:** 1.0.1 (Build 2)
**Status:** Ready for MVP Submission
