#!/bin/bash
# Simplified iOS device deployment using Xcode integration

set -e

echo "📱 iOS Device Deployment Helper"
echo "================================"
echo ""

BUNDLE_ID="com.huynguyen.lunarcalendar"
PROJECT="src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"

echo "Step 1: Register App ID and Create Profile"
echo "-------------------------------------------"
echo ""
echo "We need to create a provisioning profile. Here's the easiest way:"
echo ""
echo "Option A - Using Apple Developer Portal (Recommended):"
echo "  1. Go to: https://developer.apple.com/account/resources/identifiers/list"
echo "  2. Click '+' to add new App ID"
echo "  3. Select 'App IDs' → Continue"
echo "  4. Description: Vietnamese Lunar Calendar"
echo "  5. Bundle ID: $BUNDLE_ID"
echo "  6. Click 'Continue' → 'Register'"
echo ""
echo "  Then create provisioning profile:"
echo "  7. Go to: https://developer.apple.com/account/resources/profiles/list"
echo "  8. Click '+' to add new profile"
echo "  9. Select 'iOS App Development' → Continue"
echo "  10. Select your App ID → Continue"
echo "  11. Select your certificate → Continue"
echo "  12. Select your devices → Continue"
echo "  13. Profile Name: LunarCalendar Development"
echo "  14. Click 'Generate'"
echo "  15. Download the .mobileprovision file"
echo "  16. Double-click the file to install"
echo ""
echo "Option B - Using Xcode (Automatic):"
echo "  1. Open Xcode"
echo "  2. File → New → Project"
echo "  3. Choose 'App' → Next"
echo "  4. Product Name: LunarCalendar"
echo "  5. Team: Select your team"
echo "  6. Bundle Identifier: $BUNDLE_ID"
echo "  7. Save project anywhere"
echo "  8. Connect your iPhone"
echo "  9. Select iPhone as target"
echo "  10. Click Run (▶️)"
echo "  11. Xcode will auto-create the profile!"
echo ""
echo "After completing either option, press Enter to continue..."
read

echo ""
echo "Step 2: Build for Device"
echo "------------------------"
echo ""

dotnet build "$PROJECT" \
    -f net8.0-ios \
    -c Debug \
    -p:RuntimeIdentifier=ios-arm64 \
    -p:MtouchLink=None \
    -p:CodesignKey="Apple Development: huydnguyen129@icloud.com (PCXAANVGF7)" \
    -p:EnableCodeSigning=true

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Build successful!"
    echo ""
    
    APP_BUNDLE=$(find src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/ios-arm64 -name "*.app" -type d | head -n 1)
    
    if [ -n "$APP_BUNDLE" ]; then
        echo "📦 App bundle: $APP_BUNDLE"
        echo ""
        echo "Step 3: Install on Device"
        echo "-------------------------"
        echo ""
        
        DEVICE_ID=$(xcrun xctrace list devices 2>/dev/null | grep -E "iPhone|iPad" | grep -v "Simulator" | head -n 1 | sed -E 's/.*\(([A-F0-9-]+)\).*/\1/')
        
        if [ -n "$DEVICE_ID" ]; then
            echo "Installing on device: $DEVICE_ID"
            
            if xcrun devicectl --help &> /dev/null; then
                xcrun devicectl device install app --device "$DEVICE_ID" "$APP_BUNDLE"
                echo ""
                echo "✅ App installed!"
            else
                echo "Using alternative installation method..."
                echo "Please install manually:"
                echo "  1. Open Xcode → Window → Devices and Simulators"
                echo "  2. Select your iPhone"
                echo "  3. Click '+' under Installed Apps"
                echo "  4. Select: $APP_BUNDLE"
            fi
        fi
    fi
else
    echo ""
    echo "❌ Build failed - provisioning profile still needed"
    echo ""
    echo "Please complete Step 1 above to create the profile."
fi

