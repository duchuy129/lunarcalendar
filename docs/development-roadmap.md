# Development Roadmap
## Lunar Calendar Mobile Application

**Version:** 1.0
**Date:** 2025-12-11
**Project:** Lunar Calendar App

---

## 1. Overview

This roadmap outlines the phased development approach for the Lunar Calendar mobile application, focusing on delivering a Minimum Viable Product (MVP) followed by feature enhancements.

### 1.1 Timeline Summary
- **Phase 1 (MVP)**: 3-4 months
- **Phase 2 (Enhancements)**: 2-3 months
- **Total Duration**: 5-7 months

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

### 2.5 Sprint 5: Event Management - Create & View (2 weeks)

#### Backend Tasks
- [ ] Implement Event entity and repository
- [ ] Implement Category entity and repository
- [ ] Create event service with business logic
- [ ] Create events endpoints:
  - `POST /api/events` (create event)
  - `GET /api/events/{id}` (get single event)
  - `GET /api/events/range` (get events in date range)
  - `GET /api/events` (get all user events with pagination)
- [ ] Implement event validation (dates, required fields)
- [ ] Add authorization checks (users can only access own events)
- [ ] Write unit and integration tests
- [ ] Configure AutoMapper for DTOs

#### Mobile Tasks
- [ ] Add "Add Event" button to calendar view
- [ ] Implement guest mode check before event creation
- [ ] Show upgrade prompt when guest taps "Add Event"
- [ ] Create event creation form UI (for authenticated users)
- [ ] Create EventDetailViewModel
- [ ] Create CreateEventViewModel with validation
- [ ] Implement event form fields:
  - Title (required)
  - Description
  - Start date/time picker
  - End date/time picker
  - All-day toggle
  - Calendar type (Gregorian/Lunar)
  - Color picker
- [ ] Create EventService for API communication
- [ ] Display events on calendar dates (visual indicators)
- [ ] Create event detail page
- [ ] Implement date/time pickers
- [ ] Add event creation flow with navigation
- [ ] Cache events locally in SQLite for authenticated users
- [ ] Test upgrade prompt flow from guest to authenticated

#### Deliverables
- Authenticated users can create events from mobile app
- Guest users see upgrade prompt when attempting to create events
- Events are displayed on calendar for authenticated users
- Events sync with backend API
- Event details can be viewed

---

### 2.6 Sprint 6: Event Management - Edit & Delete (2 weeks)

#### Backend Tasks
- [ ] Create update event endpoint (`PUT /api/events/{id}`)
- [ ] Create delete event endpoint (`DELETE /api/events/{id}`)
- [ ] Implement soft delete for events (IsDeleted flag)
- [ ] Add event modification validation
- [ ] Ensure authorization for edit/delete operations
- [ ] Write tests for update and delete operations
- [ ] Add audit logging for event changes

#### Mobile Tasks
- [ ] Add edit button to event detail page
- [ ] Create edit event form (reuse create form)
- [ ] Implement EditEventViewModel
- [ ] Add delete button with confirmation dialog
- [ ] Implement delete functionality
- [ ] Update local cache on edit/delete
- [ ] Handle optimistic updates in UI
- [ ] Add undo functionality for delete (optional)
- [ ] Test edit/delete flows

#### Deliverables
- Users can edit existing events
- Users can delete events with confirmation
- Changes sync with backend
- Local cache stays synchronized

---

### 2.7 Sprint 7: Event Categories & Polish (2 weeks)

#### Backend Tasks
- [ ] Create category endpoints:
  - `GET /api/categories` (get all user categories)
  - `POST /api/categories` (create category)
  - `PUT /api/categories/{id}` (update category)
  - `DELETE /api/categories/{id}` (delete category)
- [ ] Implement default categories on user registration
- [ ] Add category filtering to events endpoint
- [ ] Write tests for category operations

#### Mobile Tasks
- [ ] Create category management UI
- [ ] Add category selector to event form
- [ ] Display event colors on calendar based on category
- [ ] Implement category list page
- [ ] Add category creation/editing
- [ ] Create CategoryService
- [ ] Polish UI/UX across all screens
- [ ] Add animations and transitions
- [ ] Implement pull-to-refresh on calendar
- [ ] Add loading skeletons
- [ ] Improve error messages and handling

#### Deliverables
- Users can create and manage event categories
- Events can be assigned to categories
- Calendar displays events with category colors
- UI is polished and user-friendly

---

### 2.8 Sprint 8: Offline Support & Synchronization (2 weeks)

#### Backend Tasks
- [ ] Optimize API response times
- [ ] Implement efficient bulk sync endpoint
- [ ] Add last-modified timestamps to all entities
- [ ] Create sync endpoint for incremental updates
- [ ] Implement conflict resolution logic (server wins)
- [ ] Add sync status tracking

#### Mobile Tasks
- [ ] Implement offline mode detection
- [ ] Queue API requests when offline
- [ ] Sync queued requests when online
- [ ] Implement background synchronization
- [ ] Add sync status indicators in UI
- [ ] Handle sync conflicts gracefully
- [ ] Test offline create/edit/delete operations
- [ ] Implement sync retry logic with exponential backoff
- [ ] Add manual sync trigger

#### Deliverables
- App works fully offline with local data
- Changes sync automatically when online
- Sync conflicts are handled appropriately

---

### 2.9 Sprint 9: Testing, Bug Fixes & MVP Release (2 weeks)

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
- Guest mode working seamlessly without barriers
- Authenticated mode with full event management
- Apps published to App Store and Google Play
- Backend API deployed and monitored
- Documentation complete

---

## 3. Phase 2: Feature Enhancements

**Goal**: Add advanced features and improvements based on user feedback and roadmap priorities.

### 3.1 Sprint 10: Search & Filtering (2 weeks)

#### Backend Tasks
- [ ] Create event search endpoint with full-text search
- [ ] Implement advanced filtering (date range, category, calendar type)
- [ ] Optimize search queries with database indexing
- [ ] Add search result pagination
- [ ] Implement search suggestions/autocomplete

#### Mobile Tasks
- [ ] Create search UI with search bar
- [ ] Implement SearchViewModel
- [ ] Add filter options (bottom sheet or modal)
- [ ] Display search results in list view
- [ ] Add search history
- [ ] Implement real-time search (debounced)
- [ ] Test search performance

#### Deliverables
- Users can search events by title/description
- Advanced filtering by multiple criteria
- Fast and responsive search experience

---

### 3.2 Sprint 11: Event Reminders & Notifications (2 weeks)

#### Backend Tasks
- [ ] Implement push notification service integration
  - Configure APNs for iOS
  - Configure FCM for Android
- [ ] Create notification scheduling service
- [ ] Create endpoints for device token registration
- [ ] Implement reminder notification logic
- [ ] Add background job for sending reminders
- [ ] Test notification delivery

#### Mobile Tasks
- [ ] Request notification permissions
- [ ] Register device token with backend
- [ ] Add reminder time picker to event form
- [ ] Implement local notifications as fallback
- [ ] Handle notification taps (deep linking to event)
- [ ] Add notification settings page
- [ ] Allow users to customize reminder defaults
- [ ] Test notifications on iOS and Android

#### Deliverables
- Users receive reminders for upcoming events
- Push notifications work on both platforms
- Users can customize notification preferences

---

### 3.3 Sprint 12: Recurring Events (2 weeks)

#### Backend Tasks
- [ ] Design recurrence rule schema (iCalendar RRULE format)
- [ ] Implement recurrence calculation service
- [ ] Update event endpoints to support recurrence
- [ ] Create endpoint to get single instance or series
- [ ] Implement logic for editing single vs. all instances
- [ ] Write tests for various recurrence patterns

#### Mobile Tasks
- [ ] Design recurring event UI
- [ ] Add recurrence pattern selector (daily, weekly, monthly, yearly)
- [ ] Add recurrence end options (never, after N times, until date)
- [ ] Update event detail page to show recurrence info
- [ ] Implement edit options (this event, all events, future events)
- [ ] Display recurring events correctly on calendar
- [ ] Test various recurrence scenarios

#### Deliverables
- Users can create recurring events
- Recurrence patterns are displayed correctly
- Users can edit individual or all instances

---

### 3.4 Sprint 13: Additional Lunar Calendar Systems (2 weeks)

#### Backend Tasks
- [ ] Research and implement Vietnamese lunar calendar
- [ ] Research and implement Islamic (Hijri) calendar
- [ ] Create configuration for multiple calendar systems
- [ ] Update calendar service to support multiple systems
- [ ] Add calendar system selection to user settings
- [ ] Test accuracy of all calendar conversions

#### Mobile Tasks
- [ ] Add calendar system selector to settings
- [ ] Update calendar display for different systems
- [ ] Show calendar-specific information (zodiac, moon phase, etc.)
- [ ] Add localization for calendar names
- [ ] Update UI to accommodate different calendar formats
- [ ] Test with users familiar with each calendar system

#### Deliverables
- Support for Chinese, Vietnamese, and Islamic calendars
- Users can select preferred calendar system
- Accurate date conversions for all systems

---

### 3.5 Sprint 14: Localization & Internationalization (2 weeks)

#### Backend Tasks
- [ ] Set up localization infrastructure
- [ ] Create resource files for multiple languages
- [ ] Translate API error messages
- [ ] Support multiple languages in responses
- [ ] Add language selection to user profile

#### Mobile Tasks
- [ ] Extract all UI strings to resource files
- [ ] Translate UI to target languages:
  - English (default)
  - Chinese (Simplified and Traditional)
  - Vietnamese
- [ ] Implement language switching in settings
- [ ] Test RTL languages if applicable
- [ ] Format dates/times according to locale
- [ ] Test all screens in each language
- [ ] Ensure layouts work with translated text

#### Deliverables
- App supports multiple languages
- Users can switch languages in settings
- All UI elements are properly translated

---

### 3.6 Sprint 15: Home Screen Widgets (2 weeks)

#### Backend Tasks
- [ ] Create optimized widget data endpoint
- [ ] Implement caching for widget data
- [ ] Ensure API performance for frequent widget refreshes

#### Mobile Tasks
- [ ] Design widget layouts for iOS (small, medium, large)
- [ ] Design widget layouts for Android
- [ ] Implement iOS widget using WidgetKit
- [ ] Implement Android widget using App Widgets
- [ ] Show today's events and lunar date in widget
- [ ] Add widget customization options
- [ ] Handle widget tap to open app
- [ ] Test widget updates and refresh
- [ ] Optimize widget performance and battery usage

#### Deliverables
- Users can add calendar widgets to home screen
- Widgets display current date and upcoming events
- Widgets update automatically

---

### 3.7 Sprint 16: Social Features & Sharing (2 weeks)

#### Backend Tasks
- [ ] Design shared calendar schema
- [ ] Implement calendar sharing service
- [ ] Create endpoints for inviting users
- [ ] Implement permissions (view-only, edit)
- [ ] Add shared events to event queries
- [ ] Implement notification for shared event updates

#### Mobile Tasks
- [ ] Create calendar sharing UI
- [ ] Implement invite flow (email or share code)
- [ ] Display shared calendars separately
- [ ] Show event ownership indicators
- [ ] Add sharing permissions UI
- [ ] Implement event export (iCal format)
- [ ] Add share event functionality (text, image)
- [ ] Test collaborative scenarios

#### Deliverables
- Users can share calendars with family/friends
- Shared events sync across all participants
- Events can be exported and shared externally

---

### 3.8 Sprint 17: Advanced Features & Integration (2 weeks)

#### Backend Tasks
- [ ] Implement holiday database for multiple countries
- [ ] Create holiday API endpoints
- [ ] Integrate weather API for event locations
- [ ] Implement auspicious dates calculation
- [ ] Add backup/restore functionality
- [ ] Create data export endpoint (JSON, CSV)

#### Mobile Tasks
- [ ] Display holidays on calendar
- [ ] Add weather information to events
- [ ] Implement auspicious dates feature
- [ ] Create backup/restore UI
- [ ] Add data export functionality
- [ ] Implement account deletion with data purge
- [ ] Add privacy settings
- [ ] Create comprehensive settings page

#### Deliverables
- Holidays are automatically displayed
- Weather integration for events
- Users can backup and restore data
- Additional cultural/traditional features

---

### 3.9 Sprint 18: Performance Optimization & Analytics (2 weeks)

#### Backend Tasks
- [ ] Implement Redis caching layer
- [ ] Optimize database queries with query analysis
- [ ] Set up database read replicas
- [ ] Implement API rate limiting per user
- [ ] Add application performance monitoring (APM)
- [ ] Optimize Docker image sizes
- [ ] Implement health check endpoints
- [ ] Add metrics collection (Prometheus/Grafana)

#### Mobile Tasks
- [ ] Implement app analytics (Firebase Analytics or AppCenter)
- [ ] Optimize image loading and caching
- [ ] Reduce app size (ProGuard, app bundles)
- [ ] Optimize battery usage
- [ ] Reduce memory footprint
- [ ] Implement crash reporting
- [ ] Add performance monitoring
- [ ] Optimize startup time

#### Deliverables
- Improved API performance
- Reduced mobile app size and resource usage
- Analytics and monitoring in place

---

### 3.10 Sprint 19: Final Polish & Phase 2 Release (2 weeks)

#### Backend Tasks
- [ ] Security audit and penetration testing
- [ ] Performance load testing
- [ ] Database optimization review
- [ ] Documentation updates
- [ ] Deployment automation improvements
- [ ] Disaster recovery testing

#### Mobile Tasks
- [ ] UI/UX polish based on user feedback
- [ ] Accessibility improvements (WCAG compliance)
- [ ] Final bug fixes
- [ ] App store optimization (ASO)
- [ ] Update screenshots and descriptions
- [ ] Submit app updates to stores
- [ ] Create release notes

#### Deliverables
- **Phase 2 Release**: Feature-complete app with enhancements
- Updated apps in App Store and Google Play
- All features tested and documented

---

## 4. Post-Phase 2: Continuous Improvement

### 4.1 Ongoing Activities
- Monitor user feedback and reviews
- Analyze usage analytics
- Fix bugs and issues
- Maintain dependencies and security updates
- Optimize based on performance metrics
- Plan Phase 3 features based on user demand

### 4.2 Potential Future Features
- **Local guest events** (Phase 3): Allow guests to create events stored locally only
- **Guest data migration**: Seamless migration of local guest events when upgrading
- Apple Watch and Android Wear companion apps
- iPad and tablet optimization
- Web application (Progressive Web App)
- Integration with Google Calendar, Apple Calendar, Outlook
- AI-powered event suggestions
- Voice input for event creation
- Advanced data visualization and insights
- Team/organization features
- Premium subscription features
- Third-party calendar plugins

---

## 5. Risk Management

### 5.1 Technical Risks

| Risk | Impact | Mitigation |
|------|--------|------------|
| Lunar calendar calculation accuracy | High | Use well-tested libraries, validate with multiple sources |
| Cross-platform compatibility issues | Medium | Regular testing on both platforms, use MAUI best practices |
| API performance under load | High | Load testing, caching, database optimization |
| Data sync conflicts | Medium | Implement robust conflict resolution, server-wins strategy |
| Third-party service downtime | Low | Implement fallbacks, graceful degradation |

### 5.2 Schedule Risks

| Risk | Impact | Mitigation |
|------|--------|------------|
| Underestimated complexity | Medium | Add 20% buffer to timeline, prioritize ruthlessly |
| Team member unavailability | Medium | Cross-train team members, maintain documentation |
| App store approval delays | Low | Submit early, follow guidelines strictly |
| Dependency on external APIs | Low | Choose reliable providers, have alternatives |

### 5.3 Business Risks

| Risk | Impact | Mitigation |
|------|--------|------------|
| Low user adoption | High | User research, beta testing, marketing strategy |
| Competing apps | Medium | Differentiation through unique features, quality |
| Privacy/security concerns | High | Follow best practices, regular security audits |
| Platform policy changes | Low | Stay updated on guidelines, adapt quickly |

---

## 6. Success Metrics

### 6.1 MVP Success Criteria
- Successfully deployed to both app stores
- 100 beta users testing the app (mix of guest and authenticated users)
- < 5% crash rate
- Average app rating > 4.0 stars
- Core features working as specified
- Guest mode adoption > 50% of new users
- Guest-to-authenticated conversion rate > 20%
- < 2 second average API response time
- 99% uptime for backend services

### 6.2 Phase 2 Success Criteria
- 1,000+ active users
- < 2% crash rate
- Average app rating > 4.5 stars
- All planned features implemented
- User retention > 40% after 30 days
- Positive user feedback on new features

---

## 7. Sprint Schedule Overview

| Sprint | Duration | Focus Area | Phase |
|--------|----------|------------|-------|
| Sprint 1 | 2 weeks | Project Setup & Infrastructure | MVP |
| Sprint 2 | 2 weeks | Guest Mode & Welcome Flow | MVP |
| Sprint 3 | 2 weeks | User Authentication | MVP |
| Sprint 4 | 2 weeks | Basic Calendar Display | MVP |
| Sprint 5 | 2 weeks | Event Management - Create & View | MVP |
| Sprint 6 | 2 weeks | Event Management - Edit & Delete | MVP |
| Sprint 7 | 2 weeks | Event Categories & Polish | MVP |
| Sprint 8 | 2 weeks | Offline Support & Synchronization | MVP |
| Sprint 9 | 2 weeks | Testing, Bug Fixes & MVP Release | MVP |
| Sprint 10 | 2 weeks | Search & Filtering | Enhancement |
| Sprint 11 | 2 weeks | Event Reminders & Notifications | Enhancement |
| Sprint 12 | 2 weeks | Recurring Events | Enhancement |
| Sprint 13 | 2 weeks | Additional Lunar Calendar Systems | Enhancement |
| Sprint 14 | 2 weeks | Localization & Internationalization | Enhancement |
| Sprint 15 | 2 weeks | Home Screen Widgets | Enhancement |
| Sprint 16 | 2 weeks | Social Features & Sharing | Enhancement |
| Sprint 17 | 2 weeks | Advanced Features & Integration | Enhancement |
| Sprint 18 | 2 weeks | Performance Optimization & Analytics | Enhancement |
| Sprint 19 | 2 weeks | Final Polish & Phase 2 Release | Enhancement |

**Total Duration**: 38 weeks (~9.5 months)

---

## 8. Dependencies and Prerequisites

### 8.1 Before Phase 1
- Development team assembled and onboarded
- Development tools and licenses acquired
- Cloud hosting account set up (Azure/AWS/GCP)
- Domain name registered
- SSL certificates obtained
- Project management tools configured (Jira, GitHub, etc.)
- Design assets and branding finalized

### 8.2 Cost Estimates

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

### 8.3 Between Phases
- MVP user feedback collected and analyzed
- Analytics data reviewed
- Priority features for Phase 2 confirmed
- Additional resources secured if needed

---

## 9. Communication Plan

### 9.1 Daily
- Daily standup meetings (15 minutes)
- Async updates in team chat

### 9.2 Weekly
- Sprint planning (start of sprint)
- Sprint retrospective (end of sprint)
- Progress demo to stakeholders

### 9.3 Bi-weekly
- Product roadmap review
- Technical architecture review
- User feedback review session

---

## 10. Quality Assurance Strategy

### 10.1 Continuous Testing
- Automated unit tests (minimum 70% coverage)
- Integration tests for API endpoints
- UI tests for critical user flows
- Regression testing before each release

### 10.2 Manual Testing
- Exploratory testing each sprint
- Usability testing with real users
- Device compatibility testing
- Accessibility testing

### 10.3 Code Quality
- Code reviews for all pull requests
- Static code analysis tools
- Consistent coding standards
- Regular refactoring sessions

---

**Document Control**

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-11 | Initial | Initial development roadmap |

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
