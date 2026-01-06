#!/bin/bash
# Quick provisioning profile setup for iOS device testing

set -e

echo "🔐 Setting up iOS Provisioning Profile"
echo "========================================"
echo ""

BUNDLE_ID="com.huynguyen.lunarcalendar"
TEAM_ID=$(security find-certificate -a -c "Apple Development" -p | openssl x509 -noout -text | grep "OU=" | head -n 1 | sed 's/.*OU=\([^,]*\).*/\1/' | xargs)

if [ -z "$TEAM_ID" ]; then
    echo "❌ Could not find Team ID from certificate"
    echo ""
    echo "Please sign in to Xcode:"
    echo "  1. Open Xcode"
    echo "  2. Go to Settings → Accounts"
    echo "  3. Sign in with your Apple ID"
    echo "  4. Make sure your team is selected"
    echo ""
    exit 1
fi

echo "✓ Team ID: $TEAM_ID"
echo "✓ Bundle ID: $BUNDLE_ID"
echo ""

# Create a simple Xcode project to trigger provisioning profile creation
TEMP_DIR=$(mktemp -d)
cd "$TEMP_DIR"

echo "📦 Creating temporary Xcode project to generate profile..."

cat > project.yml << YAML
name: TempApp
options:
  bundleIdPrefix: com.huynguyen
targets:
  TempApp:
    type: application
    platform: iOS
    deploymentTarget: "15.0"
    settings:
      PRODUCT_BUNDLE_IDENTIFIER: $BUNDLE_ID
      DEVELOPMENT_TEAM: $TEAM_ID
      CODE_SIGN_STYLE: Automatic
YAML

echo ""
echo "Now open Xcode and follow these steps:"
echo "========================================"
echo ""
echo "Option 1 - Quick & Easy (Recommended):"
echo "  1. Open Xcode"
echo "  2. File → New → Project"
echo "  3. Choose 'App' template"
echo "  4. Product Name: LunarCalendar"
echo "  5. Bundle Identifier: $BUNDLE_ID"
echo "  6. Team: Select your team"
echo "  7. Click 'Next' and save anywhere"
echo "  8. Connect your iPhone"
echo "  9. Select your iPhone as destination"
echo "  10. Click the Play button (▶️) to build"
echo "  11. Close the project when done"
echo ""
echo "This will automatically create the provisioning profile!"
echo ""
echo "Option 2 - Using Existing Project:"
echo "  1. Open your .csproj in Xcode (if you have an .xcodeproj)"
echo "  2. Select project → Signing & Capabilities"
echo "  3. Check 'Automatically manage signing'"
echo "  4. Select your Team"
echo "  5. Xcode will create the profile automatically"
echo ""

# Clean up
cd -
rm -rf "$TEMP_DIR"

echo "After Xcode creates the profile, run: ./test-ios-device.sh"
echo ""
