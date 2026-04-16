using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCatch.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSettingsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PttSettings",
                columns: new[] { "Id", "Boards", "Enabled", "MinNrec", "RefreshIntervalMinutes" },
                values: new object[] { 1, "[\"Lifeismoney\"]", true, 0, 30 });

            migrationBuilder.InsertData(
                table: "ThreadsSettings",
                columns: new[] { "Id", "Enabled", "Keywords", "MinLikes", "RefreshIntervalMinutes" },
                values: new object[] { 1, true, "[\"AI\"]", 0, 30 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PttSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ThreadsSettings",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
