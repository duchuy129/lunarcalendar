#!/bin/bash
# Test on Physical iOS Device using Xcode
# This method works better for free Apple Developer accounts

set -e

echo "📱 Deploy to Physical iOS Device (Using Xcode)"
echo "================================================"
echo ""

# Colors
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m'

# Check for connected devices
echo "🔍 Checking for connected iOS devices..."
CONNECTED_DEVICES=$(xcrun xctrace list devices 2>/dev/null | grep -E "iPhone|iPad" | grep -v "Simulator" || echo "")

if [ -z "$CONNECTED_DEVICES" ]; then
    echo -e "${RED}❌ No iOS devices connected${NC}"
    echo ""
    echo "Please connect an iPhone or iPad via USB and try again."
    exit 1
fi

echo -e "${GREEN}✓ Found connected iOS device(s):${NC}"
echo "$CONNECTED_DEVICES"
echo ""

# Get device info
DEVICE_NAME=$(echo "$CONNECTED_DEVICES" | head -n 1 | sed 's/ ([^)]*).*$//')
DEVICE_ID=$(echo "$CONNECTED_DEVICES" | head -n 1 | sed -E 's/.*\(([A-F0-9-]+)\).*/\1/')

echo "📱 Target Device:"
echo "   Name: $DEVICE_NAME"
echo "   ID: $DEVICE_ID"
echo ""

# Check if signed into Xcode
echo "🔐 Checking Apple ID in Xcode..."
echo ""
echo -e "${YELLOW}⚠️  IMPORTANT: You must be signed into Xcode with your Apple ID${NC}"
echo ""
echo "To sign in:"
echo "  1. Open Xcode"
echo "  2. Go to: Xcode → Settings (or Preferences)"
echo "  3. Click 'Accounts' tab"
echo "  4. Click '+' and add your Apple ID"
echo "  5. Free accounts work fine for testing!"
echo ""
read -p "Are you signed into Xcode with your Apple ID? (y/n) " -n 1 -r
echo ""
if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    echo "Please sign into Xcode first, then run this script again."
    exit 1
fi

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"

# Build using dotnet first
echo "🔨 Building app bundle..."
cd "$WORKSPACE_ROOT"

dotnet build "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj" \
    -f net10.0-ios \
    -c Debug \
    -p:RuntimeIdentifier=iossimulator-arm64

if [ $? -ne 0 ]; then
    echo -e "${RED}❌ Build failed${NC}"
    exit 1
fi

echo -e "${GREEN}✅ Build successful${NC}"
echo ""

# Find the app bundle
APP_BUNDLE=$(find src/LunarCalendar.MobileApp/bin/Debug/net8.0-ios/iossimulator-arm64 -name "*.app" -type d | head -n 1)

if [ -z "$APP_BUNDLE" ]; then
    echo -e "${RED}❌ App bundle not found${NC}"
    exit 1
fi

echo "📦 App bundle: $APP_BUNDLE"
echo ""

# Now we'll use Xcode to deploy
echo "📲 Deploying to device using Xcode..."
echo ""
echo -e "${YELLOW}Follow these steps:${NC}"
echo ""
echo "1. Open Xcode"
echo "2. Go to: Window → Devices and Simulators"
echo "3. Select your device: $DEVICE_NAME"
echo "4. Under 'Installed Apps', click the '+' button"
echo "5. Navigate to and select:"
echo "   $APP_BUNDLE"
echo ""
echo "Alternative: Build and deploy directly from Xcode"
echo "1. Open: src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj in Xcode"
echo "2. Select your connected device as target"
echo "3. Click Run (▶) button"
echo "   Xcode will automatically create a free provisioning profile"
echo ""

# Try to open Xcode devices window
echo "Opening Xcode Devices window..."
open -a Xcode

# Wait a moment then try to open devices window
sleep 2
osascript -e 'tell application "Xcode" to activate' 2>/dev/null || true
osascript -e 'tell application "System Events" to keystroke "2" using {command down, shift down}' 2>/dev/null || true

echo ""
echo -e "${GREEN}Next steps:${NC}"
echo "1. Use Xcode's Devices window to install the app"
echo "2. Or build directly from Xcode for automatic provisioning"
echo "3. First time: Settings → General → VPN & Device Management → Trust Developer"
echo ""
echo "See: PHYSICAL_DEVICE_TESTING_GUIDE.md for complete instructions"
