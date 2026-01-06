# iOS Physical Device Crash Fix

## Problem
The app was crashing immediately after showing the splash screen on physical iPhone devices.

## Root Cause
**SQLite linker issue** - When building for physical iOS devices with the managed linker enabled (`MtouchLink=SdkOnly`), the SQLite assemblies were being stripped out, causing the app to crash when trying to initialize the database.

## Solution Applied

### 1. Created Linker Preservation Config
Created [Platforms/iOS/LinkerConfig.xml](src/LunarCalendar.MobileApp/Platforms/iOS/LinkerConfig.xml) to preserve SQLite assemblies:

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<linker>
  <!-- Preserve all SQLite types -->
  <assembly fullname="SQLite-net">
    <type fullname="*" />
  </assembly>
  <assembly fullname="SQLitePCLRaw.core">
    <type fullname="*" />
  </assembly>
  <assembly fullname="SQLitePCLRaw.batteries_v2">
    <type fullname="*" />
  </assembly>

  <!-- Preserve app data models -->
  <assembly fullname="LunarCalendar.MobileApp">
    <namespace fullname="LunarCalendar.MobileApp.Data.Models" preserve="all" />
  </assembly>
</linker>
```

### 2. Added SQLite Initialization in AppDelegate
Updated [Platforms/iOS/AppDelegate.cs](src/LunarCalendar.MobileApp/Platforms/iOS/AppDelegate.cs):

```csharp
public override bool FinishedLaunching(UIKit.UIApplication application, NSDictionary launchOptions)
{
    // Initialize SQLite for iOS
    SQLitePCL.Batteries_V2.Init();

    return base.FinishedLaunching(application, launchOptions);
}
```

### 3. Updated Build Configuration
Modified build command to include linker config:

```bash
dotnet build \
    -p:MtouchLink=SdkOnly \
    -p:MtouchExtraArgs="--xml=Platforms/iOS/LinkerConfig.xml" \
    -p:CodesignKey="Apple Development: Huy Nguyen (PCXAANVGF7)" \
    -p:CodesignProvision="4619e0b0-9d0d-49cf-bbd9-150f949b107b"
```

## Files Changed
1. ✅ `src/LunarCalendar.MobileApp/Platforms/iOS/LinkerConfig.xml` - Created
2. ✅ `src/LunarCalendar.MobileApp/Platforms/iOS/AppDelegate.cs` - Added SQLite init
3. ✅ `src/LunarCalendar.MobileApp/Platforms/iOS/Entitlements.plist` - Created
4. ✅ `test-ios-device.sh` - Updated with linker config

## Result
✅ App now launches successfully on physical iPhone devices without crashing!

## Key Learnings
- **Always test on physical devices** - Simulators don't catch linker issues
- **SQLite requires explicit preservation** on iOS when using the managed linker
- **Initialize SQLite early** in the app lifecycle (AppDelegate.FinishedLaunching)
- **Use `MtouchLink=SdkOnly`** for stable device builds (not `None`)

## Testing
Tested on:
- Device: Huy's iPhone (iOS 26.1)
- Build: Debug configuration
- Bundle ID: com.huynguyen.lunarcalendar
- Provisioning: Development profile

## Future Deployments
Simply run:
```bash
./test-ios-device.sh
```

The script now includes all necessary fixes.
