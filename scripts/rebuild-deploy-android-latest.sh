#!/bin/bash

echo "🤖 Building LATEST Android Code and Deploying..."
echo ""

PROJECT_PATH="src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"

# Step 1: Clean everything
echo "1️⃣ Cleaning all previous builds..."
dotnet clean "$PROJECT_PATH" -c Debug -f net10.0-android
rm -rf src/LunarCalendar.MobileApp/bin/Debug/net10.0-android
rm -rf src/LunarCalendar.MobileApp/obj/Debug/net10.0-android
echo "✅ Clean complete"

# Step 2: Restore packages
echo ""
echo "2️⃣ Restoring packages..."
dotnet restore "$PROJECT_PATH"
echo "✅ Restore complete"

# Step 3: Build fresh
echo ""
echo "3️⃣ Building with latest code..."
dotnet build "$PROJECT_PATH" \
  -f net10.0-android \
  -c Debug

if [ $? -ne 0 ]; then
  echo "❌ Build failed!"
  exit 1
fi

echo "✅ Build complete"

# Step 4: Find APK
echo ""
echo "4️⃣ Finding APK..."
APK_FILE=$(find src/LunarCalendar.MobileApp/bin/Debug/net10.0-android -name "*-Signed.apk" 2>/dev/null | head -1)

if [ -z "$APK_FILE" ]; then
  APK_FILE=$(find src/LunarCalendar.MobileApp/bin/Debug/net10.0-android -name "*.apk" 2>/dev/null | head -1)
fi

if [ -z "$APK_FILE" ]; then
  echo "❌ APK not found!"
  exit 1
fi

echo "✅ APK: $APK_FILE"
ls -lh "$APK_FILE"

# Step 5: Check emulator
echo ""
echo "5️⃣ Checking emulator..."
DEVICE=$(~/Library/Android/sdk/platform-tools/adb devices | grep -v "List" | grep "device" | awk '{print $1}')

if [ -z "$DEVICE" ]; then
  echo "Starting emulator..."
  EMULATOR_NAME=$(~/Library/Android/sdk/emulator/emulator -list-avds | head -1)
  ~/Library/Android/sdk/emulator/emulator -avd "$EMULATOR_NAME" > /dev/null 2>&1 &
  ~/Library/Android/sdk/platform-tools/adb wait-for-device
  sleep 15
  echo "✅ Emulator started"
else
  echo "✅ Emulator running: $DEVICE"
fi

# Step 6: Uninstall old version
echo ""
echo "6️⃣ Removing old version..."
~/Library/Android/sdk/platform-tools/adb uninstall com.huynguyen.lunarcalendar 2>/dev/null
echo "✅ Old version removed"

# Step 7: Install new version
echo ""
echo "7️⃣ Installing LATEST version..."
~/Library/Android/sdk/platform-tools/adb install "$APK_FILE"

if [ $? -ne 0 ]; then
  echo "❌ Installation failed!"
  exit 1
fi

echo "✅ Installation complete"

# Step 8: Launch
echo ""
echo "8️⃣ Launching app..."
ACTIVITY=$(~/Library/Android/sdk/platform-tools/adb shell cmd package resolve-activity --brief com.huynguyen.lunarcalendar | tail -1)
~/Library/Android/sdk/platform-tools/adb shell am start -n "$ACTIVITY"

echo ""
echo "🎉 SUCCESS! Latest code is now running on emulator!"
echo ""
echo "Build info:"
echo "  APK: $(basename $APK_FILE)"
echo "  Size: $(ls -lh $APK_FILE | awk '{print $5}')"
echo "  Built: $(date)"
echo ""
echo "📸 Ready to take screenshots!"
