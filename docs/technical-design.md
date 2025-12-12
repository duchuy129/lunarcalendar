# Technical Design Document
## Lunar Calendar Mobile Application

**Version:** 1.0
**Date:** 2025-12-11
**Project:** Lunar Calendar App

---

## 1. Introduction

### 1.1 Purpose
This document provides the technical architecture and design specifications for the Lunar Calendar mobile application, covering both the .NET MAUI mobile clients and ASP.NET Core backend API.

### 1.2 Scope
This document covers:
- System architecture
- Technology stack details
- Database design
- API design
- Mobile app architecture
- Security design
- Deployment architecture

---

## 2. System Architecture

### 2.1 High-Level Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    Mobile Clients                        │
│  ┌──────────────────┐      ┌──────────────────┐        │
│  │   iOS App        │      │   Android App     │        │
│  │  (.NET MAUI)     │      │   (.NET MAUI)     │        │
│  └────────┬─────────┘      └─────────┬────────┘        │
└───────────┼────────────────────────────┼─────────────────┘
            │                            │
            │      HTTPS/REST API        │
            │                            │
┌───────────┴────────────────────────────┴─────────────────┐
│              API Gateway / Load Balancer                  │
└───────────┬────────────────────────────┬─────────────────┘
            │                            │
┌───────────┴────────────────────────────┴─────────────────┐
│                Backend Services (Docker)                  │
│  ┌────────────────────────────────────────────────┐     │
│  │        ASP.NET Core Web API (.NET 8)           │     │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────┐  │     │
│  │  │  Auth    │ │ Calendar │ │    Event      │  │     │
│  │  │ Service  │ │ Service  │ │   Service     │  │     │
│  │  └──────────┘ └──────────┘ └──────────────┘  │     │
│  └────────────────────┬───────────────────────────┘     │
└───────────────────────┼─────────────────────────────────┘
                        │
┌───────────────────────┴─────────────────────────────────┐
│              Database Layer                              │
│  ┌────────────────────────────────────────────────┐    │
│  │  PostgreSQL / SQL Server                       │    │
│  │  - Users, Events, Settings, Audit Logs        │    │
│  └────────────────────────────────────────────────┘    │
└──────────────────────────────────────────────────────────┘
```

### 2.2 Architecture Patterns
- **Client-Server Architecture**: Clear separation between mobile clients and backend
- **Guest-First Design**: App fully functional without backend for basic features
- **Progressive Enhancement**: Additional features unlocked with authentication
- **RESTful API**: Standard HTTP methods for resource operations
- **Repository Pattern**: Data access abstraction layer with dual storage (local/remote)
- **Dependency Injection**: Loose coupling and testability
- **MVVM Pattern**: Model-View-ViewModel for mobile UI

### 2.3 User Mode Architecture

The application supports two distinct user modes:

#### Guest Mode
- **No authentication required**: Users can start using the app immediately
- **Calendar viewing**: Full access to Gregorian and lunar calendar display
- **Lunar date conversion**: All date conversion features available
- **Local-only storage**: All user preferences stored in local SQLite
- **No backend dependency**: Core features work without internet connection
- **Upgrade prompts**: Contextual prompts when attempting to access advanced features
- **Feature limitations**: Cannot create events, sync data, or access cloud features

#### Authenticated Mode
- **Full user authentication**: JWT-based secure authentication
- **Event management**: Create, edit, and delete events
- **Cloud synchronization**: Events synced across all user devices
- **Backend integration**: Full API access for all features
- **Advanced features**: Search, sharing, notifications, recurring events
- **Dual storage**: Local SQLite cache + remote PostgreSQL database
- **Offline support**: Works offline with automatic sync when online

#### Mode Transition Flow
```
Guest User → Sign Up/Sign In → Authenticated User
     ↓                              ↓
 Local Data  →  Migration  →  Cloud + Local Cache
```

When a guest user creates an account:
1. System prompts to migrate local data (if any exists in future enhancement)
2. User credentials are created via API
3. Local data is uploaded to backend
4. User transitions to authenticated mode
5. Local storage becomes cache layer for cloud data

---

## 3. Technology Stack

### 3.1 Mobile Application
| Component | Technology | Version |
|-----------|------------|---------|
| Framework | .NET MAUI | .NET 8 |
| Language | C# | 12.0 |
| UI Pattern | MVVM with Community Toolkit | Latest |
| HTTP Client | HttpClient with Refit | 7.x |
| Local Storage | SQLite with SQLite-net | 1.8.x |
| Dependency Injection | Microsoft.Extensions.DependencyInjection | Built-in |
| Navigation | Shell Navigation | Built-in |
| State Management | CommunityToolkit.Mvvm | 8.x |

### 3.2 Backend API
| Component | Technology | Version |
|-----------|------------|---------|
| Framework | ASP.NET Core Web API | .NET 8 |
| Language | C# | 12.0 |
| ORM | Entity Framework Core | 8.x |
| Authentication | ASP.NET Core Identity + JWT | Built-in |
| API Documentation | Swashbuckle (Swagger) | 6.x |
| Validation | FluentValidation | 11.x |
| Logging | Serilog | 3.x |
| Mapping | AutoMapper | 12.x |

### 3.3 Database
| Component | Technology |
|-----------|------------|
| Primary Option | PostgreSQL 15+ |
| Alternative | SQL Server 2019+ |
| Migration Tool | EF Core Migrations |

### 3.4 Infrastructure
| Component | Technology |
|-----------|------------|
| Containerization | Docker |
| Orchestration | Docker Compose (dev), Kubernetes (production) |
| Reverse Proxy | Nginx or Traefik |
| CI/CD | GitHub Actions / Azure DevOps |
| Hosting | Azure / AWS / Google Cloud |

---

## 4. Database Design

### 4.1 Entity Relationship Diagram

```
┌─────────────────────┐
│      Users          │
├─────────────────────┤
│ Id (PK)             │
│ Email               │
│ PasswordHash        │
│ FirstName           │
│ LastName            │
│ PreferredLanguage   │
│ TimeZone            │
│ CreatedAt           │
│ UpdatedAt           │
└──────────┬──────────┘
           │
           │ 1:N
           │
┌──────────┴──────────┐
│      Events         │
├─────────────────────┤
│ Id (PK)             │
│ UserId (FK)         │
│ Title               │
│ Description         │
│ StartDate           │
│ EndDate             │
│ IsAllDay            │
│ CalendarType        │  (Gregorian/Lunar)
│ RecurrenceRule      │
│ ReminderMinutes     │
│ CategoryId (FK)     │
│ Color               │
│ CreatedAt           │
│ UpdatedAt           │
│ IsDeleted           │
└──────────┬──────────┘
           │
           │ N:1
           │
┌──────────┴──────────┐
│    Categories       │
├─────────────────────┤
│ Id (PK)             │
│ UserId (FK)         │
│ Name                │
│ Color               │
│ Icon                │
└─────────────────────┘

┌─────────────────────┐
│  RefreshTokens      │
├─────────────────────┤
│ Id (PK)             │
│ UserId (FK)         │
│ Token               │
│ ExpiresAt           │
│ CreatedAt           │
│ RevokedAt           │
└─────────────────────┘

┌─────────────────────┐
│   AuditLogs         │
├─────────────────────┤
│ Id (PK)             │
│ UserId (FK)         │
│ Action              │
│ EntityType          │
│ EntityId            │
│ Changes             │  (JSON)
│ IpAddress           │
│ Timestamp           │
└─────────────────────┘
```

### 4.2 Database Schema

#### Users Table
```sql
CREATE TABLE Users (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    PreferredLanguage VARCHAR(10) DEFAULT 'en',
    TimeZone VARCHAR(50) DEFAULT 'UTC',
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_users_email ON Users(Email);
```

#### Events Table
```sql
CREATE TABLE Events (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    UserId UUID NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    StartDate TIMESTAMP NOT NULL,
    EndDate TIMESTAMP,
    IsAllDay BOOLEAN DEFAULT FALSE,
    CalendarType VARCHAR(20) DEFAULT 'Gregorian',
    RecurrenceRule TEXT,
    ReminderMinutes INT,
    CategoryId UUID REFERENCES Categories(Id) ON DELETE SET NULL,
    Color VARCHAR(7),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    IsDeleted BOOLEAN DEFAULT FALSE
);

CREATE INDEX idx_events_userid ON Events(UserId);
CREATE INDEX idx_events_startdate ON Events(StartDate);
CREATE INDEX idx_events_categoryid ON Events(CategoryId);
```

#### Categories Table
```sql
CREATE TABLE Categories (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    UserId UUID NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    Name VARCHAR(100) NOT NULL,
    Color VARCHAR(7),
    Icon VARCHAR(50)
);

CREATE INDEX idx_categories_userid ON Categories(UserId);
```

#### RefreshTokens Table
```sql
CREATE TABLE RefreshTokens (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    UserId UUID NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    Token VARCHAR(512) UNIQUE NOT NULL,
    ExpiresAt TIMESTAMP NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    RevokedAt TIMESTAMP
);

CREATE INDEX idx_refreshtokens_token ON RefreshTokens(Token);
CREATE INDEX idx_refreshtokens_userid ON RefreshTokens(UserId);
```

#### AuditLogs Table
```sql
CREATE TABLE AuditLogs (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    UserId UUID REFERENCES Users(Id) ON DELETE SET NULL,
    Action VARCHAR(50) NOT NULL,
    EntityType VARCHAR(50),
    EntityId UUID,
    Changes JSONB,
    IpAddress VARCHAR(45),
    Timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_auditlogs_userid ON AuditLogs(UserId);
CREATE INDEX idx_auditlogs_timestamp ON AuditLogs(Timestamp);
```

---

## 5. API Design

### 5.1 API Endpoints

#### Authentication Endpoints
```
POST   /api/auth/register          - Register new user
POST   /api/auth/login             - Authenticate user
POST   /api/auth/refresh           - Refresh access token
POST   /api/auth/logout            - Logout user
POST   /api/auth/forgot-password   - Request password reset
POST   /api/auth/reset-password    - Reset password with token
```

#### User Endpoints
```
GET    /api/users/me               - Get current user profile
PUT    /api/users/me               - Update user profile
DELETE /api/users/me               - Delete user account
GET    /api/users/me/settings      - Get user settings
PUT    /api/users/me/settings      - Update user settings
```

#### Event Endpoints
```
GET    /api/events                 - Get all events (with pagination, filtering)
GET    /api/events/{id}            - Get specific event
POST   /api/events                 - Create new event
PUT    /api/events/{id}            - Update event
DELETE /api/events/{id}            - Delete event
GET    /api/events/range           - Get events within date range
GET    /api/events/search          - Search events
```

#### Category Endpoints
```
GET    /api/categories             - Get all categories
GET    /api/categories/{id}        - Get specific category
POST   /api/categories             - Create category
PUT    /api/categories/{id}        - Update category
DELETE /api/categories/{id}        - Delete category
```

#### Calendar Endpoints
```
GET    /api/calendar/convert       - Convert Gregorian to Lunar date (no auth required)
GET    /api/calendar/lunar-info    - Get lunar calendar information (no auth required)
```

**Note on Authentication**: Calendar conversion endpoints do not require authentication to support guest mode. However, they may implement rate limiting based on IP address to prevent abuse.

### 5.2 API Request/Response Examples

#### Register User
**Request:**
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePass123!",
  "firstName": "John",
  "lastName": "Doe",
  "preferredLanguage": "en",
  "timeZone": "America/New_York"
}
```

**Response:**
```http
HTTP/1.1 201 Created
Content-Type: application/json

{
  "success": true,
  "data": {
    "userId": "123e4567-e89b-12d3-a456-426614174000",
    "email": "user@example.com",
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2g...",
    "expiresIn": 3600
  }
}
```

#### Create Event
**Request:**
```http
POST /api/events
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
Content-Type: application/json

{
  "title": "Lunar New Year Celebration",
  "description": "Family gathering for Lunar New Year",
  "startDate": "2026-01-29T10:00:00Z",
  "endDate": "2026-01-29T15:00:00Z",
  "isAllDay": false,
  "calendarType": "Lunar",
  "categoryId": "456e7890-e89b-12d3-a456-426614174001",
  "color": "#FF5733",
  "reminderMinutes": 60
}
```

**Response:**
```http
HTTP/1.1 201 Created
Content-Type: application/json

{
  "success": true,
  "data": {
    "id": "789e1234-e89b-12d3-a456-426614174002",
    "title": "Lunar New Year Celebration",
    "description": "Family gathering for Lunar New Year",
    "startDate": "2026-01-29T10:00:00Z",
    "endDate": "2026-01-29T15:00:00Z",
    "isAllDay": false,
    "calendarType": "Lunar",
    "categoryId": "456e7890-e89b-12d3-a456-426614174001",
    "color": "#FF5733",
    "reminderMinutes": 60,
    "createdAt": "2025-12-11T10:30:00Z",
    "updatedAt": "2025-12-11T10:30:00Z"
  }
}
```

#### Get Events in Date Range
**Request:**
```http
GET /api/events/range?startDate=2026-01-01&endDate=2026-01-31&page=1&pageSize=20
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

**Response:**
```http
HTTP/1.1 200 OK
Content-Type: application/json

{
  "success": true,
  "data": {
    "events": [
      {
        "id": "789e1234-e89b-12d3-a456-426614174002",
        "title": "Lunar New Year Celebration",
        "startDate": "2026-01-29T10:00:00Z",
        "endDate": "2026-01-29T15:00:00Z",
        "calendarType": "Lunar",
        "color": "#FF5733"
      }
    ],
    "pagination": {
      "currentPage": 1,
      "pageSize": 20,
      "totalPages": 1,
      "totalItems": 1
    }
  }
}
```

### 5.3 Error Response Format
```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Invalid request data",
    "details": [
      {
        "field": "email",
        "message": "Email is required"
      }
    ]
  }
}
```

---

## 6. Mobile Application Architecture

### 6.1 Project Structure
```
LunarCalendar.MobileApp/
├── App.xaml.cs                    # Application entry point
├── MauiProgram.cs                 # DI configuration
├── Models/                        # Domain models
│   ├── Event.cs
│   ├── User.cs
│   ├── Category.cs
│   └── UserMode.cs                # Guest/Authenticated mode enum
├── ViewModels/                    # MVVM ViewModels
│   ├── CalendarViewModel.cs
│   ├── EventDetailViewModel.cs
│   ├── LoginViewModel.cs
│   ├── WelcomeViewModel.cs        # Guest/Auth selection
│   └── BaseViewModel.cs
├── Views/                         # XAML pages
│   ├── WelcomePage.xaml           # Initial screen for mode selection
│   ├── CalendarPage.xaml
│   ├── EventDetailPage.xaml
│   ├── LoginPage.xaml
│   ├── RegisterPage.xaml
│   └── SettingsPage.xaml
├── Services/                      # Business logic
│   ├── IAuthService.cs
│   ├── AuthService.cs
│   ├── IEventService.cs
│   ├── EventService.cs
│   ├── ICalendarService.cs
│   ├── CalendarService.cs
│   ├── IUserModeService.cs        # Manages guest/auth state
│   └── UserModeService.cs
├── Data/                          # Data access
│   ├── ApiClient.cs
│   ├── LocalDatabase.cs
│   └── Repositories/
│       ├── IEventRepository.cs
│       ├── EventRepository.cs
│       ├── IUserPreferencesRepository.cs
│       └── UserPreferencesRepository.cs
├── Helpers/                       # Utilities
│   ├── Constants.cs
│   ├── Converters/
│   ├── Extensions/
│   └── UpgradePromptHelper.cs     # Guest-to-auth prompts
└── Resources/                     # Assets, strings, styles
    ├── Images/
    ├── Fonts/
    └── Strings/
```

### 6.2 MVVM Implementation

**BaseViewModel.cs**
```csharp
public abstract class BaseViewModel : ObservableObject
{
    private bool _isBusy;
    private string _title;

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
}
```

**CalendarViewModel.cs**
```csharp
public class CalendarViewModel : BaseViewModel
{
    private readonly IEventService _eventService;
    private readonly ICalendarService _calendarService;

    public ObservableCollection<Event> Events { get; }
    public DateTime SelectedDate { get; set; }

    public IAsyncRelayCommand LoadEventsCommand { get; }
    public IRelayCommand<DateTime> DateSelectedCommand { get; }
    public IAsyncRelayCommand<string> AddEventCommand { get; }

    public CalendarViewModel(
        IEventService eventService,
        ICalendarService calendarService)
    {
        _eventService = eventService;
        _calendarService = calendarService;
        Events = new ObservableCollection<Event>();

        LoadEventsCommand = new AsyncRelayCommand(LoadEventsAsync);
        DateSelectedCommand = new RelayCommand<DateTime>(OnDateSelected);
        AddEventCommand = new AsyncRelayCommand<string>(AddEventAsync);
    }

    private async Task LoadEventsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var events = await _eventService.GetEventsForMonthAsync(SelectedDate);

            Events.Clear();
            foreach (var evt in events)
            {
                Events.Add(evt);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
```

### 6.3 Guest Mode Implementation

**UserMode.cs**
```csharp
public enum UserMode
{
    Guest,
    Authenticated
}
```

**IUserModeService.cs**
```csharp
public interface IUserModeService
{
    UserMode CurrentMode { get; }
    bool IsGuest { get; }
    bool IsAuthenticated { get; }
    Task SetModeAsync(UserMode mode);
    Task UpgradeToAuthenticatedAsync(string userId);
    Task<bool> CheckAuthenticationStatusAsync();
}
```

**UserModeService.cs**
```csharp
public class UserModeService : IUserModeService
{
    private readonly IPreferences _preferences;
    private readonly ISecureStorage _secureStorage;
    private UserMode _currentMode;

    public UserModeService(IPreferences preferences, ISecureStorage secureStorage)
    {
        _preferences = preferences;
        _secureStorage = secureStorage;
        _currentMode = LoadSavedMode();
    }

    public UserMode CurrentMode => _currentMode;
    public bool IsGuest => _currentMode == UserMode.Guest;
    public bool IsAuthenticated => _currentMode == UserMode.Authenticated;

    public async Task SetModeAsync(UserMode mode)
    {
        _currentMode = mode;
        _preferences.Set("user_mode", mode.ToString());

        // Notify app of mode change
        MessagingCenter.Send(this, "UserModeChanged", mode);
    }

    public async Task UpgradeToAuthenticatedAsync(string userId)
    {
        await SetModeAsync(UserMode.Authenticated);
        _preferences.Set("user_id", userId);
    }

    public async Task<bool> CheckAuthenticationStatusAsync()
    {
        var token = await _secureStorage.GetAsync("access_token");
        if (!string.IsNullOrEmpty(token))
        {
            await SetModeAsync(UserMode.Authenticated);
            return true;
        }
        return false;
    }

    private UserMode LoadSavedMode()
    {
        var modeName = _preferences.Get("user_mode", UserMode.Guest.ToString());
        return Enum.Parse<UserMode>(modeName);
    }
}
```

**UpgradePromptHelper.cs**
```csharp
public static class UpgradePromptHelper
{
    public static async Task<bool> ShowUpgradePromptAsync(
        string title = "Sign Up Required",
        string message = "This feature requires an account. Create one to continue?")
    {
        var result = await Shell.Current.DisplayActionSheet(
            message,
            "Cancel",
            null,
            "Sign Up",
            "Sign In");

        if (result == "Sign Up")
        {
            await Shell.Current.GoToAsync("//register");
            return true;
        }
        else if (result == "Sign In")
        {
            await Shell.Current.GoToAsync("//login");
            return true;
        }

        return false;
    }

    public static async Task ShowFeatureLockedAsync(string featureName)
    {
        await ShowUpgradePromptAsync(
            "Feature Locked",
            $"{featureName} is available for registered users. Sign up to unlock!");
    }
}
```

**WelcomeViewModel.cs**
```csharp
public class WelcomeViewModel : BaseViewModel
{
    private readonly IUserModeService _userModeService;

    public IAsyncRelayCommand ContinueAsGuestCommand { get; }
    public IAsyncRelayCommand SignInCommand { get; }
    public IAsyncRelayCommand SignUpCommand { get; }

    public WelcomeViewModel(IUserModeService userModeService)
    {
        _userModeService = userModeService;
        Title = "Welcome";

        ContinueAsGuestCommand = new AsyncRelayCommand(ContinueAsGuestAsync);
        SignInCommand = new AsyncRelayCommand(NavigateToSignInAsync);
        SignUpCommand = new AsyncRelayCommand(NavigateToSignUpAsync);
    }

    private async Task ContinueAsGuestAsync()
    {
        await _userModeService.SetModeAsync(UserMode.Guest);
        await Shell.Current.GoToAsync("//calendar");
    }

    private async Task NavigateToSignInAsync()
    {
        await Shell.Current.GoToAsync("//login");
    }

    private async Task NavigateToSignUpAsync()
    {
        await Shell.Current.GoToAsync("//register");
    }
}
```

**Modified CalendarViewModel with Guest Support**
```csharp
public class CalendarViewModel : BaseViewModel
{
    private readonly IEventService _eventService;
    private readonly ICalendarService _calendarService;
    private readonly IUserModeService _userModeService;

    public ObservableCollection<Event> Events { get; }
    public DateTime SelectedDate { get; set; }
    public bool CanCreateEvents => _userModeService.IsAuthenticated;

    public IAsyncRelayCommand LoadEventsCommand { get; }
    public IAsyncRelayCommand AddEventCommand { get; }

    public CalendarViewModel(
        IEventService eventService,
        ICalendarService calendarService,
        IUserModeService userModeService)
    {
        _eventService = eventService;
        _calendarService = calendarService;
        _userModeService = userModeService;
        Events = new ObservableCollection<Event>();

        LoadEventsCommand = new AsyncRelayCommand(LoadEventsAsync);
        AddEventCommand = new AsyncRelayCommand(AddEventAsync);

        // Listen for mode changes
        MessagingCenter.Subscribe<UserModeService, UserMode>(
            this, "UserModeChanged", OnUserModeChanged);
    }

    private async Task AddEventAsync()
    {
        if (_userModeService.IsGuest)
        {
            // Show upgrade prompt
            await UpgradePromptHelper.ShowFeatureLockedAsync("Event Creation");
            return;
        }

        // Navigate to event creation for authenticated users
        await Shell.Current.GoToAsync("//event/create");
    }

    private async Task LoadEventsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            // Load events based on user mode
            if (_userModeService.IsAuthenticated)
            {
                // Load from API and cache
                var events = await _eventService.GetEventsForMonthAsync(SelectedDate);
                Events.Clear();
                foreach (var evt in events)
                {
                    Events.Add(evt);
                }
            }
            else
            {
                // Guest mode - no events to display (or local events in future)
                Events.Clear();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void OnUserModeChanged(UserModeService sender, UserMode mode)
    {
        OnPropertyChanged(nameof(CanCreateEvents));
        _ = LoadEventsAsync(); // Reload events when mode changes
    }
}
```

### 6.4 Data Services

**ApiClient.cs**
```csharp
public interface IApiClient
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<List<Event>> GetEventsAsync(DateTime startDate, DateTime endDate);
    Task<Event> CreateEventAsync(CreateEventRequest request);
    Task UpdateEventAsync(Guid id, UpdateEventRequest request);
    Task DeleteEventAsync(Guid id);
}

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ISecureStorage _secureStorage;

    public ApiClient(HttpClient httpClient, ISecureStorage secureStorage)
    {
        _httpClient = httpClient;
        _secureStorage = secureStorage;
    }

    private async Task AddAuthHeaderAsync()
    {
        var token = await _secureStorage.GetAsync("access_token");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<Event>> GetEventsAsync(DateTime startDate, DateTime endDate)
    {
        await AddAuthHeaderAsync();
        var response = await _httpClient.GetAsync(
            $"api/events/range?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<EventListResponse>>();
        return result.Data.Events;
    }
}
```

**LocalDatabase.cs**
```csharp
public class LocalDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public LocalDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Event>().Wait();
        _database.CreateTableAsync<Category>().Wait();
    }

    public Task<List<Event>> GetEventsAsync() =>
        _database.Table<Event>().Where(e => !e.IsDeleted).ToListAsync();

    public Task<Event> GetEventAsync(Guid id) =>
        _database.Table<Event>().Where(e => e.Id == id).FirstOrDefaultAsync();

    public Task<int> SaveEventAsync(Event evt) =>
        evt.Id == Guid.Empty
            ? _database.InsertAsync(evt)
            : _database.UpdateAsync(evt);

    public Task<int> DeleteEventAsync(Event evt) =>
        _database.DeleteAsync(evt);
}
```

### 6.4 Dependency Injection Setup

**MauiProgram.cs**
```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
        builder.Services.AddSingleton<IPreferences>(Preferences.Default);

        var dbPath = Path.Combine(
            FileSystem.AppDataDirectory, "lunarcalendar.db3");
        builder.Services.AddSingleton(s => new LocalDatabase(dbPath));

        builder.Services.AddHttpClient<IApiClient, ApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.lunarcalendar.com");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        builder.Services.AddSingleton<IUserModeService, UserModeService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IEventService, EventService>();
        builder.Services.AddSingleton<ICalendarService, CalendarService>();

        // ViewModels
        builder.Services.AddTransient<WelcomeViewModel>();
        builder.Services.AddTransient<CalendarViewModel>();
        builder.Services.AddTransient<EventDetailViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();

        // Views
        builder.Services.AddTransient<WelcomePage>();
        builder.Services.AddTransient<CalendarPage>();
        builder.Services.AddTransient<EventDetailPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();

        return builder.Build();
    }
}
```

---

## 7. Backend Architecture

### 7.1 Project Structure
```
LunarCalendar.Api/
├── Program.cs
├── appsettings.json
├── Controllers/
│   ├── AuthController.cs
│   ├── EventsController.cs
│   ├── UsersController.cs
│   └── CalendarController.cs
├── Models/
│   ├── Entities/
│   │   ├── User.cs
│   │   ├── Event.cs
│   │   └── Category.cs
│   ├── DTOs/
│   │   ├── AuthDtos.cs
│   │   ├── EventDtos.cs
│   │   └── UserDtos.cs
│   └── Responses/
│       └── ApiResponse.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   └── Migrations/
├── Services/
│   ├── IAuthService.cs
│   ├── AuthService.cs
│   ├── IEventService.cs
│   ├── EventService.cs
│   ├── ILunarCalendarService.cs
│   └── LunarCalendarService.cs
├── Repositories/
│   ├── IRepository.cs
│   ├── Repository.cs
│   ├── IEventRepository.cs
│   └── EventRepository.cs
├── Middleware/
│   ├── ExceptionMiddleware.cs
│   └── RequestLoggingMiddleware.cs
├── Validators/
│   ├── CreateEventValidator.cs
│   └── RegisterUserValidator.cs
└── Helpers/
    ├── JwtHelper.cs
    └── AutoMapperProfile.cs
```

### 7.2 Key Components

**Program.cs**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMobileApps", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Services
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ILunarCalendarService, LunarCalendarService>();

// Logging
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowMobileApps");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
```

**EventsController.cs**
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IMapper _mapper;

    public EventsController(IEventService eventService, IMapper mapper)
    {
        _eventService = eventService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<EventDto>>>> GetEvents(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var events = await _eventService.GetUserEventsAsync(userId, page, pageSize);

        return Ok(new ApiResponse<List<EventDto>>
        {
            Success = true,
            Data = _mapper.Map<List<EventDto>>(events)
        });
    }

    [HttpGet("range")]
    public async Task<ActionResult<ApiResponse<EventListResponse>>> GetEventsInRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 100)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _eventService.GetEventsInRangeAsync(
            userId, startDate, endDate, page, pageSize);

        return Ok(new ApiResponse<EventListResponse>
        {
            Success = true,
            Data = result
        });
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<EventDto>>> CreateEvent(
        [FromBody] CreateEventRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var evt = await _eventService.CreateEventAsync(userId, request);

        return CreatedAtAction(
            nameof(GetEvent),
            new { id = evt.Id },
            new ApiResponse<EventDto>
            {
                Success = true,
                Data = _mapper.Map<EventDto>(evt)
            });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<EventDto>>> GetEvent(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var evt = await _eventService.GetEventByIdAsync(id, userId);

        if (evt == null)
            return NotFound(new ApiResponse<EventDto>
            {
                Success = false,
                Error = new ErrorDetails
                {
                    Code = "NOT_FOUND",
                    Message = "Event not found"
                }
            });

        return Ok(new ApiResponse<EventDto>
        {
            Success = true,
            Data = _mapper.Map<EventDto>(evt)
        });
    }
}
```

---

## 8. Security Design

### 8.1 Authentication Flow

```
1. User Registration/Login
   ↓
2. Server validates credentials
   ↓
3. Server generates JWT access token (short-lived, 1 hour)
   ↓
4. Server generates refresh token (long-lived, 30 days)
   ↓
5. Client stores both tokens securely
   ↓
6. Client includes access token in API requests
   ↓
7. When access token expires, client uses refresh token
   ↓
8. Server validates refresh token and issues new access token
```

### 8.2 JWT Token Structure
```json
{
  "header": {
    "alg": "HS256",
    "typ": "JWT"
  },
  "payload": {
    "sub": "123e4567-e89b-12d3-a456-426614174000",
    "email": "user@example.com",
    "iat": 1702345678,
    "exp": 1702349278
  }
}
```

### 8.3 Security Measures

| Area | Implementation |
|------|----------------|
| Password Storage | BCrypt hashing with salt |
| Token Storage (Mobile) | Platform-specific secure storage (Keychain/Keystore) |
| API Communication | HTTPS/TLS 1.2+ only |
| SQL Injection | Parameterized queries via EF Core |
| XSS Prevention | Input sanitization, output encoding |
| CSRF | Not applicable for mobile API (no cookies) |
| Rate Limiting | IP-based throttling (100 req/min) |
| Input Validation | FluentValidation on all endpoints |

---

## 9. Deployment Architecture

### 9.1 Docker Configuration

**Dockerfile (Backend)**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LunarCalendar.Api/LunarCalendar.Api.csproj", "LunarCalendar.Api/"]
RUN dotnet restore "LunarCalendar.Api/LunarCalendar.Api.csproj"
COPY . .
WORKDIR "/src/LunarCalendar.Api"
RUN dotnet build "LunarCalendar.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LunarCalendar.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LunarCalendar.Api.dll"]
```

**docker-compose.yml**
```yaml
version: '3.8'

services:
  api:
    build:
      context: .
      dockerfile: LunarCalendar.Api/Dockerfile
    ports:
      - "5000:80"
      - "5001:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Host=db;Database=lunarcalendar;Username=postgres;Password=${DB_PASSWORD}
      - Jwt__Key=${JWT_SECRET_KEY}
      - Jwt__Issuer=lunarcalendar-api
      - Jwt__Audience=lunarcalendar-mobile
    depends_on:
      - db
    networks:
      - app-network
    restart: unless-stopped

  db:
    image: postgres:15-alpine
    environment:
      - POSTGRES_DB=lunarcalendar
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=${DB_PASSWORD}
    volumes:
      - postgres-data:/var/lib/postgresql/data
    ports:
      - "5432:5432"
    networks:
      - app-network
    restart: unless-stopped

  nginx:
    image: nginx:alpine
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf
      - ./ssl:/etc/nginx/ssl
    depends_on:
      - api
    networks:
      - app-network
    restart: unless-stopped

volumes:
  postgres-data:

networks:
  app-network:
    driver: bridge
```

### 9.2 Environment Variables
```bash
# .env file
DB_PASSWORD=secure_database_password
JWT_SECRET_KEY=your_very_long_and_secure_random_key_here
ASPNETCORE_ENVIRONMENT=Production
```

---

## 10. Performance Optimization

### 10.1 Backend Optimizations
- **Database Indexing**: Index on frequently queried columns (UserId, StartDate, Email)
- **Query Optimization**: Use EF Core projection to select only needed fields
- **Caching**: Implement Redis caching for frequently accessed data
- **Pagination**: Limit query results to prevent large data transfers
- **Async/Await**: All I/O operations are asynchronous
- **Connection Pooling**: Default EF Core connection pooling

### 10.2 Mobile Optimizations
- **Local Caching**: SQLite database for offline access
- **Image Optimization**: Compress and cache images
- **Lazy Loading**: Load data only when needed
- **Background Sync**: Synchronize data in background threads
- **Batch Requests**: Combine multiple API calls when possible
- **Memory Management**: Dispose of resources properly, weak references for large objects

---

## 11. Testing Strategy

### 11.1 Backend Testing
```csharp
// Unit Tests (xUnit)
public class EventServiceTests
{
    [Fact]
    public async Task CreateEvent_ValidRequest_ReturnsEvent()
    {
        // Arrange
        var mockRepo = new Mock<IEventRepository>();
        var service = new EventService(mockRepo.Object);
        var request = new CreateEventRequest { Title = "Test Event" };

        // Act
        var result = await service.CreateEventAsync(userId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Event", result.Title);
    }
}

// Integration Tests
public class EventsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    [Fact]
    public async Task GetEvents_Authenticated_ReturnsOk()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _testToken);

        // Act
        var response = await _client.GetAsync("/api/events");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
```

### 11.2 Mobile Testing
- **Unit Tests**: ViewModel logic, business services
- **UI Tests**: MAUI UI testing framework
- **Integration Tests**: API communication tests
- **Manual Testing**: Device testing on iOS and Android

---

## 12. Monitoring and Logging

### 12.1 Logging Strategy
- **Serilog**: Structured logging with multiple sinks
- **Log Levels**: Debug, Information, Warning, Error, Critical
- **Log Destinations**: Console, File, Database, Application Insights

### 12.2 Monitoring Metrics
- API response times
- Database query performance
- Error rates and exceptions
- User authentication attempts
- API endpoint usage statistics

---

**Document Control**

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-11 | Initial | Initial technical design |
