using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Portfolio.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHeroStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeroStats");
        }
    }
}
