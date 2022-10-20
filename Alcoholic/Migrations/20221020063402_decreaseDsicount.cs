using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcoholic.Migrations
{
    public partial class decreaseDsicount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Products",
                table: "Discount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discount_1",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "Qualify",
                table: "Discount");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Discount",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discount_1",
                table: "Discount",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Products_ProductId",
                table: "Discount",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Products_ProductId",
                table: "Discount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discount_1",
                table: "Discount");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Discount",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Frequency",
                table: "Discount",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Qualify",
                table: "Discount",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discount_1",
                table: "Discount",
                columns: new[] { "DiscountId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Products",
                table: "Discount",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
