using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcoholic.Migrations
{
    public partial class removerepeatcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sum",
                table: "OrderDetail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sum",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
