using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AddingAlternateKeyMealOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_MealOptions_MealSizeOption_MealID",
                table: "MealOptions",
                columns: new[] { "MealSizeOption", "MealID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_MealOptions_MealSizeOption_MealID",
                table: "MealOptions");
        }
    }
}
