#!/bin/bash

echo "🤖 Rebuilding Android with Latest Updates..."
echo ""

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
PROJECT_PATH="$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj"

# Clean previous builds
echo "🧹 Cleaning previous builds..."
dotnet clean "$PROJECT_PATH" -c Debug -f net10.0-android

echo ""
echo "🔨 Building Debug version..."
dotnet build "$PROJECT_PATH" \
  -f net10.0-android \
  -c Debug

if [ $? -ne 0 ]; then
  echo "❌ Build failed!"
  exit 1
fi

echo ""
echo "✅ Build successful!"

# Find the APK
APK_FILE=$(find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Debug/net10.0-android" -name "*-Signed.apk" 2>/dev/null | head -1)

if [ -z "$APK_FILE" ]; then
  echo "⚠️  No signed APK found, looking for unsigned..."
  APK_FILE=$(find "$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Debug/net10.0-android" -name "*.apk" 2>/dev/null | head -1)
fi

if [ -n "$APK_FILE" ]; then
  echo "📦 APK: $APK_FILE"
  ls -lh "$APK_FILE"
else
  echo "⚠️  APK not found"
fi

echo ""
echo "📱 Next: Deploy to Android emulator"
