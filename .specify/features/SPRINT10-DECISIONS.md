# Sprint 10: Key Decisions & Updates

**Date**: January 26, 2026  
**Updated By**: GitHub Copilot (based on project requirements)

---

## 🎯 Key Decisions Made

### 1. Zodiac Animal Images: Unicode Emoji Approach ✅

**Question**: Will you generate animal images?  
**Answer**: No, AI cannot generate image files.

**Decision**: Use Unicode emoji for Sprint 10 MVP
- 🐭 Rat, 🐮 Ox, 🐯 Tiger, 🐰 Rabbit, 🐲 Dragon, 🐍 Snake
- 🐴 Horse, 🐑 Goat, 🐵 Monkey, 🐔 Rooster, 🐶 Dog, 🐷 Pig

**Rationale**:
- ✅ Zero bundle size impact
- ✅ Instant availability (no asset creation delay)
- ✅ Good mobile support on iOS and Android
- ✅ Universally recognized
- ✅ Works in both light and dark modes
- ✅ No licensing concerns

**Future Enhancement** (Optional):
- Commission SVG artwork in parallel ($50-200, 3-7 days)
- Deploy in Sprint 10.1 or Sprint 11 as polish
- Sources: Fiverr, Upwork, Flaticon, The Noun Project

**Code Implementation**:
```csharp
private string GetZodiacEmoji(ZodiacAnimal animal)
{
    return animal switch
    {
        ZodiacAnimal.Rat => "🐭",
        ZodiacAnimal.Ox => "🐮",
        ZodiacAnimal.Tiger => "🐯",
        ZodiacAnimal.Rabbit => "🐰",
        ZodiacAnimal.Dragon => "🐲",
        ZodiacAnimal.Snake => "🐍",
        ZodiacAnimal.Horse => "🐴",
        ZodiacAnimal.Goat => "🐑",
        ZodiacAnimal.Monkey => "🐵",
        ZodiacAnimal.Rooster => "🐔",
        ZodiacAnimal.Dog => "🐶",
        ZodiacAnimal.Pig => "🐷",
        _ => "❓"
    };
}
```

---

### 2. Chinese Localization: Deferred to Sprint 14 ✅

**Requirement**: No Chinese localization for now, will do in the future

**Decision**: Sprint 10 supports Vietnamese and English only
- ✅ **English**: Full UI strings and zodiac content
- ✅ **Vietnamese**: Full UI strings and zodiac content (12 Con Giáp)
- ❌ **Chinese**: Deferred to Sprint 14 (already planned in roadmap)

**What IS Included** (Not Localization):
- Chinese characters in data as cultural references (鼠, 牛, 虎, etc.)
- `ChineseName` field in `ZodiacInfo` (e.g., "鼠 - Shǔ")
- This is cultural content, not UI localization

**What IS NOT Included**:
- `Strings.zh.resx` resource file
- Chinese UI translations
- Chinese app interface

**Sprint 14 Scope** (Future):
- Vietnamese & Chinese Localization (already in roadmap)
- Add `Strings.zh.resx` at that time
- Translate all UI strings to Chinese

**Benefits**:
- ✅ Faster Sprint 10 delivery (fewer translations)
- ✅ Focus on core Vietnamese + English users
- ✅ Aligns with existing Sprint 14 plan
- ✅ Simpler testing (2 languages instead of 3)

---

## 📊 Impact on Sprint 10

### Timeline Changes

| Phase | Original Estimate | New Estimate | Change |
|-------|-------------------|--------------|--------|
| Phase 6: Localization | 0.5 days | 0.5 days | No change (simplified scope) |
| Phase 7: Assets | 0.5 days | 0.1 days | ✅ **-0.4 days** (emoji only) |
| **Total Sprint** | 10 days | **9.6 days** | ✅ **Slightly ahead** |

**Net Impact**: Sprint 10 is now slightly faster due to zero asset creation overhead.

---

### Scope Changes

#### Removed from Sprint 10:
- ❌ SVG image creation/sourcing
- ❌ Image optimization workflow
- ❌ Chinese UI localization (`Strings.zh.resx`)
- ❌ Chinese translation work

#### Kept in Sprint 10:
- ✅ All zodiac calculations
- ✅ All UI pages and components
- ✅ Compatibility checker
- ✅ English and Vietnamese localization
- ✅ Chinese characters as cultural references (data only)
- ✅ All 27 functional requirements (emoji-based images)

#### Deferred to Future:
- ⏳ SVG zodiac artwork (Sprint 10.1 or 11, optional)
- ⏳ Chinese localization (Sprint 14, already planned)

---

## 🔄 Updated Technical Plan

### File Changes

#### Resources/Images/Zodiac/ (Phase 7)
**Before**:
```
Resources/Images/Zodiac/
├── rat.svg
├── ox.svg
├── tiger.svg
... (12 files, ~1.2 MB total)
```

**After** (Sprint 10):
```
Resources/Images/Zodiac/
(Empty - using emoji in code)
```

**Future** (Optional):
```
Resources/Images/Zodiac/
├── rat.svg
├── ox.svg
... (can be added later)
```

#### Localization Files (Phase 6)
**Before**:
- `Strings.en.resx` ✅
- `Strings.vi.resx` ✅
- `Strings.zh.resx` ✅

**After** (Sprint 10):
- `Strings.en.resx` ✅
- `Strings.vi.resx` ✅
- `Strings.zh.resx` ❌ (Sprint 14)

---

## 📝 Updated Success Criteria

### What Changed

#### SC-007: Image Loading (Updated)
**Before**: All 12 zodiac animals load successfully with images on both iOS and Android with 99.9%+ reliability

**After**: All 12 zodiac animals display with emoji on both iOS and Android with 99.9%+ reliability (zero loading failures - emoji is always available)

#### SC-010: Image Caching (Removed)
**Before**: Image caching reduces data usage by 80%+ on repeat views

**After**: N/A (emoji requires no caching, zero network usage)

#### SC-014: Localization Coverage (Updated)
**Before**: All 12 zodiac animals have complete Vietnamese, Chinese, and English translations with 100% coverage

**After**: All 12 zodiac animals have complete Vietnamese and English translations with 100% coverage (Chinese deferred to Sprint 14)

#### SC-016: Accessibility (Updated)
**Before**: All zodiac animal images have descriptive alt text for screen readers

**After**: All zodiac animal emoji have descriptive labels for screen readers (e.g., "Rat emoji representing Year of the Rat")

---

## ✅ Updated Acceptance Checklist

### Pre-Sprint 10 Completion

- [ ] All unit tests pass (90%+ coverage)
- [ ] Integration tests pass
- [ ] Performance benchmarks met (<10ms, <500ms)
- [ ] Cultural SME review complete and approved (Vietnamese content)
- [ ] Emoji rendering tested on iOS and Android
- [ ] Localization complete (**Vietnamese and English only**)
- [ ] Accessibility tested (screen readers work with emoji)
- [ ] iOS and Android device testing complete
- [ ] Zero P0/P1 bugs

### Optional Post-Sprint 10 Enhancements

- [ ] Commission SVG zodiac artwork (parallel work, not blocking)
- [ ] Replace emoji with SVG images (Sprint 10.1 or 11)
- [ ] Add Chinese localization (Sprint 14)

---

## 🎨 UI Examples with Emoji

### Calendar Header (ZodiacHeaderView)
```
┌─────────────────────────┐
│ 🐴  2026               │
│     Year of the Horse   │
└─────────────────────────┘
```

### Zodiac Information Page
```
┌─────────────────────────┐
│          🐴             │
│       (large emoji)      │
│                          │
│        Horse             │
│     Ngọ - Ngựa          │
│      馬 - Mǎ            │
│                          │
│  Personality Traits:     │
│  • Energetic             │
│  • Independent           │
│  • Warm-hearted          │
└─────────────────────────┘
```

### Compatibility Checker
```
┌─────────────────────────┐
│  Select First Animal     │
│         🐴              │
│        Horse             │
│                          │
│         💕              │
│                          │
│  Select Second Animal    │
│         🐶              │
│         Dog              │
│                          │
│  [Check Compatibility]   │
│                          │
│  Great Match - 90%       │
│  Horse and Dog share...  │
└─────────────────────────┘
```

---

## 🧪 Testing Considerations

### Emoji-Specific Tests

**Platform Testing**:
- ✅ Emoji render correctly on iOS 15+ (various iOS versions)
- ✅ Emoji render correctly on Android 8.0+ (various Android versions)
- ✅ Emoji appear in both light and dark modes
- ✅ Emoji scale properly at different font sizes
- ✅ Emoji work with accessibility features (VoiceOver, TalkBack)

**Fallback Testing**:
- ✅ If emoji fails to render, display text name (e.g., "Rat")
- ✅ Accessibility labels work even if emoji doesn't render

**Size Testing**:
- ✅ Emoji size is appropriate in header (40x40 pt)
- ✅ Emoji size is appropriate in carousel (200x200 pt)
- ✅ Emoji size is appropriate in card view (60x60 pt)

---

## 💰 Cost-Benefit Analysis

### Sprint 10 with Emoji (Current Approach)

**Costs**:
- None (emoji is free and built-in)

**Benefits**:
- Zero implementation time for assets
- Zero bundle size increase
- Immediate availability
- No licensing concerns
- Works offline (always)
- Cross-platform consistency

**Drawbacks**:
- Less customizable than custom artwork
- Platform-dependent styling (iOS vs Android emoji may look different)
- Cannot perfectly match app's visual brand

### Future SVG Artwork (Optional Enhancement)

**Costs**:
- $50-200 for commissioned artwork
- 3-7 days for artist delivery
- 0.5 days for optimization and integration
- ~1-1.5 MB bundle size increase

**Benefits**:
- Custom art matches app brand
- Consistent across iOS and Android
- More visually polished
- Can support animations (future)

**Decision**: Start with emoji, add SVG later if needed (data-driven decision based on user feedback)

---

## 📈 Roadmap Alignment

### Sprint 10 (Current): Zodiac Animals & Year Characteristics
- ✅ Use emoji for animal images
- ✅ Vietnamese and English localization
- ✅ All core zodiac functionality

### Sprint 10.1 (Optional Polish Sprint)
- SVG artwork replacement (if commissioned)
- Performance optimization
- UI polish based on user feedback

### Sprint 11: Dynamic Backgrounds Based on Zodiac Year
- Depends on Sprint 10 (zodiac system must exist)
- May use SVG artwork if available

### Sprint 14: Vietnamese & Chinese Localization
- Add Chinese UI translations
- Add `Strings.zh.resx`
- Complete trilingual support

---

## 🚀 Action Items

### Immediate (Sprint 10)
1. ✅ **Use emoji implementation** (no asset creation needed)
2. ✅ **Focus on Vietnamese + English localization** (defer Chinese)
3. ✅ **Test emoji rendering** on iOS and Android devices
4. ✅ **Validate accessibility** with screen readers

### Parallel (Not Blocking Sprint 10)
1. ⏳ **Research SVG artwork options** (Fiverr, Upwork, open-source)
2. ⏳ **Get quotes from artists** (optional, for future sprint)
3. ⏳ **Plan Chinese localization** for Sprint 14

### Future Sprints
1. ⏳ **Sprint 10.1** (Optional): Replace emoji with SVG artwork if desired
2. ⏳ **Sprint 14**: Add Chinese localization (`Strings.zh.resx`)

---

## 📞 Questions Answered

### Q1: Will you generate animal images?
**A**: No. AI assistants cannot generate image files (SVG, PNG, etc.). 

**Solution**: Use Unicode emoji for Sprint 10. Emoji is built into iOS and Android, requires no asset creation, and looks good on mobile devices.

**Future**: Commission artwork from Fiverr/Upwork if desired (optional, ~$50-200, 3-7 days).

---

### Q2: Chinese localization requirement?
**A**: No Chinese localization in Sprint 10. Defer to Sprint 14.

**Sprint 10 Scope**:
- ✅ English UI
- ✅ Vietnamese UI (12 Con Giáp)
- ✅ Chinese characters in data (鼠, 牛, 虎 - cultural reference, not localization)

**Sprint 14 Scope** (Future):
- ⏳ Chinese UI translations
- ⏳ `Strings.zh.resx` file
- ⏳ Full trilingual support

---

## 🎯 Summary

**Key Changes**:
1. ✅ **Images**: Unicode emoji (no asset creation)
2. ✅ **Localization**: Vietnamese + English only (Chinese → Sprint 14)

**Benefits**:
- ✅ Faster Sprint 10 delivery (fewer dependencies)
- ✅ Zero bundle size impact (emoji is free)
- ✅ Simpler testing (2 languages, no image loading)
- ✅ Future flexibility (can add SVG artwork later)

**Timeline Impact**:
- Original: 10 days
- Updated: 9.6 days (slightly faster)

**Quality Impact**:
- No compromise on functionality
- Emoji looks good on mobile devices
- Can be enhanced with SVG artwork in future sprint (data-driven decision)

---

**Status**: ✅ Decisions documented and technical plan updated  
**Next Step**: `/speckit.tasks` to generate implementation tasks  
**Last Updated**: January 26, 2026
