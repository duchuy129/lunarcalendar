# Android Deployment Script Usage

## Updated: `deploy-android-sim.sh`

This script now supports Debug/Release builds, clean builds, and automatic crash detection.

## Usage

### Basic Usage (with emulator already running)

```bash
# Deploy Debug build (default)
./scripts/deploy-android-sim.sh

# Deploy Release build
./scripts/deploy-android-sim.sh --release

# Deploy with clean build
./scripts/deploy-android-sim.sh --clean

# Deploy Debug with clean build
./scripts/deploy-android-sim.sh --debug --clean
```

### Auto-start Emulator

```bash
# Auto-start emulator and deploy
./scripts/deploy-android-sim.sh --auto-start

# Auto-start, clean build, and deploy Release
./scripts/deploy-android-sim.sh --release --clean --auto-start
```

## Features

### 1. **Build Configuration Support**
- `--debug` - Build Debug configuration (default)
- `--release` - Build Release configuration

### 2. **Clean Build Option**
- `--clean` - Removes old build artifacts before building
- Ensures completely fresh build

### 3. **Smart Emulator Detection**
- Detects if emulator is already running
- Uses existing emulator if available
- With `--auto-start`: Starts emulator automatically if not running

### 4. **Fresh Deployment**
- **Always uninstalls old version** before installing new one
- Prevents stale APK caching issues
- Shows APK details (path, size, modification time)

### 5. **Automatic Crash Detection**
- Waits 3 seconds after launch
- Checks logcat for FATAL exceptions
- Shows full crash log if detected
- Verifies app process is running

### 6. **Build Version Verification**
- Shows APK path from correct build folder (Debug/Release)
- Displays file modification time
- Ensures you're deploying the latest build

## Examples

### Deploy latest changes to running emulator
```bash
./scripts/deploy-android-sim.sh
```

### Force rebuild and deploy
```bash
./scripts/deploy-android-sim.sh --clean
```

### Deploy Release build for testing
```bash
./scripts/deploy-android-sim.sh --release --clean
```

### Start emulator and deploy automatically
```bash
./scripts/deploy-android-sim.sh --auto-start --clean
```

## Output Example

```
🤖 Deploying to Android Emulator (Debug build)...
✅ Emulator already running: emulator-5554

🗑️  Uninstalling old version...
Success

🔨 Building Debug APK...
Build succeeded.

📦 APK Details:
   Path: /path/to/bin/Debug/net10.0-android/com.huynguyen.lunarcalendar-Signed.apk
   Size: 45M
   Modified: 2026-01-10 21:30:45

📲 Installing APK...
Success

🚀 Launching app...

✅ App deployed and launched on Android emulator!
📱 Device: emulator-5554

🔍 Checking for crashes...
✅ No crashes detected
✅ App is running: u0_a249 15074 357 15384368 293964
```

## Troubleshooting

### "No emulator running"
Start your emulator manually or use `--auto-start` flag

### "APK not found"
The build might have failed. Check build output above.

### "Installation failed"
- Check if emulator is responsive
- Try: `adb kill-server && adb start-server`

### Old version still showing
This shouldn't happen anymore - script always uninstalls first. If it does:
```bash
adb uninstall com.huynguyen.lunarcalendar
./scripts/deploy-android-sim.sh --clean
```

## Comparison with Old Script

| Feature | Old | New |
|---------|-----|-----|
| Debug/Release support | ❌ | ✅ |
| Clean build option | ❌ | ✅ |
| Uninstall old version | ❌ | ✅ |
| APK version verification | ❌ | ✅ |
| Crash detection | ❌ | ✅ |
| Works with running emulator | ❌ | ✅ |
| Shows APK details | ❌ | ✅ |
