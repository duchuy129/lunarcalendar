# Android App Bundle (AAB) - Ready for Google Play

**Date:** January 6, 2026
**Status:** ✅ Production-Ready

---

## 📦 Your AAB File

**Location:**
```
src/LunarCalendar.MobileApp/bin/Release/net10.0-android/com.huynguyen.lunarcalendar-Signed.aab
```

**Details:**
- **Size:** 36 MB
- **Version Name:** 1.0.1
- **Version Code:** 3 (NEW - resolves "version code 2 already used" error)
- **Package:** com.huynguyen.lunarcalendar
- **Built:** January 6, 2026 13:31
- **Signed:** Yes, with release keystore
- **Code:** Latest (includes localization fix from Jan 5)

---

## ✅ What's Included

This AAB includes all commits up to `516ed02`:

- ✅ **Localization fix** (Jan 5, 2026) - Your yesterday's fix
- ✅ **Files restructure** (Jan 5, 2026)
- ✅ **All stability fixes** (Jan 4, 2026)
- ✅ **All UI/UX enhancements** (Jan 3-4, 2026)

---

## 📤 Upload to Google Play Console

### Option 1: Internal Testing (Current Path)

1. Go to: https://play.google.com/console
2. Select your app: **Lunar Calendar**
3. Navigate to: **Testing → Internal testing**
4. Click **"Create new release"**
5. Upload this AAB file
6. Add release notes (see below)
7. Click **"Review release"** → **"Start rollout to Internal testing"**

### Option 2: Direct to Production (Recommended - Skip Testing)

1. Go to: https://play.google.com/console
2. Select your app: **Lunar Calendar**
3. Navigate to: **Release → Production**
4. Click **"Create new release"**
5. Upload this AAB file
6. Add release notes (see below)
7. Click **"Review release"** → **"Start rollout to Production"**

---

## 📝 Release Notes (Copy & Paste)

```
🎉 Initial Release

Features:
• Complete Vietnamese lunar calendar
• Dual date display (Gregorian + Lunar)
• 45+ traditional holidays and festivals
• Bilingual interface (Vietnamese/English)
• 100% offline functionality
• Zero data collection

Chúc mừng năm mới! 🎊
```

---

## 🔐 Keystore Information

Your release keystore is securely stored at:
```
~/.android/keystore/lunarcalendar-release.keystore
```

**Backup location:**
```
~/Desktop/LunarCalendar-Keystore-Backup-*
```

**⚠️ CRITICAL:** Never lose this keystore! Without it, you cannot update your app.

---

## 📋 Version History

| Version Code | Version Name | Date | Status |
|--------------|--------------|------|--------|
| 1 | 1.0.0 | - | Not used |
| 2 | 1.0.1 | Jan 6, 2026 | Uploaded to Internal Testing |
| **3** | **1.0.1** | **Jan 6, 2026** | **Current - Ready to Upload** |

**Why Version 3?**
Version code 2 was already uploaded to Google Play Console (internal testing). Each upload requires a unique, incrementing version code.

---

## 🚀 Next Steps

1. **Upload AAB** to Google Play Console (Internal Testing or Production)
2. **Complete store listing** if not done:
   - App description
   - Screenshots (from simulator)
   - Feature graphic (1024x500)
   - Privacy policy URL
3. **Submit for review**
4. **Wait 2-7 days** for Google Play review

**Guide:** See [GOOGLE_PLAY_DIRECT_PRODUCTION.md](GOOGLE_PLAY_DIRECT_PRODUCTION.md) for complete step-by-step instructions.

---

## 📱 Testing Before Upload (Optional)

You can test this exact AAB on your device or emulator:

```bash
# Install on connected device/emulator
~/Library/Android/sdk/platform-tools/adb install src/LunarCalendar.MobileApp/bin/Release/net10.0-android/com.huynguyen.lunarcalendar-Signed.aab
```

**Note:** AAB files work on devices but are optimized for Google Play's app bundle delivery system.

---

## ✅ Checklist Before Upload

- [x] AAB built with latest code
- [x] Version code incremented (3)
- [x] Signed with release keystore
- [x] Keystore backed up
- [x] Release notes prepared
- [ ] Screenshots ready (from simulator)
- [ ] Feature graphic created (1024x500)
- [ ] Privacy policy URL available
- [ ] Store listing completed

---

## 🔄 If You Need to Rebuild

To rebuild with a new version code in the future:

1. Edit `src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj`
2. Update line 32: `<ApplicationVersion>4</ApplicationVersion>` (increment by 1)
3. Run: `./build-android-release.sh`

---

## 📞 Support

**Google Play Console:**
- https://play.google.com/console

**Issue Tracking:**
- https://github.com/duchuy129/lunarcalendar/issues

---

**Last Updated:** January 6, 2026
**Ready to upload!** 🎉
