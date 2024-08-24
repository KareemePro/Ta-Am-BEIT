using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    public partial class EditingMealTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MealTag",
                table: "MealTag");

            migrationBuilder.DropIndex(
                name: "IX_MealTag_TagID",
                table: "MealTag");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MealTag",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealTag",
                table: "MealTag",
                columns: new[] { "TagID", "MealID" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MealTag",
                table: "MealTag");

            migrationBuilder.DropIndex(
                name: "IX_MealTag_MealID",
                table: "MealTag");

            migrationBuilder.DropIndex(
                name: "IX_MealTag_TagID",
                table: "MealTag");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MealTag");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealTag",
                table: "MealTag",
                columns: new[] { "MealID", "TagID" });

            migrationBuilder.CreateIndex(
                name: "IX_MealTag_TagID",
                table: "MealTag",
                column: "TagID");
        }
    }
}
