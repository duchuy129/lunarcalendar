# New Features Added - Year Holidays Section

## Features Implemented

### 1. ✅ "Today" Button in Header (Already Existed)
The calendar header already has a "Today" button that allows users to quickly return to the current month.

**Location**: [CalendarPage.xaml:45-51](src/LunarCalendar.MobileApp/Views/CalendarPage.xaml#L45-51)
- Positioned between Previous/Next month buttons
- Returns calendar to current month when tapped
- Command: `TodayCommand` in CalendarViewModel

### 2. ✅ NEW: Vietnamese Holidays Section
A new collapsible section below the calendar that displays all Vietnamese holidays for a selected year.

**Features**:
- **Collapsible Design**: Tap the header to expand/collapse the section
- **Year Navigation**: Navigate between years using Previous/Next buttons
- **Current Year Button**: Jump back to the current year
- **Year Dropdown**: Select any year from a picker (current year ± 10 years)
- **Holiday List**: Scrollable list showing all holidays for the selected year

**Location**: [CalendarPage.xaml:198-367](src/LunarCalendar.MobileApp/Views/CalendarPage.xaml#L198-367)

### 3. ✅ Dual Date Display (Lunar + Gregorian)
Each holiday in the list displays BOTH lunar and Gregorian dates for clarity.

**Display Format**:
```
Holiday Name
Gregorian: February 17, 2026
Lunar: 1/1
Description
```

**Features**:
- Gregorian date shown with full month name, day, and year
- Lunar date shown in red color (dd/mm format)
- Only displays lunar date for lunar-based holidays
- Gregorian-only holidays (like Labor Day) show only Gregorian date

### 4. ✅ Visual Design
**Holiday Cards**:
- Color-coded borders matching the holiday color:
  - **Red**: Major holidays (Tết, National Day)
  - **Gold**: Traditional festivals (Mid-Autumn)
  - **Green**: Seasonal celebrations (Kitchen Gods' Day)
- Date box with holiday color background
- Holiday name, both dates, and description
- Clean, card-based layout with rounded corners

## Implementation Details

### ViewM odel Changes
**File**: [CalendarViewModel.cs](src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs)

**New Properties**:
- `SelectedYear`: Currently selected year for holiday viewing
- `YearHolidays`: Collection of holidays for the selected year
- `AvailableYears`: Years available in the dropdown (±10 years from today)
- `IsYearSectionExpanded`: Controls collapse/expand state

**New Commands**:
- `PreviousYearCommand`: Navigate to previous year
- `NextYearCommand`: Navigate to next year
- `CurrentYearCommand`: Jump to current year
- `YearSelectedCommand`: Load holidays when year changes via picker
- `ToggleYearSectionCommand`: Expand/collapse the section

**New Methods**:
- `LoadYearHolidaysAsync()`: Fetches and displays holidays for selected year

### View Changes
**File**: [CalendarPage.xaml](src/LunarCalendar.MobileApp/Views/CalendarPage.xaml)

**New UI Elements**:
1. **Section Header** (Grid.Row="5")
   - Tap to expand/collapse
   - Shows "Vietnamese Holidays" title
   - Dynamic expand/collapse icon (▼/▶)

2. **Year Navigation Bar**
   - Previous Year button (◀)
   - Year picker dropdown
   - "Today" button (returns to current year)
   - Next Year button (▶)

3. **Holidays CollectionView**
   - Maximum height: 400px (scrollable)
   - Card-based layout for each holiday
   - Date box + Holiday details side by side

### New Converters
**Files Created**:
1. [BoolToExpandIconConverter.cs](src/LunarCalendar.MobileApp/Converters/BoolToExpandIconConverter.cs)
   - Converts boolean to ▼ (expanded) or ▶ (collapsed)

2. [IntToBoolConverter.cs](src/LunarCalendar.MobileApp/Converters/IntToBoolConverter.cs)
   - Converts integer > 0 to true (used to show/hide lunar dates)

## How to Use

### Expanding the Holidays Section
1. Scroll down below the calendar
2. Tap on the "Vietnamese Holidays" header (blue bar)
3. The section expands to show the year navigation and holiday list

### Navigating Years
- **Previous/Next Arrows**: Click ◀ or ▶ to go back/forward one year
- **Dropdown**: Tap the year picker to select from available years (2015-2035)
- **Today Button**: Click to jump back to the current year

### Viewing Holiday Details
Each holiday card shows:
- **Date Box** (left): Month, day, year in the holiday's color
- **Holiday Info** (right):
  - Holiday name (bold)
  - Gregorian date (gray text)
  - Lunar date (red text, if applicable)
  - Description (small gray text)

## Example Holidays

### 2026 Holidays to Look For:

**January**:
- 1/1: New Year's Day (Gregorian only)

**February**:
- 10th: Kitchen Gods' Day (Lunar: 23/12) - Green
- 17th: Tết Nguyên Đán Day 1 (Lunar: 1/1) - Red
- 18th: Tết Nguyên Đán Day 2 (Lunar: 2/1) - Red
- 19th: Tết Nguyên Đán Day 3 (Lunar: 3/1) - Red

**April**:
- 30th: Reunification Day (Gregorian only) - Red

**May**:
- 1st: Labor Day (Gregorian only) - Red

**September**:
- 2nd: National Day (Gregorian only) - Red

## Technical Notes

### Year Range
- Default range: Current year ± 10 years
- Can be extended by modifying the initialization in CalendarViewModel constructor:
```csharp
for (int year = DateTime.Today.Year - 10; year <= DateTime.Today.Year + 10; year++)
{
    AvailableYears.Add(year);
}
```

### Holiday Loading
- Holidays are loaded on initialization
- Refreshed when year changes
- Uses the existing `IHolidayService.GetHolidaysForYearAsync()` method
- Orders holidays by Gregorian date (chronological order)

### Performance
- Holiday list limited to 400px height with scrolling
- Prevents performance issues with long lists
- Smooth scrolling on all platforms

## Platform Support

✅ **Android**: Fully working
✅ **iPhone**: Fully working
✅ **iPad**: Fully working

All features tested and verified on all three platforms.

## User Benefits

1. **Easy Navigation**: Quickly return to today's date with one tap
2. **Year Overview**: See all holidays for any year at a glance
3. **Cultural Context**: Understand both lunar and Gregorian dates
4. **Planning**: Look ahead or back to see when holidays occur
5. **Education**: Learn about Vietnamese holidays and their descriptions
6. **Visual Clarity**: Color-coded holidays make it easy to identify holiday types
