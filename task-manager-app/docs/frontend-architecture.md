# Frontend Architecture

Angular SPA for the Task Manager. Coding conventions are in [.cursor/rules/best-practices.mdc](../.cursor/rules/best-practices.mdc).

## References

- Domain model: [../../docs/domain-model.md](../../docs/domain-model.md)
- API contract: [../task-manager-api/docs/openapi.yaml](../task-manager-api/docs/openapi.yaml)
- User story: [../../docs/user-story.md](../../docs/user-story.md)

## Routes

| Path | Access | Description |
|------|--------|-------------|
| `/login` | Public | Login form |
| `/register` | Public | Registration form |
| `/tasks` | Protected | Task list and CRUD (lazy-loaded) |
| `/` | — | Redirect to `/tasks` if authenticated, else `/login` |

Protected routes use an auth guard. Unauthenticated users redirect to `/login`.

## Folder structure

```
src/app/
  core/
    auth/           # AuthService, auth guard
    interceptors/   # JWT Bearer interceptor
  features/
    tasks/          # Task list, form components (lazy route)
  shared/           # Reusable UI components, models/interfaces
```

## Services

| Service | Responsibility |
|---------|----------------|
| `AuthService` | Register, login, logout, token storage (sessionStorage) |
| `TaskService` | CRUD calls to `/api/tasks` |

Services are thin — no domain validation logic. The API enforces business rules.

## State

- Use Signals for component and feature state.
- Use Signal Forms for login, register, and task forms.
- Use `computed()` for derived state (e.g. filtered lists).

## Auth flow

1. User logs in → API returns JWT in `AuthResponse.token`.
2. Store token in `sessionStorage`.
3. HTTP interceptor adds `Authorization: Bearer <token>` to API requests.
4. On 401 response, clear token and redirect to `/login`.

## API base URL

Configure in environment files (e.g. `https://localhost:7001`). Must match API `launchSettings.json`.
