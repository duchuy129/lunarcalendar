---
agent: speckit.plan
---

# speckit.plan prompt (repo-local)

You are running the **spec-kit planning** step for this repository.

## Inputs

- The sprint/feature decision notes in `.specify/features/` (e.g., `SPRINT10-DECISIONS.md`).
- The corresponding specification doc (e.g., `002-*.md`).
- Existing repo conventions in `.specify/features/*-plan.md`.

## Output

Create or update the **technical implementation plan** document for the sprint’s main feature:

- File: `.specify/features/<feature-id>-<feature-slug>-plan.md` (e.g., `002-zodiac-animals-plan.md`)

If a plan already exists, **update it** to reflect new decisions, keep the structure stable, and record deltas.

## Planning requirements (what the plan must include)

1. **Summary**: scope, what is in/out, what success looks like.
2. **Technical context**: frameworks, language versions, key dependencies.
3. **Explicit decisions and constraints**:
	 - If the decisions doc says to defer something, reflect that in plan scope.
	 - Keep it consistent with the sprint’s localization and assets decisions.
4. **Architecture**: services, data sources, models, UI building blocks, and integrations.
5. **API contracts**: interfaces/method signatures the code will expose.
6. **Data contracts/schemas**: JSON schemas or resource files and their shapes.
7. **Testing strategy**: unit/integration/UI tests, perf targets, boundary cases.
8. **Implementation phases**: ordered phases with dependencies and rough estimates.
9. **Risks** + mitigations: cultural accuracy, platform differences, performance.
10. **Acceptance / exit criteria**: what must be true to call planning done.

## Repo-specific guidance

- Prefer **offline-first**: bundled data, no network dependency.
- Prefer **.NET MAUI MVVM** patterns used elsewhere in the repo.
- Incorporate sprint decisions:
	- Zodiac images: **Unicode emoji** (MVP) unless a later sprint adds SVG.
	- Localization: **Vietnamese + English only** for Sprint 10; Chinese UI localization deferred.
- Keep docs practical: include file paths, class names, and example signatures.

## Style

- Use Markdown headings and checklists.
- Be precise and test-driven: list measurable performance targets and test cases.
- Avoid filler; optimize for developer execution.
