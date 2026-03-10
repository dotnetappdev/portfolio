using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogSlugAndFixProjectLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlogSlug",
                table: "Projects",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BlogSlug", "GitHubUrl" },
                values: new object[] { "building-bookit-blazor-booking-management-system", "https://github.com/dotnetappdev/BookIt" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BlogSlug", "Description", "ShortDescription", "TechStack", "Title" },
                values: new object[] { "building-bookit-blazor-booking-management-system", "BookIt Mobile is the .NET MAUI companion to the BookIt platform, targeting iOS, Android, Windows, and macOS from a single codebase. Customers use it to browse services, check live availability, and book or reschedule appointments with real-time confirmations and SMS reminders. Staff get a full daily schedule view, customer check-in, and a built-in POS screen to process card payments and issue digital receipts on the spot. The app supports offline-friendly data caching so carers and staff can keep working even when signal drops, syncing changes automatically when connectivity returns. Push notifications keep everyone up to date on booking changes without polling.", "The BookIt companion app for customers and staff on any device", ".NET MAUI, C#, XAML, REST APIs, MudBlazor, Push Notifications, POS, Offline Cache", "BookIt Mobile (MAUI)" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BlogSlug", "GitHubUrl" },
                values: new object[] { "building-curo-healthcare-care-management-platform", null });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BlogSlug", "GitHubUrl" },
                values: new object[] { null, "https://github.com/dotnetappdev/PatientCrm" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BlogSlug", "GitHubUrl" },
                values: new object[] { "building-ai-into-dotnet-without-losing-your-mind", null });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "BlogSlug", "GitHubUrl" },
                values: new object[] { "owasp-top-ten-is-not-a-checklist-it-is-a-story", null });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "BlogSlug", "GitHubUrl" },
                values: new object[] { "building-talentconnect-blazor-recruitment-platform", "https://github.com/dotnetappdev/rexrutmentportal" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlogSlug",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                column: "GitHubUrl",
                value: "https://github.com/dotnetappdev/bookit");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ShortDescription", "TechStack", "Title" },
                values: new object[] { "A cross-platform mobile and desktop application built with .NET MAUI targeting iOS, Android, Windows, and macOS. Delivers a native experience across all platforms from a single shared codebase, with REST API integration and offline-friendly data handling.", "A .NET MAUI mobile and desktop application", ".NET MAUI, C#, XAML, REST APIs", "MAUI Cross-Platform App" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                column: "GitHubUrl",
                value: "https://github.com/dotnetappdev/curo");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 4,
                column: "GitHubUrl",
                value: "https://github.com/dotnetappdev/patient-crm");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 5,
                column: "GitHubUrl",
                value: "https://github.com/dotnetappdev/ai-diagnostic-assistant");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 6,
                column: "GitHubUrl",
                value: "https://github.com/dotnetappdev/secure-api-framework");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 7,
                column: "GitHubUrl",
                value: "https://github.com/dotnetappdev/talentconnect");
        }
    }
}
