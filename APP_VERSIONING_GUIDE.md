# Mobile App Versioning Best Practices Guide

## 📱 Common Versioning Schemes

### 1. **Semantic Versioning (SemVer)** - MOST COMMON ✅
**Format:** `MAJOR.MINOR.PATCH` (e.g., 1.2.3)

**Rules:**
- **MAJOR (1.x.x)**: Breaking changes, major redesign, incompatible API changes
  - Example: 1.x.x → 2.0.0 (complete UI overhaul)
- **MINOR (x.1.x)**: New features, backward-compatible additions
  - Example: 1.1.0 → 1.2.0 (added Sexagenary Cycle)
- **PATCH (x.x.1)**: Bug fixes, minor tweaks, no new features
  - Example: 1.2.0 → 1.2.1 (fixed crash bug)

**Build Number:** Separate, always increments (1, 2, 3, 4...)

**Pros:**
- ✅ Industry standard (used by Google, Apple, Microsoft, etc.)
- ✅ Clear meaning for users (what type of update to expect)
- ✅ App Store guidelines friendly
- ✅ Widely understood by developers
- ✅ Simple and clean

**Cons:**
- ❌ Doesn't show sprint/iteration number
- ❌ Doesn't directly map to development cycles

**Example Progression:**
```
1.0.0 (Build 1)  - MVP Launch
1.0.1 (Build 2)  - Hotfix
1.1.0 (Build 3)  - Sprint 9: Sexagenary Cycle
1.2.0 (Build 4)  - Sprint 10: Date Detail Page
1.3.0 (Build 5)  - Sprint 11: Widgets
2.0.0 (Build 6)  - Major redesign (Phase 3)
```

---

### 2. **Calendar Versioning (CalVer)** - GROWING POPULARITY
**Format:** `YYYY.MINOR.PATCH` (e.g., 2026.1.0)

**Rules:**
- **YYYY**: Release year
- **MINOR**: Feature release within the year (1, 2, 3...)
- **PATCH**: Bug fix release

**Pros:**
- ✅ Shows when app was released
- ✅ Users can see if app is actively maintained
- ✅ Good for apps with regular release cycles
- ✅ Used by Ubuntu, pip, Bolt (popular apps)

**Cons:**
- ❌ Can look weird (2026.15.3 seems odd)
- ❌ Year changes reset minor version
- ❌ Not as familiar to all users

**Example:**
```
2026.1.0 - Sprint 9 (January)
2026.2.0 - Sprint 10 (February)
2026.3.0 - Sprint 11 (March)
```

---

### 3. **Marketing Version** - ENTERPRISE APPS
**Format:** `YEAR.QUARTER.SPRINT.BUILD` (e.g., 26.1.9.1)

**Your Proposed:** `1.1.9.1`
- Major: 1
- Minor: 1 (Phase?)
- Sprint: 9
- Build: 1

**Pros:**
- ✅ Shows sprint number directly
- ✅ Good for internal tracking
- ✅ Maps to development workflow

**Cons:**
- ❌ Confusing for end users (what does .9. mean?)
- ❌ Violates SemVer expectations (users expect bug fix at .x.1)
- ❌ Gets long quickly (1.2.15.3 looks complicated)
- ❌ Sprint numbers aren't meaningful to users
- ❌ App Store reviewers might question it
- ❌ No clear upgrade path (when to change major/minor?)

**Example:**
```
1.0.1.1 - Sprint 1
1.0.2.1 - Sprint 2
1.0.9.1 - Sprint 9
1.0.10.1 - Sprint 10 (looks weird)
1.0.15.1 - Sprint 15
2.0.1.1 - Phase 3, Sprint 1 (confusing for users)
```

---

### 4. **Hybrid Approach** - RECOMMENDED FOR YOU ✅
**Format:** `MAJOR.MINOR.PATCH` (SemVer) + Internal Sprint Mapping

**Public Version (App Store):** Standard SemVer
```
1.0.0 - MVP
1.1.0 - Sprint 9 (Sexagenary Cycle)
1.2.0 - Sprint 10 (Date Detail Page)
2.0.0 - Phase 3 (Major redesign)
```

**Internal Tracking:** Map sprints to versions
```
Sprint 1-8  → 1.0.x (MVP development)
Sprint 9    → 1.1.0 (Feature: Can Chi)
Sprint 10   → 1.2.0 (Feature: Date Detail)
Sprint 11   → 1.3.0 (Feature: Widgets)
Sprint 12   → 1.4.0 (Feature: Analytics)
...
Phase 3     → 2.0.0 (Major redesign)
```

**Documentation:** Keep sprint mapping in docs
```markdown
## Version History
- v1.0.0 (Build 1) - MVP Launch [Sprint 1-8]
- v1.1.0 (Build 6) - Sexagenary Cycle [Sprint 9]
- v1.2.0 (Build 7) - Date Detail Page [Sprint 10]
```

---

## 🎯 RECOMMENDATION FOR YOUR APP

### Use: **Semantic Versioning (SemVer)** ✅

**Why:**
1. **User-Friendly**: Users understand what each number means
2. **Industry Standard**: Apple and Google recommend it
3. **App Store Compatible**: No confusion during review
4. **Clear Communication**: Version number tells a story
5. **Professional**: Used by all major apps
6. **Flexible**: Easy to map internally to sprints

### Proposed Versioning for Your App:

```
Phase 1 (MVP):
├─ 1.0.0 (Build 1)  - Initial release
├─ 1.0.1 (Build 2-5) - Bug fixes [Sprints 1-8]

Phase 2 (Features):
├─ 1.1.0 (Build 6)  - Sexagenary Cycle [Sprint 9] ← YOU ARE HERE
├─ 1.2.0 (Build 7)  - Date Detail Page [Sprint 10]
├─ 1.3.0 (Build 8)  - Technical Debt Cleanup [Sprint 11]
├─ 1.4.0 (Build 9)  - Widgets [Sprint 12]
├─ 1.5.0 (Build 10) - Analytics [Sprint 13]
├─ 1.6.0 (Build 11) - iPad Optimizations [Sprint 14]
├─ 1.x.x (Build X)  - More features...

Phase 3 (Major Update):
├─ 2.0.0 (Build 20) - Complete redesign [Sprint 20+]
├─ 2.1.0 (Build 21) - New features
└─ ...

Bug Fixes (Anytime):
├─ 1.1.1 (Build 6.1) - Hotfix for 1.1.0
├─ 1.2.1 (Build 7.1) - Hotfix for 1.2.0
```

---

## 📋 Versioning Decision Matrix

| Scheme | User Friendly | Dev Friendly | App Store | Industry Standard | Recommended |
|--------|---------------|--------------|-----------|-------------------|-------------|
| **SemVer** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ✅ **YES** |
| **CalVer** | ⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐ | 🤔 Maybe |
| **Sprint-based** | ⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐ | ⭐ | ❌ No |
| **Hybrid** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ✅ **YES** |

---

## 🎨 Real-World Examples

### Apps Using SemVer (Most Common):
- **WhatsApp**: 2.23.24.78 (MAJOR.MINOR.PATCH.BUILD)
- **Instagram**: 308.0.0.32.114
- **Spotify**: 8.8.72.488
- **Slack**: 23.12.10
- **1Password**: 8.10.20

### Apps Using CalVer:
- **Ubuntu**: 22.04.3 (Year.Month.Patch)
- **pip**: 23.3.1
- **pytest**: 7.4.3

### Apps Using Custom:
- **Chrome**: 120.0.6099.234 (MAJOR.MINOR.BUILD.PATCH)
- **Firefox**: 121.0.1
- **Windows**: 10.0.19045.3803 (Complex)

### Apps Using Year-Based:
- **Xcode**: 15.1 (Major.Minor, year-ish)
- **macOS**: 14.2 (Sonoma)

---

## 💡 FINAL RECOMMENDATION

### For Vietnamese Lunar Calendar App:

**Use Semantic Versioning (SemVer)** with internal sprint tracking:

```
Public Version: 1.1.0
Build Number: 6
Internal Tag: sprint-9-sexagenary-cycle
Git Tag: v1.1.0
```

### Version Mapping Table (Keep in Docs):

| Version | Build | Sprint | Feature | Date |
|---------|-------|--------|---------|------|
| 1.0.0 | 1 | 1-8 | MVP Launch | Dec 2025 |
| 1.0.1 | 2-5 | - | Bug fixes | Jan 2026 |
| 1.1.0 | 6 | 9 | Sexagenary Cycle | Jan 2026 |
| 1.2.0 | 7 | 10 | Date Detail Page | Feb 2026 |
| 1.3.0 | 8 | 11 | Technical Debt | Feb 2026 |
| 1.x.x | X | 12+ | Future features | 2026 |
| 2.0.0 | 20+ | 20+ | Phase 3 Redesign | 2027? |

### Benefits of This Approach:

1. **✅ Users See Clear Versions**
   - 1.1.0 = "Oh, a new feature!"
   - 1.1.1 = "Just a bug fix"
   - 2.0.0 = "Wow, major update!"

2. **✅ App Store Friendly**
   - Follows Apple/Google guidelines
   - No confusion during review
   - Looks professional

3. **✅ Internal Tracking**
   - Git tags: `v1.1.0-sprint9`
   - Release notes: "Sprint 9: v1.1.0"
   - Easy to map sprints to versions

4. **✅ Flexible**
   - Can add features anytime (1.x.0)
   - Can fix bugs anytime (x.x.1)
   - Clear path to 2.0.0

5. **✅ Professional**
   - Matches industry standards
   - Easy for new team members
   - Clear communication

---

## 🚫 Why NOT Sprint-Based (1.1.9.1)?

### User Perspective:
```
User sees: 1.1.9.1
User thinks: "What do all these numbers mean?"
User compares: 1.1.9.1 vs 1.1.10.1 - "Is .10. newer than .9.?"
User confused: "Why did it jump from .8. to .9.?"
```

### Developer Perspective:
```
Sprint 9: 1.1.9.1
Sprint 10: 1.1.10.1  ← Looks weird
Sprint 20: 1.1.20.1  ← Gets long
Hot fix: 1.1.9.2 or 1.1.9.1.1? ← Confusing
Phase 3: 2.0.1.1 ← Reset sprint counter?
```

### App Store Perspective:
```
Reviewer sees: 1.1.9.1
Reviewer thinks: "Why so many digits?"
Reviewer compares: Most apps use 3 digits (1.2.3)
Potential issue: May question versioning scheme
```

---

## ✅ IMPLEMENTATION GUIDE

### 1. Update .csproj File:
```xml
<!-- Current -->
<ApplicationDisplayVersion>1.0.1</ApplicationDisplayVersion>
<ApplicationVersion>5</ApplicationVersion>

<!-- Change to SemVer -->
<ApplicationDisplayVersion>1.1.0</ApplicationDisplayVersion>
<ApplicationVersion>6</ApplicationVersion>
```

### 2. Create Version Mapping Doc:
```markdown
# Version History

## Phase 2: Feature Releases

### v1.1.0 (Build 6) - Sprint 9
**Release Date:** January 29, 2026
**Features:**
- Sexagenary Cycle (Can Chi / 干支)
- Element color coding
- Multi-language support

### v1.2.0 (Build 7) - Sprint 10 [PLANNED]
**Release Date:** February 2026
**Features:**
- Date Detail Page
- Technical debt cleanup
- Crash reporting
```

### 3. Git Tagging Convention:
```bash
# Format: vMAJOR.MINOR.PATCH-sprint-X
git tag -a v1.1.0 -m "Sprint 9: Sexagenary Cycle feature"

# Or include sprint in tag
git tag -a v1.1.0-sprint9 -m "Sprint 9: Sexagenary Cycle feature"

# Push tag
git push origin v1.1.0
```

### 4. Release Notes Template:
```markdown
# Release Notes - Version 1.1.0

**Sprint:** 9
**Build:** 6
**Date:** January 29, 2026

## 🎉 What's New
- Sexagenary Cycle (Can Chi / 干支) display
- Five Elements color coding
- Toggle in Settings

## 🐛 Bug Fixes
- None in this release

## 🔧 Technical
- 108 unit tests passing
- Zero compilation errors
- iOS 15.0+ and Android 8.0+ support
```

---

## 📊 COMPARISON: Your Proposal vs SemVer

### Your Proposal: 1.1.9.1
```
Pros:
+ Shows sprint number (9)
+ Good for internal tracking
+ Clear development mapping

Cons:
- Confusing for users
- Non-standard format
- Gets complex (1.1.20.1)
- No clear meaning for each digit
- App Store may question it
- Doesn't follow industry practice
```

### SemVer: 1.1.0 (Build 6)
```
Pros:
+ Industry standard
+ Clear user communication
+ App Store friendly
+ Professional appearance
+ Flexible for any workflow
+ Well understood globally

Cons:
- Doesn't show sprint directly
  (but can track in docs/git tags)
```

---

## 🎯 FINAL ANSWER

**For Vietnamese Lunar Calendar:**

### ✅ Use: Semantic Versioning (1.1.0)
**Not: Sprint-based versioning (1.1.9.1)**

### Rationale:
1. **Users don't care about sprints** - They care about features
2. **App Store expects SemVer** - Standard format
3. **Professional appearance** - Like major apps
4. **Easy to communicate** - "Version 1.1 adds Can Chi"
5. **Flexible** - Works for any development process
6. **Can still track sprints** - In docs and git tags

### Implementation:
```
Current Sprint 9 Release:
- Public Version: 1.1.0
- Build: 6
- Git Tag: v1.1.0 or v1.1.0-sprint9
- Internal: Sprint 9 → v1.1.0 (mapped in docs)
```

### Future Releases:
```
Sprint 10 → 1.2.0 (Build 7)
Sprint 11 → 1.3.0 (Build 8)
...
Phase 3 → 2.0.0 (Build 20+)
```

---

**Bottom Line:** Stick with industry-standard Semantic Versioning. Track sprints internally but show users a clean, professional version number they understand.

**Your users will see:** "Update to version 1.1.0 - New Sexagenary Cycle feature!"
**Not:** "Update to version 1.1.9.1" (confusing)

This is the best practice used by 90%+ of mobile apps worldwide. 🚀
