#!/bin/bash
# Deploy to Physical Android Device
# Vietnamese Lunar Calendar

set -e

echo "📱 Deploy to Physical Android Device"
echo "====================================="
echo ""

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
PROJECT_PATH="$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"
PACKAGE_NAME="com.huynguyen.lunarcalendar"
ACTIVITY_NAME="crc64b457a836cc4fb5b9.MainActivity"

# Colors
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
BLUE='\033[0;34m'
NC='\033[0m'

# ADB path
ADB="$HOME/Library/Android/sdk/platform-tools/adb"

# Check if adb exists
if [ ! -f "$ADB" ]; then
    echo -e "${RED}❌ Error: adb not found at $ADB${NC}"
    echo ""
    echo "Please ensure Android SDK is installed"
    echo "Install via Android Studio or homebrew: brew install android-platform-tools"
    exit 1
fi

# Check for connected devices
echo "🔍 Checking for connected Android devices..."
CONNECTED_DEVICES=$("$ADB" devices | grep -v "List" | grep "device$" || echo "")

if [ -z "$CONNECTED_DEVICES" ]; then
    echo -e "${RED}❌ No Android devices connected${NC}"
    echo ""
    echo "Please connect an Android device via USB and try again."
    echo "Make sure to:"
    echo "  1. Enable Developer Options on your device"
    echo "  2. Enable USB Debugging"
    echo "  3. Trust this computer on your device"
    echo ""
    exit 1
fi

echo -e "${GREEN}✓ Found connected device(s):${NC}"
"$ADB" devices | grep "device$"
DEVICE_ID=$("$ADB" devices | grep "device$" | head -1 | awk '{print $1}')
echo ""

# Get device info
DEVICE_MODEL=$("$ADB" -s "$DEVICE_ID" shell getprop ro.product.model 2>/dev/null | tr -d '\r')
ANDROID_VERSION=$("$ADB" -s "$DEVICE_ID" shell getprop ro.build.version.release 2>/dev/null | tr -d '\r')

echo "📱 Target Device:"
echo "   Model: $DEVICE_MODEL"
echo "   Android Version: $ANDROID_VERSION"
echo "   Device ID: $DEVICE_ID"
echo ""

# Build for Android device
echo "🔨 Building for Android device..."
echo "   Project: $PROJECT_PATH"
echo "   Configuration: Release"
echo "   Framework: net10.0-android"
echo ""

dotnet build "$PROJECT_PATH" \
    -f net10.0-android \
    -c Release

BUILD_EXIT_CODE=$?

if [ $BUILD_EXIT_CODE -ne 0 ]; then
    echo ""
    echo -e "${RED}❌ Build failed!${NC}"
    echo ""
    echo "Common issues:"
    echo "  1. Check that Android SDK is properly installed"
    echo "  2. Verify project compiles without errors"
    echo ""
    exit 1
fi

echo ""
echo -e "${GREEN}✅ Build successful!${NC}"
echo ""

# Find APK
echo "📦 Finding APK..."
APK_FILE=$(find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Release/net10.0-android" -name "*-Signed.apk" 2>/dev/null | head -1)

if [ -z "$APK_FILE" ]; then
    echo "Looking for unsigned APK..."
    APK_FILE=$(find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Release/net10.0-android" -name "*.apk" 2>/dev/null | head -1)
fi

if [ -z "$APK_FILE" ]; then
    echo -e "${RED}❌ APK not found at expected location${NC}"
    echo ""
    echo "Expected: $WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Release/net10.0-android/*.apk"
    echo ""
    echo "Available builds:"
    ls -la "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Release/net10.0-android/" 2>/dev/null || echo "None"
    exit 1
fi

APK_SIZE=$(du -sh "$APK_FILE" | cut -f1)
echo "   APK: $(basename "$APK_FILE")"
echo "   Size: $APK_SIZE"
echo ""

# Install on device
echo "📲 Installing on device (ID: $DEVICE_ID)..."
echo ""

"$ADB" -s "$DEVICE_ID" install -r "$APK_FILE" 2>&1 | tee /tmp/install_output.txt
INSTALL_EXIT_CODE=$?

# Check if installation failed due to version downgrade
if [ $INSTALL_EXIT_CODE -ne 0 ] && grep -q "INSTALL_FAILED_VERSION_DOWNGRADE" /tmp/install_output.txt; then
    echo ""
    echo -e "${YELLOW}⚠️  Version downgrade detected. Uninstalling existing app...${NC}"
    "$ADB" -s "$DEVICE_ID" uninstall "$PACKAGE_NAME"

    echo "Retrying installation..."
    "$ADB" -s "$DEVICE_ID" install -r "$APK_FILE"
    INSTALL_EXIT_CODE=$?
fi

if [ $INSTALL_EXIT_CODE -eq 0 ]; then
    echo ""
    echo -e "${GREEN}✅ App installed successfully!${NC}"
    echo ""

    # Launch the app
    echo "🚀 Launching app..."
    "$ADB" -s "$DEVICE_ID" shell am start -n "$PACKAGE_NAME/$ACTIVITY_NAME"

    if [ $? -eq 0 ]; then
        echo ""
        echo -e "${GREEN}🎉 App launched successfully on your device!${NC}"
        echo ""
        echo "📱 The app should now be running on your $DEVICE_MODEL"
        echo ""
        echo "💡 Tips:"
        echo "   • Check logcat for app logs:"
        echo "     $ADB logcat | grep LunarCalendar"
        echo ""
        echo "   • Stop the app:"
        echo "     $ADB shell am force-stop $PACKAGE_NAME"
        echo ""
        echo "   • Uninstall:"
        echo "     $ADB uninstall $PACKAGE_NAME"
    else
        echo ""
        echo -e "${YELLOW}⚠️  App installed but launch failed${NC}"
        echo "Please manually launch 'Lunar Calendar' from your device"
    fi
else
    echo ""
    echo -e "${RED}❌ Installation failed${NC}"
    echo ""
    echo "Possible issues:"
    echo "  1. Not enough storage on device"
    echo "  2. Previous installation conflicts - try uninstalling first:"
    echo "     $ADB uninstall $PACKAGE_NAME"
    echo "  3. Device not authorized - check device screen for authorization prompt"
    exit 1
fi
