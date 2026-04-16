using System;
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
            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false),
                    IsFavorite = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PttSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    BoardConfigs = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PttSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThreadsSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Keywords = table.Column<string>(type: "TEXT", nullable: false),
                    NumPost = table.Column<int>(type: "INTEGER", nullable: false),
                    MinLikes = table.Column<int>(type: "INTEGER", nullable: false),
                    RefreshIntervalMinutes = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadsSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PttSettings",
                columns: new[] { "Id", "BoardConfigs", "Enabled" },
                values: new object[] { 1, "[{\"Name\":\"Lifeismoney\",\"NumPost\":10,\"MinNrec\":0,\"RefreshIntervalMinutes\":30}]", true });

            migrationBuilder.InsertData(
                table: "ThreadsSettings",
                columns: new[] { "Id", "Enabled", "Keywords", "MinLikes", "NumPost", "RefreshIntervalMinutes" },
                values: new object[] { 1, true, "[\"AI\"]", 0, 10, 30 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "PttSettings");

            migrationBuilder.DropTable(
                name: "ThreadsSettings");
        }
    }
}
