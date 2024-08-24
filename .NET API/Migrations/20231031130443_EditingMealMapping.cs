using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    public partial class EditingMealMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MealTag_MealID",
                table: "MealTag");

            migrationBuilder.DropIndex(
                name: "IX_MealTag_TagID",
                table: "MealTag");

            migrationBuilder.CreateIndex(
                name: "IX_MealTag_MealID",
                table: "MealTag",
                column: "MealID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MealTag_MealID",
                table: "MealTag");

            migrationBuilder.CreateIndex(
                name: "IX_MealTag_MealID",
                table: "MealTag",
                column: "MealID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealTag_TagID",
                table: "MealTag",
                column: "TagID",
                unique: true);
        }
    }
}
