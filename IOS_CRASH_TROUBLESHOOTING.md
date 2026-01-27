# iOS 26.1 Immediate Crash - Troubleshooting Guide

## Status: App crashes immediately on launch with no visible logs

---

## Fixes Applied So Far

### ✅ Fix 1: Removed Early FileSystem Access
- Moved FileSystem.CacheDirectory access to factory pattern in MauiProgram
- Added lazy initialization to LogService
- **Result:** Build succeeds, but app still crashes

### ✅ Fix 2: Removed ServiceHelper Access from AppDelegate
- Removed LogService calls before DI container creation
- Using Debug.WriteLine for early crash logging
- **Result:** Build succeeds, but app still crashes

### ✅ Fix 3: Removed Page Finalizers
- Removed destructors from CalendarPage and YearHolidaysPage
- **Result:** Build succeeds, but app still crashes

---

## How to Capture Crash Logs

### Method 1: Xcode Device Console (RECOMMENDED)
```bash
1. Connect iOS device via USB
2. Open Xcode
3. Window > Devices and Simulators (Shift+Cmd+2)
4. Select your device
5. Click "Open Console" at the bottom
6. Deploy and run the app
7. Watch for crash output in real-time
8. Copy ALL output and send it to me
```

### Method 2: Use Provided Script
```bash
# Run after the app crashes
./capture-ios-crash-log.sh
```

### Method 3: Mac Console.app
```bash
1. Open Console.app on Mac
2. Select iOS device in left sidebar
3. Click "Start" to begin streaming
4. Run the app on device
5. Look for "=== FATAL CRASH ===" messages
6. Export log
```

---

## Additional Fixes to Try

### Try 1: Remove SQLite Initialization
The crash might be in SQLitePCL.Batteries_V2.Init(). Let's test without it:

**File:** `Platforms/iOS/AppDelegate.cs`
```csharp
public override bool FinishedLaunching(UIKit.UIApplication application, NSDictionary launchOptions)
{
    try
    {
        // TEMPORARILY COMMENT OUT SQLite init
        // SQLitePCL.Batteries_V2.Init();

        return base.FinishedLaunching(application, launchOptions);
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"CRASH: {ex}");
        throw;
    }
}
```

**Test:** If app launches without crash, the issue is SQLite initialization.

---

### Try 2: Minimal Info.plist
The crash might be due to missing iOS 26 keys in Info.plist.

**Add to Info.plist:**
```xml
<!-- iOS 26 compatibility -->
<key>CFBundleVersion</key>
<string>1</string>
<key>CFBundleShortVersionString</key>
<string>1.0</string>
<key>MinimumOSVersion</key>
<string>15.0</string>
```

---

### Try 3: Remove Privacy Manifest Temporarily
The PrivacyInfo.xcprivacy might have syntax issues for iOS 26.

```bash
# Temporarily rename the file
mv src/LunarCalendar.MobileApp/Platforms/iOS/PrivacyInfo.xcprivacy \
   src/LunarCalendar.MobileApp/Platforms/iOS/PrivacyInfo.xcprivacy.bak

# Comment out in .csproj
# <BundleResource Include="Platforms\iOS\PrivacyInfo.xcprivacy" />

# Rebuild and test
dotnet build -f net10.0-ios -c Debug
```

---

### Try 4: Check for Native Library Issues
The crash might be a missing or incompatible native library.

**Check build output for warnings:**
```bash
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
    -f net10.0-ios -c Debug -v detailed 2>&1 | grep -i "warning\|sqlite\|native"
```

---

### Try 5: Minimal AppDelegate
Replace entire AppDelegate with absolute minimum:

```csharp
using Foundation;
using System.Diagnostics;

namespace LunarCalendar.MobileApp;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp()
    {
        return MauiProgram.CreateMauiApp();
    }
}
```

---

## Common iOS 26 Crash Causes

### 1. Missing Entitlements
**Check:** `Platforms/iOS/Entitlements.plist` exists and is valid

### 2. Invalid Bundle Identifier
**Check:** Bundle ID matches between:
- Info.plist (CFBundleIdentifier)
- Entitlements.plist
- Provisioning profile

### 3. Code Signing Issues
```bash
# Check code signing
codesign -vv -d path/to/LunarCalendar.MobileApp.app
```

### 4. Missing Frameworks
**Check .csproj for:**
```xml
<ItemGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">
    <!-- Any custom frameworks? -->
</ItemGroup>
```

### 5. iOS 26 API Changes
Some APIs deprecated or removed in iOS 26. Check if any of these are used:
- UIWebView (use WKWebView)
- Certain deprecated UIKit methods

---

## Debugging Steps (In Order)

1. **Get crash logs first** (use Method 1 above)
   - Without logs, we're guessing

2. **Try minimal AppDelegate** (Try 5)
   - If this works, issue is in our AppDelegate code

3. **Remove SQLite init** (Try 1)
   - If this works, issue is SQLite native library

4. **Remove Privacy Manifest** (Try 3)
   - If this works, issue is manifest syntax

5. **Check for native library warnings** (Try 4)
   - Look for missing .dylib or .framework files

---

## What to Send Me

Please provide:
1. **Full Xcode Console output** from app launch to crash
2. **Build output** (verbose):
   ```bash
   dotnet build -f net10.0-ios -v detailed > build.log 2>&1
   ```
3. **Device info:**
   - Exact iOS version (26.1 or 26.2?)
   - Device model (iPhone 15, 16 Pro, etc.)
   - Architecture (arm64)

4. **Crash report** from Settings > Privacy > Analytics

---

## Quick Diagnostic Commands

```bash
# Check if app bundle was created
ls -la src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/*/

# Check for SQLite native libraries
find src/LunarCalendar.MobileApp -name "*sqlite*" -type f

# Check .csproj for iOS-specific config
grep -A 5 "ios" src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj

# Check for any hardcoded paths
grep -r "FileSystem\." src/LunarCalendar.MobileApp --include="*.cs" | grep -v "// "
```

---

## Emergency Rollback

If you need to roll back all my changes:

```bash
git log --oneline | head -10  # Find commit before my changes
git checkout <commit-hash> .
git restore --staged .
```

---

## My Best Guesses (Without Logs)

**Most Likely:**
1. **SQLite native library issue** - iOS 26 might need updated SQLite
2. **Missing entitlements** - App trying to access restricted APIs
3. **Code signing** - Invalid provisioning profile for iOS 26

**Less Likely:**
4. FileSystem API issue (we fixed this)
5. DI container issue (we fixed this)
6. Privacy manifest syntax (possible but unlikely)

---

**NEXT STEP:** Please run Xcode Device Console and send me the full crash output!
