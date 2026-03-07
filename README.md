# David Buckley — AI Developer & Security Engineer Portfolio

A professional portfolio website built with .NET 10, Blazor, MudBlazor, and Entity Framework Core. Positions David as a senior software engineer specialising in AI development and application security with 30 years of .NET experience.

## Screenshots

### Home Page
![Home Page](https://github.com/user-attachments/assets/b6d72e2d-44a9-4a14-b062-0af200165768)

### Projects (with Work Project category and tech chips)
![Projects](https://github.com/user-attachments/assets/3116e651-665f-4efc-88cb-c4892bd25bbe)

### Blog (with Projects category and featured SVG mockups)
![Blog Listing](https://github.com/user-attachments/assets/074821e4-806d-434a-8048-2ad3636941b9)

### Blog Post (BookIt — with tech chips)
![Blog Post](https://github.com/user-attachments/assets/a0162a28-9b72-41db-9564-7d0c6fcf8b46)

### Skills (with AI and Security)
![Skills](https://github.com/user-attachments/assets/8ab05d13-b393-4063-9a8e-a2e5733c3c91)

### About
![About](https://github.com/user-attachments/assets/5a13548e-39a0-4421-8902-9eb4c5283677)

### Contact (with math CAPTCHA)
![Contact](https://github.com/user-attachments/assets/5cdd5ecd-51cf-4848-b019-54006a15796e)

---

## CMS Screenshots

### Admin Dashboard — Hero Stats
![Admin Dashboard](https://github.com/user-attachments/assets/6b0b927f-6459-4ba4-97ee-cd9346156168)

### Admin — Static Site Generator
![Admin Static Site](https://github.com/user-attachments/assets/59c269e9-1e32-47b0-a9c4-3e41ebd12354)

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

- **AI Developer positioning** — hero section, skills, and projects lead with AI expertise
- **Security focus** — dedicated Security skills category, OWASP/OAuth2/JWT projects
- **Work Project showcase** — dedicated "Work Project" category for BookIt, Curo, and TalentConnect with SVG app-mockup images and tech chip badges
- **WordPress-style CMS** — create, edit, publish and delete blog posts and custom pages entirely from the admin dashboard with no code deploy required
- **WYSIWYG editor** — Quill rich-text editor (served locally, no CDN) for blog posts and pages; supports headings, bold/italic/lists/links/code blocks and more
- **DB-driven navigation** — add, reorder, hide or delete menu items live from the Menus admin tab
- **Custom CMS pages** — publish arbitrary pages at any slug (e.g. `/services`, `/hire-me`) with full SEO metadata
- **SEO & Open Graph** — per-post/page meta title, meta description, OG image and canonical URL injected via `<HeadContent>`
- **Featured images** — optional hero banner image on blog posts and card thumbnail on the blog listing; SVG app mockups on work project posts
- **Tech chip badges** — technology tags displayed as chips on project cards and blog posts
- **Contact form CAPTCHA** — server-side math challenge blocks spam without any external service or API key
- **Static site generator** — export a complete dark-mode static HTML snapshot of the portfolio as a deployable ZIP from the admin panel
- **Light and dark mode** — respects system preference, toggleable in the header
- **REST API with fallback** — Blazor app works standalone when API is offline
- **Configurable database provider** — SQL Server, SQLite, or PostgreSQL via one setting
- **Admin area** — create accounts, manage hero stats, configure API/SMS settings, manage blog posts, pages, menus, and generate static exports
- **In-app settings** — API base URL and SMS provider configured through the admin Settings tab (stored in DB, no restart needed)
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
| `DefaultAdmin:Email` | Seeded admin email | Set via secret or env var |
| `DefaultAdmin:Password` | Seeded admin password | Set via secret or env var |

> **API base URL** is now configured in the admin **Settings** tab (stored in the database) — no longer an `appsettings.json` key.

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

### Blog Posts — WordPress-style editor

The Blog Posts tab works like WordPress's post editor:

- **List view** — shows all posts with title, slug, category, publish date, status chip (Published / Draft) and quick-action buttons (Edit, Publish/Unpublish, Delete)
- **Status filters** — chip buttons to filter All / Published / Drafts
- **Editor view** — left column: large title field, permalink slug, Quill WYSIWYG body, excerpt; right sidebar: Publish card (status, toggle, date, Save button), Post Settings (category, tags, read time), Featured Image (URL + live preview), SEO & Social (meta title, meta description, OG image, canonical URL — expandable panel)
- **Back breadcrumb** — `← Posts` returns to the list without losing context

### Custom Pages

The Pages tab works identically to Blog Posts but creates stand-alone pages accessible at any custom slug. Published pages are rendered by the catch-all `/{**slug}` route and include full SEO head injection.

### Navigation Menus

The Menus tab lists all current nav items (label, URL, sort order, visibility). Changes — including adding new items or toggling visibility — are reflected live in the navigation bar without a page reload or restart.

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
4. In Admin → **Settings** → SMS Provider, set **Provider: Twilio**, fill in the credentials, and enter your **Admin receiver number**
5. Click **Send Test SMS** to verify

### ClickSend Setup

1. Create an account at [clicksend.com](https://www.clicksend.com)
2. Go to **Account → API Credentials** and generate an API key
3. Your login email is the **username**
4. In Admin → **Settings** → SMS Provider, set **Provider: ClickSend**, fill in the credentials
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

## Blog & CMS

The blog lives at `/blog`. Posts are stored in the database and managed entirely through the admin **Blog Posts** tab — no code changes or deployments needed.

### Creating a post

1. Go to `/admin` → **Blog Posts** → **Add New Post**
2. Enter a title (the slug is auto-generated but editable)
3. Write the body using the Quill WYSIWYG editor
4. Fill in the excerpt and any post settings (category, tags, read time)
5. Optionally add a featured image URL and SEO/OG metadata in the right sidebar
6. Click **Publish** to make it live, or **Save Draft** to keep it hidden

### Post features

- **Slug** — fully editable permalink (e.g. `/blog/my-post-title`)
- **Featured image** — displayed as a full-width hero banner on the post page and as a card thumbnail on the blog listing
- **SEO** — per-post `<title>`, `<meta name="description">`, `og:title`, `og:description`, `og:image`, and `<link rel="canonical">` injected automatically
- **Status** — toggle between Published and Draft at any time without deleting

### Seeded posts

The database is seeded with eight posts on first run:

- **Building TalentConnect: A Modern Blazor Recruitment Platform** — building a full-stack recruitment platform with job pipelines and analytics (Projects category)
- **Building Curo: A Healthcare Care Management Platform** — Blazor-based care management deployed to Azure with strict compliance (Projects category)
- **Building BookIt: A Blazor Booking Management System** — real-time booking system with SMS notifications and dark/light mode (Projects category)
- **Building AI into .NET Without Losing Your Mind** — production lessons from Semantic Kernel and Azure OpenAI
- **The OWASP Top Ten Is Not a Checklist — It Is a Story** — how to actually use OWASP in .NET
- **What Three Decades of Software Development Taught Me About Writing Code That Lasts** — personal reflection on writing durable code
- **JWT Tokens Are Not Magic and That Matters** — authentication pitfalls in ASP.NET Core
- **When AI Caught a Bug My Tests Missed** — real story from a healthcare AI project

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
| Stylesheet | Single `css/site.css` — dark mode, brand palette (`#0F0A1E` / `#C4B5FD`) |
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

The contact form includes a simple server-side math challenge ("What is A + B?"). The correct answer is required before the message is sent. A wrong answer regenerates the challenge. No external service or API key needed — pure in-component arithmetic.

---

## Work Projects

Three real-world applications are showcased under the **Work Project** category on the Projects and Home pages, each with an SVG app-mockup image, a detailed description, and a matching blog post:

| Project | Description | Tech |
|---|---|---|
| **BookIt** | Full-featured booking management system with real-time availability, SMS notifications, light/dark mode | Blazor, ASP.NET Core, MudBlazor, SQL Server, EF Core |
| **Curo** | Healthcare care management platform for coordinating patient care plans and clinical workflows | ASP.NET Core, Blazor, SQL Server, EF Core, Azure |
| **TalentConnect** | Recruitment management platform with job postings, multi-stage candidate pipelines, and analytics | Blazor, ASP.NET Core, MudBlazor, SQL Server, EF Core |

---

## Security Notes

- No public registration; admin creates accounts only
- Account lockout after 5 failed login attempts (15-minute lockout)
- Cookie auth for Blazor with 8-hour sliding expiration
- JWT for API with issuer and audience validation
- HTTPS enforced in non-development environments
- HSTS enabled in production
- Sensitive config values are empty in committed `appsettings.json`; supply via secrets or environment variables

