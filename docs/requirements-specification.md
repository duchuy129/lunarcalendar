# Requirements Specification
## Lunar Calendar Mobile Application

**Version:** 1.0
**Date:** 2025-12-11
**Project:** Lunar Calendar App

---

## 1. Introduction

### 1.1 Purpose
This document outlines the requirements for a cross-platform mobile calendar application that displays both standard Gregorian and lunar calendar systems, targeting iOS and Android platforms.

### 1.2 Scope
The application will provide users with:
- Dual calendar view (Gregorian and Lunar)
- Event management capabilities
- Cross-platform mobile experience
- Cloud-based data synchronization via API backend

### 1.3 Definitions and Acronyms
- **Lunar Calendar**: Traditional calendar based on moon phases (e.g., Chinese, Vietnamese, Islamic calendars)
- **API**: Application Programming Interface
- **MAUI**: Multi-platform App UI (.NET framework)
- **MVP**: Minimum Viable Product

---

## 2. Use Cases

### UC-001: Continue as Guest
**Actor**: New User
**Description**: User chooses to use the app without creating an account
**Preconditions**: App is installed and launched for the first time
**Main Flow**:
1. User opens the application
2. System displays welcome screen with options: "Continue as Guest" or "Sign In/Sign Up"
3. User taps "Continue as Guest"
4. System creates local-only session
5. System navigates to main calendar view

**Postconditions**: User can access basic calendar features without authentication

---

### UC-002: View Monthly Calendar
**Actor**: Guest or Authenticated User
**Description**: User views calendar showing both Gregorian and lunar dates
**Preconditions**: App is installed and launched
**Main Flow**:
1. User opens the application (or is already in the app)
2. System displays current month with both calendar systems
3. User can swipe left/right to navigate between months
4. Lunar dates are displayed alongside Gregorian dates

**Postconditions**: Calendar is displayed correctly

---

### UC-003: Create Event (Guest)
**Actor**: Guest User
**Description**: Guest attempts to create a calendar event and is prompted to sign up
**Preconditions**: User is in guest mode
**Main Flow**:
1. User taps "Add Event" button
2. System displays upgrade prompt: "Sign up to create and sync events"
3. User can choose: "Sign Up", "Sign In", or "Cancel"
4. If user selects "Sign Up" or "Sign In", system navigates to authentication flow
5. If user selects "Cancel", system returns to calendar view

**Alternative Flow** (Future Enhancement):
1. User taps "Add Event" button
2. System displays event creation form with notice: "Guest mode - events stored locally only"
3. User creates event
4. System stores event in local SQLite database only
5. System displays prompt: "Sign up to sync your events across devices"

**Postconditions**: User is prompted to authenticate or event is saved locally only

---

### UC-004: Create Event (Authenticated User)
**Actor**: Authenticated User
**Description**: User creates a calendar event
**Preconditions**: User is authenticated
**Main Flow**:
1. User taps "Add Event" button
2. System displays event creation form
3. User enters event details (title, date, time, description)
4. User selects calendar type (Gregorian or Lunar)
5. User saves the event
6. System validates and stores the event via API
7. System caches event locally in SQLite
8. System displays success confirmation

**Postconditions**: Event is saved on server and synced locally

---

### UC-005: View Event Details
**Actor**: User
**Description**: User views details of an existing event
**Preconditions**: Events exist in the calendar
**Main Flow**:
1. User taps on a date with events
2. System displays list of events for that date
3. User taps on specific event
4. System displays full event details

**Postconditions**: Event details are displayed

---

### UC-006: Upgrade from Guest to Authenticated User
**Actor**: Guest User
**Description**: Guest user converts to authenticated user and migrates local data
**Preconditions**: User is in guest mode with local events
**Main Flow**:
1. User taps "Sign Up" from settings or upgrade prompt
2. System displays registration form
3. User enters credentials and submits
4. System creates account via API
5. System prompts: "Migrate your local events to your account?"
6. User confirms migration
7. System uploads local events to backend
8. System transitions to authenticated mode
9. System displays success message

**Postconditions**: User is authenticated and local data is synced to cloud

---

### UC-007: Edit Event
**Actor**: Authenticated User
**Description**: User modifies an existing event
**Preconditions**: Event exists and user has permissions
**Main Flow**:
1. User navigates to event details
2. User taps "Edit" button
3. System displays editable event form
4. User modifies event information
5. User saves changes
6. System updates event via API
7. System updates local cache
8. System displays success confirmation

**Postconditions**: Event is updated on server and locally

---

### UC-008: Delete Event
**Actor**: Authenticated User
**Description**: User removes an event
**Preconditions**: Event exists and user has permissions
**Main Flow**:
1. User navigates to event details
2. User taps "Delete" button
3. System displays confirmation dialog
4. User confirms deletion
5. System removes event via API
6. System removes event from local cache
7. System returns to calendar view

**Postconditions**: Event is deleted from server and locally

---

### UC-009: User Registration
**Actor**: New User
**Description**: User creates a new account
**Preconditions**: User has not registered before
**Main Flow**:
1. User taps "Sign Up" button
2. System displays registration form
3. User enters email, password, and profile information
4. User submits form
5. System validates input and creates account via API
6. System sends verification email (optional)
7. System logs user in automatically

**Postconditions**: User account is created and authenticated

---

### UC-010: User Login
**Actor**: Registered User
**Description**: User authenticates to access personalized features
**Preconditions**: User has registered account
**Main Flow**:
1. User launches app or taps "Sign In" from guest mode
2. System displays login screen
3. User enters credentials
4. System authenticates via API
5. System stores authentication token securely
6. System downloads user events from server
7. System navigates to main calendar view

**Postconditions**: User is authenticated and can access full features

---

### UC-011: Sync Events Across Devices
**Actor**: Authenticated User
**Description**: Events are synchronized across multiple devices
**Preconditions**: User is logged in on multiple devices
**Main Flow**:
1. User creates/modifies event on Device A
2. App sends update to API backend
3. Backend stores changes in database
4. Device B periodically checks for updates
5. Device B retrieves updated events from API
6. Device B displays synchronized events

**Postconditions**: Events are consistent across all devices

---

### UC-012: View Lunar Date Conversion
**Actor**: Guest or Authenticated User
**Description**: User converts between Gregorian and lunar dates
**Preconditions**: None
**Main Flow**:
1. User taps on any date in calendar
2. System displays both Gregorian and lunar date equivalents
3. System shows additional lunar calendar information (zodiac, phase, etc.)

**Postconditions**: Date conversion is displayed

---

### UC-013: Search Events
**Actor**: User
**Description**: User searches for specific events
**Preconditions**: Events exist in calendar
**Main Flow**:
1. User taps search icon
2. System displays search interface
3. User enters search term
4. System queries API for matching events
5. System displays results list
6. User taps on result to view event details

**Postconditions**: Search results are displayed

---

## 3. Functional Requirements

### 3.1 Guest Mode
- **FR-001**: System shall allow users to continue as guest without authentication
- **FR-002**: System shall display welcome screen with "Continue as Guest" and "Sign In/Sign Up" options
- **FR-003**: Guest users shall have access to calendar viewing features
- **FR-004**: Guest users shall be prompted to authenticate when attempting to create events
- **FR-005**: System shall allow guest users to upgrade to authenticated accounts
- **FR-006**: System shall migrate local guest data to cloud when upgrading to authenticated account
- **FR-007**: System shall display appropriate upgrade prompts throughout the app for guest users

### 3.2 Calendar Display
- **FR-008**: System shall display monthly calendar view with Gregorian dates
- **FR-009**: System shall display corresponding lunar dates for each Gregorian date
- **FR-010**: System shall support navigation between months (previous/next)
- **FR-011**: System shall highlight current date
- **FR-012**: System shall indicate dates with events using visual markers
- **FR-013**: System shall support multiple lunar calendar systems (Chinese, Vietnamese, Islamic)
- **FR-014**: System shall display week numbers (optional setting)

### 3.3 Event Management
- **FR-015**: System shall allow authenticated users to create events with title, description, date, time
- **FR-016**: System shall allow authenticated users to specify event calendar type (Gregorian or Lunar)
- **FR-017**: System shall allow authenticated users to edit existing events
- **FR-018**: System shall allow authenticated users to delete events with confirmation
- **FR-019**: System shall support recurring events (daily, weekly, monthly, yearly)
- **FR-020**: System shall support event reminders/notifications
- **FR-021**: System shall allow authenticated users to categorize events with tags/colors
- **FR-022**: System shall store guest events locally in SQLite (future enhancement)

### 3.4 User Management
- **FR-023**: System shall provide user registration functionality
- **FR-024**: System shall provide user authentication (login/logout)
- **FR-025**: System shall validate email format and password strength
- **FR-026**: System shall support password recovery mechanism
- **FR-027**: System shall maintain user sessions with secure tokens
- **FR-028**: System shall provide seamless upgrade path from guest to authenticated user
- **FR-029**: System shall migrate guest data during account creation

### 3.5 Data Synchronization
- **FR-030**: System shall synchronize events across authenticated user devices via API
- **FR-031**: System shall handle offline mode with local data caching
- **FR-032**: System shall resolve sync conflicts with timestamp priority
- **FR-033**: System shall provide pull-to-refresh functionality
- **FR-034**: System shall upload guest events to cloud during account upgrade

### 3.6 Search and Filtering
- **FR-035**: System shall provide event search by title and description (authenticated users only)
- **FR-036**: System shall allow filtering events by date range
- **FR-037**: System shall allow filtering events by category/tag

### 3.7 Localization
- **FR-038**: System shall support multiple languages (English, Chinese, Vietnamese)
- **FR-039**: System shall display lunar calendar names in appropriate language
- **FR-040**: System shall support regional date/time formats

### 3.8 API Backend
- **FR-041**: Backend shall provide RESTful API endpoints for all operations
- **FR-042**: Backend shall implement JWT-based authentication
- **FR-043**: Backend shall validate all input data
- **FR-044**: Backend shall log all transactions and errors
- **FR-045**: Backend shall support pagination for large data sets
- **FR-046**: Backend shall support optional authentication for calendar viewing endpoints

---

## 4. Non-Functional Requirements

### 4.1 Performance
- **NFR-001**: App shall launch within 3 seconds on modern devices
- **NFR-002**: Calendar view shall render within 1 second
- **NFR-003**: API responses shall complete within 2 seconds under normal load
- **NFR-004**: App shall support smooth 60fps scrolling animations
- **NFR-005**: Database queries shall be optimized with appropriate indexing

### 4.2 Scalability
- **NFR-006**: Backend shall support minimum 10,000 concurrent users
- **NFR-007**: Database shall handle minimum 1 million events
- **NFR-008**: System shall be horizontally scalable using container orchestration

### 4.3 Reliability
- **NFR-009**: System shall have 99.5% uptime (excluding planned maintenance)
- **NFR-010**: System shall implement automatic backup every 24 hours
- **NFR-011**: System shall gracefully handle network failures
- **NFR-012**: App shall function in offline mode with cached data

### 4.4 Security
- **NFR-013**: All API communication shall use HTTPS/TLS encryption
- **NFR-014**: Passwords shall be hashed using bcrypt or similar algorithm
- **NFR-015**: API shall implement rate limiting to prevent abuse
- **NFR-016**: System shall implement SQL injection prevention
- **NFR-017**: Authentication tokens shall expire after 24 hours
- **NFR-018**: System shall log security events for audit trail

### 4.5 Usability
- **NFR-019**: UI shall follow platform-specific design guidelines (iOS HIG, Material Design)
- **NFR-020**: App shall be accessible to users with disabilities (WCAG 2.1 Level AA)
- **NFR-021**: Error messages shall be clear and actionable
- **NFR-022**: User shall be able to complete common tasks within 3 taps
- **NFR-023**: Guest mode shall provide seamless experience without authentication barriers
- **NFR-024**: Upgrade prompts shall be contextual and non-intrusive

### 4.6 Compatibility
- **NFR-025**: iOS app shall support iOS 14.0 and above
- **NFR-026**: Android app shall support Android 7.0 (API 24) and above
- **NFR-027**: Backend shall run on Docker containers
- **NFR-028**: System shall support modern browsers for potential web admin interface

### 4.7 Maintainability
- **NFR-029**: Code shall follow Microsoft C# coding conventions
- **NFR-030**: API shall be documented using OpenAPI/Swagger
- **NFR-031**: Code shall have minimum 70% unit test coverage
- **NFR-032**: System shall use structured logging for troubleshooting

### 4.8 Portability
- **NFR-033**: Backend shall be deployable on any Docker-compatible environment
- **NFR-034**: Database schema shall be migration-based for version control
- **NFR-035**: Configuration shall be externalized for different environments

### 4.9 Compliance
- **NFR-036**: System shall comply with GDPR for EU users
- **NFR-037**: System shall comply with CCPA for California users
- **NFR-038**: System shall implement user data export functionality
- **NFR-039**: System shall implement user data deletion functionality
- **NFR-040**: Guest user data shall be stored only locally until account creation

---

## 5. Constraints

### 5.1 Technical Constraints
- Must use .NET MAUI for mobile development
- Must use ASP.NET Core for backend API
- Must support Docker containerization
- Must support iOS and Android platforms

### 5.2 Business Constraints
- Initial release target: 4-6 months from project start
- Development team size: 2-4 developers
- Budget considerations for cloud hosting costs

### 5.3 Regulatory Constraints
- Must comply with App Store and Google Play policies
- Must implement privacy policy and terms of service
- Must handle user data according to privacy regulations

---

## 6. Assumptions and Dependencies

### 6.1 Assumptions
- Users have internet connectivity for initial sync
- Users have devices running supported OS versions
- Lunar calendar calculation algorithms are available/documented

### 6.2 Dependencies
- Third-party lunar calendar calculation libraries
- Cloud hosting provider (Azure, AWS, or Google Cloud)
- Push notification services (APNs for iOS, FCM for Android)
- Email service provider for notifications

---

## 7. Future Enhancements (Post-MVP)

- Widget support for home screen calendar display
- Apple Watch and Android Wear companion apps
- Social features (sharing events, family calendars)
- Integration with other calendar services (Google Calendar, iCal)
- Advanced lunar calendar features (auspicious dates, zodiac predictions)
- Weather integration
- Holiday database for multiple countries
- Export functionality (PDF, iCal format)

---

**Document Control**

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-11 | Initial | Initial requirements specification |
