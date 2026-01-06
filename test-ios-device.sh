#!/bin/bash
# Test on Physical iOS Device
# Vietnamese Lunar Calendar v1.0.1

set -e  # Exit on error

echo "📱 Deploy to Physical iOS Device"
echo "=================================="
echo ""

# Configuration
PROJECT_PATH="src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"
CONFIG="Debug"
FRAMEWORK="net8.0-ios"
RUNTIME_ID="ios-arm64"

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo "🔍 Checking for connected iOS devices..."
CONNECTED_DEVICES=$(xcrun xctrace list devices 2>/dev/null | grep -E "iPhone|iPad" | grep -v "Simulator" || echo "")

if [ -z "$CONNECTED_DEVICES" ]; then
    echo -e "${RED}❌ No iOS devices connected${NC}"
    echo ""
    echo "Please connect an iPhone or iPad via USB and try again."
    echo "Make sure to:"
    echo "  1. Trust this computer on your device"
    echo "  2. Enable Developer Mode (iOS 16+)"
    echo ""
    exit 1
fi

echo -e "${GREEN}✓ Found connected iOS device(s):${NC}"
echo "$CONNECTED_DEVICES"
echo ""

# Clean previous builds
echo "🧹 Cleaning previous builds..."
dotnet clean "$PROJECT_PATH" -c $CONFIG -f $FRAMEWORK > /dev/null 2>&1

echo ""
echo "🔨 Building for iOS device..."
echo "   Framework: $FRAMEWORK"
echo "   Configuration: $CONFIG (Debug for testing)"
echo "   Runtime: $RUNTIME_ID"
echo ""

echo "Note: This will create a development provisioning profile automatically"
echo "      if you don't have one already. Make sure you're signed into"
echo "      Xcode with your Apple ID (Xcode → Settings → Accounts)"
echo ""

# Build for device with explicit signing
# Use SdkOnly linking to prevent crashes on physical devices
# Include linker config to preserve SQLite assemblies
dotnet build "$PROJECT_PATH" \
    -f $FRAMEWORK \
    -c $CONFIG \
    -p:RuntimeIdentifier=$RUNTIME_ID \
    -p:MtouchLink=SdkOnly \
    -p:MtouchInterpreter="" \
    -p:UseInterpreter=false \
    -p:MtouchExtraArgs="--xml=Platforms/iOS/LinkerConfig.xml" \
    -p:CodesignKey="Apple Development: Huy Nguyen (PCXAANVGF7)" \
    -p:CodesignProvision="4619e0b0-9d0d-49cf-bbd9-150f949b107b" \
    -p:CodesignEntitlements="Platforms/iOS/Entitlements.plist"

BUILD_EXIT_CODE=$?

echo ""
if [ $BUILD_EXIT_CODE -eq 0 ]; then
    echo -e "${GREEN}✅ Build successful!${NC}"
    echo ""
    
    # Find the app bundle
    APP_BUNDLE=$(find src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/ios-arm64 -name "*.app" -type d | head -n 1)
    
    if [ -n "$APP_BUNDLE" ]; then
        echo "📦 App bundle: $APP_BUNDLE"
        echo ""
        
        # Try to install on device
        echo "📲 Installing on device..."
        
        # Get first connected device ID
        DEVICE_ID=$(xcrun xctrace list devices 2>/dev/null | grep -E "iPhone|iPad" | grep -v "Simulator" | head -n 1 | sed -E 's/.*\(([A-F0-9-]+)\).*/\1/')
        
        if [ -n "$DEVICE_ID" ]; then
            echo "   Device ID: $DEVICE_ID"
            
            # Install using devicectl (iOS 17+) or older methods
            if xcrun devicectl --help &> /dev/null; then
                xcrun devicectl device install app --device "$DEVICE_ID" "$APP_BUNDLE"
            else
                # Fallback for older Xcode
                echo "   Using ios-deploy (install with: brew install ios-deploy)"
                if command -v ios-deploy &> /dev/null; then
                    ios-deploy --bundle "$APP_BUNDLE"
                else
                    echo -e "${YELLOW}⚠️  Please install app manually from Xcode${NC}"
                    echo "   1. Open Xcode → Window → Devices and Simulators"
                    echo "   2. Select your device"
                    echo "   3. Click '+' under Installed Apps"
                    echo "   4. Select: $APP_BUNDLE"
                fi
            fi
            
            echo ""
            echo -e "${GREEN}✅ App deployed to device!${NC}"
            echo ""
            echo "⚠️  First time running?"
            echo "   If you see 'Untrusted Developer':"
            echo "   Settings → General → VPN & Device Management"
            echo "   → Trust your Apple ID"
            echo ""
            echo "📋 Now follow the Physical Device Testing checklist:"
            echo "   See: PHYSICAL_DEVICE_TESTING_GUIDE.md"
            echo ""
        else
            echo -e "${YELLOW}⚠️  Could not determine device ID${NC}"
            echo "   App bundle is ready at: $APP_BUNDLE"
            echo "   Install manually using Xcode"
        fi
    else
        echo -e "${YELLOW}⚠️  App bundle not found${NC}"
        echo "   Check build output above for errors"
    fi
else
    echo -e "${RED}❌ Build failed!${NC}"
    echo ""
    echo "Common issues:"
    echo "   - Device not properly connected"
    echo "   - Developer mode not enabled (iOS 16+)"
    echo "   - Provisioning profile issues"
    echo ""
    echo "See PHYSICAL_DEVICE_TESTING_GUIDE.md for troubleshooting"
    exit 1
fi
