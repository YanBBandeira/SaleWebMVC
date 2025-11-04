using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class RefitSalesRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SalesRecords_DepartmentId",
                table: "SalesRecords",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesRecords_Departments_DepartmentId",
                table: "SalesRecords",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesRecords_Departments_DepartmentId",
                table: "SalesRecords");

            migrationBuilder.DropIndex(
                name: "IX_SalesRecords_DepartmentId",
                table: "SalesRecords");
        }
    }
}
