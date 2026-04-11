# Pukar.Usermanagement

Layered JWT authentication with refresh-token rotation, BCrypt passwords, and SQL Server persistence (schema `um`). Intended as a reusable module for multiple applications.

**Cursor / VS Code:** clone this repository, open the repo folder (or open `Pukar.Usermanagement.sln` directly). `.vscode/settings.json` sets `dotnet.defaultSolution` so the solution is picked up automatically.

## Projects

| Project | Role |
|---------|------|
| `Pukar.Shared` | Shared `StringHelper`, `BusinessRuleException`, `DuplicateEmailException`, `EmailNormalizer` (also referenced by EMS) |
| `Pukar.Usermanagement.Domain` | Entities, `UserManagementDbContext`, migrations, repository interfaces |
| `Pukar.Usermanagement.Application` | DTOs, `AuthService`, options |
| `Pukar.Usermanagement.Infrastructure` | EF repositories, JWT signing, BCrypt, `AddPukarUserManagement` |
| `Pukar.Usermanagement.API` | `AuthController`, `AddPukarUserManagementApi`, JWT bearer registration |
| `Pukar.Usermanagement.Host` | Runnable dev host with **Swagger UI** (`dotnet run`); uses database `PukarUsers` on the Docker SQL instance |

## Build

```bash
dotnet build Pukar.Usermanagement.sln
```

## Run locally (Swagger)

Start SQL Server (e.g. `docker compose up -d sqlserver`), then:

```bash
dotnet run --project Pukar.Usermanagement.Host --launch-profile http
```

Open **http://localhost:5246/swagger**. Log in via `POST /api/auth/login` (after seeding), click **Authorize**, enter `Bearer {accessToken}`, then call admin endpoints.

## Configuration (host app)

Connection string: `UserManagement` or fallback `DefaultConnection`. JWT section name: `Jwt`.
Point `UserManagement` at database **PukarUsers** on the Docker SQL instance (`localhost:1433`).

```json
"ConnectionStrings": {
  "UserManagement": "Server=localhost,1433;Database=PukarUsers;User Id=sa;Password=Dev123!@#Strong;TrustServerCertificate=True"
},
"Jwt": {
  "Issuer": "your-app",
  "Audience": "your-app",
  "SigningKey": "REPLACE_WITH_AT_LEAST_32_CHARACTERS!!",
  "AccessTokenExpirationMinutes": 15,
  "RefreshTokenExpirationDays": 7
},
"UserManagementBootstrapAdmin": {
  "EnableSeeding": true,
  "Email": "admin@ems.local",
  "Password": "Admin@123456",
  "UserName": "admin"
}
```

## Database

```bash
dotnet ef database update --project Pukar.Usermanagement.Domain --startup-project Pukar.Usermanagement.Infrastructure --context UserManagementDbContext
```

Set the same connection string for EF CLI to avoid targeting the wrong database:

```powershell
$env:UM_CONNECTION="Server=localhost,1433;Database=PukarUsers;User Id=sa;Password=Dev123!@#Strong;TrustServerCertificate=True"
```

You can also pass it directly with `--connection`:

```bash
dotnet ef database update --project Pukar.Usermanagement.Domain --startup-project Pukar.Usermanagement.Infrastructure --context UserManagementDbContext --connection "Server=...;Database=PukarUsers;..."
```

The **Host** project applies pending migrations on startup (`UserManagementBootstrapHostedService`), so a manual `database update` is optional if you run `Pukar.Usermanagement.Host` first.

## Roll back user-management schema from `EMSDev` (optional)

If you previously applied this module’s migrations to **`EMSDev`** and want **full isolation** in `PukarUsers` only, remove only the objects created by `UserManagementDbContext` migrations:

```bash
dotnet ef database update 0 --project Pukar.Usermanagement.Domain --startup-project Pukar.Usermanagement.Infrastructure --context UserManagementDbContext --connection "Server=localhost,1433;Database=EMSDev;User Id=sa;Password=Dev123!@#Strong;TrustServerCertificate=True"
```

This runs migration `Down` methods in reverse order (drops `um` tables managed by this module). Other tables in `EMSDev` are not affected.

**Caveats:** If migration history in `EMSDev` was edited manually or is inconsistent, fix it or use a manual SQL cleanup as a last resort (drop `um` tables and clear `um.__EFMigrationsHistory` rows for this context only).

**If `um.Users` (or related tables) still appear in `EMSDev`:** run the idempotent script [`scripts/RemovePukarUserManagementFromEMSDev.sql`](scripts/RemovePukarUserManagementFromEMSDev.sql) against `EMSDev` in SSMS or `sqlcmd`. It drops `um` tables in the correct order and removes this module’s rows from `dbo.__EFMigrationsHistory`. Your live data for user management belongs in **`PukarUsers`** only—ensure `ConnectionStrings:UserManagement` uses `Database=PukarUsers`, then start the Host or run `dotnet ef database update` against `PukarUsers`.

## Hosting integration

```csharp
builder.Services.AddControllers().AddPukarUserManagementControllers();
builder.Services.AddPukarUserManagementApi(builder.Configuration);

app.UseAuthentication();
app.UseAuthorization();
```

Point **`ConnectionStrings:UserManagement`** at **`Database=PukarUsers`** (same SQL instance as before) so EMS and other apps do not store user-management data in `EMSDev`.

## Repository layout (monorepo consumers)

If this code is consumed from another solution via **git submodule** or **ProjectReference**, see your host application’s documentation for publish/submodule steps.

## NuGet (optional)

Packaging metadata is in `Directory.Build.props`. To produce packages locally:

```bash
dotnet pack Pukar.Usermanagement.API/Pukar.Usermanagement.API.csproj -c Release -o ../../artifacts
```

Point consumers at a feed (GitHub Packages, Azure Artifacts, nuget.org) when you are ready to switch from `ProjectReference` to `PackageReference`.
