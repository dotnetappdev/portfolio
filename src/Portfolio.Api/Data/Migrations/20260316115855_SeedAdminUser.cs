using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove any runtime-created Admin role/user with different IDs so the
            // deterministic HasData inserts below don't hit duplicate-key conflicts.
            migrationBuilder.Sql("""
                DELETE FROM AspNetUserRoles
                WHERE RoleId IN (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'ADMIN'
                                  AND Id != 'a1b2c3d4-e5f6-7890-abcd-ef1234567890');

                DELETE FROM AspNetRoles
                WHERE NormalizedName = 'ADMIN'
                  AND Id != 'a1b2c3d4-e5f6-7890-abcd-ef1234567890';

                DELETE FROM AspNetUserRoles
                WHERE UserId IN (SELECT Id FROM AspNetUsers
                                  WHERE NormalizedEmail IN ('ADMIN@PORTFOLIO.DOTNETAPPDEVNI.COM','ADMIN@PORTFOLIO.COM')
                                    AND Id != 'b2c3d4e5-f6a7-8901-bcde-f12345678901');

                DELETE FROM AspNetUsers
                WHERE NormalizedEmail IN ('ADMIN@PORTFOLIO.DOTNETAPPDEVNI.COM','ADMIN@PORTFOLIO.COM')
                  AND Id != 'b2c3d4e5-f6a7-8901-bcde-f12345678901';
                """);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a1b2c3d4-e5f6-7890-abcd-ef1234567890", "c3d4e5f6-a7b8-9012-cdef-123456789012", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b2c3d4e5-f6a7-8901-bcde-f12345678901", 0, "e5f6a7b8-c9d0-1234-ef01-345678901234", "admin@portfolio.dotnetappdevni.com", true, "David", "Buckley", true, null, "ADMIN@PORTFOLIO.DOTNETAPPDEVNI.COM", "ADMIN@PORTFOLIO.DOTNETAPPDEVNI.COM", "AQAAAAEAACcQAAAAEDacB/KH29SYZmvFPMtSZ2GtiNSXqIPC+X0riYS+Py3PqkCGmf4dEqyIdDj82UNrkw==", null, false, "d4e5f6a7-b8c9-0123-def0-234567890123", false, "admin@portfolio.dotnetappdevni.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a1b2c3d4-e5f6-7890-abcd-ef1234567890", "b2c3d4e5-f6a7-8901-bcde-f12345678901" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a1b2c3d4-e5f6-7890-abcd-ef1234567890", "b2c3d4e5-f6a7-8901-bcde-f12345678901" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-7890-abcd-ef1234567890");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b2c3d4e5-f6a7-8901-bcde-f12345678901");
        }
    }
}
