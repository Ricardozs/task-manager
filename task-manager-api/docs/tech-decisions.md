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
