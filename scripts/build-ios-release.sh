#!/bin/bash

echo "🍎 Building iOS Release for App Store..."
echo ""

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
PROJECT_PATH="$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"
CONFIGURATION="Release"
FRAMEWORK="net10.0-ios"
RUNTIME="ios-arm64"

# Clean previous builds
echo "🧹 Cleaning previous builds..."
dotnet clean "$PROJECT_PATH" -c $CONFIGURATION -f $FRAMEWORK

echo ""
echo "🔨 Building release IPA..."
echo "   Framework: $FRAMEWORK"
echo "   Configuration: $CONFIGURATION"
echo "   Runtime: $RUNTIME"
echo ""

# Build release
dotnet publish "$PROJECT_PATH" \
  -f $FRAMEWORK \
  -c $CONFIGURATION \
  -r $RUNTIME \
  -p:ArchiveOnBuild=true \
  -p:RuntimeIdentifier=ios-arm64 \
  -p:BuildIpa=true

# Check if build succeeded
if [ $? -eq 0 ]; then
  echo ""
  echo "✅ iOS Release build completed successfully!"
  echo ""
  echo "📦 IPA location:"
  find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Release/net10.0-ios" -name "*.ipa" -exec ls -lh {} \; 2>/dev/null

  echo ""
  echo "📋 Next steps:"
  echo "1. Open Transporter app (Mac App Store)"
  echo "2. Sign in with Apple ID"
  echo "3. Drag and drop the IPA file"
  echo "4. Click 'Deliver'"
  echo ""
  echo "Or upload via command line:"
  echo "xcrun altool --upload-app -f <ipa_path> -t ios -u <apple_id> -p <app_specific_password>"
else
  echo ""
  echo "❌ iOS Release build failed!"
  echo "Check error messages above for details."
  exit 1
fi
