#!/bin/bash
# Quick iOS Device Deploy Setup
# For paid Apple Developer accounts

echo "🍎 Quick iOS Device Setup"
echo "========================="
echo ""

# Get device UDID
DEVICE_ID=$(xcrun xctrace list devices 2>/dev/null | grep -E "iPhone|iPad" | grep -v "Simulator" | head -n 1 | sed -E 's/.*\(([A-F0-9-]+)\).*/\1/')

if [ -z "$DEVICE_ID" ]; then
    echo "❌ No device connected. Please connect your iPhone via USB."
    exit 1
fi

echo "✓ Device connected: $DEVICE_ID"
echo ""
echo "📝 Quick Setup Steps:"
echo ""
echo "STEP 1: Open Xcode and sign in with your Apple Developer Account"
echo "  → Xcode → Settings (or Preferences) → Accounts"
echo "  → Click '+' if your Apple ID isn't there"
echo "  → Sign in with your PAID Apple Developer account"
echo ""
echo "STEP 2: Create a simple iOS project in Xcode to auto-generate provisioning"
echo "  → File → New → Project"
echo "  → Choose 'App'"
echo "  → Bundle Identifier: com.huynguyen.lunarcalendar"
echo "  → Team: Select your paid developer team"
echo "  → Connect your iPhone and select it as target"
echo "  → Click Run (▶) - Xcode will auto-create provisioning profile"
echo "  → You can close this test project after it runs"
echo ""
echo "STEP 3: After Xcode creates the provisioning profile, come back here and run:"
echo "  ./test-ios-device.sh"
echo ""
echo "═══════════════════════════════════════════════════════════"
echo ""
echo "ALTERNATIVE - Manual Setup at Apple Developer Portal:"
echo ""
echo "1. Go to: https://developer.apple.com/account/resources/devices"
echo "   → Click '+' to register device"
echo "   → Name: Huy's iPhone"
echo "   → UDID: $DEVICE_ID"
echo ""
echo "2. Go to: https://developer.apple.com/account/resources/identifiers"
echo "   → Click '+' to create App ID (if doesn't exist)"
echo "   → Bundle ID: com.huynguyen.lunarcalendar"
echo ""
echo "3. Go to: https://developer.apple.com/account/resources/profiles"
echo "   → Click '+' to create provisioning profile"
echo "   → Select: iOS App Development"
echo "   → App ID: com.huynguyen.lunarcalendar"
echo "   → Select your certificate"
echo "   → Select your device"
echo "   → Download and double-click to install"
echo ""
echo "4. Run: ./test-ios-device.sh"
echo ""
