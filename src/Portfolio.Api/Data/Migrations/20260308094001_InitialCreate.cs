using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Portfolio.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    ShortDescription = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TechStack = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    GitHubUrl = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    LiveUrl = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IsFeatured = table.Column<bool>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Proficiency = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Category", "Description", "GitHubUrl", "ImageUrl", "IsFeatured", "LiveUrl", "ShortDescription", "Slug", "SortOrder", "TechStack", "Title" },
                values: new object[,]
                {
                    { 1, "Work Project", "BookIt is a full-featured booking management system built with ASP.NET Core Blazor. Businesses use it to manage appointments, resources, and customer bookings through a modern interface with light and dark mode support. Built on a clean architecture with real-time availability tracking, SMS notifications for customers, and a responsive MudBlazor UI.", "https://github.com/dotnetappdev/bookit", "/images/bookit.svg", true, null, "A real-time Blazor booking management system", "bookit", 1, "Blazor, ASP.NET Core, SQL Server, Entity Framework Core, MudBlazor, C# .NET 10", "BookIt" },
                    { 2, "Mobile Application", "A cross-platform mobile and desktop application built with .NET MAUI targeting iOS, Android, Windows, and macOS. Delivers a native experience across all platforms from a single shared codebase, with REST API integration and offline-friendly data handling.", "https://github.com/dotnetappdev/maui-app", "/images/maui.png", true, null, "A .NET MAUI mobile and desktop application", "maui-cross-platform-app", 2, ".NET MAUI, C#, XAML, REST APIs", "MAUI Cross-Platform App" },
                    { 3, "Work Project", "Curo is a healthcare care management platform that replaced a paper-based system used by community carers. Built with Blazor and ASP.NET Core, it gives carers a task-driven workflow on any device and provides care managers with a live dashboard showing real-time visit progress. Hosted on Azure with full audit logging and role-based access control.", "https://github.com/dotnetappdev/curo", "/images/curo.svg", true, null, "Healthcare care management platform", "curo", 3, "ASP.NET Core, Blazor, SQL Server, Entity Framework Core, Azure, MudBlazor, C# .NET 10", "Curo" },
                    { 4, "Healthcare", "A Patient CRM currently in development, designed to help healthcare providers manage patient relationships, appointments, communications, and care history in one centralised platform. Built with .NET 10, Blazor, and a REST API backend.", "https://github.com/dotnetappdev/patient-crm", "/images/patient-crm.png", true, null, "Patient relationship management system (in development)", "patient-crm", 4, "ASP.NET Core .NET 10, Blazor, SQL Server, EF Core, REST API, MudBlazor", "Patient CRM" },
                    { 5, "AI", "An AI assistant integrated into a healthcare platform that helps clinical staff surface relevant patient history, flag anomalies in test results, and draft care plan notes. Built on Semantic Kernel and Azure OpenAI with a strict evaluation layer that confidence-scores every response before it reaches clinical staff. All AI output is fully audited.", "https://github.com/dotnetappdev/ai-diagnostic-assistant", "/images/ai-assistant.png", true, null, "AI-powered clinical decision support tool", "ai-diagnostic-assistant", 5, "Semantic Kernel, Azure OpenAI, ASP.NET Core, Blazor, SQL Server, Vector Search, .NET 10", "AI Diagnostic Assistant" },
                    { 6, "Security", "A reusable security baseline for ASP.NET Core APIs covering JWT authentication with algorithm pinning, OWASP Top Ten mitigations, rate limiting, structured security logging, and automated dependency vulnerability scanning in the CI pipeline. Used as the starting point for all new API projects so that security is built in from the first commit rather than retrofitted.", "https://github.com/dotnetappdev/secure-api-framework", "/images/secure-api.png", true, null, "Hardened API security baseline for .NET", "secure-api-framework", 6, "ASP.NET Core, JWT, OAuth2, OWASP, Rate Limiting, Polly, GitHub Actions", "SecureAPI Framework" },
                    { 7, "Work Project", "TalentConnect is a full-featured recruitment management platform built with Blazor and ASP.NET Core. It streamlines the end-to-end hiring process with job posting management, a configurable multi-stage candidate pipeline, interview scheduling, automated notifications, and detailed recruitment analytics. Built for teams who want a data-driven hiring workflow without the spreadsheets.", "https://github.com/dotnetappdev/talentconnect", "/images/talentconnect.svg", true, null, "A Blazor recruitment management platform", "talentconnect", 7, "Blazor, ASP.NET Core, SQL Server, Entity Framework Core, MudBlazor, REST API, C# .NET 10", "TalentConnect" }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Category", "Name", "Proficiency" },
                values: new object[,]
                {
                    { 1, "Languages", "C#", 98 },
                    { 2, "Frameworks", "ASP.NET Core", 95 },
                    { 3, "Frameworks", "Blazor", 92 },
                    { 4, "Frameworks", ".NET MAUI", 85 },
                    { 5, "Databases", "SQL Server", 90 },
                    { 6, "Databases", "Entity Framework Core", 90 },
                    { 7, "Architecture", "REST API Design", 92 },
                    { 8, "UI Frameworks", "MudBlazor", 88 },
                    { 9, "Cloud", "Azure", 82 },
                    { 10, "Tools", "Git", 90 },
                    { 11, "Languages", "JavaScript", 75 },
                    { 12, "Languages", "HTML/CSS", 85 },
                    { 13, "Frameworks", "WPF", 80 },
                    { 14, "Frameworks", "WinForms", 85 },
                    { 15, "Architecture", "Microservices", 78 },
                    { 16, "AI", "Semantic Kernel", 88 },
                    { 17, "AI", "Azure OpenAI", 85 },
                    { 18, "AI", "ML.NET", 75 },
                    { 19, "AI", "RAG Pipelines", 82 },
                    { 20, "AI", "Vector Search", 78 },
                    { 21, "Security", "OWASP Top Ten", 90 },
                    { 22, "Security", "OAuth2 / OIDC", 88 },
                    { 23, "Security", "JWT Authentication", 92 },
                    { 24, "Security", "Threat Modelling", 80 },
                    { 25, "Security", "Penetration Testing", 72 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
