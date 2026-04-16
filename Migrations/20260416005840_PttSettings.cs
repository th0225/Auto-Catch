using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCatch.Migrations
{
    /// <inheritdoc />
    public partial class PttSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.CreateTable(
                name: "PttSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    PttBoards = table.Column<string>(type: "TEXT", nullable: false),
                    MinNrec = table.Column<int>(type: "INTEGER", nullable: false),
                    RefreshIntervalMinutes = table.Column<int>(type: "INTEGER", nullable: false)
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
                    MinLikes = table.Column<int>(type: "INTEGER", nullable: false),
                    RefreshIntervalMinutes = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadsSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PttSettings");

            migrationBuilder.DropTable(
                name: "ThreadsSettings");

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnablePtt = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnableThreads = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnableX = table.Column<bool>(type: "INTEGER", nullable: false),
                    Keywords = table.Column<string>(type: "TEXT", nullable: false),
                    RefreshIntervalMinutes = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                });
        }
    }
}
