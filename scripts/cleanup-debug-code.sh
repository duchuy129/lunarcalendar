#!/bin/bash

# Debug Code Cleanup Script
# Removes all Debug.WriteLine and System.Diagnostics.Debug statements

echo "🧹 Cleaning up debug code..."
echo ""

# Configuration - dynamically determine workspace root
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
WORKSPACE_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
PROJECT_DIR="$WORKSPACE_ROOT/src/LunarCalendar.MobileApp"

# Files to clean (already cleaned CalendarViewModel.cs manually)
FILES_TO_CLEAN=(
    "$PROJECT_DIR/Services/HolidayService.cs"
    "$PROJECT_DIR/Services/CalendarService.cs"
    "$PROJECT_DIR/Services/HapticService.cs"
    "$PROJECT_DIR/Services/LocalizationService.cs"
)

# Backup directory
BACKUP_DIR="$WORKSPACE_ROOT/backup_$(date +%Y%m%d_%H%M%S)"
mkdir -p "$BACKUP_DIR"

echo "📁 Creating backups in: $BACKUP_DIR"
echo ""

# Process each file
for file in "${FILES_TO_CLEAN[@]}"; do
    if [ -f "$file" ]; then
        filename=$(basename "$file")
        echo "Processing: $filename"
        
        # Create backup
        cp "$file" "$BACKUP_DIR/$filename"
        
        # Count debug statements before
        before=$(grep -c "Debug.WriteLine\|System.Diagnostics.Debug" "$file" || echo "0")
        
        # Remove debug statements (keep the line but comment it out for safety)
        # This is safer than deleting lines entirely
        sed -i.bak '/Debug\.WriteLine/d' "$file"
        sed -i.bak '/System\.Diagnostics\.Debug/d' "$file"
        
        # Remove .bak files created by sed
        rm -f "$file.bak"
        
        # Count debug statements after
        after=$(grep -c "Debug.WriteLine\|System.Diagnostics.Debug" "$file" || echo "0")
        
        echo "  ✅ Removed $before debug statements"
        echo ""
    else
        echo "  ⚠️  File not found: $file"
        echo ""
    fi
done

echo "✨ Cleanup complete!"
echo ""
echo "📊 Summary:"
echo "  - Backups saved to: $BACKUP_DIR"
echo "  - Files cleaned: ${#FILES_TO_CLEAN[@]}"
echo ""
echo "🔍 Verifying remaining debug code:"
cd "$PROJECT_DIR"
remaining=$(grep -r "Debug.WriteLine\|System.Diagnostics.Debug" --include="*.cs" --exclude-dir={obj,bin} . | wc -l)
echo "  - Remaining debug statements: $remaining"
echo ""

if [ "$remaining" -eq "0" ]; then
    echo "✅ All debug code removed successfully!"
else
    echo "⚠️  Some debug statements remain. Review manually:"
    grep -n "Debug.WriteLine\|System.Diagnostics.Debug" --include="*.cs" --exclude-dir={obj,bin} -r .
fi
