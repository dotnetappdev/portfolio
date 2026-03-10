# Database Migrations

Both `Portfolio.Api` and `Portfolio.Web` use **Entity Framework Core** with code-first migrations. Migrations are applied automatically at startup, and seed data is applied after migration.

---

## Automatic startup behaviour

| Startup condition | What happens |
|---|---|
| Brand-new database | EF Core creates the database and applies all migrations |
| Existing database, pending migrations | EF Core applies the missing migrations |
| Database already up to date | Nothing - the app starts normally |

This is controlled by `Database.MigrateAsync()` called during startup whenever `"SeedData": true` is set in `appsettings.json` (the default).

---

## Prerequisites

Install the EF Core CLI tools (one-time, machine-wide):

```bash
dotnet tool install -g dotnet-ef
```

Verify:

```bash
dotnet ef --version
```

---

## Creating a new migration

Run these commands from the **repository root** whenever you change a model or `DbContext`.

### Portfolio.Api

```bash
cd src/Portfolio.Api
dotnet ef migrations add <MigrationName> --output-dir Data/Migrations
```

### Portfolio.Web

```bash
cd src/Portfolio.Web
dotnet ef migrations add <MigrationName> \
    --output-dir Data/Migrations \
    --project Portfolio.Web.csproj \
    --startup-project Portfolio.Web.csproj
```

Replace `<MigrationName>` with a descriptive PascalCase name, e.g. `AddBlogFeaturedImage`.

---

## Applying migrations manually

Migrations are applied automatically at startup. To apply them manually (e.g. in a CI pipeline or when seeding is disabled):

### Portfolio.Api

```bash
cd src/Portfolio.Api
dotnet ef database update
```

### Portfolio.Web

```bash
cd src/Portfolio.Web
dotnet ef database update \
    --project Portfolio.Web.csproj \
    --startup-project Portfolio.Web.csproj
```

---

## Rolling back a migration

Remove the last migration (only if it has **not** been applied to the database yet):

```bash
dotnet ef migrations remove
```

Roll back the database to a specific migration:

```bash
dotnet ef database update <TargetMigrationName>
```

Roll back the database completely (removes all tables):

```bash
dotnet ef database update 0
```

---

## Provider-specific notes

The `DatabaseProvider` setting in `appsettings.json` controls which EF Core provider is used. Migrations are provider-agnostic - the same migration files work with all supported providers.

| `DatabaseProvider` value | Provider | Notes |
|---|---|---|
| `Sqlite` (default) | SQLite | Zero-setup; DB file stored at `ConnectionStrings:DefaultConnection` path |
| `SqlServer` | SQL Server / Azure SQL | Requires a running SQL Server instance |
| `PostgreSql` or `Postgres` | PostgreSQL | Uses `Portfolio.Data.PostgreSql` (Npgsql) |
| `MySql` | MySQL / MariaDB | Uses `Portfolio.Data.MySql` (Pomelo) |
| `CosmosDb` or `Cosmos` | Azure Cosmos DB | Uses `Portfolio.Data.CosmosDb`; connection string format differs |

### Generating migrations for a specific provider

By default `dotnet ef migrations add` uses the provider configured in `appsettings.json`. To target a specific provider, temporarily set `DatabaseProvider` in `appsettings.Development.json`, run the migration command, then revert the setting.

### Azure Cosmos DB

Cosmos DB does not support relational migrations. If `DatabaseProvider` is set to `CosmosDb`, the app calls `EnsureCreatedAsync()` in addition to `MigrateAsync()` to create the Cosmos containers. No migration files are generated or needed for Cosmos.

---

## Seed data

### Portfolio.Api

Seed data (projects, skills, admin user, Admin role) is defined in `ApplicationDbContext.OnModelCreating` via `HasData()` and is included in the `InitialCreate` migration. It is applied automatically when the migration runs.

The admin account credentials are read from configuration:

| Key | Default |
|---|---|
| `DefaultAdmin:Email` | `admin@portfolio.com` |
| `DefaultAdmin:Password` | `Admin@123456!` |

Override these via environment variable, Docker env, or a `.env` file before first run.

### Portfolio.Web

The Web CMS database (blog posts, CMS pages, menus, app settings) is initialised via `MigrateAsync()` at startup. Initial CMS content can be added through the admin panel at `/admin` after the first login.

---

## Docker

When running via `docker compose up`, both containers apply migrations automatically on startup - no manual steps required. The databases persist in named Docker volumes (`api-data` and `web-data`).

To reset and re-seed from scratch:

```bash
docker compose down -v    # removes volumes (all data is lost)
docker compose up --build
```

See **[DOCKER-AZURE.md](./DOCKER-AZURE.md)** for the complete guide on publishing both services to Azure (Container Apps or App Service), including ACR image push, environment variable setup, and switching to Azure SQL or PostgreSQL.

---

## Quick reference

For a concise plain-text list of every EF Core CLI command used in this project, see **[eftooling.txt](./eftooling.txt)**.
