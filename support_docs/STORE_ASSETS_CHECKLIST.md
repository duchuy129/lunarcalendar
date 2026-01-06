# Store Listing Assets Checklist
**Vietnamese Lunar Calendar - Required Marketing Assets**

This document lists all visual assets needed for App Store and Google Play Store submissions.

---

## 📱 App Icons (Already Created ✓)

### Current Status:
- ✅ **iOS/Android Icon:** `Resources/AppIcon/appicon.png` (1024x1024)
- ✅ Automatically generated for all required sizes

### iOS Icon Requirements:
- 1024 x 1024 pixels
- PNG format (no transparency)
- No rounded corners (iOS adds them)
- Square shape

### Android Icon Requirements:
- Adaptive icon (generated automatically)
- Multiple densities (mdpi to xxxhdpi)
- Can include rounded corners

---

## 📸 Screenshots (REQUIRED - Need to Create)

### iOS Screenshots

#### iPhone 6.7" Display (REQUIRED)
**Resolution:** 1290 x 2796 pixels
**Devices:** iPhone 15 Pro Max, 14 Pro Max
**Quantity:** Minimum 3, Maximum 10

**Recommended Shots:**
1. Main calendar view showing current month
2. Date selection with lunar details
3. Month/Year picker in action
4. Special days list
5. Settings/Language switch

**How to Capture:**
```bash
# 1. Boot iPhone 15 Pro Max simulator
xcrun simctl list devices | grep "iPhone 15 Pro Max"
xcrun simctl boot <DEVICE_ID>

# 2. Run your app
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-ios
# Install and run on simulator

# 3. Take screenshots (Cmd+S in Simulator)
# Screenshots saved to: ~/Desktop
```

#### iPhone 6.5" Display (REQUIRED)
**Resolution:** 1284 x 2778 pixels
**Devices:** iPhone 11 Pro Max, XS Max
**Quantity:** Minimum 3, Maximum 10

#### iPad Pro 12.9" (Optional but Recommended)
**Resolution:** 2048 x 2732 pixels
**Devices:** iPad Pro 12.9"
**Quantity:** Minimum 3, Maximum 10

**Status:**
- [ ] iPhone 6.7" screenshots (3-5 images)
- [ ] iPhone 6.5" screenshots (3-5 images)
- [ ] iPad screenshots (if supporting iPad)

### Android Screenshots

#### Phone Screenshots (REQUIRED)
**Resolution:** 1080 x 1920 pixels (9:16 aspect ratio)
**Alternative:** 1080 x 2340, 1440 x 3040 (any 16:9 or 9:16)
**Quantity:** Minimum 2, Maximum 8

**Recommended Shots:**
1. Main calendar with lunar dates
2. Special days feature
3. Month/Year navigation
4. Day details view
5. Language options

**How to Capture:**
```bash
# 1. Start Pixel 6 or similar emulator
~/Library/Android/sdk/emulator/emulator -avd Pixel_6_API_34 &

# 2. Build and install
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-android
~/Library/Android/sdk/platform-tools/adb install -r <APK_PATH>

# 3. Take screenshots (Click camera icon in emulator or Cmd+S)
# Screenshots saved to: ~/Desktop or emulator screenshots folder
```

#### Tablet Screenshots (Optional)
**Resolution:** 1200 x 1920 pixels or similar
**Quantity:** Minimum 2, Maximum 8

**Status:**
- [ ] Phone screenshots (2-5 images)
- [ ] Tablet screenshots (if supporting tablets)

---

## 🎨 Feature Graphic (Android Only - REQUIRED)

### Specifications:
- **Size:** 1024 x 500 pixels
- **Format:** PNG or JPEG (24-bit, no alpha)
- **File Size:** Maximum 1MB
- **Purpose:** Banner for Play Store listing

### Design Guidelines:
**Content:**
- App name: "Vietnamese Lunar Calendar"
- Visual: Moon phases, traditional calendar elements
- Colors: Match app theme (lunar/traditional Vietnamese colors)
- Text: Minimal, large enough to read on mobile

**What NOT to include:**
- ❌ Contact information
- ❌ Pricing information
- ❌ "Download" or "Install" buttons
- ❌ Low quality or pixelated images

### Design Tools:
1. **Figma** (Free) - https://figma.com
   - Professional design tool
   - Templates available
   - Export in exact dimensions

2. **Canva** (Free) - https://canva.com
   - Easy to use
   - Pre-made templates
   - Custom size: 1024 x 500

3. **Adobe Express** (Free tier) - https://adobe.com/express
   - Quick designs
   - Template library

### Template Idea:
```
Layout:
┌─────────────────────────────────────────────┐
│                                             │
│  [Moon Icon]  Vietnamese Lunar Calendar    │
│                                             │
│              Lịch Âm Việt Nam              │
│                                             │
└─────────────────────────────────────────────┘

Colors: Traditional red/gold or moon theme (blue/white)
```

**Status:**
- [ ] Feature graphic created (1024 x 500)
- [ ] Uploaded to Play Store Console

---

## 📝 App Store Listing Text

### App Name
**iOS:** Vietnamese Lunar Calendar (max 30 characters) ✓
**Android:** Vietnamese Lunar Calendar (max 50 characters) ✓

### Short Description (Android Only)
**Text:** Traditional lunar calendar with festivals and date conversion
**Length:** 80 characters max ✓

### Subtitle (iOS Only)
**Text:** Traditional Lunar Calendar
**Length:** 30 characters max ✓

### Keywords (iOS Only)
**Text:** lunar calendar,vietnamese,moon calendar,traditional,tet,festival,holiday,culture,converter
**Length:** 100 characters max ✓

### Description (Both Platforms)
See `APP_STORE_SUBMISSION_GUIDE.md` for full description text ✓

**Status:**
- ✅ App name finalized
- ✅ Descriptions written
- ✅ Keywords prepared

---

## 🎬 App Preview Video (Optional but Recommended)

### iOS App Preview
**Format:** MP4 or MOV
**Resolution:** Matches screenshot sizes
**Duration:** 15-30 seconds
**File Size:** Maximum 500MB

**Content Ideas:**
1. App launch
2. Calendar navigation
3. Date selection
4. Special days
5. Language switch

### Android Promo Video (Optional)
**Format:** YouTube URL
**Duration:** 30 seconds - 2 minutes
**Visibility:** Unlisted or public

**Status:**
- [ ] iOS app preview (optional)
- [ ] Android promo video (optional)

---

## 🔐 Privacy Policy & Support

### Privacy Policy (REQUIRED for both stores)
**Current Status:** ✓ PRIVACY_POLICY.md exists

**Hosting Options:**
1. **GitHub Pages** (Recommended - Free)
   ```bash
   # Already have the file, just enable GitHub Pages
   # Settings → Pages → Source: main branch /docs or root
   ```
   URL will be: `https://duchuy129.github.io/lunarcalendar/PRIVACY_POLICY`

2. **Alternative Hosts:**
   - Netlify (free)
   - Vercel (free)
   - Google Sites (free)
   - Your own domain

**Status:**
- ✅ Privacy policy written
- [ ] Privacy policy hosted online
- [ ] URL added to store listings

### Support URL
**Recommended:** GitHub repository
**URL:** https://github.com/duchuy129/lunarcalendar

**Alternative:** 
- Support email: your-email@example.com
- Dedicated support page

**Status:**
- [ ] Support URL/email decided
- [ ] Added to store listings

---

## 📊 Screenshot Content Suggestions

### Screenshot 1: Main Calendar View
**What to show:**
- Current month display
- Both solar and lunar dates visible
- Clean, organized layout
- Today's date highlighted

**Caption (optional):**
"View both solar and lunar dates at a glance"

### Screenshot 2: Special Days
**What to show:**
- List of Vietnamese lunar festivals
- Traditional holidays with dates
- Colorful, engaging layout

**Caption:**
"Never miss traditional celebrations"

### Screenshot 3: Date Details
**What to show:**
- Selected date with full lunar information
- Day, month, year in both calendars
- Clean information display

**Caption:**
"Instant date conversion and details"

### Screenshot 4: Month/Year Picker
**What to show:**
- Month or year selection interface
- Easy navigation demonstration
- User-friendly picker

**Caption:**
"Navigate any month and year easily"

### Screenshot 5: Language Support
**What to show:**
- Settings or language switch
- Both Vietnamese and English text visible
- Bilingual capability

**Caption:**
"Full Vietnamese and English support"

---

## ✅ Pre-Submission Asset Checklist

### iOS Assets:
- [ ] App icon (1024x1024) ✓
- [ ] iPhone 6.7" screenshots (3-10)
- [ ] iPhone 6.5" screenshots (3-10)
- [ ] iPad screenshots (if supporting iPad)
- [ ] App preview video (optional)
- [ ] Privacy policy URL
- [ ] Support URL

### Android Assets:
- [ ] App icon (512x512) ✓
- [ ] Feature graphic (1024x500)
- [ ] Phone screenshots (2-8)
- [ ] Tablet screenshots (optional)
- [ ] Promo video (optional)
- [ ] Privacy policy URL
- [ ] Support email/URL

### Text Content:
- [ ] App name ✓
- [ ] Short description (Android) ✓
- [ ] Subtitle (iOS) ✓
- [ ] Full description ✓
- [ ] Keywords (iOS) ✓
- [ ] What's new / Release notes ✓
- [ ] App category selected ✓

---

## 🎨 Quick Screenshot Capture Workflow

### For iOS:
```bash
# 1. Build and run on largest iPhone simulator
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-ios
xcrun simctl boot "iPhone 15 Pro Max"

# 2. Open simulator and take screenshots (Cmd+S)
# 3. Screenshots auto-saved to Desktop

# 4. Repeat for iPhone 11 Pro Max (6.5")
xcrun simctl boot "iPhone 11 Pro Max"

# 5. Optional: iPad Pro 12.9"
xcrun simctl boot "iPad Pro (12.9-inch)"
```

### For Android:
```bash
# 1. Build and run on Pixel 6 emulator
dotnet build src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj -f net8.0-android
~/Library/Android/sdk/emulator/emulator -avd Pixel_6_API_34

# 2. Take screenshots (Camera icon or Cmd+S)
# 3. Find in: ~/.android/avd/<AVD_NAME>/screenshots/
```

---

## 📐 Screenshot Editing Tips

### Resize Screenshots:
```bash
# If screenshots are wrong size, use ImageMagick
brew install imagemagick

# Resize to iOS 6.7" display
mogrify -resize 1290x2796! screenshot.png

# Resize to iOS 6.5" display
mogrify -resize 1284x2778! screenshot.png

# Resize to Android phone
mogrify -resize 1080x1920! screenshot.png
```

### Add Device Frames (Optional):
Use tools like:
- **Screenshot.rocks** - https://screenshot.rocks
- **Mockuphone** - https://mockuphone.com
- **Previewed** - https://previewed.app

### Quality Check:
- ✓ Text is readable
- ✓ No personal information visible
- ✓ App looks professional
- ✓ Screenshots match actual app
- ✓ Correct aspect ratio
- ✓ High resolution (not blurry)

---

## 💡 Pro Tips

1. **Use Real Content:** Don't use placeholder text or dates
2. **Show Best Features:** Highlight what makes your app special
3. **Keep It Simple:** Don't overcrowd screenshots
4. **Consistent Style:** Use same time, battery level across screenshots
5. **Test on Small Screens:** Make sure text is readable when thumbnails
6. **Localize if Possible:** Consider Vietnamese screenshots for Vietnamese market
7. **A/B Test Later:** You can update screenshots after launch

---

## 📅 Timeline for Asset Creation

### Day 1: Screenshots
- Morning: iOS screenshots (2-3 hours)
- Afternoon: Android screenshots (1-2 hours)

### Day 2: Feature Graphic
- Design and create feature graphic (2-3 hours)
- Get feedback from friends/family

### Day 3: Polish & Upload
- Review all assets
- Make any adjustments
- Upload to store consoles

**Total Time Estimate:** 1-2 days

---

## 🆘 Need Help?

### Design Resources:
- **Dribbble:** App store screenshot inspiration
- **Behance:** Professional app designs
- **App Store:** Look at successful calendar apps

### Stock Images (if needed):
- **Unsplash:** Free high-quality images
- **Pexels:** Free stock photos
- **Pixabay:** Free images and vectors

### Design Community:
- r/AppStore subreddit
- r/AndroidDev subreddit
- Indie Hackers community

---

**Last Updated:** December 30, 2024
**Status:** Ready to create assets for submission
