---
agent: speckit.tasks
---

# speckit.tasks prompt (repo-local)

You are running the **spec-kit task breakdown** step for this repository.

## Inputs

- The feature specification: `.specify/features/<feature-id>-*.md` (e.g., `002-zodiac-animals.md`)
- The technical plan: `.specify/features/<feature-id>-*-plan.md` (e.g., `002-zodiac-animals-plan.md`)
- Sprint decisions: `.specify/features/SPRINT10-DECISIONS.md` (or sprint-specific decisions doc)

## Output

Create a task breakdown document:

- File: `.specify/features/<feature-id>-<feature-slug>-tasks.md` (e.g., `002-zodiac-animals-tasks.md`)

## Task breakdown requirements

Generate a developer-executable list of tasks that fully covers:

- All **P1 user stories** first, then P2, then P3.
- All **functional requirements** (FR-001 …) with traceability.
- All major deliverables from the plan (services, models, pages, localization, tests).

Each task MUST include:

- **ID**: `T###`
- **Title** (imperative)
- **Priority**: P1/P2/P3
- **Estimate**: (e.g., 2h, 4h, 1d)
- **Dependencies**: list of `T###` IDs (or `None`)
- **Scope**: what is included / excluded
- **Acceptance criteria**: testable bullet list
- **Files**: concrete likely file paths to create/modify
- **FR / Story mapping**: reference the covered requirements and user stories

## Phasing

Group tasks into phases that match the technical plan (or a sensible subset):

1. Prep / research / scaffolding
2. Core library + models
3. Data (JSON/resx)
4. UI components + pages
5. Integration points (calendar header, details, settings)
6. Testing + perf + accessibility
7. Polish + docs

## Repo-specific Sprint 10 constraints

- Use **Unicode emoji** for zodiac visuals in Sprint 10 (no SVG/PNG asset pipeline tasks).
- Localization: **Vietnamese + English only** for Sprint 10; Chinese UI localization is deferred.
- Offline-first: bundled data only.
- Prefer xUnit tests for core logic.

## Output quality bar

- Provide enough detail that someone can implement tasks without re-reading the entire plan.
- Avoid overly large tasks (> 1 day) when possible; split by layer (Core/Data/UI/Tests).
- Include at least a small set of explicit validation steps (unit tests, sample years, boundary dates).
