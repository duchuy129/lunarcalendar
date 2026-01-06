#!/bin/bash

echo "📱 Deploying to iPad Pro 13-inch Simulator..."

DEVICE_ID="0DE27D4D-782D-4FAA-9EB7-2598F41C5E93"
PROJECT_PATH="src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"

echo "🔌 Booting iPad simulator..."
xcrun simctl boot "$DEVICE_ID" 2>/dev/null || echo "Already booted"

echo ""
echo "🔨 Building for iPad simulator..."
dotnet build "$PROJECT_PATH" \
  -f net10.0-ios \
  -c Debug \
  -p:RuntimeIdentifier=iossimulator-arm64

if [ $? -ne 0 ]; then
  echo "❌ Build failed!"
  exit 1
fi

APP_PATH=$(find src/LunarCalendar.MobileApp/bin/Debug/net10.0-ios/iossimulator-arm64 -name "*.app" -type d | head -1)

echo ""
echo "📱 Installing on iPad simulator..."
xcrun simctl install "$DEVICE_ID" "$APP_PATH"

echo ""
echo "🚀 Launching app..."
xcrun simctl launch "$DEVICE_ID" com.huynguyen.lunarcalendar

echo ""
echo "✅ Success! App is now running on iPad simulator"
echo ""
echo "📸 To take screenshots:"
echo "1. Navigate to different screens in the simulator"
echo "2. Press Cmd+S to save screenshot"
echo "3. Screenshots save to ~/Desktop"
echo "4. They'll be exactly 2064 x 2752 (perfect for App Store!)"
