using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCatch.Migrations
{
    /// <inheritdoc />
    public partial class AddDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CapturedAt",
                table: "Favorites",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Favorites",
                newName: "CapturedAt");
        }
    }
}
