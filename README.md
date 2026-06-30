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

### End-to-end (development)

1. **API** — from `task-manager-api`:

   ```bash
   dotnet run --project src/TaskManager.Api --launch-profile http
   ```

2. **App** — from `task-manager-app`:

   ```bash
   npm install
   npm start
   ```

3. Open http://localhost:4200 and log in with the demo account (`demo@example.com` / `Demo123!`) on a fresh database, or register a new user.

### Demo data

The API seeds a demo user and 3 sample tasks automatically in Development when the database is empty. See [task-manager-api/README.md](task-manager-api/README.md#demo-data-seeding) for credentials and reset instructions.

See each repo's README for details:

- [task-manager-api/README.md](task-manager-api/README.md)
- [task-manager-app/README.md](task-manager-app/README.md)

### Agent workflow

1. Read [AGENTS.md](AGENTS.md) for the agent workflow.
2. Follow [docs/implementation-plan.md](docs/implementation-plan.md) phase by phase.

## Documentation

- [User story](docs/user-story.md)
- [Domain model](docs/domain-model.md)
- [API contract](task-manager-api/docs/openapi.yaml)
- [Business rules](memory/business-rules.md)
