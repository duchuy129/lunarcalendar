# Lunar Calendar Mobile App

Vietnamese Lunar Calendar mobile application built with .NET MAUI, supporting Android, iOS, and iPad.

## 📋 Table of Contents
- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Common Commands](#common-commands)
  - [API Server](#api-server)
  - [Build Commands](#build-commands)
  - [Android Deployment](#android-deployment)
  - [iOS/iPad Deployment](#iosipad-deployment)
  - [Simulator Management](#simulator-management)
- [Development Workflow](#development-workflow)
- [Troubleshooting](#troubleshooting)

---

## Prerequisites

- .NET 8.0 SDK
- Xcode (for iOS/iPad development)
- Android SDK (for Android development)
- Visual Studio Code or Visual Studio for Mac

---

## Quick Start

### 1. Start API Server
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar/src/LunarCalendar.Api
dotnet run
```
The API will start on: **http://localhost:5090**

### 2. Build the Mobile App
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# For Android
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-android

# For iOS
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-ios
```

### 3. Run on Simulators/Emulators
See platform-specific commands below.

---

## Common Commands

### API Server

#### Start API Server (Foreground)
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar/src/LunarCalendar.Api
dotnet run
```

#### Start API Server (Background)
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar/src/LunarCalendar.Api
dotnet run &
```

#### Check if API is Running
```bash
ps aux | grep "LunarCalendar.Api" | grep -v grep
```

#### Test API Endpoint
```bash
curl http://localhost:5090/api/calendar/2025/12
```

#### Stop API Server
```bash
# Find the process ID
ps aux | grep "LunarCalendar.Api" | grep -v grep

# Kill the process (replace <PID> with actual process ID)
kill <PID>

# Or kill all dotnet processes (use with caution)
pkill -f "LunarCalendar.Api"
```

---

### Build Commands

#### Clean Build
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Clean all targets
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj

# Clean specific target
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-android
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-ios
```

#### Build for Android
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-android
```

#### Build for iOS
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-ios
```

#### Build All Platforms
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj
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
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Uninstall old version (if exists)
~/Library/Android/sdk/platform-tools/adb uninstall com.companyname.lunarcalendar.mobileapp

# Install new version
~/Library/Android/sdk/platform-tools/adb install -r src/LunarCalendar.MobileApp/bin/Debug/net8.0-android/com.companyname.lunarcalendar.mobileapp-Signed.apk
```

#### Launch on Android
```bash
# Method 1: Using activity name
~/Library/Android/sdk/platform-tools/adb shell am start -n com.companyname.lunarcalendar.mobileapp/crc641e6c7ae8ff9c86da.MainActivity

# Method 2: Using package name
~/Library/Android/sdk/platform-tools/adb shell monkey -p com.companyname.lunarcalendar.mobileapp -c android.intent.category.LAUNCHER 1
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
~/Library/Android/sdk/platform-tools/adb shell am force-stop com.companyname.lunarcalendar.mobileapp
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
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

xcrun simctl install 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app
```

#### Install on iPad Pro
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

xcrun simctl install D66062E4-3F8C-4709-B020-5F66809E3EDD \
  src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app
```

#### Launch on iPhone 15 Pro (iOS 18.2)
```bash
xcrun simctl launch 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  com.companyname.lunarcalendar.mobileapp
```

#### Launch on iPad Pro
```bash
xcrun simctl launch D66062E4-3F8C-4709-B020-5F66809E3EDD \
  com.companyname.lunarcalendar.mobileapp
```

#### Uninstall from iPhone/iPad
```bash
# iPhone 15 Pro
xcrun simctl uninstall 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  com.companyname.lunarcalendar.mobileapp

# iPad Pro
xcrun simctl uninstall D66062E4-3F8C-4709-B020-5F66809E3EDD \
  com.companyname.lunarcalendar.mobileapp
```

#### Terminate App on iPhone/iPad
```bash
# iPhone 15 Pro
xcrun simctl terminate 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  com.companyname.lunarcalendar.mobileapp

# iPad Pro
xcrun simctl terminate D66062E4-3F8C-4709-B020-5F66809E3EDD \
  com.companyname.lunarcalendar.mobileapp
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

### Complete Workflow: Build and Deploy to All Platforms

#### 1. Start API Server
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar/src/LunarCalendar.Api
dotnet run &
```

#### 2. Build App
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj
```

#### 3. Deploy to iPhone (iOS 18.2) - Recommended
```bash
# Boot simulator
xcrun simctl boot 4BEC1E56-9B92-4B3F-8065-04DDA5821951
open -a Simulator

# Install and launch
xcrun simctl install 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app

xcrun simctl launch 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  com.companyname.lunarcalendar.mobileapp
```

#### 4. Deploy to iPad
```bash
# Boot simulator
xcrun simctl boot D66062E4-3F8C-4709-B020-5F66809E3EDD
open -a Simulator

# Install and launch
xcrun simctl install D66062E4-3F8C-4709-B020-5F66809E3EDD \
  src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app

xcrun simctl launch D66062E4-3F8C-4709-B020-5F66809E3EDD \
  com.companyname.lunarcalendar.mobileapp
```

#### 5. Deploy to Android
```bash
# Start emulator (if not running)
~/Library/Android/sdk/emulator/emulator -avd <your-emulator-name> &

# Wait for emulator to boot, then install
~/Library/Android/sdk/platform-tools/adb install -r \
  src/LunarCalendar.MobileApp/bin/Debug/net8.0-android/com.companyname.lunarcalendar.mobileapp-Signed.apk

# Launch
~/Library/Android/sdk/platform-tools/adb shell monkey -p com.companyname.lunarcalendar.mobileapp -c android.intent.category.LAUNCHER 1
```

---

### Quick Restart Workflow (After Code Changes)

#### For iPhone 15 Pro (iOS 18.2)
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Rebuild
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-ios

# Terminate, reinstall, and launch
xcrun simctl terminate 4BEC1E56-9B92-4B3F-8065-04DDA5821951 com.companyname.lunarcalendar.mobileapp
xcrun simctl uninstall 4BEC1E56-9B92-4B3F-8065-04DDA5821951 com.companyname.lunarcalendar.mobileapp
xcrun simctl install 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app
xcrun simctl launch 4BEC1E56-9B92-4B3F-8065-04DDA5821951 com.companyname.lunarcalendar.mobileapp
```

#### For Android
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Rebuild
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-android

# Force stop, reinstall, and launch
~/Library/Android/sdk/platform-tools/adb shell am force-stop com.companyname.lunarcalendar.mobileapp
~/Library/Android/sdk/platform-tools/adb install -r \
  src/LunarCalendar.MobileApp/bin/Debug/net8.0-android/com.companyname.lunarcalendar.mobileapp-Signed.apk
~/Library/Android/sdk/platform-tools/adb shell monkey -p com.companyname.lunarcalendar.mobileapp -c android.intent.category.LAUNCHER 1
```

---

## Troubleshooting

### API Server Issues

**Issue**: API not responding
```bash
# Check if API is running
ps aux | grep "LunarCalendar.Api" | grep -v grep

# Check if port 5090 is in use
lsof -i :5090

# Test API directly
curl http://localhost:5090/api/calendar/2025/12
```

**Issue**: Port 5090 already in use
```bash
# Find process using port 5090
lsof -i :5090

# Kill the process (replace <PID>)
kill <PID>
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
xcrun simctl uninstall 4BEC1E56-9B92-4B3F-8065-04DDA5821951 com.companyname.lunarcalendar.mobileapp

# Clean and rebuild
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-ios
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-ios

# Reinstall
xcrun simctl install 4BEC1E56-9B92-4B3F-8065-04DDA5821951 \
  src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app
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
~/Library/Android/sdk/platform-tools/adb uninstall com.companyname.lunarcalendar.mobileapp

# Try installing again
~/Library/Android/sdk/platform-tools/adb install -r \
  src/LunarCalendar.MobileApp/bin/Debug/net8.0-android/com.companyname.lunarcalendar.mobileapp-Signed.apk
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

## Project Structure

```
lunarcalendar/
├── src/
│   ├── LunarCalendar.Api/              # Backend API (ASP.NET Core)
│   └── LunarCalendar.MobileApp/        # Mobile App (.NET MAUI)
│       ├── Platforms/
│       │   ├── Android/
│       │   └── iOS/
│       ├── Views/                      # XAML Pages
│       ├── ViewModels/                 # View Models
│       ├── Services/                   # Business Logic
│       ├── Models/                     # Data Models
│       └── Converters/                 # Value Converters
├── README.md                           # This file
├── IPHONE_FIX_COMPLETE.md             # iPhone black screen fix
└── INSTALL_IOS18_RUNTIME.md           # iOS 18.2 runtime installation guide
```

---

## Features

- ✅ Vietnamese Lunar Calendar
- ✅ Gregorian to Lunar date conversion
- ✅ Vietnamese Holidays with color-coded display
- ✅ Month navigation (Previous/Next/Today)
- ✅ Year navigation for holidays
- ✅ Year picker to jump to specific year
- ✅ Dual date display (Gregorian + Lunar)
- ✅ Guest mode / User mode toggle
- ✅ Cross-platform: Android, iPhone, iPad

---

## API Endpoints

Base URL: `http://localhost:5090`

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/calendar/{year}/{month}` | GET | Get calendar for specific month |
| `/api/holidays` | GET | Get all holidays |
| `/api/holidays/{year}` | GET | Get holidays for specific year |

---

## License

[Your License Here]

## Support

For issues and questions:
- See [IPHONE_FIX_COMPLETE.md](IPHONE_FIX_COMPLETE.md) for iPhone simulator issues
- See [INSTALL_IOS18_RUNTIME.md](INSTALL_IOS18_RUNTIME.md) for iOS runtime installation

---

**Happy Coding! 🚀**
