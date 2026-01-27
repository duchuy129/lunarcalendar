#!/bin/bash

echo "🤖 Building Android Release AAB for Google Play..."
echo "=================================================="

# Project configuration
PROJECT_PATH="src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"
CONFIGURATION="Release"
FRAMEWORK="net10.0-android"

# Keystore configuration
KEYSTORE_PROPS="$HOME/.android/keystore/lunarcalendar.properties"

# Check if keystore properties exist
if [ ! -f "$KEYSTORE_PROPS" ]; then
    echo "❌ Error: Keystore not found!"
    echo ""
    echo "Please run: ./create-android-keystore.sh first"
    echo ""
    exit 1
fi

# Load keystore properties
echo "📋 Loading keystore configuration..."
source "$KEYSTORE_PROPS"

echo "✅ Keystore found"
echo ""

# Verify keystore file exists
if [ ! -f "$storeFile" ]; then
    echo "❌ Error: Keystore file not found at: $storeFile"
    echo ""
    echo "Please run: ./create-android-keystore.sh"
    echo ""
    exit 1
fi

# Clean previous builds
echo "1️⃣ Cleaning previous builds..."
dotnet clean "$PROJECT_PATH" -c $CONFIGURATION -f $FRAMEWORK

# Restore packages
echo ""
echo "2️⃣ Restoring NuGet packages..."
dotnet restore "$PROJECT_PATH"

# Build and publish AAB
echo ""
echo "3️⃣ Building signed Android App Bundle (AAB)..."
echo ""

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
    echo "======================================"
    echo "✅ BUILD SUCCESSFUL!"
    echo "======================================"
    echo ""

    # Find the AAB file
    AAB_FILE=$(find src/LunarCalendar.MobileApp/bin/Release/net10.0-android -name "*-Signed.aab" | head -1)

    if [ -f "$AAB_FILE" ]; then
        echo "📦 Your Android App Bundle (AAB) is ready:"
        echo ""
        echo "   $AAB_FILE"
        echo ""

        # Show file size
        FILE_SIZE=$(du -h "$AAB_FILE" | cut -f1)
        echo "   Size: $FILE_SIZE"
        echo ""

        echo "======================================"
        echo "📤 NEXT STEPS:"
        echo "======================================"
        echo ""
        echo "1. Go to Google Play Console:"
        echo "   https://play.google.com/console"
        echo ""
        echo "2. Upload this AAB file to:"
        echo "   - Internal testing (if required)"
        echo "   - OR Production track (recommended)"
        echo ""
        echo "3. File to upload:"
        echo "   $AAB_FILE"
        echo ""
        echo "4. See guide: GOOGLE_PLAY_DIRECT_PRODUCTION.md"
        echo ""
    else
        echo "⚠️  Warning: AAB file not found in expected location"
        echo ""
        echo "Search for it with:"
        echo "find src/LunarCalendar.MobileApp/bin/Release -name '*.aab'"
        echo ""
    fi
else
    echo ""
    echo "======================================"
    echo "❌ BUILD FAILED"
    echo "======================================"
    echo ""
    echo "Please check the error messages above."
    echo ""
    echo "Common issues:"
    echo "  - Incorrect keystore password"
    echo "  - Missing Android SDK components"
    echo "  - Code compilation errors"
    echo ""
    exit 1
fi
