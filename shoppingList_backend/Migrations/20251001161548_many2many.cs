using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace shoppingList_backend.Migrations
{
    /// <inheritdoc />
    public partial class many2many : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroceryList_Users_Id",
                schema: "work",
                table: "GroceryList");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "work",
                table: "GroceryList",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "GroceryListUser",
                schema: "work",
                columns: table => new
                {
                    SharedGroceryListsId = table.Column<int>(type: "integer", nullable: false),
                    SharedUsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroceryListUser", x => new { x.SharedGroceryListsId, x.SharedUsersId });
                    table.ForeignKey(
                        name: "FK_GroceryListUser_GroceryList_SharedGroceryListsId",
                        column: x => x.SharedGroceryListsId,
                        principalSchema: "work",
                        principalTable: "GroceryList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroceryListUser_Users_SharedUsersId",
                        column: x => x.SharedUsersId,
                        principalSchema: "work",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroceryListUser_SharedUsersId",
                schema: "work",
                table: "GroceryListUser",
                column: "SharedUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroceryListUser",
                schema: "work");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "work",
                table: "GroceryList",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryList_Users_Id",
                schema: "work",
                table: "GroceryList",
                column: "Id",
                principalSchema: "work",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
