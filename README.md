# David Buckley — AI Developer & Security Engineer Portfolio

A professional portfolio website built with .NET 10, Blazor, MudBlazor, and Entity Framework Core. Positions David as a senior software engineer specialising in AI development and application security with 30 years of .NET experience.

## Screenshots

### Home Page
![Home Page](https://github.com/user-attachments/assets/38a7092c-c159-4027-a736-c51d6ad7c440)

### Blog
![Blog Listing](https://github.com/user-attachments/assets/813b52af-c1b1-445d-bf55-b01163ce1ea1)

### Blog Post
![Blog Post](https://github.com/user-attachments/assets/57d4c7e0-0103-4f8f-a567-fc1504b00e3c)

### Projects
![Projects](https://github.com/user-attachments/assets/d3d560ed-999f-46c3-9964-2dcd2941204c)

### Skills (with AI and Security)
![Skills](https://github.com/user-attachments/assets/4af6d3c5-d20a-4154-9749-c9455a9d03e8)

### About
![About](https://github.com/user-attachments/assets/7577c3fb-521e-48b2-9a51-888f7d4ca262)

### Contact
![Contact](https://github.com/user-attachments/assets/158c3d36-920b-4673-b336-fc2ef89cc783)

---

## Solution Structure

```
Portfolio.slnx
└── src/
    ├── Portfolio.Shared/              # Shared DTOs and models
    ├── Portfolio.Api/                 # REST Web API (.NET 10)
    │   └── Infrastructure/           # DatabaseProviderFactory
    ├── Portfolio.Sms.Abstractions/    # ISmsService, SmsMessage, SmsResult (no dependencies)
    ├── Portfolio.Sms.ClickSend/       # ClickSend REST API implementation
    ├── Portfolio.Sms.Twilio/          # Twilio REST API implementation
    └── Portfolio.Web/
        ├── Portfolio.Web/             # Blazor Server App
        │   ├── Components/Pages/      # Blazor pages (Home, Projects, Skills, About, Blog, Contact)
        │   ├── Components/Layout/     # MainLayout, NavMenu
        │   ├── Infrastructure/        # DatabaseProviderFactory
        │   └── Services/              # PortfolioApiService, BlogService, SmsSender
        └── Portfolio.Web.Client/      # Blazor WASM Client
```

## Features

- **AI Developer positioning** — hero section, skills, and projects lead with AI expertise
- **Security focus** — dedicated Security skills category, OWASP/OAuth2/JWT projects
- **Blog** — five human-written posts on AI, .NET, and security (no CMS required)
- **Light and dark mode** — respects system preference, toggleable in the header
- **REST API with fallback** — Blazor app works standalone when API is offline
- **Configurable database provider** — SQL Server, SQLite, or PostgreSQL via one setting
- **Admin area** — create accounts, manage hero stats, configure SMS provider
- **Account lockout** — 5 failed attempts triggers a 15-minute lockout
- **SMS notifications** — contact-form alerts sent to your number via Twilio or ClickSend (configured in admin)

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | ASP.NET Core Blazor (.NET 10) + MudBlazor 8 |
| Backend | ASP.NET Core REST Web API (.NET 10) |
| Database | SQL Server / SQLite / PostgreSQL + EF Core 9 |
| Auth (Web) | ASP.NET Identity with cookie auth |
| Auth (API) | JWT Bearer tokens |
| AI Skills | Semantic Kernel, Azure OpenAI, RAG, ML.NET |
| Security | OWASP, OAuth2/OIDC, Threat Modelling |
| SMS | Twilio / ClickSend (HTTP, no SDK) — provider-agnostic via `ISmsService` |

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- One of: SQL Server (LocalDB works), SQLite (zero config), or PostgreSQL

---

## Build

```bash
git clone <repo-url>
cd portfolio
dotnet build Portfolio.slnx
```

---

## Database Configuration

Set `DatabaseProvider` in `appsettings.json` (or override per environment):

| Value | Driver | Connection string format |
|---|---|---|
| `SqlServer` (default) | SQL Server / LocalDB | `Server=(localdb)\mssqllocaldb;Database=PortfolioDb;Trusted_Connection=True;` |
| `Sqlite` | SQLite | `Data Source=portfolio.db` |
| `PostgreSql` | PostgreSQL | `Host=localhost;Database=portfolio;Username=...;Password=...` |

> **PostgreSQL:** install the `Npgsql.EntityFrameworkCore.PostgreSQL` NuGet package in both
> `Portfolio.Api` and `Portfolio.Web`, then uncomment the `UseNpgsql` line in each project's
> `Infrastructure/DatabaseProviderFactory.cs`.

The database schema is created automatically on first run via `EnsureCreatedAsync`.

---

## Running Locally

### Quick start with SQLite (zero setup)

The development appsettings already configure SQLite so you can run immediately:

```bash
# Terminal 1 — API
cd src/Portfolio.Api
dotnet run

# Terminal 2 — Blazor web app
cd src/Portfolio.Web/Portfolio.Web
dotnet run
```

Open `http://localhost:5100` in your browser.

### Using SQL Server

Update `appsettings.Development.json` in both projects:

```json
{
  "DatabaseProvider": "SqlServer",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PortfolioDb;Trusted_Connection=True;"
  }
}
```

---

## Configuration Reference

### Portfolio.Api — `appsettings.json`

| Key | Description | Example |
|---|---|---|
| `DatabaseProvider` | Database driver | `SqlServer`, `Sqlite`, `PostgreSql` |
| `ConnectionStrings:DefaultConnection` | Database connection | See above |
| `Jwt:Key` | JWT signing key (min 32 chars) | Set via secret or env var |
| `Jwt:Issuer` | JWT issuer claim | `Portfolio.Api` |
| `Jwt:Audience` | JWT audience claim | `Portfolio.Web` |
| `DefaultAdmin:Email` | Seeded admin email | Set via secret or env var |
| `DefaultAdmin:Password` | Seeded admin password | Set via secret or env var |
| `AllowedOrigins` | CORS allowed origins | `https://yourdomain.com` |

### Portfolio.Web — `appsettings.json`

| Key | Description | Example |
|---|---|---|
| `DatabaseProvider` | Database driver | `SqlServer`, `Sqlite`, `PostgreSql` |
| `ConnectionStrings:DefaultConnection` | Database connection | See above |
| `ApiBaseUrl` | URL of Portfolio.Api | `https://localhost:7002/` |
| `DefaultAdmin:Email` | Seeded admin email | Set via secret or env var |
| `DefaultAdmin:Password` | Seeded admin password | Set via secret or env var |

---

## Secrets Management

Never commit real credentials. Use [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) for local development:

```bash
# API secrets
cd src/Portfolio.Api
dotnet user-secrets set "Jwt:Key" "your-secret-key-minimum-32-characters"
dotnet user-secrets set "DefaultAdmin:Email" "admin@yourdomain.com"
dotnet user-secrets set "DefaultAdmin:Password" "YourStr0ng!Password"

# Web secrets
cd src/Portfolio.Web/Portfolio.Web
dotnet user-secrets set "DefaultAdmin:Email" "admin@yourdomain.com"
dotnet user-secrets set "DefaultAdmin:Password" "YourStr0ng!Password"
```

In production use environment variables or a secrets manager (Azure Key Vault, AWS Secrets Manager, etc.):

```bash
export Jwt__Key="your-production-secret"
export DefaultAdmin__Email="admin@yourdomain.com"
export DefaultAdmin__Password="YourStr0ng!Password"
```

The development defaults (in `appsettings.Development.json`) are:

| Setting | Value |
|---|---|
| Admin email | `admin@portfolio.com` |
| Admin password | `Admin@123456!` |

> Change these before going live.

---

## Admin Area

Navigate to `/login` and sign in to access `/admin`. From the admin dashboard you can:

- **Hero Stats** — add, edit or delete the stat cards shown in the hero section
- **Users** — create new user accounts and view existing ones
- **SMS Settings** — configure Twilio or ClickSend and set your receiver number

There is no public registration page by design.

---

## SMS Notifications

Contact-form submissions trigger an SMS alert to the admin receiver number you set in the admin dashboard. No app restart needed — changes take effect immediately.

### Architecture

Three small, focused class libraries handle SMS:

| Library | Role |
|---|---|
| `Portfolio.Sms.Abstractions` | `ISmsService`, `SmsMessage`, `SmsResult` — no external deps |
| `Portfolio.Sms.Twilio` | Sends via Twilio REST API (Basic Auth, no SDK required) |
| `Portfolio.Sms.ClickSend` | Sends via ClickSend REST API v3 (Basic Auth, no SDK required) |

`SmsSender` (in `Portfolio.Web`) reads the active provider settings from the database on every call and delegates to the correct library.

### Twilio Setup

1. Create a free account at [twilio.com](https://www.twilio.com)
2. From the Console dashboard copy your **Account SID** and **Auth Token**
3. Add a verified phone number as the **From number** (E.164, e.g. `+447911123456`)
4. In Admin → SMS Settings, set **Provider: Twilio**, fill in the credentials, and enter your **Admin receiver number**
5. Click **Send Test SMS** to verify

### ClickSend Setup

1. Create an account at [clicksend.com](https://www.clicksend.com)
2. Go to **Account → API Credentials** and generate an API key
3. Your login email is the **username**
4. In Admin → SMS Settings, set **Provider: ClickSend**, fill in the credentials
5. The **Sender ID** can be up to 11 alphanumeric characters or a phone number
6. Click **Send Test SMS** to verify

### Reusing the SMS libraries in other projects

```csharp
// Static provider (Twilio)
services.AddTwilioSms(o =>
{
    o.AccountSid = "ACxxxxxxxx";
    o.AuthToken  = "your-auth-token";
    o.From       = "+447911000000";
});

// Static provider (ClickSend)
services.AddClickSendSms(o =>
{
    o.Username = "you@example.com";
    o.ApiKey   = "your-api-key";
    o.From     = "Portfolio";
});

// Then inject ISmsService wherever needed
public class MyService(ISmsService sms)
{
    public Task AlertAsync(string phone) =>
        sms.SendAsync(new SmsMessage(phone, "Hello from Portfolio!"));
}
```

---

## Blog

The blog lives at `/blog`. Posts are authored in `BlogService.cs` as plain C# strings, so no database or CMS is needed. Current posts:

- **Building AI into .NET Without Losing Your Mind** — production lessons from Semantic Kernel and Azure OpenAI
- **The OWASP Top Ten Is Not a Checklist. It Is a Story.** — how to actually use OWASP in .NET
- **What Thirty Years of C# Taught Me About Code That Lasts** — personal reflection on writing durable code
- **JWT Tokens Are Not Magic and That Matters** — authentication pitfalls in ASP.NET Core
- **When AI Caught a Bug My Tests Missed** — real story from a healthcare AI project

To add a post, append a new `BlogPost` record to the `_posts` array in `src/Portfolio.Web/Portfolio.Web/Services/BlogService.cs`.

---

## Security Notes

- No public registration; admin creates accounts only
- Account lockout after 5 failed login attempts (15-minute lockout)
- Cookie auth for Blazor with 8-hour sliding expiration
- JWT for API with issuer and audience validation
- HTTPS enforced in non-development environments
- HSTS enabled in production
- Sensitive config values are empty in committed `appsettings.json`; supply via secrets or environment variables

