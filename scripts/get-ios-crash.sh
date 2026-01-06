#!/bin/bash

echo "Attempting to get crash logs from iPhone..."
echo ""

# Method 1: Check Xcode Organizer location
ORGANIZER_PATH="$HOME/Library/Logs/DiagnosticReports"
echo "Checking for crash logs in: $ORGANIZER_PATH"
find "$ORGANIZER_PATH" -name "*LunarCalendar*" -mtime -1 2>/dev/null | head -5

echo ""
echo "Latest crash reports (last 2 hours):"
find "$ORGANIZER_PATH" -name "*.crash" -mtime -1 2>/dev/null | xargs ls -lt 2>/dev/null | head -10

