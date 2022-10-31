using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcoholic.Migrations
{
    public partial class changeFeedbackAverageTypeToDecimalfinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Join",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 31, 17, 43, 33, 241, DateTimeKind.Local).AddTicks(4771),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 31, 17, 36, 28, 459, DateTimeKind.Local).AddTicks(5525));

            migrationBuilder.AlterColumn<decimal>(
                name: "Average",
                table: "Feedback",
                type: "decimal(38,17)",
                nullable: true,
                computedColumnSql: "CAST(([Frequency]+[Environment]+[Serve]+[Dish]+[Price]+[Overall])/CONVERT(decimal(4,2), 6) AS decimal(3,1))",
                stored: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,17)",
                oldNullable: true,
                oldComputedColumnSql: "CAST(([Frequency]+[Environment]+[Serve]+[Dish]+[Price]+[Overall])/CONVERT(decimal(4,2), 6) AS decimal(4,2))",
                oldStored: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Join",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 31, 17, 36, 28, 459, DateTimeKind.Local).AddTicks(5525),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 31, 17, 43, 33, 241, DateTimeKind.Local).AddTicks(4771));

            migrationBuilder.AlterColumn<decimal>(
                name: "Average",
                table: "Feedback",
                type: "decimal(38,17)",
                nullable: true,
                computedColumnSql: "CAST(([Frequency]+[Environment]+[Serve]+[Dish]+[Price]+[Overall])/CONVERT(decimal(4,2), 6) AS decimal(4,2))",
                stored: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,17)",
                oldNullable: true,
                oldComputedColumnSql: "CAST(([Frequency]+[Environment]+[Serve]+[Dish]+[Price]+[Overall])/CONVERT(decimal(4,2), 6) AS decimal(3,1))",
                oldStored: false);
        }
    }
}
