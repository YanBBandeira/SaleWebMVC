using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventoryMovements_AspNetUsers_UserId",
                table: "inventoryMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_inventoryMovements_Products_ProductId",
                table: "inventoryMovements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inventoryMovements",
                table: "inventoryMovements");

            migrationBuilder.RenameTable(
                name: "inventoryMovements",
                newName: "InventoryMovements");

            migrationBuilder.RenameIndex(
                name: "IX_inventoryMovements_UserId",
                table: "InventoryMovements",
                newName: "IX_InventoryMovements_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_inventoryMovements_ProductId",
                table: "InventoryMovements",
                newName: "IX_InventoryMovements_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "InventoryMovements",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Observation",
                table: "InventoryMovements",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryMovements",
                table: "InventoryMovements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovements_AspNetUsers_UserId",
                table: "InventoryMovements",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovements_Products_ProductId",
                table: "InventoryMovements",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovements_AspNetUsers_UserId",
                table: "InventoryMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovements_Products_ProductId",
                table: "InventoryMovements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryMovements",
                table: "InventoryMovements");

            migrationBuilder.RenameTable(
                name: "InventoryMovements",
                newName: "inventoryMovements");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryMovements_UserId",
                table: "inventoryMovements",
                newName: "IX_inventoryMovements_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryMovements_ProductId",
                table: "inventoryMovements",
                newName: "IX_inventoryMovements_ProductId");

            migrationBuilder.UpdateData(
                table: "inventoryMovements",
                keyColumn: "UserId",
                keyValue: null,
                column: "UserId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "inventoryMovements",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "inventoryMovements",
                keyColumn: "Observation",
                keyValue: null,
                column: "Observation",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Observation",
                table: "inventoryMovements",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inventoryMovements",
                table: "inventoryMovements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_inventoryMovements_AspNetUsers_UserId",
                table: "inventoryMovements",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_inventoryMovements_Products_ProductId",
                table: "inventoryMovements",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
