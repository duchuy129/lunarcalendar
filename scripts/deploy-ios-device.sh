#!/bin/bash
# Deploy to Physical iOS Device - Works with Paid Apple Developer Account
# Vietnamese Lunar Calendar

set -e

echo "📱 Deploy to Physical iOS Device (Paid Developer Account)"
echo "=========================================================="
echo ""

# Colors
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
BLUE='\033[0;34m'
NC='\033[0m'

# Check for connected device
echo "🔍 Checking for connected iOS devices..."
CONNECTED_DEVICES=$(xcrun xctrace list devices 2>/dev/null | grep -E "iPhone|iPad" | grep -v "Simulator" || echo "")

if [ -z "$CONNECTED_DEVICES" ]; then
    echo -e "${RED}❌ No iOS devices connected${NC}"
    echo "Please connect your iPhone or iPad via USB"
    exit 1
fi

echo -e "${GREEN}✓ Found device(s):${NC}"
echo "$CONNECTED_DEVICES"
DEVICE_ID=$(echo "$CONNECTED_DEVICES" | head -n 1 | sed -E 's/.*\(([A-F0-9-]+)\).*/\1/')
echo ""

# Check for Apple Development certificate
echo "🔐 Checking for Apple Development certificate..."
CERT=$(security find-identity -v -p codesigning | grep "Apple Development" | head -n 1 || echo "")

if [ -z "$CERT" ]; then
    echo -e "${RED}❌ No Apple Development certificate found${NC}"
    echo ""
    echo "Please set up your certificate first:"
    echo "  1. Open Xcode → Settings → Accounts"
    echo "  2. Select your Apple ID → Click 'Manage Certificates'"
    echo "  3. Click '+' and add 'Apple Development' certificate"
    echo ""
    echo "OR run this command to open Xcode Settings:"
    echo "  open -a Xcode"
    echo ""
    exit 1
fi

CERT_NAME=$(echo "$CERT" | sed 's/^[^"]*"//' | sed 's/".*//')
echo -e "${GREEN}✓ Found certificate:${NC} $CERT_NAME"
echo ""

# Check for provisioning profile
echo "📋 Checking for provisioning profiles..."
PROFILES_DIR="$HOME/Library/MobileDevice/Provisioning Profiles"
PROFILE_COUNT=$(ls "$PROFILES_DIR" 2>/dev/null | wc -l | tr -d ' ')

if [ "$PROFILE_COUNT" -eq "0" ]; then
    echo -e "${YELLOW}⚠️  No provisioning profiles found${NC}"
    echo ""
    echo "Xcode will attempt to create one automatically during build."
    echo "If build fails, you need to:"
    echo ""
    echo "  1. Open: https://developer.apple.com/account/resources/profiles"
    echo "  2. Create 'iOS App Development' profile for:"
    echo "     - App ID: com.huynguyen.lunarcalendar"
    echo "     - Device: $DEVICE_ID"
    echo "  3. Download and double-click to install"
    echo ""
    read -p "Press Enter to continue with automatic provisioning..."
    echo ""
else
    echo -e "${GREEN}✓ Found $PROFILE_COUNT provisioning profile(s)${NC}"
    echo ""
fi

# Build
PROJECT_PATH="src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"

echo "🔨 Building for iOS device..."
echo "   Target: Physical Device (ios-arm64)"
echo "   Configuration: Debug"
echo ""

# Build with explicit parameters for device
dotnet build "$PROJECT_PATH" \
    -f net8.0-ios \
    -c Debug \
    -p:RuntimeIdentifier=ios-arm64 \
    -p:CodesignKey="$CERT_NAME" \
    -p:CodesignProvision="" \
    -p:IpaPackageName="" \
    -p:BuildIpa=false \
    -p:EnableCodeSigning=true

BUILD_EXIT_CODE=$?

if [ $BUILD_EXIT_CODE -ne 0 ]; then
    echo ""
    echo -e "${RED}❌ Build failed!${NC}"
    echo ""
    echo "Common issues:"
    echo "  1. No provisioning profile - Create one at developer.apple.com"
    echo "  2. Device not registered - Add UDID $DEVICE_ID to developer portal"
    echo "  3. App ID not created - Create com.huynguyen.lunarcalendar at portal"
    echo ""
    echo "Quick fix: Create a dummy project in Xcode with bundle ID"
    echo "           'com.huynguyen.lunarcalendar' and run it on your device."
    echo "           Xcode will auto-create everything needed."
    exit 1
fi

echo ""
echo -e "${GREEN}✅ Build successful!${NC}"
echo ""

# Find app bundle
APP_BUNDLE=$(find src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/ios-arm64 -name "*.app" -type d 2>/dev/null | head -n 1)

if [ -z "$APP_BUNDLE" ]; then
    echo -e "${RED}❌ App bundle not found at expected location${NC}"
    echo ""
    echo "Expected: src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/ios-arm64/*.app"
    echo ""
    echo "Available builds:"
    ls -la src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/ 2>/dev/null || echo "None"
    exit 1
fi

echo "📦 App bundle: $APP_BUNDLE"
APP_SIZE=$(du -sh "$APP_BUNDLE" | cut -f1)
echo "   Size: $APP_SIZE"
echo ""

# Install on device
echo "📲 Installing on device (ID: $DEVICE_ID)..."
echo ""

# Try using devicectl (iOS 17+)
if command -v xcrun &> /dev/null && xcrun devicectl --help &> /dev/null 2>&1; then
    xcrun devicectl device install app --device "$DEVICE_ID" "$APP_BUNDLE"
    INSTALL_EXIT_CODE=$?
else
    # Fallback to ios-deploy
    if command -v ios-deploy &> /dev/null; then
        ios-deploy --id "$DEVICE_ID" --bundle "$APP_BUNDLE"
        INSTALL_EXIT_CODE=$?
    else
        echo -e "${YELLOW}⚠️  Neither devicectl nor ios-deploy available${NC}"
        echo ""
        echo "Install with Xcode instead:"
        echo "  1. Open Xcode → Window → Devices and Simulators"
        echo "  2. Select your device"
        echo "  3. Click '+' under Installed Apps"
        echo "  4. Select: $APP_BUNDLE"
        echo ""
        echo "OR install ios-deploy:"
        echo "  brew install ios-deploy"
        echo ""
        exit 1
    fi
fi

if [ $INSTALL_EXIT_CODE -eq 0 ]; then
    echo ""
    echo -e "${GREEN}✅ App installed successfully!${NC}"
    echo ""
    echo "📱 First time running?"
    echo "   On your iPhone:"
    echo "   Settings → General → VPN & Device Management"
    echo "   → Tap your developer profile → Trust"
    echo ""
    echo "🎉 You can now launch the app from your home screen!"
    echo ""
else
    echo ""
    echo -e "${RED}❌ Installation failed${NC}"
    echo ""
    echo "Try manual installation:"
    echo "  1. Open Xcode → Window → Devices and Simulators"
    echo "  2. Select your device"
    echo "  3. Drag and drop: $APP_BUNDLE"
fi
