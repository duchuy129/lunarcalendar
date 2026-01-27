#!/bin/bash

echo "🔍 Checking for Android devices..."
adb devices -l

if [ -z "$(adb devices | grep 'device$')" ]; then
    echo "❌ No Android device/emulator found!"
    echo "Please start the Android emulator first, then run this script again."
    exit 1
fi

# Get the device ID
DEVICE=$(adb devices | grep 'device$' | head -1 | awk '{print $1}')
echo "📱 Found device: $DEVICE"

echo ""
echo "🗑️  Uninstalling old version..."
adb -s $DEVICE uninstall com.huynguyen.lunarcalendar 2>/dev/null || echo "No previous version found"

echo ""
echo "🧹 Cleaning build artifacts..."
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj >/dev/null 2>&1

echo ""
echo "🔨 Building fresh APK with latest changes..."
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-android -c Debug

if [ $? -ne 0 ]; then
    echo "❌ Build failed!"
    exit 1
fi

echo ""
echo "📦 Installing APK..."
adb -s $DEVICE install -r src/LunarCalendar.MobileApp/bin/Debug/net10.0-android/com.huynguyen.lunarcalendar-Signed.apk

if [ $? -ne 0 ]; then
    echo "❌ Installation failed!"
    exit 1
fi

echo ""
echo "🚀 Launching app..."
adb -s $DEVICE shell am start -n com.huynguyen.lunarcalendar/crc64b457a836cc4fb5b9.MainActivity

echo ""
echo "✅ Deployment complete!"
echo ""
echo "📋 Checking for crashes (waiting 5 seconds)..."
sleep 5

echo ""
echo "🔍 Crash check:"
adb -s $DEVICE logcat -d | grep -E "(FATAL|AndroidRuntime.*Exception)" | tail -10

if [ -z "$(adb -s $DEVICE logcat -d | grep -E 'FATAL')" ]; then
    echo "✅ No crashes detected!"
else
    echo "❌ Crash detected! Check logs above."
fi

echo ""
echo "📱 App status:"
adb -s $DEVICE shell "ps | grep lunarcalendar"
