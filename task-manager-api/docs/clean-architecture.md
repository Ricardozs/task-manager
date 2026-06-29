# Clean Architecture Rules

Dependencies must point inward:

```
Api -> Application -> Domain
Infrastructure -> Application / Domain
```

## Layers

### Domain

- Entities
- Value objects
- Enums
- Domain rules

### Application

- Use cases
- DTOs
- Interfaces
- Validation
- Business logic orchestration

### Infrastructure

- EF Core
- Repositories
- Identity / JWT implementation
- Database configuration

### Api

- Controllers
- Auth middleware
- Request/response mapping

## Project structure

```
task-manager-api/
  src/
    TaskManager.Domain/
    TaskManager.Application/
    TaskManager.Infrastructure/
    TaskManager.Api/
  tests/
    TaskManager.Application.Tests/
```

## Naming conventions

- Commands: `CreateTaskCommand`, `UpdateTaskCommand`
- Interfaces: `ITaskRepository`, `IUserRepository`
- Controllers: `TasksController`, `AuthController`

## Shared references

- Domain model: [../../docs/domain-model.md](../../docs/domain-model.md)
- Business rules: [../../memory/business-rules.md](../../memory/business-rules.md)
- HTTP contract: [openapi.yaml](openapi.yaml)
