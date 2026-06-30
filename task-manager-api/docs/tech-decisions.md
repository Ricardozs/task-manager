# Technical Decisions (Backend)

## Stack

| Component | Choice |
|-----------|--------|
| Runtime | .NET 9 |
| ORM | EF Core 9 |
| Database | SQLite |
| Auth | JWT via `Microsoft.AspNetCore.Authentication.JwtBearer` |
| Password hashing | ASP.NET Identity `PasswordHasher` or BCrypt |
| Tests | xUnit + FluentAssertions |

## Why SQLite?

- No installation required.
- Easy to run during the interview.
- Works well with EF Core.
- Database is stored in a single file.

## Why Clean Architecture?

- Separation of concerns.
- Easier testing.
- Independent business logic.
- Application layer does not depend on Infrastructure.

## API documentation

- OpenAPI spec: [openapi.yaml](openapi.yaml)
- Human summary: [api-contract.md](api-contract.md)
- Swagger UI enabled in development.

## Demo data seeding

Development startup can populate an empty SQLite database with a demo user and sample tasks.

| Item | Detail |
|------|--------|
| Toggle | `SeedDemoData` in `appsettings.Development.json` (default: `true`) |
| When | After migrations, only if no users exist |
| User | `demo@example.com` / `Demo123!` |
| Tasks | 3 tasks (Completed, InProgress, Pending) |
| Code | `Infrastructure/Persistence/DemoDataSeeder.cs` |

Seeding is disabled in Production and skipped when the database already has users.
