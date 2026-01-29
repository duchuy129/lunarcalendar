# Version History - Vietnamese Lunar Calendar

## Release Timeline

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

### v1.2.0 (Build 7) - Sprint 10 [PLANNED]
**Estimated:** February 2026  
**Status:** 📋 Planning

#### 🎯 Planned Features
- **Date Detail Page** - Comprehensive date information view
  - Full sexagenary information (year, month, day, hour)
  - Element associations and meanings
  - Holiday details
  - Alternative navigation pattern
- **Technical Debt Cleanup**
  - Address 100+ compiler warnings
  - Update obsolete API usage
  - Code optimization
- **Crash Reporting**
  - AppCenter or Firebase Crashlytics integration
  - Production error monitoring

---

### v1.3.0 (Build 8) - Sprint 11 [PLANNED]
**Estimated:** March 2026  
**Status:** 💭 Concept

#### 🎯 Potential Features
- Accessibility improvements (VoiceOver, TalkBack)
- Performance optimizations for older devices
- iPad-specific layout enhancements
- Widget support (iOS home screen)

---

### v2.0.0 - Phase 3 [FUTURE]
**Estimated:** 2027  
**Status:** 🔮 Vision

#### 🎯 Major Update
- Complete UI redesign
- Advanced features
- Platform-specific optimizations
- Potential monetization features

---

## Version Mapping

| Version | Build | Sprint | Phase | Date | Key Feature |
|---------|-------|--------|-------|------|-------------|
| 1.0.0 | 1 | 1-8 | Phase 1 | Dec 2025 | MVP Launch |
| 1.0.1 | 2-5 | - | Phase 1 | Jan 2026 | Bug fixes |
| **1.1.0** | **6** | **9** | **Phase 2** | **Jan 2026** | **Sexagenary Cycle** |
| 1.2.0 | 7 | 10 | Phase 2 | Feb 2026 | Date Detail + Cleanup |
| 1.3.0 | 8 | 11 | Phase 2 | Mar 2026 | Accessibility |
| 1.x.x | X | 12+ | Phase 2 | 2026 | Feature releases |
| 2.0.0 | 20+ | 20+ | Phase 3 | 2027 | Major redesign |

---

## Release Notes Template

### For App Store / Play Console

#### English:
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

#### Vietnamese:
```
🎉 Phiên bản mới 1.1.0

✨ CAN CHI (干支)
• Hiển thị chu kỳ Can Chi truyền thống 60 năm
• Thiên Can (天干) và Địa Chi (地支)
• Màu sắc theo Ngũ Hành (五行):
  🟢 Mộc • 🔴 Hỏa • 🟤 Thổ • ⚪ Kim • 🔵 Thủy
• Hỗ trợ đa ngôn ngữ (Hán, Việt, Anh)
• Bật/tắt hiển thị trong Cài đặt

Kết nối với truyền thống đếm thời gian phương Đông!

📱 Vẫn hoàn toàn offline, không quảng cáo, không theo dõi.
```

---

## Git Tags

```bash
# Production releases
v1.0.0 - MVP Launch (Dec 2025)
v1.0.1 - Bug fixes (Jan 2026)
v1.1.0 - Sprint 9: Sexagenary Cycle (Jan 2026) ← CURRENT

# Planned
v1.2.0 - Sprint 10: Date Detail Page (Feb 2026)
v1.3.0 - Sprint 11: Accessibility (Mar 2026)
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

**Last Updated:** January 29, 2026  
**Current Production Version:** 1.1.0 (Build 6)  
**Next Planned Release:** 1.2.0 (Sprint 10)
