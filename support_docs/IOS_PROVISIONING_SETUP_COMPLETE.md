# iOS Provisioning Profile Setup - Next Steps

## ✅ What Just Happened

When you double-clicked the `.mobileprovision` file:

1. **macOS installed it** to: `~/Library/MobileDevice/Provisioning Profiles/`
2. **Xcode opened** (this is normal - macOS automatically opens .mobileprovision files in Xcode)
3. **The profile is now available** for code signing

## ✅ What I Just Did

I updated your project file ([LunarCalendar.MobileApp.csproj](src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj)) with:

```xml
<!-- iOS Release Code Signing for App Store -->
<PropertyGroup Condition="'$(Configuration)' == 'Release' AND '$(TargetFramework)' == 'net10.0-ios' AND '$(RuntimeIdentifier)' == 'ios-arm64'">
    <!-- Code Signing -->
    <CodesignKey>Apple Distribution</CodesignKey>
    <CodesignProvision>Lunar Calendar App Store</CodesignProvision>
    <CodesignEntitlements>Platforms/iOS/Entitlements.plist</CodesignEntitlements>

    <!-- Build Settings -->
    <ArchiveOnBuild>true</ArchiveOnBuild>
    <BuildIpa>true</BuildIpa>
    <EnableCodeSigning>true</EnableCodeSigning>
</PropertyGroup>
```

This tells .NET MAUI to:
- Use "Apple Distribution" certificate for signing
- Use "Lunar Calendar App Store" provisioning profile
- Create an IPA file when building Release mode

---

## ⚠️ IMPORTANT: Verify Provisioning Profile Name

The configuration I added assumes your provisioning profile is named **"Lunar Calendar App Store"**.

**If you used a different name**, you need to update line 97 in the .csproj file.

### Check Your Profile Name

```bash
# Option 1: Quick check - list provisioning profile files
ls -la ~/Library/MobileDevice/Provisioning\ Profiles/

# Option 2: See the actual profile names
security cms -D -i ~/Library/MobileDevice/Provisioning\ Profiles/*.mobileprovision 2>/dev/null | grep -A 1 "<key>Name</key>" | grep "<string>" | sed 's/.*<string>\(.*\)<\/string>/\1/'
```

**If the name is different**, update this line in your .csproj:
```xml
<CodesignProvision>YOUR_ACTUAL_PROFILE_NAME_HERE</CodesignProvision>
```

---

## 🎯 What to Do Next

### Option 1: You Can Close Xcode Now ✅

**You don't need Xcode** for building .NET MAUI apps. Xcode only opened because macOS associates `.mobileprovision` files with it.

**Close Xcode** - we'll use the command line to build.

### Option 2: Verify Everything is Set Up (Optional)

```bash
# 1. Check certificates
security find-identity -v -p codesigning

# You should see:
# - "Apple Development: ..." (for Debug builds)
# - "Apple Distribution: ..." (for Release builds)

# 2. Check provisioning profiles
ls -la ~/Library/MobileDevice/Provisioning\ Profiles/*.mobileprovision

# You should see at least 2 files:
# - Development profile (for Debug)
# - Distribution/App Store profile (for Release)
```

---

## 🔨 Ready to Build iOS Release

Now you're ready to build the iOS release IPA for App Store submission!

### Build Command

```bash
# Navigate to project root (if not already there)
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Run the build script
./build-ios-release.sh
```

**What this will do:**
1. Clean previous builds
2. Build your app in Release mode
3. Sign it with "Apple Distribution" certificate
4. Use "Lunar Calendar App Store" provisioning profile
5. Create an IPA file ready for App Store submission

**Expected output location:**
```
src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/publish/*.ipa
```

---

## 🚨 Troubleshooting

### If Build Fails with "Provisioning profile not found"

**Problem:** The profile name in .csproj doesn't match the actual profile name.

**Solution:**
1. Find your actual profile name (see "Check Your Profile Name" above)
2. Update line 97 in [src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj](src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj):
   ```xml
   <CodesignProvision>EXACT_NAME_FROM_STEP_1</CodesignProvision>
   ```
3. Try building again

### If Build Fails with "Certificate not found"

**Problem:** "Apple Distribution" certificate is not installed.

**Solution:**
1. Go back to Apple Developer Portal: https://developer.apple.com/account/resources/certificates
2. Create "Apple Distribution" certificate (if not created)
3. Download and double-click to install
4. Try building again

### If Build Succeeds But No IPA File

**Problem:** IPA creation might be disabled.

**Solution:**
Check that these lines are in your .csproj (I already added them):
```xml
<ArchiveOnBuild>true</ArchiveOnBuild>
<BuildIpa>true</BuildIpa>
```

---

## 📋 Complete Setup Checklist

Before building, ensure you have:

- [x] **Apple Developer Account** - Active and paid
- [x] **Development Certificate** - "Apple Development" (for testing)
- [ ] **Distribution Certificate** - "Apple Distribution" (for App Store)
- [x] **App ID** - com.huynguyen.lunarcalendar registered
- [x] **Development Profile** - Installed (for Debug builds)
- [x] **Distribution Profile** - Installed (for Release builds)
- [x] **Project Configuration** - Updated with Release signing settings ✅ Just added!

**Still need:**
- [ ] Distribution Certificate (if not created yet)

---

## 🎯 Quick Summary - Next Steps

1. **Close Xcode** (you don't need it)
2. **Verify profile name** matches what you created (see command above)
3. **Update .csproj if needed** (line 97)
4. **Create Distribution Certificate** (if not done yet - see MVP_RELEASE_GUIDE_2026.md Section 1.1)
5. **Run build script**: `./build-ios-release.sh`
6. **Upload IPA to App Store Connect** using Transporter app

---

## 📖 Related Documentation

- **Complete Guide:** [MVP_RELEASE_GUIDE_2026.md](MVP_RELEASE_GUIDE_2026.md)
- **Asset Preparation:** [ASSET_PREPARATION_CHECKLIST.md](ASSET_PREPARATION_CHECKLIST.md)
- **Quick Start:** [RELEASE_QUICK_START.md](RELEASE_QUICK_START.md)

---

**Last Updated:** January 4, 2026
