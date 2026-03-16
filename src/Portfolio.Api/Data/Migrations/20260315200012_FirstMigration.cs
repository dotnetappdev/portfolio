using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Portfolio.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiBaseUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GoogleAnalyticsId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VisitorNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    VisitorNotificationEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VisitorEmailTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlogPostImageSlots = table.Column<int>(type: "int", nullable: false),
                    PrimaryColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SecondaryColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TertiaryColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Slug = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadMinutes = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    FeaturedImage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GitHubUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GalleryImages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaItemsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OgImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CanonicalUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CmsPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FeaturedImage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OgImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CanonicalUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmsPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeroStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Label = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroStats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MailSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FromAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FromName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MailerSendApiKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SmtpHost = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    SmtpPort = table.Column<int>(type: "int", nullable: false),
                    SmtpUsername = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SmtpPassword = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UseSsl = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    MediaType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Alt = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    OpenInNewTab = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TechStack = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GitHubUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LiveUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BlogSlug = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Proficiency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AdminReceiverNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TwilioAccountSid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TwilioAuthToken = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TwilioFromNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClickSendUsername = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ClickSendApiKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ClickSendFromName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                table: "BlogPosts",
                columns: new[] { "Id", "Body", "CanonicalUrl", "Category", "FeaturedImage", "GalleryImages", "GitHubUrl", "IsPublished", "MediaItemsJson", "MetaDescription", "MetaTitle", "OgImage", "PublishedDate", "ReadMinutes", "Slug", "Summary", "Tags", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "<h2>What Is BookIt?</h2><p>BookIt is a <strong>multi-tenant, all-in-one booking CRM</strong> built for service businesses - barbers, salons, spas, gyms, B&amp;Bs, and recruitment agencies. It lets businesses accept appointments 24/7, manage their team's availability, take online payments, and stay on top of their customer relationships, all from a single platform.</p><p>The public landing page and pricing tier were built to convert visitors on day one:</p><p><img src=\"https://github.com/user-attachments/assets/d2a6354e-cf3d-437b-8b6a-59a2bad036fb\" alt=\"BookIt Home Page\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><p><img src=\"https://github.com/user-attachments/assets/81aa988c-5fac-40ea-9ef2-a728c31f5dc6\" alt=\"BookIt Pricing Page\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><h2>The Problem We Were Solving</h2><p>Most booking tools in this space are either too expensive for small service businesses or too inflexible for anything beyond a hairdresser. We kept hearing the same complaints from clients: \"we can't add multiple staff\", \"there's no SMS reminder\", \"it doesn't support room-based bookings as well as appointment bookings\". BookIt was built to fix all of those things in one platform.</p><p>The core challenge was <strong>multi-tenancy</strong>. Every business has its own brand, staff, services, and booking rules. We needed a clean tenant isolation model that was still fast enough for real-time availability queries at scale.</p><h2>The Public Booking Flow</h2><p>Customers can book without creating an account - they pick a service, choose a time from a live availability grid, enter their details, and receive a booking confirmation with a unique access PIN. When <code>SendOtpOnBooking</code> is enabled, the PIN is also sent by SMS immediately after the booking is created.</p><p><img src=\"https://github.com/user-attachments/assets/7f728e92-c63b-4ff6-9e69-f2c3077a80c5\" alt=\"BookIt Public Booking Page\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><h2>Authentication Challenges</h2><p>One of the trickiest design decisions was the login flow. We needed a single login page that worked for regular staff <em>and</em> super admins who manage multiple tenants. The solution was a standard email/password form with a \"Super admin?\" toggle that reveals a business slug field - the tenant is resolved from the JWT claims, so regular users never see the slug at all.</p><p><img src=\"https://github.com/user-attachments/assets/ae9a3e71-5cc5-4ca6-9f4b-6b822f8e8888\" alt=\"BookIt Login\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><p><img src=\"https://github.com/user-attachments/assets/2243020a-fcf1-4e85-b5c0-185e985cf3b6\" alt=\"BookIt Super Admin Login\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><h2>The Admin Portal</h2><p>The Blazor + MudBlazor admin portal is where most of the complexity lives. Business owners and their staff get a stat-card dashboard, a calendar view of all upcoming appointments, and deep management screens for services, customers, staff availability, and settings.</p><p><img src=\"https://github.com/user-attachments/assets/a6825129-ca56-44be-a3f7-aed235bcc1b9\" alt=\"BookIt Admin Dashboard\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><p><img src=\"https://github.com/user-attachments/assets/ea02cd3d-fb68-4af3-acf2-a611c8c10442\" alt=\"BookIt Admin Calendar\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><p>Services support photo galleries, per-service online booking toggles, and custom pre-booking question forms - which was a surprisingly complex feature to build correctly on top of a dynamic form builder backed by EF Core JSON columns.</p><p><img src=\"https://github.com/user-attachments/assets/94db25b2-2d28-4502-9fc1-271cd9b2b2ad\" alt=\"BookIt Admin Services\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><h2>Rooms &amp; Lodging</h2><p>We extended the platform beyond simple appointment booking to support B&amp;Bs and lodges that need property and room management, seasonal rates, and amenity tracking. Each property and room has its own inline photo gallery.</p><p><img src=\"https://github.com/user-attachments/assets/1481259d-ada7-4ef3-a8d8-0b04dfd81782\" alt=\"BookIt Rooms &amp; Lodging\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><h2>Booking Invitations &amp; Staff Self-Scheduling</h2><p>Staff can send booking invitations to customers with up to five proposed time slots. Customers click a 14-day shareable link, pick a slot, and their booking is created automatically. Staff availability is managed with a full recurrence engine (Daily, Weekly, BiWeekly, Monthly, Custom) and IANA timezone support.</p><p><img src=\"https://github.com/user-attachments/assets/ba1d9734-3658-49cb-8da6-e544cbb4b64e\" alt=\"BookIt Booking Forms\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><h2>Settings &amp; Notifications</h2><p>Businesses configure their entire booking behaviour, branding, payment gateway, and notification templates from a single settings screen. SMS reminders, email confirmations, and per-notification HTML templates are all there.</p><p><img src=\"https://github.com/user-attachments/assets/f3ac994a-4fd0-4bff-bc30-9cf12d225eb0\" alt=\"BookIt Settings\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><h2>Customers &amp; CRM</h2><p>Every booking creates or updates a customer record. Staff can view the full booking history for any customer, add internal notes, flag VIPs, and track lifetime value. The CRM is scoped to the tenant, so each business sees only its own customers.</p><p><img src=\"https://github.com/user-attachments/assets/d86bf55c-21ed-483a-b032-3c36bba6c99f\" alt=\"BookIt Customers List\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><p><img src=\"https://github.com/user-attachments/assets/b8e4c766-1e0f-4a82-97ec-efd13af1afc1\" alt=\"BookIt Customer Detail\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><h2>Staff Management</h2><p>Business owners can add staff, assign them to services, set their working hours with a full recurrence engine, and block out unavailable periods. Each staff member gets their own calendar view within the portal. Customers can optionally pick a specific staff member when booking.</p><p><img src=\"https://github.com/user-attachments/assets/c3e5a3f2-5a1f-46b9-9fa1-4b5c4e1dde34\" alt=\"BookIt Staff Management\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><h2>Reporting &amp; Analytics</h2><p>The reports section gives business owners a clear picture of their booking performance: total bookings, revenue by service, peak hours, cancellation rates, and staff utilisation. All reports can be filtered by date range and exported to CSV.</p><p><img src=\"https://github.com/user-attachments/assets/f9c2b3a1-0e2a-4f75-8c62-bf8c6c57b6b5\" alt=\"BookIt Reports Dashboard\" style=\"max-width:100%;border-radius:8px;margin:1rem 0;\" /></p><h2>Key Technical Lessons</h2><ul><li><strong>Tenant isolation via EF Core query filters</strong> - global query filters on the DbContext made tenant isolation transparent without polluting every LINQ query with a <code>.Where(x =&gt; x.TenantId == tenantId)</code>.</li><li><strong>Real-time availability</strong> - calculating open slots against staff working windows, existing bookings, and service durations is harder than it looks. We ended up with a dedicated availability engine that caches slot windows per staff per day.</li><li><strong>Blazor component architecture</strong> - splitting each admin section into focused components with clean state management made the MudBlazor UI maintainable as the feature list grew past 30 admin screens.</li><li><strong>SMS reliability</strong> - we integrated Twilio and ClickSend with a provider abstraction so tenants can switch SMS gateway without a code deployment.</li></ul><h2>What We Would Do Differently</h2><p>If we started again, we would introduce an event-sourcing layer earlier for the booking state machine. The current approach uses status columns and audit trail records, which works, but a proper domain event log would make the notification triggers and reporting much cleaner. We would also move the availability engine into a dedicated microservice sooner - it is the most CPU-intensive part of the platform and scales differently from the rest of the CRUD screens.</p>", null, "Projects", "/images/bookit.svg", null, null, true, null, "A deep-dive into building BookIt - a production multi-tenant booking CRM with Blazor, SMS, real-time availability, and a fully responsive MudBlazor admin portal.", "Building BookIt: A Blazor Booking Management System", null, new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), 10, "building-bookit-blazor-booking-management-system", "How we built BookIt - a multi-tenant booking CRM for service businesses - with Blazor, real-time availability, SMS notifications, multi-language support, and a fully responsive MudBlazor UI.", "Blazor, ASP.NET Core, SQL Server, MudBlazor, Multi-Tenant, SMS, Booking", "Building BookIt: A Blazor Booking Management System", null },
                    { 2, "<h2>The Problem</h2><p>Community carers were carrying paper round sheets to each visit - recording notes by hand, signing off tasks with a biro, and handing sheets back to the office at the end of the week. Managers had no visibility of what had happened until those sheets were typed up. If a carer missed a visit, nobody knew until the next morning. <strong>Curo was built to change that.</strong></p><h2>What Curo Does</h2><p>Curo is a care management platform that gives community carers a task-driven workflow on any device. When a carer arrives at a service user&apos;s home, they open Curo, see the care plan tasks for that visit, work through them in order, add notes, and mark the visit complete. The office immediately sees a live dashboard showing which visits are in progress, complete, or overdue.</p><h2>Technology Choices</h2><p>We chose <strong>Blazor with ASP.NET Core</strong> because carers needed to use it on NHS-issued Windows laptops as well as personal Android phones. A Blazor progressive web app gave us a single codebase that worked everywhere without an app store deployment. <strong>MudBlazor</strong> gave us a component library that looked professional enough for office managers without requiring a front-end designer on the project.</p><p>The backend runs on <strong>Azure App Service</strong> behind Azure Front Door for global availability. All data is in <strong>Azure SQL</strong> with row-level security enforcing organisation-level tenant isolation.</p><h2>The Compliance Challenge</h2><p>Healthcare data in the UK sits under CQC and GDPR obligations. Every action - task completion, note edit, visit sign-off - writes an immutable audit record. We spent more time on audit trail design than on almost any other feature. The audit table is append-only at the database level using a check constraint that refuses UPDATE and DELETE operations against it.</p><h2>Role-Based Access Control</h2><p>The platform has four roles: <strong>Carer</strong>, <strong>Senior Carer</strong>, <strong>Care Manager</strong>, and <strong>Administrator</strong>. Carers see only their own schedule and the care plans for the service users they are assigned to. Managers see the whole organisation. The RBAC model is enforced at both the API and the UI layer - a carer who navigates directly to a manager URL gets a 403, not an empty page.</p><h2>Key Technical Lessons</h2><ul><li><strong>Offline-first is hard</strong> - carers often lose mobile signal mid-visit. We cached the visit plan locally using browser storage and synced changes when connectivity returned. The conflict resolution logic for concurrent edits was the hardest part of the whole project.</li><li><strong>Azure Front Door + App Service</strong> is a very good combination for healthcare SaaS - zero-downtime deployments, automatic scaling, and WAF rules all come with minimal ops overhead.</li><li><strong>Entity Framework Core migrations</strong> on a live healthcare database require care. We ran every migration through a staging environment with a production data copy before touching production.</li></ul><h2>Impact</h2><p>Within two months of go-live, the client reported a <strong>35% reduction</strong> in missed visit follow-up calls and managers reclaimed roughly <strong>four hours per week</strong> previously spent processing paper round sheets. The paper round sheets are gone.</p>", null, "Projects", "/images/curo.svg", null, null, true, null, "How Curo replaced a paper-based community care system with a real-time Blazor platform, Azure hosting, and strict compliance controls.", "Building Curo: A Healthcare Care Management Platform", null, new DateTime(2025, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), 8, "building-curo-healthcare-care-management-platform", "Curo replaced a paper-based community care system with a Blazor task-driven workflow, real-time visit tracking, Azure hosting, and strict RBAC and audit logging.", "Blazor, ASP.NET Core, Azure, Healthcare, RBAC, Audit, EF Core", "Building Curo: A Healthcare Care Management Platform", null },
                    { 3, "<h2>Why We Built TalentConnect</h2><p>Recruitment teams were managing candidates in spreadsheets. Column A was the candidate name. Column B was their phone number. Column C was a colour code that meant something different to every recruiter. When the spreadsheet had 400 rows, nobody could find anything. <strong>TalentConnect was built to replace the spreadsheet.</strong></p><h2>Core Features</h2><p>TalentConnect covers the end-to-end hiring lifecycle: create a job posting, build a multi-stage pipeline (Screening, Phone Interview, Technical, Final, Offer, Hired / Rejected), move candidates through stages with a drag-and-drop Kanban board, schedule interviews, send automated notifications, and view analytics on hiring velocity and pipeline conversion.</p><h2>The Pipeline Engine</h2><p>The configurable pipeline was the most complex feature. Each job can have a different set of stages with different automations attached - send an email when a candidate moves to Phone Interview, create a calendar invite when they reach the Technical stage, notify the hiring manager when they hit Offer. We modelled this as a pipeline definition with stage actions, which meant the recruiter could wire up the automations from a UI without any developer involvement.</p><h2>Analytics</h2><p>Every recruiter wants to know: how long does it take us to hire? Where do candidates drop out? Which job boards send the best candidates? TalentConnect tracks candidate source, time-in-stage, and outcome for every application. The analytics dashboard gives hiring managers a real answer to those questions for the first time.</p><h2>Technical Architecture</h2><p>TalentConnect is a <strong>Blazor Server</strong> application with an <strong>ASP.NET Core REST API</strong> backend. We chose Blazor Server over WebAssembly here because the application is used exclusively by internal staff on corporate networks - the real-time SignalR connection gives a snappier feel than a WASM download on first load, and we do not need offline support.</p><p>The database is <strong>SQL Server on Azure</strong> with Entity Framework Core. The pipeline definition and stage action configuration is stored as JSON in EF Core owned entity columns, which kept the schema clean without sacrificing queryability for the analytics layer.</p><h2>What We Learned</h2><ul><li><strong>Drag-and-drop on Blazor</strong> - there is no great drag-and-drop library for Blazor Server. We ended up writing our own using JS interop and a slim JS helper that dispatches custom browser events back into Blazor components. It works well but took longer than expected.</li><li><strong>Email deliverability</strong> - automated interview invitations go to personal inboxes and get caught by spam filters. We moved from SMTP to a transactional email API (MailerSend) and added SPF and DKIM records, which resolved the deliverability issues almost completely.</li><li><strong>Analytics queries on live data</strong> - running aggregation queries against the same database that handles real-time pipeline updates caused lock contention. We added a read replica and directed analytics queries there.</li></ul>", null, "Projects", "/images/talentconnect.svg", null, null, true, null, "A look at how TalentConnect brings job pipeline management, candidate tracking, and hiring analytics to recruitment teams using Blazor and ASP.NET Core.", "Building TalentConnect: A Modern Blazor Recruitment Platform", null, new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc), 9, "building-talentconnect-blazor-recruitment-platform", "TalentConnect is a full-featured recruitment platform with job posting management, a configurable multi-stage candidate pipeline, interview scheduling, and recruitment analytics - all built on Blazor and ASP.NET Core.", "Blazor, Recruitment, Pipeline, Analytics, ASP.NET Core, MudBlazor", "Building TalentConnect: A Modern Blazor Recruitment Platform", null },
                    { 4, "<h2>The Starting Point</h2><p>Adding an AI assistant to a healthcare platform sounds straightforward until you realise the model can confidently tell a nurse that a patient has no known allergies when the allergy record is right there in the database. Getting AI into production in a regulated environment means you cannot just call the OpenAI API, return the response, and call it done.</p><h2>Why Semantic Kernel?</h2><p>We chose <strong>Semantic Kernel</strong> over a raw OpenAI SDK wrapper because it gave us a structured way to compose AI capabilities with existing .NET services. The kernel&apos;s plugin model let us wire up real database queries as tool functions that the model could call - so when a clinician asks &quot;what medication is this patient on?&quot;, the model calls our <code>GetCurrentMedications</code> plugin, which runs a parameterised SQL query and returns structured data, not something retrieved from unstructured training data.</p><h2>The Confidence Scoring Layer</h2><p>Every AI response in the platform goes through a confidence evaluation step before it reaches clinical staff. We built a two-stage pipeline: the primary model generates a response, and a separate evaluator prompt scores the response against the source data on a 0–1 scale. Responses below 0.7 are held back and replaced with a &quot;I need more information to answer that safely&quot; message. This added latency but was non-negotiable for the clinical sign-off.</p><h2>RAG Implementation</h2><p>We use <strong>Retrieval-Augmented Generation (RAG)</strong> to ground the model in patient-specific context. Care notes, assessments, and observations are chunked, embedded using the Azure OpenAI embeddings API, and stored in a <strong>vector index</strong> in Azure AI Search. When a question comes in, the top-k most relevant chunks are retrieved and injected into the system prompt as context. This dramatically reduced hallucinations about patient history.</p><h2>Prompt Engineering Pitfalls</h2><ul><li><strong>System prompts are not magic</strong> - writing &quot;you are a helpful clinical assistant, never make up information&quot; in the system prompt does not stop the model making up information. You need structural guardrails, not just instructional ones.</li><li><strong>Temperature matters more than you think</strong> - in a healthcare context, temperature 0 (deterministic) is almost always right. Any creativity in the output is a liability.</li><li><strong>Token limits creep up on you</strong> - care notes for a long-stay patient can be tens of thousands of tokens. You need a chunking strategy from day one, not as a retrofit.</li></ul><h2>Cost Management</h2><p>Azure OpenAI costs scale with token consumption. Our confidence scoring layer doubled the token usage per query. We offset this by aggressively caching responses for identical queries within a session window and by using <strong>GPT-4o mini</strong> for the evaluator step instead of the full GPT-4o model - the evaluator is a simpler scoring task that does not need the full model.</p><h2>The Lesson</h2><p>AI in production is not an API call - it is a pipeline with guardrails, evaluation steps, fallback behaviour, cost controls, and an audit trail. Build those things before you go live, not after your first clinical incident.</p>", null, "AI", "/images/ai-dotnet.svg", null, null, true, null, "Production lessons from Semantic Kernel and Azure OpenAI in .NET - prompt engineering, confidence scoring, hallucination guards, and the cost of getting it wrong.", "Building AI into .NET Without Losing Your Mind", null, new DateTime(2025, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 12, "building-ai-into-dotnet-without-losing-your-mind", "Production lessons from integrating Semantic Kernel and Azure OpenAI into a .NET 10 application - including prompt engineering pitfalls, confidence scoring, hallucination guards, and the cost of getting it wrong in a healthcare context.", "AI, Semantic Kernel, Azure OpenAI, .NET, RAG, Vector Search, Hallucination", "Building AI into .NET Without Losing Your Mind", null },
                    { 5, "<h2>The Checklist Problem</h2><p>Every developer I have ever worked with has encountered the OWASP Top Ten as a checklist. A security team sends a spreadsheet. Column A lists the ten categories. Column B asks: &quot;Does this apply to our application?&quot; Column C is a checkbox. Someone marks all ten as &quot;Addressed&quot;, the spreadsheet goes back to the security team, and everyone moves on. Three months later, there is a SQL injection vulnerability in the search endpoint.</p><p>The problem is not that OWASP is wrong. The problem is the <strong>checklist mental model</strong>. The OWASP Top Ten is not a list of boxes to tick - it is a narrative about how attackers actually compromise real applications. Reading it as a story rather than a checklist changes everything about how you apply it.</p><h2>A01: Broken Access Control</h2><p>This is number one because it is the most common and the most dangerous. The story here is: <em>an authenticated user accesses something they should not be able to access</em>. In a .NET API, this usually means a missing <code>[Authorize]</code> attribute, an IDOR vulnerability where the user can change an object ID in the URL and get back someone else&apos;s data, or a missing ownership check on a PUT endpoint.</p><p>The fix is not a checklist item - it is a habit. Every endpoint that touches user data needs an explicit check: does the authenticated user own or have permission to access <em>this specific resource</em>?</p><h2>A02: Cryptographic Failures</h2><p>The story: sensitive data is readable by someone who should not be able to read it. In .NET applications, this is most often HTTPS misconfiguration, storing passwords with a weak hashing algorithm (or not hashing them at all), or leaving PII in application logs. <strong>Never log request bodies</strong>. They almost always contain something sensitive.</p><h2>A03: Injection</h2><p>SQL injection is not dead. It is less common than it was because Entity Framework Core parameterises queries by default, but developers still drop into raw SQL for performance-sensitive queries and forget to parameterise. The story: <em>user-controlled input reaches an interpreter without sanitisation</em>. In a .NET context this includes SQL, LDAP, OS commands, and XML queries.</p><h2>A07: Identification and Authentication Failures</h2><p>JWT authentication is ubiquitous in .NET APIs and is a rich source of vulnerabilities. The most common issues I have seen in the wild: algorithm confusion attacks (the API accepts <code>alg: none</code>), short or weak signing keys, tokens that never expire, and missing audience/issuer validation. The fix for all of these is to pin the algorithm explicitly and validate all claims:</p><pre><code>options.TokenValidationParameters = new TokenValidationParameters\n{\n    ValidateIssuer            = true,\n    ValidateAudience          = true,\n    ValidateLifetime          = true,\n    ValidAlgorithms           = new[] { SecurityAlgorithms.HmacSha256 },\n    ValidateIssuerSigningKey  = true,\n    IssuerSigningKey          = new SymmetricSecurityKey(keyBytes)\n};</code></pre><h2>Building the Habit</h2><p>The most effective security change I have made in teams is adding a security review step to the definition of done. Before any PR that touches an authentication flow, an API endpoint, or a data access layer is merged, one of these questions gets asked: <em>who can call this, what can they do, and what stops them doing more than they should?</em></p><p>That question is not on the OWASP Top Ten checklist. But it is what the Top Ten is trying to get you to ask.</p>", null, "Security", "/images/secure-api.svg", null, null, true, null, "Why the OWASP Top Ten is a story about how real applications get compromised, and how to use it to build genuinely more secure .NET APIs.", "The OWASP Top Ten Is Not a Checklist: It Is a Story", null, new DateTime(2025, 7, 12, 0, 0, 0, 0, DateTimeKind.Utc), 10, "owasp-top-ten-is-not-a-checklist-it-is-a-story", "Most teams treat the OWASP Top Ten as a compliance checklist. Here is why that is the wrong mental model, and how to actually use it to build more secure .NET applications.", "OWASP, Security, .NET, ASP.NET Core, JWT, SQL Injection, CORS", "The OWASP Top Ten Is Not a Checklist: It Is a Story", null },
                    { 6, "<h2>The Code I Am Most Proud Of</h2><p>The code I am most proud of writing is code that I have never had to look at again. Code that did its job, was clear enough for whoever maintained it after me to understand without calling me, and was correct enough that bugs were rare and obvious when they appeared. That is a higher bar than it sounds.</p><h2>Lesson 1: Naming Is the Hardest Part</h2><p>After thirty years of reading other people&apos;s code, I am convinced that the quality of a codebase correlates more strongly with the quality of its names than with any other single factor. A method called <code>Process()</code> could do anything. A method called <code>ApplyDiscount(order, customerId)</code> tells you exactly what it does and what it needs to do it. Bad names are a form of technical debt that compounds interest every day the code is read.</p><h2>Lesson 2: Tests Are Documentation</h2><p>Unit tests are the only documentation that cannot go out of date - if the test is wrong, the build fails. A test called <code>ApplyDiscount_WhenCustomerIsVip_Returns20PercentOff</code> tells the next developer exactly what the system is supposed to do in that scenario, in a way that no comment ever can, because the comment will not fail the build if it becomes untrue.</p><p><strong>Test-Driven Development</strong> is not about the tests - it is about the design pressure that writing the test first applies. If your code is hard to test, it is usually hard to understand and maintain too. The test suite is the early warning system.</p><h2>Lesson 3: The Right Time to Refactor Is Before You Add a Feature, Not After</h2><p>Technical debt is not the debt you know about - it is the debt that has become invisible because everyone has worked around it for so long. The moment to address it is when you are about to add a feature that touches the problematic code, because that is when the cost of the debt becomes concrete: you can feel exactly how much harder the messy code is making the new feature to add.</p><h2>Lesson 4: Abstractions Have a Cost</h2><p>Every layer of abstraction you add makes the code more flexible and less readable. Junior developers tend to add too few abstractions. Senior developers sometimes add too many. The discipline is knowing when the flexibility is worth the reading cost, and that judgement only comes with experience. A good heuristic: <strong>add the abstraction when you need it for the second time, not the first</strong>.</p><h2>Lesson 5: Simple Beats Clever</h2><p>The most senior developer in the room is not the one who writes the most complex code. It is the one who takes a complex problem and produces the simplest possible solution. Clever code is easy to write and hard to read. Simple code is hard to write and easy to read. The economics of software development strongly favour the second option.</p><h2>Lesson 6: Security Is Not a Feature, It Is a Foundation</h2><p>In thirty years I have seen security treated as a sprint item more times than I can count. &quot;We will add authentication in the next sprint.&quot; &quot;We will fix the SQL injection in the next release.&quot; Security that is retrofitted is always more expensive, always more fragile, and always has gaps. Build it in from the first commit or pay three times as much to add it later.</p><h2>What Has Not Changed</h2><p>In three decades, the tools, languages, frameworks, and platforms have changed beyond recognition. The fundamentals have not. Clarity over cleverness. Tests as design feedback. Naming as communication. Security as a foundation. These are not .NET lessons or C# lessons - they are software lessons, and they were true before I wrote my first line of code and they will be true after I write my last.</p>", null, ".NET", "/images/dotnet-dev.svg", null, null, true, null, "30 years of .NET development distilled into the principles and habits that separate code written to last from code written to ship.", "What Three Decades of Software Development Taught Me About Writing Code That Lasts", null, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "three-decades-software-development-writing-code-that-lasts", "A personal reflection on the principles, habits, and hard lessons that separate code written to last from code written to ship.", ".NET, Clean Code, Architecture, Career, TDD, Refactoring, Craftsmanship", "What Three Decades of Software Development Taught Me About Writing Code That Lasts", null },
                    { 7, "<h2>The Prototype Trap</h2><p>Building a RAG prototype in .NET takes an afternoon. You chunk a few documents, embed them with the Azure OpenAI embeddings API, store the vectors in a list, do a cosine similarity search, inject the top results into a prompt, and get back a surprisingly good answer. Then you take it to production and everything falls apart.</p><p>The problems are not in the happy path. They are in the edge cases: documents that are too long, queries that match the wrong chunks, indexes that are three weeks out of date, and embedding costs that are ten times higher than you estimated. This post covers the production lessons.</p><h2>Chunking Strategy Matters More Than Anything</h2><p>The most common RAG mistake is using a fixed-size character or token chunker and calling it done. Fixed-size chunking splits documents at arbitrary points, and the semantic content of a chunk often depends on the sentences before and after the split. You retrieve a chunk that says &quot;this is contraindicated in patients with renal impairment&quot; with no context for what &quot;this&quot; refers to because the sentence before the split was in the previous chunk.</p><p>Better chunking strategies for .NET production systems:</p><ul><li><strong>Sentence-boundary chunking</strong> - split on sentence boundaries, not character counts. Use a lightweight NLP library or a simple regex that respects full stops, question marks, and paragraph breaks.</li><li><strong>Semantic chunking</strong> - embed each sentence, then group consecutive sentences whose embeddings are similar. More expensive at index time but produces far more coherent chunks.</li><li><strong>Overlapping chunks</strong> - include the last N tokens of the previous chunk at the start of each new chunk. This preserves context across boundaries at the cost of some redundancy in the index.</li></ul><h2>Embedding Drift</h2><p>Embedding models change. When Azure OpenAI updates the <code>text-embedding-ada-002</code> model or you migrate to <code>text-embedding-3-large</code>, your existing vectors are no longer comparable to new vectors produced by the updated model. Cosine similarity between old and new embeddings is meaningless.</p><p>The fix: version your embedding model in the index. Store the model name and version alongside each vector. When you change models, re-embed everything. This sounds obvious but the re-embedding job is easy to forget until it is urgent.</p><h2>Stale Index Data</h2><p>In production, source documents change. A care plan is updated. A pricing page is rewritten. A policy document is superseded. Your RAG pipeline will confidently answer questions based on the old version for as long as the stale chunks exist in the index.</p><p>The solution is a document fingerprint: hash the source content, store the hash alongside the chunks, and run a background job that re-indexes any document whose hash has changed. In Azure AI Search, use the <code>mergeOrUpload</code> action to update existing documents in place rather than deleting and re-inserting.</p><h2>Query Latency</h2><p>A naive RAG pipeline has two serial latency contributions: the embedding API call (to embed the query) and the vector search call. In Azure AI Search with a small index these are fast, but with a large index and complex hybrid queries (vector + keyword + semantic reranking), the search step alone can take 800ms to 1.5s before you even hit the LLM.</p><p>Optimisations that helped in production:</p><ul><li><strong>Cache query embeddings</strong> - identical or near-identical queries appear far more often than you expect. A short-lived in-memory cache keyed on the normalised query text cuts the embedding call for repeated queries.</li><li><strong>Reduce top-k</strong> - retrieving 20 chunks and injecting all of them into the context window is rarely better than retrieving 5 well-ranked chunks. Fewer chunks means a shorter prompt and lower latency on the LLM call too.</li><li><strong>Semantic reranking only on the final step</strong> - run a fast vector search to get the top 20, then apply the slower semantic reranker only to those 20, not the whole index.</li></ul><h2>The Evaluation Problem</h2><p>How do you know your RAG pipeline is getting better or worse between deployments? Without an evaluation framework, you are guessing. We built a test harness with a golden question set: 50 questions with known correct answers. Before every deployment, the pipeline is run against the golden set and scored on answer correctness, source attribution accuracy, and response latency. A deployment that regresses any of these metrics by more than 5% is blocked.</p><p>Building the evaluation harness was the most valuable thing we did for the long-term health of the system.</p>", null, "AI", "/images/ai-dotnet.svg", null, null, true, null, "What production Retrieval-Augmented Generation actually looks like in .NET - chunking, embedding drift, stale indexes, query latency, and lessons learned.", "RAG Pipelines in .NET: From Prototype to Production", null, new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 11, "rag-pipelines-dotnet-from-prototype-to-production", "Retrieval-Augmented Generation sounds simple until you hit the real problems: chunking strategy, embedding drift, stale index data, and query latency. Here is what production RAG in .NET actually looks like.", "AI, RAG, Semantic Kernel, Azure AI Search, Vector Search, .NET, Embeddings", "RAG Pipelines in .NET: From Prototype to Production", null },
                    { 8, "<h2>Why JWT Gets Misconfigured So Often</h2><p>JWT authentication in ASP.NET Core is easy to add. Call <code>AddJwtBearer</code>, paste in your key, and the middleware handles the rest. That ease of setup is also the problem. Most tutorials show the minimal configuration needed to get a token accepted. They do not show what happens when you accept a token signed with <code>alg: none</code>, or a token with no expiry, or a token signed by a completely different service.</p><p>These are not theoretical vulnerabilities. They show up in real codebases. Here are the ones I see most often.</p><h2>Mistake 1: Not Pinning the Algorithm</h2><p>The JWT header includes an <code>alg</code> claim that tells the recipient which algorithm was used to sign the token. If your API trusts this claim without checking it, an attacker can forge a token signed with <code>alg: none</code> and the standard <code>JwtBearerHandler</code> will accept it.</p><p>The fix is explicit algorithm pinning in <code>TokenValidationParameters</code>:</p><pre><code>options.TokenValidationParameters = new TokenValidationParameters\n{\n    ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },\n    ValidateIssuerSigningKey = true,\n    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)\n};</code></pre><h2>Mistake 2: Not Validating Issuer and Audience</h2><p>By default, <code>ValidateIssuer</code> and <code>ValidateAudience</code> are true in ASP.NET Core, but they are useless unless you also set <code>ValidIssuer</code> and <code>ValidAudience</code>. If you set them to <code>false</code> or leave <code>ValidIssuer</code> empty, a token issued by a completely different service will be accepted by your API.</p><p>This matters in multi-service architectures where several services share the same signing key. Without audience validation, a token issued for Service A is valid for Service B.</p><h2>Mistake 3: Long-Lived Tokens With No Refresh Strategy</h2><p>Tokens that last 30 days are common. They reduce the friction of expiry but they also mean a stolen token is valid for up to 30 days with no way to revoke it. JWTs are stateless by design, so there is no built-in revocation.</p><p>The practical answer is short-lived access tokens (15-60 minutes) combined with a refresh token that is stored server-side and can be revoked. The refresh token is rotated on each use. If a refresh token is used twice, the family is revoked immediately (refresh token reuse detection).</p><h2>Mistake 4: Storing Sensitive Claims in the Token</h2><p>JWTs are base64-encoded, not encrypted. Anyone who gets hold of the token can read every claim in the payload. Do not store PII, permissions matrices, or anything sensitive in a JWT unless you are using JWE (JSON Web Encryption). Store a user ID and role, look everything else up from the database when you need it.</p><h2>Mistake 5: Weak Signing Keys</h2><p>The HMAC-SHA256 algorithm requires a key of at least 256 bits (32 bytes). Using a short or predictable key makes the token brutable offline once an attacker has a sample token. The signing key should be:</p><ul><li>At least 32 bytes of cryptographically random data</li><li>Stored in a secrets manager (Azure Key Vault, AWS Secrets Manager), not in appsettings.json</li><li>Rotated on a schedule, with a grace period for in-flight tokens</li></ul><h2>Mistake 6: Not Logging Authentication Failures</h2><p>A burst of 401 responses from your authentication endpoint is a signal - either a brute-force attempt, a misconfigured client, or a key rotation that was not applied everywhere. If you are not logging authentication failures with enough context (IP, user agent, claimed subject), you have no visibility into what is happening.</p><p>Add structured logging to your JWT events handler:</p><pre><code>options.Events = new JwtBearerEvents\n{\n    OnAuthenticationFailed = ctx =>\n    {\n        logger.LogWarning(\"JWT auth failed: {Error} | IP: {Ip}\",\n            ctx.Exception.Message,\n            ctx.HttpContext.Connection.RemoteIpAddress);\n        return Task.CompletedTask;\n    }\n};</code></pre><h2>The Fast Track</h2><p>If you want a secure baseline without reading the full JWT spec, configure these five things: pin the algorithm, validate issuer and audience, set a short expiry with refresh tokens, use a 32-byte random signing key from a secrets manager, and log authentication failures. That covers the vast majority of real-world JWT vulnerabilities in .NET APIs.</p>", null, "Security", "/images/secure-api.svg", null, null, true, null, "The JWT authentication mistakes that appear in almost every .NET API codebase, and the concrete fixes that actually matter for production security.", "JWT Authentication in .NET: Common Mistakes and How to Fix Them", null, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), 9, "jwt-authentication-dotnet-common-mistakes-and-how-to-fix-them", "JWT is everywhere in .NET APIs and it is consistently misconfigured. Here are the mistakes that show up in almost every codebase, and the fixes that actually matter.", "Security, JWT, ASP.NET Core, Authentication, OAuth2, OWASP, .NET", "JWT Authentication in .NET: Common Mistakes and How to Fix Them", null }
                });

            migrationBuilder.InsertData(
                table: "HeroStats",
                columns: new[] { "Id", "Color", "Label", "SortOrder", "Value" },
                values: new object[,]
                {
                    { 1, "Primary", "Years in .NET", 1, "30+" },
                    { 2, "Secondary", "First Approach", 2, "AI" },
                    { 3, "Error", "Security Built In", 3, "SecOps" },
                    { 4, "Success", "Test-Focused Developer", 4, "TDD/BDD" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "BlogSlug", "Category", "Description", "GitHubUrl", "ImageUrl", "IsFeatured", "LiveUrl", "ShortDescription", "Slug", "SortOrder", "TechStack", "Title" },
                values: new object[,]
                {
                    { 1, "building-bookit-blazor-booking-management-system", "Work Project", "BookIt is a full-featured booking management system built with ASP.NET Core Blazor. Businesses use it to manage appointments, resources, and customer bookings through a modern interface with light and dark mode support. Built on a clean architecture with real-time availability tracking, SMS notifications for customers, and a responsive MudBlazor UI.", "https://github.com/dotnetappdev/BookIt", "/images/bookit.svg", true, null, "A real-time Blazor booking management system", "bookit", 1, "Blazor, ASP.NET Core, SQL Server, Entity Framework Core, MudBlazor, C# .NET 10", "BookIt" },
                    { 2, "building-bookit-blazor-booking-management-system", "Mobile Application", "A cross-platform mobile and desktop application built with .NET MAUI targeting iOS, Android, Windows, and macOS. Delivers a native experience across all platforms from a single shared codebase, with REST API integration and offline-friendly data handling.", "https://github.com/dotnetappdev/BookIt", "/images/maui.png", true, null, "A .NET MAUI mobile and desktop application", "maui-cross-platform-app", 2, ".NET MAUI, C#, XAML, REST APIs", "MAUI Cross-Platform App" },
                    { 3, "building-curo-healthcare-care-management-platform", "Work Project", "Curo is a healthcare care management platform that replaced a paper-based system used by community carers. Built with Blazor and ASP.NET Core, it gives carers a task-driven workflow on any device and provides care managers with a live dashboard showing real-time visit progress. Hosted on Azure with full audit logging and role-based access control.", null, "/images/curo.svg", true, null, "Healthcare care management platform", "curo", 3, "ASP.NET Core, Blazor, SQL Server, Entity Framework Core, Azure, MudBlazor, C# .NET 10", "Curo" },
                    { 4, null, "Healthcare", "A Patient CRM currently in development, designed to help healthcare providers manage patient relationships, appointments, communications, and care history in one centralised platform. Built with .NET 10, Blazor, and a REST API backend.", "https://github.com/dotnetappdev/PatientCrm", "/images/patient-crm.png", true, null, "Patient relationship management system (in development)", "patient-crm", 4, "ASP.NET Core .NET 10, Blazor, SQL Server, EF Core, REST API, MudBlazor", "Patient CRM" },
                    { 5, "building-ai-into-dotnet-without-losing-your-mind", "AI", "An AI assistant integrated into a healthcare platform that helps clinical staff surface relevant patient history, flag anomalies in test results, and draft care plan notes. Built on Semantic Kernel and Azure OpenAI with a strict evaluation layer that confidence-scores every response before it reaches clinical staff. All AI output is fully audited.", null, "/images/ai-assistant.png", true, null, "AI-powered clinical decision support tool", "ai-diagnostic-assistant", 5, "Semantic Kernel, Azure OpenAI, ASP.NET Core, Blazor, SQL Server, Vector Search, .NET 10", "AI Diagnostic Assistant" },
                    { 6, "owasp-top-ten-is-not-a-checklist-it-is-a-story", "Security", "A reusable security baseline for ASP.NET Core APIs covering JWT authentication with algorithm pinning, OWASP Top Ten mitigations, rate limiting, structured security logging, and automated dependency vulnerability scanning in the CI pipeline. Used as the starting point for all new API projects so that security is built in from the first commit rather than retrofitted.", null, "/images/secure-api.png", true, null, "Hardened API security baseline for .NET", "secure-api-framework", 6, "ASP.NET Core, JWT, OAuth2, OWASP, Rate Limiting, Polly, GitHub Actions", "SecureAPI Framework" },
                    { 7, "building-talentconnect-blazor-recruitment-platform", "Work Project", "TalentConnect is a full-featured recruitment management platform built with Blazor and ASP.NET Core. It streamlines the end-to-end hiring process with job posting management, a configurable multi-stage candidate pipeline, interview scheduling, automated notifications, and detailed recruitment analytics. Built for teams who want a data-driven hiring workflow without the spreadsheets.", "https://github.com/dotnetappdev/rexrutmentportal", "/images/talentconnect.svg", true, null, "A Blazor recruitment management platform", "talentconnect", 7, "Blazor, ASP.NET Core, SQL Server, Entity Framework Core, MudBlazor, REST API, C# .NET 10", "TalentConnect" }
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
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettings");

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
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "CmsPages");

            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.DropTable(
                name: "HeroStats");

            migrationBuilder.DropTable(
                name: "MailSettings");

            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "SmsSettings");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
