using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class CorrectSupplierForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // AÇÃO FORÇADA: Remover a Restrição de Chave Estrangeira usando SQL puro
            // A sintaxe para remoção de FK no MySQL é simples
            migrationBuilder.Sql("ALTER TABLE Suppliers DROP FOREIGN KEY FK_Suppliers_State_StateId;");

            // 2. SEGUNDA AÇÃO: Remover a coluna StateId
            migrationBuilder.DropColumn(
                name: "StateId",
                table: "suppliers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // O método Down deve reverter essas ações caso precise voltar atrás.
            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "suppliers",
                nullable: true); // Ajuste 'nullable' se a coluna original não era nula

            // Recria a chave estrangeira (ajuste 'column' e 'principalTable' se necessário)
            migrationBuilder.AddForeignKey(
                name: "'FK_Suppliers_State_StateId'",
                column: "StateId",
                principalTable: "State",
                principalColumn: "Id",
                table: "suppliers");
        }
    }
}
