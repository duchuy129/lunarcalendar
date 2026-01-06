# 🚀 Quick Start Guide - Vietnamese Lunar Calendar MVP

**Your step-by-step checklist to go from code to production in 2 weeks!**

---

## ⏱️ Timeline Overview

| Phase | Days | Priority | Status |
|-------|------|----------|--------|
| **Phase 1: Deployment** | 1-3 | 🔴 Critical | ⏳ Pending |
| **Phase 2: Testing** | 4-7 | 🔴 Critical | ⏳ Pending |
| **Phase 3: App Stores** | 8-14 | 🟡 High | ⏳ Pending |
| **Phase 4: Launch** | 15+ | 🟢 Medium | ⏳ Pending |

---

## 📋 Phase 1: Deploy to DigitalOcean (Days 1-3)

### Day 1: Preparation ☕

**Morning (2 hours)**
```bash
# 1. Generate JWT secret key
openssl rand -base64 32
# Save this! You'll need it for DigitalOcean

# 2. Test Docker build locally
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar
docker-compose up --build

# 3. Verify API works
open http://localhost:8080/swagger
curl http://localhost:8080/health
```

**Afternoon (2 hours)**
- [ ] Create DigitalOcean account at https://www.digitalocean.com
- [ ] Activate $200 credit (60 days free)
- [ ] Create GitHub repository (if not already)
- [ ] Push code to GitHub:
  ```bash
  git add .
  git commit -m "Sprint 8 complete - Ready for MVP deployment"
  git push origin main
  ```

---

### Day 2: DigitalOcean Deployment 🚀

**Follow**: [DEPLOY_TO_DIGITALOCEAN.md](DEPLOY_TO_DIGITALOCEAN.md)

**Quick Steps** (Dashboard method - easiest):

1. Go to https://cloud.digitalocean.com/apps
2. Click "Create App"
3. Connect to GitHub → Select your repo → Branch: `main`
4. Configure:
   - Name: `lunarcalendar-api`
   - Region: New York (or closest)
   - Instance: Basic (512MB - $5/month)
   - Port: 8080

5. Environment Variables (Settings → Environment Variables):
   ```
   ASPNETCORE_ENVIRONMENT = Production
   ASPNETCORE_URLS = http://+:8080
   AllowedHosts = *
   JwtSettings__Issuer = LunarCalendarApi
   JwtSettings__Audience = LunarCalendarApp
   JwtSettings__ExpirationMinutes = 1440
   JwtSettings__SecretKey = [YOUR_GENERATED_SECRET] ← Encrypted!
   ```

6. Health Check:
   - Path: `/health`
   - Port: 8080

7. Click "Create Resources" → Wait 5-10 minutes

8. Copy your app URL: `https://lunarcalendar-api-xxxxx.ondigitalocean.app`

---

### Day 3: Verify Deployment ✅

```bash
# Set your production URL
export API_URL="https://your-app-url.ondigitalocean.app"

# Test endpoints
curl $API_URL/health
curl $API_URL/api/v1/lunardate/2024/12/26
curl $API_URL/api/v1/holiday/year/2024

# View Swagger docs
open $API_URL/swagger
```

**Checklist**:
- [ ] Health check returns `{"status":"Healthy"}`
- [ ] Lunar date endpoint works
- [ ] Holiday endpoint works
- [ ] Swagger UI loads

---

## 📱 Phase 2: Mobile App Testing (Days 4-7)

### Day 4: Update Mobile App 🔧

**Files to update**:

1. **HolidayService.cs** (line 36):
```csharp
// OLD:
_baseUrl = "http://10.0.0.72:5090";

// NEW:
_baseUrl = "https://your-app-url.ondigitalocean.app";
```

2. **CalendarService.cs** (similar change):
```csharp
_baseUrl = "https://your-app-url.ondigitalocean.app";
```

3. **Rebuild**:
```bash
# iOS
dotnet build -c Release -f net8.0-ios

# Android
dotnet build -c Release -f net8.0-android
```

---

### Days 5-7: Physical Device Testing 📲

**Follow**: [PHYSICAL_DEVICE_TESTING.md](PHYSICAL_DEVICE_TESTING.md)

**Priority Test Cases** (30 most critical):

#### ✅ Installation (TC1-2)
- [ ] App installs on iPhone
- [ ] App installs on Android

#### ✅ Core Features (TC3-8)
- [ ] Calendar displays current month
- [ ] Today's date highlighted
- [ ] Lunar dates show correctly
- [ ] Holidays display with colors
- [ ] Previous/Next month navigation works
- [ ] Month/Year picker works

#### ✅ Offline Mode (TC12-14) - CRITICAL!
- [ ] Works offline with cached data
- [ ] Status shows "Offline mode"
- [ ] Reconnects and syncs when online

#### ✅ Performance (TC15-17)
- [ ] App launches in < 3 seconds
- [ ] No lag when switching months
- [ ] No crashes during 30-min session

**Bug Tracking**:
Create a simple spreadsheet:
| Bug # | Severity | Description | Fixed? |
|-------|----------|-------------|--------|
| 1 | Critical | App crashes on startup | ❌ |
| 2 | Medium | Holiday text too small | ✅ |

**Fix critical bugs immediately!**

---

## 🏪 Phase 3: App Store Preparation (Days 8-14)

### Day 8: Create Developer Accounts 💳

#### Apple ($99/year)
1. Go to https://developer.apple.com/programs/
2. Enroll as individual developer
3. Complete payment
4. Wait for approval (usually 24-48 hours)

#### Google Play ($25 one-time)
1. Go to https://play.google.com/console/signup
2. Pay $25 registration fee
3. Complete account setup (immediate)

---

### Day 9-10: Screenshots & Assets 📸

**Requirements**:

**iOS**:
- iPhone 6.5" (1290 x 2796 px) - Required
- iPad Pro 12.9" (2048 x 2732 px) - Required

**Android**:
- Phone (1080 x 1920 px) - Required
- 7" Tablet (1200 x 1920 px) - Optional

**What to capture**:
1. Calendar main view (December with holidays)
2. Month/year picker open
3. Holiday detail popup
4. Settings page

**Tips**:
- Use December or January (has many holidays)
- Show "Today" highlighted
- Clean, realistic data
- Good lighting

---

### Day 11: Privacy Policy Hosting 📄

**Option 1: GitHub Pages (Free, Easiest)**

1. Create file: `docs/privacy.html`
2. Copy content from [PRIVACY_POLICY.md](PRIVACY_POLICY.md)
3. Convert to HTML or use as-is
4. Enable GitHub Pages: Repo Settings → Pages → Source: main/docs
5. URL will be: `https://your-username.github.io/lunarcalendar/privacy.html`

**Option 2: DigitalOcean Static Site**
- Create simple static site app
- Host privacy policy and terms

**Update placeholders**:
- Replace `privacy@lunarcalendar.app` with your real email
- Replace `support@lunarcalendar.app` with your real email

---

### Day 12-13: App Store Listings 📝

#### Apple App Store Connect

1. Log in: https://appstoreconnect.apple.com
2. Create new app
3. Fill in metadata:
   - **Name**: Vietnamese Lunar Calendar
   - **Subtitle**: Traditional Holidays & Lunar Dates
   - **Category**: Lifestyle or Reference
   - **Privacy Policy URL**: Your GitHub Pages URL

4. App Description (see [MVP_READY_SUMMARY.md](MVP_READY_SUMMARY.md) for template)

5. Keywords: `lunar,calendar,vietnamese,tet,holiday,festival`

6. Screenshots: Upload captured images

7. Build: Upload via Xcode or Transporter

#### Google Play Console

1. Log in: https://play.google.com/console
2. Create app
3. Fill in:
   - **App name**: Vietnamese Lunar Calendar
   - **Short description**: (80 chars - see template)
   - **Full description**: (4000 chars - see template)
   - **Category**: Lifestyle
   - **Privacy Policy URL**: Your GitHub Pages URL

4. Screenshots: Upload captured images

5. Build: Upload APK/AAB

---

### Day 14: Submit for Review 🎯

#### iOS
- [ ] Complete App Store Connect listing
- [ ] Upload build (via Xcode → Archive → Distribute)
- [ ] Select build in App Store Connect
- [ ] Submit for review

**Review time**: 1-3 days (often 24 hours)

#### Android
- [ ] Complete Play Console listing
- [ ] Upload AAB (Bundles → Upload)
- [ ] Set pricing (Free)
- [ ] Submit for review

**Review time**: Few hours to 1 day

---

## 🎉 Phase 4: Launch! (Day 15+)

### When Apps are Approved ✅

**iOS**:
- [ ] App appears in App Store
- [ ] Share link: `https://apps.apple.com/app/your-app-id`

**Android**:
- [ ] App appears in Google Play
- [ ] Share link: `https://play.google.com/store/apps/details?id=com.lunarcalendar.app`

### Post-Launch Monitoring 📊

**Week 1 After Launch**:
- [ ] Check DigitalOcean logs daily for errors
- [ ] Monitor app crash reports (Xcode Organizer, Play Console)
- [ ] Respond to user reviews
- [ ] Track download numbers

**Ongoing**:
- [ ] Monitor API costs (should be $0 with credit)
- [ ] Plan Sprint 9+ features based on feedback
- [ ] Regular updates (monthly or as needed)

---

## 🆘 Common Issues & Quick Fixes

### Issue: Docker build fails
```bash
# Clean and rebuild
docker system prune -a
docker-compose up --build
```

### Issue: DigitalOcean deployment fails
- Check build logs in DigitalOcean dashboard
- Verify Dockerfile is in repo root
- Check environment variables are set

### Issue: Mobile app can't connect to API
- Verify API URL is correct (HTTPS, not HTTP)
- Test API in browser first
- Check CORS is enabled (already configured)

### Issue: App rejected by Apple
**Common reasons**:
1. Missing privacy policy → Add URL in App Store Connect
2. Crashes on review → Test on real device first
3. Incomplete metadata → Fill all required fields

**Fix and resubmit** → Usually approved in 24 hours

### Issue: Android app won't install
- Enable "Install from Unknown Sources" in device settings
- Ensure APK is signed (release build)
- Check minimum Android version (8.0+)

---

## 📞 Getting Help

**During deployment**:
1. Check relevant guide first
2. Review error messages carefully
3. Ask me! Provide:
   - What you're trying to do
   - Error message (full text)
   - Steps you've taken

**I can help with**:
- Deployment issues
- Build errors
- Testing problems
- App store submission questions
- Bug fixes

---

## ✅ Final Checklist Before Launch

### Technical
- [ ] API deployed and accessible
- [ ] Health check working
- [ ] All endpoints tested
- [ ] Mobile app tested on real devices
- [ ] No critical bugs

### Legal
- [ ] Privacy policy online and linked
- [ ] Terms of service created
- [ ] Contact email working

### App Stores
- [ ] Developer accounts created
- [ ] Screenshots captured
- [ ] Descriptions written
- [ ] Builds uploaded
- [ ] Submitted for review

### Monitoring
- [ ] DigitalOcean alerts configured
- [ ] API logs accessible
- [ ] Crash reporting enabled (optional)

---

## 🎯 Success Metrics (First Month)

**Downloads**:
- Target: 100+ downloads (realistic for MVP)

**Ratings**:
- Target: 4.0+ stars average

**Technical**:
- Crash-free rate: 99%+
- API uptime: 99.9%+
- Response time: < 500ms average

**Costs**:
- Month 1-20: $0 (DigitalOcean credit)
- Total: $99 (Apple) + $25 (Google) = $124 first year

---

## 📚 Quick Reference Links

| Resource | Link |
|----------|------|
| **Deployment Guide** | [DEPLOY_TO_DIGITALOCEAN.md](DEPLOY_TO_DIGITALOCEAN.md) |
| **Testing Guide** | [PHYSICAL_DEVICE_TESTING.md](PHYSICAL_DEVICE_TESTING.md) |
| **Privacy Policy** | [PRIVACY_POLICY.md](PRIVACY_POLICY.md) |
| **Terms of Service** | [TERMS_OF_SERVICE.md](TERMS_OF_SERVICE.md) |
| **MVP Summary** | [MVP_READY_SUMMARY.md](MVP_READY_SUMMARY.md) |
| **Accessibility** | [ACCESSIBILITY_ENHANCEMENTS.md](ACCESSIBILITY_ENHANCEMENTS.md) |

| External | URL |
|----------|-----|
| DigitalOcean | https://cloud.digitalocean.com |
| Apple Developer | https://developer.apple.com |
| Google Play Console | https://play.google.com/console |
| App Store Connect | https://appstoreconnect.apple.com |

---

## 🎓 Pro Tips

1. **Start Early**: Don't wait for perfect - MVP is about learning!

2. **Test Thoroughly**: Physical device testing catches 90% of issues

3. **Privacy First**: Host privacy policy before submitting

4. **Monitor Costs**: Check DigitalOcean dashboard weekly

5. **Iterate Fast**: Use feedback to improve in Sprint 9+

6. **Stay Calm**: App rejections happen - fix and resubmit

7. **Celebrate**: You've built something amazing! 🎉

---

**Ready to start?**

👉 **Day 1**: Open [DEPLOY_TO_DIGITALOCEAN.md](DEPLOY_TO_DIGITALOCEAN.md) and begin!

**Questions?** Just ask - I'm here to help you succeed! 🚀

Good luck! 🍀
