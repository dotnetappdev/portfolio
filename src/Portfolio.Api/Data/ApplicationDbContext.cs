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
            new Project { Id = 1, Title = "BookIt", ShortDescription = "A Blazor booking management application", Description = "BookIt is a full-featured booking management system built with ASP.NET Core Blazor. It allows businesses to manage appointments, resources, and customer bookings with an intuitive and modern interface featuring light and dark mode support.", TechStack = "Blazor, ASP.NET Core, SQL Server, Entity Framework Core, MudBlazor", Category = "Web Application", IsFeatured = true, SortOrder = 1, ImageUrl = "/images/bookit.png" },
            new Project { Id = 2, Title = "MAUI Cross-Platform App", ShortDescription = "A .NET MAUI mobile and desktop application", Description = "A cross-platform mobile and desktop application built with .NET MAUI targeting iOS, Android, Windows, and macOS. Delivers a native experience across all platforms with shared business logic.", TechStack = ".NET MAUI, C#, XAML, REST APIs", Category = "Mobile Application", IsFeatured = true, SortOrder = 2, ImageUrl = "/images/maui.png" },
            new Project { Id = 3, Title = "Curo", ShortDescription = "Healthcare care management platform", Description = "Curo is a healthcare care management platform designed to streamline patient care coordination. It provides tools for care plan management, patient tracking, and clinical workflow automation.", TechStack = "ASP.NET Core, Blazor, SQL Server, Entity Framework Core, Azure", Category = "Healthcare", IsFeatured = true, SortOrder = 3, ImageUrl = "/images/curo.png" },
            new Project { Id = 4, Title = "Patient CRM", ShortDescription = "Patient relationship management system (In Development)", Description = "A modern Patient CRM system currently in development, designed to help healthcare providers manage patient relationships, appointments, communications, and care history in one centralised platform. Built with the latest .NET 10 technology stack.", TechStack = "ASP.NET Core .NET 10, Blazor, SQL Server, EF Core, REST API, MudBlazor", Category = "Healthcare", IsFeatured = true, SortOrder = 4, ImageUrl = "/images/patient-crm.png" }
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
            new Skill { Id = 9, Name = "Azure", Category = "Cloud", Proficiency = 80 },
            new Skill { Id = 10, Name = "Git", Category = "Tools", Proficiency = 90 },
            new Skill { Id = 11, Name = "JavaScript", Category = "Languages", Proficiency = 75 },
            new Skill { Id = 12, Name = "HTML/CSS", Category = "Languages", Proficiency = 85 },
            new Skill { Id = 13, Name = "WPF", Category = "Frameworks", Proficiency = 80 },
            new Skill { Id = 14, Name = "WinForms", Category = "Frameworks", Proficiency = 85 },
            new Skill { Id = 15, Name = "Microservices", Category = "Architecture", Proficiency = 78 }
        );
    }
}
