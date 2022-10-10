using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcoholic.Migrations
{
    public partial class AddDeskInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Reserf",
                newName: "Reserves");

            migrationBuilder.CreateTable(
                name: "DeskInfo",
                columns: table => new
                {
                    Desk = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeskInfo", x => x.Desk);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeskInfo");

            migrationBuilder.RenameTable(
                name: "Reserves",
                newName: "Reserf");
        }
    }
}
