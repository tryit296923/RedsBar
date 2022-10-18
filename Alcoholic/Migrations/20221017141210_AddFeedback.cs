using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcoholic.Migrations
{
    public partial class AddFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    FeedbackName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    Environment = table.Column<int>(type: "int", nullable: false),
                    Serve = table.Column<int>(type: "int", nullable: false),
                    Dish = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Overall = table.Column<int>(type: "int", nullable: false),
                    Suggestion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Feedback_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedback");
        }
    }
}
