#!/bin/bash

echo "🍎 Building iOS Release for App Store Distribution"
echo "=================================================="
echo ""

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
PROJECT_PATH="$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"
PROVISIONING_PROFILE_PATH="$WORKSPACE_ROOT/releases/Lunar_Calendar_App_Store.mobileprovision"
CONFIGURATION="Release"
FRAMEWORK="net10.0-ios"
RUNTIME="ios-arm64"

# Verify prerequisites
echo "🔍 Verifying prerequisites..."

# Check if provisioning profile exists
if [ ! -f "$PROVISIONING_PROFILE_PATH" ]; then
  echo "❌ Provisioning profile not found at: $PROVISIONING_PROFILE_PATH"
  exit 1
fi

# Verify Apple Distribution certificate
DISTRIBUTION_CERT=$(security find-identity -v -p codesigning | grep "Apple Distribution" | head -n 1)
if [ -z "$DISTRIBUTION_CERT" ]; then
  echo "❌ Apple Distribution certificate not found in keychain!"
  echo ""
  echo "Please ensure you have installed the Apple Distribution certificate."
  echo "You can download it from: https://developer.apple.com/account/resources/certificates/list"
  exit 1
fi

echo "✅ Apple Distribution certificate found:"
echo "   $DISTRIBUTION_CERT"

# Verify provisioning profile details
echo ""
echo "📋 Provisioning Profile Details:"
PROFILE_NAME=$(security cms -D -i "$PROVISIONING_PROFILE_PATH" | plutil -extract Name raw - 2>/dev/null)
PROFILE_TEAM=$(security cms -D -i "$PROVISIONING_PROFILE_PATH" | plutil -extract TeamName raw - 2>/dev/null)
PROFILE_BUNDLE_ID=$(security cms -D -i "$PROVISIONING_PROFILE_PATH" | plutil -extract Entitlements.application-identifier raw - 2>/dev/null)

echo "   Name: $PROFILE_NAME"
echo "   Team: $PROFILE_TEAM"
echo "   Bundle ID: $PROFILE_BUNDLE_ID"

# Install provisioning profile to system
echo ""
echo "📦 Installing provisioning profile to system..."
PROFILE_UUID=$(security cms -D -i "$PROVISIONING_PROFILE_PATH" | plutil -extract UUID raw - 2>/dev/null)
PROFILES_DIR="$HOME/Library/MobileDevice/Provisioning Profiles"
mkdir -p "$PROFILES_DIR"
cp "$PROVISIONING_PROFILE_PATH" "$PROFILES_DIR/$PROFILE_UUID.mobileprovision"
echo "✅ Installed: $PROFILE_UUID.mobileprovision"

# Clean previous builds
echo ""
echo "🧹 Cleaning previous builds..."
dotnet clean "$PROJECT_PATH" -c $CONFIGURATION -f $FRAMEWORK

# Remove any existing obj/bin folders for clean build
rm -rf "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/obj/Release/net10.0-ios"
rm -rf "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Release/net10.0-ios"

echo ""
echo "🔨 Building App Store IPA..."
echo "   Framework: $FRAMEWORK"
echo "   Configuration: $CONFIGURATION"
echo "   Runtime: $RUNTIME"
echo "   Provisioning: $PROFILE_NAME"
echo ""

# Build release with App Store settings
# The .csproj already has the correct settings in the Release configuration
# for ios-arm64, so we just need to ensure we're building with Release config
dotnet publish "$PROJECT_PATH" \
  -f $FRAMEWORK \
  -c $CONFIGURATION \
  -r $RUNTIME \
  -p:ArchiveOnBuild=true \
  -p:RuntimeIdentifier=ios-arm64 \
  -p:BuildIpa=true \
  -p:CodesignKey="Apple Distribution" \
  -p:CodesignProvision="Lunar Calendar App Store" \
  -p:EnableCodeSigning=true

# Check if build succeeded
if [ $? -eq 0 ]; then
  echo ""
  echo "✅ App Store IPA build completed successfully!"
  echo ""

  # Find the IPA
  IPA_PATH=$(find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Release/net10.0-ios" -name "*.ipa" | head -n 1)

  if [ -n "$IPA_PATH" ]; then
    echo "📦 IPA Details:"
    ls -lh "$IPA_PATH"
    echo ""

    # Verify IPA signature
    echo "🔐 Verifying IPA signature..."
    TEMP_DIR=$(mktemp -d)
    unzip -q "$IPA_PATH" -d "$TEMP_DIR"
    APP_PATH=$(find "$TEMP_DIR" -name "*.app" | head -n 1)

    if [ -n "$APP_PATH" ]; then
      CODESIGN_INFO=$(codesign -dvvv "$APP_PATH" 2>&1)
      echo "$CODESIGN_INFO" | grep -E "Authority|TeamIdentifier|Identifier"

      # Check if signed with Distribution cert
      if echo "$CODESIGN_INFO" | grep -q "Apple Distribution"; then
        echo ""
        echo "✅ IPA is correctly signed with Apple Distribution certificate"
      else
        echo ""
        echo "⚠️  WARNING: IPA may not be signed with correct certificate!"
        echo "   Expected: Apple Distribution"
        echo "   Found:"
        echo "$CODESIGN_INFO" | grep "Authority"
      fi
    fi

    rm -rf "$TEMP_DIR"

    echo ""
    echo "📍 IPA Location:"
    echo "   $IPA_PATH"
    echo ""
    echo "📋 Next Steps:"
    echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
    echo ""
    echo "Option 1: Upload via Transporter (Recommended)"
    echo "  1. Open Transporter app from Mac App Store"
    echo "  2. Sign in with your Apple ID"
    echo "  3. Drag and drop the IPA file"
    echo "  4. Click 'Deliver'"
    echo ""
    echo "Option 2: Upload via Command Line"
    echo "  xcrun altool --upload-app \\"
    echo "    -f \"$IPA_PATH\" \\"
    echo "    -t ios \\"
    echo "    -u <your-apple-id> \\"
    echo "    -p <app-specific-password>"
    echo ""
    echo "Option 3: Upload via xcrun (Xcode 13+)"
    echo "  xcrun altool --upload-package \"$IPA_PATH\" \\"
    echo "    --type ios \\"
    echo "    --apple-id <your-apple-id> \\"
    echo "    --password <app-specific-password>"
    echo ""
    echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
    echo ""
    echo "📝 Notes:"
    echo "  • App-specific password: https://appleid.apple.com/account/manage"
    echo "  • After upload, go to App Store Connect to submit for review"
    echo "  • Processing usually takes 5-15 minutes"
    echo ""
  else
    echo "⚠️  Warning: Could not find IPA file"
    echo "   Searched in: $WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Release/net10.0-ios"
  fi
else
  echo ""
  echo "❌ App Store IPA build failed!"
  echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
  echo ""
  echo "Common issues:"
  echo "  1. Certificate not installed: Check Keychain Access for 'Apple Distribution'"
  echo "  2. Provisioning profile mismatch: Verify bundle ID matches"
  echo "  3. Expired provisioning profile: Check expiration date"
  echo ""
  echo "To diagnose:"
  echo "  • Check Keychain Access → My Certificates"
  echo "  • Verify provisioning profile: security cms -D -i \"$PROVISIONING_PROFILE_PATH\""
  echo ""
  exit 1
fi
