using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shoppingList_backend.Migrations
{
    /// <inheritdoc />
    public partial class UserAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListId",
                schema: "work",
                table: "Recomendation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Recomendation_ListId",
                schema: "work",
                table: "Recomendation",
                column: "ListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recomendation_GroceryList_ListId",
                schema: "work",
                table: "Recomendation",
                column: "ListId",
                principalSchema: "work",
                principalTable: "GroceryList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recomendation_GroceryList_ListId",
                schema: "work",
                table: "Recomendation");

            migrationBuilder.DropIndex(
                name: "IX_Recomendation_ListId",
                schema: "work",
                table: "Recomendation");

            migrationBuilder.DropColumn(
                name: "ListId",
                schema: "work",
                table: "Recomendation");
        }
    }
}
