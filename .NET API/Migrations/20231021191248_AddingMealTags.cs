using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    public partial class AddingMealTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealTag_Meals_MealsID",
                table: "MealTag");

            migrationBuilder.DropForeignKey(
                name: "FK_MealTag_Tags_TagsID",
                table: "MealTag");

            migrationBuilder.RenameColumn(
                name: "TagsID",
                table: "MealTag",
                newName: "TagID");

            migrationBuilder.RenameColumn(
                name: "MealsID",
                table: "MealTag",
                newName: "MealID");

            migrationBuilder.RenameIndex(
                name: "IX_MealTag_TagsID",
                table: "MealTag",
                newName: "IX_MealTag_TagID");

            migrationBuilder.AddForeignKey(
                name: "FK_MealTag_Meals_MealID",
                table: "MealTag",
                column: "MealID",
                principalTable: "Meals",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealTag_Tags_TagID",
                table: "MealTag",
                column: "TagID",
                principalTable: "Tags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealTag_Meals_MealID",
                table: "MealTag");

            migrationBuilder.DropForeignKey(
                name: "FK_MealTag_Tags_TagID",
                table: "MealTag");

            migrationBuilder.RenameColumn(
                name: "TagID",
                table: "MealTag",
                newName: "TagsID");

            migrationBuilder.RenameColumn(
                name: "MealID",
                table: "MealTag",
                newName: "MealsID");

            migrationBuilder.RenameIndex(
                name: "IX_MealTag_TagID",
                table: "MealTag",
                newName: "IX_MealTag_TagsID");

            migrationBuilder.AddForeignKey(
                name: "FK_MealTag_Meals_MealsID",
                table: "MealTag",
                column: "MealsID",
                principalTable: "Meals",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealTag_Tags_TagsID",
                table: "MealTag",
                column: "TagsID",
                principalTable: "Tags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
