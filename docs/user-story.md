# User Story

As a registered user, I want to manage my personal tasks so that I can organize my work and keep track of pending activities.

## Acceptance Criteria

- Users can register.
- Users can log in.
- Authenticated users can create tasks.
- Authenticated users can view only their own tasks.
- Authenticated users can update their own tasks.
- Authenticated users can delete their own tasks.
- Anonymous users cannot access protected task endpoints.

## Verification map

| Criterion | Backend | Frontend |
|-----------|---------|----------|
| Register | `POST /api/auth/register` | `/register` |
| Log in | `POST /api/auth/login` → JWT | `/login` |
| Create tasks | `POST /api/tasks` (Bearer) | Create form in `/tasks` |
| View own tasks only | Filter by UserId; 403 if not owner | List from API (no client-side filter needed) |
| Update own tasks | `PUT /api/tasks/{id}` (Bearer) | Edit form in `/tasks` |
| Delete own tasks | `DELETE /api/tasks/{id}` (Bearer) | Delete action in `/tasks` |
| Anonymous blocked | 401 on protected endpoints | Auth guard redirects to `/login` |
