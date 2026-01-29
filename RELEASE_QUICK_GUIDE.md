# Quick Action Guide: Prepare for App Store Release

## 🚨 CRITICAL ACTIONS (Do These First)

### 1. Update Version Numbers (5 minutes)

Open `src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj` and update:

```xml
<!-- CHANGE FROM: -->
<ApplicationDisplayVersion>1.0.1</ApplicationDisplayVersion>
<ApplicationVersion>5</ApplicationVersion>

<!-- CHANGE TO: -->
<ApplicationDisplayVersion>1.1.0</ApplicationDisplayVersion>
<ApplicationVersion>6</ApplicationVersion>
```

**Why:** 
- 1.1.0 = Minor version bump for new Sexagenary Cycle feature
- Build 6 = Next sequential build number

---

### 2. Create App Store Screenshots (30-60 minutes)

#### Quick Screenshot Method:

**For iOS:**
```bash
# Launch on largest iPhone simulator
xcrun simctl boot "iPhone 16 Pro Max"
bash scripts/deploy-iphone-simulator.sh

# Take screenshots with: Cmd + S in Simulator
# Or use: xcrun simctl io booted screenshot screenshot.png
```

**For Android:**
```bash
# Launch on emulator
~/Library/Android/sdk/emulator/emulator -avd Pixel_6_Pro_API_34

# Take screenshot: Cmd + S in emulator window
```

#### Required Screenshots (5-10 images):
1. **Main Calendar** - Shows Can Chi below dates with colors
2. **Calendar (Zoomed)** - Close-up of Can Chi and element colors  
3. **Holiday View** - Shows holiday list
4. **Settings Page** - Shows new "Show Sexagenary Cycle" toggle
5. **Year Picker** - Shows year selection

#### Screenshot Specifications:
- **iPhone 6.7"**: 1290 x 2796 pixels (iPhone 14 Pro Max, 15 Pro Max)
- **iPad Pro 12.9"**: 2048 x 2732 pixels
- **Android Phone**: 1080 x 1920 minimum

---

### 3. Write "What's New" Text (10 minutes)

Use this template for App Store Connect / Play Console:

```
🎉 New in Version 1.1.0

✨ SEXAGENARY CYCLE (CAN CHI / 干支)
• Traditional Chinese 60-year cycle display
• Shows Heavenly Stems (Thiên Can / 天干)  
• Shows Earthly Branches (Địa Chi / 地支)
• Color-coded Five Elements (Ngũ Hành / 五行):
  🟢 Wood (Mộc) • 🔴 Fire (Hỏa) • 🟤 Earth (Thổ) • ⚪ Metal (Kim) • 🔵 Water (Thủy)
• Multi-language support (Chinese, Vietnamese, English)
• Toggle display in Settings

Connect with ancient Asian timekeeping traditions!

📱 As always: Fully offline, no ads, no tracking.
```

**Vietnamese Version:**
```
🎉 Phiên bản mới 1.1.0

✨ CAN CHI (干支)
• Hiển thị chu kỳ Can Chi truyền thống
• Thiên Can (天干) và Địa Chi (地支)
• Màu sắc theo Ngũ Hành (五行):
  🟢 Mộc • 🔴 Hỏa • 🟤 Thổ • ⚪ Kim • 🔵 Thủy
• Hỗ trợ đa ngôn ngữ
• Bật/tắt trong Cài đặt

Kết nối với truyền thống phương Đông!

📱 Vẫn hoàn toàn offline, không quảng cáo.
```

---

### 4. Update App Description (15 minutes)

Add to your existing App Store description under Features:

```
✨ NEW: SEXAGENARY CYCLE (CAN CHI / 干支)
• Traditional 60-year Chinese zodiac cycle
• Heavenly Stems & Earthly Branches display
• Five Elements color coding (Wood, Fire, Earth, Metal, Water)
• Cultural depth and historical significance
• Toggle on/off in Settings

🌙 LUNAR CALENDAR
• Accurate Vietnamese lunar date conversion
[... rest of existing description ...]
```

---

## ⚡ QUICK BUILD COMMANDS

### Build for iOS App Store:
```bash
# Clean first
rm -rf src/LunarCalendar.MobileApp/bin src/LunarCalendar.MobileApp/obj

# Build Release
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net10.0-ios \
  -c Release \
  -p:RuntimeIdentifier=ios-arm64

# Or use Xcode to create archive
```

### Build for Android Play Store:
```bash
# Build AAB (App Bundle)
dotnet publish src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj \
  -f net10.0-android \
  -c Release \
  -p:AndroidPackageFormat=aab

# Output: bin/Release/net10.0-android/publish/
```

---

## 📝 PRE-SUBMISSION CHECKLIST

```
Phase 1: Code & Build
□ Update version to 1.1.0 (Build 6)
□ Merge feature branch to main
□ Create git tag: git tag -a v1.1.0 -m "Sprint 9 Release"
□ Push tag: git push origin v1.1.0
□ Build iOS Release (0 errors)
□ Build Android Release (0 errors)
□ Test on physical iPhone
□ Test on physical Android device

Phase 2: App Store Assets
□ Create 5-10 screenshots (iOS)
□ Create 5-10 screenshots (Android)
□ Write "What's New" text (English)
□ Write "What's New" text (Vietnamese)
□ Update app description
□ Verify Privacy Manifest exists

Phase 3: Submission
□ Upload to TestFlight
□ Test on TestFlight
□ Upload to Play Console Internal Testing
□ Test internal build
□ Submit for App Store review
□ Submit for Play Store review

Phase 4: Post-Release
□ Monitor reviews (respond within 24h)
□ Check for crash reports
□ Track download numbers
□ Plan Sprint 10
```

---

## 🎯 ESTIMATED TIMELINE

| Task | Time | Status |
|------|------|--------|
| Update version numbers | 5 min | ⏳ TODO |
| Create screenshots | 60 min | ⏳ TODO |
| Write descriptions | 30 min | ⏳ TODO |
| Build & test | 30 min | ⏳ TODO |
| Upload & submit | 60 min | ⏳ TODO |
| **TOTAL** | **~3 hours** | |

Plus:
- Review time: 1-3 days (iOS), few hours (Android)
- TestFlight testing: 1-2 days (optional but recommended)

---

## 🆘 QUICK HELP

### If Build Fails:
```bash
# Clean everything
dotnet clean
rm -rf */bin */obj

# Restore packages
dotnet restore

# Try build again
dotnet build -c Release
```

### If Screenshots Look Wrong:
- Make sure app is displaying Can Chi (check Settings toggle)
- Use light theme for consistency
- Capture different months to show variety
- Zoom in to show element colors clearly

### If TestFlight Upload Fails:
- Verify version number was incremented
- Check that signing certificates are valid
- Ensure proper entitlements in Info.plist
- Try archiving through Xcode instead of CLI

### Need Help?
- See full details in: `PRODUCTION_READINESS_REPORT.md`
- iOS submission guide: https://developer.apple.com/app-store/submissions/
- Android submission guide: https://support.google.com/googleplay/android-developer/

---

## ✅ WHEN READY TO SUBMIT

1. **Final verification:**
   ```bash
   # Run all tests one more time
   dotnet test src/LunarCalendar.Core.Tests/
   
   # Verify builds work
   dotnet build -c Release -f net10.0-ios
   dotnet build -c Release -f net10.0-android
   ```

2. **Commit and tag:**
   ```bash
   git add -A
   git commit -m "release: version 1.1.0 - Sexagenary Cycle feature"
   git tag -a v1.1.0 -m "Sprint 9: Sexagenary Cycle (Can Chi) Release"
   git push origin main
   git push origin v1.1.0
   ```

3. **Build and upload** using Xcode (iOS) and Play Console (Android)

4. **Monitor and respond** to any reviewer questions

---

**You're ready!** 🚀

The app is production-ready. Follow this guide to submit to stores.

**Next Steps After Release:**
- Sprint 10: Date Detail Page + Technical Debt cleanup
- Add crash reporting (AppCenter/Firebase)
- Consider user analytics (privacy-friendly)
- Plan iPad-specific optimizations
