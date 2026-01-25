# Debugging Guide for Database Issues

## Debug Logs Added

I've added comprehensive debug logging to `LunarCalendarDatabase.cs` with the `[DB]` prefix. Here's what's being logged:

### Database Operations Logged:
1. **Initialization** - Database path and table creation
2. **Save Operations** - Individual and batch saves with counts
3. **Query Operations** - What data is being fetched and how many records
4. **Errors** - Full exception messages and stack traces

## How to View the Logs

### Option 1: Console.app (Recommended for iPhone)
1. Open Console.app on your Mac
2. Select your iPhone "Huy's iPhone" in the left sidebar
3. Click the search box and enter: `process:LunarCalendar`
4. Click "Start" to begin streaming
5. Launch the app on your iPhone
6. Look for lines starting with `[DB]`

### Option 2: Xcode Device Console
1. Open Xcode
2. Window → Devices and Simulators
3. Select your iPhone
4. Click "Open Console" button
5. Search for `[DB]` in the filter box

### Option 3: Terminal Stream
```bash
# Stream logs in real-time
log stream --predicate 'process == "LunarCalendar"' --style compact

# Or filter for just DB logs
log stream --predicate 'process == "LunarCalendar"' --style compact | grep '\[DB\]'
```

### Option 4: iOS Simulator Logs
```bash
# For simulator, use xcrun simctl
xcrun simctl spawn booted log stream --predicate 'process == "LunarCalendar"' | grep '\[DB\]'
```

## What to Look For

### Success Pattern:
```
[DB] InitAsync: Initializing database at path: /path/to/database.db3
[DB] Database initialized successfully
[DB] GetLunarDatesForMonthAsync: Fetching lunar dates for 2026-01
[DB] Found 31 lunar dates for 2026-01
[DB] SaveLunarDatesAsync: Starting to save 31 lunar dates
[DB] Prepared 0 updates and 31 inserts
[DB] Successfully saved 31 lunar dates
```

### Error Pattern:
```
[DB] InitAsync: Initializing database at path: /path/to/database.db3
[DB] ERROR initializing database: <error message>
[DB] Stack trace: <stack trace>
```

### Common Issues to Check:

#### 1. Database Path Issues
Look for:
```
[DB] InitAsync: Initializing database at path: ...
```
- Is the path valid?
- Does the app have write permissions?

#### 2. Transaction Failures
Look for:
```
[DB] ERROR saving lunar dates: ...
```
- Could be constraint violations
- Could be concurrent access issues

#### 3. Data Not Being Saved
Look for:
```
[DB] Prepared 0 updates and 0 inserts
```
- This means no data is being processed

#### 4. Query Returning No Results
Look for:
```
[DB] Found 0 lunar dates for 2026-01
```
- Data might not be saved yet
- Date range might be wrong

## Deploy and Test

### Deploy to iOS Simulator (iOS 26.2):
```bash
# Clean
rm -rf src/LunarCalendar.MobileApp/bin src/LunarCalendar.MobileApp/obj

# Build and run on simulator
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net10.0-ios -c Debug \
  /p:RuntimeIdentifier=iossimulator-arm64 \
  /t:Run
```

### Deploy to iPhone (iOS 26.1):
```bash
# Clean
rm -rf src/LunarCalendar.MobileApp/bin src/LunarCalendar.MobileApp/obj

# Build for device
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net10.0-ios -c Debug \
  /p:RuntimeIdentifier=ios-arm64

# Install on device (use Xcode or devicectl)
# Then launch manually on the device
```

## Next Steps

1. **Deploy the app** with the new logging
2. **Launch the app** and use it normally
3. **Capture the logs** using one of the methods above
4. **Look for `[DB]` messages** to see what's happening
5. **Share any ERROR messages** you find

The logs will show exactly:
- When database operations start
- What data is being processed
- Whether operations succeed or fail
- Full error details if something goes wrong
