using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace shoppingList_backend.Migrations
{
    /// <inheritdoc />
    public partial class rewrited_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "work",
                table: "GroceryList");

            migrationBuilder.DropColumn(
                name: "IsBought",
                schema: "work",
                table: "GroceryList");

            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "work",
                table: "GroceryList");

            migrationBuilder.DropColumn(
                name: "color",
                schema: "work",
                table: "GroceryList");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "work",
                table: "GroceryList",
                newName: "OwnerId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "work",
                table: "GroceryList",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "GroceryItem",
                schema: "work",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ListId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<string>(type: "text", nullable: true),
                    IsBought = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    color = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroceryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroceryItem_GroceryList_ListId",
                        column: x => x.ListId,
                        principalSchema: "work",
                        principalTable: "GroceryList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recomendation",
                schema: "work",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    color = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recomendation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroceryList_OwnerId",
                schema: "work",
                table: "GroceryList",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_GroceryItem_ListId",
                schema: "work",
                table: "GroceryItem",
                column: "ListId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryList_Users_Id",
                schema: "work",
                table: "GroceryList",
                column: "Id",
                principalSchema: "work",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryList_Users_OwnerId",
                schema: "work",
                table: "GroceryList",
                column: "OwnerId",
                principalSchema: "work",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            //TODO: copydata from old Items table to Recomendation table

            migrationBuilder.DropTable(
                    name: "Items",
                    schema: "work");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroceryList_Users_Id",
                schema: "work",
                table: "GroceryList");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryList_Users_OwnerId",
                schema: "work",
                table: "GroceryList");

            migrationBuilder.DropTable(
                name: "GroceryItem",
                schema: "work");

            migrationBuilder.DropTable(
                name: "Recomendation",
                schema: "work");

            migrationBuilder.DropIndex(
                name: "IX_GroceryList_OwnerId",
                schema: "work",
                table: "GroceryList");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                schema: "work",
                table: "GroceryList",
                newName: "UserId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "work",
                table: "GroceryList",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "work",
                table: "GroceryList",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsBought",
                schema: "work",
                table: "GroceryList",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Quantity",
                schema: "work",
                table: "GroceryList",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "color",
                schema: "work",
                table: "GroceryList",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Items",
                schema: "work",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    color = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });
        }
    }
}
