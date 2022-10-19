using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcoholic.Migrations
{
    public partial class changefk_for_feedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Feedback_OrderId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Orders_OrderId",
                table: "Feedback",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Orders_OrderId",
                table: "Feedback");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Feedback_OrderId",
                table: "Orders",
                column: "OrderId",
                principalTable: "Feedback",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
