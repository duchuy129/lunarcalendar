#!/bin/bash

echo "📱 Deploying to iPad with Automatic Provisioning..."

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"

DEVICE_ID="60A80AA4-D712-579C-8C62-BB756F8E5706"
PROJECT_PATH="$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"

echo "🔨 Building with automatic code signing..."
dotnet build "$PROJECT_PATH" \
  -f net10.0-ios \
  -c Debug \
  -p:RuntimeIdentifier=ios-arm64 \
  -p:CodesignKey="Apple Development" \
  -p:CodesignProvision="" \
  -p:_CodeSigningIdentity="Apple Development" \
  -p:EnableCodeSigning=true

if [ $? -ne 0 ]; then
  echo "❌ Build failed!"
  exit 1
fi

APP_PATH=$(find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/ios-arm64" -name "*.app" -type d | head -1)

echo ""
echo "📱 Installing on iPad..."
xcrun devicectl device install app \
  --device "$DEVICE_ID" \
  "$APP_PATH"

if [ $? -eq 0 ]; then
  echo ""
  echo "✅ Success! App installed on iPad"
  echo "📸 Now take your screenshots!"
else
  echo "❌ Installation failed - need to add iPad to provisioning profile"
fi
