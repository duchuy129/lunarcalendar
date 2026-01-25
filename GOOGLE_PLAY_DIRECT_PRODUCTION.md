# Google Play Console - Direct to Production Guide

**Date:** January 5, 2026
**App:** Lunar Calendar v1.0.1

---

## 🎯 You Can Skip Testing Tracks!

**Good news:** You can publish directly to Production without internal/closed testing!

---

## Prerequisites

Before starting, ensure you have:

- [x] Google Play Developer account ($25 paid)
- [ ] Android release AAB file (run `./build-android-release.sh`)
- [ ] Android keystore created (run `./create-android-keystore.sh`)
- [ ] Screenshots captured (minimum 2, recommended 4-8)
- [ ] Feature graphic created (1024x500 PNG)
- [ ] Privacy policy URL (from GitHub Gist or Pages)
- [ ] App icon 512x512 PNG

---

## Step-by-Step: Direct Production Release

### Step 1: Create App in Play Console (5 min)

1. Go to: https://play.google.com/console
2. Click **"Create app"**
3. Fill out:
   - **App name:** Lunar Calendar
   - **Default language:** English (United States)
   - **App or game:** App
   - **Free or paid:** Free
4. Check declarations:
   - ✅ I confirm this app complies with Google Play policies
   - ✅ I confirm this app complies with US export laws
5. Click **"Create app"**

---

### Step 2: Set Up App (10 min)

Complete the **Dashboard tasks** (left sidebar):

#### 2.1 App Access (1 min)
- **All or some functionality is available without restrictions**
- Click "Save"

#### 2.2 Ads (1 min)
- **No, my app does not contain ads**
- Click "Save"

#### 2.3 Content Rating (3 min)
- Click "Start questionnaire"
- **Email:** duchuy129@gmail.com
- **Category:** Reference, News, or Educational
- Answer questions (all "No" for violence, adult content, etc.)
- Should get **"Everyone"** or **"Everyone 10+"** rating
- Click "Submit"

#### 2.4 Target Audience (2 min)
- **Age groups:** All ages (check all boxes)
- **Does your app appeal primarily to children?** No
- Click "Save"

#### 2.5 News App (1 min)
- **Is this a news app?** No
- Click "Save"

#### 2.6 Data Safety (5 min)
- **Does your app collect or share user data?** No
- **Is all of the user data collected by your app encrypted in transit?** N/A (not collecting data)
- **Privacy policy URL:** [Your GitHub Gist or Pages URL]
- Click "Save"

---

### Step 3: Store Listing (10 min)

Go to **"Main store listing"** (left sidebar):

#### App Details

**Short description (80 chars):**
```
Track Vietnamese holidays and lunar dates. Bilingual. Offline. Free!
```

**Full description:**
```
LUNAR CALENDAR - Your Vietnamese Cultural Companion

Stay connected to Vietnamese traditions with this beautiful, easy-to-use lunar calendar app. Perfect for tracking Tết, traditional holidays, and special lunar days.

✨ KEY FEATURES
• 📅 Dual Calendar Display - View both Gregorian and lunar dates
• 🎊 Vietnamese Holidays - Track all major festivals
• 🌙 Special Lunar Days - Never miss Mùng 1 and Rằm
• 🌍 Bilingual Support - Vietnamese and English
• 📱 Works Offline - No internet required
• 🎨 Beautiful Interface - Clean, modern design

🎉 HOLIDAYS INCLUDED
• Tết Nguyên Đán (Lunar New Year)
• Giỗ Tổ Hùng Vương (Hung Kings Festival)
• Tết Đoan Ngọ (Dragon Boat Festival)
• Tết Trung Thu (Mid-Autumn Festival)
• And 40+ more traditional celebrations

🔒 PRIVACY FOCUSED
• No data collection
• No ads or tracking
• Completely offline
• 100% free forever

Made with ❤️ for the Vietnamese community worldwide.
```

#### Graphics

- **App icon:** Upload 512x512 PNG (resize from your 1024x1024)
- **Feature graphic:** Upload 1024x500 PNG
- **Phone screenshots:** Upload 2-8 screenshots (1080x1920 recommended)

#### Categorization

- **App category:** Lifestyle (or Productivity)
- **Tags:** vietnamese, calendar, holidays, lunar calendar, culture

#### Contact Details

- **Email:** duchuy129@gmail.com
- **Website:** https://github.com/duchuy129/lunarcalendar (optional)
- **Privacy policy URL:** [Your Gist/Pages URL]

Click **"Save"**

---

### Step 4: Create Production Release (10 min)

1. Go to **"Production"** (left sidebar under "Release")
2. Click **"Create new release"**

#### Upload AAB

3. Click **"Upload"** and select your AAB file:
   ```
   src/LunarCalendar.MobileApp/bin/Release/net10.0-android/com.huynguyen.lunarcalendar-Signed.aab
   ```

#### Release Details

4. **Release name:** 1.0.1 (100) - or use auto-generated
5. **Release notes:**
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

#### App Integrity

6. **Use Google Play App Signing:** Recommended (default)
   - This lets Google manage your upload key
   - Makes future updates easier
   - Click "Continue"

7. Click **"Save"**
8. Click **"Review release"**

---

### Step 5: Review and Rollout (2 min)

1. Review all information
2. **Rollout percentage:** 100% (full rollout)
3. Click **"Start rollout to Production"**
4. Confirm: **"Rollout"**

**Done!** 🎉

---

## Timeline After Submission

**Review Process:**
- **In Review:** Typically 2-7 days (sometimes faster)
- **Average:** 3-5 days
- **Status updates:** Via email and Play Console dashboard

**Possible Outcomes:**
1. ✅ **Approved** - App goes live immediately
2. ⚠️ **Changes Requested** - Fix issues and resubmit
3. ❌ **Rejected** - Review rejection reason, fix, resubmit

---

## If Google Requires Testing First

**Rare, but if Play Console shows "Complete testing track first":**

### Quick Internal Testing Workaround (5 min)

1. Go to **"Internal testing"** (left sidebar)
2. Click **"Create new release"**
3. Upload same AAB
4. Click **"Testers"** tab
5. Create email list: Add your own email (duchuy129@gmail.com)
6. Click **"Save"**
7. Copy testing link from email
8. Open link, click "Accept" and "Download"
9. Return to Play Console
10. Go to **"Internal testing"** → **"Promote release"** → **"Production"**
11. Click **"Promote"**

**Total time:** 5 minutes, 1 tester (yourself)

---

## Common Issues & Solutions

### Issue: "App content not complete"
**Solution:** Complete all Dashboard tasks (App access, Ads, Content rating, etc.)

### Issue: "Store listing incomplete"
**Solution:** Ensure all required fields filled:
- Short description (80 chars)
- Full description
- Screenshots (minimum 2)
- Feature graphic
- App icon
- Category
- Contact email

### Issue: "Privacy policy required"
**Solution:** Add privacy policy URL in:
- Main store listing → Contact details
- Data safety section

### Issue: "Upload failed - duplicate package name"
**Solution:** Package already exists. If first release, check:
- Bundle ID matches: com.huynguyen.lunarcalendar
- Increment version code if resubmitting

---

## Monitoring Your Release

### Check Status

1. Go to: https://play.google.com/console
2. Click your app
3. Check **"Production"** track status

**Status meanings:**
- 📝 **Draft** - Not submitted yet
- ⏳ **In Review** - Google is reviewing
- ✅ **Published** - Live on Play Store!
- ⚠️ **Rejected** - Needs fixes

### Email Notifications

Watch for emails from Google Play:
- ✉️ **play-developer-support@google.com**
- 📧 **googleplay-developer-support@google.com**

---

## After Approval (Within 2-7 Days)

When status shows **"Published"**:

1. ✅ **Test the live app:**
   ```
   https://play.google.com/store/apps/details?id=com.huynguyen.lunarcalendar
   ```

2. 📱 **Download on your device** from Play Store

3. 🎊 **Share with friends/family**

4. ⭐ **Ask for 5-star reviews**

5. 📊 **Monitor statistics** in Play Console:
   - Downloads
   - Ratings
   - Crashes (if any)
   - User reviews

---

## Quick Reference: Production Checklist

Before clicking "Start rollout to Production":

**Required Assets:**
- [x] AAB file signed with keystore
- [x] App icon 512x512
- [x] Feature graphic 1024x500
- [x] Minimum 2 screenshots
- [x] Short description (80 chars)
- [x] Full description
- [x] Privacy policy URL

**Required Settings:**
- [x] App access completed
- [x] Ads declaration completed
- [x] Content rating completed
- [x] Target audience completed
- [x] Data safety completed
- [x] News app declaration completed
- [x] Contact email set

**Build Information:**
- [x] Package name: com.huynguyen.lunarcalendar
- [x] Version code: 2 (or higher)
- [x] Version name: 1.0.1
- [x] Signed with release keystore
- [x] Format: AAB (not APK)

---

## Key Takeaway

**You do NOT need testers to publish to Production!**

Just:
1. Create app in Play Console
2. Complete dashboard setup (10 min)
3. Fill store listing (10 min)
4. Upload AAB to Production track
5. Click "Start rollout to Production"
6. Wait 2-7 days for review

**No internal testing required!**

---

**Last Updated:** January 5, 2026
**Ready to publish directly to production!** 🚀
