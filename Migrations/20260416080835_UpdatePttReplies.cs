using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCatch.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePttReplies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RefreshIntervalMinutes",
                table: "PttSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PttSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BoardConfigs", "RefreshIntervalMinutes" },
                values: new object[] { "[{\"Name\":\"Lifeismoney\",\"NumPost\":10,\"MinNrec\":0,\"HideReplies\":true}]", 30 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshIntervalMinutes",
                table: "PttSettings");

            migrationBuilder.UpdateData(
                table: "PttSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "BoardConfigs",
                value: "[{\"Name\":\"Lifeismoney\",\"NumPost\":10,\"MinNrec\":0,\"RefreshIntervalMinutes\":30}]");
        }
    }
}
