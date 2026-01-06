# Installing iOS 18.0 Simulator Runtime for iPhone Testing

## Problem
Your Xcode 26.2 beta only includes iOS 26.2 runtime, which has a known incompatibility with .NET 8 MAUI on iPhone simulators.

## Solution
Install iOS 18.0 (or 17.5) simulator runtime alongside iOS 26.2.

---

## Method 1: Using Xcode Settings (Easiest - If Available)

1. **Open Xcode 26.2**
2. **Go to Xcode > Settings (or Preferences)**
3. **Click on "Platforms" tab**
4. **Look for "iOS" section**
5. **Click the "+" button or "Get" button next to older iOS versions**
6. **Select iOS 18.0 or iOS 17.5**
7. **Click "Download & Install"**
8. **Wait for download to complete** (this can take 10-30 minutes depending on your internet speed)

After installation, you'll be able to create iPhone simulators with iOS 18.0.

---

## Method 2: Download Manually from Apple (If Method 1 doesn't show iOS 18)

### Step 1: Download iOS 18.0 Runtime

1. **Go to Apple Developer Downloads**
   - Visit: https://developer.apple.com/download/all/
   - Log in with your Apple Developer account

2. **Search for "iOS 18" in the search box**

3. **Download one of these:**
   - **iOS 18.0 Simulator Runtime** (recommended)
   - **iOS 17.5 Simulator Runtime** (alternative)

   The file will be named something like:
   - `iOS_18.0_Simulator_Runtime.dmg` (7-8 GB)
   - `iOS_17.5_Simulator_Runtime.dmg`

4. **Wait for download to complete**

### Step 2: Install the Runtime

Once downloaded, open Terminal and run:

```bash
# If you downloaded iOS 18.0 runtime
xcrun simctl runtime add ~/Downloads/iOS_18.0_Simulator_Runtime.dmg

# OR if you downloaded iOS 17.5 runtime
xcrun simctl runtime add ~/Downloads/iOS_17.5_Simulator_Runtime.dmg
```

**Note**: Replace the path if you downloaded to a different location.

The installation will:
- Verify the image
- Mount it
- Add it to available runtimes (takes 5-10 minutes)

### Step 3: Verify Installation

Check that iOS 18.0 is now available:

```bash
xcrun simctl runtime list
```

You should see both iOS 26.2 and iOS 18.0 in the list:
```
== Disk Images ==
-- iOS --
iOS 18.0 (22A3351) - XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX (Ready)
iOS 26.2 (23C54) - 5247F920-EAB1-4BF6-850B-DBA30F750BFB (Ready)
```

---

## Step 4: Create iPhone Simulator with iOS 18.0

Once iOS 18.0 runtime is installed, create a new iPhone simulator:

```bash
# List available device types
xcrun simctl list devicetypes | grep iPhone

# Create iPhone 15 Pro with iOS 18.0
xcrun simctl create "iPhone 15 Pro (iOS 18)" com.apple.CoreSimulator.SimDeviceType.iPhone-15-Pro com.apple.CoreSimulator.SimRuntime.iOS-18-0

# Or create iPhone 14 Pro with iOS 18.0
xcrun simctl create "iPhone 14 Pro (iOS 18)" com.apple.CoreSimulator.SimDeviceType.iPhone-14-Pro com.apple.CoreSimulator.SimRuntime.iOS-18-0
```

**Note**: The exact runtime identifier may vary. Use the output from `xcrun simctl runtime list` to get the correct identifier.

---

## Step 5: Boot the New Simulator

```bash
# List all devices to find your new iPhone's UUID
xcrun simctl list devices | grep "iPhone.*iOS 18"

# Boot it (replace UUID with your device's UUID)
xcrun simctl boot <DEVICE-UUID>

# Open Simulator app
open -a Simulator
```

---

## Step 6: Deploy Your App

Now deploy your Lunar Calendar app to the new iOS 18.0 iPhone simulator:

```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Build for iOS
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-ios

# Install on the new simulator (replace UUID)
xcrun simctl install <DEVICE-UUID> src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/iossimulator-arm64/LunarCalendar.MobileApp.app

# Launch the app
xcrun simctl launch <DEVICE-UUID> com.companyname.lunarcalendar.mobileapp
```

The app should now work correctly on the iPhone simulator with iOS 18.0! ✅

---

## Alternative: Use Xcode Simulator Manager

After installing iOS 18.0 runtime:

1. **Open Xcode**
2. **Go to Window > Devices and Simulators**
3. **Click the "Simulators" tab**
4. **Click the "+" button in the bottom left**
5. **Configure:**
   - **Device Type**: iPhone 15 Pro (or any iPhone model)
   - **OS Version**: iOS 18.0
   - **Name**: iPhone 15 Pro (iOS 18)
6. **Click "Create"**

Then use this simulator from Visual Studio or command line.

---

## Troubleshooting

### Issue: "Runtime not found" error
**Solution**: Make sure you used the exact runtime identifier from `xcrun simctl runtime list`

### Issue: Download taking too long
**Solution**: The runtime files are 7-8 GB. Ensure stable internet connection. Download can take 30-60 minutes.

### Issue: "Invalid image" error
**Solution**: Re-download the runtime DMG file. The download may have been corrupted.

### Issue: Not enough disk space
**Solution**: You need at least 15 GB free space. Delete old Xcode simulators or runtimes:
```bash
# List runtimes
xcrun simctl runtime list

# Delete old runtime (use the UUID from the list)
xcrun simctl runtime delete <RUNTIME-UUID>
```

---

## Cleanup (Optional)

If you want to remove iOS 26.2 beta runtime later (after confirming iOS 18 works):

```bash
# List runtimes to get the UUID
xcrun simctl runtime list

# Delete iOS 26.2 runtime
xcrun simctl runtime delete 5247F920-EAB1-4BF6-850B-DBA30F750BFB
```

**Warning**: Only do this if you're sure you don't need iOS 26.2 beta for other testing.

---

## Expected Result

After following these steps:
- ✅ iPhone simulators with iOS 18.0 will work perfectly
- ✅ Lunar Calendar app will display correctly on iPhone
- ✅ All new features (year holidays section) will be visible
- ✅ You can test iPhone-specific UI/UX
- ✅ You keep iOS 26.2 for testing newer features

This gives you the best of both worlds - stable iOS 18 for development and bleeding-edge iOS 26.2 for future compatibility testing.
