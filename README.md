# Employee Management System (EMS)

.NET API solution for the Employee Management System (SQL Server, Docker-friendly local dev).

## Git branches

- **`development`** — default integration branch; feature PRs target this branch.
- **`pre-prod`** — UAT; promoted from `development`.
- **`prod`** — production-aligned; promoted from `pre-prod`.

Feature branches are created from **`development`** and removed after merge. See **[CONTRIBUTING.md](CONTRIBUTING.md)** for the full workflow and GitHub setup (default branch + branch protection).

## Local development

- **Docker**: `docker compose up -d` (SQL Server, MongoDB, Redis; see `docker-compose.yml`).
- **API**: run from `EMS.API` (`dotnet run`); health check: `GET /health`.

## Scripts

- **`scripts/Set-GitHubBranchPolicies.ps1`** — one-time GitHub API setup: default branch `development` + protection on `development`, `pre-prod`, `prod` (requires `GITHUB_TOKEN`). Details in [CONTRIBUTING.md](CONTRIBUTING.md).
