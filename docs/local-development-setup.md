# Local Development Setup Guide
## Lunar Calendar Mobile Application

**Version:** 1.0
**Date:** 2025-12-11
**Project:** Lunar Calendar App

---

## 1. Overview

This guide walks you through setting up a complete local development environment for the Lunar Calendar application. You'll be able to develop and test the entire application stack (mobile apps + backend API + database) on your local machine before deploying to Azure.

### 1.1 Development Workflow

```
Local Development → Local Testing → Azure Deployment

Phase 1: Local Setup (This Guide)
├── Install development tools
├── Set up local database
├── Run backend API locally
├── Run mobile apps on emulators/devices
└── Test complete integration locally

Phase 2: Azure Deployment (After local testing succeeds)
├── Create Azure account (free tier)
├── Deploy backend to Azure
├── Configure production database
└── Point mobile apps to Azure API
```

**Benefits of Local Development First:**
- ✅ No cloud costs during development
- ✅ Faster iteration and debugging
- ✅ Work offline without internet
- ✅ Learn the stack before cloud deployment
- ✅ Catch issues early in a controlled environment

---

## 2. Prerequisites & Tools Installation

### 2.1 Required Software

#### Windows Users

| Tool | Purpose | Download Link | Cost |
|------|---------|--------------|------|
| **Visual Studio 2022 Community** | Primary IDE for backend and mobile | [Download](https://visualstudio.microsoft.com/downloads/) | Free |
| **.NET 8 SDK** | Runtime for backend API | [Download](https://dotnet.microsoft.com/download/dotnet/8.0) | Free |
| **PostgreSQL 15+** | Local database | [Download](https://www.postgresql.org/download/windows/) | Free |
| **Docker Desktop** | Container runtime (optional) | [Download](https://www.docker.com/products/docker-desktop) | Free |
| **Git** | Version control | [Download](https://git-scm.com/downloads) | Free |
| **Postman** | API testing | [Download](https://www.postman.com/downloads/) | Free |

**Visual Studio 2022 Workloads to Install:**
- ASP.NET and web development
- .NET Multi-platform App UI development
- Mobile development with .NET

#### macOS Users

| Tool | Purpose | Installation | Cost |
|------|---------|-------------|------|
| **Visual Studio 2022 for Mac** | Primary IDE | [Download](https://visualstudio.microsoft.com/vs/mac/) | Free |
| **Visual Studio Code** | Alternative/lightweight IDE | [Download](https://code.visualstudio.com/) | Free |
| **.NET 8 SDK** | Runtime | `brew install dotnet@8` | Free |
| **PostgreSQL 15+** | Local database | `brew install postgresql@15` | Free |
| **Docker Desktop** | Container runtime | [Download](https://www.docker.com/products/docker-desktop) | Free |
| **Xcode** | iOS development & simulators | Mac App Store | Free |
| **Git** | Version control | `brew install git` | Free |

**Note:** Xcode is required for iOS development and includes iOS Simulator. It's a large download (~12GB).

#### Linux Users

```bash
# .NET 8 SDK
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0

# PostgreSQL
sudo apt-get update
sudo apt-get install postgresql postgresql-contrib

# Docker
sudo apt-get install docker.io docker-compose

# Git
sudo apt-get install git
```

---

## 3. Local Environment Setup

### 3.1 PostgreSQL Database Setup

#### Option 1: Install PostgreSQL Directly (Recommended for Beginners)

**Windows:**
1. Run PostgreSQL installer
2. Set password for `postgres` user (remember this!)
3. Default port: 5432
4. Create database using pgAdmin or command line:

```sql
-- Open pgAdmin or psql command line
CREATE DATABASE lunarcalendar;
```

**macOS:**
```bash
# Start PostgreSQL service
brew services start postgresql@15

# Create database
createdb lunarcalendar

# Access database
psql lunarcalendar
```

**Connection String for Local Development:**
```
Host=localhost;Database=lunarcalendar;Username=postgres;Password=YOUR_PASSWORD
```

#### Option 2: Use Docker for PostgreSQL (Recommended for Advanced Users)

Create `docker-compose.dev.yml` in your project root:

```yaml
version: '3.8'

services:
  postgres:
    image: postgres:15-alpine
    container_name: lunarcalendar-db
    environment:
      POSTGRES_DB: lunarcalendar
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: dev_password_123
    ports:
      - "5432:5432"
    volumes:
      - postgres-data:/var/lib/postgresql/data
    restart: unless-stopped

volumes:
  postgres-data:
```

**Start database:**
```bash
docker-compose -f docker-compose.dev.yml up -d
```

**Connection String:**
```
Host=localhost;Database=lunarcalendar;Username=postgres;Password=dev_password_123
```

### 3.2 Verify Database Connection

**Using psql:**
```bash
psql -h localhost -U postgres -d lunarcalendar
```

**Using pgAdmin:**
1. Open pgAdmin
2. Create new server connection
3. Host: localhost, Port: 5432
4. Database: lunarcalendar, User: postgres

---

## 4. Backend API Setup

### 4.1 Create ASP.NET Core Project

```bash
# Navigate to your project directory
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar

# Create solution
dotnet new sln -n LunarCalendar

# Create Web API project
dotnet new webapi -n LunarCalendar.Api -o src/LunarCalendar.Api

# Add to solution
dotnet sln add src/LunarCalendar.Api/LunarCalendar.Api.csproj

# Navigate to API project
cd src/LunarCalendar.Api
```

### 4.2 Install Required NuGet Packages

```bash
# Entity Framework Core for PostgreSQL
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.0

# Entity Framework Tools (for migrations)
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0

# Authentication & Security
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add package BCrypt.Net-Next --version 4.0.3

# Validation
dotnet add package FluentValidation.AspNetCore --version 11.3.0

# AutoMapper
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1

# Logging
dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package Serilog.Sinks.Console --version 5.0.1
dotnet add package Serilog.Sinks.File --version 5.0.0

# Swagger (already included in template)
# Microsoft.AspNetCore.OpenApi
# Swashbuckle.AspNetCore
```

### 4.3 Configure Local Development Settings

**appsettings.Development.json:**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=lunarcalendar;Username=postgres;Password=dev_password_123"
  },
  "Jwt": {
    "Key": "THIS_IS_A_SUPER_SECRET_KEY_FOR_LOCAL_DEVELOPMENT_ONLY_DO_NOT_USE_IN_PRODUCTION",
    "Issuer": "lunarcalendar-api-dev",
    "Audience": "lunarcalendar-mobile-dev",
    "ExpiryMinutes": 60
  },
  "AllowedHosts": "*",
  "Cors": {
    "AllowedOrigins": ["http://localhost", "https://localhost"]
  }
}
```

**⚠️ Important:** Never commit real passwords or production keys to Git. This is for local development only.

### 4.4 Run Database Migrations

```bash
# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migration to database
dotnet ef database update
```

### 4.5 Run Backend API Locally

```bash
# Run the API
dotnet run

# Or with hot reload (recommended during development)
dotnet watch run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

**Test the API:**
- Open browser: https://localhost:5001/swagger
- You should see the Swagger UI with API documentation

**Test Health Check:**
```bash
curl https://localhost:5001/api/health
```

---

## 5. Mobile App Setup

### 5.1 Create .NET MAUI Project

```bash
# Navigate to src folder
cd /Users/huynguyen/Documents/GitHub/MobileProjects/lunarcalendar/src

# Create MAUI project
dotnet new maui -n LunarCalendar.MobileApp -o LunarCalendar.MobileApp

# Add to solution
cd ..
dotnet sln add src/LunarCalendar.MobileApp/LunarCalendar.MobileApp.csproj
```

### 5.2 Install Mobile NuGet Packages

```bash
cd src/LunarCalendar.MobileApp

# MVVM Toolkit
dotnet add package CommunityToolkit.Mvvm --version 8.2.2

# HTTP Client
dotnet add package Refit --version 7.0.0

# SQLite
dotnet add package sqlite-net-pcl --version 1.8.116
```

### 5.3 Configure API Endpoint for Local Development

**Constants.cs:**
```csharp
public static class Constants
{
    // Android emulator uses 10.0.2.2 to access host machine's localhost
    // iOS simulator uses localhost directly

#if ANDROID
    public const string ApiBaseUrl = "https://10.0.2.2:5001";
#elif IOS
    public const string ApiBaseUrl = "https://localhost:5001";
#else
    public const string ApiBaseUrl = "https://localhost:5001";
#endif

    public const string DatabaseFilename = "lunarcalendar.db3";
}
```

### 5.4 Trust Development SSL Certificate (Important!)

**Windows:**
```bash
dotnet dev-certs https --trust
```

**macOS:**
```bash
dotnet dev-certs https --trust
```

**Linux:**
```bash
dotnet dev-certs https
# Manual trust required - check your distribution's documentation
```

This is necessary for the mobile app to communicate with your local HTTPS API.

---

## 6. Running the Complete Stack Locally

### 6.1 Start Backend API

**Terminal 1:**
```bash
cd src/LunarCalendar.Api
dotnet watch run
```

Keep this terminal open. The API should be running on https://localhost:5001

### 6.2 Run iOS App (macOS only)

**Terminal 2:**
```bash
cd src/LunarCalendar.MobileApp
dotnet build -t:Run -f net8.0-ios
```

**Or use Visual Studio:**
1. Open solution in Visual Studio for Mac
2. Set `LunarCalendar.MobileApp` as startup project
3. Select iOS Simulator (e.g., iPhone 14)
4. Press F5 or click Run

### 6.3 Run Android App

**Terminal 2:**
```bash
cd src/LunarCalendar.MobileApp
dotnet build -t:Run -f net8.0-android
```

**Or use Visual Studio:**
1. Open solution
2. Set `LunarCalendar.MobileApp` as startup project
3. Select Android Emulator
4. Press F5 or click Run

**First Time Android Setup:**
1. Install Android SDK via Visual Studio
2. Create Android Virtual Device (AVD):
   - Open Android Device Manager in Visual Studio
   - Create new device (e.g., Pixel 5, API 31)

---

## 7. Local Development Tools & Workflow

### 7.1 Recommended VS Code Extensions (if using VS Code)

```bash
# Install C# extension
code --install-extension ms-dotnettools.csharp

# Install .NET MAUI extension
code --install-extension ms-dotnettools.dotnet-maui

# Install Docker extension
code --install-extension ms-azuretools.vscode-docker

# Install PostgreSQL extension
code --install-extension ms-ossdata.vscode-postgresql
```

### 7.2 Database Management Tools

**Option 1: pgAdmin (GUI)**
- Comes with PostgreSQL installation
- Great for beginners
- Visual query builder

**Option 2: DBeaver (Free, Cross-platform)**
- Download: https://dbeaver.io/download/
- Supports multiple databases
- More lightweight than pgAdmin

**Option 3: Command Line (psql)**
```bash
# Connect to database
psql -h localhost -U postgres -d lunarcalendar

# Common commands
\dt          # List tables
\d events    # Describe events table
SELECT * FROM users LIMIT 10;  # Query data
```

### 7.3 API Testing with Postman

1. Download Postman: https://www.postman.com/downloads/
2. Import API collection (create one from Swagger)
3. Set environment variables:
   - `baseUrl`: https://localhost:5001
   - `accessToken`: (obtained after login)

**Sample Request:**
```
POST https://localhost:5001/api/auth/register
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Test123!",
  "firstName": "Test",
  "lastName": "User"
}
```

### 7.4 Debugging Tips

**Backend Debugging (Visual Studio):**
1. Set breakpoints in your code
2. Press F5 to start debugging
3. Step through code when breakpoint is hit

**Mobile App Debugging:**
1. Set breakpoints in ViewModels or Services
2. Run app in debug mode (F5)
3. Interact with app to trigger breakpoints

**Common Issues & Solutions:**

| Issue | Solution |
|-------|----------|
| "Cannot connect to database" | Check PostgreSQL is running: `brew services list` or check Windows Services |
| "SSL certificate error" | Run `dotnet dev-certs https --trust` |
| "Android emulator not found" | Create AVD in Android Device Manager |
| "iOS simulator not found" | Install Xcode from Mac App Store |
| "Port 5000 already in use" | Change port in `launchSettings.json` |

---

## 8. Local Development Checklist

### Before Starting Development:

- [ ] All required software installed
- [ ] PostgreSQL database running
- [ ] Database `lunarcalendar` created
- [ ] .NET 8 SDK installed and verified (`dotnet --version`)
- [ ] Git configured with your credentials
- [ ] Development SSL certificates trusted

### Before Each Coding Session:

- [ ] Pull latest code from Git (`git pull`)
- [ ] Start PostgreSQL (if not auto-started)
- [ ] Start backend API in Terminal 1
- [ ] Verify API is running (visit Swagger UI)
- [ ] Run mobile app in Terminal 2

### After Making Changes:

- [ ] Test changes locally
- [ ] Run database migrations if schema changed
- [ ] Commit changes to Git with clear messages
- [ ] Push to repository

---

## 9. Local vs Azure: Configuration Differences

### Local Development
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=lunarcalendar;Username=postgres;Password=dev_password_123"
  },
  "ApiBaseUrl": "https://localhost:5001"
}
```

### Azure Production (After Deployment)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=lunarcalendar-db.postgres.database.azure.com;Database=lunarcalendar;Username=adminuser@lunarcalendar-db;Password=SECURE_PASSWORD;SslMode=Require"
  },
  "ApiBaseUrl": "https://lunarcalendar-api.azurewebsites.net"
}
```

**You'll update these values when deploying to Azure in Phase 2.**

---

## 10. Transition to Azure (Future Steps)

Once your local development is complete and working smoothly:

### 10.1 When to Move to Azure

✅ **Ready for Azure when:**
- Backend API runs successfully on localhost
- Mobile apps can connect to local API
- Database migrations work correctly
- Basic authentication flow works
- You can create/read/update/delete events locally
- Guest mode works as expected

### 10.2 Azure Setup Steps (Covered in Separate Guide)

1. Create Azure free account ($200 credit)
2. Create Azure App Service for backend API
3. Create Azure Database for PostgreSQL
4. Configure connection strings in Azure
5. Deploy API to Azure App Service
6. Update mobile app to point to Azure API URL
7. Test complete cloud deployment

### 10.3 Cost Savings During Development

| Phase | Infrastructure | Monthly Cost |
|-------|---------------|--------------|
| **Local Development** | Your computer only | $0 |
| **Azure Free Trial** | Azure free credits | $0 (for 12 months) |
| **Azure Production (MVP)** | Pay-as-you-go | $42-76/month |

**Recommendation:** Stay on local development for 1-2 months while building features, then migrate to Azure free trial.

---

## 11. Quick Start Commands Reference

### Start Everything (macOS/Linux)

```bash
# Terminal 1: Start Database (if using Docker)
docker-compose -f docker-compose.dev.yml up -d

# Terminal 2: Start Backend API
cd src/LunarCalendar.Api && dotnet watch run

# Terminal 3: Start iOS App
cd src/LunarCalendar.MobileApp && dotnet build -t:Run -f net8.0-ios

# Terminal 4: Start Android App
cd src/LunarCalendar.MobileApp && dotnet build -t:Run -f net8.0-android
```

### Stop Everything

```bash
# Stop API: Ctrl+C in Terminal 2
# Stop mobile apps: Ctrl+C in Terminals 3 & 4
# Stop Docker database
docker-compose -f docker-compose.dev.yml down
```

---

## 12. Learning Resources

### Official Documentation
- [.NET MAUI Documentation](https://docs.microsoft.com/dotnet/maui/)
- [ASP.NET Core Web API](https://docs.microsoft.com/aspnet/core/web-api/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)

### Video Tutorials
- [.NET MAUI for Beginners](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oUBAdL2NwBpDs32zwGqb9DY)
- [Building Web APIs with ASP.NET Core](https://www.youtube.com/playlist?list=PL4WEkbdagHIQVbiTwos0E38VghMJA06OT)

### Community Support
- [Stack Overflow - .NET MAUI](https://stackoverflow.com/questions/tagged/.net-maui)
- [.NET Discord Community](https://discord.gg/dotnet)
- [r/dotnet on Reddit](https://reddit.com/r/dotnet)

---

## 13. Troubleshooting Guide

### Issue: PostgreSQL won't start

**Solution:**
```bash
# macOS
brew services restart postgresql@15

# Windows
# Open Services app, find PostgreSQL, restart service

# Check logs
tail -f /usr/local/var/log/postgres.log  # macOS
# Windows: Check Event Viewer
```

### Issue: Cannot access https://localhost:5001

**Solution:**
```bash
# Check if port is in use
lsof -i :5001  # macOS/Linux
netstat -ano | findstr :5001  # Windows

# Try HTTP instead
http://localhost:5000/swagger

# Re-trust certificates
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### Issue: Android emulator is slow

**Solution:**
- Enable Hardware Acceleration (HAXM on Windows, Hypervisor.Framework on Mac)
- Allocate more RAM to emulator (edit AVD settings)
- Use a lower API level device (API 30 instead of 33)
- Consider using a physical device for testing

### Issue: iOS build fails

**Solution:**
- Update Xcode to latest version
- Accept Xcode license: `sudo xcodebuild -license`
- Clean build: `dotnet clean && dotnet build`
- Restart Visual Studio

---

## 14. Next Steps

After completing local setup:

1. **Week 1-2:** Set up local environment, run sample API endpoints
2. **Week 3-4:** Implement Sprint 1 tasks (infrastructure setup)
3. **Week 5-6:** Implement Sprint 2 tasks (guest mode)
4. **Week 7-8:** Test everything locally with emulators
5. **Week 9:** Deploy to Azure and test on real devices

**Remember:** Perfect is the enemy of good. Get something working locally first, then iterate and improve!

---

**Document Control**

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-11 | Initial | Initial local development setup guide |
