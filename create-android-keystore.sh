#!/bin/bash

echo "🔑 Creating Android Release Keystore"
echo ""
echo "⚠️  CRITICAL: This keystore is required to update your app!"
echo "If you lose it, you'll have to create a NEW app with a new package name!"
echo ""
echo "You'll be asked for:"
echo "1. Keystore password (use a strong password!)"
echo "2. Key password (can be same as keystore password)"
echo "3. Your name, organization, etc."
echo ""
read -p "Press ENTER to continue..."

keytool -genkey -v \
  -keystore ~/.android/keystore/lunarcalendar-release.keystore \
  -alias lunarcalendar \
  -keyalg RSA \
  -keysize 2048 \
  -validity 10000

if [ $? -eq 0 ]; then
  echo ""
  echo "✅ Keystore created successfully!"
  echo ""
  echo "📍 Location: ~/.android/keystore/lunarcalendar-release.keystore"
  echo ""
  echo "⚠️  BACKUP THIS FILE IMMEDIATELY!"
  echo ""
  echo "Creating backup on Desktop..."
  cp ~/.android/keystore/lunarcalendar-release.keystore ~/Desktop/BACKUP_lunarcalendar-release.keystore
  echo "✅ Backup created: ~/Desktop/BACKUP_lunarcalendar-release.keystore"
  echo ""
  echo "📝 Now creating properties file..."
  echo "You'll need to enter your passwords again..."
  echo ""
  
  # Prompt for passwords
  read -sp "Enter keystore password: " STORE_PASS
  echo ""
  read -sp "Enter key password (press ENTER if same as keystore password): " KEY_PASS
  echo ""
  
  if [ -z "$KEY_PASS" ]; then
    KEY_PASS="$STORE_PASS"
  fi
  
  # Create properties file
  cat > ~/.android/keystore/lunarcalendar.properties << PROPS
storeFile=$HOME/.android/keystore/lunarcalendar-release.keystore
storePassword=$STORE_PASS
keyAlias=lunarcalendar
keyPassword=$KEY_PASS
PROPS
  
  chmod 600 ~/.android/keystore/lunarcalendar.properties
  
  echo ""
  echo "✅ Properties file created!"
  echo ""
  echo "🎉 You're ready to build the release AAB!"
  echo ""
  echo "Run: ./build-android-release.sh"
else
  echo "❌ Keystore creation failed!"
  exit 1
fi
