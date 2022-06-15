using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBackup.WPF.Migrations
{
    public partial class Connections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "URL",
                table: "Websites",
                newName: "Url");

            migrationBuilder.CreateTable(
                name: "Ftp_Connections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsSSLEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPassive = table.Column<bool>(type: "INTEGER", nullable: false),
                    WebsiteId = table.Column<int>(type: "INTEGER", nullable: false),
                    Hostname = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ftp_Connections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ftp_Connections_Websites_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Websites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sql_Connections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Databasename = table.Column<string>(type: "TEXT", nullable: false),
                    WebsiteId = table.Column<int>(type: "INTEGER", nullable: false),
                    Hostname = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sql_Connections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sql_Connections_Websites_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Websites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Ftp_Connections",
                columns: new[] { "Id", "Hostname", "IsPassive", "IsSSLEnabled", "Password", "Username", "WebsiteId" },
                values: new object[] { 1, "ftp.dlptest.com", false, false, "rNrKYTX9g7z3RgJRmxWuGHbeu", "dlpuser", 1 });

            migrationBuilder.InsertData(
                table: "Sql_Connections",
                columns: new[] { "Id", "Databasename", "Hostname", "Password", "Username", "WebsiteId" },
                values: new object[] { 1, "test", "localhost", "", "root", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Ftp_Connections_WebsiteId",
                table: "Ftp_Connections",
                column: "WebsiteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sql_Connections_WebsiteId",
                table: "Sql_Connections",
                column: "WebsiteId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ftp_Connections");

            migrationBuilder.DropTable(
                name: "Sql_Connections");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Websites",
                newName: "URL");
        }
    }
}
