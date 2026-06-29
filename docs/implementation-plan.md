# Implementation Plan

Execute **one phase per session**. Mark checkboxes when done. Do not advance if tests fail.

Entry point: [../AGENTS.md](../AGENTS.md)

| Phase | Repo        | Summary                                      |
|-------|-------------|----------------------------------------------|
| 0     | api + app   | Bootstrap solution structure                   |
| 1     | api         | Auth backend (JWT, register, login)          |
| 2     | api         | Tasks CRUD backend                           |
| 3     | app         | Auth frontend                                |
| 4     | app         | Tasks frontend                               |
| 5     | both        | Polish (CORS, READMEs, responsive)           |

---

## Phase 0 — Bootstrap

**Repo:** `task-manager-api` + `task-manager-app`

**Read:**
- [domain-model.md](domain-model.md)
- [task-manager-api/docs/clean-architecture.md](../task-manager-api/docs/clean-architecture.md)
- [task-manager-api/docs/tech-decisions.md](../task-manager-api/docs/tech-decisions.md)
- [task-manager-app/docs/frontend-architecture.md](../task-manager-app/docs/frontend-architecture.md)

**Tasks:**
- [x] Create .NET solution with projects: `Domain`, `Application`, `Infrastructure`, `Api`
- [x] Configure EF Core + SQLite; add initial empty migration
- [x] Configure Swagger aligned with [openapi.yaml](../task-manager-api/docs/openapi.yaml)
- [x] Configure Angular: routing, HttpClient, lazy-loaded feature routes
- [x] Both projects build successfully

**Done when:** `dotnet build` and `ng build` pass.

---

## Phase 1 — Auth Backend

**Repo:** `task-manager-api`

**Read:**
- [domain-model.md](domain-model.md)
- [task-manager-api/docs/openapi.yaml](../task-manager-api/docs/openapi.yaml)
- [task-manager-api/docs/clean-architecture.md](../task-manager-api/docs/clean-architecture.md)

**Tasks:**
- [x] User entity + password hashing
- [x] Register and Login use cases
- [x] JWT generation and validation middleware
- [x] `POST /api/auth/register` and `POST /api/auth/login`
- [x] Unit tests: password validation, duplicate email (409)

**Done when:** `dotnet test` passes; endpoints match openapi.yaml.

---

## Phase 2 — Tasks CRUD Backend

**Repo:** `task-manager-api`

**Read:**
- [domain-model.md](domain-model.md)
- [memory/business-rules.md](../memory/business-rules.md)
- [task-manager-api/docs/openapi.yaml](../task-manager-api/docs/openapi.yaml)

**Tasks:**
- [x] Task entity + repository + use cases
- [x] Authorization: users only access their own tasks
- [x] `GET/POST /api/tasks`, `GET/PUT/DELETE /api/tasks/{id}`
- [x] Unit tests: user isolation, due date in past on create, required title

**Done when:** `dotnet test` passes; endpoints match openapi.yaml.

---

## Phase 3 — Auth Frontend

**Repo:** `task-manager-app`

**Read:**
- [task-manager-api/docs/openapi.yaml](../task-manager-api/docs/openapi.yaml)
- [task-manager-app/docs/frontend-architecture.md](../task-manager-app/docs/frontend-architecture.md)

**Tasks:**
- [ ] Login and Register components (Signal Forms)
- [ ] AuthService + JWT interceptor (Bearer header)
- [ ] Store token in sessionStorage
- [ ] Auth guard on protected routes

**Done when:** User can register, log in, and is redirected to `/tasks`.

---

## Phase 4 — Tasks Frontend

**Repo:** `task-manager-app`

**Read:**
- [domain-model.md](domain-model.md)
- [task-manager-api/docs/openapi.yaml](../task-manager-api/docs/openapi.yaml)
- [task-manager-app/docs/frontend-architecture.md](../task-manager-app/docs/frontend-architecture.md)

**Tasks:**
- [ ] Task list, create, edit, delete, status change
- [ ] TaskService calling API endpoints
- [ ] Signals for local state
- [ ] Handle 401/403 errors (redirect to login)

**Done when:** Full CRUD works against running API; `npm test` passes.

---

## Phase 5 — Polish

**Repo:** `both`

**Tasks:**
- [ ] CORS configuration in API for Angular dev server
- [ ] README in each repo with run instructions
- [ ] Basic responsive layout
- [ ] Optional: seed data for demo

**Done when:** End-to-end flow works; READMEs are accurate.
