# 🔍 Verify Sprint 5 Changes - Quick Guide

## Apps Have Been Restarted! ✅

Both Android and iOS apps have been **completely rebuilt** with all fixes applied.

---

## What to Check Now

### 1️⃣ **Background** (Should Work Now)
Look at the calendar page - you should see:
- **Subtle cream/golden background** instead of plain white
- Background color: Warm cream (#FFF9F0) with gold tint
- This replaces the dragon SVG that wasn't loading

**How to verify:** The entire calendar should have a soft, warm golden glow instead of stark white.

---

### 2️⃣ **Today's Lunar Date** (Already Working)
At the top of the calendar (below the month navigation):
- You should see: **"Today: 25/11/2025"** or similar
- This shows the current Gregorian date converted to lunar calendar

**How to verify:** Look right below the "December 2025" header.

---

### 3️⃣ **Holidays with Colored Borders** (Should Work Now)
Navigate to months with Vietnamese holidays to see them:

#### **January 2026** (Most holidays!)
Swipe right or click "▶" to go to January 2026. You'll see:
- **1/1**: New Year's Day - **RED** border
- **29/1-31/1**: Tết Nguyên Đán (Days 1-3) - **RED** borders
- Small holiday names displayed on these dates

#### **April 2026**
- **30/4**: Reunification Day - **RED** border

#### **May 2026**
- **1/5**: Labor Day - **RED** border

#### **September 2026**
- **2/9**: National Day - **RED** border

**How to verify:**
- Look for **colored borders** (thick, 2px) around these dates
- The border color should be **red (#DC143C)** for major holidays
- Holiday names should appear in small text on the cell

---

### 4️⃣ **Lunar Dates** (Already Working)
Every day should show:
- **Large number** = Gregorian day (top)
- **Small red number** = Lunar day/month in dd/mm format (bottom)

**How to verify:** Look at any date - below the main number, you should see a small red "dd/mm".

---

## 📱 Testing Steps

### On Android Emulator:
The app is **already running**. Just:
1. Look at the background color (should be golden/cream, not white)
2. Swipe right to navigate to **January 2026**
3. Look for **red borders** on dates 1, 29, 30, 31
4. Check if holiday names appear on those dates

### On iOS Simulator:
The app is **building now**. Once it launches:
1. Same steps as Android above
2. Check that the screen isn't black (should show the calendar)

---

## 🐛 If Issues Persist

### Holidays Still Not Showing?
```bash
# Check API is responding
curl http://localhost:5090/api/holiday/month/2026/1

# You should see JSON with ~4-5 holidays for January
```

### Background Still White?
```bash
# The background should be #FFF9F0 (cream) with gold overlay
# If still white, try:
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar/src/LunarCalendar.MobileApp
dotnet clean
dotnet build -f net8.0-android
~/Library/Android/sdk/platform-tools/adb uninstall com.companyname.lunarcalendar.mobileapp
~/Library/Android/sdk/platform-tools/adb install -r bin/Debug/net8.0-android/com.companyname.lunarcalendar.mobileapp-Signed.apk
```

### App Logs (Android):
```bash
# Monitor in real-time
~/Library/Android/sdk/platform-tools/adb logcat | grep -i "lunarcalendar\|holiday"
```

---

## ✅ Success Checklist

- [ ] Background has **golden/cream tint** (not white)
- [ ] Today's lunar date shows in header
- [ ] January 2026 shows holidays with **red borders** on 1st, 29th, 30th, 31st
- [ ] Holiday names visible on calendar cells
- [ ] Lunar dates (dd/mm) show below Gregorian dates in red
- [ ] No black screen on iPhone/iPad
- [ ] No connection errors in logs

---

## 🎯 Quick Navigation Tips

**To see holidays quickly:**
1. Tap the **"▶"** button (next month) repeatedly until you reach **January 2026**
2. Or tap **"Today"** button, then swipe right to next month

**Most visible holidays are in January 2026!**

---

## Current Status

✅ **Android**: Rebuilt and deployed
✅ **iOS**: Rebuilding now
✅ **API**: Running on port 5090
✅ **Fixes Applied**: Port corrected (5079→5090), Background updated (SVG→BoxView)

The apps should now show **all Sprint 5 features** correctly! 🎉
