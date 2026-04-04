# Employee Management System (EMS)

Backend API for employee and organization directory data. The solution uses a layered structure: **Domain** (entities, EF Core context, migrations), **Application** (DTOs, services, mapping), **Infrastructure** (repository implementations), and **API** (HTTP endpoints, hosting).

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) reachable from your machine (LocalDB, Docker, or a named instance)
- Optional: MongoDB if you want Serilog’s MongoDB sink and the related health check (see connection strings below)

## Solution structure

| Project | Role |
|---------|------|
| `EMS.Domain` | EF Core `AppDbContext`, entity configurations, migrations, repository interfaces |
| `EMS.Application` | DTOs grouped by area under `DTOs/{Employee,Organization,Department,Location,JobPosition}/`, application services, entity ↔ DTO mapping |
| `EMS.Infrastructure` | `BaseRepository<T>` and other infrastructure implementations |
| `EMS.API` | ASP.NET Core host, controllers, DI wiring, Serilog, health checks |
| `ems-web` | Next.js (App Router) frontend — [quick start](ems-web/README.md), [full frontend guide](ems-web/docs/FRONTEND.md) |

More detail: [docs/architecture.md](docs/architecture.md).

## Docker Compose (SQL Server, MongoDB, Redis, Mongo Express, Next.js)

- **[docker-compose.yml](docker-compose.yml)** — infrastructure plus **`ems-web`** (production Next.js image on port **3000**).
- **[docker-compose.env.example](docker-compose.env.example)** — optional copy to **`.env`** beside `docker-compose.yml` to override `NEXT_PUBLIC_*` build args for the web image.
- **Batch (Windows):** double-click **[start-ems-docker.bat](start-ems-docker.bat)** to build and start containers; **[stop-ems-docker.bat](stop-ems-docker.bat)** runs `docker compose down` (containers removed; volumes kept).

**Typical flow:** start Docker Desktop → run `start-ems-docker.bat` → start **EMS.API** on the host (`dotnet run --project EMS.API --launch-profile http`) so the browser can reach the API at `http://localhost:5246` while the UI is at `http://localhost:3000`. Ensure [CORS](EMS.API/Program.cs) allows your web origin.

## Configuration

Connection strings and other settings live under `EMS.API` (`appsettings.json`, `appsettings.Development.json`).

| Key | Purpose |
|-----|---------|
| `ConnectionStrings:DefaultConnection` | SQL Server for EF Core (required) |
| `ConnectionStrings:MongoLogs` | Optional; enables Serilog MongoDB sink and MongoDB health check when set |

For local development, prefer [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) so credentials are not committed:

```bash
cd EMS.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_SQL_CONNECTION_STRING"
```

Replace placeholders in `appsettings*.json` with your own values for any environment you use.

## Database migrations

Migrations are in **EMS.Domain** (same assembly as `AppDbContext`). Apply them using the API project as startup (so configuration loads from `EMS.API`):

```bash
dotnet ef database update --project EMS.Domain --startup-project EMS.API
```

Add a new migration after model changes:

```bash
dotnet ef migrations add YourMigrationName --project EMS.Domain --startup-project EMS.API
```

Design-time: `AppDbContextFactory` in `EMS.Domain` resolves `DefaultConnection` from `EMS.API` appsettings when the EF tools run.

## Run the API

From the repository root:

```bash
dotnet run --project EMS.API
```

With the default launch profile, the app listens on HTTPS (see `EMS.API/Properties/launchSettings.json` for URLs).

## API documentation (OpenAPI)

In the **Development** environment, the OpenAPI document is exposed for tooling (e.g. import into Postman or an OpenAPI viewer):

- Document URL: `/openapi/v1.json` (default for ASP.NET Core 9 OpenAPI)

There is no Swagger UI in this template; you can add one later (e.g. Swashbuckle or Scalar) if you want interactive docs in the browser.

## HTTP endpoints (current)

REST-style CRUD under `api/{resource}`:

| Resource | Base route |
|----------|------------|
| Employees | `GET/POST /api/Employees`, `GET/PUT/DELETE /api/Employees/{id}` |
| Organizations | `GET/POST /api/Organizations`, `GET/PUT/DELETE /api/Organizations/{id}` |
| Departments | `GET/POST /api/Departments`, `GET/PUT/DELETE /api/Departments/{id}` |
| Locations | `GET/POST /api/Locations`, `GET/PUT/DELETE /api/Locations/{id}` |
| Job positions | `GET /api/JobPositions?organizationId={id}`, `GET/POST/PUT/DELETE /api/JobPositions/{id}` |

**Employees** may reference an optional **`jobPositionId`** (nullable) pointing at a row in **`org.JobPositions`**. Job positions are scoped per organization (`organizationId` on create; title and optional code are unique within the org). This replaces an older two-level Role/Job model so the name **JobPosition** stays distinct from application **user roles** (e.g. identity/authorization).

**Health:** `GET /health` (includes database; MongoDB when `MongoLogs` is configured).

## Building

From the repository root (same folder as `EmployeeManagementSystem.sln`):

```bash
dotnet build EmployeeManagementSystem.sln
```

Release configuration:

```bash
dotnet build EmployeeManagementSystem.sln -c Release
```

## Tests

Automated tests are not included yet. When you add a test project, document the `dotnet test` command here.
