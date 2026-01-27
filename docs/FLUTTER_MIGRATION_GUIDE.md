# Vietnamese Lunar Calendar Application
## Flutter Migration Guide

**Version:** 2.0  
**Date:** January 3, 2026  
**From:** .NET MAUI  
**To:** Flutter 3.x+

---

## Table of Contents

1. [Migration Overview](#migration-overview)
2. [Technology Mapping](#technology-mapping)
3. [Project Structure](#project-structure)
4. [Core Algorithm Implementation](#core-algorithm-implementation)
5. [State Management](#state-management)
6. [UI Implementation](#ui-implementation)
7. [Localization](#localization)
8. [Data Persistence](#data-persistence)
9. [Platform-Specific Code](#platform-specific-code)
10. [Package Recommendations](#package-recommendations)
11. [Migration Strategy](#migration-strategy)

---

## 1. Migration Overview

### 1.1 Why Flutter?

**Advantages:**
- ✅ Hot reload for faster development
- ✅ Rich widget ecosystem
- ✅ Excellent performance (Skia rendering)
- ✅ Strong community support
- ✅ Better UI customization
- ✅ Smaller app size compared to .NET MAUI

**Considerations:**
- ⚠️ Learning curve (Dart language)
- ⚠️ Need to rewrite all UI code
- ⚠️ Different state management patterns
- ⚠️ Different navigation system

### 1.2 Migration Effort Estimate

| Component | Complexity | Estimated Time |
|-----------|------------|----------------|
| Core algorithms (lunar calculation) | Medium | 3-5 days |
| UI implementation | High | 10-15 days |
| State management | Medium | 3-5 days |
| Localization | Low | 2-3 days |
| Data persistence | Low | 2-3 days |
| Platform-specific code | Low | 2-3 days |
| Testing & debugging | Medium | 5-7 days |
| **Total** | | **27-43 days** |

---

## 2. Technology Mapping

### 2.1 Framework Comparison

| .NET MAUI | Flutter | Notes |
|-----------|---------|-------|
| C# | Dart | Similar syntax, easier learning curve |
| XAML | Widgets | Declarative UI, but code-based |
| Dependency Injection | Provider/GetIt | Different DI approach |
| MVVM Pattern | BLoC/Provider/Riverpod | Multiple state management options |
| SQLite (sqlite-net-pcl) | sqflite | Similar API |
| Preferences | shared_preferences | Key-value storage |
| Resource files (.resx) | intl/easy_localization | JSON-based localization |
| NavigationPage | Navigator | Different navigation API |
| ObservableCollection | ChangeNotifier | Reactive state |
| ICommand | VoidCallback | Simpler callback approach |

---

### 2.2 Package Equivalents

| .NET MAUI Package | Flutter Package | Purpose |
|-------------------|-----------------|---------|
| CommunityToolkit.Mvvm | provider 6.1.0 | State management |
| sqlite-net-pcl | sqflite 2.3.0 | SQLite database |
| (built-in) | shared_preferences 2.2.2 | Persistent key-value storage |
| (built-in) | intl 0.18.1 | Internationalization |
| (built-in) | path_provider 2.1.1 | File system paths |
| (built-in) | vibration 1.8.4 | Haptic feedback |

---

## 3. Project Structure

### 3.1 Recommended Flutter Structure

```
lunar_calendar/
├── lib/
│   ├── main.dart                      # App entry point
│   ├── app.dart                       # MaterialApp configuration
│   │
│   ├── core/                          # Core business logic
│   │   ├── models/
│   │   │   ├── lunar_date.dart
│   │   │   ├── holiday.dart
│   │   │   └── holiday_occurrence.dart
│   │   ├── services/
│   │   │   ├── lunar_calculation_service.dart
│   │   │   ├── holiday_calculation_service.dart
│   │   │   └── holiday_seeder.dart
│   │   └── constants/
│   │       ├── heavenly_stems.dart
│   │       ├── earthly_branches.dart
│   │       └── animal_signs.dart
│   │
│   ├── features/                      # Feature modules
│   │   ├── calendar/
│   │   │   ├── presentation/
│   │   │   │   ├── calendar_screen.dart
│   │   │   │   ├── widgets/
│   │   │   │   │   ├── calendar_grid.dart
│   │   │   │   │   ├── calendar_day_cell.dart
│   │   │   │   │   └── month_header.dart
│   │   │   │   └── providers/
│   │   │   │       └── calendar_provider.dart
│   │   │   ├── data/
│   │   │   │   ├── repositories/
│   │   │   │   │   └── calendar_repository.dart
│   │   │   │   └── datasources/
│   │   │   │       ├── calendar_local_datasource.dart
│   │   │   │       └── calendar_cache_datasource.dart
│   │   │   └── domain/
│   │   │       ├── entities/
│   │   │       └── usecases/
│   │   │
│   │   ├── holidays/
│   │   │   ├── presentation/
│   │   │   │   ├── year_holidays_screen.dart
│   │   │   │   ├── holiday_detail_screen.dart
│   │   │   │   └── providers/
│   │   │   │       └── holiday_provider.dart
│   │   │   └── data/
│   │   │
│   │   └── settings/
│   │       ├── presentation/
│   │       │   ├── settings_screen.dart
│   │       │   └── providers/
│   │       │       ├── settings_provider.dart
│   │       │       └── localization_provider.dart
│   │       └── data/
│   │
│   ├── shared/                        # Shared utilities
│   │   ├── widgets/
│   │   │   ├── loading_indicator.dart
│   │   │   └── error_widget.dart
│   │   ├── helpers/
│   │   │   ├── date_formatter.dart
│   │   │   └── color_helper.dart
│   │   ├── extensions/
│   │   │   └── datetime_extensions.dart
│   │   └── theme/
│   │       ├── app_colors.dart
│   │       ├── app_text_styles.dart
│   │       └── app_theme.dart
│   │
│   └── l10n/                          # Localization
│       ├── app_en.arb
│       ├── app_vi.arb
│       └── l10n.dart (generated)
│
├── assets/                            # Assets
│   ├── images/
│   ├── fonts/
│   └── icons/
│
├── test/                              # Unit tests
│   ├── core/
│   └── features/
│
├── pubspec.yaml                       # Dependencies
└── l10n.yaml                          # Localization config
```

---

## 4. Core Algorithm Implementation

### 4.1 Lunar Calculation Service (Dart)

**File:** `lib/core/services/lunar_calculation_service.dart`

```dart
import 'package:lunar_calendar_dart/lunar_calendar_dart.dart';
import 'package:lunar_calendar/core/models/lunar_date.dart';
import 'package:lunar_calendar/core/constants/heavenly_stems.dart';
import 'package:lunar_calendar/core/constants/earthly_branches.dart';
import 'package:lunar_calendar/core/constants/animal_signs.dart';

class LunarCalculationService {
  /// Convert Gregorian date to Lunar date
  /// Uses lunar_calendar_dart package for accurate calculations
  LunarDate convertToLunar(DateTime gregorianDate) {
    try {
      // Use lunar_calendar_dart package
      final lunar = Lunar.fromDate(gregorianDate);
      
      // Calculate cycle components
      final lunarYear = lunar.year;
      final heavenlyStem = HeavenlyStems.values[(lunarYear - 4) % 10];
      final earthlyBranch = EarthlyBranches.values[(lunarYear - 4) % 12];
      final animalSign = AnimalSigns.values[(lunarYear - 4) % 12];
      
      return LunarDate(
        gregorianDate: gregorianDate,
        lunarYear: lunar.year,
        lunarMonth: lunar.month,
        lunarDay: lunar.day,
        isLeapMonth: lunar.isLeapMonth,
        heavenlyStem: heavenlyStem,
        earthlyBranch: earthlyBranch,
        animalSign: animalSign,
      );
    } catch (e) {
      // Return default values if conversion fails
      return LunarDate(
        gregorianDate: gregorianDate,
        lunarYear: gregorianDate.year,
        lunarMonth: gregorianDate.month,
        lunarDay: gregorianDate.day,
        isLeapMonth: false,
        heavenlyStem: HeavenlyStems.jia,
        earthlyBranch: EarthlyBranches.zi,
        animalSign: AnimalSigns.rat,
      );
    }
  }
  
  /// Convert Lunar date to Gregorian date
  DateTime convertToGregorian({
    required int year,
    required int month,
    required int day,
    bool isLeapMonth = false,
  }) {
    final solar = Solar.fromLunar(
      Lunar.fromYmd(year, month, day, isLeapMonth: isLeapMonth),
    );
    return DateTime(solar.year, solar.month, solar.day);
  }
  
  /// Get lunar dates for entire month
  List<LunarDate> getMonthInfo(int year, int month) {
    final daysInMonth = DateTime(year, month + 1, 0).day;
    final results = <LunarDate>[];
    
    for (int day = 1; day <= daysInMonth; day++) {
      final gregorianDate = DateTime(year, month, day);
      results.add(convertToLunar(gregorianDate));
    }
    
    return results;
  }
}
```

**Alternative:** If `lunar_calendar_dart` doesn't exist, you can:
1. Port the .NET ChineseLunisolarCalendar algorithm to Dart
2. Use a JavaScript library with JS interop
3. Create platform channels to call native code

---

### 4.2 Data Models (Dart)

**File:** `lib/core/models/lunar_date.dart`

```dart
import 'package:lunar_calendar/core/constants/heavenly_stems.dart';
import 'package:lunar_calendar/core/constants/earthly_branches.dart';
import 'package:lunar_calendar/core/constants/animal_signs.dart';

class LunarDate {
  final DateTime gregorianDate;
  final int lunarYear;
  final int lunarMonth;
  final int lunarDay;
  final bool isLeapMonth;
  final HeavenlyStems heavenlyStem;
  final EarthlyBranches earthlyBranch;
  final AnimalSigns animalSign;
  
  const LunarDate({
    required this.gregorianDate,
    required this.lunarYear,
    required this.lunarMonth,
    required this.lunarDay,
    required this.isLeapMonth,
    required this.heavenlyStem,
    required this.earthlyBranch,
    required this.animalSign,
  });
  
  // Computed properties
  String get lunarYearName => 
      '${heavenlyStem.chineseCharacter}${earthlyBranch.chineseCharacter}年';
  
  String get lunarMonthName {
    const monthNames = [
      '正月', '二月', '三月', '四月', '五月', '六月',
      '七月', '八月', '九月', '十月', '冬月', '腊月'
    ];
    final baseName = monthNames[lunarMonth - 1];
    return isLeapMonth ? '闰$baseName' : baseName;
  }
  
  String get lunarDayName {
    const dayNames = [
      '初一', '初二', '初三', '初四', '初五', '初六', '初七', '初八', '初九', '初十',
      '十一', '十二', '十三', '十四', '十五', '十六', '十七', '十八', '十九', '二十',
      '廿一', '廿二', '廿三', '廿四', '廿五', '廿六', '廿七', '廿八', '廿九', '三十'
    ];
    return dayNames[lunarDay - 1];
  }
  
  // JSON serialization
  Map<String, dynamic> toJson() => {
    'gregorianDate': gregorianDate.toIso8601String(),
    'lunarYear': lunarYear,
    'lunarMonth': lunarMonth,
    'lunarDay': lunarDay,
    'isLeapMonth': isLeapMonth,
    'heavenlyStem': heavenlyStem.index,
    'earthlyBranch': earthlyBranch.index,
    'animalSign': animalSign.index,
  };
  
  factory LunarDate.fromJson(Map<String, dynamic> json) => LunarDate(
    gregorianDate: DateTime.parse(json['gregorianDate']),
    lunarYear: json['lunarYear'],
    lunarMonth: json['lunarMonth'],
    lunarDay: json['lunarDay'],
    isLeapMonth: json['isLeapMonth'],
    heavenlyStem: HeavenlyStems.values[json['heavenlyStem']],
    earthlyBranch: EarthlyBranches.values[json['earthlyBranch']],
    animalSign: AnimalSigns.values[json['animalSign']],
  );
}
```

---

**File:** `lib/core/models/holiday.dart`

```dart
enum HolidayType {
  majorHoliday,
  traditionalFestival,
  seasonalCelebration,
  lunarSpecialDay,
}

class Holiday {
  final int id;
  final String name;
  final String description;
  final String nameResourceKey;
  final String descriptionResourceKey;
  final int lunarMonth;
  final int lunarDay;
  final bool isLeapMonth;
  final int? gregorianMonth;
  final int? gregorianDay;
  final HolidayType type;
  final String colorHex;
  final bool isPublicHoliday;
  final String culture;
  
  const Holiday({
    required this.id,
    required this.name,
    required this.description,
    required this.nameResourceKey,
    required this.descriptionResourceKey,
    required this.lunarMonth,
    required this.lunarDay,
    required this.isLeapMonth,
    this.gregorianMonth,
    this.gregorianDay,
    required this.type,
    required this.colorHex,
    required this.isPublicHoliday,
    this.culture = 'Vietnamese',
  });
  
  bool get hasLunarDate => lunarMonth > 0 && lunarDay > 0;
  
  // Color getter
  Color get color => Color(int.parse(colorHex.replaceFirst('#', '0xFF')));
}

class HolidayOccurrence {
  final Holiday holiday;
  final DateTime gregorianDate;
  final String animalSign;
  
  const HolidayOccurrence({
    required this.holiday,
    required this.gregorianDate,
    required this.animalSign,
  });
  
  String get holidayName => holiday.name;
  String get holidayDescription => holiday.description;
  Color get color => holiday.color;
  bool get isPublicHoliday => holiday.isPublicHoliday;
}
```

---

## 5. State Management

### 5.1 Recommended: Provider Pattern

**Why Provider?**
- ✅ Official Flutter recommendation
- ✅ Easy to learn
- ✅ Good performance
- ✅ Works well with ChangeNotifier

### 5.2 CalendarProvider Implementation

**File:** `lib/features/calendar/presentation/providers/calendar_provider.dart`

```dart
import 'package:flutter/foundation.dart';
import 'package:lunar_calendar/core/models/lunar_date.dart';
import 'package:lunar_calendar/core/models/holiday.dart';
import 'package:lunar_calendar/features/calendar/data/repositories/calendar_repository.dart';
import 'package:lunar_calendar/features/holidays/data/repositories/holiday_repository.dart';

class CalendarProvider with ChangeNotifier {
  final CalendarRepository _calendarRepository;
  final HolidayRepository _holidayRepository;
  
  CalendarProvider({
    required CalendarRepository calendarRepository,
    required HolidayRepository holidayRepository,
  })  : _calendarRepository = calendarRepository,
        _holidayRepository = holidayRepository {
    _initialize();
  }
  
  // State
  DateTime _currentMonth = DateTime.now();
  LunarDate? _todayLunarDate;
  List<CalendarDay> _calendarDays = [];
  List<HolidayOccurrence> _upcomingHolidays = [];
  bool _isLoading = false;
  String? _error;
  
  // Getters
  DateTime get currentMonth => _currentMonth;
  LunarDate? get todayLunarDate => _todayLunarDate;
  List<CalendarDay> get calendarDays => _calendarDays;
  List<HolidayOccurrence> get upcomingHolidays => _upcomingHolidays;
  bool get isLoading => _isLoading;
  String? get error => _error;
  
  String get monthYearDisplay => 
      DateFormatter.formatMonthYear(_currentMonth);
  
  // Actions
  Future<void> _initialize() async {
    await loadToday();
    await loadMonth();
    await loadUpcomingHolidays();
  }
  
  Future<void> loadToday() async {
    try {
      _todayLunarDate = await _calendarRepository.getLunarDate(DateTime.now());
      notifyListeners();
    } catch (e) {
      _error = e.toString();
      notifyListeners();
    }
  }
  
  Future<void> loadMonth() async {
    try {
      _isLoading = true;
      _error = null;
      notifyListeners();
      
      // Get lunar dates for month
      final lunarDates = await _calendarRepository.getMonthLunarDates(
        _currentMonth.year,
        _currentMonth.month,
      );
      
      // Get holidays for month
      final holidays = await _holidayRepository.getHolidaysForMonth(
        _currentMonth.year,
        _currentMonth.month,
      );
      
      // Build calendar grid
      _calendarDays = _buildCalendarGrid(lunarDates, holidays);
      
      _isLoading = false;
      notifyListeners();
    } catch (e) {
      _isLoading = false;
      _error = e.toString();
      notifyListeners();
    }
  }
  
  Future<void> previousMonth() async {
    _currentMonth = DateTime(_currentMonth.year, _currentMonth.month - 1);
    await loadMonth();
  }
  
  Future<void> nextMonth() async {
    _currentMonth = DateTime(_currentMonth.year, _currentMonth.month + 1);
    await loadMonth();
  }
  
  Future<void> goToToday() async {
    _currentMonth = DateTime.now();
    await loadMonth();
  }
  
  Future<void> loadUpcomingHolidays() async {
    try {
      _upcomingHolidays = await _holidayRepository.getUpcomingHolidays(5);
      notifyListeners();
    } catch (e) {
      _error = e.toString();
      notifyListeners();
    }
  }
  
  List<CalendarDay> _buildCalendarGrid(
    List<LunarDate> lunarDates,
    List<HolidayOccurrence> holidays,
  ) {
    // Implementation similar to .NET MAUI version
    final calendarDays = <CalendarDay>[];
    
    final firstDayOfMonth = DateTime(_currentMonth.year, _currentMonth.month, 1);
    final daysInMonth = DateTime(_currentMonth.year, _currentMonth.month + 1, 0).day;
    final startDayOfWeek = firstDayOfMonth.weekday % 7; // Convert to 0-6 (Sun-Sat)
    
    // Previous month filler
    final previousMonth = DateTime(_currentMonth.year, _currentMonth.month - 1);
    final daysInPreviousMonth = DateTime(previousMonth.year, previousMonth.month + 1, 0).day;
    
    for (int i = startDayOfWeek - 1; i >= 0; i--) {
      final day = daysInPreviousMonth - i;
      calendarDays.add(CalendarDay(
        date: DateTime(previousMonth.year, previousMonth.month, day),
        isCurrentMonth: false,
      ));
    }
    
    // Current month days
    for (int day = 1; day <= daysInMonth; day++) {
      final date = DateTime(_currentMonth.year, _currentMonth.month, day);
      final lunarDate = lunarDates.firstWhere(
        (l) => l.gregorianDate.day == day,
        orElse: () => null,
      );
      final dayHolidays = holidays
          .where((h) => h.gregorianDate.day == day)
          .toList();
      
      calendarDays.add(CalendarDay(
        date: date,
        lunarDate: lunarDate,
        holidays: dayHolidays,
        isCurrentMonth: true,
        isToday: date.year == DateTime.now().year &&
                 date.month == DateTime.now().month &&
                 date.day == DateTime.now().day,
      ));
    }
    
    // Next month filler
    final remainingCells = 42 - calendarDays.length;
    final nextMonth = DateTime(_currentMonth.year, _currentMonth.month + 1);
    
    for (int day = 1; day <= remainingCells; day++) {
      calendarDays.add(CalendarDay(
        date: DateTime(nextMonth.year, nextMonth.month, day),
        isCurrentMonth: false,
      ));
    }
    
    return calendarDays;
  }
}

class CalendarDay {
  final DateTime date;
  final LunarDate? lunarDate;
  final List<HolidayOccurrence> holidays;
  final bool isCurrentMonth;
  final bool isToday;
  
  const CalendarDay({
    required this.date,
    this.lunarDate,
    this.holidays = const [],
    required this.isCurrentMonth,
    this.isToday = false,
  });
  
  bool get hasHolidays => holidays.isNotEmpty;
  
  Color? get holidayColor => hasHolidays ? holidays.first.color : null;
}
```

---

### 5.3 Setting up Provider in main.dart

**File:** `lib/main.dart`

```dart
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:lunar_calendar/core/services/lunar_calculation_service.dart';
import 'package:lunar_calendar/core/services/holiday_calculation_service.dart';
import 'package:lunar_calendar/features/calendar/data/repositories/calendar_repository.dart';
import 'package:lunar_calendar/features/holidays/data/repositories/holiday_repository.dart';
import 'package:lunar_calendar/features/calendar/presentation/providers/calendar_provider.dart';
import 'package:lunar_calendar/features/holidays/presentation/providers/holiday_provider.dart';
import 'package:lunar_calendar/features/settings/presentation/providers/localization_provider.dart';
import 'package:lunar_calendar/app.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  
  // Initialize services
  final lunarCalculationService = LunarCalculationService();
  final holidayCalculationService = HolidayCalculationService(
    lunarCalculationService: lunarCalculationService,
  );
  
  // Initialize repositories
  final calendarRepository = CalendarRepository(
    lunarCalculationService: lunarCalculationService,
  );
  final holidayRepository = HolidayRepository(
    holidayCalculationService: holidayCalculationService,
  );
  
  runApp(
    MultiProvider(
      providers: [
        // Services
        Provider.value(value: lunarCalculationService),
        Provider.value(value: holidayCalculationService),
        Provider.value(value: calendarRepository),
        Provider.value(value: holidayRepository),
        
        // Providers
        ChangeNotifierProvider(
          create: (_) => LocalizationProvider(),
        ),
        ChangeNotifierProvider(
          create: (context) => CalendarProvider(
            calendarRepository: context.read<CalendarRepository>(),
            holidayRepository: context.read<HolidayRepository>(),
          ),
        ),
        ChangeNotifierProvider(
          create: (context) => HolidayProvider(
            holidayRepository: context.read<HolidayRepository>(),
          ),
        ),
      ],
      child: const LunarCalendarApp(),
    ),
  );
}
```

---

## 6. UI Implementation

### 6.1 Calendar Screen

**File:** `lib/features/calendar/presentation/calendar_screen.dart`

```dart
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:lunar_calendar/features/calendar/presentation/providers/calendar_provider.dart';
import 'package:lunar_calendar/features/calendar/presentation/widgets/calendar_grid.dart';
import 'package:lunar_calendar/features/calendar/presentation/widgets/month_header.dart';
import 'package:lunar_calendar/features/calendar/presentation/widgets/today_section.dart';

class CalendarScreen extends StatelessWidget {
  const CalendarScreen({Key? key}) : super(key: key);
  
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(context.l10n.calendarTitle),
        centerTitle: true,
      ),
      body: Consumer<CalendarProvider>(
        builder: (context, provider, child) {
          if (provider.isLoading) {
            return const Center(child: CircularProgressIndicator());
          }
          
          if (provider.error != null) {
            return Center(
              child: Text('Error: ${provider.error}'),
            );
          }
          
          return SingleChildScrollView(
            padding: const EdgeInsets.all(16),
            child: Column(
              children: [
                // Today's date section
                TodaySection(
                  todayDate: DateTime.now(),
                  todayLunarDate: provider.todayLunarDate,
                ),
                
                const SizedBox(height: 16),
                
                // Month navigation
                MonthHeader(
                  currentMonth: provider.currentMonth,
                  monthYearDisplay: provider.monthYearDisplay,
                  onPreviousMonth: provider.previousMonth,
                  onNextMonth: provider.nextMonth,
                  onGoToToday: provider.goToToday,
                ),
                
                const SizedBox(height: 16),
                
                // Calendar grid
                CalendarGrid(
                  calendarDays: provider.calendarDays,
                  onDayTapped: (day) {
                    // Navigate to holiday detail if has holidays
                    if (day.hasHolidays) {
                      Navigator.pushNamed(
                        context,
                        '/holiday-detail',
                        arguments: day.holidays.first,
                      );
                    }
                  },
                ),
              ],
            ),
          );
        },
      ),
    );
  }
}
```

---

### 6.2 Calendar Grid Widget

**File:** `lib/features/calendar/presentation/widgets/calendar_grid.dart`

```dart
import 'package:flutter/material.dart';
import 'package:lunar_calendar/features/calendar/presentation/providers/calendar_provider.dart';
import 'package:lunar_calendar/features/calendar/presentation/widgets/calendar_day_cell.dart';

class CalendarGrid extends StatelessWidget {
  final List<CalendarDay> calendarDays;
  final Function(CalendarDay) onDayTapped;
  
  const CalendarGrid({
    Key? key,
    required this.calendarDays,
    required this.onDayTapped,
  }) : super(key: key);
  
  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        // Day of week headers
        Row(
          children: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat']
              .map((day) => Expanded(
                    child: Center(
                      child: Text(
                        day,
                        style: const TextStyle(
                          fontWeight: FontWeight.bold,
                          fontSize: 12,
                        ),
                      ),
                    ),
                  ))
              .toList(),
        ),
        
        const SizedBox(height: 8),
        
        // Calendar days (6 weeks × 7 days = 42 cells)
        GridView.builder(
          shrinkWrap: true,
          physics: const NeverScrollableScrollPhysics(),
          gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
            crossAxisCount: 7,
            crossAxisSpacing: 4,
            mainAxisSpacing: 4,
            childAspectRatio: 0.8,
          ),
          itemCount: calendarDays.length,
          itemBuilder: (context, index) {
            final day = calendarDays[index];
            return CalendarDayCell(
              day: day,
              onTap: () => onDayTapped(day),
            );
          },
        ),
      ],
    );
  }
}
```

---

### 6.3 Calendar Day Cell Widget

**File:** `lib/features/calendar/presentation/widgets/calendar_day_cell.dart`

```dart
import 'package:flutter/material.dart';
import 'package:lunar_calendar/features/calendar/presentation/providers/calendar_provider.dart';
import 'package:lunar_calendar/shared/helpers/date_formatter.dart';

class CalendarDayCell extends StatelessWidget {
  final CalendarDay day;
  final VoidCallback onTap;
  
  const CalendarDayCell({
    Key? key,
    required this.day,
    required this.onTap,
  }) : super(key: key);
  
  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    
    return InkWell(
      onTap: onTap,
      borderRadius: BorderRadius.circular(8),
      child: Container(
        decoration: BoxDecoration(
          color: day.isToday 
              ? theme.primaryColor.withOpacity(0.1)
              : Colors.transparent,
          borderRadius: BorderRadius.circular(8),
          border: Border.all(
            color: day.isToday
                ? theme.primaryColor
                : Colors.grey.shade300,
            width: day.isToday ? 2 : 1,
          ),
        ),
        padding: const EdgeInsets.all(4),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            // Gregorian date
            Text(
              day.date.day.toString(),
              style: TextStyle(
                fontSize: 16,
                fontWeight: day.isCurrentMonth
                    ? FontWeight.bold
                    : FontWeight.normal,
                color: day.isCurrentMonth
                    ? Colors.black
                    : Colors.grey,
              ),
            ),
            
            const SizedBox(height: 2),
            
            // Lunar date
            if (day.lunarDate != null)
              Text(
                DateFormatter.formatLunarDateCompact(
                  day.lunarDate!.lunarDay,
                  day.lunarDate!.lunarMonth,
                ),
                style: TextStyle(
                  fontSize: 10,
                  color: Colors.grey.shade600,
                ),
                maxLines: 1,
                overflow: TextOverflow.ellipsis,
              ),
            
            const SizedBox(height: 2),
            
            // Holiday indicator
            if (day.hasHolidays)
              Container(
                height: 4,
                width: 20,
                decoration: BoxDecoration(
                  color: day.holidayColor,
                  borderRadius: BorderRadius.circular(2),
                ),
              ),
          ],
        ),
      ),
    );
  }
}
```

---

## 7. Localization

### 7.1 Setup intl Package

**File:** `pubspec.yaml`

```yaml
dependencies:
  flutter:
    sdk: flutter
  flutter_localizations:
    sdk: flutter
  intl: ^0.18.1

flutter:
  generate: true
```

**File:** `l10n.yaml`

```yaml
arb-dir: lib/l10n
template-arb-file: app_en.arb
output-localization-file: app_localizations.dart
```

---

### 7.2 Localization Files

**File:** `lib/l10n/app_en.arb`

```json
{
  "@@locale": "en",
  "appTitle": "Vietnamese Lunar Calendar",
  "calendarTitle": "Calendar",
  "today": "Today",
  "holidays": "Holidays",
  "settings": "Settings",
  "language": "Language",
  "vietnamese": "Vietnamese",
  "english": "English"
}
```

**File:** `lib/l10n/app_vi.arb`

```json
{
  "@@locale": "vi",
  "appTitle": "Lịch Âm Việt Nam",
  "calendarTitle": "Lịch",
  "today": "Hôm Nay",
  "holidays": "Ngày Lễ",
  "settings": "Cài Đặt",
  "language": "Ngôn Ngữ",
  "vietnamese": "Tiếng Việt",
  "english": "Tiếng Anh"
}
```

---

### 7.3 Using Localization

**In app.dart:**

```dart
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

class LunarCalendarApp extends StatelessWidget {
  const LunarCalendarApp({Key? key}) : super(key: key);
  
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Lunar Calendar',
      localizationsDelegates: const [
        AppLocalizations.delegate,
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
      ],
      supportedLocales: const [
        Locale('en', ''),
        Locale('vi', ''),
      ],
      locale: context.watch<LocalizationProvider>().currentLocale,
      theme: AppTheme.lightTheme,
      home: const CalendarScreen(),
    );
  }
}
```

**In widgets:**

```dart
// Access localized strings
Text(AppLocalizations.of(context)!.calendarTitle)

// Or with extension
Text(context.l10n.calendarTitle)
```

---

## 8. Data Persistence

### 8.1 SQLite with sqflite

**File:** `lib/core/data/database_helper.dart`

```dart
import 'package:sqflite/sqflite.dart';
import 'package:path/path.dart';
import 'package:lunar_calendar/core/models/lunar_date.dart';

class DatabaseHelper {
  static final DatabaseHelper instance = DatabaseHelper._internal();
  static Database? _database;
  
  DatabaseHelper._internal();
  
  Future<Database> get database async {
    if (_database != null) return _database!;
    _database = await _initDatabase();
    return _database!;
  }
  
  Future<Database> _initDatabase() async {
    final dbPath = await getDatabasesPath();
    final path = join(dbPath, 'lunarcalendar.db');
    
    return await openDatabase(
      path,
      version: 1,
      onCreate: _createDb,
    );
  }
  
  Future<void> _createDb(Database db, int version) async {
    await db.execute('''
      CREATE TABLE lunar_dates (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        gregorian_date TEXT UNIQUE NOT NULL,
        lunar_year INTEGER NOT NULL,
        lunar_month INTEGER NOT NULL,
        lunar_day INTEGER NOT NULL,
        is_leap_month INTEGER NOT NULL,
        lunar_year_name TEXT NOT NULL,
        lunar_month_name TEXT NOT NULL,
        lunar_day_name TEXT NOT NULL,
        animal_sign TEXT NOT NULL,
        created_at TEXT DEFAULT CURRENT_TIMESTAMP
      )
    ''');
    
    await db.execute('''
      CREATE INDEX idx_gregorian_date ON lunar_dates(gregorian_date)
    ''');
  }
  
  Future<LunarDate?> getLunarDate(DateTime date) async {
    final db = await database;
    final dateStr = date.toIso8601String().split('T').first;
    
    final results = await db.query(
      'lunar_dates',
      where: 'gregorian_date = ?',
      whereArgs: [dateStr],
      limit: 1,
    );
    
    if (results.isEmpty) return null;
    
    return LunarDate.fromMap(results.first);
  }
  
  Future<void> saveLunarDate(LunarDate lunarDate) async {
    final db = await database;
    await db.insert(
      'lunar_dates',
      lunarDate.toMap(),
      conflictAlgorithm: ConflictAlgorithm.replace,
    );
  }
  
  Future<void> clearCache() async {
    final db = await database;
    await db.delete('lunar_dates');
  }
}
```

---

### 8.2 SharedPreferences for Settings

```dart
import 'package:shared_preferences/shared_preferences.dart';

class PreferencesHelper {
  static const String _keyLanguage = 'app_language';
  static const String _keyTheme = 'app_theme';
  
  static Future<String?> getLanguage() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getString(_keyLanguage);
  }
  
  static Future<void> setLanguage(String language) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString(_keyLanguage, language);
  }
  
  static Future<String?> getTheme() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getString(_keyTheme);
  }
  
  static Future<void> setTheme(String theme) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString(_keyTheme, theme);
  }
}
```

---

## 9. Platform-Specific Code

### 9.1 Haptic Feedback

```dart
import 'package:flutter/services.dart';

class HapticService {
  static void light() {
    HapticFeedback.lightImpact();
  }
  
  static void medium() {
    HapticFeedback.mediumImpact();
  }
  
  static void heavy() {
    HapticFeedback.heavyImpact();
  }
  
  static void selection() {
    HapticFeedback.selectionClick();
  }
}
```

---

## 10. Package Recommendations

### 10.1 Essential Packages

**File:** `pubspec.yaml`

```yaml
dependencies:
  flutter:
    sdk: flutter
  flutter_localizations:
    sdk: flutter
  
  # State Management
  provider: ^6.1.0
  
  # Database
  sqflite: ^2.3.0
  path_provider: ^2.1.1
  path: ^1.8.3
  
  # Persistent Storage
  shared_preferences: ^2.2.2
  
  # Localization
  intl: ^0.18.1
  
  # UI
  flutter_svg: ^2.0.9
  cached_network_image: ^3.3.0
  
  # Utilities
  collection: ^1.18.0
  
dev_dependencies:
  flutter_test:
    sdk: flutter
  flutter_lints: ^3.0.0
  mockito: ^5.4.4
  build_runner: ^2.4.7
```

---

## 11. Migration Strategy

### 11.1 Phase 1: Core Logic (Week 1)

**Tasks:**
1. Set up Flutter project structure
2. Port lunar calculation algorithms
3. Port holiday calculation logic
4. Create data models
5. Write unit tests for core logic

**Deliverables:**
- Working lunar date conversion
- Holiday calculation service
- All unit tests passing

---

### 11.2 Phase 2: Data Layer (Week 2)

**Tasks:**
1. Implement SQLite database
2. Create repositories
3. Implement caching logic
4. Set up SharedPreferences

**Deliverables:**
- Working database layer
- Caching functional
- Preferences storage working

---

### 11.3 Phase 3: State Management (Week 3)

**Tasks:**
1. Set up Provider
2. Create all Providers
3. Implement business logic
4. Connect services to providers

**Deliverables:**
- All providers implemented
- State management working
- Data flows correctly

---

### 11.4 Phase 4: UI Implementation (Week 4-5)

**Tasks:**
1. Create all screens
2. Build reusable widgets
3. Implement navigation
4. Apply theming
5. Add animations

**Deliverables:**
- Complete UI
- All screens functional
- Smooth animations

---

### 11.5 Phase 5: Localization & Polish (Week 6)

**Tasks:**
1. Set up intl package
2. Create localization files
3. Translate all strings
4. Test language switching
5. UI/UX polish

**Deliverables:**
- Full Vietnamese/English support
- Polished UI
- All features working

---

### 11.6 Phase 6: Testing & Deployment (Week 7)

**Tasks:**
1. Integration testing
2. Physical device testing
3. Performance optimization
4. Bug fixes
5. App store submission

**Deliverables:**
- Production-ready app
- Submitted to stores

---

## Conclusion

This guide provides a complete roadmap for migrating the Vietnamese Lunar Calendar app from .NET MAUI to Flutter. The core algorithms, data models, and business logic remain largely the same, with the main differences being in UI implementation and state management patterns.

**Key Takeaways:**
- Flutter offers excellent performance and development experience
- Provider is the recommended state management solution
- Most logic can be directly ported from C# to Dart
- UI needs complete rewrite, but widgets are more flexible
- Localization is simpler with ARB files
- Migration is feasible in 6-7 weeks with focused effort

**Next Steps:**
1. Review this guide thoroughly
2. Set up a new Flutter project
3. Start with Phase 1 (Core Logic)
4. Follow the migration plan sequentially
5. Test frequently on both iOS and Android

---

**Document End**
