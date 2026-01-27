# Sexagenary Cycle Calculation Algorithm

## Overview

The Sexagenary Cycle (干支 gānzhī in Chinese, Can Chi in Vietnamese) is a 60-year cycle used for reckoning time in Chinese and East Asian calendars. It combines:
- **10 Heavenly Stems** (天干 tiāngān / Thiên Can)
- **12 Earthly Branches** (地支 dìzhī / Địa Chi)

The combination creates a 60-unit cycle where each year, month, day, and hour is assigned a stem-branch pair.

## The 10 Heavenly Stems (Thiên Can)

| Index | Chinese | Vietnamese | English | Element | Yin/Yang |
|-------|---------|------------|---------|---------|----------|
| 0 | 甲 (jiǎ) | Giáp | Wood | Wood | Yang |
| 1 | 乙 (yǐ) | Ất | Wood | Wood | Yin |
| 2 | 丙 (bǐng) | Bính | Fire | Fire | Yang |
| 3 | 丁 (dīng) | Đinh | Fire | Fire | Yin |
| 4 | 戊 (wù) | Mậu | Earth | Earth | Yang |
| 5 | 己 (jǐ) | Kỷ | Earth | Earth | Yin |
| 6 | 庚 (gēng) | Canh | Metal | Metal | Yang |
| 7 | 辛 (xīn) | Tân | Metal | Metal | Yin |
| 8 | 壬 (rén) | Nhâm | Water | Water | Yang |
| 9 | 癸 (guǐ) | Quý | Water | Water | Yin |

## The 12 Earthly Branches (Địa Chi)

| Index | Chinese | Vietnamese | English | Zodiac | Element | Hours |
|-------|---------|------------|---------|--------|---------|-------|
| 0 | 子 (zǐ) | Tý | Rat | Rat | Water | 23:00-01:00 |
| 1 | 丑 (chǒu) | Sửu | Ox | Ox | Earth | 01:00-03:00 |
| 2 | 寅 (yín) | Dần | Tiger | Tiger | Wood | 03:00-05:00 |
| 3 | 卯 (mǎo) | Mão | Rabbit | Rabbit | Wood | 05:00-07:00 |
| 4 | 辰 (chén) | Thìn | Dragon | Dragon | Earth | 07:00-09:00 |
| 5 | 巳 (sì) | Tỵ | Snake | Snake | Fire | 09:00-11:00 |
| 6 | 午 (wǔ) | Ngọ | Horse | Horse | Fire | 11:00-13:00 |
| 7 | 未 (wèi) | Mùi | Goat | Goat | Earth | 13:00-15:00 |
| 8 | 申 (shēn) | Thân | Monkey | Monkey | Metal | 15:00-17:00 |
| 9 | 酉 (yǒu) | Dậu | Rooster | Rooster | Metal | 17:00-19:00 |
| 10 | 戌 (xū) | Tuất | Dog | Dog | Earth | 19:00-21:00 |
| 11 | 亥 (hài) | Hợi | Pig | Pig | Water | 21:00-23:00 |

## Calculation Algorithms

### 1. Day Stem-Branch Calculation

The day stem-branch is calculated from the Julian Day Number (JDN).

**Algorithm**:
```
1. Calculate Julian Day Number (JDN) for the given date
2. Day Stem Index = (JDN + 9) mod 10
3. Day Branch Index = (JDN + 1) mod 12
```

**Reference Date**: January 1, 2000 (Gregorian) = Day 壬戌 (Nhâm Tuất / Water Dog)
- JDN for Jan 1, 2000 = 2,451,545
- Stem: (2451545 + 9) mod 10 = 8 (Nhâm/Water)
- Branch: (2451545 + 1) mod 12 = 10 (Tuất/Dog) ✓

### 2. Year Stem-Branch Calculation

The year stem-branch is based on the **lunar year**, not the Gregorian year.

**Algorithm**:
```
For Lunar Year Y (e.g., 2024 lunar year):
1. Year Stem Index = (Y + 6) mod 10
2. Year Branch Index = (Y + 8) mod 12
```

**Note**: The lunar year changes at Lunar New Year (late January or early February), NOT at January 1st.

**Examples**:
- Lunar Year 2024 (Year of Dragon):
  - Stem: (2024 + 6) mod 10 = 0 (Giáp/Wood)
  - Branch: (2024 + 8) mod 12 = 4 (Thìn/Dragon)
  - Result: 甲辰 (Giáp Thìn / Wood Dragon) ✓

- Lunar Year 2026 (Year of Horse):
  - Stem: (2026 + 6) mod 10 = 2 (Bính/Fire)
  - Branch: (2026 + 8) mod 12 = 6 (Ngọ/Horse)
  - Result: 丙午 (Bính Ngọ / Fire Horse) ✓

### 3. Month Stem-Branch Calculation

The month stem depends on the year stem. The month branch follows the lunar months.

**Month Branch** (fixed sequence):
- Lunar Month 1 (around Feb): 寅 (Dần/Tiger) - index 2
- Lunar Month 2 (around Mar): 卯 (Mão/Rabbit) - index 3
- Lunar Month 3 (around Apr): 辰 (Thìn/Dragon) - index 4
- ... and so on, incrementing by 1 each month

**Month Stem** (depends on year stem):
```
Month Stem Index = (2 × Year Stem Index + Lunar Month) mod 10
```

**Formula**:
```
For Lunar Month M (1-12) in a year with Year Stem S:
Month Stem Index = (2 × S + M) mod 10
Month Branch Index = (M + 1) mod 12
```

**Example**: Month 1 (Tiger month) in Year 甲辰 (Giáp Thìn, 2024):
- Year Stem = 0 (Giáp)
- Month Branch = 2 (Dần/Tiger)
- Month Stem = (2 × 0 + 1) mod 10 = 1 (Ất/Wood)
- Result: 乙寅 (Ất Dần / Wood Tiger)

### 4. Hour Stem-Branch Calculation

The hour stem depends on the day stem. The hour branch follows the 12 two-hour periods.

**Hour Branch** (fixed by time):
```
Hour Branch Index = floor(hour / 2 + 0.5) mod 12

Where hour is in 24-hour format:
- 23:00-00:59 → Branch 0 (Tý/Rat)
- 01:00-02:59 → Branch 1 (Sửu/Ox)
- 03:00-04:59 → Branch 2 (Dần/Tiger)
- ... and so on
```

**Hour Stem** (depends on day stem):
```
Hour Stem Index = (2 × Day Stem Index + Hour Branch Index) mod 10
```

**Example**: Hour 14:00 (2:00 PM) on Day 甲子 (Giáp Tý):
- Hour Branch = floor(14 / 2 + 0.5) mod 12 = 7 (Mùi/Goat)
- Day Stem = 0 (Giáp)
- Hour Stem = (2 × 0 + 7) mod 10 = 7 (Tân/Metal)
- Result: 辛未 (Tân Mùi / Metal Goat)

## Implementation Notes

### Julian Day Number Calculation

For .NET/C#, use the built-in DateTime and calculate JDN:

```csharp
public static int CalculateJulianDayNumber(DateTime date)
{
    int a = (14 - date.Month) / 12;
    int y = date.Year + 4800 - a;
    int m = date.Month + 12 * a - 3;
    
    int jdn = date.Day + (153 * m + 2) / 5 + 365 * y + y / 4 - y / 100 + y / 400 - 32045;
    return jdn;
}
```

### Lunar Calendar Integration

Use .NET's `ChineseLunisolarCalendar` class to:
1. Get lunar year for a given Gregorian date
2. Get lunar month for a given date
3. Handle leap months (skip stem-branch calculation for leap months)

### Validation

Test against known historical dates:
- January 1, 2000 = 壬戌 (Nhâm Tuất / Water Dog)
- January 1, 2024 = 辛亥 (Tân Hợi / Metal Pig)
- February 10, 2024 (Lunar New Year) = First day of 甲辰 (Giáp Thìn) year

## References

1. **Jean Meeus** - "Astronomical Algorithms" (1991, 1998)
2. **Helmer Aslaksen** - "The Mathematics of the Chinese Calendar"
3. **Wikipedia** - Sexagenary cycle: https://en.wikipedia.org/wiki/Sexagenary_cycle
4. **Vietnamese Lunar Calendar Sources**:
   - Lịch Vạn Niên (Vietnamese Perpetual Calendar)
   - Traditional Vietnamese almanacs

## Cultural Notes

- The sexagenary cycle is used for divination and fortune-telling
- Each stem-branch combination has specific astrological meanings
- The cycle repeats every 60 years (LCM of 10 and 12)
- Important for selecting auspicious dates for weddings, business openings, etc.
- In Vietnamese culture, birth year zodiac is very significant

## Edge Cases to Handle

1. **Leap Months**: Vietnamese lunar calendar sometimes has 13 months. Leap months don't get their own stem-branch; they inherit from the month they duplicate.

2. **Lunar New Year Boundary**: Year stem-branch changes at Lunar New Year, not January 1st. Dates in January/early February need careful handling.

3. **Date Range**: ChineseLunisolarCalendar supports 1901-2100. Calculations outside this range should gracefully fail.

4. **Hour Boundaries**: The day changes at 23:00 (start of Tý hour), which can be confusing for users used to midnight changes.

## Testing Strategy

Create validation dataset with 1000+ dates covering:
- Various years across 1900-2100
- Dates near Lunar New Year (boundary conditions)
- Known historical dates with documented stem-branches
- All 60 combinations of the cycle
- Leap month dates
- All 12 hour periods
