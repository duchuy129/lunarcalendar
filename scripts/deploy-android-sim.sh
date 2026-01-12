#!/bin/bash

# Parse command line arguments
BUILD_CONFIG="Debug"
CLEAN_BUILD=false
AUTO_START_EMULATOR=false

while [[ $# -gt 0 ]]; do
  case $1 in
    --release)
      BUILD_CONFIG="Release"
      shift
      ;;
    --debug)
      BUILD_CONFIG="Debug"
      shift
      ;;
    --clean)
      CLEAN_BUILD=true
      shift
      ;;
    --auto-start)
      AUTO_START_EMULATOR=true
      shift
      ;;
    *)
      echo "Unknown option: $1"
      echo "Usage: $0 [--debug|--release] [--clean] [--auto-start]"
      exit 1
      ;;
  esac
done

echo "🤖 Deploying to Android Emulator (${BUILD_CONFIG} build)..."

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"

# Check if emulator is already running
RUNNING_DEVICE=$(adb devices | grep 'emulator-' | head -1 | awk '{print $1}')

if [ -n "$RUNNING_DEVICE" ]; then
  echo "✅ Emulator already running: $RUNNING_DEVICE"
  EMULATOR_DEVICE=$RUNNING_DEVICE
elif [ "$AUTO_START_EMULATOR" = true ]; then
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
  adb wait-for-device
  sleep 10

  EMULATOR_DEVICE=$(adb devices | grep 'emulator-' | head -1 | awk '{print $1}')
else
  echo "❌ No emulator running! Please start an emulator or use --auto-start"
  echo "Available emulators:"
  ~/Library/Android/sdk/emulator/emulator -list-avds | head -3
  exit 1
fi

# Uninstall old version to ensure fresh deployment
echo ""
echo "🗑️  Uninstalling old version..."
adb -s $EMULATOR_DEVICE uninstall com.huynguyen.lunarcalendar 2>/dev/null || echo "No previous version found"

# Clean build if requested
if [ "$CLEAN_BUILD" = true ]; then
  echo ""
  echo "🧹 Cleaning build artifacts..."
  cd "$WORKSPACE_ROOT"
  dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj > /dev/null 2>&1
fi

# Build the project
echo ""
echo "🔨 Building ${BUILD_CONFIG} APK..."
cd "$WORKSPACE_ROOT"
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net10.0-android -c $BUILD_CONFIG

if [ $? -ne 0 ]; then
  echo "❌ Build failed!"
  exit 1
fi

# Find APK - look in correct configuration folder
BUILD_PATH="$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/${BUILD_CONFIG}/net10.0-android"
APK_FILE=$(find "$BUILD_PATH" -name "*-Signed.apk" 2>/dev/null | head -1)

if [ -z "$APK_FILE" ]; then
  echo "Looking for unsigned APK..."
  APK_FILE=$(find "$BUILD_PATH" -name "*.apk" 2>/dev/null | head -1)
fi

if [ -z "$APK_FILE" ]; then
  echo "❌ No APK found in $BUILD_PATH!"
  exit 1
fi

# Get APK version info
echo ""
echo "📦 APK Details:"
echo "   Path: $APK_FILE"
echo "   Size: $(du -h "$APK_FILE" | awk '{print $1}')"
echo "   Modified: $(stat -f "%Sm" -t "%Y-%m-%d %H:%M:%S" "$APK_FILE")"

echo ""
echo "📲 Installing APK..."
adb -s $EMULATOR_DEVICE install -r "$APK_FILE"

if [ $? -eq 0 ]; then
  echo ""
  echo "🚀 Launching app..."
  adb -s $EMULATOR_DEVICE shell am start -n com.huynguyen.lunarcalendar/crc64b457a836cc4fb5b9.MainActivity

  echo ""
  echo "✅ App deployed and launched on Android emulator!"
  echo "📱 Device: $EMULATOR_DEVICE"

  # Wait a moment for app to start
  sleep 3

  # Check for crashes
  echo ""
  echo "🔍 Checking for crashes..."
  CRASH_CHECK=$(adb -s $EMULATOR_DEVICE logcat -d | grep -E "FATAL.*lunarcalendar" | tail -1)

  if [ -n "$CRASH_CHECK" ]; then
    echo "❌ Crash detected!"
    echo "$CRASH_CHECK"
    echo ""
    echo "📋 Full crash log:"
    adb -s $EMULATOR_DEVICE logcat -d | grep -A 50 "FATAL" | tail -60
    exit 1
  else
    echo "✅ No crashes detected"

    # Show app status
    APP_STATUS=$(adb -s $EMULATOR_DEVICE shell "ps | grep lunarcalendar")
    if [ -n "$APP_STATUS" ]; then
      echo "✅ App is running: $APP_STATUS"
    else
      echo "⚠️  Warning: App process not found"
    fi
  fi

  if [ -n "$EMULATOR_PID" ]; then
    echo "📱 Emulator PID: $EMULATOR_PID (kill with: kill $EMULATOR_PID)"
  fi
else
  echo "❌ Installation failed!"
  exit 1
fi
