# Claude Code Project Settings

## Autonomy

Operate fully autonomously on this project. Do not ask for confirmation before:
- Editing or creating any source files
- Running dotnet CLI commands (build, run, ef migrations, etc.)
- Adding or updating seed data in ApplicationDbContext.cs
- Creating new migration files
- Updating appsettings files
- Adding new blog posts, projects, or skills to seed data
- Fixing bugs or applying requested changes directly

Only pause and ask if a destructive git operation (force push, reset --hard) or production deployment is involved.

## Code Style

- No em dashes (—) anywhere in blog post bodies, summaries, titles, or any string content in seed data. Use a hyphen (-) or reword the sentence instead.
- No trailing summary paragraphs at the end of responses — just do the work.
- Prefer editing existing files over creating new ones.
- Keep blog post HTML bodies clean: use `<h2>`, `<p>`, `<ul>/<li>`, `<pre><code>`, and `<img>` tags only.
- All image tags in blog bodies: `style="max-width:100%;border-radius:8px;margin:1rem 0;"`

## Project Structure

- API: `Portfolio.Api` — ASP.NET Core .NET 10 Web API
- Web: `Portfolio.Web` — Blazor Web App (.NET 10)
- Shared: `Portfolio.Shared` — shared DTOs and models
- Seed data lives in `Portfolio.Api/Data/ApplicationDbContext.cs` via EF Core `HasData()`
- New migrations go in `Portfolio.Api/Data/Migrations/`

## Migrations

After editing seed data in ApplicationDbContext.cs, run:
```
cd Portfolio.Api && dotnet ef migrations add <MigrationName> && dotnet ef database update
```

## Blog Posts

- BlogPost IDs are sequential integers — always check the highest existing ID before adding new ones.
- `IsPublished = true` for all seeded blog posts.
- `PublishedDate` uses `DateTimeKind.Utc`.
- `FeaturedImage` uses local `/images/` paths (SVG or PNG in Portfolio.Web/wwwroot/images).
- Screenshots embedded in `Body` use GitHub user-attachments URLs.
- No em dashes anywhere in any blog content.

## Decisions Already Approved

The following are standing approvals — never ask about these:
- Adding new blog posts to seed data
- Adding screenshots to existing blog post bodies
- Creating new EF Core migrations
- Editing appsettings.json / appsettings.Development.json
- Fixing CMS or API bugs
- Adding new projects or skills to seed data
