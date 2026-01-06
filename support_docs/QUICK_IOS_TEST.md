# Quick iOS Device Testing Guide

Since the app is still crashing, let's get the actual crash information:

## Method 1: Get Crash Report from iPhone (EASIEST)

1. **On your iPhone:**
   - Settings → Privacy & Security → Analytics & Improvements
   - Tap "Analytics Data"
   - Scroll down to find any entries starting with "LunarCalendar"
   - Tap on the most recent one
   - Tap the Share button (top right)
   - AirDrop or Email it to yourself

2. **Once you have the crash report:**
   - Open it and look for the "Exception Type" line
   - Look for the "Termination Reason" section
   - Find the stack trace showing what code was executing

## Method 2: Console.app Filtering

1. **Open Console.app** on your Mac
2. **Select your iPhone** from the left sidebar
3. **Click "Start"** to begin streaming
4. **In the search/filter box at the top**, enter exactly:
   ```
   process:"LunarCalendar.MobileApp"
   ```
5. **Clear the display** (Edit → Clear Display)
6. **Launch the app** on your iPhone
7. **Look for our debug messages:**
   - `=== AppDelegate: FinishedLaunching START ===`
   - `=== Initializing SQLite ===`
   - `=== Creating MauiApp ===`
   - `=== App Constructor START ===`
   - Any lines with `CRASH` in them

## Method 3: Try Minimal Test

Let's test if it's a specific page causing the crash:

Run this modified version:
```bash
./test-ios-device.sh
```

## What To Look For

### If you see NO debug messages at all:
- The crash is happening BEFORE our .NET code runs
- This suggests a framework/runtime issue
- Likely causes:
  - Missing iOS frameworks
  - AOT compilation issue
  - Incompatible .NET MAUI version with iOS 26.x

### If you see some debug messages but it crashes:
- Note the LAST message you see
- This tells us exactly where it's failing:
  - "FinishedLaunching START" but not "END" = crashes in AppDelegate
  - "App Constructor START" but not "END" = crashes in App.xaml.cs
  - "AppShell Constructor START" but not "END" = crashes in AppShell

### Common Crash Signatures:
- **"dyld: Library not loaded"** = Missing framework
- **"Terminated due to signal 9"** = Killed by iOS (out of memory or security)
- **"EXC_BAD_ACCESS"** = Memory corruption / null reference
- **"EXC_CRASH (SIGABRT)"** = Assertion failure or exception

## Next Steps

Once you have the crash report or see which debug message is the last one, we can pinpoint the exact issue!
