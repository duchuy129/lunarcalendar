#!/bin/bash
# Capture iOS device logs while app is running

DEVICE_ID="00008110-001E38192E50401E"
BUNDLE_ID="com.huynguyen.lunarcalendar"

echo "📱 iOS Device Log Capture"
echo "=========================="
echo ""
echo "Instructions:"
echo "1. This script will start capturing logs"
echo "2. Launch the app on your iPhone"
echo "3. Watch for crash messages below"
echo "4. Press Ctrl+C to stop capturing"
echo ""
echo "Starting log capture in 3 seconds..."
sleep 3

# Try to launch the app and capture logs
echo ""
echo "Attempting to launch app and capture logs..."
echo "=========================================="
echo ""

# Launch app
xcrun devicectl device process launch --device $DEVICE_ID $BUNDLE_ID 2>&1 &
LAUNCH_PID=$!

# Capture logs
echo "Capturing logs (press Ctrl+C to stop)..."
xcrun devicectl device info log --device $DEVICE_ID | grep -i "lunar\|error\|exception\|crash" --line-buffered

# Cleanup
kill $LAUNCH_PID 2>/dev/null || true
