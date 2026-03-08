using System.IO.Compression;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;
using Portfolio.Shared.Models;

namespace Portfolio.Web.Services;

/// <summary>
/// Generates a complete, self-contained static HTML site as a ZIP archive.
/// The output is dark-mode by default with working navigation and all CMS content baked in.
/// </summary>
public class StaticSiteGeneratorService
{
    private readonly ApplicationDbContext _db;
    private readonly BlogService _blogService;
    private readonly MenuService _menuService;
    private readonly PortfolioApiService _apiService;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<StaticSiteGeneratorService> _logger;

    public StaticSiteGeneratorService(
        ApplicationDbContext db,
        BlogService blogService,
        MenuService menuService,
        PortfolioApiService apiService,
        IWebHostEnvironment env,
        ILogger<StaticSiteGeneratorService> logger)
    {
        _db = db;
        _blogService = blogService;
        _menuService = menuService;
        _apiService = apiService;
        _env = env;
        _logger = logger;
    }

    // ─── Public entry point ──────────────────────────────────────────────────

    public async Task<byte[]> GenerateAsync()
    {
        _logger.LogInformation("Starting static site generation");

        var posts      = await _blogService.GetAllPostsAsync();
        var menuItems  = await _menuService.GetVisibleItemsAsync();
        var projects   = await _apiService.GetProjectsAsync() ?? [];
        var skills     = await _apiService.GetSkillsAsync()   ?? [];
        var heroStats  = await _db.HeroStats.OrderBy(s => s.SortOrder).ToListAsync();
        var cmsPages   = await _db.CmsPages.Where(p => p.IsPublished).ToListAsync();

        using var ms = new MemoryStream();
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
        {
            AddText(zip, "css/site.css", BuildCss());

            AddText(zip, "index.html",           BuildHomePage(menuItems, heroStats, projects, depth: 0));
            AddText(zip, "about/index.html",     BuildAboutPage(menuItems, depth: 1));
            AddText(zip, "projects/index.html",  BuildProjectsPage(menuItems, projects, depth: 1));
            AddText(zip, "skills/index.html",    BuildSkillsPage(menuItems, skills, depth: 1));
            AddText(zip, "blog/index.html",      BuildBlogListPage(menuItems, posts, depth: 1));
            AddText(zip, "contact/index.html",   BuildContactPage(menuItems, depth: 1));

            foreach (var post in posts)
            {
                var related = posts.Where(p => p.Slug != post.Slug).Take(2).ToList();
                AddText(zip, $"blog/{post.Slug}/index.html",
                    BuildBlogPostPage(menuItems, post, related, depth: 2));
            }

            foreach (var project in projects.Where(p => !string.IsNullOrWhiteSpace(p.Slug)))
            {
                var others = projects.Where(p => p.Slug != project.Slug).Take(3).ToList();
                AddText(zip, $"projects/{project.Slug}/index.html",
                    BuildProjectDetailPage(menuItems, project, others, depth: 2));
            }

            foreach (var page in cmsPages)
                AddText(zip, $"{page.Slug}/index.html", BuildCmsPage(menuItems, page, depth: 1));

            // Copy wwwroot/images into the zip
            var imagesDir = Path.Combine(_env.WebRootPath, "images");
            if (Directory.Exists(imagesDir))
            {
                foreach (var file in Directory.GetFiles(imagesDir))
                {
                    var bytes = await File.ReadAllBytesAsync(file);
                    var entry = zip.CreateEntry($"images/{Path.GetFileName(file)}");
                    await using var es = entry.Open();
                    await es.WriteAsync(bytes);
                }
            }
        }

        _logger.LogInformation("Static site generation complete: {Bytes} bytes", ms.Length);
        return ms.ToArray();
    }

    // ─── ZIP helpers ─────────────────────────────────────────────────────────

    private static void AddText(ZipArchive zip, string path, string content)
    {
        var entry = zip.CreateEntry(path);
        using var writer = new StreamWriter(entry.Open(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        writer.Write(content);
    }

    // ─── Utility ─────────────────────────────────────────────────────────────

    /// <summary>Returns a relative-path prefix for the given nesting depth (0 = root).</summary>
    private static string Root(int depth) => string.Concat(Enumerable.Repeat("../", depth));

    private static string H(string? s) => WebUtility.HtmlEncode(s ?? string.Empty);

    private static string CategoryChipClass(string category) => category switch
    {
        "AI"       => "chip-filled-ai",
        "Security" => "chip-filled-security",
        ".NET"     => "chip-filled-dotnet",
        "Projects" => "chip-filled-projects",
        _          => "chip-filled",
    };

    private static string ProjectColorClass(string category) => category switch
    {
        "Healthcare"        => "chip-error",
        "Mobile Application"=> "chip-secondary",
        "Web Application"   => "chip-primary",
        "Work Project"      => "chip-primary",
        "AI"                => "chip-secondary",
        "Security"          => "chip-warning",
        _                   => "chip-primary",
    };

    private static string SkillBarClass(int p) => p switch
    {
        >= 90 => "high",
        >= 75 => "medium",
        _     => "low",
    };

    private static string StatValueClass(string color) => color.ToLowerInvariant() switch
    {
        "primary"   => "primary",
        "secondary" => "secondary",
        "tertiary"  => "tertiary",
        "error"     => "error",
        "success"   => "success",
        "warning"   => "warning",
        _           => "primary",
    };

    // ─── Shared page shell ────────────────────────────────────────────────────

    private static string PageShell(
        string title,
        string bodyContent,
        IReadOnlyList<MenuItem> menuItems,
        int depth,
        string? metaDescription = null)
    {
        var r = Root(depth);
        var navLinks = BuildNavLinks(menuItems, r);
        var desc = H(metaDescription ?? $"{title} | David Buckley, Senior .NET Engineer");

        return $$"""
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <meta name="description" content="{{desc}}" />
  <title>{{H(title)}} | David Buckley</title>
  <link rel="stylesheet" href="{{r}}css/site.css" />
</head>
<body>
  <nav class="nav" role="navigation">
    <a class="nav-brand" href="{{r}}">David Buckley</a>
    <ul class="nav-links" id="nav-links">
{{navLinks}}
    </ul>
    <button class="nav-toggle" onclick="toggleNav()" aria-label="Toggle navigation">&#9776;</button>
  </nav>
  <main>
{{bodyContent}}
  </main>
  <footer class="footer">
    <p>&copy; {{DateTime.UtcNow.Year}} David Buckley. Senior Software Engineer &amp; AI Developer.</p>
  </footer>
  <script>
    function toggleNav() {
      document.getElementById('nav-links').classList.toggle('open');
    }
  </script>
</body>
</html>
""";
    }

    private static string BuildNavLinks(IReadOnlyList<MenuItem> menuItems, string root)
    {
        var sb = new StringBuilder();
        foreach (var item in menuItems)
        {
            // Convert absolute URL to relative by stripping leading slash
            var href = item.Url.TrimStart('/');
            var target = item.OpenInNewTab ? " target=\"_blank\" rel=\"noopener\"" : string.Empty;
            // Root page is just the root prefix, everything else gets a folder/index.html
            var url = href.Length == 0
                ? root
                : $"{root}{href}/";
            sb.AppendLine($"      <li><a href=\"{url}\"{target}>{H(item.Label)}</a></li>");
        }
        return sb.ToString();
    }

    // ─── CSS ─────────────────────────────────────────────────────────────────

    private static string BuildCss() => """
/* ── Reset ───────────────────────────────────────────── */
*, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }
html { scroll-behavior: smooth; }
body {
  font-family: 'Segoe UI', system-ui, -apple-system, sans-serif;
  font-size: 16px;
  line-height: 1.6;
  background-color: #0F0A1E;
  color: #F5F0FF;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

/* ── Design tokens (dark mode by default) ────────────── */
:root {
  --bg:           #0F0A1E;
  --surface:      #1E1335;
  --surface-alt:  #180E2C;
  --appbar:       #2D1B69;
  --primary:      #C4B5FD;
  --secondary:    #67E8F9;
  --tertiary:     #6EE7B7;
  --text:         #F5F0FF;
  --text-muted:   #A78BFA;
  --divider:      #3B2A6E;
  --border:       rgba(196,181,253,0.2);
  --warning:      #FFA726;
  --error:        #EF9A9A;
  --success:      #A5D6A7;
}

/* ── Navigation ──────────────────────────────────────── */
.nav {
  background: var(--appbar);
  padding: 0 1.5rem;
  display: flex;
  align-items: center;
  height: 64px;
  position: sticky;
  top: 0;
  z-index: 100;
  box-shadow: 0 2px 12px rgba(0,0,0,0.4);
}
.nav-brand {
  color: #fff;
  text-decoration: none;
  font-size: 1.15rem;
  font-weight: 700;
  margin-right: auto;
  white-space: nowrap;
}
.nav-links {
  display: flex;
  gap: 0.25rem;
  list-style: none;
  align-items: center;
}
.nav-links a {
  color: rgba(255,255,255,0.9);
  text-decoration: none;
  padding: 0.45rem 0.8rem;
  border-radius: 6px;
  transition: background 0.2s;
  font-size: 0.95rem;
  white-space: nowrap;
}
.nav-links a:hover { background: rgba(255,255,255,0.12); }
.nav-toggle {
  display: none;
  background: none;
  border: none;
  cursor: pointer;
  padding: 0.5rem;
  color: #fff;
  font-size: 1.5rem;
}

/* ── Hero section ─────────────────────────────────────── */
.hero {
  background: #12132e radial-gradient(ellipse at 70% 40%, rgba(99,102,241,0.18) 0%, transparent 60%),
              radial-gradient(ellipse at 20% 80%, rgba(109,40,217,0.12) 0%, transparent 60%);
  padding: 5rem 1.5rem 4rem;
  min-height: 460px;
  display: flex;
  align-items: center;
}
.hero-inner {
  max-width: 1200px;
  margin: 0 auto;
  width: 100%;
  display: flex;
  gap: 3rem;
  align-items: center;
}
.hero-text { flex: 1; }
.hero-badge {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  padding: 0.3rem 0.9rem;
  border-radius: 999px;
  border: 1px solid var(--primary);
  color: var(--primary);
  font-size: 0.8rem;
  font-weight: 600;
  letter-spacing: 0.05em;
  margin-bottom: 1.25rem;
}
.hero h1 {
  font-size: 2.8rem;
  font-weight: 800;
  line-height: 1.2;
  color: #F5F0FF;
  margin-bottom: 1rem;
}
.hero-accent { color: #C4B5FD; }
.hero-sub {
  font-size: 1.1rem;
  color: #A78BFA;
  margin-bottom: 2rem;
  max-width: 500px;
}
.hero-ctas { display: flex; gap: 1rem; flex-wrap: wrap; margin-bottom: 1.5rem; }
.hero-trust { display: flex; gap: 1.5rem; flex-wrap: wrap; }
.hero-trust span { font-size: 0.9rem; color: #A78BFA; }
.hero-trust span::before { content: "✓ "; color: #6EE7B7; }

/* ── Dashboard panel ─────────────────────────────────── */
.hero-dash {
  flex: 0 0 380px;
  background: rgba(15,10,30,0.75);
  border: 1px solid rgba(196,181,253,0.2);
  border-radius: 12px;
  padding: 1.5rem;
  backdrop-filter: blur(12px);
}
.dash-header { display: flex; align-items: center; gap: 0.4rem; margin-bottom: 1.25rem; }
.dash-header span { font-size: 0.8rem; color: var(--text-muted); margin-left: 0.5rem; }
.dot { width: 12px; height: 12px; border-radius: 50%; }
.dot-red { background: #FF5F57; }
.dot-amber { background: #FEBC2E; }
.dot-green { background: #28C840; }
.stats-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 0.75rem; margin-bottom: 1rem; }
.stat-card {
  background: rgba(255,255,255,0.05);
  border-radius: 8px;
  padding: 0.9rem;
  text-align: center;
}
.stat-value { font-size: 1.8rem; font-weight: 800; }
.stat-value.primary   { color: #C4B5FD; }
.stat-value.secondary { color: #67E8F9; }
.stat-value.tertiary  { color: #6EE7B7; }
.stat-value.error     { color: #EF9A9A; }
.stat-value.success   { color: #A5D6A7; }
.stat-value.warning   { color: #FFA726; }
.stat-label { font-size: 0.8rem; color: var(--text-muted); }
.dash-subtitle { font-size: 0.7rem; letter-spacing: 0.08em; color: var(--text-muted); margin-bottom: 0.5rem; }

/* ── Page banner ──────────────────────────────────────── */
.page-banner {
  background: linear-gradient(135deg, #4338CA, #5B5BD6 50%, #7C3AED);
  padding: 2.5rem 1.5rem;
  color: #fff;
}
.page-banner h1 { font-size: 2rem; font-weight: 700; margin-bottom: 0.3rem; }
.page-banner p { opacity: 0.9; font-size: 1.05rem; }
.page-banner .container { max-width: 1200px; margin: 0 auto; }

/* ── Page content ─────────────────────────────────────── */
.page-content {
  flex: 1;
  padding: 2rem 1.5rem;
  max-width: 1200px;
  margin: 0 auto;
  width: 100%;
}
main { display: flex; flex-direction: column; flex: 1; }

/* ── Section headings ─────────────────────────────────── */
h2.section-title {
  font-size: 1.6rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
}
.section-divider { border: none; border-top: 1px solid var(--divider); margin-bottom: 1.5rem; }

/* ── Cards ────────────────────────────────────────────── */
.card {
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: 12px;
  padding: 1.5rem;
  transition: transform 0.2s, box-shadow 0.2s;
}
.card:hover { transform: translateY(-2px); box-shadow: 0 8px 24px rgba(196,181,253,0.12); }
.card-header { margin-bottom: 0.75rem; }
.card-header h3 { font-size: 1.15rem; font-weight: 700; }
.card-header .category { font-size: 0.85rem; color: var(--text-muted); }

/* ── Grid layouts ─────────────────────────────────────── */
.grid-2 { display: grid; grid-template-columns: repeat(2, 1fr); gap: 1.5rem; }
.grid-3 { display: grid; grid-template-columns: repeat(3, 1fr); gap: 1.5rem; }
.grid-4 { display: grid; grid-template-columns: repeat(4, 1fr); gap: 1.5rem; }

/* ── Chips / Tags ─────────────────────────────────────── */
.chip-set { display: flex; flex-wrap: wrap; gap: 0.35rem; margin-top: 0.5rem; }
.chip {
  display: inline-flex;
  align-items: center;
  padding: 0.2rem 0.7rem;
  border-radius: 999px;
  font-size: 0.78rem;
  font-weight: 500;
  background: rgba(196,181,253,0.1);
  color: var(--primary);
  border: 1px solid var(--border);
}
.chip-primary   { background: rgba(196,181,253,0.15); color: var(--primary); border-color: var(--primary); }
.chip-secondary { background: rgba(103,232,249,0.12); color: var(--secondary); border-color: var(--secondary); }
.chip-tertiary  { background: rgba(110,231,183,0.12); color: var(--tertiary); border-color: var(--tertiary); }
.chip-warning   { background: rgba(255,167,38,0.15);  color: var(--warning); border-color: var(--warning); }
.chip-error     { background: rgba(239,154,154,0.15); color: var(--error); border-color: var(--error); }
.chip-success   { background: rgba(165,214,167,0.12); color: var(--success); border-color: var(--success); }
.chip-filled-ai       { background: rgba(196,181,253,0.35); color: #fff; border-color: transparent; }
.chip-filled-security { background: rgba(239,154,154,0.35); color: #fff; border-color: transparent; }
.chip-filled-dotnet   { background: rgba(196,181,253,0.25); color: #fff; border-color: transparent; }
.chip-filled-projects { background: rgba(165,214,167,0.3);  color: #fff; border-color: transparent; }
.chip-filled          { background: rgba(196,181,253,0.2);  color: #fff; border-color: transparent; }

/* ── Buttons ──────────────────────────────────────────── */
.btn {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  padding: 0.65rem 1.3rem;
  border-radius: 8px;
  text-decoration: none;
  font-size: 0.95rem;
  font-weight: 600;
  transition: opacity 0.2s, transform 0.1s;
  cursor: pointer;
  border: none;
}
.btn:hover { opacity: 0.85; transform: translateY(-1px); }
.btn-primary { background: var(--primary); color: #1a0050; }
.btn-outline { background: transparent; color: var(--primary); border: 1.5px solid var(--primary); }
.btn-sm { padding: 0.4rem 0.9rem; font-size: 0.85rem; }

/* ── Progress bars (skills) ───────────────────────────── */
.skill-row { display: flex; align-items: center; gap: 0.75rem; margin-bottom: 0.85rem; }
.skill-name { min-width: 160px; font-size: 0.9rem; }
.skill-bar { flex: 1; height: 8px; background: var(--divider); border-radius: 4px; overflow: hidden; }
.skill-fill { height: 100%; border-radius: 4px; background: var(--primary); }
.skill-fill.high   { background: var(--tertiary); }
.skill-fill.medium { background: var(--primary); }
.skill-fill.low    { background: var(--warning); }
.skill-pct { min-width: 38px; font-size: 0.8rem; color: var(--text-muted); text-align: right; }

/* ── Timeline ─────────────────────────────────────────── */
.timeline { position: relative; padding-left: 2rem; }
.timeline::before {
  content: '';
  position: absolute;
  left: 0.5rem;
  top: 0.5rem;
  bottom: 0.5rem;
  width: 2px;
  background: var(--divider);
}
.timeline-item { position: relative; margin-bottom: 2rem; }
.timeline-item::before {
  content: '';
  position: absolute;
  left: -1.65rem;
  top: 0.4rem;
  width: 12px;
  height: 12px;
  border-radius: 50%;
  background: var(--primary);
}
.timeline-item:nth-child(2)::before { background: var(--secondary); }
.timeline-item:nth-child(3)::before { background: var(--tertiary); }
.timeline-item:nth-child(4)::before { background: var(--warning); }
.timeline-period { font-size: 0.8rem; color: var(--text-muted); margin-bottom: 0.3rem; }

/* ── Blog cards ───────────────────────────────────────── */
.blog-card { margin-bottom: 1rem; }
.blog-card .card-meta { display: flex; gap: 1rem; font-size: 0.85rem; color: var(--text-muted); margin-top: 0.75rem; }
.blog-card h2 { font-size: 1.3rem; font-weight: 700; margin: 0.5rem 0; }
.blog-card h2 a { color: var(--text); text-decoration: none; }
.blog-card h2 a:hover { color: var(--primary); }
.blog-card p { color: var(--text-muted); }
.blog-card .read-btn { margin-top: 1rem; }
.featured-img { width: 100%; max-height: 220px; object-fit: cover; border-radius: 8px 8px 0 0; }
.featured-img-post { width: 100%; max-height: 400px; object-fit: cover; border-radius: 8px; margin-bottom: 1.5rem; }

/* ── Post body ────────────────────────────────────────── */
.post-body { font-size: 1.05rem; line-height: 1.8; }
.post-body p { margin-bottom: 1.5rem; }
.post-body h1, .post-body h2, .post-body h3 { margin: 1.5rem 0 0.75rem; color: var(--primary); }
.post-body a { color: var(--secondary); }
.post-body blockquote {
  border-left: 4px solid var(--primary);
  padding-left: 1rem;
  font-style: italic;
  opacity: 0.85;
  margin: 1rem 0;
}
.post-meta { display: flex; gap: 1.5rem; flex-wrap: wrap; font-size: 0.9rem; color: var(--text-muted); margin-bottom: 1.5rem; }
.author-card { background: var(--surface); border: 1px solid var(--border); border-radius: 12px; padding: 1.5rem; }
.author-card h3 { font-size: 1rem; font-weight: 700; }
.author-card .author-title { font-size: 0.85rem; color: var(--text-muted); margin-bottom: 0.75rem; }

/* ── About page ───────────────────────────────────────── */
.about-flex { display: flex; gap: 2rem; align-items: flex-start; }
.about-sidebar { flex: 0 0 280px; }
.about-content { flex: 1; }
.profile-card { background: var(--surface); border: 1px solid var(--border); border-radius: 12px; padding: 1.5rem; text-align: center; }
.profile-avatar {
  width: 120px; height: 120px; border-radius: 50%; margin: 0 auto 1rem;
  background: var(--primary); display: flex; align-items: center; justify-content: center;
  font-size: 3rem; color: #1a0050;
}
.profile-info { margin: 1rem 0; text-align: left; }
.profile-info li { list-style: none; padding: 0.4rem 0; font-size: 0.9rem; border-top: 1px solid var(--divider); }
.profile-info li:first-child { border-top: none; }
.about-para { margin-bottom: 1.25rem; }

/* ── Contact page ─────────────────────────────────────── */
.contact-note {
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: 12px;
  padding: 2.5rem;
  text-align: center;
  max-width: 600px;
  margin: 2rem auto;
}
.contact-note h2 { font-size: 1.5rem; margin-bottom: 0.75rem; }
.contact-note p { color: var(--text-muted); margin-bottom: 1.25rem; }

/* ── CMS pages ────────────────────────────────────────── */
.cms-body { font-size: 1.05rem; line-height: 1.8; }
.cms-body p { margin-bottom: 1.25rem; }
.cms-body h1, .cms-body h2, .cms-body h3 { margin: 1.5rem 0 0.75rem; color: var(--primary); }

/* ── Divider ──────────────────────────────────────────── */
.divider { border: none; border-top: 1px solid var(--divider); margin: 2rem 0; }

/* ── Footer ───────────────────────────────────────────── */
.footer {
  background: var(--surface-alt);
  border-top: 1px solid var(--divider);
  padding: 1.5rem;
  text-align: center;
  color: var(--text-muted);
  font-size: 0.9rem;
  margin-top: auto;
}

/* ── Responsive ───────────────────────────────────────── */
@media (max-width: 768px) {
  .nav-links { display: none; }
  .nav-links.open {
    display: flex;
    flex-direction: column;
    gap: 0;
    position: absolute;
    top: 64px;
    left: 0;
    right: 0;
    background: var(--appbar);
    padding: 0.75rem 1rem;
    box-shadow: 0 4px 12px rgba(0,0,0,0.4);
    z-index: 99;
  }
  .nav-toggle { display: block; }
  .hero-inner { flex-direction: column; }
  .hero-dash { flex: none; width: 100%; }
  .hero h1 { font-size: 2rem; }
  .grid-2, .grid-3, .grid-4 { grid-template-columns: 1fr; }
  .about-flex { flex-direction: column; }
  .about-sidebar { flex: none; width: 100%; }
  .stats-grid { grid-template-columns: 1fr 1fr; }
}
""";

    // ─── Home page ────────────────────────────────────────────────────────────

    private static string BuildHomePage(
        IReadOnlyList<MenuItem> menuItems,
        List<HeroStat> heroStats,
        List<ProjectDto> projects,
        int depth)
    {
        var r = Root(depth);

        var statCards = string.Join("\n", heroStats.Select(s => $"""
              <div class="stat-card">
                <div class="stat-value {StatValueClass(s.Color)}">{H(s.Value)}</div>
                <div class="stat-label">{H(s.Label)}</div>
              </div>
"""));

        var featuredProjects = string.Join("\n", projects.Take(4).Select(p =>
        {
            var detailUrl = !string.IsNullOrWhiteSpace(p.Slug) ? $"{Root(depth)}projects/{p.Slug}/" : string.Empty;
            var titleHtml = !string.IsNullOrEmpty(detailUrl)
                ? $"""<a href="{detailUrl}" style="color:inherit;text-decoration:none;">{H(p.Title)}</a>"""
                : H(p.Title);
            var detailBtn = !string.IsNullOrEmpty(detailUrl)
                ? $"""<a class="btn btn-outline btn-sm" href="{detailUrl}">Details</a>"""
                : string.Empty;
            var ghBtn = p.GitHubUrl is not null
                ? $"""<a class="btn btn-outline btn-sm" href="{H(p.GitHubUrl)}" target="_blank" rel="noopener">GitHub</a>"""
                : string.Empty;
            var liveBtn = p.LiveUrl is not null
                ? $"""<a class="btn btn-primary btn-sm" href="{H(p.LiveUrl)}" target="_blank" rel="noopener">Live Demo</a>"""
                : string.Empty;
            var buttons = string.Join(" ", new[] { detailBtn, ghBtn, liveBtn }.Where(b => !string.IsNullOrEmpty(b)));
            var actions = !string.IsNullOrEmpty(buttons)
                ? $"""<div class="card-actions" style="margin-top:0.75rem;">{buttons}</div>"""
                : $"""<div class="card-actions" style="margin-top:0.75rem;"><a class="btn btn-outline btn-sm" href="{Root(depth)}projects/">View All Projects</a></div>""";
            return $"""
            <div class="card">
              <div class="card-header">
                <h3>{titleHtml}</h3>
                <span class="category {ProjectColorClass(p.Category)}" style="display:inline-block;border-radius:999px;padding:0.1rem 0.6rem;font-size:0.8rem;margin-top:0.25rem;">{H(p.Category)}</span>
              </div>
              <p style="font-size:0.9rem;color:var(--text-muted);margin:0.5rem 0 0.75rem;">{H(p.ShortDescription)}</p>
              <div class="chip-set">
{string.Join("\n", p.TechStack.Split(',').Take(3).Select(t => $"                <span class=\"chip chip-primary\">{H(t.Trim())}</span>"))}
              </div>
              {actions}
            </div>
""";
        }));

        var body = $$"""
    <section class="hero">
      <div class="hero-inner">
        <div class="hero-text">
          <div class="hero-badge">&#x1F680; SENIOR SOFTWARE ENGINEER</div>
          <h1>Your software,<br /><span class="hero-accent">always delivered.</span></h1>
          <p class="hero-sub">The all-in-one .NET engineer for AI-driven apps, hardened APIs, and enterprise-grade Blazor solutions. 30 years shipping production software, built to last and secured from day one.</p>
          <div class="hero-ctas">
            <a class="btn btn-primary" href="{{r}}projects/">View My Work</a>
            <a class="btn btn-outline" href="{{r}}contact/">Get In Touch</a>
          </div>
          <div class="hero-trust">
            <span>30-year track record</span>
            <span>AI &amp; Security specialist</span>
            <span>Open to new roles</span>
          </div>
        </div>
        <div class="hero-dash">
          <div class="dash-header">
            <span class="dot dot-red"></span>
            <span class="dot dot-amber"></span>
            <span class="dot dot-green"></span>
            <span>Portfolio at a Glance</span>
          </div>
          <div class="stats-grid">
{{statCards}}
          </div>
          <hr class="divider" style="margin:1rem 0 0.75rem;" />
          <div class="dash-subtitle">CORE EXPERTISE</div>
          <div class="chip-set">
            <span class="chip chip-primary">Semantic Kernel</span>
            <span class="chip chip-secondary">Azure OpenAI</span>
            <span class="chip chip-error">OWASP</span>
            <span class="chip chip-success">.NET 10</span>
            <span class="chip chip-tertiary">Blazor</span>
          </div>
        </div>
      </div>
    </section>

    <div class="page-content">
      <h2 class="section-title">Featured Projects</h2>
      <hr class="section-divider" />
      <div class="grid-4">
{{featuredProjects}}
      </div>
      <div style="text-align:center;margin-top:2rem;">
        <a class="btn btn-outline" href="{{r}}projects/">View All Projects &#8594;</a>
      </div>

      <div class="grid-2" style="margin-top:3rem;">
        <div class="card">
          <h3 style="margin-bottom:0.75rem;">&#129504; AI Development</h3>
          <p style="color:var(--text-muted);margin-bottom:1rem;">I specialise in AI-driven chat assistants and conversational interfaces that give real users genuine value using Semantic Kernel and Azure OpenAI.</p>
          <div class="chip-set">
            <span class="chip chip-primary">Semantic Kernel</span>
            <span class="chip chip-primary">Azure OpenAI</span>
            <span class="chip chip-primary">Ollama</span>
            <span class="chip chip-primary">RAG</span>
            <span class="chip chip-primary">ML.NET</span>
            <span class="chip chip-primary">Vector Search</span>
          </div>
        </div>
        <div class="card">
          <h3 style="margin-bottom:0.75rem;">&#128737; Application Security</h3>
          <p style="color:var(--text-muted);margin-bottom:1rem;">Security is not a checkbox at the end of a sprint. I build it into every layer from authentication design and threat modelling to penetration testing.</p>
          <div class="chip-set">
            <span class="chip chip-error">OWASP</span>
            <span class="chip chip-error">OAuth2 / OIDC</span>
            <span class="chip chip-error">Threat Modelling</span>
            <span class="chip chip-error">Pen Testing</span>
            <span class="chip chip-error">ASP.NET Identity</span>
          </div>
        </div>
      </div>

      <div class="card" style="margin-top:2rem;text-align:center;">
        <h2 style="font-size:1.4rem;font-weight:700;margin-bottom:1rem;">Core Technologies</h2>
        <div class="chip-set" style="justify-content:center;">
          <span class="chip chip-filled">C#</span>
          <span class="chip chip-filled">.NET 10</span>
          <span class="chip chip-filled">ASP.NET Core</span>
          <span class="chip chip-filled">Blazor</span>
          <span class="chip chip-filled">Semantic Kernel</span>
          <span class="chip chip-filled">Azure OpenAI</span>
          <span class="chip chip-filled">SQL Server</span>
          <span class="chip chip-filled">Entity Framework Core</span>
          <span class="chip chip-filled">REST APIs</span>
          <span class="chip chip-filled">OAuth2</span>
          <span class="chip chip-filled">OWASP</span>
          <span class="chip chip-filled">MudBlazor</span>
          <span class="chip chip-filled">Azure</span>
          <span class="chip chip-filled">.NET MAUI</span>
        </div>
      </div>
    </div>
""";

        return PageShell("Home", body, menuItems, depth,
            "David Buckley, Senior .NET Engineer specialising in AI, Security and Blazor");
    }

    // ─── About page ───────────────────────────────────────────────────────────

    private static string BuildAboutPage(IReadOnlyList<MenuItem> menuItems, int depth)
    {
        var body = """
    <div class="page-banner">
      <div class="container">
        <h1>About Me</h1>
        <p>Senior software engineer with 30 years building production systems across healthcare, finance, and enterprise.</p>
      </div>
    </div>
    <div class="page-content">
      <div class="about-flex">
        <div class="about-sidebar">
          <div class="profile-card">
            <div class="profile-avatar">&#128100;</div>
            <h2 style="font-size:1.2rem;font-weight:700;">David Buckley</h2>
            <p style="color:var(--text-muted);font-size:0.9rem;">Senior Software Engineer &amp; AI Developer</p>
            <ul class="profile-info">
              <li>&#128188; 30+ Years Experience</li>
              <li>&#128205; United Kingdom</li>
              <li>&#129504; AI &amp; Security Specialist</li>
              <li>&#9993; Available for Opportunities</li>
            </ul>
            <a class="btn btn-primary" href="../contact/" style="width:100%;justify-content:center;margin-top:1rem;">Get In Touch</a>
          </div>
        </div>
        <div class="about-content">
          <p class="about-para">I have been writing software professionally since 1994 and I still find it genuinely exciting. Over three decades I have worked across desktop, web, mobile and cloud, but the last few years have pulled me strongly toward two things that I think are shaping where software goes next: artificial intelligence and application security.</p>
          <p class="about-para">On the AI side I build real production systems rather than proof of concept toys. I use Semantic Kernel and Azure OpenAI to add intelligent features to .NET applications, things like AI chat assistants, document summarisation, anomaly detection and smart search. I care about making AI reliable and auditable because most of my work touches sectors like healthcare where a wrong answer is not just annoying, it can cause harm.</p>
          <p class="about-para">Security has been a growing focus for the same reason. Working on patient data systems forces you to treat security as a first class concern rather than something you bolt on at the end. I apply OWASP principles, model threats during design, and push for penetration testing before go live rather than hoping nothing breaks.</p>
          <p class="about-para">The foundation under all of this is the .NET ecosystem. C#, ASP.NET Core, Blazor, Entity Framework Core, and .NET MAUI are the tools I reach for every day.</p>

          <h2 style="font-size:1.3rem;font-weight:700;margin-bottom:1.25rem;">Career Highlights</h2>
          <div class="timeline">
            <div class="timeline-item">
              <h3>AI Engineering and Security Focus</h3>
              <p class="timeline-period">2022 to Present</p>
              <p style="color:var(--text-muted);font-size:0.9rem;">Shipping AI powered features into production .NET applications using Semantic Kernel, Azure OpenAI and RAG pipelines. Embedding security practices into every project from threat modelling and OWASP reviews to penetration testing and identity hardening.</p>
            </div>
            <div class="timeline-item">
              <h3>Modern .NET and Cloud</h3>
              <p class="timeline-period">2016 to 2022</p>
              <p style="color:var(--text-muted);font-size:0.9rem;">Deep dive into ASP.NET Core, Azure and microservices. Designed REST APIs and event driven architectures for enterprise clients.</p>
            </div>
            <div class="timeline-item">
              <h3>Enterprise .NET Development</h3>
              <p class="timeline-period">2005 to 2016</p>
              <p style="color:var(--text-muted);font-size:0.9rem;">Senior developer across finance, retail and healthcare. WinForms, WPF, ASP.NET Web Forms, early MVC, SQL Server design and optimisation.</p>
            </div>
            <div class="timeline-item">
              <h3>Early Career</h3>
              <p class="timeline-period">1994 to 2005</p>
              <p style="color:var(--text-muted);font-size:0.9rem;">Started with Visual Basic and early .NET Framework. Built desktop and database systems across several industries.</p>
            </div>
          </div>
        </div>
      </div>
    </div>
""";
        return PageShell("About Me", body, menuItems, depth,
            "About David Buckley: 30 years building .NET software");
    }

    // ─── Projects page ────────────────────────────────────────────────────────

    private static string BuildProjectsPage(
        IReadOnlyList<MenuItem> menuItems,
        List<ProjectDto> projects,
        int depth)
    {
        var r = Root(depth);

        var cards = string.Join("\n", projects.Select(p =>
        {
            var imageHtml = !string.IsNullOrWhiteSpace(p.ImageUrl)
                ? $"""<img class="featured-img" src="{r}{p.ImageUrl.TrimStart('/')}" alt="{H(p.Title)}" loading="lazy" />"""
                : string.Empty;

            var detailUrl = !string.IsNullOrWhiteSpace(p.Slug) ? $"{r}projects/{p.Slug}/" : string.Empty;
            var titleHtml = !string.IsNullOrEmpty(detailUrl)
                ? $"""<a href="{detailUrl}" style="color:inherit;text-decoration:none;">{H(p.Title)}</a>"""
                : H(p.Title);

            var ghBtn = p.GitHubUrl is not null
                ? $"""<a class="btn btn-outline btn-sm" href="{H(p.GitHubUrl)}" target="_blank" rel="noopener">GitHub</a>"""
                : string.Empty;
            var liveBtn = p.LiveUrl is not null
                ? $"""<a class="btn btn-primary btn-sm" href="{H(p.LiveUrl)}" target="_blank" rel="noopener">Live Demo</a>"""
                : string.Empty;
            var detailBtn = !string.IsNullOrEmpty(detailUrl)
                ? $"""<a class="btn btn-outline btn-sm" href="{detailUrl}">Details</a>"""
                : string.Empty;
            var featured = p.IsFeatured
                ? """<span class="chip chip-warning" style="margin-left:0.5rem;">Featured</span>"""
                : string.Empty;
            var buttons = string.Join(" ", new[] { detailBtn, ghBtn, liveBtn }.Where(b => !string.IsNullOrEmpty(b)));

            return $"""
            <div class="card">
              {imageHtml}
              <div class="card-header" style="margin-top:{(string.IsNullOrEmpty(imageHtml) ? "0" : "0.75rem")};">
                <h3>{titleHtml} {featured}</h3>
                <span class="chip {ProjectColorClass(p.Category)}" style="margin-top:0.25rem;">{H(p.Category)}</span>
              </div>
              <p style="margin:0.75rem 0;color:var(--text-muted);font-size:0.9rem;">{H(p.Description)}</p>
              <p style="font-size:0.8rem;color:var(--text-muted);font-weight:600;margin-bottom:0.4rem;">Technologies Used:</p>
              <div class="chip-set">
{string.Join("\n", p.TechStack.Split(',').Select(t => $"                <span class=\"chip chip-primary\">{H(t.Trim())}</span>"))}
              </div>
              {(!string.IsNullOrEmpty(buttons) ? $"""<div style="display:flex;gap:0.5rem;margin-top:1rem;">{buttons}</div>""" : "")}
            </div>
""";
        }));

        var body = $$"""
    <div class="page-banner">
      <div class="container">
        <h1>My Projects</h1>
        <p>A showcase of applications and systems I've designed and built over my career.</p>
      </div>
    </div>
    <div class="page-content">
      <div class="grid-2">
{{cards}}
      </div>
    </div>
""";
        return PageShell("Projects", body, menuItems, depth,
            "Portfolio projects by David Buckley: Blazor, ASP.NET Core, AI and Security applications");
    }

    // ─── Project detail page ──────────────────────────────────────────────────

    private static string BuildProjectDetailPage(
        IReadOnlyList<MenuItem> menuItems,
        ProjectDto project,
        List<ProjectDto> others,
        int depth)
    {
        var r = Root(depth);
        var imageHtml = !string.IsNullOrWhiteSpace(project.ImageUrl)
            ? $"""<img src="{r}{project.ImageUrl.TrimStart('/')}" alt="{H(project.Title)}" style="width:100%;max-height:280px;object-fit:contain;border-radius:8px;margin-bottom:1.5rem;background:rgba(255,255,255,0.05);padding:0.75rem;" loading="lazy" />"""
            : string.Empty;
        var ghBtn = project.GitHubUrl is not null
            ? $"""<a class="btn btn-outline" href="{H(project.GitHubUrl)}" target="_blank" rel="noopener">View on GitHub</a>"""
            : string.Empty;
        var liveBtn = project.LiveUrl is not null
            ? $"""<a class="btn btn-primary" href="{H(project.LiveUrl)}" target="_blank" rel="noopener">Live Demo</a>"""
            : string.Empty;
        var techChips = string.Join("\n", project.TechStack.Split(',').Select(t =>
            $"""<span class="chip chip-primary">{H(t.Trim())}</span>"""));
        var otherLinks = string.Join("\n", others.Where(p => !string.IsNullOrWhiteSpace(p.Slug)).Select(p =>
            $"""<li><a href="{r}projects/{p.Slug}/" style="color:var(--primary);">{H(p.Title)}</a> <span style="color:var(--text-muted);font-size:0.8rem;">— {H(p.Category)}</span></li>"""));
        var detailButtons = string.Join(" ", new[] { ghBtn, liveBtn }.Where(b => !string.IsNullOrEmpty(b)));
        var detailButtonsHtml = !string.IsNullOrEmpty(detailButtons)
            ? $"""<div style="display:flex;gap:0.75rem;margin-top:1rem;">{detailButtons}</div>"""
            : string.Empty;

        var body = $"""
    <div class="page-banner">
      <div class="container">
        <h1>{H(project.Title)}</h1>
        <div style="margin-top:0.5rem;display:flex;flex-wrap:wrap;gap:0.5rem;align-items:center;">
          <span class="chip {ProjectColorClass(project.Category)}">{H(project.Category)}</span>
          {(project.IsFeatured ? """<span class="chip chip-warning">Featured</span>""" : "")}
        </div>
      </div>
    </div>
    <div class="page-content">
      <div class="grid-content" style="display:grid;grid-template-columns:1fr 300px;gap:2rem;max-width:1100px;margin:0 auto;">
        <div>
          <a href="{r}projects/" style="display:inline-flex;align-items:center;gap:0.4rem;color:var(--primary);text-decoration:none;margin-bottom:1.5rem;font-size:0.9rem;">← Back to Projects</a>
          {imageHtml}
          <div class="card" style="margin-bottom:1.5rem;padding:1.5rem;line-height:1.8;">
            <p>{H(project.Description)}</p>
          </div>
          <div class="card" style="margin-bottom:1.5rem;">
            <p style="font-size:0.85rem;font-weight:600;margin-bottom:0.5rem;">Technologies Used</p>
            <div class="chip-set">{techChips}</div>
            {detailButtonsHtml}
          </div>
        </div>
        <div>
          <div class="card" style="margin-bottom:1.5rem;padding:1.25rem;">
            <p style="font-size:0.85rem;font-weight:600;margin-bottom:0.5rem;">About the Author</p>
            <p style="font-size:0.9rem;color:var(--text-muted);margin-bottom:1rem;">30 years building .NET software. Currently focused on AI integration and application security.</p>
            <a class="btn btn-primary btn-sm" href="{r}contact/" style="width:100%;justify-content:center;">Get In Touch</a>
          </div>
          {(otherLinks.Length > 0 ?
              $"<div class=\"card\" style=\"padding:1.25rem;\"><p style=\"font-size:0.85rem;font-weight:600;margin-bottom:0.75rem;\">More Projects</p><ul style=\"list-style:none;padding:0;margin:0;display:flex;flex-direction:column;gap:0.6rem;\">{otherLinks}</ul><div style=\"margin-top:1rem;\"><a href=\"{r}projects/\" style=\"color:var(--primary);font-size:0.9rem;\">View All Projects →</a></div></div>"
              : "")}
        </div>
      </div>
    </div>
""";
        return PageShell(project.Title, body, menuItems, depth, project.ShortDescription);
    }

    // ─── Skills page ──────────────────────────────────────────────────────────

    private static string BuildSkillsPage(
        IReadOnlyList<MenuItem> menuItems,
        List<SkillDto> skills,
        int depth)
    {
        var grouped = skills
            .GroupBy(s => s.Category)
            .ToDictionary(g => g.Key, g => g.ToList());

        var cards = string.Join("\n", grouped.Select(kvp =>
        {
            var rows = string.Join("\n", kvp.Value.Select(sk => $"""
              <div class="skill-row">
                <span class="skill-name">{H(sk.Name)}</span>
                <div class="skill-bar">
                  <div class="skill-fill {SkillBarClass(sk.Proficiency)}" style="width:{sk.Proficiency}%;"></div>
                </div>
                <span class="skill-pct">{sk.Proficiency}%</span>
              </div>
"""));

            return $"""
          <div class="card">
            <div class="card-header"><h3>{H(kvp.Key)}</h3></div>
{rows}
          </div>
""";
        }));

        var body = $$"""
    <div class="page-banner">
      <div class="container">
        <h1>Technical Skills</h1>
        <p>30 years of hands-on experience across the full software development stack.</p>
      </div>
    </div>
    <div class="page-content">
      <div class="grid-2">
{{cards}}
      </div>
    </div>
""";
        return PageShell("Skills", body, menuItems, depth,
            "Technical skills of David Buckley: C#, .NET, Blazor, AI, Security");
    }

    // ─── Blog list page ───────────────────────────────────────────────────────

    private static string BuildBlogListPage(
        IReadOnlyList<MenuItem> menuItems,
        IReadOnlyList<BlogPost> posts,
        int depth)
    {
        var r = Root(depth);
        var cards = string.Join("\n", posts.Select(p =>
        {
            var imgHtml = !string.IsNullOrWhiteSpace(p.FeaturedImage)
                ? $"""<img class="featured-img" src="{r}{p.FeaturedImage.TrimStart('/')}" alt="{H(p.Title)}" loading="lazy" />"""
                : string.Empty;

            var tags = string.Join("", p.TagList.Take(3).Select(t =>
                $"""<span class="chip" style="margin:0.15rem;">{H(t)}</span>"""));

            return $"""
          <div class="card blog-card">
            {imgHtml}
            <div style="padding:{(string.IsNullOrEmpty(imgHtml) ? "0" : "0.75rem 0 0")};">
              <div style="display:flex;align-items:center;flex-wrap:wrap;gap:0.35rem;margin-bottom:0.5rem;">
                <span class="chip {CategoryChipClass(p.Category)}">{H(p.Category)}</span>
                {tags}
              </div>
              <h2><a href="{r}blog/{p.Slug}/">{H(p.Title)}</a></h2>
              <p>{H(p.Summary)}</p>
              <div class="card-meta">
                <span>&#128197; {p.PublishedDate:d MMMM yyyy}</span>
                <span>&#128336; {p.ReadMinutes} min read</span>
              </div>
              <div class="read-btn">
                <a class="btn btn-outline btn-sm" href="{r}blog/{p.Slug}/">Read Post &#8594;</a>
              </div>
            </div>
          </div>
""";
        }));

        var body = $$"""
    <div class="page-banner">
      <div class="container">
        <h1>Blog</h1>
        <p>Thoughts on AI development, application security, and .NET from someone who has been building software since 1994.</p>
      </div>
    </div>
    <div class="page-content">
{{cards}}
    </div>
""";
        return PageShell("Blog", body, menuItems, depth,
            "Blog by David Buckley: AI, .NET, Security articles");
    }

    // ─── Blog post page ───────────────────────────────────────────────────────

    private static string BuildBlogPostPage(
        IReadOnlyList<MenuItem> menuItems,
        BlogPost post,
        IReadOnlyList<BlogPost> related,
        int depth)
    {
        var r = Root(depth);

        var imgHtml = !string.IsNullOrWhiteSpace(post.FeaturedImage)
            ? $"""<img class="featured-img-post" src="{r}{post.FeaturedImage.TrimStart('/')}" alt="{H(post.Title)}" />"""
            : string.Empty;

        var tags = string.Join("", post.TagList.Select(t =>
            $"""<span class="chip" style="margin:0.15rem;">{H(t)}</span>"""));

        var bodyHtml = IsHtmlBody(post.Body)
            ? $"""<div class="post-body cms-body">{post.Body}</div>"""
            : $"""<div class="post-body">{string.Join("", GetParagraphs(post.Body).Select(p => $"<p>{H(p)}</p>"))}</div>""";

        var relatedCards = string.Join("\n", related.Select(rp => $"""
              <div class="card" style="flex:1;">
                <span class="chip {CategoryChipClass(rp.Category)}" style="margin-bottom:0.5rem;">{H(rp.Category)}</span>
                <h3 style="font-size:1rem;font-weight:700;margin:0.4rem 0;">{H(rp.Title)}</h3>
                <p style="font-size:0.85rem;color:var(--text-muted);">{H(rp.Summary)}</p>
                <a class="btn btn-outline btn-sm" href="{r}blog/{rp.Slug}/" style="margin-top:0.75rem;">Read Post</a>
              </div>
"""));

        var body = $$"""
    <div class="page-content" style="max-width:800px;">
      <a class="btn btn-outline btn-sm" href="{{r}}blog/" style="margin-bottom:1.5rem;display:inline-flex;">&#8592; Back to Blog</a>

      {{imgHtml}}

      <div style="display:flex;align-items:center;flex-wrap:wrap;gap:0.35rem;margin-bottom:1rem;">
        <span class="chip {{CategoryChipClass(post.Category)}}">{H(post.Category)}</span>
        {{tags}}
      </div>

      <h1 style="font-size:2rem;font-weight:800;margin-bottom:1rem;">{{H(post.Title)}}</h1>

      <div class="post-meta">
        <span>&#128100; David Buckley</span>
        <span>&#128197; {{post.PublishedDate:d MMMM yyyy}}</span>
        <span>&#128336; {{post.ReadMinutes}} min read</span>
      </div>

      <hr class="divider" />

      {{bodyHtml}}

      <hr class="divider" />

      <div class="author-card">
        <h3>David Buckley</h3>
        <p class="author-title">Senior Software Engineer &amp; AI Developer</p>
        <p style="font-size:0.9rem;color:var(--text-muted);margin-bottom:1rem;">30 years building .NET software. Currently focused on AI integration and application security.</p>
        <a class="btn btn-primary btn-sm" href="{{r}}contact/">Get In Touch</a>
      </div>

      {{(relatedCards.Length > 0 ? $"""
      <hr class="divider" />
      <h2 style="font-size:1.3rem;font-weight:700;margin-bottom:1rem;">More Posts</h2>
      <div style="display:flex;gap:1.5rem;flex-wrap:wrap;">
{relatedCards}
      </div>
""" : "")}}
    </div>
""";

        return PageShell(
            post.MetaTitle ?? post.Title,
            body,
            menuItems,
            depth,
            post.MetaDescription ?? post.Summary);
    }

    // ─── Contact page ─────────────────────────────────────────────────────────

    private static string BuildContactPage(IReadOnlyList<MenuItem> menuItems, int depth)
    {
        var body = """
    <div class="page-banner">
      <div class="container">
        <h1>Get In Touch</h1>
        <p>I'm always open to discussing new opportunities, interesting projects, or just having a chat about technology.</p>
      </div>
    </div>
    <div class="page-content">
      <div class="contact-note">
        <h2>&#9993; Send Me a Message</h2>
        <p>This is a static snapshot of my portfolio. To send me a message, please visit the live site or reach out directly using the options below.</p>
        <div style="margin:1.5rem 0;display:flex;flex-direction:column;gap:0.75rem;text-align:left;">
          <div style="display:flex;align-items:center;gap:0.75rem;">
            <span style="font-size:1.25rem;">&#128205;</span>
            <span>United Kingdom</span>
          </div>
          <div style="display:flex;align-items:center;gap:0.75rem;">
            <span style="font-size:1.25rem;">&#128100;</span>
            <span>David Buckley, Senior Software Engineer &amp; AI Developer</span>
          </div>
        </div>
        <a class="btn btn-primary" href="../">View Portfolio</a>
      </div>
    </div>
""";
        return PageShell("Contact", body, menuItems, depth,
            "Contact David Buckley, Senior .NET Engineer");
    }

    // ─── CMS page ─────────────────────────────────────────────────────────────

    private static string BuildCmsPage(
        IReadOnlyList<MenuItem> menuItems,
        CmsPage page,
        int depth)
    {
        var imgHtml = !string.IsNullOrWhiteSpace(page.FeaturedImage)
            ? $"""<img class="featured-img-post" src="{Root(depth)}{page.FeaturedImage.TrimStart('/')}" alt="{H(page.Title)}" />"""
            : string.Empty;

        var bodyHtml = IsHtmlBody(page.Body)
            ? $"""<div class="cms-body">{page.Body}</div>"""
            : $"""<div class="cms-body">{string.Join("", GetParagraphs(page.Body).Select(p => $"<p>{H(p)}</p>"))}</div>""";

        var body = $"""
    <div class="page-banner">
      <div class="container">
        <h1>{H(page.Title)}</h1>
      </div>
    </div>
    <div class="page-content">
      {imgHtml}
      {bodyHtml}
    </div>
""";
        return PageShell(
            page.MetaTitle ?? page.Title,
            body,
            menuItems,
            depth,
            page.MetaDescription);
    }

    // ─── Body helpers ─────────────────────────────────────────────────────────

    private static bool IsHtmlBody(string body) =>
        body.TrimStart().StartsWith('<');

    private static IEnumerable<string> GetParagraphs(string body) =>
        body.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .Where(p => p.Length > 0);
}
