# Store Assets & Policies Guide
## Vietnamese Lunar Calendar App - App Store & Google Play Submission

**Date:** January 3, 2026  
**Version:** 1.0.0  
**Target Launch:** January 20, 2026

---

## Table of Contents

1. [Required Assets Overview](#required-assets-overview)
2. [iOS App Store Assets](#ios-app-store-assets)
3. [Google Play Store Assets](#google-play-store-assets)
4. [App Descriptions & Copy](#app-descriptions--copy)
5. [Privacy Policy](#privacy-policy)
6. [Data Safety & Compliance](#data-safety--compliance)
7. [Support & Contact](#support--contact)

---

## 1. Required Assets Overview

### Quick Checklist

| Asset | iOS | Android | Status |
|-------|-----|---------|--------|
| App Icon (1024x1024) | ✅ Required | ❌ No | ⏳ Todo |
| App Icon (512x512) | ❌ No | ✅ Required | ⏳ Todo |
| Feature Graphic (1024x500) | ❌ No | ✅ Required | ⏳ Todo |
| iPhone 6.7" Screenshots (min 3) | ✅ Required | ❌ No | ⏳ Todo |
| Phone Screenshots (min 2) | ❌ No | ✅ Required | ⏳ Todo |
| App Description | ✅ Required | ✅ Required | ⏳ Todo |
| Privacy Policy | ⚠️ Optional* | ⚠️ Optional* | ⏳ Todo |
| Keywords | ✅ Required | ❌ No | ⏳ Todo |

**Note:** Privacy policy is optional if no data collection, but highly recommended.

---

## 2. iOS App Store Assets

### 2.1 App Icon

**Requirements:**
- Size: 1024x1024 pixels
- Format: PNG
- Color Space: sRGB or P3
- No transparency
- No rounded corners (iOS adds them automatically)
- No text or UI elements that might be unclear at small sizes

**Design Guidelines:**
- Use calendar motif with lunar elements
- Red/gold color scheme (traditional Vietnamese colors)
- Simple, recognizable at all sizes
- Avoid gradients that might not scale well

**Tools:**
- Design: Figma, Adobe Illustrator, Canva
- Export: PNG at @1x (1024x1024)

**File Naming:** `ios-app-icon-1024.png`

---

### 2.2 Screenshots (iPhone)

**Required Sizes:**
- **iPhone 6.7" (iPhone 15 Pro Max, 14 Pro Max):** 1290 x 2796 pixels
  - **REQUIRED** - Must provide at least 3
- **iPhone 6.5" (iPhone 11 Pro Max, XS Max):** 1242 x 2688 pixels
  - **Recommended** - For older devices

**Screenshot Content (5-6 recommended):**

1. **Calendar View - Main Screen**
   - Today's date highlighted
   - Lunar dates visible
   - Holiday indicators
   - Caption: "Dual Calendar Display"

2. **Calendar with Holiday**
   - Calendar showing a major holiday (Tet)
   - Holiday highlighted in red
   - Lunar date visible
   - Caption: "Never Miss Vietnamese Holidays"

3. **Holiday Details**
   - Holiday detail page for Tết Nguyên Đán
   - Description visible
   - Animal sign displayed
   - Caption: "Learn About Each Holiday"

4. **Year Holidays List**
   - Scrollable list of all holidays
   - Colorful holiday indicators
   - Dates for each holiday
   - Caption: "Plan Ahead All Year"

5. **Bilingual Support**
   - Side-by-side or before/after showing Vietnamese & English
   - Settings page showing language toggle
   - Caption: "Bilingual: Vietnamese & English"

**Screenshot Tips:**
- Use actual app screenshots (not mockups for MVP)
- Show real data (2026 calendar data)
- Clean status bar (use iOS Simulator, hide battery/time if needed)
- Portrait orientation only
- Use device frames (optional but professional-looking)

**Tools for Generating:**
- iOS Simulator (Xcode)
- Screenshot tool: Cmd + S in Simulator
- Frame tool: [screenshots.pro](https://screenshots.pro) or Figma

---

### 2.3 App Preview Video (Optional)

**Requirements:**
- Length: 15-30 seconds
- Resolution: 1080 x 1920 (portrait)
- Format: .mov, .m4v, or .mp4
- Max file size: 500MB

**Content:**
- Open app → Show today's date
- Navigate to next month
- Tap on holiday → Show details
- Switch language
- End with logo/title

**Tools:**
- QuickTime Screen Recording (Mac)
- iMovie for editing
- Add text overlays in Final Cut Pro/iMovie

**MVP Recommendation:** Skip for now, add in v1.1

---

### 2.4 App Store Listing Text

#### App Name
```
Vietnamese Lunar Calendar
```
- **Limit:** 30 characters
- **Character count:** 26 ✅

#### Subtitle
```
Track Tet & Holidays
```
- **Limit:** 30 characters  
- **Character count:** 21 ✅

#### Promotional Text (Optional, editable without new review)
```
🎊 Celebrate Tết 2026 on February 17! Year of the Horse 🐴
```
- **Limit:** 170 characters
- **Use:** Seasonal updates, announcements

#### Description (Full)
```
VIETNAMESE LUNAR CALENDAR - Never Miss Tết Again

Perfect for Vietnamese diaspora and anyone celebrating Vietnamese culture!

✨ FEATURES
• 📅 Dual Calendar Display - See both Gregorian and lunar dates
• 🎊 Vietnamese Holidays - All major holidays (Tết, Hùng Kings, National Day)
• 🌙 Lunar Special Days - Track Mùng 1 and Rằm (1st and 15th)
• 🐉 Zodiac Animals - See the year's zodiac animal for each date
• 🌍 Bilingual - Full support for Vietnamese and English
• ✈️ Works Offline - No internet required

🎉 HOLIDAYS INCLUDED
• Tết Nguyên Đán (Lunar New Year)
• Giỗ Tổ Hùng Vương (Hung Kings Festival)
• Tết Đoan Ngọ (Dragon Boat Festival)
• Tết Trung Thu (Mid-Autumn Festival)
• And 40+ more traditional festivals

👨‍👩‍👧‍👦 PERFECT FOR
• Planning celebrations around lunar dates
• Teaching children about Vietnamese culture
• Coordinating with family in Vietnam
• Never forgetting important traditional days

🆓 100% FREE. NO ADS. NO DATA COLLECTION.

Made with ❤️ for the Vietnamese community worldwide.
```
- **Limit:** 4000 characters
- **Character count:** ~850 ✅

#### Keywords (100 characters max)
```
lunar calendar,vietnamese,tet,holidays,zodiac,moon,festival,culture,viet,ngay am lich
```
- **Character count:** 90 ✅
- **Tips:** Comma-separated, no spaces after commas, lowercase, include Vietnamese terms

#### What's New (Version 1.0.0)
```
🎉 Initial Release - Welcome!

Features:
• Dual calendar (Gregorian + Lunar)
• 45+ Vietnamese holidays and festivals
• Bilingual (Vietnamese/English)
• Works completely offline
• No ads, no tracking

Chúc mừng năm mới! 🎊
```

---

### 2.5 App Store Connect Configuration

#### App Information
- **Bundle ID:** `com.huynguyen.lunarcalendar`
- **SKU:** `lunarcalendar-ios-2026`
- **Primary Category:** Lifestyle
- **Secondary Category:** Utilities
- **Content Rights:** I own the rights to this app

#### Pricing & Availability
- **Price:** Free
- **Availability:** All territories
- **Pre-order:** No

#### Age Rating
Complete questionnaire:
- **Unrestricted Web Access:** No
- **Gambling:** No
- **Contests:** No
- **Mature/Suggestive Themes:** None
- **Violence:** None
- **Profanity/Crude Humor:** None
- **Result:** **4+** (Everyone)

#### App Privacy
Answer questions:
- **Does this app collect data from users?** NO
- **Do you or third-party partners track users?** NO
- **Privacy Policy URL:** (Optional - provide if you create one)

**Result:** "This app does not collect any data"

#### App Review Information
- **Contact Email:** your-email@example.com
- **Contact Phone:** +1-XXX-XXX-XXXX
- **Demo Account:** Not needed
- **Notes for Reviewer:**
  ```
  This is a simple offline calendar app for the Vietnamese community.
  It displays lunar dates and Vietnamese holidays. No user accounts,
  no data collection, no backend server. All calculations are done
  locally on the device.
  
  To test:
  1. Open app - see today's date with lunar date below
  2. Navigate months using < > arrows
  3. Tap any holiday to see details
  4. Go to Settings tab to switch language
  5. Check Year Holidays tab for full year view
  ```

---

## 3. Google Play Store Assets

### 3.1 App Icon

**Requirements:**
- Size: 512x512 pixels
- Format: 32-bit PNG
- Max file size: 1MB
- Color space: sRGB

**Design:** Same as iOS icon, just export at 512x512

**File Naming:** `android-app-icon-512.png`

---

### 3.2 Feature Graphic (Banner)

**Requirements:**
- Size: 1024 x 500 pixels
- Format: PNG or JPEG
- Max file size: 1MB
- **REQUIRED** for Google Play

**Design Ideas:**
- App name + icon on left
- Calendar visual on right
- Vietnamese colors (red/gold)
- Tagline: "Never Miss Tết Again"

**Example Layout:**
```
[APP ICON]  Vietnamese Lunar Calendar
            Never Miss Vietnamese Holidays
            [Calendar visual with lunar dates]
```

**Tools:**
- Canva (has templates)
- Figma
- Adobe Photoshop

**File Naming:** `feature-graphic-1024x500.png`

---

### 3.3 Screenshots (Phone)

**Requirements:**
- **Minimum:** 2 screenshots
- **Recommended:** 8 screenshots
- **Size:** Min 320px on short side, max 3840px on long side
- **Aspect Ratio:** 16:9 or 9:16 (portrait recommended)
- **Format:** PNG or JPEG

**Recommended Size:** 1080 x 1920 pixels (Full HD portrait)

**Content:** Same 5-6 screenshots as iOS

**Tools:**
- Android Emulator (Android Studio)
- Or export iOS screenshots (if UI is identical)

---

### 3.4 Play Store Listing

#### App Name
```
Vietnamese Lunar Calendar
```
- **Limit:** 50 characters
- **Character count:** 26 ✅

#### Short Description
```
Track Vietnamese holidays and lunar dates. Bilingual (Vietnamese/English). Works offline. No ads.
```
- **Limit:** 80 characters
- **Character count:** 97 ❌ **TOO LONG**

**Shortened:**
```
Track Tết & Vietnamese holidays. Bilingual. Offline. Free. No ads.
```
- **Character count:** 71 ✅

#### Full Description
```
VIETNAMESE LUNAR CALENDAR - Never Miss Tết Again 🎊

Perfect for Vietnamese diaspora and anyone celebrating Vietnamese culture!

✨ FEATURES
📅 Dual Calendar Display - See both Gregorian and lunar dates
🎊 Vietnamese Holidays - All major holidays (Tết, Hùng Kings, National Day)
🌙 Lunar Special Days - Track Mùng 1 and Rằm (1st and 15th of each month)
🐉 Zodiac Animals - See the year's zodiac animal for each date
🌍 Bilingual - Full support for Vietnamese and English
✈️ Works Offline - No internet required

🎉 HOLIDAYS INCLUDED
• Tết Nguyên Đán (Lunar New Year)
• Giỗ Tổ Hùng Vương (Hung Kings Festival)  
• Tết Đoan Ngọ (Dragon Boat Festival)
• Tết Trung Thu (Mid-Autumn Festival)
• Tết Nguyên Tiêu (Lantern Festival)
• Vu Lan (Ullambana - Parents' Day)
• And 40+ more traditional festivals

👨‍👩‍👧‍👦 PERFECT FOR
• Planning celebrations around lunar dates
• Teaching children about Vietnamese culture
• Coordinating with family in Vietnam  
• Never forgetting important traditional days
• Learning about Vietnamese zodiac animals

🆓 100% FREE
• No advertisements
• No data collection
• No tracking
• No accounts required
• No internet needed

💝 MADE WITH LOVE
Created for the Vietnamese community worldwide. Keep your cultural traditions alive!

📱 WORKS COMPLETELY OFFLINE
All calculations are done on your device. No internet connection required.

🌏 BILINGUAL
Switch seamlessly between Vietnamese and English.

---

Keywords: lịch âm, lịch vạn niên, lunar calendar, vietnamese calendar, tet, zodiac, holidays, festivals
```
- **Limit:** 4000 characters
- **Character count:** ~1400 ✅

#### Category
- **Primary:** Lifestyle
- **Secondary:** (None needed)

#### Tags (Choose up to 5)
- Lifestyle
- Culture
- Calendar
- Education
- Productivity

---

### 3.5 Content Rating

Complete questionnaire:
- **Violence:** None
- **Sexual Content:** None
- **Profanity:** None
- **Controlled Substances:** None
- **User Interaction:** None
- **Shares Location:** No
- **Shares Personal Info:** No

**Result:** **EVERYONE** rating

---

### 3.6 Data Safety Section

**CRITICAL:** This is prominently displayed in Play Store

#### Does your app collect or share user data?
**Answer:** NO

#### Explanation for users:
```
This app does not collect, store, or share any user data. All calendar calculations are performed locally on your device. No internet connection is required. No analytics, no tracking, no data collection.
```

#### Security practices:
- **Data encrypted in transit:** N/A (no data transmitted)
- **Users can request data deletion:** N/A (no data collected)
- **Independent security review:** No
- **Privacy policy:** (Provide URL if you create one)

**This "No data collection" stance is a HUGE selling point - emphasize it!**

---

## 4. App Descriptions & Copy

### 4.1 Key Selling Points (Use in marketing)

1. **Offline-first** - Works without internet
2. **No data collection** - Complete privacy
3. **Bilingual** - Vietnamese & English
4. **Comprehensive** - 45+ holidays
5. **Cultural education** - Zodiac animals, traditions
6. **Free forever** - No ads, no IAP
7. **Simple UX** - Easy for all ages

### 4.2 Target Audience Personas

1. **Vietnamese Diaspora (Primary)**
   - Age: 25-55
   - Location: US, Canada, Australia, Europe
   - Need: Stay connected to culture, plan family celebrations

2. **Vietnamese in Vietnam (Secondary)**
   - Age: 30-60
   - Need: Convenient lunar calendar reference

3. **Non-Vietnamese Partners/Friends (Tertiary)**
   - Age: 25-50
   - Need: Learn about Vietnamese culture, remember partner's/friend's holidays

### 4.3 Value Propositions by Audience

**For Parents:**
> "Teach your children about Vietnamese traditions. Never miss Tết or important lunar days."

**For Expats:**
> "Stay connected to your roots. Plan celebrations and coordinate with family back home."

**For Learners:**
> "Explore Vietnamese culture. Understand the lunar calendar and traditional festivals."

---

## 5. Privacy Policy

### 5.1 Do You Need One?

**For your app: OPTIONAL but RECOMMENDED**

- iOS: Optional if no data collection
- Android: Optional if no data collection
- **But:** Having one builds trust, even if it says "we collect nothing"

### 5.2 Simple Privacy Policy Template

```markdown
# Privacy Policy for Vietnamese Lunar Calendar

**Last Updated:** January 3, 2026

## Our Commitment to Privacy

Vietnamese Lunar Calendar ("the App") is committed to protecting your privacy. This policy explains our data practices.

## Data Collection

**We collect ZERO data.**

The App:
- Does NOT collect any personal information
- Does NOT track your usage
- Does NOT use analytics services
- Does NOT use advertising networks
- Does NOT require an account
- Does NOT connect to the internet (except for app updates)

## How the App Works

All calendar calculations are performed locally on your device using the device's built-in calendar system. No data leaves your device.

## Data Storage

The only data stored on your device:
- Your app settings (language preference, display options)
- Cached calendar calculations for faster performance

This data:
- Stays on your device only
- Is never transmitted anywhere
- Is deleted when you uninstall the app

## Third-Party Services

We do not use any third-party services, SDKs, or analytics tools.

## Children's Privacy

The App is safe for all ages. We do not knowingly collect data from anyone, including children under 13.

## Changes to This Policy

We will update this policy if our data practices change. The "Last Updated" date will reflect any changes.

## Contact Us

If you have questions about this privacy policy:
- Email: your-email@example.com
- GitHub: https://github.com/duchuy129/lunarcalendar

## Your Rights

Since we collect no data, there is no data to:
- Access
- Correct
- Delete
- Export

You remain in complete control of your device and the App's settings at all times.

---

**Summary: We don't collect any data. Period.**
```

### 5.3 Where to Host Privacy Policy

**Options:**

1. **GitHub Pages (Free, Recommended)**
   - Create `privacy-policy.md` in repo
   - Enable GitHub Pages
   - URL: `https://duchuy129.github.io/lunarcalendar/privacy-policy`

2. **Personal Website**
   - Host on your domain
   - URL: `https://yourdomain.com/privacy-policy`

3. **Google Docs (Quick & Easy)**
   - Create doc, set to "Anyone with link can view"
   - Publish to web
   - Use published URL

**MVP Recommendation:** GitHub Pages (free, version controlled, professional)

---

## 6. Data Safety & Compliance

### 6.1 GDPR Compliance (EU)

**Status:** ✅ COMPLIANT (by default - no data collection)

**Why:** If you don't collect data, GDPR doesn't apply to data handling.

### 6.2 COPPA Compliance (US - Children's Privacy)

**Status:** ✅ COMPLIANT

**Why:** App doesn't collect any data, safe for all ages.

### 6.3 CCPA Compliance (California Privacy)

**Status:** ✅ COMPLIANT

**Why:** No data collection = no data to manage under CCPA.

### 6.4 App Store Small Business Program

**Are you eligible?**
- Earn less than $1M/year from all apps: YES (free app)
- **Benefit:** 15% commission (vs 30%) if you add IAP later

**Action:** Nothing required now (app is free)

---

## 7. Support & Contact

### 7.1 Support Email

**Set up a dedicated email:**
- `lunarcalendar.support@gmail.com` (recommended)
- Or use personal email with filter/label

**Auto-response template:**
```
Thank you for contacting Vietnamese Lunar Calendar support!

I'll respond within 24-48 hours.

Common questions:
- How do I change language? → Settings tab → Language
- Does it work offline? → Yes, 100% offline!
- Is my data safe? → We don't collect ANY data.

For bug reports, please include:
- Device model
- OS version (iOS/Android)
- App version
- Steps to reproduce

Best regards,
Huy Nguyen
Developer, Vietnamese Lunar Calendar
```

### 7.2 Support Page (Optional)

**Create simple FAQ:**
```markdown
# Vietnamese Lunar Calendar - Support

## Frequently Asked Questions

### How do I switch language?
1. Open app
2. Tap "Settings" tab (bottom right)
3. Tap "Language"
4. Choose Vietnamese or English

### Does the app work offline?
Yes! 100% offline. No internet connection needed.

### Why do I need this app?
To track Vietnamese holidays and lunar dates, especially if you're celebrating Tết or other traditional festivals.

### Is my data private?
Yes! We don't collect ANY data. Everything stays on your device.

### How do I report a bug?
Email: your-email@example.com

### What holidays are included?
45+ Vietnamese holidays including:
- Tết Nguyên Đán (Lunar New Year)
- Giỗ Tổ Hùng Vương
- Tết Đoan Ngọ
- Tết Trung Thu
- And many more!

### Is it really free?
Yes! No ads, no in-app purchases, free forever.
```

### 7.3 GitHub Repository

**Should you make it public?**

**Pros:**
- Builds trust (users can see source code)
- Community contributions (bug reports, translations)
- Portfolio piece
- Open source = more credibility

**Cons:**
- Code is public (but that's okay for this app)
- Need to manage issues/PRs

**Recommendation:** Make it public after launch (or now)

---

## Quick Action Items for Store Submission

### Before Submission Day:

1. **Create App Icons**
   - [ ] iOS: 1024x1024 PNG
   - [ ] Android: 512x512 PNG

2. **Take Screenshots**
   - [ ] iOS: 6.7" (min 3) - Use iPhone 15 Pro Max simulator
   - [ ] Android: 1080x1920 (min 2) - Use Android emulator

3. **Create Feature Graphic**
   - [ ] Android: 1024x500 PNG

4. **Write Copy**
   - [ ] App name (verified: "Vietnamese Lunar Calendar")
   - [ ] Short description (done above)
   - [ ] Full description (done above)
   - [ ] Keywords/tags (done above)

5. **Set Up Privacy Policy**
   - [ ] Create privacy-policy.md
   - [ ] Host on GitHub Pages
   - [ ] Get URL

6. **Set Up Support**
   - [ ] Create support email
   - [ ] Set up auto-responder
   - [ ] Create FAQ page (optional)

7. **Test Final Build**
   - [ ] iOS: Real device test
   - [ ] Android: Real device test
   - [ ] Verify no debug code
   - [ ] Check version number (1.0.0)

---

## Resources & Tools

### Design Tools
- **Icons:** Figma, Adobe Illustrator, Canva
- **Screenshots:** iOS Simulator, Android Emulator
- **Frames:** screenshots.pro, Figma community templates
- **Feature Graphic:** Canva (has templates)

### Text Tools
- **Character Counter:** https://charactercounttool.com
- **Grammarly:** For proofreading descriptions

### Privacy Policy Generators
- **TermsFeed:** https://www.termsfeed.com/privacy-policy-generator/
- **FreePrivacyPolicy:** https://www.freeprivacypolicy.com/

### Store Guidelines
- **iOS:** https://developer.apple.com/app-store/review/guidelines/
- **Android:** https://play.google.com/about/developer-content-policy/

---

## Next Steps

1. ✅ Code cleanup complete
2. ⏳ Create assets (icons, screenshots, feature graphic)
3. ⏳ Set up privacy policy
4. ⏳ Create support email
5. ⏳ Final device testing
6. ⏳ Submit to stores!

**Target Submission Date:** January 15-17, 2026  
**Target Launch Date:** January 20, 2026

Good luck! 🚀🎊
