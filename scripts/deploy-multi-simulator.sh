#!/bin/bash

# Multi-Platform Deployment Script with Logging Verification
# Deploys to multiple iOS versions and Android emulators

echo "🚀 Vietnamese Lunar Calendar - Multi-Platform Deployment"
echo "📋 Testing Logging Implementation"
echo ""

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
cd "$WORKSPACE_ROOT"

# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# ============================================
# iOS Deployment
# ============================================

echo -e "${BLUE}📱 iOS Deployment${NC}"
echo ""

# iOS Simulators to test (targeting iOS 15+)
IOS_DEVICES=(
    "928962C4-CCDB-40DF-92B8-8794CB867716:iPhone 16 Pro (iOS 18)"
    "4BEC1E56-9B92-4B3F-8065-04DDA5821951:iPhone 15 Pro (iOS 18.2)"
)

for device in "${IOS_DEVICES[@]}"; do
    IFS=":" read -r udid name <<< "$device"
    
    echo -e "${YELLOW}Deploying to: $name${NC}"
    echo "UDID: $udid"
    
    # Boot simulator if not running
    echo "  ⚙️  Booting simulator..."
    xcrun simctl boot "$udid" 2>/dev/null || echo "  Already booted"
    
    # Build and deploy
    echo "  🔨 Building..."
    dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
        -t:Run \
        -f net10.0-ios \
        -p:_DeviceName=":v2:udid=$udid" \
        -p:Configuration=Debug \
        > /tmp/ios-build-$udid.log 2>&1
    
    if [ $? -eq 0 ]; then
        echo -e "  ${GREEN}✅ Deployed successfully to $name${NC}"
        echo ""
    else
        echo -e "  ${RED}❌ Deployment failed${NC}"
        echo "  Check log: /tmp/ios-build-$udid.log"
        echo ""
    fi
    
    # Wait a bit before next deployment
    sleep 2
done

# ============================================
# Android Deployment
# ============================================

echo -e "${BLUE}🤖 Android Deployment${NC}"
echo ""

# Check if Android SDK is available
if [ -z "$ANDROID_HOME" ]; then
    echo -e "${YELLOW}⚠️  ANDROID_HOME not set. Skipping Android deployment.${NC}"
    echo "To deploy to Android, set up Android SDK and emulators."
    echo ""
else
    # Check for running emulators
    echo "Checking for running Android emulators..."
    adb devices | grep -v "List" | grep "emulator"
    
    if [ $? -eq 0 ]; then
        echo "  🔨 Building for Android..."
        dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
            -t:Run \
            -f net10.0-android \
            -p:Configuration=Debug \
            > /tmp/android-build.log 2>&1
        
        if [ $? -eq 0 ]; then
            echo -e "  ${GREEN}✅ Deployed to Android emulator${NC}"
        else
            echo -e "  ${RED}❌ Android deployment failed${NC}"
            echo "  Check log: /tmp/android-build.log"
        fi
    else
        echo -e "${YELLOW}⚠️  No running Android emulators found${NC}"
        echo "To test on Android:"
        echo "  1. Create Android emulator in Android Studio"
        echo "  2. Start emulator: emulator -avd <avd-name>"
        echo "  3. Run this script again"
    fi
fi

echo ""
echo -e "${GREEN}🎉 Deployment Complete!${NC}"
echo ""
echo "📊 Next Steps:"
echo "  1. Test app on each simulator"
echo "  2. Verify logging by navigating: Settings → View Logs"
echo "  3. Trigger errors to test error logging"
echo "  4. Check log file location:"
echo "     iOS: FileSystem.AppDataDirectory/Logs/"
echo "     Android: FileSystem.AppDataDirectory/Logs/"
echo ""
echo "📝 To view build logs:"
echo "  iOS: cat /tmp/ios-build-*.log"
echo "  Android: cat /tmp/android-build.log"
