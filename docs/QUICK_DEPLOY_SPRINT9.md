# 🚀 Quick Deployment Guide - Sprint 9 (v1.1.0)
## One-Page Reference for App Store Release

**Version:** 1.1.0 (Build 6) | **Target:** Feb 2026 | **Features:** Sexagenary Cycle

---

## ⚡ 5-Minute Pre-Flight Check

```bash
# 1. Verify tests pass
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar
dotnet test tests/LunarCalendar.Core.Tests/LunarCalendar.Core.Tests.csproj
# Expected: 108 passed ✅

# 2. Check version numbers
grep -A1 "ApplicationDisplayVersion\|ApplicationVersion" src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj
# Expected: 1.1.0 and 6

# 3. Verify certificates
security find-identity -v -p codesigning | grep "Apple Distribution"
ls -lh releases/Lunar_Calendar_App_Store.mobileprovision
# Both should exist ✅

# 4. Build IPA
bash scripts/build-ios-appstore.sh
# Expected: ✅ IPA created in bin/Release/net10.0-ios/ios-arm64/
```

---

## 📦 Build & Submit (30 min)

### 1️⃣ Build (10 min)
```bash
# Clean and build
dotnet clean src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -c Release
bash scripts/build-ios-appstore.sh

# Verify IPA
ls -lh src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/*.ipa
```

### 2️⃣ Upload (10 min)
- Open **Transporter** app
- Drag IPA file
- Click **Deliver**
- Wait for "Package uploaded successfully"

### 3️⃣ Configure (10 min)
**App Store Connect:** appstoreconnect.apple.com

1. My Apps → Vietnamese Lunar Calendar
2. **+ Version** → **1.1.0** → Create
3. Select Build: **1.1.0 (6)** (wait if "Processing")
4. **What's New:** Copy text below
5. Upload **6 screenshots** (1290x2796px)
6. **Submit for Review**

---

## 📝 Release Notes (Copy-Paste)

### English
```
🎉 New in Version 1.1.0

✨ SEXAGENARY CYCLE (CAN CHI / 干支)
• Traditional Chinese 60-year cycle display
• Shows Heavenly Stems (Thiên Can / 天干)
• Shows Earthly Branches (Địa Chi / 地支)
• Color-coded Five Elements (Ngũ Hành / 五行):
  🟢 Wood (Mộc) • 🔴 Fire (Hỏa) • 🟤 Earth (Thổ)
  ⚪ Metal (Kim) • 🔵 Water (Thủy)
• Multi-language support (Chinese, Vietnamese, English)
• Toggle display in Settings

Connect with ancient Asian timekeeping traditions!

📱 As always: Fully offline, no ads, no tracking.
```

### Vietnamese
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

## 📸 Screenshots (Required)

**Capture on iPhone 16 Pro Max (1290x2796px):**
```bash
xcrun simctl boot "iPhone 16 Pro Max"
bash scripts/deploy-iphone-simulator.sh
# Cmd+S in Simulator to take screenshots
```

**Required shots:**
1. ✅ Main calendar with Can Chi visible
2. ✅ Calendar zoomed (show colors)
3. ✅ Settings page (toggle visible)
4. ✅ Holiday list
5. ✅ Year picker
6. ✅ Dark mode calendar

---

## 🎯 App Review Notes (Copy-Paste)

```
Hello Apple Review Team,

This is version 1.1.0 of Vietnamese Lunar Calendar, adding Sexagenary Cycle (Can Chi / 干支) display.

KEY POINTS:
• Traditional Asian calendar feature (not fortune telling)
• 100% offline - no network, ads, or tracking
• Tested on iOS 15.0 - 18.x

TEST INSTRUCTIONS:
1. Open app → Calendar displays
2. Settings → Enable "Show Sexagenary Cycle"
3. Return to calendar → See Can Chi below dates with colors
4. Switch languages: EN, VI, ZH

Contact: [Your email]
```

---

## ⏱️ Timeline

| When | What | Duration |
|------|------|----------|
| Now | Build & Submit | 30 min |
| +10 min | App processes | Auto |
| +1-3 days | Apple reviews | Wait |
| +4 days | Release to store | Manual |
| +7 days | 100% rollout | Auto |

---

## 🚨 Emergency Contacts

**Critical issue:** Pause rollout in App Store Connect  
**Hotfix needed:** Increment to 1.1.1 (Build 7) and request expedited review  
**Documentation:** See `docs/APP_STORE_DEPLOYMENT_RUNBOOK.md`

---

## ✅ Success Criteria

- [ ] Build: 0 errors, 0 warnings
- [ ] Tests: 108/108 passing
- [ ] Upload: No errors
- [ ] Review: Approved in 1-3 days
- [ ] Crash rate: <0.1%
- [ ] Rating: >4.0 stars

---

## 🔗 Quick Links

- **App Store Connect:** https://appstoreconnect.apple.com
- **Apple Developer:** https://developer.apple.com/account
- **Full Runbook:** `docs/APP_STORE_DEPLOYMENT_RUNBOOK.md`
- **Checklist:** `docs/SPRINT9_DEPLOYMENT_CHECKLIST.md`

---

**🚀 You got this! Good luck with the release!**
