# API Contract

Human-readable summary of the HTTP API. If there is a conflict, **openapi.yaml prevails**.

Base URL (development): `https://localhost:7xxx` (see `launchSettings.json`)

## Authentication

All task endpoints require `Authorization: Bearer <token>` header.

Public endpoints: `POST /api/auth/register`, `POST /api/auth/login`

## Endpoints

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| POST | `/api/auth/register` | No | Register a new user |
| POST | `/api/auth/login` | No | Login and receive JWT |
| GET | `/api/tasks` | Bearer | List current user's tasks |
| POST | `/api/tasks` | Bearer | Create a task |
| GET | `/api/tasks/{id}` | Bearer | Get task by id (own only) |
| PUT | `/api/tasks/{id}` | Bearer | Update task (own only) |
| DELETE | `/api/tasks/{id}` | Bearer | Delete task (own only) |

## HTTP status codes

| Code | When |
|------|------|
| 200 | Success (GET, PUT) |
| 201 | Created (POST register, POST task) |
| 204 | Deleted (DELETE) |
| 400 | Validation error (missing title, past due date on create) |
| 401 | Missing or invalid token |
| 403 | Task belongs to another user |
| 404 | Task or resource not found |
| 409 | Email already registered |

## Error format

Errors return `application/problem+json` (RFC 7807 ProblemDetails).

## Schemas

See [openapi.yaml](openapi.yaml) for request/response schemas:
- `RegisterRequest`, `LoginRequest`, `AuthResponse`
- `TaskDto`, `CreateTaskRequest`, `UpdateTaskRequest`
- `ProblemDetails`
