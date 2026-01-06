# Accessibility Enhancements Guide

**Vietnamese Lunar Calendar App - Making the App Accessible to All Users**

---

## Overview

This document outlines accessibility improvements to ensure the app is usable by people with disabilities, including those using screen readers, voice control, and other assistive technologies.

---

## Key Accessibility Properties in .NET MAUI

### 1. SemanticProperties
Used to provide meaningful descriptions for screen readers:

```xml
<Button Text="Next"
        SemanticProperties.Description="Navigate to next month"
        SemanticProperties.Hint="Double tap to go to the next month"/>
```

### 2. AutomationId
For UI testing and automation:

```xml
<Button AutomationId="NextMonthButton"/>
```

### 3. IsInAccessibleTree
Control whether an element is accessible:

```xml
<Image IsInAccessibleTree="false"/> <!-- Decorative images -->
```

---

## Recommended Enhancements by Page

### CalendarPage.xaml

#### Navigation Buttons
```xml
<!-- Previous Month Button -->
<Button Grid.Row="0" Grid.Column="0"
        Text="◀"
        Command="{Binding PreviousMonthCommand}"
        AutomationId="PreviousMonthButton"
        SemanticProperties.Description="Previous month"
        SemanticProperties.Hint="Navigate to previous month"
        BackgroundColor="Transparent"
        TextColor="White"
        FontSize="20"
        WidthRequest="50"/>

<!-- Next Month Button -->
<Button Grid.Row="0" Grid.Column="3"
        Text="▶"
        Command="{Binding NextMonthCommand}"
        AutomationId="NextMonthButton"
        SemanticProperties.Description="Next month"
        SemanticProperties.Hint="Navigate to next month"
        BackgroundColor="Transparent"
        TextColor="White"
        FontSize="20"
        WidthRequest="50"/>

<!-- Today Button -->
<Button Grid.Row="0" Grid.Column="2"
        Text="Today"
        Command="{Binding TodayCommand}"
        AutomationId="TodayButton"
        SemanticProperties.Description="Jump to today"
        SemanticProperties.Hint="Navigate to current date"
        BackgroundColor="Transparent"
        TextColor="White"
        FontSize="14"/>
```

#### Month/Year Display
```xml
<Label Text="{Binding MonthYearDisplay}"
       AutomationId="MonthYearLabel"
       SemanticProperties.Description="{Binding MonthYearDisplay}"
       SemanticProperties.HeadingLevel="Level1"
       FontSize="18"
       FontAttributes="Bold"
       TextColor="White"/>
```

#### Sync Status
```xml
<Label Text="{Binding SyncStatus}"
       AutomationId="SyncStatusLabel"
       SemanticProperties.Description="{Binding SyncStatus}"
       SemanticProperties.Hint="Connection and sync status"
       FontSize="10"
       TextColor="White"/>
```

#### Calendar Days
For each calendar day in the grid:

```xml
<Frame BorderColor="{Binding BorderColor}"
       AutomationId="{Binding AutomationId}"
       SemanticProperties.Description="{Binding AccessibilityDescription}"
       SemanticProperties.Hint="{Binding AccessibilityHint}">
    <!-- Day content -->
</Frame>
```

**ViewModel Support** - Add to CalendarDayViewModel:
```csharp
public string AccessibilityDescription =>
    $"{GregorianDay} {LunarDateDisplay}" +
    (IsToday ? ", Today" : "") +
    (HolidayName != null ? $", Holiday: {HolidayName}" : "");

public string AccessibilityHint =>
    HolidayName != null ? "Double tap to view holiday details" : "Calendar date";

public string AutomationId => $"Day_{GregorianDate:yyyyMMdd}";
```

---

### SettingsPage.xaml

#### Section Headers
```xml
<Label Text="Sync &amp; Offline"
       SemanticProperties.HeadingLevel="Level2"
       FontSize="16"
       FontAttributes="Bold"/>
```

#### Status Labels
```xml
<Label Grid.Column="0" Text="Status"
       SemanticProperties.Description="Connection status"/>

<Label Grid.Column="1" Text="{Binding IsOnline}"
       AutomationId="ConnectionStatusLabel"
       SemanticProperties.Description="{Binding IsOnline, StringFormat='Currently {0}'}">
```

#### Buttons
```xml
<Button Text="Sync Now"
        Command="{Binding SyncDataCommand}"
        IsEnabled="{Binding IsOnline}"
        AutomationId="SyncNowButton"
        SemanticProperties.Description="Manually sync calendar data"
        SemanticProperties.Hint="Synchronize with server"
        BackgroundColor="{DynamicResource Primary}"
        TextColor="White"/>

<Button Text="Clear Cache"
        Command="{Binding ClearCacheCommand}"
        AutomationId="ClearCacheButton"
        SemanticProperties.Description="Clear cached data"
        SemanticProperties.Hint="Remove locally stored calendar data"
        BackgroundColor="#FF6B6B"
        TextColor="White"/>
```

---

### HolidayDetailPage.xaml

```xml
<!-- Holiday Name -->
<Label Text="{Binding Holiday.Name}"
       AutomationId="HolidayNameLabel"
       SemanticProperties.HeadingLevel="Level1"
       SemanticProperties.Description="{Binding Holiday.Name, StringFormat='Holiday: {0}'}"
       FontSize="24"
       FontAttributes="Bold"/>

<!-- Holiday Description -->
<Label Text="{Binding Holiday.Description}"
       AutomationId="HolidayDescriptionLabel"
       SemanticProperties.Description="{Binding Holiday.Description}"
       FontSize="16"/>

<!-- Holiday Type -->
<Label Text="{Binding Holiday.Type}"
       AutomationId="HolidayTypeLabel"
       SemanticProperties.Description="{Binding Holiday.Type, StringFormat='Type: {0}'}"
       FontSize="14"/>

<!-- Close Button -->
<Button Text="Close"
        Command="{Binding CloseCommand}"
        AutomationId="CloseButton"
        SemanticProperties.Description="Close holiday details"
        SemanticProperties.Hint="Return to calendar"/>
```

---

## Color Contrast & Visual Accessibility

### Current Color Scheme Assessment

Based on WCAG 2.1 guidelines (minimum contrast ratio: 4.5:1 for normal text, 3:1 for large text):

#### Check These Combinations:
1. **Primary Text on Background**:
   - Background: `#FFF9F0` (off-white)
   - Text: Needs to be dark enough for contrast

2. **Header Text on Primary Color**:
   - Background: `{DynamicResource Primary}` (verify actual color)
   - Text: White - should have good contrast

3. **Holiday Color Coding**:
   - Red holidays: `#FFB3BA`
   - Orange holidays: `#FFD9A3`
   - Blue holidays: `#B3D9FF`
   - Ensure text on these backgrounds is readable

### Recommendations

```xml
<!-- High Contrast Mode Support -->
<ContentPage.Resources>
    <ResourceDictionary>
        <!-- Define high-contrast alternatives -->
        <Color x:Key="HighContrastText">#000000</Color>
        <Color x:Key="HighContrastBackground">#FFFFFF</Color>
    </ResourceDictionary>
</ContentPage.Resources>
```

---

## Font Scaling Support

Ensure text scales properly with system font size settings:

```xml
<Label Text="{Binding MonthYearDisplay}"
       FontSize="18"
       MaxLines="2"
       LineBreakMode="TailTruncation"/>
```

**Test**: Change device font size to largest and verify UI doesn't break.

---

## Touch Target Sizes

### Minimum Sizes (per platform guidelines):
- **iOS**: 44x44 points
- **Android**: 48x48 dp

### Recommendations

```xml
<!-- Ensure buttons meet minimum size -->
<Button Text="Next"
        WidthRequest="50"
        HeightRequest="50"/> <!-- Meets both iOS and Android standards -->

<!-- For smaller visual buttons, add padding -->
<Button Text="▶"
        WidthRequest="44"
        HeightRequest="44"
        Padding="12"/> <!-- Visual icon smaller, but touch target is adequate -->
```

---

## Screen Reader Announcements

### iOS VoiceOver Focus Order

Elements are read in visual order (top-to-bottom, left-to-right).

**To control order**, use `TabIndex`:

```xml
<Button Text="◀" TabIndex="1"/>
<Label Text="December 2024" TabIndex="2"/>
<Button Text="Today" TabIndex="3"/>
<Button Text="▶" TabIndex="4"/>
```

### Dynamic Announcements

For important state changes, announce to screen reader:

```csharp
// In ViewModel when sync completes
SemanticScreenReader.Announce("Calendar data synchronized successfully");

// When month changes
SemanticScreenReader.Announce($"Showing {MonthYearDisplay}");

// When going offline
SemanticScreenReader.Announce("Offline mode - showing cached data");
```

---

## Implementation Checklist

### Phase 1: Core Navigation (High Priority)
- [ ] Add `SemanticProperties.Description` to all buttons
- [ ] Add `SemanticProperties.Hint` for interactive elements
- [ ] Add `AutomationId` to all testable elements
- [ ] Set `HeadingLevel` for section headers

### Phase 2: Dynamic Content (High Priority)
- [ ] Add accessibility descriptions to calendar days
- [ ] Announce month navigation changes
- [ ] Announce online/offline status changes
- [ ] Announce sync completion

### Phase 3: Visual Enhancements (Medium Priority)
- [ ] Verify color contrast ratios (WCAG AA: 4.5:1 minimum)
- [ ] Test with system font scaling (up to 200%)
- [ ] Verify touch targets meet platform minimums
- [ ] Add high-contrast mode support

### Phase 4: Advanced (Low Priority)
- [ ] Add keyboard navigation support (for tablets/external keyboards)
- [ ] Implement focus indicators for keyboard navigation
- [ ] Add haptic feedback for important actions
- [ ] Support iOS Dynamic Type and Android font scaling

---

## Testing Accessibility

### iOS VoiceOver Testing

1. Enable VoiceOver: Settings → Accessibility → VoiceOver
2. Navigate the app using swipe gestures
3. Verify all elements are announced correctly
4. Check that buttons explain their action
5. Ensure focus order is logical

**Gestures**:
- Swipe right: Next element
- Swipe left: Previous element
- Double tap: Activate element
- Three-finger swipe: Scroll

### Android TalkBack Testing

1. Enable TalkBack: Settings → Accessibility → TalkBack
2. Navigate using swipe gestures (similar to iOS)
3. Verify announcements are clear
4. Test with explore-by-touch mode

### Automated Testing

```csharp
[Test]
public void Calendar_PreviousMonthButton_HasAccessibilityLabel()
{
    var button = App.Query(c => c.Marked("PreviousMonthButton")).FirstOrDefault();
    Assert.IsNotNull(button);
    Assert.IsTrue(button.Description.Contains("Previous month"));
}

[Test]
public void Calendar_TodayButton_HasHint()
{
    var button = App.Query(c => c.Marked("TodayButton")).FirstOrDefault();
    Assert.IsNotNull(button);
    Assert.IsTrue(button.Hint.Contains("Navigate"));
}
```

---

## Common Accessibility Pitfalls to Avoid

### ❌ DON'T:
1. **Use color alone to convey information**
   - Bad: Red text for errors only
   - Good: Red text + error icon + descriptive message

2. **Have unlabeled buttons or icons**
   - Bad: `<Button Text="⚙️"/>`
   - Good: `<Button Text="⚙️" SemanticProperties.Description="Settings"/>`

3. **Use tiny touch targets**
   - Bad: `<Button WidthRequest="20" HeightRequest="20"/>`
   - Good: `<Button WidthRequest="44" HeightRequest="44"/>`

4. **Forget about decorative elements**
   - Bad: All images accessible
   - Good: `<Image IsInAccessibleTree="false"/>` for decorative images

5. **Have low contrast text**
   - Bad: Light gray text on white background
   - Good: Dark text on light background (4.5:1 ratio minimum)

### ✅ DO:
1. Provide text alternatives for all non-text content
2. Use semantic HTML/XAML headings
3. Test with actual assistive technologies
4. Support system font size scaling
5. Provide keyboard navigation (for tablets)
6. Use clear, descriptive labels

---

## WCAG 2.1 Compliance Levels

### Level A (Minimum - Must Have)
- [ ] All images have alt text (or marked decorative)
- [ ] Color is not the only means of conveying information
- [ ] All functionality available via keyboard (tablets)

### Level AA (Recommended - Should Have)
- [ ] Contrast ratio 4.5:1 for normal text, 3:1 for large text
- [ ] Text can be resized up to 200% without loss of functionality
- [ ] No keyboard trap (can navigate away from all elements)
- [ ] Page titles are descriptive

### Level AAA (Enhanced - Nice to Have)
- [ ] Contrast ratio 7:1 for normal text, 4.5:1 for large text
- [ ] No images of text (use actual text)
- [ ] Provide extended audio descriptions (if applicable)

**For MVP, aim for Level AA compliance.**

---

## Quick Implementation Example

Here's a before/after for a calendar day button:

### Before (No Accessibility)
```xml
<Frame BorderColor="Gray">
    <StackLayout>
        <Label Text="15" FontSize="16"/>
        <Label Text="Rằm" FontSize="12"/>
    </StackLayout>
</Frame>
```

### After (With Accessibility)
```xml
<Frame BorderColor="Gray"
       AutomationId="CalendarDay_20241215"
       SemanticProperties.Description="December 15, Lunar day Rằm"
       SemanticProperties.Hint="Double tap to view details">
    <StackLayout>
        <Label Text="15" FontSize="16"
               IsInAccessibleTree="false"/> <!-- Part of parent description -->
        <Label Text="Rằm" FontSize="12"
               IsInAccessibleTree="false"/> <!-- Part of parent description -->
    </StackLayout>
</Frame>
```

**Key Changes**:
1. Added `AutomationId` for testing
2. Added combined `SemanticProperties.Description` at container level
3. Added `SemanticProperties.Hint` to explain interaction
4. Marked child labels as not accessible (to avoid redundant announcements)

---

## Resources

- [.NET MAUI Accessibility Documentation](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/accessibility)
- [WCAG 2.1 Guidelines](https://www.w3.org/WAI/WCAG21/quickref/)
- [iOS VoiceOver Guide](https://support.apple.com/guide/iphone/turn-on-and-practice-voiceover-iph3e2e415f/ios)
- [Android TalkBack Guide](https://support.google.com/accessibility/android/answer/6283677)
- [Color Contrast Checker](https://webaim.org/resources/contrastchecker/)

---

**Next Steps**:
1. Implement Phase 1 (Core Navigation) enhancements
2. Test with VoiceOver and TalkBack
3. Address any issues found
4. Move to Phase 2 (Dynamic Content)

**Estimated Effort**: 4-8 hours for full accessibility implementation across all pages.
