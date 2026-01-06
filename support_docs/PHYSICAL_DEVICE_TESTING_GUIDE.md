# Physical Device Testing Guide
**Vietnamese Lunar Calendar - Pre-Submission Testing**

## 🎯 Purpose
This guide provides step-by-step instructions for testing the Vietnamese Lunar Calendar app on real physical devices before submitting to app stores. Physical device testing is **MANDATORY** before submission.

---

## 📱 iOS Physical Device Testing

### Prerequisites
- [ ] iPhone or iPad running iOS 15.0 or later
- [ ] USB cable (Lightning or USB-C depending on device)
- [ ] Mac with Xcode installed
- [ ] Apple Developer account (free or paid)

### Step 1: Enable Developer Mode on iPhone

#### For iOS 16+:
1. Connect iPhone to Mac via USB
2. Trust the computer when prompted on iPhone
3. On iPhone: **Settings → Privacy & Security → Developer Mode**
4. Toggle **Developer Mode** ON
5. Restart iPhone when prompted
6. Confirm enabling Developer Mode

#### For iOS 15:
Developer Mode is enabled automatically when deploying from Xcode

### Step 2: Get Device UDID

```bash
# Method 1: Using xctrace (recommended)
xcrun xctrace list devices

# Method 2: Using system_profiler
system_profiler SPUSBDataType | grep -A 11 "iPhone\|iPad"

# Method 3: In Xcode
# Window → Devices and Simulators → Select your device → Identifier
```

Copy the device UDID (format: XXXXXXXX-XXXXXXXXXXXXXXXX)

### Step 3: Register Device in Developer Portal (if needed)

**For Free Developer Account:**
- Automatic registration when building
- Device limit: 3 devices
- App expires after 7 days and needs rebuilding

**For Paid Developer Account:**
1. Go to [developer.apple.com/account](https://developer.apple.com/account)
2. Navigate to **Certificates, Identifiers & Profiles**
3. Click **Devices** → **+** (Add Device)
4. Enter:
   - **Name:** iPhone 15 Pro (or your device name)
   - **UDID:** Paste the UDID from Step 2
5. Click **Continue** → **Register**

### Step 4: Build and Deploy to Device

#### Option A: Using .NET CLI (Recommended for testing)

```bash
# Navigate to project directory
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Build for iOS device (Debug configuration for testing)
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net8.0-ios \
  -c Debug \
  -p:RuntimeIdentifier=ios-arm64 \
  -p:CodesignKey="Apple Development" \
  -p:CodesignProvision="Automatic" \
  -p:_DeviceName=:v=iPhone

# The build process will:
# 1. Generate provisioning profile automatically
# 2. Sign the app
# 3. Deploy to connected device

# Check build output for app location:
# src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/ios-arm64/LunarCalendar.MobileApp.app
```

#### Option B: Using Xcode (Alternative)

```bash
# 1. Build the project first
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net8.0-ios \
  -c Debug

# 2. Open the generated app bundle in Xcode
open src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/ios-arm64/LunarCalendar.MobileApp.app

# 3. In Xcode:
#    - Select your connected device as target
#    - Click Run (▶) or Product → Run
```

### Step 5: Trust Developer on Device

**First Time Running:**
1. App will fail to launch with "Untrusted Developer" message
2. On iPhone: **Settings → General → VPN & Device Management**
3. Find your Apple ID or developer name
4. Tap → **Trust "[Your Name]"**
5. Confirm trust

**Now launch the app again - it should work!**

### Step 6: Run Test Scenarios

Complete all tests in the **iOS Test Checklist** (see below)

---

## 🤖 Android Physical Device Testing

### Prerequisites
- [ ] Android phone or tablet running Android 8.0+ (API 26)
- [ ] USB cable
- [ ] Mac with Android SDK installed

### Step 1: Enable Developer Options on Android

1. On Android device: **Settings → About Phone**
2. Find **Build Number** (may be under "Software Information")
3. Tap **Build Number** 7 times rapidly
4. Enter PIN/password if prompted
5. You'll see "You are now a developer!" message

### Step 2: Enable USB Debugging

1. Go back to main Settings
2. Navigate to **Settings → System → Developer Options**
   (Location may vary: sometimes **Settings → Developer Options**)
3. Toggle **Developer Options** ON
4. Enable **USB Debugging**
5. Enable **Install via USB** (if available)

### Step 3: Connect Device to Mac

```bash
# Connect device via USB

# Verify connection
~/Library/Android/sdk/platform-tools/adb devices

# You should see:
# List of devices attached
# XXXXXXXX    device

# If you see "unauthorized":
# 1. Check device screen for authorization prompt
# 2. Tap "Allow" and check "Always allow from this computer"
# 3. Run: adb devices again
```

### Step 4: Build and Deploy to Device

```bash
# Navigate to project directory
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Build APK for Android (Debug configuration for testing)
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net8.0-android \
  -c Debug

# Install APK on connected device
~/Library/Android/sdk/platform-tools/adb install -r \
  src/LunarCalendar.MobileApp/bin/Debug/net8.0-android/com.huynguyen.lunarcalendar-Signed.apk

# Launch app on device
~/Library/Android/sdk/platform-tools/adb shell am start \
  -n com.huynguyen.lunarcalendar/crc64b4eb7c64d0c09b15.MainActivity

# Alternative: Manually tap app icon on device
```

### Step 5: Monitor Logs (Optional)

```bash
# Clear previous logs
~/Library/Android/sdk/platform-tools/adb logcat -c

# Monitor app logs in real-time
~/Library/Android/sdk/platform-tools/adb logcat | grep -i "lunarcalendar\|mono\|crash"

# Or save to file for later analysis
~/Library/Android/sdk/platform-tools/adb logcat > ~/Desktop/android_logs.txt
```

### Step 6: Run Test Scenarios

Complete all tests in the **Android Test Checklist** (see below)

---

## ✅ iOS Test Checklist

### Installation & Launch (Critical)
- [ ] App installs without errors
- [ ] App icon appears on home screen
- [ ] App launches successfully (< 3 seconds)
- [ ] Splash screen displays correctly
- [ ] No crash on launch

### Core Functionality
- [ ] **Calendar Display**
  - [ ] Current month displays on launch
  - [ ] Solar dates are correct
  - [ ] Lunar dates are correct
  - [ ] Day names in correct language
  - [ ] Month/year header shows correctly
  
- [ ] **Date Selection**
  - [ ] Can tap any date
  - [ ] Selected date highlights
  - [ ] Date details view opens
  - [ ] Lunar date info is accurate
  - [ ] Can close date details

- [ ] **Month/Year Navigation**
  - [ ] Month picker opens
  - [ ] Year picker opens
  - [ ] Can select different months
  - [ ] Can select different years
  - [ ] Calendar updates correctly
  - [ ] Can navigate to past dates (test: Jan 2020)
  - [ ] Can navigate to future dates (test: Dec 2030)

- [ ] **Special Days**
  - [ ] Special days list opens
  - [ ] Shows lunar festivals
  - [ ] Dates are accurate
  - [ ] Descriptions display correctly
  - [ ] Can scroll through list
  - [ ] Can close special days view

- [ ] **Settings**
  - [ ] Settings page opens
  - [ ] Can switch to Vietnamese
  - [ ] Can switch to English
  - [ ] All UI text updates immediately
  - [ ] Date formatting changes correctly
  - [ ] Settings persist after app restart

### UI/UX Testing
- [ ] **Visual Design**
  - [ ] All text is readable (not cut off)
  - [ ] Colors are consistent
  - [ ] Icons display properly
  - [ ] No overlapping elements
  - [ ] Proper spacing and alignment
  
- [ ] **Touch Interactions**
  - [ ] All buttons are easily tappable
  - [ ] Touch targets are adequate (44x44pt minimum)
  - [ ] No accidental touches
  - [ ] Swipe gestures work (if implemented)
  
- [ ] **Screen Compatibility**
  - [ ] Safe area respected (notch/Dynamic Island)
  - [ ] Status bar visible
  - [ ] Navigation bar works
  - [ ] No elements hidden behind notch

### Performance Testing
- [ ] **Responsiveness**
  - [ ] No lag when tapping dates
  - [ ] Smooth month transitions
  - [ ] Quick language switching
  - [ ] Instant settings changes
  
- [ ] **Stability**
  - [ ] No crashes during 15-minute session
  - [ ] No memory warnings
  - [ ] No freezing or hanging
  - [ ] Battery usage is reasonable

### Orientation & Multitasking
- [ ] **Rotation** (if supported)
  - [ ] Portrait mode works
  - [ ] Landscape mode works (if enabled)
  - [ ] Content adapts correctly
  
- [ ] **Multitasking**
  - [ ] App survives backgrounding
  - [ ] Returns to same state after background
  - [ ] No crash when reopening
  - [ ] No data loss

### Edge Cases
- [ ] **Rapid Interaction**
  - [ ] Quick date tapping doesn't crash
  - [ ] Fast month switching works
  - [ ] Rapid language toggling stable
  
- [ ] **Data Accuracy**
  - [ ] Leap years calculated correctly
  - [ ] Month boundaries accurate
  - [ ] Special days on correct dates
  - [ ] Date conversion is accurate (spot check 5 dates)

### Accessibility (Bonus)
- [ ] VoiceOver reads elements (if implemented)
- [ ] Dynamic Type scales text (if supported)
- [ ] Sufficient color contrast
- [ ] Buttons have accessible labels

### Final Checks
- [ ] No spelling errors in UI
- [ ] Correct app name displayed
- [ ] Version number correct (if shown)
- [ ] All features work as expected
- [ ] Ready for submission ✅

---

## ✅ Android Test Checklist

### Installation & Launch (Critical)
- [ ] APK installs without errors
- [ ] App icon appears in app drawer
- [ ] App icon looks good (proper resolution)
- [ ] App launches successfully (< 3 seconds)
- [ ] Splash screen displays correctly
- [ ] No crash on launch
- [ ] No ANR (App Not Responding) warnings

### Core Functionality
- [ ] **Calendar Display**
  - [ ] Current month displays on launch
  - [ ] Solar dates are correct
  - [ ] Lunar dates are correct
  - [ ] Day names in correct language
  - [ ] Month/year header shows correctly
  
- [ ] **Date Selection**
  - [ ] Can tap any date
  - [ ] Selected date highlights
  - [ ] Date details view opens
  - [ ] Lunar date info is accurate
  - [ ] Can close date details

- [ ] **Month/Year Navigation**
  - [ ] Month picker opens
  - [ ] Year picker opens
  - [ ] Can select different months
  - [ ] Can select different years
  - [ ] Calendar updates correctly
  - [ ] Can navigate to past dates (test: Jan 2020)
  - [ ] Can navigate to future dates (test: Dec 2030)

- [ ] **Special Days**
  - [ ] Special days list opens
  - [ ] Shows lunar festivals
  - [ ] Dates are accurate
  - [ ] Descriptions display correctly
  - [ ] Can scroll through list
  - [ ] Can close special days view

- [ ] **Settings**
  - [ ] Settings page opens
  - [ ] Can switch to Vietnamese
  - [ ] Can switch to English
  - [ ] All UI text updates immediately
  - [ ] Date formatting changes correctly
  - [ ] Settings persist after app restart

### Android-Specific Features
- [ ] **Navigation**
  - [ ] Back button closes dialogs/pickers
  - [ ] Back button navigates properly
  - [ ] Back button exits app from main screen
  - [ ] Home button backgrounds app
  - [ ] Recent apps shows correct preview
  
- [ ] **System UI**
  - [ ] Status bar visible
  - [ ] Navigation bar doesn't overlap content
  - [ ] Notification shade works
  - [ ] System theme respected (if applicable)

### UI/UX Testing
- [ ] **Visual Design**
  - [ ] All text is readable
  - [ ] Colors display correctly on device
  - [ ] Icons are clear and sharp
  - [ ] No overlapping elements
  - [ ] Proper spacing and alignment
  
- [ ] **Touch Interactions**
  - [ ] All buttons are easily tappable
  - [ ] Touch targets are adequate (48dp minimum)
  - [ ] Ripple effects work (Material Design)
  - [ ] No accidental touches
  
- [ ] **Screen Compatibility**
  - [ ] Looks good on phone screen
  - [ ] Works on different densities
  - [ ] No cutoff on smaller screens
  - [ ] Scales well on larger screens

### Performance Testing
- [ ] **Responsiveness**
  - [ ] No lag when tapping dates
  - [ ] Smooth scrolling
  - [ ] Quick month transitions
  - [ ] Fast language switching
  
- [ ] **Stability**
  - [ ] No crashes during 15-minute session
  - [ ] No ANR errors
  - [ ] No force close dialogs
  - [ ] Battery usage is reasonable
  - [ ] No excessive heating

### Orientation & Multitasking
- [ ] **Rotation** (if supported)
  - [ ] Portrait mode works
  - [ ] Landscape mode works (if enabled)
  - [ ] Content adapts correctly
  - [ ] No crash on rotation
  
- [ ] **Multitasking**
  - [ ] App survives backgrounding
  - [ ] Returns to same state after background
  - [ ] No crash when reopening
  - [ ] Split screen works (if supported)
  - [ ] Picture-in-picture doesn't break (if applicable)

### Edge Cases
- [ ] **Rapid Interaction**
  - [ ] Quick date tapping doesn't crash
  - [ ] Fast month switching works
  - [ ] Rapid language toggling stable
  - [ ] No ANR during rapid taps
  
- [ ] **Data Accuracy**
  - [ ] Leap years calculated correctly
  - [ ] Month boundaries accurate
  - [ ] Special days on correct dates
  - [ ] Date conversion is accurate (spot check 5 dates)

- [ ] **Low Resources**
  - [ ] Works with limited memory
  - [ ] Handles low battery mode
  - [ ] No crash under stress

### Accessibility (Bonus)
- [ ] TalkBack reads elements (if implemented)
- [ ] Font size changes respect system settings
- [ ] Sufficient color contrast
- [ ] Touch targets meet accessibility size

### Final Checks
- [ ] No spelling errors in UI
- [ ] Correct app name displayed
- [ ] Version number correct (if shown)
- [ ] All features work as expected
- [ ] No crashes logged
- [ ] Ready for submission ✅

---

## 🐛 Common Issues & Solutions

### iOS Issues

**Issue: "Untrusted Developer"**
```
Solution:
Settings → General → VPN & Device Management → Trust Developer
```

**Issue: "Unable to install"**
```
Solution:
1. Delete existing app from device
2. Restart iPhone
3. Clean and rebuild: dotnet clean, then build again
4. Check provisioning profile is valid
```

**Issue: App crashes on launch**
```
Solution:
1. Check Console.app on Mac for crash logs
2. Ensure iOS version is 15.0+
3. Verify all required frameworks are included
4. Rebuild in Debug mode for better error messages
```

**Issue: "Code signing failed"**
```
Solution:
1. Open Xcode → Preferences → Accounts
2. Verify Apple ID is signed in
3. Download provisioning profiles
4. Clean derived data: rm -rf ~/Library/Developer/Xcode/DerivedData/*
```

### Android Issues

**Issue: "adb devices" shows "unauthorized"**
```
Solution:
1. Check device for authorization dialog
2. Unplug and replug USB cable
3. Restart adb: 
   adb kill-server
   adb start-server
```

**Issue: "Installation failed"**
```
Solution:
1. Uninstall existing app from device
2. Clear package cache:
   adb shell pm clear com.huynguyen.lunarcalendar
3. Ensure USB debugging is enabled
4. Try different USB port/cable
```

**Issue: App crashes on launch (Android)**
```
Solution:
1. Check logs: adb logcat | grep -i crash
2. Verify minimum Android version (API 26+)
3. Check for missing permissions
4. Rebuild with Debug configuration
```

**Issue: "App not installed as package appears to be invalid"**
```
Solution:
1. Check APK is properly signed
2. Verify package name matches AndroidManifest.xml
3. Clear Android cache: dotnet clean
4. Rebuild from scratch
```

---

## 📊 Testing Results Template

Use this template to document your testing results:

```markdown
# Physical Device Testing Results
Date: [Date]
Tester: [Your Name]

## iOS Testing
Device: [iPhone Model, iOS Version]
Build: 1.0.1 (2)
Configuration: Debug

### Results:
- Installation: ✅ Pass / ❌ Fail
- Launch: ✅ Pass / ❌ Fail
- Core Features: ✅ Pass / ❌ Fail
- Performance: ✅ Pass / ❌ Fail
- Stability: ✅ Pass / ❌ Fail

### Issues Found:
1. [Issue description]
2. [Issue description]

### Notes:
[Any additional observations]

---

## Android Testing
Device: [Phone Model, Android Version, API Level]
Build: 1.0.1 (2)
Configuration: Debug

### Results:
- Installation: ✅ Pass / ❌ Fail
- Launch: ✅ Pass / ❌ Fail
- Core Features: ✅ Pass / ❌ Fail
- Performance: ✅ Pass / ❌ Fail
- Stability: ✅ Pass / ❌ Fail

### Issues Found:
1. [Issue description]
2. [Issue description]

### Notes:
[Any additional observations]

---

## Overall Assessment
Ready for submission: ✅ Yes / ❌ No (needs fixes)

Blocker issues: [List any critical issues]
Next steps: [What needs to be done]
```

---

## 🎯 Testing Best Practices

### Do:
✅ Test on actual devices, not just simulators
✅ Test on both older and newer device models
✅ Test in different lighting conditions (screen visibility)
✅ Test with poor network (airplane mode for offline)
✅ Test after device restart
✅ Test with low battery mode enabled
✅ Use the app as a real user would
✅ Try to break the app with unexpected inputs
✅ Document all issues with screenshots

### Don't:
❌ Skip physical device testing
❌ Test only on latest devices
❌ Assume simulator testing is sufficient
❌ Rush through test checklist
❌ Ignore minor UI issues
❌ Test only happy paths
❌ Submit without completing full checklist

---

## 📱 Recommended Test Devices

### iOS (Minimum):
- **1 iPhone:** iPhone 11 or newer (iOS 15+)
- **1 iPad:** iPad (9th gen) or iPad Air (if supporting iPad)

### iOS (Ideal):
- **2 iPhones:** One older (iPhone 11-13), one newer (iPhone 14-15)
- **1 iPad:** iPad Pro or iPad Air

### Android (Minimum):
- **1 Phone:** Android 8.0+ (API 26) from Samsung, Google, or similar

### Android (Ideal):
- **2 Phones:** One mid-range, one flagship
- **Different Brands:** Samsung, Google Pixel, OnePlus, etc.
- **1 Tablet:** Samsung Galaxy Tab or similar (if supporting tablets)

**Can't access multiple devices?**
- Ask friends/family to test
- Use device labs (Firebase Test Lab, BrowserStack)
- Beta testing programs (TestFlight for iOS, Internal Testing for Android)

---

## 🚀 Ready for Submission!

Once you've completed all tests and checked all boxes:

1. ✅ All iOS tests passed
2. ✅ All Android tests passed
3. ✅ No critical bugs found
4. ✅ Performance is acceptable
5. ✅ UI looks good on real devices

**You're ready to build production releases and submit to app stores!**

Return to [APP_STORE_SUBMISSION_GUIDE.md](APP_STORE_SUBMISSION_GUIDE.md) for submission steps.

---

**Last Updated:** December 30, 2024
**App Version:** 1.0.1 (Build 2)
