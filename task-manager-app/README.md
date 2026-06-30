# Task Manager App



Angular SPA for the Task Manager monorepo. Users register, log in with JWT, and manage their own tasks.



## Prerequisites



- [Node.js](https://nodejs.org/) (LTS)

- Task Manager API running locally (see [../task-manager-api/README.md](../task-manager-api/README.md))



## Install



```bash

npm install

```



## Run locally



1. Start the API (from `task-manager-api`):



   ```bash

   dotnet run --project src/TaskManager.Api --launch-profile http

   ```



   On first run, the API seeds a demo user and sample tasks (see [API demo data](../task-manager-api/README.md#demo-data-seeding)).



2. Start the dev server:



   ```bash

   npm start

   ```



   Or: `ng serve`



3. Open http://localhost:4200



The app calls the API at `http://localhost:5221` (see `src/environments/environment.development.ts`).



### Quick start with demo account



Use these credentials after a fresh API start (empty database):



| Field | Value |

|-------|-------|

| Email | `demo@example.com` |

| Password | `Demo123!` |



You should see 3 preloaded tasks on `/tasks`.



## Tests



```bash

npm test

```



## E2E tests (Playwright)



Requires the [.NET SDK](https://dotnet.microsoft.com/download) so Playwright can start the API automatically.



```bash

# Install browsers (first time only)

npx playwright install



# Run E2E (starts API on :5221 and Angular on :4200)

npm run e2e

```



Other commands:



```bash

npm run e2e:ui      # Playwright UI mode

npm run e2e:debug   # Debug mode

```



Each test registers a unique user via the UI, so you do not need to reset the database between runs.



## Build



```bash

ng build

```



## Documentation



- [Frontend architecture](docs/frontend-architecture.md)

- [Domain model](../docs/domain-model.md)

- [API contract](../task-manager-api/docs/openapi.yaml)



## Routes



| Path | Description |

|------|-------------|

| `/login` | Log in |

| `/register` | Create account |

| `/tasks` | Task list and CRUD (protected) |


