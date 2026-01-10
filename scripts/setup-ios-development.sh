#!/bin/bash
# Setup iOS Development for Physical Device (Paid Apple Developer Account)
# This script helps you set up certificates and provisioning profiles

set -e

echo "🍎 iOS Development Setup for Physical Devices"
echo "=============================================="
echo ""

# Colors
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
BLUE='\033[0;34m'
NC='\033[0m'

echo -e "${BLUE}This script will guide you through setting up iOS development${NC}"
echo -e "${BLUE}for deploying to physical devices with your paid Apple Developer account.${NC}"
echo ""

# Step 1: Check connected device
echo "📱 Step 1: Checking for connected iOS device..."
CONNECTED_DEVICES=$(xcrun xctrace list devices 2>/dev/null | grep -E "iPhone|iPad" | grep -v "Simulator" || echo "")

if [ -z "$CONNECTED_DEVICES" ]; then
    echo -e "${RED}❌ No iOS devices connected${NC}"
    echo "Please connect your iPhone or iPad via USB first."
    exit 1
fi

echo -e "${GREEN}✓ Found device(s):${NC}"
echo "$CONNECTED_DEVICES"
DEVICE_ID=$(echo "$CONNECTED_DEVICES" | head -n 1 | sed -E 's/.*\(([A-F0-9-]+)\).*/\1/')
echo ""

# Step 2: Check Apple ID in Xcode
echo "📝 Step 2: Checking Apple Developer Account..."
echo ""
echo "Opening Xcode Settings to verify your Apple Developer account..."
open -a Xcode
sleep 2
osascript <<EOF
tell application "Xcode"
    activate
end tell
tell application "System Events"
    tell process "Xcode"
        keystroke "," using command down
    end tell
end tell
EOF

echo ""
echo -e "${YELLOW}In Xcode Settings:${NC}"
echo "  1. Go to 'Accounts' tab"
echo "  2. Make sure your Apple ID is added (with paid developer account)"
echo "  3. Select your Apple ID → Click 'Manage Certificates'"
echo "  4. If you don't have 'Apple Development' certificate, click '+' and add it"
echo ""
read -p "Press Enter after you've verified/added your Apple Development certificate..."

# Step 3: Check certificates
echo ""
echo "🔐 Step 3: Verifying installed certificates..."
CERTS=$(security find-identity -v -p codesigning)

if echo "$CERTS" | grep -q "Apple Development"; then
    echo -e "${GREEN}✓ Apple Development certificate found${NC}"
    CERT_NAME=$(echo "$CERTS" | grep "Apple Development" | head -n 1 | sed 's/^.*") //' | sed 's/ ".*$//')
    echo "  Certificate: $CERT_NAME"
else
    echo -e "${RED}❌ No Apple Development certificate found${NC}"
    echo ""
    echo "Please create one in Xcode:"
    echo "  1. Xcode → Settings → Accounts"
    echo "  2. Select your Apple ID"
    echo "  3. Click 'Manage Certificates'"
    echo "  4. Click '+' → Select 'Apple Development'"
    echo ""
    exit 1
fi

# Step 4: Create/Download Provisioning Profile
echo ""
echo "📋 Step 4: Setting up Provisioning Profile..."
echo ""
echo "You need to create a provisioning profile for:"
echo "  App ID: com.huynguyen.lunarcalendar"
echo "  Device UDID: $DEVICE_ID"
echo ""
echo -e "${YELLOW}Option A: Automatic (Recommended)${NC}"
echo "  Let Xcode manage provisioning automatically"
echo ""
echo -e "${YELLOW}Option B: Manual${NC}"
echo "  Create manually at: https://developer.apple.com/account/resources/profiles"
echo ""
read -p "Use automatic provisioning? (y/n) " -n 1 -r
echo ""

if [[ $REPLY =~ ^[Yy]$ ]]; then
    echo -e "${GREEN}✓ Will use automatic provisioning${NC}"
    AUTO_PROVISION=true
else
    echo ""
    echo "To create manual provisioning profile:"
    echo "  1. Go to: https://developer.apple.com/account/resources/profiles"
    echo "  2. Click '+' to create new profile"
    echo "  3. Select 'iOS App Development'"
    echo "  4. Choose App ID: com.huynguyen.lunarcalendar (or create it)"
    echo "  5. Select your certificate"
    echo "  6. Select device with UDID: $DEVICE_ID"
    echo "  7. Download and double-click to install"
    echo ""
    read -p "Press Enter after you've installed the provisioning profile..."
    AUTO_PROVISION=false
fi

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"

# Step 5: Test build
echo ""
echo "🔨 Step 5: Testing build for device..."
cd "$WORKSPACE_ROOT"

if [ "$AUTO_PROVISION" = true ]; then
    echo "Building with automatic provisioning..."
    dotnet build "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj" \
        -f net10.0-ios \
        -c Debug \
        -p:RuntimeIdentifier=ios-arm64 \
        -p:CodesignProvision=""
else
    echo "Building with manual provisioning..."
    echo "Enter your provisioning profile name (or press Enter to skip):"
    read PROVISION_NAME
    
    if [ -z "$PROVISION_NAME" ]; then
        dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
            -f net8.0-ios \
            -c Debug \
            -p:RuntimeIdentifier=ios-arm64
    else
        dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
            -f net8.0-ios \
            -c Debug \
            -p:RuntimeIdentifier=ios-arm64 \
            -p:CodesignProvision="$PROVISION_NAME"
    fi
fi

if [ $? -eq 0 ]; then
    echo ""
    echo -e "${GREEN}✅ Build successful!${NC}"
    echo ""
    echo "You're now set up for iOS device development!"
    echo ""
    echo "To deploy to your device, run:"
    echo "  ./test-ios-device.sh"
    echo ""
else
    echo ""
    echo -e "${RED}❌ Build failed${NC}"
    echo ""
    echo "Common issues:"
    echo "  1. Device not registered: Add UDID $DEVICE_ID to developer portal"
    echo "  2. App ID not created: Create com.huynguyen.lunarcalendar at developer portal"
    echo "  3. Provisioning profile missing/expired"
    echo ""
    echo "Visit: https://developer.apple.com/account/resources"
fi
