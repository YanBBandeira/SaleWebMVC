using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class SallesRecordsDepartmentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "SalesRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "SalesRecords");
        }
    }
}
