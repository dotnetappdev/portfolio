# David Buckley: AI Developer and Security Engineer Portfolio

A professional portfolio website built with .NET 10, Blazor, MudBlazor, and Entity Framework Core. Positions David as a senior software engineer specialising in AI development and application security with 30 years of .NET experience.

## Screenshots

### Home Page
![Home Page](https://github.com/user-attachments/assets/066d11be-7bb1-4fd8-8846-f4db81449dd8)

### Projects (with improved descriptions and tech chips)
![Projects](https://github.com/user-attachments/assets/6c3d546d-9aae-473c-9d43-f1291d2a507f)

### Blog listing (8 posts, paginated — 5 per page — each with a themed featured image)
![Blog Listing](https://github.com/user-attachments/assets/ad473d52-f82a-4b8d-8daf-f113958f2045)

### Blog Post: BookIt (product SVG mockup + newspaper-style H2 sections)
![Blog Post BookIt](https://github.com/user-attachments/assets/05454c04-bdde-4ff4-b670-b37d85236e6b)

### Blog Post: AI in .NET (AI chat mockup featured image)
![Blog Post AI](https://github.com/user-attachments/assets/e5cbaba8-2f22-47bd-99ea-6603dc2844cb)

### Blog Post: OWASP (security dashboard featured image)
![Blog Post OWASP](https://github.com/user-attachments/assets/5aa434c2-a009-4e00-9752-e5c17bd3d8bb)

### Blog Post: Three Decades (code editor featured image)
![Blog Post .NET](https://github.com/user-attachments/assets/e07ad50b-a977-4e0b-9800-aa7d65e70273)

### Skills (with AI and Security categories)
![Skills](https://github.com/user-attachments/assets/81c0b73d-1544-4bed-83dc-e37934382beb)

### About
![About](https://github.com/user-attachments/assets/fdfc3213-dd66-4399-997e-f507245af7b2)

### Contact (with math CAPTCHA)
![Contact](https://github.com/user-attachments/assets/bed68ebf-d13e-42da-9c6e-d0a899062e9b)

---

## CMS Screenshots

### Admin Dashboard: Hero Stats
![Admin Dashboard](https://github.com/user-attachments/assets/8475c135-b188-4736-9978-d0ae288fb129)

### Admin: SMS Provider Settings (Twilio / ClickSend credentials stored in the database, managed entirely in-app)

The Settings tab shows a two-column layout: on the left, the SMS Provider form with an Enable toggle, provider selector, Admin receiver number, and provider-specific credential fields (Account SID, Auth Token, From number for Twilio; Username, API Key, Sender ID for ClickSend). On the right, a concise Setup Guide card with links to each provider's console. A **Save SMS Settings** button and a **Send Test SMS** button sit at the bottom of the form.

### Admin: Static Site Generator
![Admin Static Site](https://github.com/user-attachments/assets/c781f737-e6be-41d3-b93c-7b42823e240e)

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
        │   ├── Components/
        │   │   ├── Layout/            # MainLayout (DB-driven nav), NavMenu
        │   │   ├── Pages/
        │   │   │   ├── Admin/         # Admin dashboard (Hero Stats, Users, Settings, Blog Posts, Pages, Menus)
        │   │   │   ├── Blog/          # Blog index + post view (SEO, OG tags, featured images)
        │   │   │   └── CmsPageView/   # Catch-all /{**slug} for custom CMS pages
        │   │   └── Shared/            # RichTextEditor (Quill WYSIWYG wrapper)
        │   ├── Data/                  # ApplicationDbContext, BlogPost, CmsPage, MenuItem, AppSettings, SmsSettings
        │   ├── Infrastructure/        # DatabaseProviderFactory
        │   └── Services/              # BlogService, CmsPageService, MenuService, AppSettingsService,
        │                              #   PortfolioApiService, SmsSender, StaticSiteGeneratorService
        └── Portfolio.Web.Client/      # Blazor WASM Client
```

## Features

- **AI Developer positioning**: hero section, skills, and projects lead with AI expertise
- **Security focus**: dedicated Security skills category, OWASP/OAuth2/JWT projects
- **Work Project showcase**: dedicated "Work Project" category for BookIt, Curo, and TalentConnect with SVG app-mockup images and tech chip badges
- **WordPress-style CMS**: create, edit, publish and delete blog posts and custom pages entirely from the admin dashboard with no code deploy required
- **WYSIWYG editor**: Quill rich-text editor (served locally, no CDN) for blog posts and pages; supports headings, bold/italic/lists/links/code blocks and more
- **DB-driven navigation**: add, reorder, hide or delete menu items live from the Menus admin tab
- **Custom CMS pages**: publish arbitrary pages at any slug (e.g. `/services`, `/hire-me`) with full SEO metadata
- **SEO and Open Graph**: per-post/page meta title, meta description, OG image and canonical URL injected via `<HeadContent>`
- **Featured images**: optional hero banner image on blog posts and card thumbnail on the blog listing; SVG app mockups on work project posts
- **Tech chip badges**: technology tags displayed as chips on project cards and blog posts
- **Contact form CAPTCHA**: server-side math challenge blocks spam without any external service or API key
- **Static site generator**: export a complete dark-mode static HTML snapshot of the portfolio as a deployable ZIP from the admin panel
- **Light and dark mode**: respects system preference, toggleable in the header
- **REST API with fallback**: Blazor app works standalone when API is offline
- **Configurable database provider**: SQL Server, SQLite, or PostgreSQL via one setting
- **Admin area**: create accounts, manage hero stats, configure API/SMS settings, manage blog posts, pages, menus, and generate static exports
- **In-app settings**: API base URL and SMS provider (with all API keys/tokens) configured through the admin Settings tab — stored in the database, no environment variables or app restart needed
- **Paginated blog listing**: public blog page shows 5 posts per page; admin blog table shows 10 rows per page (options: 5 / 10 / 25)
- **Account lockout**: 5 failed attempts triggers a 15-minute lockout
- **SMS notifications**: contact-form alerts sent to your number via Twilio or ClickSend — all credentials (Account SID, Auth Token, API Key, etc.) stored in DB and managed in Admin → Settings

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
| SMS | Twilio / ClickSend (HTTP, no SDK), provider-agnostic via `ISmsService` |

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- One of: SQL Server (LocalDB works), SQLite (zero config), or PostgreSQL

---

## Docker

Both the Web API and the Blazor site have Dockerfiles. A `docker-compose.yml` at the repo root orchestrates both containers together using SQLite for zero-setup persistence.

### Quick start with Docker Compose

1. **Set the required JWT key** (minimum 32 characters). Copy the example env file and fill in your values:

   ```bash
   cp .env.example .env
   # Edit .env and set JWT_KEY to a strong random string (32+ characters)
   ```

   Or export the variable directly in your shell:

   ```bash
   export JWT_KEY="your-secret-key-minimum-32-characters-long"
   ```

2. **Build and start both containers:**

   ```bash
   docker compose up --build -d
   ```

3. **Access the apps:**
   - Blazor Web App: `http://localhost:5072`
   - REST API: `http://localhost:5008`

4. **Stop the containers:**

   ```bash
   docker compose down
   ```

   Add `-v` to also remove the database volumes: `docker compose down -v`

### Environment variables

The following variables can be set in a `.env` file at the repo root or exported in your shell before running `docker compose up`:

| Variable | Required | Default | Description |
|---|---|---|---|
| `JWT_KEY` | **Yes** | _(none)_ | JWT signing key — minimum 32 characters |
| `ADMIN_EMAIL` | No | `admin@portfolio.com` | Seeded admin account email |
| `ADMIN_PASSWORD` | No | `Admin@123456!` | Seeded admin account password |
| `ALLOWED_ORIGINS` | No | `http://localhost:5072` | CORS allowed origin for the API |

> **Security:** change `ADMIN_EMAIL`, `ADMIN_PASSWORD`, and `JWT_KEY` before exposing containers publicly.

### Building images individually

```bash
# API image only (build context is the src/ directory)
docker build -t portfolio-api -f src/Portfolio.Api/Dockerfile ./src

# Blazor Web image only
docker build -t portfolio-web -f src/Portfolio.Web/Portfolio.Web/Dockerfile ./src
```

### Persistent data

Each container stores its SQLite database in a named Docker volume (`api-data` and `web-data`). Data survives container restarts. To inspect or back up the volumes:

```bash
# List volumes
docker volume ls

# Copy a database file out of a volume
docker cp portfolio-api:/app/data/portfolio-api.db ./backup-api.db
docker cp portfolio-web:/app/data/portfolio-web.db ./backup-web.db
```

### Connecting Web to API inside Docker

After starting the containers, sign in to the Blazor admin panel (`http://localhost:5072/login`) and navigate to **Settings**. Set the **Portfolio API Base URL** to `http://portfolio-api:8080` so the web container can reach the API over the internal Docker network.

### Changing the database provider in Docker

The default `docker-compose.yml` uses SQLite for zero-setup persistence. To switch to **SQL Server** or **PostgreSQL**, update the environment variables for each service in `docker-compose.yml`.

#### SQL Server

Add a SQL Server service and update both app services:

```yaml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=${SA_PASSWORD}
    ports:
      - "1433:1433"
    volumes:
      - sqlserver-data:/var/opt/mssql

  portfolio-api:
    # ... existing config ...
    environment:
      - DatabaseProvider=SqlServer
      - ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PortfolioApiDb;User Id=sa;Password=${SA_PASSWORD};TrustServerCertificate=True
      # ... other env vars ...
    depends_on:
      - sqlserver

  portfolio-web:
    # ... existing config ...
    environment:
      - DatabaseProvider=SqlServer
      - ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=PortfolioWebDb;User Id=sa;Password=${SA_PASSWORD};TrustServerCertificate=True
      # ... other env vars ...
    depends_on:
      - sqlserver

volumes:
  sqlserver-data:
```

Add `SA_PASSWORD` to your `.env` file (must meet SQL Server complexity requirements).

#### PostgreSQL

> **Note:** PostgreSQL requires installing `Npgsql.EntityFrameworkCore.PostgreSQL` in both `Portfolio.Api` and `Portfolio.Web` and uncommenting `UseNpgsql` in each project's `Infrastructure/DatabaseProviderFactory.cs`. See [Database Configuration](#database-configuration) for full instructions.

Once the packages are installed, add a PostgreSQL service and update the env vars:

```yaml
services:
  postgres:
    image: postgres:16
    environment:
      - POSTGRES_USER=portfolio
      - POSTGRES_PASSWORD=${POSTGRES_PASSWORD}
      - POSTGRES_DB=portfolio
    volumes:
      - postgres-data:/var/lib/postgresql/data

  portfolio-api:
    # ... existing config ...
    environment:
      - DatabaseProvider=PostgreSql
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=portfolio_api;Username=portfolio;Password=${POSTGRES_PASSWORD}
      # ... other env vars ...

  portfolio-web:
    # ... existing config ...
    environment:
      - DatabaseProvider=PostgreSql
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=portfolio_web;Username=portfolio;Password=${POSTGRES_PASSWORD}
      # ... other env vars ...

volumes:
  postgres-data:
```

### Notification (SMS) settings in Docker

The contact form sends an SMS alert to your phone when a visitor submits a message. These settings are **not** environment variables — they are stored in the database and managed entirely through the admin panel. No container restart is needed after changing them.

To configure SMS notifications after the containers are running:

1. Sign in at `http://localhost:5072/login`
2. Go to **Admin → Settings → SMS Provider**
3. Choose a provider (**Twilio** or **ClickSend**), fill in your API credentials, and enter the **Admin receiver number** (E.164 format, e.g. `+447911123456`)
4. Click **Save SMS Settings**
5. Click **Send Test SMS** to verify delivery

See the [SMS Notifications → Admin credentials fields](#admin--settings-sms-provider-fields) table for the full list of fields and where to obtain each value for Twilio and ClickSend.

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
# Terminal 1: API
cd src/Portfolio.Api
dotnet run

# Terminal 2: Blazor web app
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

### Portfolio.Api: `appsettings.json`

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

### Portfolio.Web: `appsettings.json`

| Key | Description | Example |
|---|---|---|
| `DatabaseProvider` | Database driver | `SqlServer`, `Sqlite`, `PostgreSql` |
| `ConnectionStrings:DefaultConnection` | Database connection | See above |
| `DefaultAdmin:Email` | Seeded admin email | Set via secret or env var |
| `DefaultAdmin:Password` | Seeded admin password | Set via secret or env var |

> **API base URL** is now configured in the admin **Settings** tab (stored in the database), no longer an `appsettings.json` key.

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

Navigate to `/login` and sign in to access `/admin`. The admin dashboard is organised into seven tabs:

| Tab | What you can do |
|---|---|
| **Hero Stats** | Add, edit or delete the stat cards shown in the hero section |
| **Users** | Create new user accounts and view existing ones |
| **Settings** | Configure the Portfolio API base URL; set up Twilio or ClickSend SMS |
| **Blog Posts** | Create, edit, publish/unpublish and delete blog posts using the Quill WYSIWYG editor; manage slug, excerpt, tags, read time, featured image and SEO metadata |
| **Pages** | Create custom CMS pages at any slug (e.g. `/services`); same editor and SEO fields as blog posts |
| **Menus** | Add, edit, reorder, show/hide and delete navigation menu items; changes appear immediately in the nav bar |
| **Static Site** | Generate a complete dark-mode static HTML snapshot of the portfolio and download it as a deployable ZIP |

There is no public registration page by design.

### Blog Posts: WordPress-style editor

The Blog Posts tab works like WordPress's post editor:

- **List view**: shows all posts with title, slug, category, publish date, status chip (Published / Draft) and quick-action buttons (Edit, Publish/Unpublish, Delete); paginated (10 rows per page, options: 5 / 10 / 25)
- **Status filters**: chip buttons to filter All / Published / Drafts
- **Editor view**: left column: large title field, permalink slug, Quill WYSIWYG body, excerpt; right sidebar: Publish card (status, toggle, date, Save button), Post Settings (category, tags, read time), Featured Image (URL + live preview), SEO and Social (meta title, meta description, OG image, canonical URL, expandable panel)
- **Back breadcrumb**: `← Posts` returns to the list without losing context

### Custom Pages

The Pages tab works identically to Blog Posts but creates stand-alone pages accessible at any custom slug. Published pages are rendered by the catch-all `/{**slug}` route and include full SEO head injection.

### Navigation Menus

The Menus tab lists all current nav items (label, URL, sort order, visibility). Changes, including adding new items or toggling visibility, are reflected live in the navigation bar without a page reload or restart.

---

## SMS Notifications

Contact-form submissions trigger an SMS alert to the admin receiver number you set in the admin dashboard. **All SMS credentials (API keys, tokens, phone numbers) are stored in the database and managed entirely through Admin → Settings — no config files, environment variables, or app restart required.**

### Admin → Settings: SMS Provider fields

Navigate to `/admin` → **Settings** tab → **SMS Provider** card. All fields are stored encrypted in the database and take effect immediately on Save.

#### Common fields (both providers)

| Field | Description |
|---|---|
| **Enable SMS sending** | Toggle to turn notifications on or off without losing your credentials |
| **Provider** | Select **Twilio**, **ClickSend**, or **None** |
| **Admin receiver number** | Your phone number in E.164 format (e.g. `+447911123456`) — contact-form alerts go here |

#### Twilio credential fields

| Field | Where to find it |
|---|---|
| **Account SID** | [Twilio Console](https://console.twilio.com) dashboard, starts with `AC` |
| **Auth Token** | Twilio Console dashboard (click to reveal) |
| **From number** | A verified Twilio phone number or Messaging Service SID (E.164, e.g. `+14155552671`) |

#### ClickSend credential fields

| Field | Where to find it |
|---|---|
| **Username** | Your ClickSend login email address |
| **API Key** | [ClickSend dashboard](https://dashboard.clicksend.com) → Account → API Credentials → Generate Key |
| **Sender ID** | Optional — up to 11 alphanumeric characters shown as the sender name (e.g. `Portfolio`); leave blank to use your account number |

### Architecture

Three small, focused class libraries handle SMS:

| Library | Role |
|---|---|
| `Portfolio.Sms.Abstractions` | `ISmsService`, `SmsMessage`, `SmsResult` (no external deps) |
| `Portfolio.Sms.Twilio` | Sends via Twilio REST API (Basic Auth, no SDK required) |
| `Portfolio.Sms.ClickSend` | Sends via ClickSend REST API v3 (Basic Auth, no SDK required) |

`SmsSender` (in `Portfolio.Web`) reads the active provider settings from the database on every call and delegates to the correct library.

### Twilio Setup

1. Create a free account at [twilio.com](https://www.twilio.com)
2. From the Console dashboard copy your **Account SID** and **Auth Token**
3. Add a verified phone number as the **From number** (E.164, e.g. `+447911123456`)
4. In Admin → **Settings** → SMS Provider, set **Provider: Twilio**, fill in the credentials, and enter your **Admin receiver number**
5. Click **Save SMS Settings**, then **Send Test SMS** to verify

### ClickSend Setup

1. Create an account at [clicksend.com](https://www.clicksend.com)
2. Go to **Account → API Credentials** and generate an API key
3. Your login email is the **username**
4. In Admin → **Settings** → SMS Provider, set **Provider: ClickSend**, fill in the credentials
5. The **Sender ID** can be up to 11 alphanumeric characters or a phone number
6. Click **Save SMS Settings**, then **Send Test SMS** to verify

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

## Blog & CMS

The blog lives at `/blog`. Posts are stored in the database and managed entirely through the admin **Blog Posts** tab, no code changes or deployments needed.

### Creating a post

1. Go to `/admin` → **Blog Posts** → **Add New Post**
2. Enter a title (the slug is auto-generated but editable)
3. Write the body using the Quill WYSIWYG editor
4. Fill in the excerpt and any post settings (category, tags, read time)
5. Optionally add a featured image URL and SEO/OG metadata in the right sidebar
6. Click **Publish** to make it live, or **Save Draft** to keep it hidden

### Post features

- **Slug**: fully editable permalink (e.g. `/blog/my-post-title`)
- **Featured image**: displayed as a full-width hero banner on the post page and as a card thumbnail on the blog listing
- **SEO**: per-post `<title>`, `<meta name="description">`, `og:title`, `og:description`, `og:image`, and `<link rel="canonical">` injected automatically
- **Status**: toggle between Published and Draft at any time without deleting

### Seeded posts

The database is seeded with eight posts on first run. Each post has a themed SVG featured image, newspaper-style HTML body with H2 section headings and key terms in bold, and a punchy summary:

- **Building TalentConnect: A Modern Blazor Recruitment Platform**: building a full-stack recruitment platform with job pipelines and analytics (Projects category)
- **Building Curo: A Healthcare Care Management Platform**: Blazor-based care management deployed to Azure with strict compliance (Projects category)
- **Building BookIt: A Blazor Booking Management System**: real-time booking system with SMS notifications and dark/light mode (Projects category)
- **Building AI into .NET Without Losing Your Mind**: production lessons from Semantic Kernel and Azure OpenAI
- **The OWASP Top Ten Is Not a Checklist: It Is a Story**: how to actually use OWASP in .NET
- **What Three Decades of Software Development Taught Me About Writing Code That Lasts**: personal reflection on writing durable code
- **JWT Tokens Are Not Magic and That Matters**: authentication pitfalls in ASP.NET Core
- **Eight Seconds to Eighty Milliseconds: Diagnosing a Production Performance Problem**: tracking down an N+1 query and a missing index to cut API response time from 8 s to 80 ms

### Custom CMS Pages

Create arbitrary pages at any slug from Admin → **Pages**. Published pages are rendered automatically at `/{slug}` and include full SEO metadata. Useful for pages like `/services`, `/hire-me`, `/speaking`, etc.

---

## Static Site Generator

The admin **Static Site** tab generates a complete, deployable, dark-mode HTML snapshot of the entire portfolio in one click.

### What's included in the ZIP

| Content | Details |
|---|---|
| Pages | Home, About, Projects, Skills, Blog (listing + all posts), Contact |
| CMS pages | All published custom pages |
| Stylesheet | Single `css/site.css`, dark mode, brand palette (`#0F0A1E` / `#C4B5FD`) |
| Navigation | Responsive nav with mobile hamburger (pure CSS/JS, no dependencies) |
| Tech chips | Technology badges on project cards and blog posts |
| Images | All project SVGs and featured images bundled at correct relative paths |

### Deployment

| Host | Steps |
|---|---|
| **GitHub Pages** | Extract ZIP into `gh-pages` branch root or a `/docs` folder |
| **Netlify / Vercel** | Drag and drop the extracted folder into the deploy UI |
| **Azure Static Web Apps** | Point the build output to the extracted folder path |

> The static site is a point-in-time snapshot. Re-generate and re-deploy whenever you update content.

---

## Contact Form CAPTCHA

The contact form includes a simple server-side math challenge ("What is A + B?"). The correct answer is required before the message is sent. A wrong answer regenerates the challenge. No external service or API key needed, pure in-component arithmetic.

---

## Work Projects

Three real-world applications are showcased under the **Work Project** category on the Projects and Home pages, each with an SVG app-mockup image, a detailed description, and a matching blog post:

| Project | Description | Tech |
|---|---|---|
| **BookIt** | Full-featured booking management system with real-time availability, SMS notifications, light/dark mode | Blazor, ASP.NET Core, MudBlazor, SQL Server, EF Core |
| **Curo** | Healthcare care management platform for coordinating patient care plans and clinical workflows | ASP.NET Core, Blazor, SQL Server, EF Core, Azure |
| **TalentConnect** | Recruitment management platform with job postings, multi-stage candidate pipelines, and analytics | Blazor, ASP.NET Core, MudBlazor, SQL Server, EF Core |

The full project catalogue also includes:

| Project | Category | Description |
|---|---|---|
| **MAUI Cross-Platform App** | Mobile Application | .NET MAUI app targeting iOS, Android, Windows and macOS from one codebase |
| **Patient CRM** | Healthcare | Patient relationship management system (in development) |
| **AI Diagnostic Assistant** | AI | AI-powered clinical decision support using Semantic Kernel and Azure OpenAI |
| **SecureAPI Framework** | Security | Hardened API security baseline for .NET covering JWT, OWASP mitigations and rate limiting |

---

## Security Notes

- No public registration; admin creates accounts only
- Account lockout after 5 failed login attempts (15-minute lockout)
- Cookie auth for Blazor with 8-hour sliding expiration
- JWT for API with issuer and audience validation
- HTTPS enforced in non-development environments
- HSTS enabled in production
- Sensitive config values are empty in committed `appsettings.json`; supply via secrets or environment variables

