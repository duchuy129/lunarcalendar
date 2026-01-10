#!/bin/bash

echo "📱 Deploying to iPad Air..."

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"

DEVICE_ID="60A80AA4-D712-579C-8C62-BB756F8E5706"
PROJECT_PATH="$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"

echo "🧹 Cleaning previous builds..."
dotnet clean "$PROJECT_PATH" -c Debug -f net10.0-ios

echo ""
echo "🔨 Building for iPad (iOS device)..."
dotnet build "$PROJECT_PATH" \
  -f net10.0-ios \
  -c Debug \
  -p:RuntimeIdentifier=ios-arm64 \
  -p:CodesignKey="Apple Development" \
  -p:CodesignProvision="com.huynguyen.lunarcalendar"

if [ $? -ne 0 ]; then
  echo "❌ Build failed!"
  exit 1
fi

echo ""
echo "📦 Finding app bundle..."
APP_PATH=$(find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/ios-arm64" -name "*.app" -type d | head -1)

if [ -z "$APP_PATH" ]; then
  echo "❌ App bundle not found!"
  exit 1
fi

echo "✅ Found: $APP_PATH"

echo ""
echo "📱 Installing app on iPad..."
xcrun devicectl device install app --device "$DEVICE_ID" "$APP_PATH"

if [ $? -eq 0 ]; then
  echo ""
  echo "✅ App installed successfully on iPad!"
  echo ""
  echo "📸 Now you can:"
  echo "1. Open the app on your iPad"
  echo "2. Navigate to different screens"
  echo "3. Take screenshots (Power + Volume Up)"
  echo "4. Screenshots will be in Photos app"
  echo "5. AirDrop them to your Mac"
else
  echo ""
  echo "❌ Installation failed!"
  exit 1
fi
