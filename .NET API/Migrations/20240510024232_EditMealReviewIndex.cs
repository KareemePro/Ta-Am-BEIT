using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class EditMealReviewIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MealReviews",
                table: "MealReviews");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealReviews",
                table: "MealReviews",
                columns: new[] { "MealID", "CustomerID" })
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_MealReviews_MealID",
                table: "MealReviews",
                column: "MealID")
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MealReviews",
                table: "MealReviews");

            migrationBuilder.DropIndex(
                name: "IX_MealReviews_MealID",
                table: "MealReviews");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealReviews",
                table: "MealReviews",
                columns: new[] { "MealID", "CustomerID" });
        }
    }
}
