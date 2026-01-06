# Quick Test Checklist - iOS Crash Fix

## 🎯 CRITICAL: Test the iOS Crash Fix First!

### ✅ Test 1: Basic Range Change (2 min)
**On iOS Simulator:**
- [ ] Open app → Settings tab
- [ ] Change "Upcoming Holidays Range" from 30 → 90
- [ ] Go back to Calendar tab
- [ ] **Result:** No crash? ✅ PASS | Crashed? ❌ FAIL

### ✅ Test 2: Rapid Changes (3 min)
**On iOS Simulator:**
- [ ] Settings → 60 days → Calendar
- [ ] Settings → 7 days → Calendar
- [ ] Settings → 90 days → Calendar
- [ ] Repeat 5 times rapidly
- [ ] **Result:** No crash? ✅ PASS | Crashed? ❌ FAIL

### ✅ Test 3: Quick Navigation (2 min)
**On iOS Simulator:**
- [ ] Calendar → Tap any holiday → Back
- [ ] Repeat 10 times quickly
- [ ] **Result:** Smooth? ✅ PASS | Crashed? ❌ FAIL

### ✅ Test 4: Stress Test (5 min)
**On iOS Simulator:**
- [ ] Change range multiple times: 30→60→90→7→14→30
- [ ] Navigate Calendar ↔ Settings 10 times
- [ ] **Result:** Stable? ✅ PASS | Issues? ❌ FAIL

---

## 🌐 Vietnamese Default Language

### ✅ Test 5: Fresh Install (1 min)
**On both simulators:**
- [ ] App shows Vietnamese on first launch?
- [ ] **Result:** Vietnamese? ✅ PASS | English? ❌ FAIL

---

## 📱 Where Are The Apps?

### Android Emulator (emulator-5554)
- App is installed and running
- Package: com.huynguyen.lunarcalendar
- Version: 1.0.1 (Build 2)

### iOS Simulator (iPad Pro 13")
- App is installed and running
- Bundle: com.huynguyen.lunarcalendar
- Process: 54561
- Version: 1.0.1 (Build 2)

---

## 🐛 What to Look For

### ✅ Good Signs:
- Smooth transitions
- Holiday list updates instantly
- No freezing or stuttering
- Debug shows: `[iOS Fix] Loaded X holidays`

### ❌ Bad Signs:
- App crashes (most critical!)
- UI freezes
- Blank holiday list
- Error messages in console

---

## 📊 Quick Results

After testing, mark your results:

**iOS Crash Fix:**
- Test 1: ⬜ PASS | ⬜ FAIL
- Test 2: ⬜ PASS | ⬜ FAIL
- Test 3: ⬜ PASS | ⬜ FAIL
- Test 4: ⬜ PASS | ⬜ FAIL

**Vietnamese Default:**
- Test 5: ⬜ PASS | ⬜ FAIL

**Overall Status:** ⬜ ALL PASS - Ready for production | ⬜ ISSUES FOUND

---

## ⏱️ Total Test Time: ~15 minutes

Focus on iOS tests first - that's where the critical fix is!
