using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMailSettingsAndVisitorNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VisitorEmailTemplate",
                table: "AppSettings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorNotificationEmail",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisitorNotificationsEnabled",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MailSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    SmtpHost = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    SmtpPort = table.Column<int>(type: "INTEGER", nullable: false),
                    SmtpUsername = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    SmtpPassword = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    FromAddress = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    FromName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    UseSsl = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailSettings", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "VisitorEmailTemplate", "VisitorNotificationEmail", "VisitorNotificationsEnabled" },
                values: new object[] { null, null, false });

            migrationBuilder.InsertData(
                table: "MailSettings",
                columns: new[] { "Id", "FromAddress", "FromName", "IsEnabled", "SmtpHost", "SmtpPassword", "SmtpPort", "SmtpUsername", "UseSsl" },
                values: new object[] { 1, null, null, false, null, null, 587, null, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailSettings");

            migrationBuilder.DropColumn(
                name: "VisitorEmailTemplate",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "VisitorNotificationEmail",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "VisitorNotificationsEnabled",
                table: "AppSettings");
        }
    }
}
