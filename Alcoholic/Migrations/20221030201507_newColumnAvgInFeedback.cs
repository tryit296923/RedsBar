using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcoholic.Migrations
{
    public partial class newColumnAvgInFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Join",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 31, 4, 15, 6, 908, DateTimeKind.Local).AddTicks(8984),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 27, 19, 40, 56, 562, DateTimeKind.Local).AddTicks(1954));

            migrationBuilder.AddColumn<int>(
                name: "Average",
                table: "Feedback",
                type: "int",
                nullable: true,
                computedColumnSql: "(([Frequency]+[Environment]+[Serve]+[Dish]+[Price]+[Overall])/6)",
                stored: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Average",
                table: "Feedback");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Join",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 27, 19, 40, 56, 562, DateTimeKind.Local).AddTicks(1954),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 31, 4, 15, 6, 908, DateTimeKind.Local).AddTicks(8984));
        }
    }
}
