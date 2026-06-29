# Agent Guide

Monorepo router for Cursor. This file contains navigation only — no repo-specific technical rules.

## Workflow

1. Open [docs/implementation-plan.md](docs/implementation-plan.md) and identify the current phase.
2. Read shared context in order:
   - [memory/project-context.md](memory/project-context.md)
   - [memory/business-rules.md](memory/business-rules.md)
   - [docs/domain-model.md](docs/domain-model.md)
3. Work in the repo indicated by the phase:
   - **Backend (phases 0–2, partial 5):** `task-manager-api/` — read its `docs/` and apply `.cursor/rules/`
   - **Frontend (phases 3–4, partial 5):** `task-manager-app/` — read its `docs/` and apply `.cursor/rules/`
4. HTTP contract (owned by API): [task-manager-api/docs/openapi.yaml](task-manager-api/docs/openapi.yaml)
5. Complete one phase per session. Run tests before marking done. Do not advance if tests fail.

## Commands

```bash
# Backend
cd task-manager-api && dotnet build && dotnet test

# Frontend
cd task-manager-app && npm install && npm test && ng serve
```

## Repo index

| Repo | Rules | Docs |
|------|-------|------|
| [task-manager-api](task-manager-api/) | `.cursor/rules/clean-architecture.mdc` | `docs/openapi.yaml`, `docs/api-contract.md`, `docs/clean-architecture.md`, `docs/tech-decisions.md` |
| [task-manager-app](task-manager-app/) | `.cursor/rules/best-practices.mdc` | `docs/frontend-architecture.md` |

## Shared docs (root)

- [docs/user-story.md](docs/user-story.md) — acceptance criteria
- [docs/domain-model.md](docs/domain-model.md) — entities and relationships
- [docs/implementation-plan.md](docs/implementation-plan.md) — phased execution plan
- [memory/ai-instructions.md](memory/ai-instructions.md) — monorepo workflow rules
