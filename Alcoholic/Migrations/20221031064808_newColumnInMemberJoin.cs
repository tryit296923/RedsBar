using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcoholic.Migrations
{
    public partial class newColumnInMemberJoin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Join",
                table: "Members",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "(GETDATE())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Join",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 31, 14, 48, 8, 23, DateTimeKind.Local).AddTicks(6750),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 31, 4, 15, 6, 908, DateTimeKind.Local).AddTicks(8984));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Join",
                table: "Members");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Join",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 31, 4, 15, 6, 908, DateTimeKind.Local).AddTicks(8984),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 31, 14, 48, 8, 23, DateTimeKind.Local).AddTicks(6750));
        }
    }
}
