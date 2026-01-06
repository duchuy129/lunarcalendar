#!/bin/bash

echo "🔍 Verifying Release Builds & Assets..."
echo ""
echo "═══════════════════════════════════════════════════════════"
echo ""

# Check iOS IPA
echo "📱 iOS RELEASE BUILD:"
echo "-----------------------------------------------------------"
IPA_FILE=$(find src/LunarCalendar.MobileApp/bin/Release/net10.0-ios -name "*.ipa" 2>/dev/null | head -1)
if [ -n "$IPA_FILE" ]; then
  ls -lh "$IPA_FILE"
  echo "✅ iOS IPA found"

  # Check file size
  SIZE=$(du -h "$IPA_FILE" | cut -f1)
  echo "   Size: $SIZE"
  echo "   Expected: < 50MB for initial review"
else
  echo "❌ iOS IPA not found"
  echo "   Run: ./build-ios-release.sh"
fi

echo ""

# Check Android AAB
echo "🤖 ANDROID RELEASE BUILD:"
echo "-----------------------------------------------------------"
AAB_FILE=$(find src/LunarCalendar.MobileApp/bin/Release/net10.0-android -name "*-Signed.aab" 2>/dev/null | head -1)
if [ -n "$AAB_FILE" ]; then
  ls -lh "$AAB_FILE"
  echo "✅ Android AAB found"

  # Check signature
  if jarsigner -verify "$AAB_FILE" &> /dev/null; then
    echo "✅ AAB is properly signed"
  else
    echo "⚠️  AAB signature issue detected"
  fi

  # Check file size
  SIZE=$(du -h "$AAB_FILE" | cut -f1)
  echo "   Size: $SIZE"
  echo "   Expected: < 30MB for initial review"
else
  echo "❌ Android AAB not found"
  echo "   Run: ./build-android-release.sh"
fi

echo ""

# Check App Icon
echo "🎨 APP ICON:"
echo "-----------------------------------------------------------"
ICON_PATH="src/LunarCalendar.MobileApp/Resources/AppIcon/appicon.png"
if [ -f "$ICON_PATH" ]; then
  echo "✅ App icon found: $ICON_PATH"

  # Check dimensions
  DIMENSIONS=$(sips -g pixelWidth -g pixelHeight "$ICON_PATH" 2>/dev/null | grep pixel | awk '{print $2}')
  WIDTH=$(echo "$DIMENSIONS" | head -1)
  HEIGHT=$(echo "$DIMENSIONS" | tail -1)

  if [ "$WIDTH" = "1024" ] && [ "$HEIGHT" = "1024" ]; then
    echo "✅ Icon dimensions correct: ${WIDTH}x${HEIGHT}"
  else
    echo "⚠️  Icon dimensions: ${WIDTH}x${HEIGHT} (expected 1024x1024)"
  fi

  # Check file format
  FILE_TYPE=$(file "$ICON_PATH" | grep -o "PNG")
  if [ -n "$FILE_TYPE" ]; then
    echo "✅ Icon format: PNG"
  else
    echo "⚠️  Icon format issue - must be PNG"
  fi
else
  echo "❌ App icon not found"
fi

echo ""

# Check Screenshots
echo "📸 SCREENSHOTS:"
echo "-----------------------------------------------------------"
SCREENSHOT_COUNT=$(ls -1 *.png *.jpg 2>/dev/null | grep -v "lunar_bg.png" | wc -l | xargs)

if [ "$SCREENSHOT_COUNT" -ge 4 ]; then
  echo "✅ Found $SCREENSHOT_COUNT screenshot files"
  echo ""
  echo "Screenshots found:"
  ls -lh *.png *.jpg 2>/dev/null | grep -v "lunar_bg.png" | awk '{print "   " $9 " (" $5 ")"}'

  echo ""
  echo "Screenshot requirements:"
  echo "   iOS:     1290 x 2796 pixels (iPhone 6.7\") - minimum 3"
  echo "   Android: 1080 x 1920 pixels (or similar) - minimum 2"
  echo ""

  # Check specific screenshot dimensions
  for img in android_coral_fixed.png ios_coral_fixed.png; do
    if [ -f "$img" ]; then
      DIMS=$(sips -g pixelWidth -g pixelHeight "$img" 2>/dev/null | grep pixel | awk '{print $2}')
      W=$(echo "$DIMS" | head -1)
      H=$(echo "$DIMS" | tail -1)
      echo "   $img: ${W}x${H}"
    fi
  done
else
  echo "⚠️  Only found $SCREENSHOT_COUNT screenshots (need at least 4)"
  echo "   Recommended: 4-8 screenshots showing key features"
fi

echo ""

# Check for Feature Graphic (Android only)
echo "🎯 FEATURE GRAPHIC (Android):"
echo "-----------------------------------------------------------"
FEATURE_GRAPHIC=$(ls -1 *feature*.png *banner*.png 2>/dev/null | head -1)
if [ -n "$FEATURE_GRAPHIC" ]; then
  echo "✅ Feature graphic found: $FEATURE_GRAPHIC"
  DIMS=$(sips -g pixelWidth -g pixelHeight "$FEATURE_GRAPHIC" 2>/dev/null | grep pixel | awk '{print $2}')
  W=$(echo "$DIMS" | head -1)
  H=$(echo "$DIMS" | tail -1)

  if [ "$W" = "1024" ] && [ "$H" = "500" ]; then
    echo "✅ Dimensions correct: ${W}x${H}"
  else
    echo "⚠️  Dimensions: ${W}x${H} (expected 1024x500)"
  fi
else
  echo "❌ Feature graphic not found"
  echo "   Android requires: 1024 x 500 pixels banner image"
  echo "   Create with: Canva, Figma, or image editor"
fi

echo ""

# Check Privacy Policy
echo "📄 PRIVACY POLICY:"
echo "-----------------------------------------------------------"
if [ -f "PRIVACY_POLICY.md" ]; then
  echo "✅ Privacy policy file exists"
  echo "   Next: Host online and get URL"
  echo "   Options: GitHub Pages, GitHub Gist, Google Docs"
else
  echo "⚠️  Privacy policy file not found"
  echo "   Create: PRIVACY_POLICY.md"
  echo "   Reference: MVP_RELEASE_GUIDE_2026.md (Assets section)"
fi

echo ""

# Check Support Contact
echo "📧 SUPPORT CONTACT:"
echo "-----------------------------------------------------------"
echo "⚠️  Ensure you have:"
echo "   - Support email set up"
echo "   - Auto-responder configured (optional)"
echo "   - Support URL (can use GitHub repo)"

echo ""
echo "═══════════════════════════════════════════════════════════"
echo ""

# Summary
echo "📊 SUBMISSION READINESS SUMMARY:"
echo "-----------------------------------------------------------"

READY_COUNT=0
TOTAL_COUNT=8

# Count ready items
[ -n "$IPA_FILE" ] && ((READY_COUNT++))
[ -n "$AAB_FILE" ] && ((READY_COUNT++))
[ -f "$ICON_PATH" ] && ((READY_COUNT++))
[ "$SCREENSHOT_COUNT" -ge 4 ] && ((READY_COUNT++))

echo "Ready: $READY_COUNT / $TOTAL_COUNT items"
echo ""

if [ "$READY_COUNT" -ge 6 ]; then
  echo "✅ You're almost ready to submit!"
  echo ""
  echo "📋 Final checklist:"
  echo "   1. Create privacy policy and host online"
  echo "   2. Set up support email"
  echo "   3. Create feature graphic for Android (1024x500)"
  echo "   4. Review App Store Connect listing"
  echo "   5. Review Google Play Console listing"
  echo "   6. Submit for review!"
elif [ "$READY_COUNT" -ge 3 ]; then
  echo "⚠️  Getting there - need more preparation"
  echo ""
  echo "📋 Next steps:"
  [ -z "$IPA_FILE" ] && echo "   - Build iOS release: ./build-ios-release.sh"
  [ -z "$AAB_FILE" ] && echo "   - Build Android release: ./build-android-release.sh"
  [ ! -f "$ICON_PATH" ] && echo "   - Create app icon (1024x1024 PNG)"
  [ "$SCREENSHOT_COUNT" -lt 4 ] && echo "   - Capture more screenshots (need at least 4)"
else
  echo "❌ Significant work needed before submission"
  echo ""
  echo "📖 See MVP_RELEASE_GUIDE_2026.md for detailed instructions"
fi

echo ""
echo "═══════════════════════════════════════════════════════════"
