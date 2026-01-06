# Vietnamese Lunar Calendar

A cross-platform mobile application for Vietnamese lunar calendar with holiday tracking, built with .NET MAUI.

## 📱 Overview

Vietnamese Lunar Calendar is a fully offline-capable mobile application that provides accurate lunar date calculations and comprehensive Vietnamese holiday information. The app uses on-device calculations for instant performance and supports both English and Vietnamese languages.

## ✨ Features

- **Lunar Calendar Conversion**: Instant Gregorian to Vietnamese Lunar date conversion
- **Vietnamese Holidays**: Comprehensive holiday database with detailed descriptions
  - Color-coded holiday types (National, Cultural, International, Lunar)
  - Holiday filtering by category
  - Detailed holiday information pages
- **Smart Navigation**:
  - Month-by-month calendar view
  - Year-level holiday overview
  - Quick year picker (1900-2100)
  - Today button for instant navigation
- **Bilingual Support**: Full localization in English and Vietnamese
- **Offline First**: All calculations happen on-device, no internet required
- **Modern UI/UX**:
  - Adaptive layouts for iPhone and iPad
  - Haptic feedback for interactions
  - Smooth animations and transitions
  - Dark/light theme support

## 🏗️ Architecture

### Tech Stack

- **Framework**: .NET MAUI (Multi-platform App UI) with .NET 10.0
- **Target Platforms**:
  - iOS 15.0+ (iPhone & iPad)
  - Android 8.0+ (API level 26+)
- **Languages**: C# 12, XAML
- **Patterns**: MVVM (Model-View-ViewModel)
- **Database**: SQLite (offline storage)
- **Localization**: .NET Resource files (.resx)

### Key Dependencies

- **Microsoft.Maui.Controls** - UI framework
- **CommunityToolkit.Mvvm** 8.2.2 - MVVM helpers
- **sqlite-net-pcl** 1.9.172 - Local database
- **ChineseLunisolarCalendar** - Built-in .NET lunar calculations

### Project Structure

```
lunarcalendar/
├── src/
│   ├── LunarCalendar.Core/           # Shared business logic
│   │   ├── Models/                   # Data models (Holiday, LunarDate)
│   │   ├── Services/                 # Calculation services
│   │   └── Data/                     # Holiday seed data
│   ├── LunarCalendar.MobileApp/      # .NET MAUI application
│   │   ├── Views/                    # XAML pages
│   │   ├── ViewModels/               # View models (MVVM)
│   │   ├── Services/                 # App services
│   │   ├── Data/                     # SQLite database
│   │   ├── Models/                   # App-specific models
│   │   ├── Converters/               # XAML value converters
│   │   ├── Resources/                # Images, fonts, strings
│   │   └── Platforms/                # Platform-specific code
│   └── LunarCalendar.Api/            # Backend API (optional)
└── docs/                             # Documentation
```

## 📋 Table of Contents
- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Development Scripts](#development-scripts)
- [Common Commands](#common-commands)
  - [Build Commands](#build-commands)
  - [Android Deployment](#android-deployment)
  - [iOS/iPad Deployment](#iosipad-deployment)
  - [Simulator Management](#simulator-management)
- [Development Workflow](#development-workflow)
- [Troubleshooting](#troubleshooting)

---

## Prerequisites

### Required
- **.NET 10.0 SDK** or later
- **macOS** (for iOS/iPad development)
  - Xcode 16.0+
  - iOS SDK 15.0+
- **Android SDK** (for Android development)
  - Android API level 26+ (Android 8.0+)
  - Target API level 36 (Android 14+)

### Development Environment
- **Visual Studio Code** with C# Dev Kit, or
- **Visual Studio 2022 for Mac** (17.6+), or
- **JetBrains Rider**

---

## Quick Start

### Clone and Build

```bash
# Clone the repository
git clone <repository-url>
cd lunarcalendar

# Restore dependencies
dotnet restore

# Build for Android
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-android

# Build for iOS
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-ios
```

### Run on Simulator

```bash
# Android (using helper script)
./deploy-android-sim.sh

# iOS iPhone (using helper script)
./deploy-ipad-auto.sh

# Or use the comprehensive multi-platform script
./deploy-multi-simulator.sh
```

---

## Development Scripts

The project includes several helper scripts for common development tasks:

### Android Scripts
- `build-android-release.sh` - Build release APK/AAB
- `deploy-android-sim.sh` - Deploy to Android emulator
- `rebuild-deploy-android-latest.sh` - Clean build and deploy to latest emulator
- `create-android-keystore.sh` - Create signing keystore

### iOS Scripts
- `build-ios-release.sh` - Build release IPA
- `deploy-ios-device.sh` - Deploy to physical iOS device
- `deploy-ipad-simulator.sh` - Deploy to iPad simulator
- `deploy-ipad-auto.sh` - Auto-deploy to iPad

### Utility Scripts
- `deploy-multi-simulator.sh` - Deploy to multiple simulators
- `verify-release-builds.sh` - Verify release builds
- `capture-logs.sh` / `capture-device-log.sh` - Capture app logs

---

## Common Commands

### Build Commands

#### Clean Build
```bash
# Clean all targets
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj

# Clean specific target
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-android
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-ios
```

#### Build for Android
```bash
# Debug build
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-android

# Release build (AAB for Play Store)
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net10.0-android -c Release
```

#### Build for iOS
```bash
# Simulator build (default)
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-ios

# Device build
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net10.0-ios -r ios-arm64

# Release build for App Store
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net10.0-ios -c Release -r ios-arm64
```

---

### Android Deployment

#### List Android Emulators
```bash
~/Library/Android/sdk/emulator/emulator -list-avds
```

#### Start Android Emulator
```bash
# Replace <emulator-name> with your emulator name
~/Library/Android/sdk/emulator/emulator -avd <emulator-name> &
```

#### Check Connected Android Devices
```bash
~/Library/Android/sdk/platform-tools/adb devices
```

#### Install on Android Emulator
```bash
# Uninstall old version (if exists)
~/Library/Android/sdk/platform-tools/adb uninstall com.huynguyen.lunarcalendar

# Install new version
~/Library/Android/sdk/platform-tools/adb install -r \
  src/LunarCalendar.MobileApp/bin/Debug/net10.0-android/com.huynguyen.lunarcalendar-Signed.apk
```

#### Launch on Android
```bash
# Using package name
~/Library/Android/sdk/platform-tools/adb shell monkey \
  -p com.huynguyen.lunarcalendar -c android.intent.category.LAUNCHER 1
```

#### Android Logs
```bash
# View logs
~/Library/Android/sdk/platform-tools/adb logcat

# View app-specific logs
~/Library/Android/sdk/platform-tools/adb logcat | grep "lunarcalendar"

# Clear logs
~/Library/Android/sdk/platform-tools/adb logcat -c
```

#### Force Stop Android App
```bash
~/Library/Android/sdk/platform-tools/adb shell am force-stop com.huynguyen.lunarcalendar
```

---

### iOS/iPad Deployment

#### List iOS Simulators
```bash
xcrun simctl list devices
```

#### List Available iOS Runtimes
```bash
xcrun simctl runtime list
```

#### Boot iPhone Simulator (iOS 18.2) - **Recommended**
```bash
# iPhone 15 Pro with iOS 18.2
xcrun simctl boot 4BEC1E56-9B92-4B3F-8065-04DDA5821951

# Open Simulator app
open -a Simulator
```

#### Boot iPad Simulator
```bash
# iPad Pro 13-inch
xcrun simctl boot D66062E4-3F8C-4709-B020-5F66809E3EDD

# Open Simulator app
open -a Simulator
```

#### Install on iPhone 15 Pro (iOS 18.2)
```bash
xcrun simctl install 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app
```

#### Install on iPad Pro
```bash
xcrun simctl install D66062E4-3F8C-4709-B020-5F66809E3EDD \
  src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app
```

#### Launch on iPhone/iPad
```bash
# iPhone 15 Pro
xcrun simctl launch 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  com.huynguyen.lunarcalendar

# iPad Pro
xcrun simctl launch D66062E4-3F8C-4709-B020-5F66809E3EDD \
  com.huynguyen.lunarcalendar
```

#### Uninstall from iPhone/iPad
```bash
# iPhone 15 Pro
xcrun simctl uninstall 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  com.huynguyen.lunarcalendar

# iPad Pro
xcrun simctl uninstall D66062E4-3F8C-4709-B020-5F66809E3EDD \
  com.huynguyen.lunarcalendar
```

#### Terminate App on iPhone/iPad
```bash
# iPhone 15 Pro
xcrun simctl terminate 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  com.huynguyen.lunarcalendar

# iPad Pro
xcrun simctl terminate D66062E4-3F8C-4709-B020-5F66809E3EDD \
  com.huynguyen.lunarcalendar
```

#### Take Screenshot
```bash
# iPhone 15 Pro
xcrun simctl io 4BEC1E56-9B92-4B3F-8065-04DDA5821951 screenshot ~/Desktop/iphone_screenshot.png

# iPad Pro
xcrun simctl io D66062E4-3F8C-4709-B020-5F66809E3EDD screenshot ~/Desktop/ipad_screenshot.png
```

#### iOS Simulator Logs
```bash
# iPhone 15 Pro
xcrun simctl spawn 4BEC1E56-9B92-4B3F-8065-04DDA5821951 log stream --predicate 'processImagePath CONTAINS "LunarCalendar"'
```

---

### Simulator Management

#### Create New iPhone Simulator with iOS 18.2
```bash
# iPhone 15 Pro
xcrun simctl create "iPhone 15 Pro (iOS 18.2)" \
  com.apple.CoreSimulator.SimDeviceType.iPhone-15-Pro \
  com.apple.CoreSimulator.SimRuntime.iOS-18-2

# iPhone 14 Pro
xcrun simctl create "iPhone 14 Pro (iOS 18.2)" \
  com.apple.CoreSimulator.SimDeviceType.iPhone-14-Pro \
  com.apple.CoreSimulator.SimRuntime.iOS-18-2
```

#### Shutdown Simulator
```bash
# iPhone 15 Pro
xcrun simctl shutdown 4BEC1E56-9B92-4B3F-8065-04DDA5821951

# iPad Pro
xcrun simctl shutdown D66062E4-3F8C-4709-B020-5F66809E3EDD

# Shutdown all
xcrun simctl shutdown all
```

#### Delete Simulator
```bash
# Replace <UUID> with simulator UUID
xcrun simctl delete <UUID>

# Delete all unavailable simulators
xcrun simctl delete unavailable
```

#### Erase Simulator (Reset to factory)
```bash
# iPhone 15 Pro
xcrun simctl erase 4BEC1E56-9B92-4B3F-8065-04DDA5821951

# iPad Pro
xcrun simctl erase D66062E4-3F8C-4709-B020-5F66809E3EDD
```

---

## Development Workflow

### Complete Development Workflow

#### 1. Build the App
```bash
# Build for both platforms
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj

# Or build specific platform
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-ios
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-android
```

#### 2. Deploy to iPhone (iOS 18.2) - Recommended
```bash
# Boot simulator
xcrun simctl boot 4BEC1E56-9B92-4B3F-8065-04DDA5821951
open -a Simulator

# Install and launch
xcrun simctl install 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app

xcrun simctl launch 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  com.huynguyen.lunarcalendar
```

#### 3. Deploy to iPad
```bash
# Boot simulator
xcrun simctl boot D66062E4-3F8C-4709-B020-5F66809E3EDD
open -a Simulator

# Install and launch
xcrun simctl install D66062E4-3F8C-4709-B020-5F66809E3EDD \
  src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app

xcrun simctl launch D66062E4-3F8C-4709-B020-5F66809E3EDD \
  com.huynguyen.lunarcalendar
```

#### 4. Deploy to Android
```bash
# Start emulator (if not running)
~/Library/Android/sdk/emulator/emulator -avd <your-emulator-name> &

# Wait for emulator to boot, then install
~/Library/Android/sdk/platform-tools/adb install -r \
  src/LunarCalendar.MobileApp/bin/Debug/net10.0-android/com.huynguyen.lunarcalendar-Signed.apk

# Launch
~/Library/Android/sdk/platform-tools/adb shell monkey \
  -p com.huynguyen.lunarcalendar -c android.intent.category.LAUNCHER 1
```

---

### Quick Restart Workflow (After Code Changes)

#### For iPhone 15 Pro (iOS 18.2)
```bash
# Rebuild
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-ios

# Terminate, reinstall, and launch
xcrun simctl terminate 4BEC1E56-9B92-4B3F-8065-04DDA5821951 com.huynguyen.lunarcalendar
xcrun simctl uninstall 4BEC1E56-9B92-4B3F-8065-04DDA5821951 com.huynguyen.lunarcalendar
xcrun simctl install 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app
xcrun simctl launch 4BEC1E56-9B92-4B3F-8065-04DDA5821951 com.huynguyen.lunarcalendar
```

#### For Android
```bash
# Rebuild
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-android

# Force stop, reinstall, and launch
~/Library/Android/sdk/platform-tools/adb shell am force-stop com.huynguyen.lunarcalendar
~/Library/Android/sdk/platform-tools/adb install -r \
  src/LunarCalendar.MobileApp/bin/Debug/net10.0-android/com.huynguyen.lunarcalendar-Signed.apk
~/Library/Android/sdk/platform-tools/adb shell monkey \
  -p com.huynguyen.lunarcalendar -c android.intent.category.LAUNCHER 1
```

---

### Using Helper Scripts (Recommended)

For faster development, use the provided helper scripts:

```bash
# Android - rebuild and deploy
./rebuild-deploy-android-latest.sh

# iOS - deploy to iPad
./deploy-ipad-auto.sh

# Multi-platform deployment
./deploy-multi-simulator.sh
```

---

## Troubleshooting

### General Issues

**Issue**: Build fails with "workload not found"
```bash
# Check installed workloads
dotnet workload list

# Install required workloads
dotnet workload install maui-ios maui-android
```

**Issue**: Dependency resolution errors
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj
```

---

### iOS Simulator Issues

**Issue**: Simulator shows black screen (iPhone with iOS 26.2)
- **Solution**: Use iPhone 15 Pro with iOS 18.2 (UUID: 4BEC1E56-9B92-4B3F-8065-04DDA5821951)
- See [IPHONE_FIX_COMPLETE.md](IPHONE_FIX_COMPLETE.md) for details

**Issue**: Simulator won't boot
```bash
# Shutdown all simulators
xcrun simctl shutdown all

# Boot specific simulator
xcrun simctl boot 4BEC1E56-9B92-4B3F-8065-04DDA5821951
```

**Issue**: App not updating after rebuild
```bash
# Uninstall app completely
xcrun simctl uninstall 4BEC1E56-9B92-4B3F-8065-04DDA5821951 com.huynguyen.lunarcalendar

# Clean and rebuild
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-ios
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-ios

# Reinstall
xcrun simctl install 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app
```

**Issue**: iOS device build fails with code signing errors
```bash
# List available signing identities
security find-identity -v -p codesigning

# Clean build artifacts
rm -rf src/LunarCalendar.MobileApp/bin src/LunarCalendar.MobileApp/obj

# Rebuild with device runtime
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net10.0-ios -r ios-arm64
```

---

### Android Emulator Issues

**Issue**: Emulator not starting
```bash
# Kill all emulator processes
pkill -9 qemu-system-x86_64

# Start emulator again
~/Library/Android/sdk/emulator/emulator -avd <emulator-name> &
```

**Issue**: ADB not detecting device
```bash
# Restart ADB server
~/Library/Android/sdk/platform-tools/adb kill-server
~/Library/Android/sdk/platform-tools/adb start-server
~/Library/Android/sdk/platform-tools/adb devices
```

**Issue**: App installation fails
```bash
# Uninstall first
~/Library/Android/sdk/platform-tools/adb uninstall com.huynguyen.lunarcalendar

# Try installing again
~/Library/Android/sdk/platform-tools/adb install -r \
  src/LunarCalendar.MobileApp/bin/Debug/net10.0-android/com.huynguyen.lunarcalendar-Signed.apk
```

---

### Build Issues

**Issue**: Build fails with dependency errors
```bash
# Restore dependencies
dotnet restore src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj

# Clean and rebuild
dotnet clean
dotnet build
```

**Issue**: "Cannot resolve type" error in XAML
```bash
# Clean build output and obj folders
rm -rf src/LunarCalendar.MobileApp/bin
rm -rf src/LunarCalendar.MobileApp/obj

# Rebuild
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj
```

---

## Important Device UUIDs

For quick reference:

| Device | UUID | iOS Version | Status |
|--------|------|-------------|--------|
| iPhone 15 Pro | `4BEC1E56-9B92-4B3F-8065-04DDA5821951` | iOS 18.2 | ✅ Recommended |
| iPad Pro 13-inch | `D66062E4-3F8C-4709-B020-5F66809E3EDD` | iOS 26.2 | ✅ Working |
| iPhone 17 Pro | `C7848DA7-CAE4-4E24-B95F-C458FFA74615` | iOS 26.2 | ❌ Black screen issue |

**Recommendation**: Always use **iPhone 15 Pro (iOS 18.2)** for iPhone testing to avoid compatibility issues.

---

## App Information

### Version
- **Current Version**: 1.0.1 (Build 2)
- **Bundle ID**: com.huynguyen.lunarcalendar
- **Supported Languages**: English, Vietnamese (default)

### Platform Support
- **iOS**: 15.0+ (iPhone and iPad)
- **Android**: 8.0+ (API level 26+)

---

## Design Decisions

### Offline-First Architecture
The app is designed to work completely offline by bundling all calculation logic on-device:
- **Lunar calculations**: Uses .NET's built-in `ChineseLunisolarCalendar`
- **Holiday data**: Seeded into SQLite database on first launch
- **No API dependency**: All features work without internet connection

### Localization Strategy
- Default language: Vietnamese (matches primary user base)
- Resource files (.resx) for both English and Vietnamese
- Dynamic language switching without app restart
- Localized holiday names and descriptions

### Performance Optimizations
- **Instant calculations**: No network latency
- **Efficient rendering**: Virtualized lists for large datasets
- **Smart caching**: SQLite for historical data
- **Release mode**: AOT compilation for iOS, R8 optimization for Android

---

## Contributing

### Code Style
- Follow C# conventions and .NET best practices
- Use MVVM pattern for UI logic
- Add XML documentation for public APIs
- Keep platform-specific code in Platforms/ folders

### Testing
- Manual testing on both iOS and Android required
- Test language switching thoroughly
- Verify offline functionality

---

## License

[Your License Here]

---

## Support & Documentation

For detailed guides and troubleshooting:
- [Privacy Policy](PRIVACY_POLICY.md)
- See project documentation in `docs/` folder
- Check `*.md` files in root for specific guides

---

## Acknowledgments

Built with:
- .NET MAUI - Cross-platform framework by Microsoft
- CommunityToolkit.Mvvm - MVVM helpers
- SQLite - Local database
- .NET ChineseLunisolarCalendar - Lunar date calculations

---

**Happy Coding! 🚀**
