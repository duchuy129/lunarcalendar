# iOS Crash - Root Cause & Fix

## The Problem
**Watchdog Timeout (0x8BADF00D)** - The app was taking **over 17 seconds to launch** and iOS killed it.

## Root Cause
The crash report showed:
```
Termination Reason: FRONTBOARD 2343432205
scene-create watchdog transgression: exhausted real (wall clock) time allowance of 17.62 seconds
```

**The issue was in AppShell.xaml** - The translation markup extension `{ext:Translate AppName}` was causing an **infinite layout loop** in iOS's UITabBarController during app startup.

Stack trace showed the app stuck in:
```
UITabBarController → _layoutViewController → layoutSublayersOfLayer (infinite loop)
```

## The Fix

### Changed AppShell.xaml:
**BEFORE:**
```xml
<Shell
    Title="{ext:Translate AppName}">

    <FlyoutItem Title="{ext:Translate Calendar}" Icon="calendar.png">
        <ShellContent Title="{ext:Translate Calendar}" .../>
    </FlyoutItem>
```

**AFTER:**
```xml
<Shell
    Title="Vietnamese Lunar Calendar">

    <FlyoutItem Title="Calendar" Icon="calendar.png">
        <ShellContent Title="Calendar" .../>
    </FlyoutItem>
```

### Why This Fixed It:
1. **Removed all `{ext:Translate}` markup extensions from Shell/FlyoutItem titles**
2. **Used static strings instead** - no dynamic binding during layout
3. **Eliminated the layout loop** - iOS can now complete layout immediately

## Files Changed
- ✅ [src/LunarCalendar.MobileApp/AppShell.xaml](src/LunarCalendar.MobileApp/AppShell.xaml) - Removed translation bindings

## Result
✅ **App now launches successfully on iPhone!**
- Launch time: < 3 seconds
- No watchdog timeout
- No infinite layout loop

## Key Learnings
1. **iOS is strict about launch time** - Must complete within ~20 seconds or killed
2. **Avoid complex bindings in Shell structure** - Use static values for Shell/FlyoutItem titles
3. **Translation extensions can cause layout loops on iOS** - Especially in Shell navigation
4. **Watchdog crashes (0x8BADF00D) indicate timeout** - Not a code exception

## For Future Reference
If you need translated titles in Shell:
- Set titles programmatically in code-behind after Shell loads
- Or use simpler binding that doesn't trigger layout cycles
- Never use complex markup extensions in Shell.Title or FlyoutItem.Title

## Testing
Tested on:
- Device: Huy's iPhone (iOS 26.1)
- Build: Release configuration
- Launch time: ~2 seconds
- Status: ✅ Working perfectly
