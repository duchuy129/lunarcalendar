#!/bin/bash

echo "🔐 Creating Android Release Keystore"
echo "======================================"
echo ""
echo "⚠️  CRITICAL: This keystore is REQUIRED for all future app updates!"
echo "    If you lose it, you can NEVER update your app on Google Play."
echo "    We will create a backup on your Desktop."
echo ""

# Keystore details
KEYSTORE_DIR="$HOME/.android/keystore"
KEYSTORE_FILE="$KEYSTORE_DIR/lunarcalendar-release.keystore"
BACKUP_DIR="$HOME/Desktop/LunarCalendar-Keystore-Backup-$(date +%Y%m%d)"
PROPERTIES_FILE="$KEYSTORE_DIR/lunarcalendar.properties"

# Create keystore directory
mkdir -p "$KEYSTORE_DIR"

# Check if keystore already exists
if [ -f "$KEYSTORE_FILE" ]; then
    echo "✅ Keystore already exists at: $KEYSTORE_FILE"
    echo ""
    echo "Using existing keystore for build."
    echo ""

    # Show backup reminder
    if [ -f "$PROPERTIES_FILE" ]; then
        echo "📋 Keystore properties found at: $PROPERTIES_FILE"
    else
        echo "⚠️  Warning: Properties file not found. You may need to recreate it."
    fi

    exit 0
fi

echo "📝 Creating new keystore..."
echo ""
echo "You'll be asked for:"
echo "  1. Keystore password (choose a strong password)"
echo "  2. Key password (can be same as keystore password)"
echo "  3. Your name"
echo "  4. Organization (can use your name or skip)"
echo "  5. City, State, Country"
echo ""
echo "Press Enter to continue..."
read

# Generate keystore
keytool -genkey -v \
  -keystore "$KEYSTORE_FILE" \
  -alias lunarcalendar-key \
  -keyalg RSA \
  -keysize 2048 \
  -validity 10000 \
  -storepass "" \
  -keypass ""

# Check if keystore was created
if [ ! -f "$KEYSTORE_FILE" ]; then
    echo "❌ Error: Keystore creation failed!"
    exit 1
fi

echo ""
echo "✅ Keystore created successfully!"
echo ""

# Create backup
echo "📦 Creating backup on Desktop..."
mkdir -p "$BACKUP_DIR"
cp "$KEYSTORE_FILE" "$BACKUP_DIR/"

echo ""
echo "✅ Backup created at: $BACKUP_DIR"
echo ""

# Prompt for passwords to save in properties file
echo "📝 Now, let's save your keystore credentials for the build script."
echo ""
echo "Enter your keystore password:"
read -s STORE_PASSWORD
echo ""
echo "Enter your key password (press Enter if same as keystore password):"
read -s KEY_PASSWORD
if [ -z "$KEY_PASSWORD" ]; then
    KEY_PASSWORD="$STORE_PASSWORD"
fi

# Create properties file for build script
cat > "$PROPERTIES_FILE" << EOF
storePassword=$STORE_PASSWORD
keyPassword=$KEY_PASSWORD
keyAlias=lunarcalendar-key
storeFile=$KEYSTORE_FILE
EOF

echo ""
echo "✅ Credentials saved to: $PROPERTIES_FILE"
echo ""

# Show summary
echo "======================================"
echo "🎉 KEYSTORE SETUP COMPLETE!"
echo "======================================"
echo ""
echo "📂 Keystore location: $KEYSTORE_FILE"
echo "📂 Backup location:   $BACKUP_DIR"
echo "📂 Properties file:   $PROPERTIES_FILE"
echo ""
echo "⚠️  IMPORTANT NEXT STEPS:"
echo ""
echo "1. ✅ Keystore created and backed up to Desktop"
echo "2. 🔒 BACKUP this keystore to a SECURE location:"
echo "   - Cloud storage (Google Drive, Dropbox, etc.)"
echo "   - External hard drive"
echo "   - Password manager (1Password, LastPass, etc.)"
echo ""
echo "3. 📝 SAVE your passwords somewhere safe!"
echo "   (They are in $PROPERTIES_FILE)"
echo ""
echo "4. 🚀 Ready to build! Run: ./build-android-release.sh"
echo ""
echo "⚠️  DO NOT LOSE THIS KEYSTORE!"
echo "    Without it, you cannot update your app on Google Play!"
echo ""
