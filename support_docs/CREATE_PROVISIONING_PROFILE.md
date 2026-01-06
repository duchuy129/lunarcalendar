# Create iOS Provisioning Profile - Quick Guide

Your certificates are ready, but you need a provisioning profile to deploy to your iPhone.

## Quick Method (5 minutes)

### Option 1: Using Xcode (Easiest - Recommended)

1. **Open Xcode** (not VS Code)

2. **Create a temporary project:**
   - File → New → Project
   - Choose "App" template → Next
   - Fill in:
     - Product Name: `LunarCalendar`
     - Team: **Select your Apple Developer account**
     - Organization Identifier: `com.huynguyen`
     - Bundle Identifier: `com.huynguyen.lunarcalendar`
     - Interface: SwiftUI
     - Language: Swift
   - Click Next, save anywhere (e.g., Desktop)

3. **Connect your iPhone** via USB
   - Make sure it's unlocked
   - Trust this computer if prompted

4. **Select your iPhone** as the build target:
   - Top bar: Click on the device dropdown
   - Select "Huy's iPhone"

5. **Build the project:**
   - Click the ▶️ Play button (or press Cmd+R)
   - Xcode will automatically:
     - Register the App ID
     - Create a provisioning profile
     - Install the profile on your Mac
     - Deploy to your iPhone

6. **You'll see a trust prompt on your iPhone:**
   - Settings → General → VPN & Device Management
   - Tap your Apple ID
   - Tap "Trust"

7. **Done!** You can now close and delete this temp project.

### Option 2: Using Apple Developer Portal (Manual)

1. **Register App ID:**
   - Go to: https://developer.apple.com/account/resources/identifiers/list
   - Click "+" → App IDs → Continue
   - Description: `Vietnamese Lunar Calendar`
   - Bundle ID: `com.huynguyen.lunarcalendar` (Explicit)
   - Click Continue → Register

2. **Create Provisioning Profile:**
   - Go to: https://developer.apple.com/account/resources/profiles/list
   - Click "+" → iOS App Development → Continue
   - Select your App ID → Continue
   - Select your certificate (Apple Development: huydnguyen129@icloud.com) → Continue
   - Select your iPhone device → Continue
   - Profile Name: `LunarCalendar Development`
   - Click Generate

3. **Download and Install:**
   - Download the `.mobileprovision` file
   - Double-click it to install
   - Verify: `ls ~/Library/MobileDevice/Provisioning\ Profiles/`

## After Creating the Profile

Run the deployment script:

```bash
./test-ios-device.sh
```

The app will build and install on your iPhone!

## Verification

Check if profile was created:
```bash
ls ~/Library/MobileDevice/Provisioning\ Profiles/
```

You should see a `.mobileprovision` file.

## Common Issues

**"No provisioning profiles found"**
- Make sure you completed Option 1 or 2 above
- Double-click the .mobileprovision file if you downloaded it manually

**"Untrusted Developer" on iPhone**
- Settings → General → VPN & Device Management
- Tap your Apple ID → Trust

**"Device not registered"**
- Add your device at: https://developer.apple.com/account/resources/devices/list
- Use the UDID from: Window → Devices and Simulators in Xcode
