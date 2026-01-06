# Physical Device Setup Guide

## Issue Summary

Physical iPhone devices cannot connect to `http://localhost:5090` because localhost refers to the device itself, not your development computer. This causes the app to fail when loading data from the API.

## Solution Implemented

### 1. API Configuration Update

**File**: `src/LunarCalendar.Api/Properties/launchSettings.json`

Changed the API to listen on all network interfaces:
```json
"applicationUrl": "http://0.0.0.0:5090"  // Was: "http://localhost:5090"
```

This allows the API to accept connections from:
- Localhost (for simulators/emulators)
- Your computer's network IP (for physical devices)

### 2. Mobile App Configuration Update

**Files Modified**:
- `src/LunarCalendar.MobileApp/MauiProgram.cs`
- `src/LunarCalendar.MobileApp/Services/HolidayService.cs`

Added intelligent base URL configuration:

```csharp
string baseUrl;
if (DeviceInfo.Platform == DevicePlatform.Android && DeviceInfo.DeviceType == DeviceType.Virtual)
{
    // Android emulator uses special IP to access host machine
    baseUrl = "http://10.0.2.2:5090";
}
else if (DeviceInfo.DeviceType == DeviceType.Physical)
{
    // For physical devices, use your computer's actual IP address
    baseUrl = "http://10.0.0.72:5090"; // Your computer's IP
}
else
{
    // iOS simulator and other virtual devices use localhost
    baseUrl = "http://localhost:5090";
}
```

## Testing on Different Platforms

### ✅ iOS Simulator
- Uses: `http://localhost:5090`
- Status: Working
- No changes needed

### ✅ Android Emulator
- Uses: `http://10.0.2.2:5090`
- Status: Working
- No changes needed

### ✅ Physical iPhone
- Uses: `http://10.0.0.72:5090` (your computer's network IP)
- Status: Fixed
- Requirements:
  - iPhone and computer must be on the same WiFi network
  - API must be running on your computer
  - No firewall blocking port 5090

### ✅ Physical Android Device
- Uses: `http://10.0.0.72:5090` (your computer's network IP)
- Status: Fixed
- Requirements:
  - Android device and computer must be on the same WiFi network
  - API must be running on your computer
  - No firewall blocking port 5090

## How to Deploy to Physical Devices

### For iOS Physical Device:

1. **Connect your iPhone** via USB or WiFi
2. **Ensure same network**: Make sure iPhone is on the same WiFi as your Mac
3. **Build for device**:
   ```bash
   cd src/LunarCalendar.MobileApp
   dotnet build -t:Run -f net8.0-ios -p:RuntimeIdentifier=ios-arm64
   ```
4. **API must be running** with the updated configuration

### For Android Physical Device:

1. **Enable Developer Mode** on Android device
2. **Enable USB Debugging**
3. **Connect via USB** or **WiFi ADB**
4. **Build for device**:
   ```bash
   cd src/LunarCalendar.MobileApp
   dotnet build -t:Run -f net8.0-android
   ```
5. **API must be running** with the updated configuration

## Verifying API Accessibility

Test that your API is accessible from the network:

```bash
# From your Mac (should work)
curl http://localhost:5090/health
curl http://10.0.0.72:5090/health

# From your iPhone/Android (using browser or curl app)
# Navigate to: http://10.0.0.72:5090/health
```

You should see:
```json
{
  "status": "Healthy",
  "timestamp": "2025-12-26T..."
}
```

## Firewall Configuration

If physical devices can't connect, you may need to allow port 5090:

### macOS Firewall
```bash
# Allow incoming connections on port 5090
sudo /usr/libexec/ApplicationFirewall/socketfilterfw --add /usr/local/share/dotnet/dotnet
```

## Network IP Address

Your current computer IP: **10.0.0.72**

If your IP changes (e.g., after connecting to different WiFi):
1. Find new IP: `ifconfig | grep "inet " | grep -v 127.0.0.1`
2. Update in `MauiProgram.cs` and `HolidayService.cs`
3. Rebuild the mobile app

## Troubleshooting

### Issue: Physical device shows no data

**Check**:
1. Is the API running? `curl http://10.0.0.72:5090/health`
2. Is the device on the same network?
3. Is firewall blocking the connection?
4. Is the IP address correct in the code?

### Issue: Simulator works but physical device doesn't

This is the exact issue we fixed! Make sure:
1. API is running with `http://0.0.0.0:5090` (not just localhost)
2. Mobile app is using the correct IP for physical devices
3. Both are on the same network

## Summary

✅ **iOS Simulator**: Works with localhost
✅ **Android Emulator**: Works with 10.0.2.2
✅ **Physical Devices**: Now works with your network IP (10.0.0.72)

All platforms are now properly configured and should work!
