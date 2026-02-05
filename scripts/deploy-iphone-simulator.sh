#!/bin/bash

echo "📱 Deploying to iPhone 16 Pro Simulator..."

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"

# iPhone 16 Pro (iOS 18.2)
DEVICE_ID="928962C4-CCDB-40DF-92B8-8794CB867716"
PROJECT_PATH="$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"

echo "🔌 Booting iPhone simulator..."
xcrun simctl boot "$DEVICE_ID" 2>/dev/null || echo "Already booted"

echo ""
echo "🔨 Building for iPhone simulator..."
dotnet build "$PROJECT_PATH" \
  -f net10.0-ios \
  -c Debug \
  -p:RuntimeIdentifier=iossimulator-arm64

if [ $? -ne 0 ]; then
  echo "❌ Build failed!"
  exit 1
fi

APP_PATH=$(find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/iossimulator-arm64" -name "*.app" -type d | head -1)

echo ""
echo "📱 Installing on iPhone simulator..."
xcrun simctl install "$DEVICE_ID" "$APP_PATH"

echo ""
echo "🚀 Launching app..."
APP_IDENTIFIER=$(defaults read "$APP_PATH/Info.plist" CFBundleIdentifier)
xcrun simctl launch "$DEVICE_ID" "$APP_IDENTIFIER"

echo ""
echo "✅ Success! App is now running on iPhone 16 Pro simulator"
echo ""
echo "📝 How to use DateDetailPage:"
echo "1. You'll see the calendar view"
echo "2. TAP on any date cell in the calendar"
echo "3. DateDetailPage will open showing:"
echo "   - Gregorian date"
echo "   - Lunar date"
echo "   - Sexagenary cycle (Can Chi / 干支)"
echo "   - Element information with color"
echo "   - Holiday info (if applicable)"
echo "4. Tap 'Back' button to return to calendar"
