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
| `EMS.Application` | DTOs grouped by area under `DTOs/{Employee,Organization,Department,Location}/`, application services, entity ↔ DTO mapping |
| `EMS.Infrastructure` | `BaseRepository<T>` and other infrastructure implementations |
| `EMS.API` | ASP.NET Core host, controllers, DI wiring, Serilog, health checks |

More detail: [docs/architecture.md](docs/architecture.md).

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

**Health:** `GET /health` (includes database; MongoDB when `MongoLogs` is configured).

## Building

```bash
dotnet build EmployeeManagementSystem.sln
```

## Tests

Automated tests are not included yet. When you add a test project, document the `dotnet test` command here.
