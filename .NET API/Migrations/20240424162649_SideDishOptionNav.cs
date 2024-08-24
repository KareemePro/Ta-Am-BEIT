using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class SideDishOptionNav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CostPerKilo",
                table: "ChiefIngredients",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "ChiefIngredients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CartItemOption_MealSideDishOptionID_SideDishSizeOption",
                table: "CartItemOption",
                columns: new[] { "MealSideDishOptionID", "SideDishSizeOption" });

            migrationBuilder.AddForeignKey(
                name: "FK_CartItemOption_SideDishOptions_MealSideDishOptionID_SideDishSizeOption",
                table: "CartItemOption",
                columns: new[] { "MealSideDishOptionID", "SideDishSizeOption" },
                principalTable: "SideDishOptions",
                principalColumns: new[] { "SideDishID", "SideDishSizeOption" },
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItemOption_SideDishOptions_MealSideDishOptionID_SideDishSizeOption",
                table: "CartItemOption");

            migrationBuilder.DropIndex(
                name: "IX_CartItemOption_MealSideDishOptionID_SideDishSizeOption",
                table: "CartItemOption");

            migrationBuilder.DropColumn(
                name: "Visible",
                table: "ChiefIngredients");

            migrationBuilder.AlterColumn<float>(
                name: "CostPerKilo",
                table: "ChiefIngredients",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
