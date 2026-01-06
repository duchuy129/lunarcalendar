#!/bin/bash
DEVICE_ID="00008110-001E38192E50401E"

echo "Fetching crash logs from device..."
echo ""

# Try to get crash logs using devicectl
xcrun devicectl device info crashlogs --device $DEVICE_ID list 2>/dev/null | tail -20

echo ""
echo "Latest crash info:"
xcrun devicectl device info crashlogs --device $DEVICE_ID list --format json 2>/dev/null | python3 -c "
import json, sys
try:
    data = json.load(sys.stdin)
    # Process JSON output
    print(json.dumps(data, indent=2))
except:
    print('Could not parse crash logs')
" 2>/dev/null || echo "JSON parsing failed"

