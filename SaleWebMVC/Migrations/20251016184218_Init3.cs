using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<double>(
                name: "BaseSalary",
                table: "AspNetUsers",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "BaseSalary",
                table: "AspNetUsers",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);
        }
    }
}
