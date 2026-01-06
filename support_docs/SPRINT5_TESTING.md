# Sprint 5 Testing Guide

## ✅ Fixes Applied

### Issues Fixed:
1. **Holiday API Port Mismatch** ✅
   - Changed from port 5079 → 5090 in [HolidayService.cs](src/LunarCalendar.MobileApp/Services/HolidayService.cs#L19-20)

2. **Background Image Issue** ✅
   - Replaced SVG dragon image with subtle gold background overlay
   - Changed to BoxView with golden tint (#FFD700 at 5% opacity)
   - Added warm cream background (#FFF9F0)

## Features Implemented

### ✅ Backend (API - Port 5090)
- [x] Holiday entity and database seeder (19 Vietnamese holidays)
- [x] Holiday service with lunar-to-Gregorian conversion
- [x] Holiday API endpoints (`/api/holiday`, `/api/holiday/month/{year}/{month}`)
- [x] Service registered in dependency injection

### ✅ Mobile App
- [x] Holiday service integration
- [x] Today's lunar date display in header
- [x] Cultural background (golden tint)
- [x] Holiday color-coding:
  - Red (#DC143C) - Major holidays (Tết, National Day)
  - Gold (#FFD700) - Traditional festivals (Mid-Autumn, Vu Lan)
  - Green (#32CD32) - Seasonal celebrations (Kitchen Gods' Day)
- [x] Holiday names on calendar cells
- [x] Lunar dates below Gregorian dates (dd/mm format)
- [x] iPhone/iPad black screen fix (Info.plist NSAppTransportSecurity)

## Testing Instructions

### Prerequisites
- API running on port 5090 (check with: `curl http://localhost:5090/health`)
- Android emulator or iOS simulator

### Android Testing

**Start Android Emulator:**
```bash
~/Library/Android/sdk/emulator/emulator -avd maui_avd -no-snapshot-load &
```

**Build and Deploy:**
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar/src/LunarCalendar.MobileApp
dotnet build -f net8.0-android
~/Library/Android/sdk/platform-tools/adb install -r bin/Debug/net8.0-android/com.companyname.lunarcalendar.mobileapp-Signed.apk
~/Library/Android/sdk/platform-tools/adb shell am start -n com.companyname.lunarcalendar.mobileapp/crc64b457a836cc4fb5b9.MainActivity
```

**Check Logs:**
```bash
~/Library/Android/sdk/platform-tools/adb logcat | grep -i "lunarcalendar\|holiday"
```

### iOS Testing (iPhone)

**Start Simulator:**
```bash
xcrun simctl boot FFDD49FC-3414-4A76-B448-2AC5B18AEED5  # iPhone 17
open -a Simulator
```

**Build and Run:**
```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar/src/LunarCalendar.MobileApp
dotnet build -f net8.0-ios -t:Run
```

### iOS Testing (iPad)

**Start iPad Simulator:**
```bash
xcrun simctl boot 80419DFF-CB4A-49EB-AF07-FE8040C893A2  # iPad Air 13-inch
open -a Simulator
```

**Build and Run:**
```bash
dotnet build -f net8.0-ios -t:Run
```

## What You Should See

### 1. Welcome Screen
- Guest/Sign In/Sign Up buttons
- Clean, professional layout

### 2. Calendar View
✅ **Background**: Subtle golden tint on cream background (#FFF9F0 + #FFD700 overlay)

✅ **Header Section**:
- Month/Year display (e.g., "December 2025")
- Previous/Next month buttons
- Today button
- **Today's Lunar Date**: "Today: 25/11/2025" (displayed below header)

✅ **Calendar Grid**:
- 7 columns (Sun-Sat)
- 6 weeks grid
- Gregorian dates (large, bold)
- Lunar dates below (small, red, format: dd/mm)

✅ **Holidays** (when present):
- **Colored borders** around holiday dates:
  - Red = Major holidays (New Year's Day on 1/1, National Day on 9/2)
  - Gold/Yellow = Traditional festivals
  - Green = Seasonal celebrations
- **Holiday names** displayed in small text on calendar cells
- **Light background tint** in holiday color

### 3. Holidays to Look For

**January 2026** (navigate forward):
- 1/1: Tết Dương Lịch (New Year's Day) - RED
- 29/1: Tết Nguyên Đán (Lunar New Year Day 1) - RED
- 30/1: Tết Nguyên Đán Day 2 - RED
- 31/1: Tết Nguyên Đán Day 3 - RED

**April 2026**:
- 30/4: Ngày Giải Phóng Miền Nam (Reunification Day) - RED

**May 2026**:
- 1/5: Ngày Quốc Tế Lao Động (Labor Day) - RED

**September 2026**:
- 2/9: Quốc Khánh (National Day) - RED

## Verification Checklist

### Visual Checks:
- [ ] Background has subtle golden tint (not white)
- [ ] Today's lunar date shows in header
- [ ] Lunar dates show below Gregorian dates (red color)
- [ ] Current date highlighted with primary color
- [ ] Holiday dates have colored borders
- [ ] Holiday names visible on cells (small text)

### Functional Checks:
- [ ] Month navigation works (previous/next buttons)
- [ ] Today button returns to current month
- [ ] Holidays load without errors (check logs)
- [ ] App doesn't crash on startup
- [ ] No black screen on iPhone/iPad

### API Health Check:
```bash
# Check API is running
curl http://localhost:5090/health

# Test holiday endpoint for January 2026
curl http://localhost:5090/api/holiday/month/2026/1 | json_pp
```

## Troubleshooting

### API Not Responding
```bash
# Check if API is running
lsof -i :5090

# If not, start it
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar/src/LunarCalendar.Api
dotnet run
```

### Holidays Not Showing
1. Check app logs for connection errors
2. Verify API is on port 5090
3. Confirm HolidayService uses correct port
4. Check network permissions (NSAppTransportSecurity for iOS)

### Black Screen on iOS
- Fixed with Info.plist updates (NSAppTransportSecurity, UILaunchStoryboardName)
- Rebuild if issue persists

## Current Status

✅ **Android**: Built and deployed successfully
✅ **iPhone**: Built successfully, ready to test
✅ **iPad**: Compatible, ready to test
✅ **API**: Running on port 5090
✅ **Holiday Data**: 19 Vietnamese holidays loaded
✅ **Background**: Golden cultural theme applied

## Next Steps (Post Sprint 5)

- Test event creation (Sprint 6-7 features moved to post-MVP)
- Add more cultural imagery options
- Implement settings to toggle background
- Add holiday detail views on tap
- Test on physical devices
