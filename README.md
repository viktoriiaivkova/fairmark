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

_Coming soon — local setup is added together with the solution skeleton (FM-22)._

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
