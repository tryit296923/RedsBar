using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcoholic.Migrations
{
    public partial class changeFeedbackAverageTypeToDecimalstep1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Average",
                table: "Feedback",
                type: "decimal(38,17)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComputedColumnSql: "(([Frequency]+[Environment]+[Serve]+[Dish]+[Price]+[Overall])/6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Join",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 31, 15, 43, 35, 851, DateTimeKind.Local).AddTicks(8091),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 31, 14, 48, 8, 23, DateTimeKind.Local).AddTicks(6750));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Join",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 31, 14, 48, 8, 23, DateTimeKind.Local).AddTicks(6750),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 31, 15, 43, 35, 851, DateTimeKind.Local).AddTicks(8091));

            migrationBuilder.AlterColumn<int>(
                name: "Average",
                table: "Feedback",
                type: "int",
                nullable: true,
                computedColumnSql: "(([Frequency]+[Environment]+[Serve]+[Dish]+[Price]+[Overall])/6)",
                stored: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,17)",
                oldNullable: true);
        }
    }
}
