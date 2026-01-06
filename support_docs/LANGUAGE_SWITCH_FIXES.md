# Language Switch Fixes - December 29, 2025

## Issues Identified

### Issue 1: Settings Header Not Changing to Vietnamese Immediately
**Problem**: When switching from English to Vietnamese, the Settings page header stayed in English and didn't update to "Cài Đặt" immediately.

**Root Cause**: The `SettingsViewModel` was not subscribing to the `LanguageChangedMessage` to update its `Title` property when the language changed.

### Issue 2: Today Section Shows "Year of the Snake" Instead of "Năm Tỵ"
**Problem**: When switching to Vietnamese, the Today section sometimes showed "Year of the Snake" instead of the properly localized "Năm Tỵ" (or appropriate animal year).

**Root Cause**: 
1. The language change handler in `CalendarViewModel` was using fire-and-forget pattern (`_ = LoadCalendarAsync()`) which could cause timing issues
2. The upcoming holidays were not being refreshed when language changed

## Fixes Implemented

### Fix 1: SettingsViewModel Language Subscription
**File**: `src/LunarCalendar.MobileApp/ViewModels/SettingsViewModel.cs`

Added a subscription to `LanguageChangedMessage` in the constructor:

```csharp
// Subscribe to language changes
WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
{
    Title = AppResources.Settings; // Update title with new language
});
```

**Result**: The Settings page title now updates immediately when switching languages.

### Fix 2: CalendarViewModel Language Update Improvements
**File**: `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`

Made three improvements:

1. **Changed to async handler**: Changed the language change handler from fire-and-forget to proper async/await:
   ```csharp
   WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, async (r, m) =>
   {
       // ... existing code ...
       await LoadCalendarAsync(); // Now properly awaited
       await LoadUpcomingHolidaysAsync(); // Added to refresh upcoming holidays
   });
   ```

2. **Added upcoming holidays refresh**: Now refreshes upcoming holidays when language changes to ensure all localized strings are updated.

3. **Added debug logging**: Added diagnostic logging to help track language change behavior:
   ```csharp
   System.Diagnostics.Debug.WriteLine($"=== Today Display Updated: Culture={CultureInfo.CurrentUICulture.Name}, YearOfThe={yearOfTheText}, Animal={localizedAnimalSign} ===");
   ```

**Result**: 
- The Today section now consistently updates to show the correct localized text
- "Year of the Snake" becomes "Năm Tỵ" when switching to Vietnamese
- "Năm Tỵ" becomes "Year of the Snake" when switching to English
- All upcoming holiday strings are properly refreshed

## How It Works

### Language Change Flow

1. User selects a different language in Settings page
2. `LocalizationService.SetLanguage()` is called
3. Culture is updated (`CultureInfo.CurrentUICulture`)
4. `LanguageChangedMessage` is broadcast via `WeakReferenceMessenger`
5. All subscribed ViewModels receive the message:
   - **SettingsViewModel**: Updates `Title` property
   - **CalendarViewModel**: 
     - Updates `Title` property
     - Reloads month names
     - Refreshes holiday properties
     - Reloads calendar (which updates Today section)
     - Reloads upcoming holidays
6. UI bindings automatically update to show new values

### Resource String Lookup

The `AppResources` class uses .NET's `ResourceManager` which automatically:
1. Checks `CultureInfo.CurrentUICulture`
2. Looks up the appropriate .resx file (e.g., `AppResources.vi.resx` for Vietnamese)
3. Returns the localized string

### Key Localized Strings

- **English**: "Year of the Snake" → **Vietnamese**: "Năm Tỵ"
- **English**: "Settings" → **Vietnamese**: "Cài Đặt"
- **English**: "Calendar" → **Vietnamese**: "Lịch"

## Testing Recommendations

1. **Test Settings Header**:
   - Start app in English
   - Navigate to Settings
   - Verify header shows "Settings"
   - Switch to Vietnamese
   - Verify header immediately changes to "Cài Đặt"
   - Switch back to English
   - Verify header changes back to "Settings"

2. **Test Today Section**:
   - Start app in English
   - Note the animal year displayed (e.g., "Year of the Snake")
   - Switch to Vietnamese
   - Verify it changes to Vietnamese (e.g., "Năm Tỵ")
   - Switch back to English
   - Verify it changes back to English

3. **Test Upcoming Holidays**:
   - Verify all holiday names update when switching languages
   - Check that lunar date displays update correctly

## Debug Output

When language changes, you should see console output like:
```
=== Language changed to: vi-VN ===
=== Today Display Updated: Culture=vi-VN, YearOfThe=Năm, Animal=Tỵ ===
```

## Files Modified

1. `src/LunarCalendar.MobileApp/ViewModels/SettingsViewModel.cs`
2. `src/LunarCalendar.MobileApp/ViewModels/CalendarViewModel.cs`

## Related Resources

- Vietnamese translations: `src/LunarCalendar.MobileApp/Resources/Strings/AppResources.vi.resx`
- English translations: `src/LunarCalendar.MobileApp/Resources/Strings/AppResources.resx`
- Localization service: `src/LunarCalendar.MobileApp/Services/LocalizationService.cs`
- Localization helper: `src/LunarCalendar.MobileApp/Services/LocalizationHelper.cs`

## Notes

- The fixes ensure proper async/await patterns to avoid race conditions
- All ViewModels that display localized text should subscribe to `LanguageChangedMessage`
- The `WeakReferenceMessenger` pattern prevents memory leaks
- Debug logging can be removed in production if desired
