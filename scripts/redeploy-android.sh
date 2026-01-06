#!/bin/bash

echo "🤖 Redeploying Lunar Calendar to Android Emulator..."
echo ""

# Check emulator status
echo "1️⃣ Checking emulator..."
DEVICE=$(~/Library/Android/sdk/platform-tools/adb devices | grep -v "List" | grep "device" | awk '{print $1}')

if [ -z "$DEVICE" ]; then
  echo "❌ No emulator running!"
  echo "Starting emulator..."
  EMULATOR_NAME=$(~/Library/Android/sdk/emulator/emulator -list-avds | head -1)
  ~/Library/Android/sdk/emulator/emulator -avd "$EMULATOR_NAME" > /dev/null 2>&1 &
  ~/Library/Android/sdk/platform-tools/adb wait-for-device
  sleep 15
  echo "✅ Emulator started"
else
  echo "✅ Emulator running: $DEVICE"
fi

# Uninstall old version
echo ""
echo "2️⃣ Uninstalling old version..."
~/Library/Android/sdk/platform-tools/adb uninstall com.huynguyen.lunarcalendar 2>/dev/null
echo "✅ Old version removed"

# Find APK
echo ""
echo "3️⃣ Finding APK..."
APK_FILE=$(find src/LunarCalendar.MobileApp/bin/Debug/net10.0-android -name "*-Signed.apk" 2>/dev/null | head -1)

if [ -z "$APK_FILE" ]; then
  APK_FILE=$(find src/LunarCalendar.MobileApp/bin/Debug/net10.0-android -name "*.apk" 2>/dev/null | head -1)
fi

if [ -z "$APK_FILE" ]; then
  echo "❌ No APK found! Building fresh..."
  dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-android -c Debug
  APK_FILE=$(find src/LunarCalendar.MobileApp/bin/Debug/net10.0-android -name "*.apk" 2>/dev/null | head -1)
fi

echo "✅ Found APK: $APK_FILE"

# Install
echo ""
echo "4️⃣ Installing app..."
~/Library/Android/sdk/platform-tools/adb install -r "$APK_FILE"

if [ $? -ne 0 ]; then
  echo "❌ Installation failed!"
  exit 1
fi

echo "✅ App installed"

# Get correct package and activity name
echo ""
echo "5️⃣ Finding app package info..."
PACKAGE=$(~/Library/Android/sdk/platform-tools/adb shell pm list packages | grep lunarcalendar | cut -d: -f2)
echo "Package: $PACKAGE"

# Get main activity
ACTIVITY=$(~/Library/Android/sdk/platform-tools/adb shell cmd package resolve-activity --brief $PACKAGE | tail -1)
echo "Activity: $ACTIVITY"

# Launch app
echo ""
echo "6️⃣ Launching app..."
~/Library/Android/sdk/platform-tools/adb shell am start -n "$ACTIVITY"

if [ $? -eq 0 ]; then
  echo ""
  echo "✅ SUCCESS! App is now running on emulator!"
  echo ""
  echo "📸 To take screenshots, use the camera icon on the right side of emulator"
else
  echo ""
  echo "⚠️  Trying alternative launch method..."
  ~/Library/Android/sdk/platform-tools/adb shell am start -a android.intent.action.MAIN -c android.intent.category.LAUNCHER com.huynguyen.lunarcalendar
fi

echo ""
echo "📱 Check the emulator window - app should be open!"
