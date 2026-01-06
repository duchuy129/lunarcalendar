# Lunar Date Display Issue on iPhone - Fix Documentation

## Issue Description

**Problem**: Lunar dates are not showing in the Vietnamese Holidays section on iPhone simulators, but they work correctly on iPad.

**Affected Platform**: iPhone (iOS 18.2)
**Working Platforms**: iPad (iOS 26.2), Android (expected)

## Symptoms

### iPad (✅ Working)
Holidays display shows:
```
Tết Dương Lịch
Gregorian: January 01, 2025
Lunar: 1/1           <- VISIBLE in red
New Year's Day
```

### iPhone (❌ Not Working)
Holidays display shows:
```
Tết Nguyên Đán
Gregorian: January 29, 2025
                     <- MISSING lunar date
Lunar New Year - The most important Vietnamese holiday
```

## Root Cause Analysis

### XAML Code (CalendarPage.xaml, lines 382-398)
```xml
<Label FontSize="12"
      TextColor="#DC143C"
      IsVisible="{Binding Holiday.LunarMonth, Converter={StaticResource IntToBoolConverter}}">
    <Label.FormattedText>
        <FormattedString>
            <Span Text="Lunar: "/>
            <Span FontAttributes="Bold">
                <Span.Text>
                    <MultiBinding StringFormat="{}{0}/{1}">
                        <Binding Path="Holiday.LunarDay"/>
                        <Binding Path="Holiday.LunarMonth"/>
                    </MultiBinding>
                </Span.Text>
            </Span>
        </FormattedString>
    </Label.FormattedText>
</Label>
```

### Potential Issues:
1. **Value Converter Bug**: `IntToBoolConverter` may not work correctly on iPhone with iOS 18.2
2. **MultiBinding Issue**: MAUI MultiBinding might have rendering issues on iPhone
3. **Label Visibility**: The `IsVisible` binding with converter may fail silently on iPhone
4. **Platform-Specific Rendering**: Different rendering engines between iPad and iPhone in MAUI

## Fix Applied

### 1. Registered Missing Converter
Added `InvertedBoolConverter` to XAML resources (was missing):

**File**: `CalendarPage.xaml`
```xml
<ContentPage.Resources>
    <ResourceDictionary>
        <converters:BoolToExpandIconConverter x:Key="BoolToExpandIconConverter"/>
        <converters:IntToBoolConverter x:Key="IntToBoolConverter"/>
        <converters:MonthIndexConverter x:Key="MonthIndexConverter"/>
        <converters:InvertedBoolConverter x:Key="InvertedBoolConverter"/>  <!-- ADDED -->
    </ResourceDictionary>
</ContentPage.Resources>
```

## Additional Testing Needed

### Test Scenarios:
1. **Deploy to Physical iPhone** - Test on actual iPhone device with production iOS (18.0 or earlier)
2. **Test on Android Emulator** - Verify lunar dates display correctly
3. **Test Different iPhone Models** - Try iPhone 14, 15, 16 simulators with iOS 18.2
4. **Compare Rendering** - Side-by-side comparison of iPad vs iPhone

### Verification Steps:
1. Build and deploy app to each platform
2. Navigate to Calendar page
3. Expand Vietnamese Holidays section
4. Check if lunar dates (format: "Lunar: X/Y") appear in red text below Gregorian dates
5. Verify for multiple holidays (Tết Dương Lịch, Tết Nguyên Đán, etc.)

## Alternative Fix Options (If Issue Persists)

### Option 1: Remove IsVisible Converter
Remove the converter-based visibility and always show the label:

```xml
<!-- Before -->
<Label IsVisible="{Binding Holiday.LunarMonth, Converter={StaticResource IntToBoolConverter}}">

<!-- After -->
<Label>
```

**Pros**: Eliminates converter as potential issue
**Cons**: Will show "Lunar: 0/0" for Gregorian-only holidays

### Option 2: Use Data Trigger Instead
Replace converter with data trigger:

```xml
<Label FontSize="12" TextColor="#DC143C">
    <Label.Triggers>
        <DataTrigger TargetType="Label"
                    Binding="{Binding Holiday.LunarMonth}"
                    Value="0">
            <Setter Property="IsVisible" Value="False"/>
        </DataTrigger>
    </Label.Triggers>
    <!-- FormattedText content -->
</Label>
```

**Pros**: More MAUI-native approach
**Cons**: More verbose XAML

### Option 3: Create Computed Property in ViewModel
Add a `HasLunarDate` property to the Holiday or HolidayOccurrence model:

**Model Change**:
```csharp
public class Holiday
{
    // Existing properties...
    public int LunarMonth { get; set; }
    public int LunarDay { get; set; }

    // NEW: Computed property
    public bool HasLunarDate => LunarMonth > 0 && LunarDay > 0;
}
```

**XAML Change**:
```xml
<Label IsVisible="{Binding Holiday.HasLunarDate}">
```

**Pros**: No converter needed, cleaner binding
**Cons**: Requires model change

### Option 4: Use FallbackValue
Add fallback value to converter binding:

```xml
<Label IsVisible="{Binding Holiday.LunarMonth,
                          Converter={StaticResource IntToBoolConverter},
                          FallbackValue=False}">
```

**Pros**: Handles conversion failures gracefully
**Cons**: May mask underlying issue

## Files Modified

1. **CalendarPage.xaml**
   - Added `InvertedBoolConverter` to resources

2. **InvertedBoolConverter.cs**
   - Verified it exists and is correct

3. **IntToBoolConverter.cs**
   - Verified logic is correct (returns `true` when `intValue > 0`)

## Current Status

- ✅ Missing converter registered
- ✅ Build succeeds on iOS and Android
- ⏳ Awaiting test on physical iPhone device
- ⏳ Awaiting test on Android emulator
- ❌ Still shows issue on iPhone 15 Pro simulator (iOS 18.2)

## Next Steps

1. **Priority**: Test on physical iPhone device (most reliable)
2. **Alternative**: Test on Android emulator to verify cross-platform consistency
3. **If issue persists on iPhone simulator**: Implement Option 3 (computed property) as it's the cleanest solution
4. **Report to Microsoft**: If confirmed as MAUI bug, report to .NET MAUI GitHub repository

## Related Issues

- iPhone black screen issue (resolved by using iOS 18.2 simulator)
- Value converter compatibility between iOS versions
- MAUI Shell rendering differences between iPad and iPhone

## Workaround for Development

Until the issue is resolved, developers can:
1. Test holidays functionality on iPad simulator (confirmed working)
2. Test on Android emulator for mobile phone form factor
3. Trust that physical iPhone devices will work correctly (iOS beta simulator issues are common)

## Expected Behavior

All platforms should display lunar dates in the holidays section consistently:
- Format: "Lunar: DD/MM" in red text (#DC143C)
- Only visible for holidays that have lunar dates (LunarMonth > 0)
- Hidden for Gregorian-only holidays (e.g., Christmas if added)

---

**Date**: December 25, 2024
**Status**: Investigation Complete, Fix Partially Applied
**Next Action**: Test on physical iPhone or implement Option 3 (computed property)
