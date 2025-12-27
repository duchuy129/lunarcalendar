# Deploying LunarCalendar API to DigitalOcean App Platform

**Complete Step-by-Step Guide for MVP Deployment**

---

## 📋 Table of Contents

1. [Prerequisites](#prerequisites)
2. [Cost Overview](#cost-overview)
3. [Preparation Steps](#preparation-steps)
4. [Deployment Methods](#deployment-methods)
5. [Configuration](#configuration)
6. [Post-Deployment](#post-deployment)
7. [Monitoring & Maintenance](#monitoring--maintenance)
8. [Troubleshooting](#troubleshooting)

---

## Prerequisites

### Required Accounts
- ✅ **DigitalOcean Account** with $200 credit (60 days)
  - Sign up at: https://www.digitalocean.com/
  - Activate your $200 credit
- ✅ **GitHub Account** (for source code hosting)
  - Repository: Push your code to GitHub
- ✅ **Command Line Tools** (optional but recommended)
  - Install `doctl` (DigitalOcean CLI): `brew install doctl` (macOS)

### What You'll Need
- Your GitHub repository URL
- A strong JWT secret key (generate with: `openssl rand -base64 32`)
- Your mobile app updated with production API URL

---

## Cost Overview

### MVP Phase (No Database)
**Monthly Cost: $5-10/month**
- Basic Web Service (512MB-1GB RAM): $5-10/month
- **With $200 credit: FREE for 20-40 months!**

### Future with Database (Sprint 9+)
**Monthly Cost: ~$20-25/month**
- Web Service: $10/month
- PostgreSQL Database: $15/month
- **With $200 credit: FREE for 8-10 months!**

---

## Preparation Steps

### Step 1: Push Code to GitHub

```bash
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Ensure all files are committed
git add .
git commit -m "Add DigitalOcean deployment configuration"
git push origin main
```

### Step 2: Generate JWT Secret Key

```bash
# Generate a secure secret key
openssl rand -base64 32

# Save this output - you'll need it for environment variables
# Example output: xK3n8mP9qR7sT1vW2yZ4aC6bD8eF0gH2iJ4kL6mN8oP0q=
```

### Step 3: Test Locally with Docker (Optional but Recommended)

```bash
# Create .env file for local testing
cp .env.example .env

# Edit .env with your generated JWT secret
# For local testing, you can use the example values

# Build and run with Docker Compose
docker-compose up --build

# Test the API
curl http://localhost:8080/health
# Should return: {"status":"Healthy","timestamp":"..."}

# Test API endpoints
curl http://localhost:8080/api/v1/lunardate/2024/1/1
curl http://localhost:8080/api/v1/holiday/year/2024

# Stop when done
docker-compose down
```

---

## Deployment Methods

### Method A: Deploy via DigitalOcean Dashboard (Easiest)

#### 1. Create New App

1. Go to https://cloud.digitalocean.com/apps
2. Click **"Create App"**
3. Choose **"GitHub"** as source
4. Authorize DigitalOcean to access your GitHub account
5. Select your repository: `your-username/lunarcalendar`
6. Select branch: `main`
7. Click **"Next"**

#### 2. Configure Resources

**App Settings:**
- **Name**: `lunarcalendar-api`
- **Region**: Choose closest to your users (e.g., `New York - NYC3`)

**Web Service Configuration:**
- **Resource Type**: Web Service
- **Build Command**: Auto-detected (uses Dockerfile)
- **Run Command**: Auto-detected
- **HTTP Port**: `8080`
- **Instance Size**: Basic (512MB RAM, $5/month) ✅ Sufficient for MVP
- **Instance Count**: 1

**Environment Variables:**
Add these in the "Environment Variables" section:

```
ASPNETCORE_ENVIRONMENT = Production
ASPNETCORE_URLS = http://+:8080
AllowedHosts = *
JwtSettings__Issuer = LunarCalendarApi
JwtSettings__Audience = LunarCalendarApp
JwtSettings__ExpirationMinutes = 1440
```

**Secret Environment Variables** (Mark as "Encrypted"):
```
JwtSettings__SecretKey = [paste your generated secret key here]
```

#### 3. Configure Health Check

In "Health Checks" section:
- **HTTP Path**: `/health`
- **Initial Delay**: 10 seconds
- **Period**: 30 seconds
- **Timeout**: 3 seconds
- **Success Threshold**: 1
- **Failure Threshold**: 3

#### 4. Review and Deploy

1. Review all settings
2. Click **"Create Resources"**
3. Wait for deployment (usually 5-10 minutes)
4. Your app will be available at: `https://lunarcalendar-api-xxxxx.ondigitalocean.app`

---

### Method B: Deploy via doctl CLI (For Automation)

#### 1. Install and Configure doctl

```bash
# Install doctl (macOS)
brew install doctl

# Authenticate
doctl auth init
# Enter your API token from: https://cloud.digitalocean.com/account/api/tokens
```

#### 2. Update .do/app.yaml

Edit [.do/app.yaml](.do/app.yaml) and update:

```yaml
github:
  repo: your-username/lunarcalendar  # ← Change this
  branch: main
```

#### 3. Deploy

```bash
# Run deployment script
./.do/deploy.sh

# Or manually:
doctl apps create --spec .do/app.yaml
```

#### 4. Configure Secrets via Dashboard

Even with CLI deployment, you need to set secrets in the dashboard:
1. Go to your app in DigitalOcean dashboard
2. Go to **Settings** → **Environment Variables**
3. Add encrypted variable:
   - Key: `JwtSettings__SecretKey`
   - Value: [your generated secret]
   - Type: **Encrypted** ✅

---

## Configuration

### Required Environment Variables

| Variable | Value | Type | Notes |
|----------|-------|------|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` | Plain | Environment mode |
| `ASPNETCORE_URLS` | `http://+:8080` | Plain | Port binding |
| `AllowedHosts` | `*` | Plain | CORS settings |
| `JwtSettings__SecretKey` | `[your-secret]` | **Encrypted** | Min 32 chars |
| `JwtSettings__Issuer` | `LunarCalendarApi` | Plain | JWT issuer |
| `JwtSettings__Audience` | `LunarCalendarApp` | Plain | JWT audience |
| `JwtSettings__ExpirationMinutes` | `1440` | Plain | 24 hours |

### For Future Database (Sprint 9+)

When you add a database, uncomment these in `.do/app.yaml`:

```yaml
databases:
  - name: lunarcalendar-db
    engine: PG
    version: "15"
    production: false
    size: db-s-1vcpu-1gb  # $15/month
```

And add environment variable:
```
ConnectionStrings__DefaultConnection = ${db.DATABASE_URL}
```

---

## Post-Deployment

### 1. Get Your App URL

```bash
# Via dashboard:
# https://cloud.digitalocean.com/apps → Your App → Settings

# Via CLI:
doctl apps list
# Copy your app URL: https://lunarcalendar-api-xxxxx.ondigitalocean.app
```

### 2. Test Your Deployment

```bash
# Set your app URL
export API_URL="https://lunarcalendar-api-xxxxx.ondigitalocean.app"

# Test health check
curl $API_URL/health
# Expected: {"status":"Healthy","timestamp":"2024-XX-XXTXX:XX:XX.XXXXXXXZ"}

# Test API endpoints
curl $API_URL/api/v1/lunardate/2024/12/26
curl $API_URL/api/v1/holiday/year/2024
curl $API_URL/api/v1/holiday/month/2024/12

# Test Swagger docs
open $API_URL/swagger
```

### 3. Update Mobile App Configuration

Update the API URL in your mobile app:

**File**: `src/LunarCalendar.MobileApp/Services/HolidayService.cs`

**Find**:
```csharp
// For physical devices, use your computer's actual IP address
_baseUrl = "http://10.0.0.72:5090";
```

**Replace with**:
```csharp
// Production API URL
_baseUrl = "https://lunarcalendar-api-xxxxx.ondigitalocean.app";
```

**Do the same for**: `CalendarService.cs`

### 4. Test Mobile App with Production API

1. Update API URLs in both services (above)
2. Rebuild mobile app
3. Test on simulator/device
4. Verify calendar and holidays load correctly

---

## Monitoring & Maintenance

### View Logs

```bash
# Via CLI
doctl apps list  # Get your app ID
doctl apps logs YOUR_APP_ID --type run --follow

# Via Dashboard
# Go to your app → Runtime Logs
```

### Monitor Performance

**Dashboard View:**
1. Go to https://cloud.digitalocean.com/apps
2. Click your app
3. View **"Insights"** tab for:
   - CPU usage
   - Memory usage
   - Request rate
   - Response time
   - Error rate

### Set Up Alerts (Recommended)

1. Go to your app → **Settings** → **Alerts**
2. Create alerts for:
   - **CPU > 80%** for 5 minutes
   - **Memory > 80%** for 5 minutes
   - **Error Rate > 5%** for 5 minutes
3. Set notification email

### Cost Monitoring

View costs at: https://cloud.digitalocean.com/billing

**Expected costs with $200 credit:**
- Month 1-20: $0 (covered by credit)
- Month 21+: $5-10/month

---

## Troubleshooting

### Issue: App Fails to Deploy

**Check build logs:**
```bash
doctl apps logs YOUR_APP_ID --type build
```

**Common fixes:**
- Verify Dockerfile exists and is correct
- Check that all files are committed to GitHub
- Verify branch name is correct in app.yaml

### Issue: Health Check Failing

**Symptoms**: App shows "Unhealthy" status

**Fixes:**
1. Verify `/health` endpoint is accessible:
   ```bash
   curl https://your-app-url.ondigitalocean.app/health
   ```
2. Check logs for errors
3. Increase initial delay in health check settings

### Issue: API Returns 500 Errors

**Check:**
1. View runtime logs for exceptions
2. Verify environment variables are set correctly
3. Check JWT secret is configured

**Debug:**
```bash
# Get recent errors
doctl apps logs YOUR_APP_ID --type run | grep ERROR
```

### Issue: Mobile App Can't Connect

**Verify:**
1. API URL is correct (HTTPS, not HTTP)
2. API is accessible from browser
3. CORS is configured (should be set to `AllowAll` in production for MVP)

**Test connection:**
```bash
curl -I https://your-app-url.ondigitalocean.app/api/v1/holiday/year/2024
```

### Issue: High Costs

**If costs exceed expectations:**
1. Check instance size - downgrade to Basic 512MB if possible
2. Verify only 1 instance is running
3. Check for database if you haven't added one yet
4. Review bandwidth usage

---

## Useful Commands

### App Management

```bash
# List all apps
doctl apps list

# Get app details
doctl apps get YOUR_APP_ID

# Update app configuration
doctl apps update YOUR_APP_ID --spec .do/app.yaml

# Restart app
doctl apps update YOUR_APP_ID --spec .do/app.yaml

# Delete app
doctl apps delete YOUR_APP_ID
```

### Logs

```bash
# Build logs
doctl apps logs YOUR_APP_ID --type build

# Runtime logs (real-time)
doctl apps logs YOUR_APP_ID --type run --follow

# Recent logs
doctl apps logs YOUR_APP_ID --type run --tail 100
```

### Deployments

```bash
# List deployments
doctl apps list-deployments YOUR_APP_ID

# Get deployment details
doctl apps get-deployment YOUR_APP_ID DEPLOYMENT_ID

# Create new deployment (re-deploy)
doctl apps create-deployment YOUR_APP_ID
```

---

## Next Steps After Deployment

### Immediate (Week 1)
1. ✅ Verify API is accessible
2. ✅ Update mobile app with production URL
3. ✅ Test all endpoints from mobile app
4. ✅ Set up monitoring alerts
5. ✅ Document your production URL

### Short-term (Week 2-4)
1. 📱 Test on physical devices (iPhone, iPad, Android)
2. 📊 Monitor performance and errors
3. 🔒 Review security (HTTPS, rate limiting)
4. 📝 Prepare app store submissions

### Future Enhancements (Sprint 9+)
1. 🗄️ Add PostgreSQL database for user events
2. 📧 Add email notifications
3. 🔔 Add push notifications
4. 📈 Add analytics
5. 🌍 Add CDN for global distribution

---

## Support & Resources

- **DigitalOcean Docs**: https://docs.digitalocean.com/products/app-platform/
- **Community**: https://www.digitalocean.com/community/
- **Status**: https://status.digitalocean.com/
- **Billing**: https://cloud.digitalocean.com/billing
- **Support**: https://cloud.digitalocean.com/support

---

## Summary Checklist

Before going live:
- [ ] Code pushed to GitHub
- [ ] JWT secret generated and configured
- [ ] App deployed to DigitalOcean
- [ ] Health check passing
- [ ] All API endpoints tested
- [ ] Mobile app updated with production URL
- [ ] Mobile app tested with production API
- [ ] Monitoring alerts configured
- [ ] Logs reviewed for errors
- [ ] Costs monitored

You're ready for MVP! 🚀

---

**Estimated Setup Time**: 30-60 minutes
**Monthly Cost**: $5-10 (FREE with $200 credit for 20-40 months)
**Difficulty**: ⭐⭐⚪⚪⚪ (Easy to Moderate)
