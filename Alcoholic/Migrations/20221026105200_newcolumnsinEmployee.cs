using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcoholic.Migrations
{
    public partial class newcolumnsinEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Salary",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 30000);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Employee");
        }
    }
}
