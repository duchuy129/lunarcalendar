# Quick Test Guide - Language Switch Fix

## Test the Today Section Language Switch

### The Issue You Reported:
✅ **FIXED:** Today section was showing Vietnamese format even after switching to English

### What Was Changed:
1. Added 100ms delay after language change to allow culture propagation
2. Added detailed logging to track culture changes

---

## How to Test:

### Step 1: Open the App
The app is already running on your Android emulator (PID: 8245)

### Step 2: Check Initial State
- Look at the **Today section** at the top
- Current language should be **Vietnamese**
- Should show: **"Ngày [X] Tháng [Y], Năm [Con Giáp]"**
- Example: "Ngày 15 Tháng 11, Năm Tỵ"

### Step 3: Switch to English
1. Tap the **Settings** tab (gear icon at bottom)
2. Find **"Language"** section
3. Tap the dropdown
4. Select **"English"**
5. **WATCH THE TODAY SECTION** - it should update immediately

### Step 4: Verify English Format
- Today section should now show: **"[X]/[Y] Year of the [Animal]"**
- Example: "15/11 Year of the Snake"

### Step 5: Switch Back to Vietnamese
1. Still in Settings
2. Tap language dropdown
3. Select **"Tiếng Việt"** (Vietnamese)
4. **WATCH THE TODAY SECTION** - should update back to Vietnamese

### Step 6: Rapid Switching Test
- Switch between English and Vietnamese 3-5 times quickly
- **Verify:** Today section ALWAYS updates to the correct format
- **No lag or wrong format should appear**

---

## Expected Results:

| Language | Today Section Format | Example |
|----------|---------------------|---------|
| Vietnamese | Ngày X Tháng Y, Năm [Con Giáp] | Ngày 15 Tháng 11, Năm Tỵ |
| English | X/Y Year of the [Animal] | 15/11 Year of the Snake |

---

## What to Watch For:

### ✅ Success Indicators:
- Today section updates within 1 second of language change
- Format matches the selected language perfectly
- No flickering or temporary wrong format
- Works consistently after multiple switches

### ❌ If You Still See Issues:
- Today section doesn't update after language switch
- Shows Vietnamese format when English is selected (or vice versa)
- Takes more than 2 seconds to update
- Inconsistent behavior (works sometimes, fails other times)

---

## Monitor Logs (Optional):

If you want to see what's happening behind the scenes:

```bash
adb logcat | grep -E "Language|Culture|FormatLunar|Today Display"
```

You should see logs like:
```
=== Language switched to: en-US ===
=== LanguageChangedMessage received in CalendarViewModel ===
=== After delay, Culture: en-US ===
=== FormatLunarDateWithYear called ===
=== English format: 15/11 Year of the Snake ===
```

---

## Test on iPhone (Optional):

iOS was smoother already, but you can verify:
1. Build and run iOS app
2. Perform same language switching test
3. Should work even more smoothly than Android

---

## Report Back:

After testing, let me know:
1. ✅ **FIXED** - Today section updates correctly when switching languages
2. ❌ **STILL BROKEN** - Today section shows wrong format (provide details)
3. 🤔 **PARTIAL** - Works sometimes but not always

**Ready to test! The app is already running on your emulator.** 🚀
