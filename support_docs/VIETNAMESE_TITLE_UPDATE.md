# Vietnamese Title Update - December 29, 2025

## Change Summary

Updated the Vietnamese app title/header from **"Lịch"** to **"Lịch Vạn Niên"** for a more descriptive and traditional name.

## What Was Changed

### File Modified
- `src/LunarCalendar.MobileApp/Resources/Strings/AppResources.vi.resx`

### Before
```xml
<data name="Calendar" xml:space="preserve">
  <value>Lịch</value>
</data>
```

### After
```xml
<data name="Calendar" xml:space="preserve">
  <value>Lịch Vạn Niên</value>
</data>
```

## Where This Appears

The title "Lịch Vạn Niên" will now appear in:
1. **App Header/Title** - Top of the Calendar page
2. **Navigation Menu** - Menu item for Calendar
3. **Tab Bar** - If using tabbed navigation

## Language Context

- **"Lịch"** = Calendar (generic)
- **"Lịch Vạn Niên"** = Perpetual Calendar / Ten Thousand Year Calendar (traditional Vietnamese term)

The term "Lịch Vạn Niên" is the traditional Vietnamese name for perpetual calendars that show both solar and lunar dates, making it more appropriate for this app's purpose.

## Deployment Status

### ✅ iOS Devices
- **iPhone 16 Pro**: Updated and Running (Process ID: 33345)
- **iPad Pro 13-inch**: Updated and Running (Process ID: 33675)

### ✅ Android Devices
- **Android Emulator (emulator-5554)**: Updated and Running

### Build Times
- iOS: 1.18 seconds
- Android: 1.39 seconds

## Testing Instructions

1. **Switch Language to Vietnamese**:
   - Go to Settings
   - Change Language to "Tiếng Việt"
   - Return to main menu

2. **Verify New Title**:
   - Check the app header shows "Lịch Vạn Niên"
   - Check the menu shows "Lịch Vạn Niên"
   - Verify it displays correctly on both iPhone and iPad

3. **Verify English Unchanged**:
   - Switch language to English
   - Verify it still shows "Calendar"

## Related Changes

This change is in addition to:
- ✅ Upcoming Holidays Range Fix (completed earlier)
- ✅ Multi-platform deployment (Android, iOS, MacCatalyst)

## Notes

- The English title remains "Calendar" (unchanged)
- Other Vietnamese translations remain unchanged
- No code changes required - only resource file updated
- Change is effective immediately upon app restart or language switch

---

**Status**: ✅ Complete - All platforms deployed with new Vietnamese title
