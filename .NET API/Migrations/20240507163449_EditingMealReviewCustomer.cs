using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class EditingMealReviewCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MealReviews_CustomerID",
                table: "MealReviews");

            migrationBuilder.CreateIndex(
                name: "IX_MealReviews_CustomerID",
                table: "MealReviews",
                column: "CustomerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MealReviews_CustomerID",
                table: "MealReviews");

            migrationBuilder.CreateIndex(
                name: "IX_MealReviews_CustomerID",
                table: "MealReviews",
                column: "CustomerID",
                unique: true);
        }
    }
}
