#!/bin/bash

echo "🤖 Deploying to Android Emulator..."

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"

# Find available emulators
echo "📱 Available emulators:"
~/Library/Android/sdk/emulator/emulator -list-avds | head -3

# Use first available emulator
EMULATOR_NAME=$(~/Library/Android/sdk/emulator/emulator -list-avds | head -1)

if [ -z "$EMULATOR_NAME" ]; then
  echo "❌ No emulators found!"
  exit 1
fi

echo ""
echo "🚀 Starting emulator: $EMULATOR_NAME"
~/Library/Android/sdk/emulator/emulator -avd "$EMULATOR_NAME" > /dev/null 2>&1 &
EMULATOR_PID=$!

echo "⏳ Waiting for emulator to boot..."
~/Library/Android/sdk/platform-tools/adb wait-for-device
sleep 10

# Find APK
APK_FILE=$(find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Debug/net10.0-android" -name "*-Signed.apk" 2>/dev/null | head -1)

if [ -z "$APK_FILE" ]; then
  echo "Looking for unsigned APK..."
  APK_FILE=$(find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Debug/net10.0-android" -name "*.apk" 2>/dev/null | head -1)
fi

if [ -z "$APK_FILE" ]; then
  echo "❌ No APK found!"
  exit 1
fi

echo ""
echo "📦 Installing: $APK_FILE"
~/Library/Android/sdk/platform-tools/adb install -r "$APK_FILE"

if [ $? -eq 0 ]; then
  echo ""
  echo "🚀 Launching app..."
  ~/Library/Android/sdk/platform-tools/adb shell am start -n com.huynguyen.lunarcalendar/crc64b457a836cc4fb5b9.MainActivity

  echo ""
  echo "✅ App deployed and launched on Android emulator!"
  echo "📱 Emulator PID: $EMULATOR_PID (kill with: kill $EMULATOR_PID)"
else
  echo "❌ Installation failed!"
  exit 1
fi
