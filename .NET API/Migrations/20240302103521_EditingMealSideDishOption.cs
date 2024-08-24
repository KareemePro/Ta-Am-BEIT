using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class EditingMealSideDishOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealSideDishOption_SideDishes_SideDishID",
                table: "MealSideDishOption");

            migrationBuilder.CreateIndex(
                name: "IX_MealSideDishOption_SideDishID_SideDishSizeOption",
                table: "MealSideDishOption",
                columns: new[] { "SideDishID", "SideDishSizeOption" });

            migrationBuilder.AddForeignKey(
                name: "FK_MealSideDishOption_SideDishOptions_SideDishID_SideDishSizeOption",
                table: "MealSideDishOption",
                columns: new[] { "SideDishID", "SideDishSizeOption" },
                principalTable: "SideDishOptions",
                principalColumns: new[] { "SideDishID", "SideDishSizeOption" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealSideDishOption_SideDishOptions_SideDishID_SideDishSizeOption",
                table: "MealSideDishOption");

            migrationBuilder.DropIndex(
                name: "IX_MealSideDishOption_SideDishID_SideDishSizeOption",
                table: "MealSideDishOption");

            migrationBuilder.AddForeignKey(
                name: "FK_MealSideDishOption_SideDishes_SideDishID",
                table: "MealSideDishOption",
                column: "SideDishID",
                principalTable: "SideDishes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
