#!/bin/bash
# Deploy to Physical iOS Device - Works with Paid Apple Developer Account
# Vietnamese Lunar Calendar

set -e

echo "📱 Deploy to Physical iOS Device (Paid Developer Account)"
echo "=========================================================="
echo ""

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
PROJECT_PATH="$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"
PROVISIONING_PROFILE="$WORKSPACE_ROOT/releases/comhuynguyenlunarcalendar.mobileprovision"
PROVISIONING_PROFILE_NAME="com.huynguyen.lunarcalendar"

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

# Check for Apple Development certificate that matches the provisioning profile
echo "🔐 Checking for Apple Development certificate..."
# Look for the specific certificate that matches our provisioning profile
CERT=$(security find-identity -v -p codesigning | grep "Apple Development: Huy Nguyen (PCXAANVGF7)" || echo "")

if [ -z "$CERT" ]; then
    echo -e "${YELLOW}⚠️  Specific certificate not found, trying any Apple Development certificate...${NC}"
    CERT=$(security find-identity -v -p codesigning | grep "Apple Development" | head -n 1 || echo "")

    if [ -z "$CERT" ]; then
        echo -e "${RED}❌ No Apple Development certificate found${NC}"
        echo ""
        echo "Please set up your certificate first:"
        echo "  1. Open Xcode → Settings → Accounts"
        echo "  2. Select your Apple ID → Click 'Manage Certificates'"
        echo "  3. Click '+' and add 'Apple Development' certificate"
        echo ""
        exit 1
    fi
fi

CERT_NAME=$(echo "$CERT" | sed 's/^[^"]*"//' | sed 's/".*//')
echo -e "${GREEN}✓ Found certificate:${NC} $CERT_NAME"
echo ""

# Check for and install provisioning profile
echo "📋 Checking for provisioning profile..."
PROFILES_DIR="$HOME/Library/MobileDevice/Provisioning Profiles"

if [ ! -f "$PROVISIONING_PROFILE" ]; then
    echo -e "${RED}❌ Provisioning profile not found${NC}"
    echo "Expected location: $PROVISIONING_PROFILE"
    exit 1
fi

echo -e "${GREEN}✓ Found provisioning profile:${NC} $(basename "$PROVISIONING_PROFILE")"

# Create profiles directory if it doesn't exist
mkdir -p "$PROFILES_DIR"

# Install the provisioning profile
echo "📥 Installing provisioning profile..."
PROFILE_UUID=$(security cms -D -i "$PROVISIONING_PROFILE" 2>/dev/null | grep -A 1 "<key>UUID</key>" | tail -1 | sed 's/.*<string>\(.*\)<\/string>.*/\1/')

if [ -z "$PROFILE_UUID" ]; then
    echo -e "${RED}❌ Failed to extract UUID from provisioning profile${NC}"
    exit 1
fi

cp "$PROVISIONING_PROFILE" "$PROFILES_DIR/$PROFILE_UUID.mobileprovision"
echo -e "${GREEN}✓ Provisioning profile installed${NC}"
echo "   UUID: $PROFILE_UUID"
echo ""

echo "🔨 Building for iOS device..."
echo "   Project: $PROJECT_PATH"
echo "   Target: Physical Device (ios-arm64)"
echo "   Configuration: Debug (Development profile — avoids App Store signing override)"
echo "   Framework: net10.0-ios"
echo "   Certificate: $CERT_NAME"
echo "   Provisioning Profile: $PROVISIONING_PROFILE_NAME"
echo ""

# Build with Debug config for device testing.
# IMPORTANT: Must use Debug (not Release) because the csproj Release PropertyGroup
# hard-codes CodesignKey="Apple Distribution" and CodesignProvision="Lunar Calendar App Store"
# for App Store submissions — which overrides whatever the script passes and causes signing
# failures. The Development provisioning profile (get-task-allow=true) requires Debug config.
dotnet build "$PROJECT_PATH" \
    -f net10.0-ios \
    -c Debug \
    -p:RuntimeIdentifier=ios-arm64 \
    -p:CodesignKey="$CERT_NAME" \
    -p:CodesignProvision="$PROVISIONING_PROFILE_NAME" \
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
APP_BUNDLE=$(find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/ios-arm64" -name "*.app" -type d 2>/dev/null | head -n 1)

if [ -z "$APP_BUNDLE" ]; then
    echo -e "${RED}❌ App bundle not found at expected location${NC}"
    echo ""
    echo "Expected: $WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/ios-arm64/*.app"
    echo ""
    echo "Available builds:"
    ls -la "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/" 2>/dev/null || echo "None"
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
