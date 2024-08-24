using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class Ingredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealSideDishes_MealOptions_MealOptionID",
                table: "MealSideDishes");

            migrationBuilder.AlterColumn<Guid>(
                name: "MealOptionID",
                table: "MealSideDishes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ChiefIngredient",
                columns: table => new
                {
                    ChiefID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FoodIngredient = table.Column<int>(type: "int", nullable: false),
                    CostPerKilo = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiefIngredient", x => new { x.ChiefID, x.FoodIngredient });
                    table.ForeignKey(
                        name: "FK_ChiefIngredient_Chiefs_ChiefID",
                        column: x => x.ChiefID,
                        principalTable: "Chiefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealOptionIngredient",
                columns: table => new
                {
                    MealOptionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FoodIngredient = table.Column<int>(type: "int", nullable: false),
                    ChiefID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AmountInGrams = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealOptionIngredient", x => new { x.FoodIngredient, x.ChiefID, x.MealOptionID });
                    table.ForeignKey(
                        name: "FK_MealOptionIngredient_ChiefIngredient_ChiefID_FoodIngredient",
                        columns: x => new { x.ChiefID, x.FoodIngredient },
                        principalTable: "ChiefIngredient",
                        principalColumns: new[] { "ChiefID", "FoodIngredient" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealOptionIngredient_MealOptions_MealOptionID",
                        column: x => x.MealOptionID,
                        principalTable: "MealOptions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealOptionIngredient_ChiefID_FoodIngredient",
                table: "MealOptionIngredient",
                columns: new[] { "ChiefID", "FoodIngredient" });

            migrationBuilder.CreateIndex(
                name: "IX_MealOptionIngredient_MealOptionID",
                table: "MealOptionIngredient",
                column: "MealOptionID");

            migrationBuilder.AddForeignKey(
                name: "FK_MealSideDishes_MealOptions_MealOptionID",
                table: "MealSideDishes",
                column: "MealOptionID",
                principalTable: "MealOptions", 
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealSideDishes_MealOptions_MealOptionID",
                table: "MealSideDishes");

            migrationBuilder.DropTable(
                name: "MealOptionIngredient");

            migrationBuilder.DropTable(
                name: "ChiefIngredient");

            migrationBuilder.AlterColumn<Guid>(
                name: "MealOptionID",
                table: "MealSideDishes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_MealSideDishes_MealOptions_MealOptionID",
                table: "MealSideDishes",
                column: "MealOptionID",
                principalTable: "MealOptions",
                principalColumn: "ID");
        }
    }
}
