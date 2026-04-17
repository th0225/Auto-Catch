using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCatch.Migrations
{
    /// <inheritdoc />
    public partial class AddHideGiveMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PttSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "BoardConfigs",
                value: "[{\"Name\":\"Lifeismoney\",\"NumPost\":10,\"MinNrec\":0,\"HideReplies\":true,\"HideGiveMoney\":true}]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PttSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "BoardConfigs",
                value: "[{\"Name\":\"Lifeismoney\",\"NumPost\":10,\"MinNrec\":0,\"HideReplies\":true}]");
        }
    }
}
