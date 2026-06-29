# Task Manager

Monorepo for a personal task management system. Registered users authenticate via JWT and manage their own tasks.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (LTS)

## Repos

| Folder | Stack | Purpose |
|--------|-------|---------|
| [task-manager-api](task-manager-api/) | ASP.NET Core, EF Core, SQLite | REST API + auth |
| [task-manager-app](task-manager-app/) | Angular | SPA frontend |

## Getting started

1. Read [AGENTS.md](AGENTS.md) for the agent workflow.
2. Follow [docs/implementation-plan.md](docs/implementation-plan.md) phase by phase.

## Documentation

- [User story](docs/user-story.md)
- [Domain model](docs/domain-model.md)
- [API contract](task-manager-api/docs/openapi.yaml)
- [Business rules](memory/business-rules.md)
