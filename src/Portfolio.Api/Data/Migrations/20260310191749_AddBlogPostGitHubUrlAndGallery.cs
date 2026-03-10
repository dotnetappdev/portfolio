using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogPostGitHubUrlAndGallery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GalleryImages",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHubUrl",
                table: "BlogPosts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "GalleryImages", "GitHubUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "GalleryImages", "GitHubUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "GalleryImages", "GitHubUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "GalleryImages", "GitHubUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "GalleryImages", "GitHubUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "GalleryImages", "GitHubUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ShortDescription", "TechStack", "Title" },
                values: new object[] { "A cross-platform mobile and desktop application built with .NET MAUI targeting iOS, Android, Windows, and macOS. Delivers a native experience across all platforms from a single shared codebase, with REST API integration and offline-friendly data handling.", "A .NET MAUI mobile and desktop application", ".NET MAUI, C#, XAML, REST APIs", "MAUI Cross-Platform App" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GalleryImages",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "GitHubUrl",
                table: "BlogPosts");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ShortDescription", "TechStack", "Title" },
                values: new object[] { "BookIt Mobile is the .NET MAUI companion to the BookIt platform, targeting iOS, Android, Windows, and macOS from a single codebase. Customers use it to browse services, check live availability, and book or reschedule appointments with real-time confirmations and SMS reminders. Staff get a full daily schedule view, customer check-in, and a built-in POS screen to process card payments and issue digital receipts on the spot. The app supports offline-friendly data caching so carers and staff can keep working even when signal drops, syncing changes automatically when connectivity returns. Push notifications keep everyone up to date on booking changes without polling.", "The BookIt companion app for customers and staff on any device", ".NET MAUI, C#, XAML, REST APIs, MudBlazor, Push Notifications, POS, Offline Cache", "BookIt Mobile (MAUI)" });
        }
    }
}
