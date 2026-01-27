# Vietnamese Lunar Calendar Constitution

## Core Principles

### I. Offline-First Architecture (NON-NEGOTIABLE)
**All core functionality must work without internet connectivity.**

- Lunar date calculations must execute on-device using bundled algorithms
- Holiday data must be embedded in the application
- No network dependency for primary features (calendar, holidays, conversions)
- Local SQLite database for caching and user preferences
- Graceful degradation when offline (no error states for missing connectivity)

**Rationale:** Users rely on this app for time-sensitive cultural events and may not have reliable internet access. Offline capability ensures reliability and instant performance.

### II. Cultural Accuracy & Authenticity
**Cultural information must be accurate, respectful, and properly localized.**

- All holiday data must be verified against authoritative Vietnamese sources
- Translations must be culturally appropriate, not just literal
- Dates must align with Vietnamese lunar calendar conventions (not generic Chinese lunar)
- Religious and cultural context must be presented with respect and accuracy
- Changes to cultural content require review by cultural subject matter experts

**Rationale:** This app serves Vietnamese diaspora and cultural practitioners who depend on accurate information for important life events and religious practices.

### III. Privacy & Guest-First Design
**The app must provide full functionality without requiring user accounts or data collection.**

- No forced registration or authentication
- No tracking or analytics without explicit opt-in
- Local data storage only (no cloud sync required)
- Clear data policies and user controls
- Optional features may require opt-in authentication

**Rationale:** Cultural and religious practices are personal. Users must feel confident their usage patterns and data remain private.

### IV. Cross-Platform Consistency
**User experience must be consistent across iOS, Android, and tablet devices.**

- Shared business logic in LunarCalendar.Core library
- MVVM pattern for clean separation of concerns
- Platform-specific code isolated to Platforms/ folders
- Responsive layouts that adapt to iPhone, iPad, Android phones, and tablets
- Consistent navigation patterns and UI conventions across platforms

**Rationale:** Users may access the app on multiple devices. Consistency reduces learning curve and builds trust.

### V. Performance & Responsiveness
**The app must feel instant and responsive in all interactions.**

- Calendar calculations must complete in < 100ms
- UI interactions must provide immediate feedback (haptic, visual, audio)
- Month/year navigation must be smooth and animated
- No loading spinners for core calculations
- Lazy loading for large data sets (e.g., year-level holiday lists)

**Rationale:** Calendar lookups are time-sensitive. Slow responses frustrate users and reduce app utility.

### VI. Bilingual Support (Vietnamese & English)
**All user-facing content must be fully localized in Vietnamese and English.**

- Use .NET Resource files (.resx) for string localization
- Support dynamic language switching without app restart
- Format dates, numbers, and currencies according to locale
- Maintain parallel documentation in both languages
- Test all features in both languages before release

**Rationale:** The app serves Vietnamese speakers and English-speaking diaspora. Both audiences deserve equal access to full functionality.

### VII. Test Coverage & Quality Assurance
**Code quality and correctness are non-negotiable.**

- Unit tests required for all business logic in LunarCalendar.Core
- Integration tests for service layer (CalendarService, HolidayService)
- UI tests for critical user flows (navigation, date selection, holiday viewing)
- Test coverage minimum: 80% for Core library, 60% for mobile app
- All tests must pass before merging to main branch

**Rationale:** Incorrect lunar date calculations can cause users to miss important cultural events. Testing prevents these critical failures.

## Technology Constraints

### Required Technology Stack
- **.NET MAUI** (Multi-platform App UI) with **.NET 10.0+**
- **C# 12+** for all application code
- **XAML** for UI definitions
- **SQLite** (sqlite-net-pcl) for local data storage
- **CommunityToolkit.Mvvm** for MVVM infrastructure
- **ChineseLunisolarCalendar** (.NET built-in) for lunar calculations

### Architectural Patterns
- **MVVM (Model-View-ViewModel)** for presentation layer
- **Repository Pattern** for data access
- **Dependency Injection** for service management
- **Command Pattern** for user interactions

### Platform Support
- **iOS**: Minimum 15.0+ (iPhone & iPad)
- **Android**: Minimum API 26 (Android 8.0+), Target API 36 (Android 14+)
- **Future**: macOS, Windows (not currently prioritized)

### Code Quality Standards
- Follow .NET coding conventions and naming standards
- Use nullable reference types (C# 8.0+)
- Avoid suppressing warnings without documented justification
- Use async/await for I/O operations
- Prefer composition over inheritance
- Keep methods under 50 lines, classes under 500 lines

## Development Workflow

### Feature Development Process
1. **Specification Phase**: Document feature requirements in `.specify/features/`
2. **Design Phase**: Create technical plan with architecture decisions
3. **Implementation Phase**: Develop in feature branches with regular commits
4. **Testing Phase**: Write and pass all required tests
5. **Review Phase**: Code review with at least one approver
6. **Documentation Phase**: Update user docs and API reference
7. **Deployment Phase**: Release to TestFlight/Internal Testing → Production

### Branching Strategy
- `main` branch always deployable
- Feature branches: `feature/[issue-number]-[short-description]`
- Hotfix branches: `hotfix/[issue-number]-[short-description]`
- Release branches: `release/[version]`

### Code Review Requirements
- All code must be reviewed before merging to main
- Reviewer must verify:
  - Code follows architectural principles
  - Tests are present and passing
  - Documentation is updated
  - No hardcoded strings (use localization)
  - No performance regressions
  - Platform-specific code is properly isolated

### Testing Requirements
- Unit tests for all public methods in Core library
- Integration tests for service interactions
- UI tests for critical paths: calendar navigation, date selection, holiday details
- Manual testing on physical devices (iOS and Android) before production release
- Regression testing for bug fixes

### Documentation Standards
- Update README.md for new features
- Document public APIs with XML comments
- Maintain CHANGELOG.md for all releases
- Keep architecture diagrams current
- Create user guides for complex features

## Quality Gates

### Pre-Merge Checklist
- [ ] All unit tests pass
- [ ] Integration tests pass
- [ ] UI tests pass (if applicable)
- [ ] Code reviewed and approved
- [ ] No compiler warnings
- [ ] Documentation updated
- [ ] Localization strings added for both languages
- [ ] Manual testing completed on at least one iOS and one Android device

### Pre-Release Checklist
- [ ] Full test suite passes
- [ ] Performance benchmarks met (< 100ms for calculations)
- [ ] Manual testing on multiple devices (iPhone, iPad, Android phone, Android tablet)
- [ ] Accessibility testing (VoiceOver, TalkBack)
- [ ] Localization verification (Vietnamese and English)
- [ ] Privacy policy compliance verified
- [ ] App store metadata updated (screenshots, descriptions)
- [ ] Release notes prepared

## Governance

### Amendment Process
This constitution supersedes all other development practices and guidelines. Amendments require:
1. Documented proposal with rationale
2. Team discussion and consensus
3. Update to this document with version increment
4. Communication to all contributors
5. Migration plan if changes affect existing code

### Compliance
- All pull requests must demonstrate compliance with these principles
- Non-compliance requires documented exception with justification
- Repeated violations trigger architecture review

### Exception Handling
Exceptions to these principles require:
- Written justification
- Approval from project maintainer
- Documentation in code comments
- Plan to remove technical debt in future release

**Version**: 1.0.0 | **Ratified**: January 25, 2026 | **Last Amended**: January 25, 2026
