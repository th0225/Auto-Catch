using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCatch.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePttSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PttBoards",
                table: "PttSettings",
                newName: "Boards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Boards",
                table: "PttSettings",
                newName: "PttBoards");
        }
    }
}
