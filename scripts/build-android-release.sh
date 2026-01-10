#!/bin/bash

echo "🤖 Building Android Release for Google Play..."
echo ""

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
PROJECT_PATH="$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"
CONFIGURATION="Release"
FRAMEWORK="net10.0-android"
KEYSTORE_PROPS="$HOME/.android/keystore/lunarcalendar.properties"

# Check if keystore properties file exists
if [ -f "$KEYSTORE_PROPS" ]; then
  echo "✓ Found keystore properties file"
  # Source the properties file to get variables
  export $(cat "$KEYSTORE_PROPS" | grep -v '^#' | xargs)
else
  echo "⚠️  Keystore properties file not found at: $KEYSTORE_PROPS"
  echo ""
  echo "You have two options:"
  echo ""
  echo "Option 1: Create keystore properties file"
  echo "  Create file: $KEYSTORE_PROPS"
  echo "  With content:"
  echo "    storeFile=$HOME/.android/keystore/lunarcalendar-release.keystore"
  echo "    storePassword=YOUR_PASSWORD"
  echo "    keyAlias=lunarcalendar"
  echo "    keyPassword=YOUR_PASSWORD"
  echo ""
  echo "Option 2: Set environment variables"
  echo "  export ANDROID_KEYSTORE_PASSWORD='your_password'"
  echo "  export ANDROID_KEY_PASSWORD='your_password'"
  echo ""

  # Check if environment variables are set
  if [ -n "$ANDROID_KEYSTORE_PASSWORD" ] && [ -n "$ANDROID_KEY_PASSWORD" ]; then
    echo "✓ Using environment variables for keystore passwords"
    storeFile="$HOME/.android/keystore/lunarcalendar-release.keystore"
    keyAlias="lunarcalendar"
    storePassword="$ANDROID_KEYSTORE_PASSWORD"
    keyPassword="$ANDROID_KEY_PASSWORD"
  else
    echo "❌ No keystore configuration found!"
    exit 1
  fi
fi

# Verify keystore file exists
if [ ! -f "$storeFile" ]; then
  echo "❌ Keystore file not found: $storeFile"
  echo ""
  echo "Create keystore with:"
  echo "  mkdir -p ~/.android/keystore"
  echo "  keytool -genkey -v -keystore $storeFile -alias lunarcalendar -keyalg RSA -keysize 2048 -validity 10000"
  exit 1
fi

echo "✓ Keystore file found: $storeFile"
echo ""

# Clean previous builds
echo "🧹 Cleaning previous builds..."
dotnet clean "$PROJECT_PATH" -c $CONFIGURATION -f $FRAMEWORK

echo ""
echo "🔨 Building release AAB..."
echo "   Framework: $FRAMEWORK"
echo "   Configuration: $CONFIGURATION"
echo "   Package Format: AAB (Android App Bundle)"
echo ""

# Build release AAB
dotnet publish "$PROJECT_PATH" \
  -f $FRAMEWORK \
  -c $CONFIGURATION \
  -p:AndroidPackageFormat=aab \
  -p:AndroidKeyStore=true \
  -p:AndroidSigningKeyStore="$storeFile" \
  -p:AndroidSigningKeyAlias="$keyAlias" \
  -p:AndroidSigningKeyPass="$keyPassword" \
  -p:AndroidSigningStorePass="$storePassword"

# Check if build succeeded
if [ $? -eq 0 ]; then
  echo ""
  echo "✅ Android Release build completed successfully!"
  echo ""
  echo "📦 AAB location:"
  find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Release/net10.0-android" -name "*-Signed.aab" -exec ls -lh {} \; 2>/dev/null

  # Verify signature
  AAB_FILE=$(find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Release/net10.0-android" -name "*-Signed.aab" 2>/dev/null | head -1)
  if [ -n "$AAB_FILE" ]; then
    echo ""
    echo "🔐 Verifying AAB signature..."
    if jarsigner -verify "$AAB_FILE" &> /dev/null; then
      echo "✅ AAB is properly signed"
    else
      echo "⚠️  AAB signature verification failed - may need to check signing configuration"
    fi
  fi

  echo ""
  echo "📋 Next steps:"
  echo "1. Go to Google Play Console: https://play.google.com/console"
  echo "2. Select your app"
  echo "3. Navigate to: Production → Releases"
  echo "4. Create new release"
  echo "5. Upload the AAB file above"
  echo "6. Add release notes and submit for review"
else
  echo ""
  echo "❌ Android Release build failed!"
  echo "Check error messages above for details."
  exit 1
fi
