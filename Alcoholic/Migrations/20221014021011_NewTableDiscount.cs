using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcoholic.Migrations
{
    public partial class NewTableDiscount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");


            migrationBuilder.AddColumn<int>(
                name: "MemberLevel",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DiscountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountAmount = table.Column<float>(type: "real", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    Qualify = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.DiscountId);
                    table.ForeignKey(
                        name: "FK_Discount_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discount_ProductId",
                table: "Discount",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MemberLevel",
                table: "Members");

            migrationBuilder.AlterColumn<byte[]>(
                name: "MemberPassword",
                table: "Members",
                type: "varbinary(225)",
                maxLength: 225,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(225)",
                oldMaxLength: 225);
        }
    }
}
