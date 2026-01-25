#!/bin/bash

# Script to capture iOS crash logs for debugging

echo "=== iOS Crash Log Capture Tool ==="
echo ""
echo "Instructions:"
echo "1. Connect your iOS device via USB"
echo "2. Deploy and run the app (it will crash)"
echo "3. Run this script IMMEDIATELY after the crash"
echo ""
echo "Press Enter to continue..."
read

echo ""
echo "Capturing crash logs..."
echo ""

# Get device UDID
UDID=$(idevice_id -l 2>/dev/null | head -1)

if [ -z "$UDID" ]; then
    echo "No iOS device found via USB. Trying alternative method..."
    UDID=$(xcrun xctrace list devices 2>/dev/null | grep -v "Simulator" | grep -v "=" | grep "(" | head -1 | sed 's/.*(\(.*\)).*/\1/')
fi

if [ -z "$UDID" ]; then
    echo "ERROR: Cannot find connected iOS device"
    echo ""
    echo "Alternative: Check Xcode Console"
    echo "1. Open Xcode"
    echo "2. Window > Devices and Simulators"
    echo "3. Select your device"
    echo "4. Click 'View Device Logs'"
    echo "5. Find the most recent crash for LunarCalendar.MobileApp"
    exit 1
fi

echo "Found device: $UDID"
echo ""

# Try to get crash reports
CRASH_DIR=~/Library/Logs/CrashReporter/MobileDevice/$UDID
CRASHES_FOUND=false

if [ -d "$CRASH_DIR" ]; then
    echo "Searching for recent crashes..."
    LATEST_CRASH=$(find "$CRASH_DIR" -name "*LunarCalendar*" -type f -mmin -5 | head -1)

    if [ -n "$LATEST_CRASH" ]; then
        echo "Found crash log: $LATEST_CRASH"
        echo ""
        echo "=== CRASH LOG CONTENT ==="
        cat "$LATEST_CRASH"
        echo ""
        echo "=== END CRASH LOG ==="
        CRASHES_FOUND=true
    fi
fi

# Try Console.app logs
echo ""
echo "Checking system console logs..."
log show --predicate 'process == "LunarCalendar.MobileApp"' --last 5m 2>/dev/null | tail -100

if [ "$CRASHES_FOUND" = false ]; then
    echo ""
    echo "No crash logs found automatically."
    echo ""
    echo "MANUAL STEPS TO GET CRASH LOGS:"
    echo "================================"
    echo ""
    echo "Method 1: Xcode Device Console"
    echo "1. Open Xcode"
    echo "2. Window > Devices and Simulators (Shift+Cmd+2)"
    echo "3. Select your iOS device on the left"
    echo "4. Click 'Open Console' button at bottom"
    echo "5. Run your app"
    echo "6. Watch for crash output in real-time"
    echo ""
    echo "Method 2: Device Logs"
    echo "1. Settings > Privacy & Security > Analytics & Improvements"
    echo "2. Analytics Data"
    echo "3. Find LunarCalendar crash"
    echo "4. Tap and use Share button to export"
    echo ""
    echo "Method 3: Mac Console App"
    echo "1. Open Console.app on Mac"
    echo "2. Select your iOS device in sidebar"
    echo "3. Start streaming"
    echo "4. Run the app"
    echo "5. Look for crash messages"
fi

echo ""
echo "Done."
