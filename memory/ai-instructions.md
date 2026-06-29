# AI Instructions

Monorepo workflow for agents.

## Before coding

1. Read [AGENTS.md](../AGENTS.md) and the current phase in [docs/implementation-plan.md](../docs/implementation-plan.md).
2. Work only in the repo indicated by the phase (`task-manager-api` or `task-manager-app`).
3. Apply that repo's `.cursor/rules/` — do not mix backend and frontend conventions.

## General principles

- Prefer small focused classes.
- Add or update tests before or alongside new behavior.
- Use clear names.
- Avoid overengineering.
- Keep the project interview-friendly and easy to explain.

## Contract changes

- OpenAPI is owned by `task-manager-api`. Edit [openapi.yaml](../task-manager-api/docs/openapi.yaml) only from that repo.
- If openapi.yaml changes, update [api-contract.md](../task-manager-api/docs/api-contract.md) in the same commit.
- Domain rule changes go in [business-rules.md](business-rules.md) and [domain-model.md](../docs/domain-model.md).

## Commits

- Create commits only when the user asks.
- Prefer atomic commits per phase when requested.
