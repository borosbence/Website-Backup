using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBackup.WPF.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Websites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    URL = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websites", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Websites",
                columns: new[] { "Id", "Name", "URL" },
                values: new object[] { 1, "TestSite1", null });

            migrationBuilder.InsertData(
                table: "Websites",
                columns: new[] { "Id", "Name", "URL" },
                values: new object[] { 2, "TestSite2", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Websites");
        }
    }
}
