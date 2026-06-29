# Business Rules

## Users

- A user can register with email and password.
- A user can log in and receive a JWT.
- Passwords must never be stored as plain text.
- Authenticated users can access protected endpoints.
- Anonymous users can only access public endpoints.
- Email must be unique; duplicate registration returns **409 Conflict**.

## Tasks

- A task belongs to exactly one user.
- A user can only see their own tasks.
- A user can create, update, complete, and delete their own tasks.
- A task must have:
  - title
  - description
  - status
  - due date
- Title is required.
- Due date cannot be in the past when **creating** a task.
- On **update**, due date may remain in the past if the task already existed.
- Status `Completed` is terminal (cannot revert to Pending or InProgress).
- Status values:
  - Pending
  - InProgress
  - Completed
