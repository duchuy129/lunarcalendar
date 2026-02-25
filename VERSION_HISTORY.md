# Version History - Vietnamese Lunar Calendar

## Release Timeline

### v1.2.0 (Build 7) - Sprints 10 & 11: Zodiac Animals & Compatibility
**Release Date:** February 24, 2026
**Status:** 🚀 Ready for Production
**Branch:** feature/003-zodiac-tab

#### 🎉 New Features
- **Dedicated Zodiac Tab** — New 🔯 tab in bottom navigation bar
  - My Zodiac Profile card — enter birth year, see your elemental animal instantly
  - Browse Animals shortcut — navigate to full 12-animal reference
  - Check Compatibility shortcut — jump straight to compatibility checker
- **12 Zodiac Animals Reference** (`ZodiacInformationPage`)
  - Full detail for each of the 12 animals: personality, lucky numbers, colors, directions
  - Recent birth years list per animal
  - Lucky Directions section (new — was missing previously)
  - All sections fully bilingual EN ↔ VI
- **Zodiac Compatibility Engine** (`ZodiacCompatibilityPage`)
  - Select any two animals and get a compatibility score + rating
  - Ratings: Excellent / Good / Fair / Challenging
  - Full localization — picker and results update on language change
- **Elemental Animal in Calendar** — Today card shows your elemental zodiac animal
- **Full Localization Refresh** — All zodiac strings (colors, directions, labels) update
  instantly when switching EN ↔ VI with no restart required

#### 🔧 Technical
- 344 unit tests passing (100%)
- Zero compilation errors (iOS & Android)
- `MtouchLink=None` for iOS Release — prevents trimmer stripping DI/reflection types
- `LinkerConfig.xml` expanded to preserve entire app + Core assemblies
- Deploy script fixed: uses `Debug` config for device testing (correct Development profile)
- Platforms: iOS 15.0+, Android 8.0+ (API 26+)

#### 🐛 Bug Fixes
- Fixed `abort()` crash on physical iOS device at startup (linker stripping DI types)
- Fixed deploy script using Release config with Development provisioning profile
- Fixed hardcoded "Recent Years" label not updating on language change
- Fixed lucky colors/directions showing raw JSON values (`"blue"`, `"southeast"`) instead of
  translated strings
- Fixed `ZodiacCompatibilityViewModel` picker not rebuilding on language switch
- Fixed `ZodiacInformationViewModel.Title` hardcoded as "Zodiac Animals"

#### 📊 Quality Metrics
- **Build Status:** ✅ iOS (0 errors), ✅ Android (0 errors)
- **Test Coverage:** ✅ 344/344 passing
- **Device Testing:** ✅ iPhone 14 Pro Max (iOS 26.3), Android Emulator (API 36)
- **Compatibility:** Backward compatible with v1.1.x

---

### v1.1.0 (Build 6) - Sprint 9: Sexagenary Cycle
**Release Date:** January 29, 2026  
**Status:** 🚀 Ready for Production  
**Branch:** feature/001-sexagenary-cycle-complete

#### 🎉 New Features
- **Sexagenary Cycle (Can Chi / 干支)** - Traditional Chinese 60-year cycle display
  - Heavenly Stems (Thiên Can / 天干) calculation
  - Earthly Branches (Địa Chi / 地支) calculation
  - Year, Month, Day, and Hour cycle support
- **Five Elements (Ngũ Hành / 五行)** - Color-coded element indicators
  - 🟢 Wood (Mộc) - Forest Green
  - 🔴 Fire (Hỏa) - Crimson
  - 🟤 Earth (Thổ) - Saddle Brown
  - ⚪ Metal (Kim) - Silver
  - 🔵 Water (Thủy) - Deep Sky Blue
- **Settings Toggle** - Show/hide Can Chi display option
- **Multi-language Support** - Chinese, Pinyin, Vietnamese, English

#### 🔧 Technical
- 108 unit tests passing (100%)
- 36 historical validation tests (100% accuracy)
- Zero compilation errors (iOS & Android)
- Code coverage: 97% for new code
- Platforms: iOS 15.0+, Android 8.0+ (API 26+)

#### 📊 Quality Metrics
- **Build Status:** ✅ iOS (0 errors), ✅ Android (0 errors)
- **Test Coverage:** ✅ 108/108 passing
- **Historical Accuracy:** ✅ 100% validated
- **Performance:** No measurable impact on load time
- **Compatibility:** Backward compatible with v1.0.x

#### ⏭️ Deferred to Next Release
- Date Detail Page (due to MAUI framework limitations - planned for v1.2.0)

---

### v1.0.1 (Build 5) - Stability & Bug Fixes
**Release Date:** January 2026  
**Status:** ✅ In Production

#### 🐛 Bug Fixes
- Fixed crash on iOS when navigating rapidly
- Improved holiday data loading performance
- Fixed lunar date calculation edge cases
- Resolved memory leak in settings page

#### 🔧 Improvements
- Enhanced error handling and logging
- Optimized database queries
- Improved app startup time
- Better offline functionality

---

### v1.0.0 (Build 1) - MVP Launch
**Release Date:** December 2025  
**Status:** ✅ Production (superseded)

#### ✨ Initial Features
- Lunar calendar conversion (Gregorian ↔ Vietnamese Lunar)
- Comprehensive Vietnamese holiday database
- Month-by-month calendar view
- Year-level holiday overview
- Quick year picker (1900-2100)
- Bilingual support (English & Vietnamese)
- Offline-first architecture
- Modern UI with haptic feedback

---

## Upcoming Releases

### v1.3.0 (Build 8) - Sprint 12 [PLANNED]
**Estimated:** March 2026
**Status:** 📋 Planning

#### 🎯 Planned Features
- **Accessibility improvements** (VoiceOver / TalkBack)
- **Performance optimizations** for older devices
- **iPad-specific layout** enhancements
- **Technical Debt Cleanup**
  - Address compiler warnings
  - Update obsolete API usage

---

### v2.0.0 - Phase 3 [FUTURE]
**Estimated:** 2027
**Status:** � Vision

#### 🎯 Major Update
- Complete UI redesign
- Widget support (iOS home screen / Android)
- Advanced features
- Platform-specific optimizations
- Potential monetization features

---

## Version Mapping

| Version | Build | Sprint | Phase | Date | Key Feature |
|---------|-------|--------|-------|------|-------------|
| 1.0.0 | 1 | 1-8 | Phase 1 | Dec 2025 | MVP Launch |
| 1.0.1 | 2-5 | - | Phase 1 | Jan 2026 | Bug fixes |
| 1.1.0 | 6 | 9 | Phase 2 | Jan 2026 | Sexagenary Cycle |
| **1.2.0** | **7** | **10-11** | **Phase 2** | **Feb 2026** | **Zodiac Animals & Compatibility** |
| 1.3.0 | 8 | 12 | Phase 2 | Mar 2026 | Accessibility |
| 1.x.x | X | 13+ | Phase 2 | 2026 | Feature releases |
| 2.0.0 | 20+ | 20+ | Phase 3 | 2027 | Major redesign |

---

## Release Notes Template

### For App Store / Play Console

#### English:
```
🎉 New in Version 1.2.0

🔯 DEDICATED ZODIAC TAB
• New tab in the bottom navigation bar
• See your personal zodiac animal instantly from your birth year
• Browse all 12 zodiac animals with full details
• Check compatibility between any two animals

� 12 ZODIAC ANIMALS REFERENCE
• Personality traits, lucky numbers, colors & directions
• Recent birth years for each animal
• Bilingual: English & Vietnamese

🔮 ZODIAC COMPATIBILITY
• Pick any two animals and get an instant compatibility score
• Ratings: Excellent, Good, Fair, Challenging

🐛 BUG FIXES
• Fixed crash on launch for physical iOS devices
• Fixed lucky colors & directions showing in English only

📱 Fully offline, no ads, no tracking.
```

#### Vietnamese:
```
🎉 Phiên bản mới 1.2.0

🔯 TAB CON GIÁP RIÊNG BIỆT
• Tab mới trong thanh điều hướng bên dưới
• Xem con giáp của bạn ngay từ năm sinh
• Duyệt 12 con giáp với đầy đủ thông tin
• Kiểm tra tương hợp giữa hai con giáp bất kỳ

🐉 12 CON GIÁP
• Tính cách, số may mắn, màu sắc & hướng may mắn
• Các năm sinh gần đây theo từng con giáp
• Song ngữ: Tiếng Anh & Tiếng Việt

� TƯƠNG HỢP CON GIÁP
• Chọn hai con giáp và nhận điểm tương hợp ngay lập tức
• Đánh giá: Xuất sắc, Tốt, Trung bình, Khó hòa hợp

🐛 SỬA LỖI
• Sửa lỗi crash khi khởi động trên thiết bị iOS thực
• Sửa lỗi màu sắc & hướng may mắn chỉ hiển thị bằng tiếng Anh

📱 Hoàn toàn offline, không quảng cáo, không theo dõi.
```

---

## Git Tags

```bash
# Production releases
v1.0.0 - MVP Launch (Dec 2025)
v1.0.1 - Bug fixes (Jan 2026)
v1.1.0 - Sprint 9: Sexagenary Cycle (Jan 2026)
v1.2.0 - Sprints 10-11: Zodiac Animals & Compatibility (Feb 2026) ← CURRENT

# Planned
v1.3.0 - Sprint 12: Accessibility (Mar 2026)
```

---

## Support & Compatibility

### Minimum Requirements
- **iOS:** 15.0+ (iPhone, iPad)
- **Android:** 8.0+ (API Level 26+)
- **Storage:** ~20 MB
- **Internet:** Not required (fully offline)

### Supported Languages
- English (en)
- Vietnamese (vi)
- Chinese characters (zh) - Display only

### Testing Coverage
- **Unit Tests:** 108 tests
- **Historical Validation:** 36 test cases
- **Manual Testing:** iOS Simulator, Android Emulator
- **Device Testing:** iPhone 8+, Android devices

---

**Last Updated:** February 24, 2026
**Current Production Version:** 1.2.0 (Build 7)
**Next Planned Release:** 1.3.0 (Sprint 12)
