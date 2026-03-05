# David Buckley — Software Developer Portfolio

A professional portfolio website built with ASP.NET Core Blazor (.NET 10), MudBlazor, SQL Server, and Entity Framework Core. Showcasing 30 years of software development experience.

## Solution Structure

```
Portfolio.slnx
└── src/
    ├── Portfolio.Shared/          # Shared DTOs & models
    ├── Portfolio.Api/             # REST Web API (.NET 10)
    └── Portfolio.Web/
        ├── Portfolio.Web/         # Blazor Server App
        └── Portfolio.Web.Client/  # Blazor WASM Client
```

## Featured Projects

- **BookIt** — Blazor booking management application
- **MAUI Cross-Platform App** — .NET MAUI mobile & desktop app
- **Curo** — Healthcare care management platform
- **Patient CRM** — Patient relationship management system *(In Development)*

## Tech Stack

- **Frontend**: ASP.NET Core Blazor (.NET 10) + MudBlazor 8 (light/dark mode)
- **Backend**: ASP.NET Core REST Web API (.NET 10)
- **Database**: SQL Server + Entity Framework Core 9
- **Auth**: ASP.NET Identity (cookie auth for web, JWT for API)
- **Architecture**: Clean separation — shared library, API project, Blazor app

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server (LocalDB, Express, or full)

### Configuration

Set the following via environment variables or [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) in production:

**Portfolio.Api:**
```
Jwt__Key=<your-secret-key-min-32-chars>
DefaultAdmin__Email=<admin-email>
DefaultAdmin__Password=<strong-password>
ConnectionStrings__DefaultConnection=<your-connection-string>
```

**Portfolio.Web:**
```
DefaultAdmin__Email=<admin-email>
DefaultAdmin__Password=<strong-password>
ConnectionStrings__DefaultConnection=<your-connection-string>
ApiBaseUrl=<url-of-portfolio-api>
```

### Running Locally

```bash
# Run the API
cd src/Portfolio.Api
dotnet run

# Run the Blazor app (in a separate terminal)
cd src/Portfolio.Web/Portfolio.Web
dotnet run
```

The default development credentials (set in `appsettings.Development.json`) are:
- Email: `admin@portfolio.com`
- Password: `Admin@123456!`

> **Important:** Change these credentials before deploying to production using environment variables or secrets management.

### Admin Area

Navigate to `/login` and sign in with your admin credentials to access the admin dashboard at `/admin`. From there you can create additional user accounts. There is no public registration page.

## Security

- No public user registration — admin creates accounts only
- Account lockout after 5 failed login attempts (15-minute lockout)
- JWT tokens for API authentication
- Cookie-based auth for Blazor web app with sliding expiration
- Anti-forgery protection enabled
- HSTS in production
- Sensitive config values use empty placeholders in committed `appsettings.json`
