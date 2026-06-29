# Project Context

The application is a Task Management System where registered users can manage their own tasks.

## Main goals

- Provide CRUD API endpoints.
- Provide authentication and authorization.
- Provide a simple responsive frontend.
- Show responsible GenAI usage.

## Monorepo layout

```
task-manager/
├── AGENTS.md              # Agent entry point (routing)
├── docs/                  # Shared domain docs and implementation plan
├── memory/                # Shared business context
├── task-manager-api/      # Backend (.NET)
└── task-manager-app/      # Frontend (Angular)
```

## What to read in each repo

| Working on | Read first | Rules |
|------------|------------|-------|
| Backend | [docs/domain-model.md](../docs/domain-model.md), [task-manager-api/docs/openapi.yaml](../task-manager-api/docs/openapi.yaml) | [clean-architecture.mdc](../task-manager-api/.cursor/rules/clean-architecture.mdc) |
| Frontend | [docs/domain-model.md](../docs/domain-model.md), [task-manager-api/docs/openapi.yaml](../task-manager-api/docs/openapi.yaml) | [best-practices.mdc](../task-manager-app/.cursor/rules/best-practices.mdc) |
| Either | [docs/implementation-plan.md](../docs/implementation-plan.md), [business-rules.md](business-rules.md) | — |
