# Development Roadmap
## Lunar Calendar Mobile Application

**Version:** 2.0
**Date:** 2025-12-26
**Project:** Lunar Calendar App

---

## 1. Overview

This roadmap outlines the phased development approach for the Lunar Calendar mobile application, focusing on delivering a Minimum Viable Product (MVP) followed by authentic cultural enhancements and social features.

### 1.1 Timeline Summary
- **Phase 1 (MVP)**: 16 weeks (4 months) - Sprints 1-8
- **Phase 2 (Authentic Lunar Calendar Features)**: 18 weeks (4.5 months) - Sprints 9-17
- **Phase 3 (Event Management & Social Features)**: 16 weeks (4 months) - Sprints 18-25
- **Total Duration**: 50 weeks (~12.5 months)

### 1.2 Team Structure
- **1 Backend Developer**: ASP.NET Core API, database, DevOps
- **1 Mobile Developer**: .NET MAUI iOS/Android apps
- **1 Full-Stack Developer**: Support both frontend and backend
- **1 QA/Tester** (part-time): Testing and quality assurance

---

## 2. Phase 1: Minimum Viable Product (MVP)

**Goal**: Deliver a functional cross-platform mobile calendar app with core features for both Gregorian and Lunar calendars.

### 2.1 Sprint 1: Project Setup & Infrastructure (2 weeks)

#### Backend Tasks
- [ ] Set up development environment and tools
- [ ] Create ASP.NET Core Web API project (.NET 8)
- [ ] Configure project structure and folder organization
- [ ] Set up PostgreSQL database (local and development environments)
- [ ] Configure Entity Framework Core with code-first migrations
- [ ] Create initial database schema (Users, Events, Categories tables)
- [ ] Set up Serilog for structured logging
- [ ] Configure Docker and Docker Compose files
- [ ] Set up Git repository and branching strategy
- [ ] Configure CI/CD pipeline basics (GitHub Actions or Azure DevOps)

#### Mobile Tasks
- [ ] Create .NET MAUI project for iOS and Android
- [ ] Set up project structure (MVVM pattern)
- [ ] Configure dependency injection (MauiProgram.cs)
- [ ] Set up navigation using Shell
- [ ] Configure platform-specific settings
- [ ] Set up local SQLite database for offline storage
- [ ] Create basic app theme and color scheme
- [ ] Test app deployment on iOS and Android emulators/devices

#### Deliverables
- Backend API skeleton running in Docker
- Mobile app builds successfully on both platforms
- Database schema created with migrations
- Version control and CI/CD pipeline operational

---

### 2.2 Sprint 2: Guest Mode & Welcome Flow (2 weeks)

#### Mobile Tasks
- [ ] Create UserMode enum and IUserModeService interface
- [ ] Implement UserModeService for managing guest/auth state
- [ ] Create WelcomePage UI with three options:
  - "Continue as Guest" button
  - "Sign In" button
  - "Sign Up" button
- [ ] Implement WelcomeViewModel with navigation commands
- [ ] Configure Shell navigation for welcome/auth flow
- [ ] Implement mode persistence in Preferences
- [ ] Create UpgradePromptHelper for contextual prompts
- [ ] Add mode detection on app startup
- [ ] Test guest mode calendar navigation
- [ ] Test mode switching between guest and authenticated

#### Backend Tasks
- [ ] Configure calendar endpoints to work without authentication
- [ ] Implement optional authentication middleware
- [ ] Add IP-based rate limiting for unauthenticated endpoints
- [ ] Document guest-accessible endpoints in Swagger

#### Deliverables
- Users can choose to continue as guest or authenticate
- Guest users can access calendar viewing features
- App remembers user's mode preference
- Smooth navigation between guest and auth modes

---

### 2.3 Sprint 3: User Authentication (2 weeks)

#### Backend Tasks
- [ ] Implement User entity and repository
- [ ] Create authentication service with JWT token generation
- [ ] Implement password hashing with BCrypt
- [ ] Create registration endpoint (`POST /api/auth/register`)
- [ ] Create login endpoint (`POST /api/auth/login`)
- [ ] Create refresh token endpoint (`POST /api/auth/refresh`)
- [ ] Implement refresh token storage and validation
- [ ] Add input validation using FluentValidation
- [ ] Create authentication middleware
- [ ] Write unit tests for authentication service
- [ ] Configure Swagger with JWT authentication

#### Mobile Tasks
- [ ] Create login page UI (XAML)
- [ ] Create registration page UI (XAML)
- [ ] Implement LoginViewModel with form validation
- [ ] Implement RegistrationViewModel
- [ ] Create AuthService for API communication
- [ ] Implement secure token storage (Keychain/Keystore)
- [ ] Create API client with authentication headers
- [ ] Implement auto-login on app start for returning users
- [ ] Implement mode transition from guest to authenticated
- [ ] Add loading indicators and error handling
- [ ] Test authentication flow end-to-end
- [ ] Test guest-to-authenticated upgrade flow

#### Deliverables
- Users can register and log in from mobile app
- JWT tokens are securely stored and used
- Session management works correctly
- Guest users can seamlessly upgrade to authenticated accounts

---

### 2.4 Sprint 4: Basic Calendar Display (2 weeks)

#### Backend Tasks
- [ ] Implement lunar calendar calculation service
  - Research and integrate lunar calendar algorithm library
  - Support Chinese lunar calendar initially
- [ ] Create calendar conversion endpoint (`GET /api/calendar/convert`)
- [ ] Create lunar info endpoint (`GET /api/calendar/lunar-info`)
- [ ] Add caching for calendar calculations
- [ ] Write unit tests for calendar service
- [ ] Document API endpoints in Swagger

#### Mobile Tasks
- [ ] Design and implement monthly calendar view UI
- [ ] Create CalendarViewModel with month navigation
- [ ] Inject IUserModeService into CalendarViewModel
- [ ] Implement calendar grid layout (7 columns × 5-6 rows)
- [ ] Display Gregorian dates in calendar cells
- [ ] Display corresponding lunar dates below Gregorian dates
- [ ] Highlight current date
- [ ] Implement month navigation (previous/next buttons or swipe)
- [ ] Create CalendarService for date calculations
- [ ] Implement lunar date conversion display
- [ ] Add month/year header display
- [ ] Test calendar rendering for both guest and authenticated users
- [ ] Test calendar rendering on different screen sizes

#### Deliverables
- Calendar displays both Gregorian and lunar dates
- Users can navigate between months
- Lunar date conversion works correctly
- Calendar accessible to both guest and authenticated users

---

### 2.5 Sprint 5: Cultural Enhancements - Holidays & Visuals (2 weeks)

#### Backend Tasks
- [ ] Research and compile Vietnamese lunar holidays database
- [ ] Research and compile traditional days (Tết, Mid-Autumn Festival, Wandering Souls' Day, etc.)
- [ ] Create Holiday entity and repository (name, date, type, color, description)
- [ ] Implement holiday endpoints:
  - `GET /api/holidays` (get all holidays)
  - `GET /api/holidays/year/{year}` (get holidays for specific year)
  - `GET /api/holidays/month/{year}/{month}` (get holidays for specific month)
- [ ] Add holiday types (public holiday, traditional day, festival)
- [ ] Implement holiday color coding system
- [ ] Write tests for holiday service
- [ ] Document API endpoints in Swagger

#### Mobile Tasks
- [ ] Create HolidayService for fetching holiday data
- [ ] Integrate holiday display into calendar view
- [ ] Implement color-coding for different holiday types:
  - Red for major holidays (Tết, National Day)
  - Gold/Yellow for traditional festivals
  - Green for seasonal celebrations
- [ ] Add today's lunar date display in calendar header/view
- [ ] Design and implement app background with cultural imagery:
  - Dragon, phoenix, or other traditional animals
  - Ensure background doesn't interfere with readability
  - Make background optional in settings
- [ ] Create holiday detail view when date is tapped
- [ ] Cache holiday data locally in SQLite
- [ ] Add visual indicators (dots, colors, icons) for holidays on calendar cells
- [ ] Test holiday display for current and future years
- [ ] Test cultural background on different screen sizes
- [ ] Ensure accessibility with background images

#### Deliverables
- Vietnamese lunar holidays displayed on calendar
- Traditional days highlighted with distinct colors
- Cultural background imagery enhances app aesthetic
- Today's lunar date visible in calendar view
- Holiday information accessible to both guest and authenticated users

---

### 2.6 Sprint 6: UI Polish & User Experience (2 weeks)

#### Backend Tasks
- [ ] Optimize calendar and holiday API response times
- [ ] Add caching for frequently accessed data
- [ ] Implement API versioning
- [ ] Review and optimize database queries

#### Mobile Tasks
- [ ] Polish calendar UI/UX across all screens
- [ ] Add smooth animations and transitions
- [ ] Implement pull-to-refresh on calendar
- [ ] Add loading skeletons for better perceived performance
- [ ] Improve error messages and user feedback
- [ ] Add haptic feedback for interactions (iOS)
- [ ] Implement swipe gestures for month navigation
- [ ] Add settings page for:
  - Background image toggle
  - Color theme preferences
  - Calendar display options
  - Lunar calendar system selection
- [ ] Create onboarding flow for first-time users
- [ ] Add tooltips and help text for features
- [ ] Test user flows on both platforms
- [ ] Optimize app performance and memory usage
- [ ] Ensure consistent styling across screens

#### Deliverables
- Polished, intuitive user interface
- Smooth animations and transitions
- Comprehensive settings page
- First-time user onboarding
- Optimized app performance

---

### 2.7 Sprint 7: Offline Support & Synchronization (2 weeks)

#### Backend Tasks
- [ ] Optimize API response times
- [ ] Implement efficient bulk sync endpoint for holiday data
- [ ] Add last-modified timestamps to holiday entities
- [ ] Create sync endpoint for incremental updates
- [ ] Add sync status tracking

#### Mobile Tasks
- [ ] Implement offline mode detection
- [ ] Cache holiday data for offline access
- [ ] Ensure calendar works fully offline with cached data
- [ ] Implement background synchronization for holiday updates
- [ ] Add sync status indicators in UI
- [ ] Test offline calendar viewing
- [ ] Implement sync retry logic with exponential backoff
- [ ] Add manual sync trigger for holiday data updates

#### Deliverables
- App works fully offline with cached data
- Holiday data syncs automatically when online
- Calendar remains functional without internet connection

---

### 2.8 Sprint 8: Testing, Bug Fixes & MVP Release (2 weeks)

#### Backend Tasks
- [ ] Comprehensive security audit
- [ ] Performance testing and optimization
- [ ] Load testing with realistic user scenarios
- [ ] Fix all critical and high-priority bugs
- [ ] Complete API documentation
- [ ] Set up production environment
- [ ] Configure production database with backups
- [ ] Deploy to production server/cloud
- [ ] Set up monitoring and alerting
- [ ] Create admin documentation

#### Mobile Tasks
- [ ] End-to-end testing on real iOS devices
- [ ] End-to-end testing on real Android devices
- [ ] Test guest mode flow thoroughly
- [ ] Test authenticated user flow thoroughly
- [ ] Test guest-to-authenticated upgrade flow
- [ ] Test on various screen sizes and OS versions
- [ ] Fix all critical and high-priority bugs
- [ ] Performance optimization (app size, startup time, memory)
- [ ] Accessibility testing and improvements
- [ ] Prepare app store assets (screenshots showing both modes, descriptions)
- [ ] Highlight guest mode as key feature in store listings
- [ ] Submit to Apple App Store for review
- [ ] Submit to Google Play Store for review
- [ ] Create user documentation/help section

#### Deliverables
- **MVP Release**: Fully functional app with core features
- Apps published to App Store and Google Play
- Backend API deployed and monitored
- Documentation complete

---

## 3. Phase 2: Authentic Lunar Calendar Features

**Goal**: Transform the app from a basic calendar converter into a comprehensive Vietnamese/Chinese lunar calendar with authentic astronomical and astrological information.

**Duration**: 18 weeks (9 sprints × 2 weeks)

---

### 3.1 Sprint 9: Sexagenary Cycle Foundation (2 weeks)

#### Backend Tasks
- [ ] Research and implement Sexagenary cycle (Can Chi / 干支) calculation algorithm
- [ ] Create data models for:
  - 10 Heavenly Stems (Thiên Can / 天干): Giáp, Ất, Bính, Đinh, Mậu, Kỷ, Canh, Tân, Nhâm, Quý
  - 12 Earthly Branches (Địa Chi / 地支): Tý, Sửu, Dần, Mão, Thìn, Tỵ, Ngọ, Mùi, Thân, Dậu, Tuất, Hợi
  - Five elements (Ngũ hành / 五行): Metal, Wood, Water, Fire, Earth
- [ ] Implement calculation for:
  - Year stem-branch (Năm Can Chi)
  - Month stem-branch (Tháng Can Chi)
  - Day stem-branch (Ngày Can Chi)
  - Hour stem-branch (Giờ Can Chi)
- [ ] Create API endpoints:
  - `GET /api/calendar/sexagenary/{date}` - Get full sexagenary info for a date
  - `GET /api/calendar/year-info/{year}` - Get year's zodiac animal and element
- [ ] Write comprehensive unit tests for cycle calculations
- [ ] Add localization support for stem-branch names (Vietnamese, Chinese, English)

#### Mobile Tasks
- [ ] Create SexagenaryService for fetching cycle data
- [ ] Design UI components for displaying:
  - Daily stem-branch (Ngày Can Chi) on calendar cells
  - Current day's full sexagenary info in header
  - Visual element indicators (五行 symbols or colors)
- [ ] Add "Today's Information" section showing:
  - Date in Gregorian, Lunar, and Sexagenary formats
  - Zodiac animal for the day
  - Five element association
- [ ] Implement caching for sexagenary data
- [ ] Add educational tooltips explaining what stem-branch means
- [ ] Test display on different screen sizes

#### Deliverables
- Full sexagenary cycle calculation working for any date
- Year, month, day, and hour stem-branch displayed
- Users can see traditional Chinese/Vietnamese date representation
- Educational information helps users understand the system

---

### 3.2 Sprint 10: Zodiac Animals & Year Characteristics (2 weeks)
### 3.3 Sprint 11: Dynamic Backgrounds Based on Zodiac Year (2 weeks)
### 3.4 Sprint 12: Moon Phases & Lunar Day Information (2 weeks)
### 3.5 Sprint 13: Additional Lunar Calendar Systems (2 weeks)
### 3.6 Sprint 14: Vietnamese & Chinese Localization (2 weeks)
### 3.7 Sprint 15: Auspicious Dates & Chinese Almanac (2 weeks)
### 3.8 Sprint 16: Hour-based Zodiac & Time Selection (2 weeks)
### 3.9 Sprint 17: Solar Terms & Agricultural Calendar (2 weeks)

**For complete sprint details including backend tasks, mobile tasks, and deliverables for Sprints 10-17, see [PHASE2_PHASE3_PLAN.md](../PHASE2_PHASE3_PLAN.md)**

---

## Phase 2 Summary

At the end of Phase 2, the app will be a **comprehensive, culturally authentic Vietnamese/Chinese lunar calendar** with:

✅ **Astronomical Features:**
- Complete sexagenary cycle (Can Chi / 干支)
- 12 zodiac animals with rich information
- Moon phases and lunar day characteristics
- 24 solar terms and agricultural calendar

✅ **Astrological Features:**
- Chinese almanac (Huangli / Lịch Vạn Niên)
- Auspicious date finder
- Hour-based luck ratings
- Daily recommendations and warnings

✅ **Cultural Features:**
- Dynamic zodiac backgrounds that change with lunar year
- Vietnamese and Chinese calendar systems
- Full localization (Vietnamese, Chinese, English)
- Traditional farming wisdom

✅ **User Experience:**
- Beautiful, culturally appropriate design
- Dark mode support
- Excellent performance
- Accessibility features

---

## 4. Phase 3: Event Management & Social Features

**Goal**: Add personal event management and social features to complement the rich cultural calendar foundation.

**Duration**: 14 weeks (7 sprints × 2 weeks)

**Note**: For complete details of Phase 2 (Authentic Lunar Calendar Features) and Phase 3 (Event Management & Social Features), see [PHASE2_PHASE3_PLAN.md](../PHASE2_PHASE3_PLAN.md)

### Key Phase 3 Features:
- Event Management (Create, Edit, Delete with Lunar date support)
- Smart Scheduling with Almanac Integration
- Event Reminders with Cultural Context
- Recurring Events (Gregorian and Lunar)
- Calendar Sharing & Family Features
- Export, Backup & Data Portability
- Home Screen Widgets & Quick Actions

---

## 5. Post-Phase 3: Continuous Improvement

### 5.1 Ongoing Activities
- Monitor user feedback and reviews
- Analyze usage analytics
- Fix bugs and issues
- Maintain dependencies and security updates
- Optimize based on performance metrics
- Plan future features based on user demand

### 5.2 Potential Future Features (Phase 4+)
- **Advanced Astrology**: Birth chart (Tứ Trụ / 四柱), Feng Shui calculator
- **Platform Expansion**: Apple Watch, Android Wear, iPad optimization, Web app
- **Calendar Integration**: Google Calendar, Apple Calendar, Outlook sync
- **AI Features**: Smart event suggestions, voice input, natural language date parsing
- **Premium Features**: Professional astrology consultations, detailed compatibility reports
- **Community**: Share almanac interpretations, regional customs, expert Q&A

---

## 6. Risk Management

### 6.1 Technical Risks

| Risk | Impact | Mitigation |
|------|--------|------------|
| Lunar calendar calculation accuracy | High | Use well-tested libraries, validate with multiple sources |
| Cross-platform compatibility issues | Medium | Regular testing on both platforms, use MAUI best practices |
| API performance under load | High | Load testing, caching, database optimization |
| Data sync conflicts | Medium | Implement robust conflict resolution, server-wins strategy |
| Third-party service downtime | Low | Implement fallbacks, graceful degradation |

### 6.2 Schedule Risks

| Risk | Impact | Mitigation |
|------|--------|------------|
| Underestimated complexity | Medium | Add 20% buffer to timeline, prioritize ruthlessly |
| Team member unavailability | Medium | Cross-train team members, maintain documentation |
| App store approval delays | Low | Submit early, follow guidelines strictly |
| Dependency on external APIs | Low | Choose reliable providers, have alternatives |

### 6.3 Business Risks

| Risk | Impact | Mitigation |
|------|--------|------------|
| Low user adoption | High | User research, beta testing, marketing strategy |
| Competing apps | Medium | Differentiation through unique features, quality |
| Privacy/security concerns | High | Follow best practices, regular security audits |
| Platform policy changes | Low | Stay updated on guidelines, adapt quickly |

---

## 7. Success Metrics

### 7.1 MVP Success Criteria
- Successfully deployed to both app stores
- 100 beta users testing the app (mix of guest and authenticated users)
- < 5% crash rate
- Average app rating > 4.0 stars
- Core features working as specified
- Guest mode adoption > 50% of new users
- Guest-to-authenticated conversion rate > 20%
- < 2 second average API response time
- 99% uptime for backend services

### 7.2 Phase 2 Success Criteria
- 1,000+ active users
- < 2% crash rate
- Average app rating > 4.5 stars
- All planned features implemented
- User retention > 40% after 30 days
- Positive user feedback on new features

### 7.3 Phase 3 Success Criteria
- All event management features adopted by > 60% of users
- Calendar sharing used by > 30% of users
- Widget installation > 40% of users
- Smart scheduling (almanac integration) used > 50% of time
- App rating maintained at 4.7+
- User retention > 60% after 30 days
- Monthly active users growing 15%+ month-over-month

---

## 8. Sprint Schedule Overview

| Sprint | Duration | Focus Area | Phase |
|--------|----------|------------|-------|
| Sprint 1 | 2 weeks | Project Setup & Infrastructure | MVP |
| Sprint 2 | 2 weeks | Guest Mode & Welcome Flow | MVP |
| Sprint 3 | 2 weeks | User Authentication | MVP |
| Sprint 4 | 2 weeks | Basic Calendar Display | MVP |
| Sprint 5 | 2 weeks | Cultural Enhancements - Holidays & Visuals | MVP |
| Sprint 6 | 2 weeks | UI Polish & User Experience | MVP |
| Sprint 7 | 2 weeks | Offline Support & Synchronization | MVP |
| Sprint 8 | 2 weeks | Testing, Bug Fixes & MVP Release | MVP |
| Sprint 9 | 2 weeks | Sexagenary Cycle Foundation | Phase 2 |
| Sprint 10 | 2 weeks | Zodiac Animals & Year Characteristics | Phase 2 |
| Sprint 11 | 2 weeks | Dynamic Backgrounds Based on Zodiac Year | Phase 2 |
| Sprint 12 | 2 weeks | Moon Phases & Lunar Day Information | Phase 2 |
| Sprint 13 | 2 weeks | Additional Lunar Calendar Systems | Phase 2 |
| Sprint 14 | 2 weeks | Vietnamese & Chinese Localization | Phase 2 |
| Sprint 15 | 2 weeks | Auspicious Dates & Chinese Almanac | Phase 2 |
| Sprint 16 | 2 weeks | Hour-based Zodiac & Time Selection | Phase 2 |
| Sprint 17 | 2 weeks | Solar Terms & Agricultural Calendar | Phase 2 |
| Sprint 18 | 2 weeks | Event Management - Create & View | Phase 3 |
| Sprint 19 | 2 weeks | Event Management - Edit, Delete & Categories | Phase 3 |
| Sprint 20 | 2 weeks | Search, Filtering & Event Intelligence | Phase 3 |
| Sprint 21 | 2 weeks | Event Reminders & Notifications | Phase 3 |
| Sprint 22 | 2 weeks | Recurring Events | Phase 3 |
| Sprint 23 | 2 weeks | Calendar Sharing & Family Features | Phase 3 |
| Sprint 24 | 2 weeks | Export, Backup & Data Portability | Phase 3 |
| Sprint 25 | 2 weeks | Home Screen Widgets & Quick Actions | Phase 3 |

**Total Duration**: 50 weeks (~12.5 months)

---

## 9. Dependencies and Prerequisites

### 9.1 Before Phase 1
- Development team assembled and onboarded
- Development tools and licenses acquired
- Cloud hosting account set up (Azure/AWS/GCP)
- Domain name registered
- SSL certificates obtained
- Project management tools configured (Jira, GitHub, etc.)
- Design assets and branding finalized

### 9.2 Cost Estimates

#### Initial Setup Costs (One-Time)

| Item | Description | Estimated Cost |
|------|-------------|----------------|
| **Development Tools** | | |
| Visual Studio Professional | 2-4 licenses for team | $2,100 - $4,200 ($45/mo × 12 × 2-4) or use VS Community (Free) |
| GitHub Team | Version control & CI/CD | $48/year ($4/user/month × 1 user) or use Free tier |
| Design Tools | Figma/Adobe XD for UI/UX | $144 - $600/year |
| **Apple Developer** | | |
| Apple Developer Program | Required for App Store | $99/year |
| **Google Developer** | | |
| Google Play Console | One-time registration | $25 one-time |
| **Domain & SSL** | | |
| Domain Registration | lunarcalendar.com | $12 - $50/year |
| SSL Certificate | Let's Encrypt | $0 (Free) |
| **Cloud Services Setup** | | |
| Initial Cloud Credits | Azure/AWS/GCP new account credits | $200 - $300 (Free credits) |
| **Total Initial Setup** | | **$428 - $5,286/year** |

*Note: Using free tiers and VS Community can reduce initial costs to ~$136/year (Apple Dev $99 + Google Play $25 + Domain $12)*

---

#### Monthly Operating Costs - MVP Phase (100-500 users)

| Service | Configuration | Monthly Cost |
|---------|--------------|--------------|
| **Cloud Hosting (Azure - Recommended)** | | |
| App Service (API) | Basic B1 (1.75 GB RAM, 1 core) | $13 |
| PostgreSQL Database | Basic tier, 2 vCores, 5 GB storage | $25 |
| Blob Storage | Standard tier for backups/logs | $1 - $3 |
| Application Insights | Monitoring & analytics | $0 - $5 |
| CDN (optional) | Azure CDN Standard | $0 - $10 |
| **Push Notifications** | | |
| Azure Notification Hubs | 10M pushes/month (Free tier) | $0 |
| **Email Service** | | |
| SendGrid | 100 emails/day (Free tier) | $0 |
| **Domain & DNS** | | |
| Azure DNS or Cloudflare | DNS hosting | $1 - $5 |
| **Backup & Disaster Recovery** | | |
| Automated Backups | Database backups | $2 - $5 |
| **Monitoring & Logging** | | |
| Log Analytics | 5 GB/month included | $0 - $10 |
| **Total MVP Monthly** | | **$42 - $76/month** |

---

#### Monthly Operating Costs - Growth Phase (1,000-5,000 users)

| Service | Configuration | Monthly Cost |
|---------|--------------|--------------|
| **Cloud Hosting (Azure)** | | |
| App Service (API) | Standard S1 (1.75 GB RAM, 1 core) with auto-scaling | $70 - $140 |
| PostgreSQL Database | General Purpose, 4 vCores, 32 GB storage | $185 |
| Blob Storage | Standard tier, increased usage | $5 - $15 |
| Application Insights | Increased telemetry | $10 - $30 |
| CDN | Azure CDN with traffic | $20 - $50 |
| **Push Notifications** | | |
| Azure Notification Hubs | 100M pushes/month | $10 |
| **Email Service** | | |
| SendGrid | Essential plan (50K emails/month) | $20 |
| **Domain & DNS** | | |
| DNS with increased queries | DNS hosting | $5 |
| **Backup & Disaster Recovery** | | |
| Automated Backups + Geo-redundancy | Enhanced backup strategy | $20 - $40 |
| **Security** | | |
| Azure DDoS Protection (optional) | Basic tier included | $0 - $30 |
| **Monitoring & Logging** | | |
| Log Analytics & Monitoring | Increased data ingestion | $20 - $50 |
| **Total Growth Monthly** | | **$365 - $575/month** |

---

#### Monthly Operating Costs - Scale Phase (10,000+ users)

| Service | Configuration | Monthly Cost |
|---------|--------------|--------------|
| **Cloud Hosting (Azure)** | | |
| App Service (API) | Premium P1V2 with auto-scaling (2-4 instances) | $400 - $800 |
| PostgreSQL Database | General Purpose, 8 vCores, 128 GB storage + read replicas | $600 - $900 |
| Redis Cache | Standard tier for caching | $75 - $150 |
| Blob Storage | Standard tier, high usage | $30 - $60 |
| Application Insights | Enterprise telemetry | $50 - $100 |
| CDN | Premium CDN with global traffic | $100 - $200 |
| **Push Notifications** | | |
| Azure Notification Hubs | 1B+ pushes/month | $200 |
| **Email Service** | | |
| SendGrid | Pro plan (1.5M emails/month) | $90 |
| **Domain & DNS** | | |
| Premium DNS | High-availability DNS | $10 - $20 |
| **Backup & Disaster Recovery** | | |
| Enterprise Backup & DR | Geo-redundant, point-in-time recovery | $100 - $200 |
| **Security** | | |
| Azure DDoS Protection Standard | DDoS protection | $30 - $50 |
| Web Application Firewall | Security layer | $50 - $100 |
| **Monitoring & Logging** | | |
| Comprehensive monitoring | Advanced analytics | $100 - $200 |
| **Total Scale Monthly** | | **$1,835 - $2,980/month** |

---

#### Annual Cost Summary

| Phase | User Range | Monthly Cost | Annual Cost |
|-------|-----------|--------------|-------------|
| **Initial Setup** | - | - | $428 - $5,286 |
| **MVP** | 100-500 users | $42 - $76 | $504 - $912 |
| **Growth** | 1,000-5,000 users | $365 - $575 | $4,380 - $6,900 |
| **Scale** | 10,000+ users | $1,835 - $2,980 | $22,020 - $35,760 |

---

#### Alternative Cloud Providers Comparison

**AWS (Amazon Web Services)**
- MVP: $50-90/month (EC2 + RDS + S3)
- Growth: $400-650/month
- Scale: $2,000-3,200/month
- Pros: Mature ecosystem, excellent documentation
- Cons: Complex pricing, can get expensive quickly

**Google Cloud Platform (GCP)**
- MVP: $45-85/month (Cloud Run + Cloud SQL + Cloud Storage)
- Growth: $380-600/month
- Scale: $1,900-3,100/month
- Pros: Strong free tier, good for serverless
- Cons: Smaller ecosystem than AWS/Azure

**DigitalOcean (Budget Option)**
- MVP: $30-60/month (Droplets + Managed Database)
- Growth: $200-350/month
- Scale: $800-1,500/month
- Pros: Simple pricing, easy to use, predictable costs
- Cons: Fewer managed services, manual scaling

---

#### Cost Optimization Strategies

**MVP Phase:**
1. Use Azure free credits ($200-300)
2. Start with Basic tier services
3. Use free tiers for SendGrid, Notification Hubs
4. Implement caching early to reduce database load
5. Use VS Community Edition (free)
6. Consider DigitalOcean for lower costs

**Growth Phase:**
1. Enable auto-scaling to optimize resource usage
2. Implement Redis caching to reduce database queries
3. Use CDN to reduce bandwidth costs
4. Optimize database queries and add indexes
5. Monitor and eliminate unused resources
6. Consider reserved instances for 1-year commitment (30% savings)

**Scale Phase:**
1. Use reserved instances for 3-year commitment (50% savings)
2. Implement multi-region deployment strategically
3. Optimize image and asset delivery
4. Use database read replicas efficiently
5. Implement comprehensive caching strategy
6. Consider Kubernetes for better resource utilization

---

#### Development Team Costs (Not Included Above)

| Role | Quantity | Monthly Cost (Contract/Freelance) | Monthly Cost (Full-Time) |
|------|----------|-----------------------------------|--------------------------|
| Senior Backend Developer | 1 | $8,000 - $12,000 | $10,000 - $15,000 |
| Mobile Developer (.NET MAUI) | 1 | $7,000 - $11,000 | $9,000 - $13,000 |
| Full-Stack Developer | 1 | $7,000 - $10,000 | $8,500 - $12,000 |
| QA/Tester (part-time) | 0.5 | $2,500 - $4,000 | $3,000 - $5,000 |
| **Total Team Cost** | **2-4 people** | **$24,500 - $37,000/month** | **$30,500 - $45,000/month** |

**Note:** Team costs vary significantly by location. Consider offshore development for 40-60% cost reduction.

---

#### Hidden/Miscellaneous Costs

| Item | Frequency | Estimated Cost |
|------|-----------|----------------|
| Third-party API integrations | Monthly | $0 - $100 |
| Marketing & User Acquisition | Monthly | $500 - $5,000 |
| App Store Optimization (ASO) | One-time | $500 - $2,000 |
| Legal (Privacy Policy, ToS) | One-time | $500 - $2,000 |
| Customer Support Tools | Monthly | $50 - $200 |
| Analytics Tools (Mixpanel, Amplitude) | Monthly | $0 - $300 |
| Code Quality Tools (SonarCloud) | Monthly | $0 - $100 |

---

#### Total Cost of Ownership (TCO) - First Year

**Minimal Budget Approach:**
- Initial Setup: $136 (free tools + essential licenses)
- Infrastructure (MVP): $504/year
- Development Team (offshore, 9 months): $90,000
- Miscellaneous: $2,000
- **Total Year 1: ~$92,640**

**Recommended Budget Approach:**
- Initial Setup: $2,500
- Infrastructure (MVP → Growth transition): $3,500/year
- Development Team (contract, 9 months): $220,000
- Marketing: $10,000
- Miscellaneous: $5,000
- **Total Year 1: ~$241,000**

**Premium Approach:**
- Initial Setup: $5,000
- Infrastructure (Growth → Scale): $8,000/year
- Development Team (full-time, 9 months): $340,000
- Marketing & UA: $30,000
- Legal, Support, Tools: $10,000
- **Total Year 1: ~$393,000**

---

### 9.3 Between Phases
- MVP user feedback collected and analyzed
- Analytics data reviewed
- Priority features for Phase 2 confirmed
- Additional resources secured if needed

---

## 10. Communication Plan

### 10.1 Daily
- Daily standup meetings (15 minutes)
- Async updates in team chat

### 10.2 Weekly
- Sprint planning (start of sprint)
- Sprint retrospective (end of sprint)
- Progress demo to stakeholders

### 10.3 Bi-weekly
- Product roadmap review
- Technical architecture review
- User feedback review session

---

## 11. Quality Assurance Strategy

### 11.1 Continuous Testing
- Automated unit tests (minimum 70% coverage)
- Integration tests for API endpoints
- UI tests for critical user flows
- Regression testing before each release

### 11.2 Manual Testing
- Exploratory testing each sprint
- Usability testing with real users
- Device compatibility testing
- Accessibility testing

### 11.3 Code Quality
- Code reviews for all pull requests
- Static code analysis tools
- Consistent coding standards
- Regular refactoring sessions

---

**Document Control**

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-11 | Initial | Initial development roadmap |
| 1.1 | 2025-12-25 | Updated | Reorganized MVP to prioritize cultural features (holidays, visuals) over event management. Moved event management (old Sprints 5-7) to Post-MVP phase. Added Sprint 5: Cultural Enhancements and Sprint 6: UI Polish. |
| 2.0 | 2025-12-26 | Updated | Major reorganization: Phase 2 now focuses on authentic lunar calendar features (Sexagenary cycle, zodiac animals, dynamic backgrounds, almanac, solar terms, localization). Phase 3 contains event management and social features. Updated timeline from 42 weeks to 50 weeks. Added detailed link to [PHASE2_PHASE3_PLAN.md](../PHASE2_PHASE3_PLAN.md) for complete sprint details. |

---

## Appendix A: Technology Learning Resources

### .NET MAUI
- [Official .NET MAUI Documentation](https://docs.microsoft.com/dotnet/maui/)
- [.NET MAUI Workshop](https://github.com/dotnet-presentations/dotnet-maui-workshop)

### ASP.NET Core
- [Official ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Building Web APIs](https://docs.microsoft.com/aspnet/core/web-api/)

### Entity Framework Core
- [EF Core Documentation](https://docs.microsoft.com/ef/core/)
- [Code-First Migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/)

### Docker
- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
