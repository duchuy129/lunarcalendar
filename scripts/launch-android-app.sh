#!/bin/bash

echo "🤖 Launching Lunar Calendar on Android Emulator..."

# Check if emulator is running
DEVICE=$(~/Library/Android/sdk/platform-tools/adb devices | grep -v "List" | grep "device" | awk '{print $1}')

if [ -z "$DEVICE" ]; then
  echo "📱 No emulator running, starting one..."
  EMULATOR_NAME=$(~/Library/Android/sdk/emulator/emulator -list-avds | head -1)
  ~/Library/Android/sdk/emulator/emulator -avd "$EMULATOR_NAME" > /dev/null 2>&1 &
  echo "⏳ Waiting for emulator to boot..."
  ~/Library/Android/sdk/platform-tools/adb wait-for-device
  sleep 15
else
  echo "✅ Emulator is already running: $DEVICE"
fi

echo ""
echo "🚀 Launching Lunar Calendar app..."

# Try to launch the app
~/Library/Android/sdk/platform-tools/adb shell monkey -p com.huynguyen.lunarcalendar 1

if [ $? -eq 0 ]; then
  echo ""
  echo "✅ App launched successfully!"
  echo ""
  echo "📸 To take screenshots:"
  echo "1. Navigate to different screens in the app"
  echo "2. Use one of these methods:"
  echo ""
  echo "   Method 1: Emulator Screenshot Tool"
  echo "   - Click the camera icon in emulator toolbar"
  echo "   - Screenshots save to ~/Desktop"
  echo ""
  echo "   Method 2: Command line (run this for each screen):"
  echo "   ~/Library/Android/sdk/platform-tools/adb exec-out screencap -p > screenshot-1.png"
  echo ""
  echo "   Method 3: Emulator keyboard shortcut"
  echo "   - Press Cmd+S in the emulator window"
else
  echo ""
  echo "⚠️  App launch had an issue, trying alternative method..."
  
  # Get the actual activity name from the installed app
  PACKAGE="com.huynguyen.lunarcalendar"
  
  echo "📱 Opening app from launcher..."
  ~/Library/Android/sdk/platform-tools/adb shell am start -a android.intent.action.MAIN -c android.intent.category.LAUNCHER -n $PACKAGE/.MainActivity
  
  if [ $? -ne 0 ]; then
    echo ""
    echo "💡 Manually open the app:"
    echo "1. Look at the emulator screen"
    echo "2. Swipe up or click the apps icon"
    echo "3. Find and tap 'Lunar Calendar'"
  fi
fi

echo ""
echo "📱 Emulator window should be visible on your screen"
