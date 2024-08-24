using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class EditingImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewImage",
                table: "MealReviews",
                newName: "ThumbnailImage");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "MealOptions",
                newName: "ThumbnailImage");

            migrationBuilder.AddColumn<string>(
                name: "FullScreenImage",
                table: "MealReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullScreenImage",
                table: "MealOptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullScreenImage",
                table: "MealReviews");

            migrationBuilder.DropColumn(
                name: "FullScreenImage",
                table: "MealOptions");

            migrationBuilder.RenameColumn(
                name: "ThumbnailImage",
                table: "MealReviews",
                newName: "ReviewImage");

            migrationBuilder.RenameColumn(
                name: "ThumbnailImage",
                table: "MealOptions",
                newName: "Image");
        }
    }
}
