# Task Manager API

ASP.NET Core REST API for the Task Manager monorepo. Handles JWT authentication and task CRUD with user isolation.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)

## Run locally

From this folder:

```bash
dotnet run --project src/TaskManager.Api --launch-profile http
```

The API listens at **http://localhost:5221**.

- Swagger UI: http://localhost:5221/swagger
- OpenAPI spec: http://localhost:5221/openapi/v1.yaml

## Demo data seeding

In **Development**, the API seeds demo data automatically on startup when the database has no users.

Configuration in `src/TaskManager.Api/appsettings.Development.json`:

| Setting | Default (Development) | Purpose |
|---------|----------------------|---------|
| `SeedDemoData` | `true` | Inserts a demo user and sample tasks when the database is empty |

### Demo account

| Field | Value |
|-------|-------|
| Email | `demo@example.com` |
| Password | `Demo123!` |

### Sample tasks

The seeder creates 3 tasks for the demo user (Completed, InProgress, and Pending).

### Reset demo data

Delete the SQLite file and restart the API:

```bash
# From task-manager-api/
Remove-Item src/TaskManager.Api/taskmanager.db -ErrorAction SilentlyContinue
dotnet run --project src/TaskManager.Api --launch-profile http
```

Seeding is **idempotent**: if users already exist, startup skips it.

Implementation: `src/TaskManager.Infrastructure/Persistence/DemoDataSeeder.cs`

## Development settings

| Setting | Purpose |
|---------|---------|
| `Cors:AllowedOrigins` | Allows the Angular dev server (`http://localhost:4200`) |

## Database

SQLite file: `src/TaskManager.Api/taskmanager.db` (created on first run).

Migrations run automatically at startup.

## Tests

```bash
dotnet test
```

## Documentation

- [OpenAPI contract](docs/openapi.yaml)
- [API summary](docs/api-contract.md)
- [Clean Architecture](docs/clean-architecture.md)
- [Technical decisions](docs/tech-decisions.md)

## Frontend

Run the Angular app from [../task-manager-app](../task-manager-app) and point it at `http://localhost:5221` (configured in `environment.development.ts`).
