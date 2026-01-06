#!/bin/bash
# Launch the app and capture logs

DEVICE_ID="00008110-001E38192E50401E"
BUNDLE_ID="com.huynguyen.lunarcalendar"

echo "Step 1: Clear old logs..."
# Try to terminate the app first
xcrun devicectl device process kill --device $DEVICE_ID --name "$BUNDLE_ID" 2>/dev/null || true

echo ""
echo "Step 2: Launch app and capture logs..."
echo "Please launch the app on your iPhone NOW..."
echo ""

# Use Console.app approach
echo "Opening Console.app to view live logs..."
open -a Console

echo ""
echo "In Console.app:"
echo "1. Select your iPhone in the left sidebar"
echo "2. In the search box, type: process:LunarCalendar"
echo "3. Click 'Start' streaming"
echo "4. Launch the app on your phone"
echo "5. Watch for ERROR or EXCEPTION messages"
echo ""
echo "Look for any messages containing:"
echo "  - 'exception'"
echo "  - 'crash'"
echo "  - 'error'"
echo "  - 'LunarCalendar'"

