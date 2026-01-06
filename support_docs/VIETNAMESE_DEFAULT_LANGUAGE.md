# Vietnamese as Default Language

**Date:** December 30, 2025

## Overview
Changed the mobile app's default language from English to Vietnamese. The app will now start in Vietnamese for all new users, while still supporting language switching between Vietnamese and English.

## Changes Made

### 1. App.xaml.cs
**File:** `src/LunarCalendar.MobileApp/App.xaml.cs`

- **Before:** Used system language as default when no saved preference exists
- **After:** Defaults to Vietnamese ("vi") when no saved preference exists
- **Impact:** New app installations will start in Vietnamese instead of following system language

Changed the `InitializeLocalization()` method:
```csharp
// Old: Used system language
if (string.IsNullOrEmpty(savedLanguage))
{
    savedLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
}

// New: Defaults to Vietnamese
if (string.IsNullOrEmpty(savedLanguage))
{
    savedLanguage = "vi";
}
```

Also updated the switch statement to make Vietnamese the default case:
```csharp
switch (savedLanguage.ToLower())
{
    case "en":
        culture = new CultureInfo("en-US");
        break;
    case "vi":
    default:
        culture = new CultureInfo("vi-VN");
        break;
}
```

### 2. LocalizationService.cs
**File:** `src/LunarCalendar.MobileApp/Services/LocalizationService.cs`

Made four changes to ensure Vietnamese is the default throughout:

#### a) Constructor
- **Before:** Used system language as fallback
- **After:** Defaults to Vietnamese ("vi")

```csharp
// Old
else
{
    var systemLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
    SetLanguage(systemLanguage);
}

// New
else
{
    SetLanguage("vi");
}
```

#### b) SetLanguage() Method
- **Before:** English was the default case in switch statement
- **After:** Vietnamese is now the default case

```csharp
switch (languageCode.ToLower())
{
    case "en":
        culture = new CultureInfo("en-US");
        break;
    case "vi":
    default:
        culture = new CultureInfo("vi-VN");
        break;
}
```

#### c) Error Fallback
- **Before:** Fell back to English on errors
- **After:** Falls back to Vietnamese on errors

```csharp
// Old
var fallbackCulture = new CultureInfo("en-US");

// New
var fallbackCulture = new CultureInfo("vi-VN");
```

#### d) GetSavedLanguage() Method
- **Before:** Returned "en" as default
- **After:** Returns "vi" as default

```csharp
// Old
return Preferences.Get(LanguagePreferenceKey, "en");

// New
return Preferences.Get(LanguagePreferenceKey, "vi");
```

## Testing Recommendations

1. **New Installation Test:**
   - Install app on a fresh device/simulator
   - Verify app starts in Vietnamese
   - Check all UI elements display Vietnamese text

2. **Language Switching Test:**
   - Switch from Vietnamese to English in Settings
   - Verify language changes properly
   - Restart app and verify English is maintained

3. **Existing Installation Test:**
   - Users with saved language preferences should not be affected
   - Their chosen language should be preserved

4. **Error Handling Test:**
   - Verify fallback behavior works correctly
   - App should default to Vietnamese if any language setting errors occur

## User Impact

### New Users
- ✅ App will start in Vietnamese by default
- ✅ Better experience for Vietnamese-speaking users
- ✅ Can still switch to English via Settings if needed

### Existing Users
- ✅ No impact - their saved language preference is preserved
- ✅ App will continue using their previously selected language

## Build Status
✅ Build successful with no errors
✅ All compilation checks passed

## Files Modified
1. `src/LunarCalendar.MobileApp/App.xaml.cs`
2. `src/LunarCalendar.MobileApp/Services/LocalizationService.cs`

## Next Steps
1. Test on physical devices (iOS and Android)
2. Verify Vietnamese resource strings are complete
3. Consider adding more languages in the future if needed
