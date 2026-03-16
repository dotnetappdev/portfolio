using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE AspNetUsers
                SET    Email              = 'admin@portfolio.dotnetappdevni.com',
                       NormalizedEmail    = 'ADMIN@PORTFOLIO.DOTNETAPPDEVNI.COM',
                       UserName           = 'admin@portfolio.dotnetappdevni.com',
                       NormalizedUserName = 'ADMIN@PORTFOLIO.DOTNETAPPDEVNI.COM'
                WHERE  NormalizedEmail    = 'ADMIN@PORTFOLIO.COM'
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE AspNetUsers
                SET    Email              = 'admin@portfolio.com',
                       NormalizedEmail    = 'ADMIN@PORTFOLIO.COM',
                       UserName           = 'admin@portfolio.com',
                       NormalizedUserName = 'ADMIN@PORTFOLIO.COM'
                WHERE  NormalizedEmail    = 'ADMIN@PORTFOLIO.DOTNETAPPDEVNI.COM'
                """);
        }
    }
}
