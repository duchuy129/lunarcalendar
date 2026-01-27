# Vietnamese Lunar Calendar Application
## Data Models & API Reference

**Version:** 2.0  
**Date:** January 3, 2026  
**Status:** MVP Complete

---

## Table of Contents

1. [Data Models](#data-models)
2. [Database Schema](#database-schema)
3. [API Endpoints](#api-endpoints)
4. [Request/Response Examples](#requestresponse-examples)
5. [Error Handling](#error-handling)
6. [Data Validation Rules](#data-validation-rules)

---

## 1. Data Models

### 1.1 LunarDate

**Description:** Represents a date with both Gregorian and Lunar calendar information.

**Properties:**

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `gregorianDate` | DateTime | Yes | The Gregorian calendar date |
| `lunarYear` | int | Yes | Lunar year (1900-2100) |
| `lunarMonth` | int | Yes | Lunar month (1-12) |
| `lunarDay` | int | Yes | Lunar day (1-30) |
| `isLeapMonth` | boolean | Yes | Whether this is a leap month |
| `lunarYearName` | string | Yes | Chinese year name (e.g., "甲辰年") |
| `lunarMonthName` | string | Yes | Chinese month name (e.g., "正月") |
| `lunarDayName` | string | Yes | Chinese day name (e.g., "初一") |
| `animalSign` | string | Yes | Zodiac animal sign (e.g., "Dragon") |

**C# Model:**

```csharp
public class LunarDate
{
    public DateTime GregorianDate { get; set; }
    public int LunarYear { get; set; }
    public int LunarMonth { get; set; }
    public int LunarDay { get; set; }
    public bool IsLeapMonth { get; set; }
    public string LunarYearName { get; set; } = string.Empty;
    public string LunarMonthName { get; set; } = string.Empty;
    public string LunarDayName { get; set; } = string.Empty;
    public string AnimalSign { get; set; } = string.Empty;
}
```

**JSON Example:**

```json
{
  "gregorianDate": "2026-02-17T00:00:00Z",
  "lunarYear": 2026,
  "lunarMonth": 1,
  "lunarDay": 1,
  "isLeapMonth": false,
  "lunarYearName": "丙午年",
  "lunarMonthName": "正月",
  "lunarDayName": "初一",
  "animalSign": "Horse"
}
```

**Validation Rules:**
- `gregorianDate`: Must be between 1901-02-19 and 2101-01-28
- `lunarYear`: 1900-2100
- `lunarMonth`: 1-12
- `lunarDay`: 1-30 (depends on month)
- All string fields: Non-empty

---

### 1.2 Holiday

**Description:** Definition of a holiday (static data).

**Properties:**

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `id` | int | Yes | Unique identifier |
| `name` | string | Yes | Holiday name (default language) |
| `description` | string | Yes | Holiday description |
| `nameResourceKey` | string | Yes | Localization key for name |
| `descriptionResourceKey` | string | Yes | Localization key for description |
| `lunarMonth` | int | No | Lunar month (0 if Gregorian-based) |
| `lunarDay` | int | No | Lunar day (0 if Gregorian-based) |
| `isLeapMonth` | boolean | Yes | Whether this occurs in leap month |
| `gregorianMonth` | int? | No | Gregorian month (null if lunar-based) |
| `gregorianDay` | int? | No | Gregorian day (null if lunar-based) |
| `type` | HolidayType | Yes | Type of holiday (enum) |
| `colorHex` | string | Yes | Display color (e.g., "#DC143C") |
| `isPublicHoliday` | boolean | Yes | Whether it's a public day off |
| `culture` | string | Yes | Cultural context (default: "Vietnamese") |

**C# Model:**

```csharp
public class Holiday
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string NameResourceKey { get; set; } = string.Empty;
    public string DescriptionResourceKey { get; set; } = string.Empty;
    public int LunarMonth { get; set; }
    public int LunarDay { get; set; }
    public bool IsLeapMonth { get; set; }
    public int? GregorianMonth { get; set; }
    public int? GregorianDay { get; set; }
    public HolidayType Type { get; set; }
    public string ColorHex { get; set; } = string.Empty;
    public bool IsPublicHoliday { get; set; }
    public string Culture { get; set; } = "Vietnamese";
    
    // Computed property
    public bool HasLunarDate => LunarMonth > 0 && LunarDay > 0;
}

public enum HolidayType
{
    MajorHoliday = 1,
    TraditionalFestival = 2,
    SeasonalCelebration = 3,
    LunarSpecialDay = 4
}
```

**JSON Example (Lunar-based):**

```json
{
  "id": 1,
  "name": "Tết Nguyên Đán",
  "description": "Lunar New Year - The most important Vietnamese holiday",
  "nameResourceKey": "Holiday_TetNguyenDan1_Name",
  "descriptionResourceKey": "Holiday_TetNguyenDan1_Description",
  "lunarMonth": 1,
  "lunarDay": 1,
  "isLeapMonth": false,
  "gregorianMonth": null,
  "gregorianDay": null,
  "type": "MajorHoliday",
  "colorHex": "#DC143C",
  "isPublicHoliday": true,
  "culture": "Vietnamese"
}
```

**JSON Example (Gregorian-based):**

```json
{
  "id": 8,
  "name": "Quốc Khánh",
  "description": "National Day - Independence Day",
  "nameResourceKey": "Holiday_NationalDay_Name",
  "descriptionResourceKey": "Holiday_NationalDay_Description",
  "lunarMonth": 0,
  "lunarDay": 0,
  "isLeapMonth": false,
  "gregorianMonth": 9,
  "gregorianDay": 2,
  "type": "MajorHoliday",
  "colorHex": "#DC143C",
  "isPublicHoliday": true,
  "culture": "Vietnamese"
}
```

**Validation Rules:**
- `id`: Positive integer, unique
- `name`, `description`: Non-empty strings
- Either `lunarMonth`/`lunarDay` OR `gregorianMonth`/`gregorianDay` must be set
- `lunarMonth`: 1-12 if set
- `lunarDay`: 1-30 if set
- `gregorianMonth`: 1-12 if set
- `gregorianDay`: 1-31 if set
- `colorHex`: Valid hex color (e.g., "#RRGGBB")
- `type`: Valid HolidayType enum value

---

### 1.3 HolidayOccurrence

**Description:** A holiday occurrence on a specific date (calculated).

**Properties:**

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `holiday` | Holiday | Yes | Holiday definition |
| `gregorianDate` | DateTime | Yes | Date when holiday occurs |
| `animalSign` | string | Yes | Animal sign for that lunar year |

**Computed Properties:**

| Property | Type | Description |
|----------|------|-------------|
| `holidayName` | string | Shortcut to `holiday.name` |
| `holidayDescription` | string | Shortcut to `holiday.description` |
| `colorHex` | string | Shortcut to `holiday.colorHex` |
| `isPublicHoliday` | boolean | Shortcut to `holiday.isPublicHoliday` |
| `lunarDateDisplay` | string | Formatted lunar date with animal sign |

**C# Model:**

```csharp
public class HolidayOccurrence
{
    public Holiday Holiday { get; set; } = null!;
    public DateTime GregorianDate { get; set; }
    public string AnimalSign { get; set; } = string.Empty;

    // Computed properties (for iOS binding compatibility)
    public bool HasLunarDate => Holiday?.HasLunarDate ?? false;
    public string HolidayName => Holiday?.Name ?? string.Empty;
    public string HolidayDescription => Holiday?.Description ?? string.Empty;
    public string ColorHex => Holiday?.ColorHex ?? "#FF0000";
    public bool IsPublicHoliday => Holiday?.IsPublicHoliday ?? false;
    
    public string LunarDateDisplay
    {
        get
        {
            if (!HasLunarDate) return string.Empty;
            
            var lunarText = $"Lunar: {Holiday.LunarDay}/{Holiday.LunarMonth}";
            
            // Add animal sign for Tết holidays (1/1, 1/2, 1/3)
            if (!string.IsNullOrEmpty(AnimalSign) &&
                Holiday.LunarMonth == 1 &&
                Holiday.LunarDay >= 1 &&
                Holiday.LunarDay <= 3)
            {
                lunarText += $" - Year of the {AnimalSign}";
            }
            
            return lunarText;
        }
    }
}
```

**JSON Example:**

```json
{
  "holiday": {
    "id": 1,
    "name": "Tết Nguyên Đán",
    "description": "Lunar New Year - The most important Vietnamese holiday",
    "lunarMonth": 1,
    "lunarDay": 1,
    "type": "MajorHoliday",
    "colorHex": "#DC143C",
    "isPublicHoliday": true
  },
  "gregorianDate": "2026-02-17T00:00:00Z",
  "animalSign": "Horse",
  "lunarDateDisplay": "Lunar: 1/1 - Year of the Horse"
}
```

---

### 1.4 CalendarDay (UI Model)

**Description:** Represents a day in the calendar grid (UI layer).

**Properties:**

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `date` | DateTime | Yes | The date |
| `lunarDate` | LunarDate? | No | Lunar date info (null if not loaded) |
| `holidays` | List<HolidayOccurrence> | Yes | Holidays on this date |
| `isCurrentMonth` | boolean | Yes | Whether date is in current viewed month |
| `isToday` | boolean | Yes | Whether date is today |

**Computed Properties:**

| Property | Type | Description |
|----------|------|-------------|
| `hasHolidays` | boolean | Whether this date has any holidays |
| `holidayColor` | Color? | Color of first holiday (for indicator) |
| `lunarDateDisplay` | string | Formatted lunar date for display |

**C# Model:**

```csharp
public class CalendarDay
{
    public DateTime Date { get; set; }
    public LunarDate? LunarDate { get; set; }
    public List<HolidayOccurrence> Holidays { get; set; } = new();
    public bool IsCurrentMonth { get; set; }
    public bool IsToday { get; set; }
    
    // Computed properties
    public bool HasHolidays => Holidays?.Any() ?? false;
    
    public Color? HolidayColor => HasHolidays 
        ? Color.FromArgb(Holidays.First().ColorHex)
        : null;
    
    public string LunarDateDisplay => LunarDate != null
        ? $"{LunarDate.LunarDay}/{LunarDate.LunarMonth}"
        : string.Empty;
}
```

---

## 2. Database Schema

### 2.1 LunarDateEntity (SQLite)

**Table Name:** `LunarDateEntity`

**Purpose:** Cache calculated lunar dates for performance.

**Columns:**

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `Id` | INTEGER | PRIMARY KEY, AUTO INCREMENT | Unique identifier |
| `GregorianDate` | TEXT | UNIQUE, NOT NULL | ISO 8601 date string |
| `LunarYear` | INTEGER | NOT NULL | Lunar year |
| `LunarMonth` | INTEGER | NOT NULL | Lunar month |
| `LunarDay` | INTEGER | NOT NULL | Lunar day |
| `IsLeapMonth` | INTEGER | NOT NULL | Boolean (0/1) |
| `LunarYearName` | TEXT | NOT NULL | Chinese year name |
| `LunarMonthName` | TEXT | NOT NULL | Chinese month name |
| `LunarDayName` | TEXT | NOT NULL | Chinese day name |
| `AnimalSign` | TEXT | NOT NULL | Animal sign |
| `CreatedAt` | TEXT | DEFAULT CURRENT_TIMESTAMP | Creation timestamp |

**Indexes:**

```sql
CREATE INDEX idx_gregorian_date ON LunarDateEntity(GregorianDate);
```

**SQLite DDL:**

```sql
CREATE TABLE LunarDateEntity (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    GregorianDate TEXT NOT NULL UNIQUE,
    LunarYear INTEGER NOT NULL,
    LunarMonth INTEGER NOT NULL,
    LunarDay INTEGER NOT NULL,
    IsLeapMonth INTEGER NOT NULL,
    LunarYearName TEXT NOT NULL,
    LunarMonthName TEXT NOT NULL,
    LunarDayName TEXT NOT NULL,
    AnimalSign TEXT NOT NULL,
    CreatedAt TEXT DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_gregorian_date ON LunarDateEntity(GregorianDate);
```

**C# Entity (sqlite-net-pcl):**

```csharp
[Table("LunarDateEntity")]
public class LunarDateEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    [Unique]
    public DateTime GregorianDate { get; set; }
    
    public int LunarYear { get; set; }
    public int LunarMonth { get; set; }
    public int LunarDay { get; set; }
    public bool IsLeapMonth { get; set; }
    public string LunarYearName { get; set; } = string.Empty;
    public string LunarMonthName { get; set; } = string.Empty;
    public string LunarDayName { get; set; } = string.Empty;
    public string AnimalSign { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

**Usage Patterns:**

```csharp
// Insert or replace
await connection.InsertOrReplaceAsync(entity);

// Query by date
var entity = await connection.Table<LunarDateEntity>()
    .Where(e => e.GregorianDate == date.Date)
    .FirstOrDefaultAsync();

// Clear cache
await connection.DeleteAllAsync<LunarDateEntity>();
```

---

## 3. API Endpoints

**Base URL:** `https://api.lunarcalendar.app/api/v1`  
**Authentication:** JWT Bearer Token (for future features)  
**Content-Type:** `application/json`

---

### 3.1 Convert Gregorian to Lunar

**Endpoint:** `GET /calendar/convert`

**Description:** Convert a Gregorian date to lunar date information.

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `date` | DateTime | No | Today | Date to convert (ISO 8601 format) |

**Request Example:**

```http
GET /api/v1/calendar/convert?date=2026-02-17 HTTP/1.1
Host: api.lunarcalendar.app
Accept: application/json
```

**Response:** `200 OK`

```json
{
  "gregorianDate": "2026-02-17T00:00:00Z",
  "lunarYear": 2026,
  "lunarMonth": 1,
  "lunarDay": 1,
  "isLeapMonth": false,
  "lunarYearName": "丙午年",
  "lunarMonthName": "正月",
  "lunarDayName": "初一",
  "animalSign": "Horse",
  "heavenlyStem": "丙",
  "earthlyBranch": "午"
}
```

**Error Responses:**

- `400 Bad Request`: Invalid date format
- `422 Unprocessable Entity`: Date out of range (1901-2101)

---

### 3.2 Get Month Information

**Endpoint:** `GET /calendar/month/{year}/{month}`

**Description:** Get lunar calendar information for all days in a month.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `year` | int | Yes | Gregorian year (1901-2101) |
| `month` | int | Yes | Gregorian month (1-12) |

**Request Example:**

```http
GET /api/v1/calendar/month/2026/2 HTTP/1.1
Host: api.lunarcalendar.app
Accept: application/json
```

**Response:** `200 OK`

```json
[
  {
    "gregorianDate": "2026-02-01T00:00:00Z",
    "lunarYear": 2025,
    "lunarMonth": 12,
    "lunarDay": 14,
    "isLeapMonth": false,
    "lunarYearName": "乙巳年",
    "lunarMonthName": "腊月",
    "lunarDayName": "十四",
    "animalSign": "Snake"
  },
  {
    "gregorianDate": "2026-02-02T00:00:00Z",
    "lunarYear": 2025,
    "lunarMonth": 12,
    "lunarDay": 15,
    "isLeapMonth": false,
    "lunarYearName": "乙巳年",
    "lunarMonthName": "腊月",
    "lunarDayName": "十五",
    "animalSign": "Snake"
  }
  // ... 28 days total
]
```

**Caching:** Response cached for 24 hours.

**Error Responses:**

- `400 Bad Request`: Invalid year or month
- `422 Unprocessable Entity`: Date range not supported

---

### 3.3 Get Holidays for Year

**Endpoint:** `GET /holidays/year/{year}`

**Description:** Get all holiday occurrences for a specific year.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `year` | int | Yes | Gregorian year |

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `type` | HolidayType | No | All | Filter by holiday type |

**Request Example:**

```http
GET /api/v1/holidays/year/2026?type=MajorHoliday HTTP/1.1
Host: api.lunarcalendar.app
Accept: application/json
```

**Response:** `200 OK`

```json
[
  {
    "holiday": {
      "id": 1,
      "name": "Tết Nguyên Đán",
      "description": "Lunar New Year - The most important Vietnamese holiday",
      "nameResourceKey": "Holiday_TetNguyenDan1_Name",
      "descriptionResourceKey": "Holiday_TetNguyenDan1_Description",
      "lunarMonth": 1,
      "lunarDay": 1,
      "isLeapMonth": false,
      "gregorianMonth": null,
      "gregorianDay": null,
      "type": "MajorHoliday",
      "colorHex": "#DC143C",
      "isPublicHoliday": true,
      "culture": "Vietnamese"
    },
    "gregorianDate": "2026-02-17T00:00:00Z",
    "animalSign": "Horse"
  },
  {
    "holiday": {
      "id": 5,
      "name": "Tết Dương Lịch",
      "description": "New Year's Day",
      "nameResourceKey": "Holiday_NewYear_Name",
      "descriptionResourceKey": "Holiday_NewYear_Description",
      "lunarMonth": 0,
      "lunarDay": 0,
      "isLeapMonth": false,
      "gregorianMonth": 1,
      "gregorianDay": 1,
      "type": "MajorHoliday",
      "colorHex": "#DC143C",
      "isPublicHoliday": true,
      "culture": "Vietnamese"
    },
    "gregorianDate": "2026-01-01T00:00:00Z",
    "animalSign": "Snake"
  }
  // ... more holidays
]
```

**Caching:** Response cached for 1 hour.

---

### 3.4 Get Holidays for Month

**Endpoint:** `GET /holidays/month/{year}/{month}`

**Description:** Get holidays occurring in a specific month.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `year` | int | Yes | Gregorian year |
| `month` | int | Yes | Gregorian month (1-12) |

**Request Example:**

```http
GET /api/v1/holidays/month/2026/2 HTTP/1.1
Host: api.lunarcalendar.app
Accept: application/json
```

**Response:** `200 OK`

```json
[
  {
    "holiday": {
      "id": 1,
      "name": "Tết Nguyên Đán",
      "type": "MajorHoliday",
      "colorHex": "#DC143C",
      "isPublicHoliday": true
    },
    "gregorianDate": "2026-02-17T00:00:00Z",
    "animalSign": "Horse"
  },
  {
    "holiday": {
      "id": 2,
      "name": "Tết Nguyên Đán (Day 2)",
      "type": "MajorHoliday",
      "colorHex": "#DC143C",
      "isPublicHoliday": true
    },
    "gregorianDate": "2026-02-18T00:00:00Z",
    "animalSign": "Horse"
  }
  // ... more holidays in February
]
```

---

### 3.5 Get All Holiday Definitions

**Endpoint:** `GET /holidays`

**Description:** Get all holiday definitions (static data).

**Request Example:**

```http
GET /api/v1/holidays HTTP/1.1
Host: api.lunarcalendar.app
Accept: application/json
```

**Response:** `200 OK`

```json
[
  {
    "id": 1,
    "name": "Tết Nguyên Đán",
    "description": "Lunar New Year - The most important Vietnamese holiday",
    "nameResourceKey": "Holiday_TetNguyenDan1_Name",
    "descriptionResourceKey": "Holiday_TetNguyenDan1_Description",
    "lunarMonth": 1,
    "lunarDay": 1,
    "isLeapMonth": false,
    "type": "MajorHoliday",
    "colorHex": "#DC143C",
    "isPublicHoliday": true,
    "culture": "Vietnamese"
  }
  // ... 45+ holidays
]
```

**Caching:** Response cached for 7 days (static data).

---

### 3.6 Health Check

**Endpoint:** `GET /health`

**Description:** API health check endpoint.

**Request Example:**

```http
GET /api/v1/health HTTP/1.1
Host: api.lunarcalendar.app
Accept: application/json
```

**Response:** `200 OK`

```json
{
  "status": "Healthy",
  "timestamp": "2026-01-03T10:30:00.000Z",
  "version": "1.0.0",
  "environment": "Production",
  "uptime": 3600
}
```

---

## 4. Request/Response Examples

### 4.1 Example 1: Converting Today's Date

**Scenario:** User opens app and needs today's lunar date.

**Request:**

```http
GET /api/v1/calendar/convert HTTP/1.1
Host: api.lunarcalendar.app
Accept: application/json
```

**Response:**

```json
{
  "gregorianDate": "2026-01-03T00:00:00Z",
  "lunarYear": 2025,
  "lunarMonth": 11,
  "lunarDay": 15,
  "isLeapMonth": false,
  "lunarYearName": "乙巳年",
  "lunarMonthName": "冬月",
  "lunarDayName": "十五",
  "animalSign": "Snake",
  "heavenlyStem": "乙",
  "earthlyBranch": "巳"
}
```

---

### 4.2 Example 2: Loading February 2026 Calendar

**Scenario:** User navigates to February 2026.

**Step 1: Get Lunar Dates**

```http
GET /api/v1/calendar/month/2026/2 HTTP/1.1
```

**Response:** Array of 28 LunarDate objects (see section 3.2)

**Step 2: Get Holidays**

```http
GET /api/v1/holidays/month/2026/2 HTTP/1.1
```

**Response:** Array of HolidayOccurrence objects for February

**Step 3: Combine Data**

Mobile app merges lunar dates with holidays to build calendar grid.

---

### 4.3 Example 3: Viewing Year Holidays

**Scenario:** User views all holidays for 2026.

**Request:**

```http
GET /api/v1/holidays/year/2026 HTTP/1.1
Host: api.lunarcalendar.app
Accept: application/json
```

**Response:** Array of all HolidayOccurrence objects for 2026 (40-50 items)

---

## 5. Error Handling

### 5.1 Error Response Format

**Standard Error Response:**

```json
{
  "error": {
    "code": "INVALID_DATE",
    "message": "The provided date is invalid or out of supported range.",
    "details": {
      "field": "date",
      "value": "1800-01-01",
      "constraint": "Date must be between 1901-02-19 and 2101-01-28"
    },
    "timestamp": "2026-01-03T10:30:00.000Z",
    "requestId": "req_abc123"
  }
}
```

---

### 5.2 Error Codes

| HTTP Status | Error Code | Description |
|-------------|------------|-------------|
| 400 | `INVALID_REQUEST` | Malformed request or missing required parameters |
| 400 | `INVALID_DATE_FORMAT` | Date format is incorrect |
| 401 | `UNAUTHORIZED` | Missing or invalid authentication token |
| 404 | `NOT_FOUND` | Resource not found |
| 422 | `INVALID_DATE_RANGE` | Date is out of supported range (1901-2101) |
| 422 | `INVALID_PARAMETERS` | Parameters are syntactically correct but semantically invalid |
| 429 | `RATE_LIMIT_EXCEEDED` | Too many requests |
| 500 | `INTERNAL_SERVER_ERROR` | Unexpected server error |
| 503 | `SERVICE_UNAVAILABLE` | Service temporarily unavailable |

---

### 5.3 Example Error Responses

**Invalid Date Format:**

```http
HTTP/1.1 400 Bad Request
Content-Type: application/json
```

```json
{
  "error": {
    "code": "INVALID_DATE_FORMAT",
    "message": "Date must be in ISO 8601 format (YYYY-MM-DD).",
    "details": {
      "field": "date",
      "value": "2026/01/03",
      "expected": "2026-01-03"
    }
  }
}
```

**Date Out of Range:**

```http
HTTP/1.1 422 Unprocessable Entity
Content-Type: application/json
```

```json
{
  "error": {
    "code": "INVALID_DATE_RANGE",
    "message": "Date is outside the supported range.",
    "details": {
      "field": "date",
      "value": "1800-01-01",
      "minDate": "1901-02-19",
      "maxDate": "2101-01-28"
    }
  }
}
```

---

## 6. Data Validation Rules

### 6.1 Date Validation

**Gregorian Dates:**
- Format: ISO 8601 (YYYY-MM-DD or YYYY-MM-DDTHH:MM:SSZ)
- Range: 1901-02-19 to 2101-01-28
- Time component ignored (dates only)

**Lunar Dates:**
- Year: 1900-2100
- Month: 1-12
- Day: 1-30 (depends on month)
- Leap month: boolean

---

### 6.2 Holiday Validation

**Lunar-based Holidays:**
- Must have `lunarMonth` and `lunarDay` > 0
- `gregorianMonth` and `gregorianDay` must be null

**Gregorian-based Holidays:**
- Must have `gregorianMonth` and `gregorianDay` set
- `lunarMonth` and `lunarDay` must be 0

**Common Fields:**
- `id`: Positive integer
- `name`: 1-100 characters
- `description`: 1-500 characters
- `colorHex`: Valid hex color (#RRGGBB format)
- `type`: Valid HolidayType enum value

---

### 6.3 Query Parameter Validation

**Year Parameter:**
- Type: Integer
- Range: 1901-2101
- Example: `?year=2026`

**Month Parameter:**
- Type: Integer
- Range: 1-12
- Example: `?month=2`

**Date Parameter:**
- Type: DateTime (ISO 8601)
- Range: 1901-02-19 to 2101-01-28
- Example: `?date=2026-02-17`

**Type Parameter:**
- Type: String (HolidayType enum)
- Values: `MajorHoliday`, `TraditionalFestival`, `SeasonalCelebration`, `LunarSpecialDay`
- Example: `?type=MajorHoliday`

---

## Appendix A: Complete Holiday List (45 items)

### Major Holidays (8)
1. Tết Nguyên Đán (Day 1) - Lunar 1/1
2. Tết Nguyên Đán (Day 2) - Lunar 1/2
3. Tết Nguyên Đán (Day 3) - Lunar 1/3
4. Giỗ Tổ Hùng Vương - Lunar 3/10
5. Tết Dương Lịch - January 1
6. Ngày Giải Phóng Miền Nam - April 30
7. Ngày Quốc Tế Lao Động - May 1
8. Quốc Khánh - September 2

### Traditional Festivals (12)
9. Tết Nguyên Tiêu - Lunar 1/15
10. Tết Hàn Thực - Lunar 3/3
11. Tết Đoan Ngọ - Lunar 5/5
12. Vu Lan - Lunar 7/15
13. Tết Trung Thu - Lunar 8/15
14. Tết Trùng Cửu - Lunar 9/9
15. Lễ Cúng Ông Công Ông Táo - Lunar 12/23
16. Tết Ông Bà - Lunar 12/30
... (and more)

### Lunar Special Days (24)
- Mùng 1 of each month (12 days)
- Rằm (15th) of each month (12 days)

---

## Appendix B: Localization Resource Keys

### Holiday Names
- `Holiday_TetNguyenDan1_Name`
- `Holiday_TetNguyenDan2_Name`
- `Holiday_HungKings_Name`
- `Holiday_NewYear_Name`
- `Holiday_ReunificationDay_Name`
- `Holiday_LaborDay_Name`
- `Holiday_NationalDay_Name`
... (45+ keys)

### Holiday Descriptions
- `Holiday_TetNguyenDan1_Description`
- `Holiday_TetNguyenDan2_Description`
... (45+ keys)

### UI Strings
- `Calendar_Title`
- `Today`
- `Holidays`
- `Settings`
- `Language`
... (50+ keys)

---

**Document End**

This reference provides complete documentation of all data models, database schemas, and API endpoints for the Vietnamese Lunar Calendar application. Use this as a definitive guide for any implementation (Flutter, React Native, native iOS/Android, etc.).
