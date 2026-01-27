# iOS SQLite Crash Fix - Apple Review Report

**Date:** January 6, 2026
**Issue:** App crashes on iOS during database operations
**Status:** 🔴 Critical - Identified and fix ready

---

## 🐛 Problem Identified

From Apple's diagnostic log, the app crashes in `SaveLunarDatesAsync` method due to **synchronous SQLite calls inside async transaction context**.

### Error Location

**File:** [src/LunarCalendar.MobileApp/Data/LunarCalendarDatabase.cs](src/LunarCalendar.MobileApp/Data/LunarCalendarDatabase.cs:68-91)

**Problem Code (Lines 76-78):**
```csharp
var existing = tran.Table<LunarDateEntity>()
    .Where(ld => ld.GregorianDate == lunarDate.GregorianDate)
    .FirstOrDefault();  // ❌ Synchronous call!
```

**Same issue in `SaveHolidayOccurrencesAsync` (Lines 130-132):**
```csharp
var existing = tran.Table<HolidayOccurrenceEntity>()
    .Where(ho => ho.GregorianDate == occurrence.GregorianDate && ho.Name == occurrence.Name)
    .FirstOrDefault();  // ❌ Synchronous call!
```

---

## ⚠️ Why This Causes Crashes on iOS

1. **Async/Sync Mismatch:** Using synchronous `.FirstOrDefault()` inside `RunInTransactionAsync` creates threading issues
2. **iOS Strict Threading:** iOS runtime is stricter about thread safety than Android
3. **SQLite Lock:** Can cause database locks and crashes under certain conditions
4. **Apple's Review:** This shows up as a crash during automated testing

---

## ✅ Solution

Replace all synchronous SQLite calls with async versions or use batch operations properly.

### Option 1: Use Async Outside Transaction (Recommended)
```csharp
public async Task SaveLunarDatesAsync(List<LunarDateEntity> lunarDates)
{
    await InitAsync();

    // Prepare data BEFORE transaction
    var updates = new List<LunarDateEntity>();
    var inserts = new List<LunarDateEntity>();

    foreach (var lunarDate in lunarDates)
    {
        lunarDate.LastSyncedAt = DateTime.UtcNow;
        var existing = await _database!.Table<LunarDateEntity>()
            .Where(ld => ld.GregorianDate == lunarDate.GregorianDate)
            .FirstOrDefaultAsync();  // ✅ Async!

        if (existing != null)
        {
            lunarDate.Id = existing.Id;
            updates.Add(lunarDate);
        }
        else
        {
            inserts.Add(lunarDate);
        }
    }

    // Execute batch operations in transaction
    await _database!.RunInTransactionAsync(tran =>
    {
        foreach (var update in updates)
            tran.Update(update);
        foreach (var insert in inserts)
            tran.Insert(insert);
    });
}
```

### Option 2: Use InsertOrReplace (Simpler)
```csharp
public async Task SaveLunarDatesAsync(List<LunarDateEntity> lunarDates)
{
    await InitAsync();
    foreach (var lunarDate in lunarDates)
    {
        lunarDate.LastSyncedAt = DateTime.UtcNow;
        await _database!.InsertOrReplaceAsync(lunarDate);  // ✅ Simple and safe
    }
}
```

---

## 📝 Files to Fix

1. **SaveLunarDatesAsync** - Lines 68-91
2. **SaveHolidayOccurrencesAsync** - Lines 122-145

Both methods need the same fix pattern.

---

## 🧪 Testing Plan

After fix:
1. ✅ Deploy to iPhone 11 (iOS 26.2) simulator
2. ✅ Test calendar view loading
3. ✅ Test settings tab
4. ✅ Navigate through multiple months
5. ✅ Test language switching
6. ✅ Verify no crashes in diagnostic logs

---

## 🚀 Deployment Steps

1. Fix the code
2. Rebuild iOS Release IPA
3. Test on simulator
4. Update version to 1.0.2 (build 3)
5. Resubmit to App Store

---

**Priority:** 🔴 **CRITICAL** - Must fix before app can be approved

