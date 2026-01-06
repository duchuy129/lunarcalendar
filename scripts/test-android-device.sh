#!/bin/bash
# Test on Physical Android Device
# Vietnamese Lunar Calendar v1.0.1

set -e  # Exit on error

echo "📱 Deploy to Physical Android Device"
echo "====================================="
echo ""

# Configuration
PROJECT_PATH="src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"
CONFIG="Debug"
FRAMEWORK="net10.0-android"
PACKAGE_NAME="com.huynguyen.lunarcalendar"
ACTIVITY_NAME="crc64b457a836cc4fb5b9.MainActivity"
ADB="$HOME/Library/Android/sdk/platform-tools/adb"

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Check if adb exists
if [ ! -f "$ADB" ]; then
    echo -e "${RED}❌ Error: adb not found at $ADB${NC}"
    echo ""
    echo "Please ensure Android SDK is installed"
    exit 1
fi

echo "🔍 Checking for connected Android devices..."
CONNECTED_DEVICES=$("$ADB" devices | grep -v "List" | grep "device$" || echo "")

if [ -z "$CONNECTED_DEVICES" ]; then
    echo -e "${RED}❌ No Android devices connected${NC}"
    echo ""
    echo "Please connect an Android device via USB and try again."
    echo "Make sure to:"
    echo "  1. Enable Developer Options (tap Build Number 7 times)"
    echo "  2. Enable USB Debugging in Developer Options"
    echo "  3. Authorize this computer on your device"
    echo ""
    echo "Check connection with: $ADB devices"
    exit 1
fi

echo -e "${GREEN}✓ Found connected Android device(s):${NC}"
"$ADB" devices
echo ""

# Check for multiple devices and select physical device
DEVICE_COUNT=$(echo "$CONNECTED_DEVICES" | wc -l | tr -d ' ')
if [ "$DEVICE_COUNT" -gt 1 ]; then
    echo -e "${YELLOW}Multiple devices detected. Looking for physical device...${NC}"
    PHYSICAL_DEVICE=$(echo "$CONNECTED_DEVICES" | grep -v "emulator" | head -n 1 | awk '{print $1}')
    if [ -n "$PHYSICAL_DEVICE" ]; then
        DEVICE_SERIAL="$PHYSICAL_DEVICE"
        echo -e "${GREEN}✓ Selected physical device: $DEVICE_SERIAL${NC}"
        echo ""
        ADB="$ADB -s $DEVICE_SERIAL"
    else
        echo -e "${YELLOW}⚠️  No physical device found, using first available device${NC}"
        DEVICE_SERIAL=$(echo "$CONNECTED_DEVICES" | head -n 1 | awk '{print $1}')
        ADB="$ADB -s $DEVICE_SERIAL"
    fi
else
    DEVICE_SERIAL=$(echo "$CONNECTED_DEVICES" | head -n 1 | awk '{print $1}')
    ADB="$ADB -s $DEVICE_SERIAL"
fi

# Get device info
DEVICE_MODEL=$($ADB shell getprop ro.product.model 2>/dev/null | tr -d '\r\n' | xargs)
ANDROID_VERSION=$($ADB shell getprop ro.build.version.release 2>/dev/null | tr -d '\r\n' | xargs)
API_LEVEL=$($ADB shell getprop ro.build.version.sdk 2>/dev/null | tr -d '\r\n' | xargs)

echo "📱 Device Information:"
echo "   Model: $DEVICE_MODEL"
echo "   Android: $ANDROID_VERSION (API $API_LEVEL)"
echo ""

# Check API level
if [ -z "$API_LEVEL" ]; then
    echo -e "${YELLOW}⚠️  Warning: Could not determine API level${NC}"
    echo "   Proceeding anyway..."
elif [ "$API_LEVEL" -lt 26 ]; then
    echo -e "${RED}❌ Error: Device API level ($API_LEVEL) is below minimum (26)${NC}"
    echo "   This app requires Android 8.0+ (API 26)"
    exit 1
fi

# Clean previous builds
echo "🧹 Cleaning previous builds..."
dotnet clean "$PROJECT_PATH" -c $CONFIG -f $FRAMEWORK > /dev/null 2>&1

echo ""
echo "🔨 Building for Android device..."
echo "   Framework: $FRAMEWORK"
echo "   Configuration: $CONFIG (Debug for testing)"
echo ""

# Build APK
dotnet build "$PROJECT_PATH" \
    -f $FRAMEWORK \
    -c $CONFIG

BUILD_EXIT_CODE=$?

echo ""
if [ $BUILD_EXIT_CODE -eq 0 ]; then
    echo -e "${GREEN}✅ Build successful!${NC}"
    echo ""
    
    # Find the APK
    APK_PATH=$(find src/LunarCalendar.MobileApp/bin/Debug/net10.0-android -name "*-Signed.apk" -type f | head -n 1)
    
    if [ -n "$APK_PATH" ]; then
        APK_SIZE=$(du -h "$APK_PATH" | cut -f1)
        echo "📦 APK: $APK_PATH"
        echo "   Size: $APK_SIZE"
        echo ""
        
        # Uninstall old version if exists
        echo "🗑️  Uninstalling old version (if exists)..."
        "$ADB" uninstall "$PACKAGE_NAME" 2>/dev/null || echo "   No previous installation found"
        
        # Install APK
        echo ""
        echo "📲 Installing APK on device..."
        "$ADB" install -r "$APK_PATH"
        
        INSTALL_EXIT_CODE=$?
        
        if [ $INSTALL_EXIT_CODE -eq 0 ]; then
            echo ""
            echo -e "${GREEN}✅ App installed successfully!${NC}"
            echo ""
            
            # Launch app
            echo "🚀 Launching app..."
            "$ADB" shell am start -n "$PACKAGE_NAME/$ACTIVITY_NAME"
            
            echo ""
            echo -e "${GREEN}✅ App launched on device!${NC}"
            echo ""
            echo "📊 Monitor logs with:"
            echo "   $ADB logcat | grep -i 'lunarcalendar\\|mono\\|crash'"
            echo ""
            echo "📋 Now follow the Physical Device Testing checklist:"
            echo "   See: PHYSICAL_DEVICE_TESTING_GUIDE.md"
            echo ""
            
            # Optional: Start monitoring logs
            read -p "Start monitoring logs now? (y/n) " -n 1 -r
            echo
            if [[ $REPLY =~ ^[Yy]$ ]]; then
                echo ""
                echo "📊 Monitoring logs (Ctrl+C to stop)..."
                echo "=========================================="
                "$ADB" logcat | grep -i --color=always "lunarcalendar\|mono\|AndroidRuntime\|FATAL"
            fi
        else
            echo -e "${RED}❌ Installation failed!${NC}"
            echo ""
            echo "Common issues:"
            echo "   - Insufficient storage on device"
            echo "   - USB debugging not authorized"
            echo "   - Device security settings blocking installation"
            exit 1
        fi
    else
        echo -e "${YELLOW}⚠️  APK file not found${NC}"
        echo "   Check build output above for errors"
        exit 1
    fi
else
    echo -e "${RED}❌ Build failed!${NC}"
    echo ""
    echo "Common issues:"
    echo "   - Build configuration errors"
    echo "   - Missing dependencies"
    echo ""
    echo "See PHYSICAL_DEVICE_TESTING_GUIDE.md for troubleshooting"
    exit 1
fi
