using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Models;

namespace Portfolio.Api.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Project>().HasData(
            new Project { Id = 1, Title = "BookIt", ShortDescription = "A real-time Blazor booking management system", Description = "BookIt is a full-featured booking management system built with ASP.NET Core Blazor. Businesses use it to manage appointments, resources, and customer bookings through a modern interface with light and dark mode support. Built on a clean architecture with real-time availability tracking, SMS notifications for customers, and a responsive MudBlazor UI.", TechStack = "Blazor, ASP.NET Core, SQL Server, Entity Framework Core, MudBlazor, C# .NET 10", Category = "Work Project", IsFeatured = true, SortOrder = 1, ImageUrl = "/images/bookit.svg", GitHubUrl = "https://github.com/dotnetappdev/bookit" },
            new Project { Id = 2, Title = "MAUI Cross-Platform App", ShortDescription = "A .NET MAUI mobile and desktop application", Description = "A cross-platform mobile and desktop application built with .NET MAUI targeting iOS, Android, Windows, and macOS. Delivers a native experience across all platforms from a single shared codebase, with REST API integration and offline-friendly data handling.", TechStack = ".NET MAUI, C#, XAML, REST APIs", Category = "Mobile Application", IsFeatured = true, SortOrder = 2, ImageUrl = "/images/maui.png", GitHubUrl = "https://github.com/dotnetappdev/maui-app" },
            new Project { Id = 3, Title = "Curo", ShortDescription = "Healthcare care management platform", Description = "Curo is a healthcare care management platform that replaced a paper-based system used by community carers. Built with Blazor and ASP.NET Core, it gives carers a task-driven workflow on any device and provides care managers with a live dashboard showing real-time visit progress. Hosted on Azure with full audit logging and role-based access control.", TechStack = "ASP.NET Core, Blazor, SQL Server, Entity Framework Core, Azure, MudBlazor, C# .NET 10", Category = "Work Project", IsFeatured = true, SortOrder = 3, ImageUrl = "/images/curo.svg", GitHubUrl = "https://github.com/dotnetappdev/curo" },
            new Project { Id = 4, Title = "Patient CRM", ShortDescription = "Patient relationship management system (in development)", Description = "A Patient CRM currently in development, designed to help healthcare providers manage patient relationships, appointments, communications, and care history in one centralised platform. Built with .NET 10, Blazor, and a REST API backend.", TechStack = "ASP.NET Core .NET 10, Blazor, SQL Server, EF Core, REST API, MudBlazor", Category = "Healthcare", IsFeatured = true, SortOrder = 4, ImageUrl = "/images/patient-crm.png", GitHubUrl = "https://github.com/dotnetappdev/patient-crm" },
            new Project { Id = 5, Title = "AI Diagnostic Assistant", ShortDescription = "AI-powered clinical decision support tool", Description = "An AI assistant integrated into a healthcare platform that helps clinical staff surface relevant patient history, flag anomalies in test results, and draft care plan notes. Built on Semantic Kernel and Azure OpenAI with a strict evaluation layer that confidence-scores every response before it reaches clinical staff. All AI output is fully audited.", TechStack = "Semantic Kernel, Azure OpenAI, ASP.NET Core, Blazor, SQL Server, Vector Search, .NET 10", Category = "AI", IsFeatured = true, SortOrder = 5, ImageUrl = "/images/ai-assistant.png", GitHubUrl = "https://github.com/dotnetappdev/ai-diagnostic-assistant" },
            new Project { Id = 6, Title = "SecureAPI Framework", ShortDescription = "Hardened API security baseline for .NET", Description = "A reusable security baseline for ASP.NET Core APIs covering JWT authentication with algorithm pinning, OWASP Top Ten mitigations, rate limiting, structured security logging, and automated dependency vulnerability scanning in the CI pipeline. Used as the starting point for all new API projects so that security is built in from the first commit rather than retrofitted.", TechStack = "ASP.NET Core, JWT, OAuth2, OWASP, Rate Limiting, Polly, GitHub Actions", Category = "Security", IsFeatured = true, SortOrder = 6, ImageUrl = "/images/secure-api.png", GitHubUrl = "https://github.com/dotnetappdev/secure-api-framework" },
            new Project { Id = 7, Title = "TalentConnect", ShortDescription = "A Blazor recruitment management platform", Description = "TalentConnect is a full-featured recruitment management platform built with Blazor and ASP.NET Core. It streamlines the end-to-end hiring process with job posting management, a configurable multi-stage candidate pipeline, interview scheduling, automated notifications, and detailed recruitment analytics. Built for teams who want a data-driven hiring workflow without the spreadsheets.", TechStack = "Blazor, ASP.NET Core, SQL Server, Entity Framework Core, MudBlazor, REST API, C# .NET 10", Category = "Work Project", IsFeatured = true, SortOrder = 7, ImageUrl = "/images/talentconnect.svg", GitHubUrl = "https://github.com/dotnetappdev/talentconnect" }
        );

        builder.Entity<Skill>().HasData(
            new Skill { Id = 1, Name = "C#", Category = "Languages", Proficiency = 98 },
            new Skill { Id = 2, Name = "ASP.NET Core", Category = "Frameworks", Proficiency = 95 },
            new Skill { Id = 3, Name = "Blazor", Category = "Frameworks", Proficiency = 92 },
            new Skill { Id = 4, Name = ".NET MAUI", Category = "Frameworks", Proficiency = 85 },
            new Skill { Id = 5, Name = "SQL Server", Category = "Databases", Proficiency = 90 },
            new Skill { Id = 6, Name = "Entity Framework Core", Category = "Databases", Proficiency = 90 },
            new Skill { Id = 7, Name = "REST API Design", Category = "Architecture", Proficiency = 92 },
            new Skill { Id = 8, Name = "MudBlazor", Category = "UI Frameworks", Proficiency = 88 },
            new Skill { Id = 9, Name = "Azure", Category = "Cloud", Proficiency = 82 },
            new Skill { Id = 10, Name = "Git", Category = "Tools", Proficiency = 90 },
            new Skill { Id = 11, Name = "JavaScript", Category = "Languages", Proficiency = 75 },
            new Skill { Id = 12, Name = "HTML/CSS", Category = "Languages", Proficiency = 85 },
            new Skill { Id = 13, Name = "WPF", Category = "Frameworks", Proficiency = 80 },
            new Skill { Id = 14, Name = "WinForms", Category = "Frameworks", Proficiency = 85 },
            new Skill { Id = 15, Name = "Microservices", Category = "Architecture", Proficiency = 78 },
            new Skill { Id = 16, Name = "Semantic Kernel", Category = "AI", Proficiency = 88 },
            new Skill { Id = 17, Name = "Azure OpenAI", Category = "AI", Proficiency = 85 },
            new Skill { Id = 18, Name = "ML.NET", Category = "AI", Proficiency = 75 },
            new Skill { Id = 19, Name = "RAG Pipelines", Category = "AI", Proficiency = 82 },
            new Skill { Id = 20, Name = "Vector Search", Category = "AI", Proficiency = 78 },
            new Skill { Id = 21, Name = "OWASP Top Ten", Category = "Security", Proficiency = 90 },
            new Skill { Id = 22, Name = "OAuth2 / OIDC", Category = "Security", Proficiency = 88 },
            new Skill { Id = 23, Name = "JWT Authentication", Category = "Security", Proficiency = 92 },
            new Skill { Id = 24, Name = "Threat Modelling", Category = "Security", Proficiency = 80 },
            new Skill { Id = 25, Name = "Penetration Testing", Category = "Security", Proficiency = 72 }
        );
    }
}
