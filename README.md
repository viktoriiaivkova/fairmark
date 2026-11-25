# Fairmark

Fairmark is a web app for student team projects that makes each member's
individual contribution transparent and fairly assessable — combining objective
activity data with calibrated peer review into an explainable contribution index.

> 🚧 Work in progress. MVP (v1.0) is planned for February 2027.

## Key features (MVP)

- Teams with roles: student, team lead, teacher, admin
- Kanban board, tasks, sprints and complexity estimates
- Time tracking and an immutable activity log
- Peer review by criteria and teacher rubrics
- Contribution index with a per-member breakdown

## Tech stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core (.NET 10), EF Core, PostgreSQL |
| Frontend | React, TypeScript, Vite |
| Testing | xUnit, Testcontainers, Vitest, Playwright |
| Infrastructure | Docker, GitHub Actions |

## Repository structure

```
backend/    ASP.NET Core API and tests
frontend/   React + TypeScript client
e2e/        Playwright end-to-end tests
```

## Getting started

### Prerequisites

- [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0) — the exact version is pinned in `global.json`
- [Node.js 22](https://nodejs.org/) (22.22.2 or later, see `frontend/.nvmrc`)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) — the integration tests start PostgreSQL in a container

The local database for running the API (Docker Compose) is added in FM-45.

### Backend (`backend/`)

```bash
cd backend
dotnet build                               # build all projects
dotnet test                                # unit + integration tests (Docker must be running)
dotnet run --project src/Fairmark.Api      # API on http://localhost:5188
```

- Health check: `GET http://localhost:5188/health`
- OpenAPI document (Development only): `http://localhost:5188/openapi/v1.json`
- Sample requests for the VS Code REST Client: `src/Fairmark.Api/Fairmark.Api.http`

### Frontend (`frontend/`)

```bash
cd frontend
npm ci                 # install the exact versions from package-lock.json
npm run dev            # dev server on http://localhost:5173
npm run lint           # ESLint + Prettier check
npm run format         # format all files with Prettier
npm run typecheck      # TypeScript
npm test               # Vitest (npm run test:watch for watch mode)
npm run build          # production build into dist/
```

The dev server forwards every request under `/api` to the API on `http://localhost:5188`, so the
web app and the API share one origin. To work on both, run the API and the dev server in two terminals.

## How this project is run

The project follows a team-style Scrum process, even though it has a single
contributor:

- **Jira** — backlog, two-week sprints, releases and bug tracking
- **Confluence** — requirements, architecture decisions (ADRs), test strategy, sprint retrospectives
- **AIO Tests** — test cases linked to user stories, test cycles per story and release
- **GitHub** — every change goes through a pull request linked to a Jira issue

Jira and Confluence are a private workspace; access is available on request:
[Jira board](https://knowflow-dev.atlassian.net/jira/software/c/projects/FM/boards/2) ·
[Confluence space](https://knowflow-dev.atlassian.net/wiki/spaces/FM/overview)

### Conventions

- Branches: `feature/FM-12-short-description`, `fix/FM-57-short-description`
- Commits: [Conventional Commits](https://www.conventionalcommits.org/) with the issue key, e.g. `feat(auth): FM-12 add JWT login`
- `main` is protected: changes land only through pull requests with passing checks

## Author

Viktoriia Ivkova — bachelor's thesis project
