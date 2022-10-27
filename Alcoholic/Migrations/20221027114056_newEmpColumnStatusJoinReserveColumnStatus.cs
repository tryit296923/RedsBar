using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcoholic.Migrations
{
    public partial class newEmpColumnStatusJoinReserveColumnStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Reserves",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "Join",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 27, 19, 40, 56, 562, DateTimeKind.Local).AddTicks(1954));

            migrationBuilder.AddColumn<DateTime>(
                name: "Leave",
                table: "Employee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reserves");

            migrationBuilder.DropColumn(
                name: "Join",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Leave",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Employee");
        }
    }
}
