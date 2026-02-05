#!/bin/bash

echo "📊 Sprint 9 (v1.1.0) Post-Deployment Monitoring Dashboard"
echo "============================================================"
echo ""

# Configuration
WORKSPACE_ROOT="$( cd "$( dirname "${BASH_SOURCE[0]}" )/.." && pwd )"
VERSION="1.1.0"
BUILD="6"
RELEASE_DATE="2026-02-12"  # Update with actual release date

# Colors
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo "📱 App Information"
echo "   Version: $VERSION (Build $BUILD)"
echo "   Release Date: $RELEASE_DATE"
echo "   Branch: feature/001-sexagenary-cycle-complete"
echo ""

# Calculate days since release
RELEASE_TIMESTAMP=$(date -j -f "%Y-%m-%d" "$RELEASE_DATE" "+%s" 2>/dev/null || echo "0")
CURRENT_TIMESTAMP=$(date "+%s")
DAYS_SINCE_RELEASE=$(( (CURRENT_TIMESTAMP - RELEASE_TIMESTAMP) / 86400 ))

if [ $DAYS_SINCE_RELEASE -lt 0 ]; then
    echo -e "${BLUE}ℹ️  Status: Pre-Release (T-${DAYS_SINCE_RELEASE#-} days)${NC}"
    echo ""
elif [ $DAYS_SINCE_RELEASE -eq 0 ]; then
    echo -e "${GREEN}🚀 Status: RELEASE DAY!${NC}"
    echo ""
elif [ $DAYS_SINCE_RELEASE -le 7 ]; then
    echo -e "${YELLOW}⚠️  Status: Phased Rollout (Day $DAYS_SINCE_RELEASE of 7)${NC}"
    echo ""
else
    echo -e "${GREEN}✅ Status: Full Rollout Complete${NC}"
    echo ""
fi

# Phased rollout schedule
echo "📈 Phased Rollout Schedule"
echo "   Day 1: 1% of users"
echo "   Day 2: 2% of users"
echo "   Day 3: 5% of users"
echo "   Day 4: 10% of users"
echo "   Day 5: 20% of users"
echo "   Day 6: 50% of users"
echo "   Day 7: 100% of users (COMPLETE)"
echo ""

# Check for crash logs
echo "🔍 Checking for Recent Crash Logs..."
CRASH_LOG_DIR="$HOME/Library/Logs/DiagnosticReports"
CRASH_COUNT=0

if [ -d "$CRASH_LOG_DIR" ]; then
    # Look for crashes in last 7 days
    CRASH_COUNT=$(find "$CRASH_LOG_DIR" -name "*LunarCalendar*" -mtime -7 -type f 2>/dev/null | wc -l | tr -d ' ')
    
    if [ $CRASH_COUNT -eq 0 ]; then
        echo -e "   ${GREEN}✅ No local crash logs found (last 7 days)${NC}"
    else
        echo -e "   ${RED}⚠️  Found $CRASH_COUNT crash log(s) in last 7 days${NC}"
        echo "   Review: $CRASH_LOG_DIR"
    fi
else
    echo "   ℹ️  No crash log directory found"
fi
echo ""

# Build validation
echo "🏗️  Build Validation"

# Check if IPA exists
IPA_PATH="$WORKSPACE_ROOT/src/LunarCalendar.MobileApp/bin/Release/net10.0-ios/ios-arm64/LunarCalendar.MobileApp.ipa"
if [ -f "$IPA_PATH" ]; then
    IPA_SIZE=$(ls -lh "$IPA_PATH" | awk '{print $5}')
    echo -e "   ${GREEN}✅ IPA exists: $IPA_SIZE${NC}"
else
    echo -e "   ${YELLOW}⚠️  IPA not found (may have been cleaned)${NC}"
fi

# Check git tag
GIT_TAG_EXISTS=$(git tag -l "v$VERSION" 2>/dev/null)
if [ -n "$GIT_TAG_EXISTS" ]; then
    echo -e "   ${GREEN}✅ Git tag exists: v$VERSION${NC}"
else
    echo -e "   ${RED}❌ Git tag missing: v$VERSION${NC}"
    echo "   Create with: git tag -a v$VERSION -m \"Release v$VERSION - Sprint 9\""
fi
echo ""

# Test status
echo "🧪 Test Coverage"
cd "$WORKSPACE_ROOT"
TEST_PROJECT="tests/LunarCalendar.Core.Tests/LunarCalendar.Core.Tests.csproj"

if [ -f "$TEST_PROJECT" ]; then
    echo "   Running tests..."
    TEST_OUTPUT=$(dotnet test "$TEST_PROJECT" --verbosity quiet 2>&1)
    TEST_RESULT=$?
    
    if [ $TEST_RESULT -eq 0 ]; then
        PASSED=$(echo "$TEST_OUTPUT" | grep -o "Passed: [0-9]*" | grep -o "[0-9]*" || echo "0")
        FAILED=$(echo "$TEST_OUTPUT" | grep -o "Failed: [0-9]*" | grep -o "[0-9]*" || echo "0")
        
        if [ "$FAILED" -eq "0" ]; then
            echo -e "   ${GREEN}✅ All tests passing: $PASSED/108${NC}"
        else
            echo -e "   ${RED}❌ Tests failing: $FAILED failed, $PASSED passed${NC}"
        fi
    else
        echo -e "   ${RED}❌ Test run failed${NC}"
    fi
else
    echo -e "   ${YELLOW}⚠️  Test project not found${NC}"
fi
echo ""

# Documentation check
echo "📚 Documentation Status"
DOCS=(
    "docs/APP_STORE_DEPLOYMENT_RUNBOOK.md"
    "docs/SPRINT9_DEPLOYMENT_CHECKLIST.md"
    "docs/QUICK_DEPLOY_SPRINT9.md"
    "VERSION_HISTORY.md"
    "SPRINT9_IMPLEMENTATION_COMPLETE.md"
)

for doc in "${DOCS[@]}"; do
    if [ -f "$WORKSPACE_ROOT/$doc" ]; then
        echo -e "   ${GREEN}✅${NC} $doc"
    else
        echo -e "   ${RED}❌${NC} $doc (missing)"
    fi
done
echo ""

# Xcode crash logs check
echo "🔧 Xcode Organizer Crash Logs"
echo "   Manual check required:"
echo "   1. Open Xcode"
echo "   2. Window → Organizer → Crashes"
echo "   3. Look for 'LunarCalendar' app"
echo "   4. Target crash rate: <0.1%"
echo ""

# App Store Connect info
echo "🍎 App Store Connect"
echo "   URL: https://appstoreconnect.apple.com"
echo ""
echo "   Check these metrics:"
echo "   📊 Analytics → App Store Views, Downloads, Conversion"
echo "   ⭐ Ratings & Reviews → Current rating (target: >4.0)"
echo "   💥 Crashes → Crash rate (target: <0.1%)"
echo "   📈 Phased Release → Current rollout percentage"
echo ""

# Success metrics template
echo "📊 Success Metrics (Update Manually)"
echo "┌────────────────────────────┬─────────┬─────────┬────────┐"
echo "│ Metric                     │ Target  │ Actual  │ Status │"
echo "├────────────────────────────┼─────────┼─────────┼────────┤"
echo "│ Crash-free rate            │ >99.5%  │ ____%   │ ⏳     │"
echo "│ App Store rating           │ >4.0    │ ___     │ ⏳     │"
echo "│ Downloads (Week 1)         │ 100+    │ ___     │ ⏳     │"
echo "│ Positive reviews           │ >80%    │ ____%   │ ⏳     │"
echo "│ Support tickets            │ <10     │ ___     │ ⏳     │"
echo "└────────────────────────────┴─────────┴─────────┴────────┘"
echo ""

# Action items
echo "✅ Recommended Actions"
if [ $DAYS_SINCE_RELEASE -lt 0 ]; then
    echo "   📋 Pre-Release"
    echo "   - Complete testing checklist"
    echo "   - Prepare screenshots"
    echo "   - Review release notes"
    echo "   - Build and validate IPA"
elif [ $DAYS_SINCE_RELEASE -eq 0 ]; then
    echo "   🚀 Release Day"
    echo "   - Monitor crash reports every 2 hours"
    echo "   - Check App Store Connect analytics"
    echo "   - Respond to early reviews"
    echo "   - Verify phased rollout at 1%"
elif [ $DAYS_SINCE_RELEASE -le 7 ]; then
    echo "   📈 Phased Rollout (Day $DAYS_SINCE_RELEASE)"
    echo "   - Check crash rate daily"
    echo "   - Monitor user reviews"
    echo "   - Track download metrics"
    echo "   - Prepare hotfix if needed"
else
    echo "   ✅ Post-Rollout"
    echo "   - Compile user feedback report"
    echo "   - Plan next sprint (v1.2.0)"
    echo "   - Update documentation with lessons learned"
    echo "   - Celebrate success! 🎉"
fi
echo ""

# Quick links
echo "🔗 Quick Links"
echo "   App Store Connect: https://appstoreconnect.apple.com"
echo "   Apple Developer:   https://developer.apple.com/account"
echo "   GitHub Repo:       https://github.com/duchuy129/lunarcalendar"
echo ""

# Troubleshooting
echo "🆘 Troubleshooting"
echo "   Critical bug found?    → Pause rollout in App Store Connect"
echo "   Need hotfix?           → Create v1.1.1 with expedited review"
echo "   High crash rate?       → Check Xcode Organizer for crash logs"
echo "   Negative reviews?      → Respond within 24 hours, investigate issues"
echo "   Rollback needed?       → See docs/APP_STORE_DEPLOYMENT_RUNBOOK.md"
echo ""

echo "============================================================"
echo "📊 Dashboard generated: $(date '+%Y-%m-%d %H:%M:%S')"
echo "============================================================"
echo ""
echo "💡 Tip: Run this script daily during rollout to track progress"
echo "   Usage: bash scripts/monitor-sprint9-deployment.sh"
echo ""
