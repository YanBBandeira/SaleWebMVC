using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesRecords_AspNetUsers_SellerId",
                table: "SalesRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesRecords_AspNetUsers_SellerId",
                table: "SalesRecords",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesRecords_AspNetUsers_SellerId",
                table: "SalesRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesRecords_AspNetUsers_SellerId",
                table: "SalesRecords",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
