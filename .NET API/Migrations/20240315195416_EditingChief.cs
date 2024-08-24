using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class EditingChief : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiefIngredient_Chiefs_ChiefID",
                table: "ChiefIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_MealOptionIngredient_ChiefIngredient_ChiefID_FoodIngredient",
                table: "MealOptionIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_MealOptionIngredient_MealOptions_MealOptionID",
                table: "MealOptionIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealOptionIngredient",
                table: "MealOptionIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiefIngredient",
                table: "ChiefIngredient");

            migrationBuilder.RenameTable(
                name: "MealOptionIngredient",
                newName: "MealOptionIngredients");

            migrationBuilder.RenameTable(
                name: "ChiefIngredient",
                newName: "ChiefIngredients");

            migrationBuilder.RenameIndex(
                name: "IX_MealOptionIngredient_MealOptionID",
                table: "MealOptionIngredients",
                newName: "IX_MealOptionIngredients_MealOptionID");

            migrationBuilder.RenameIndex(
                name: "IX_MealOptionIngredient_ChiefID_FoodIngredient",
                table: "MealOptionIngredients",
                newName: "IX_MealOptionIngredients_ChiefID_FoodIngredient");

            migrationBuilder.AddColumn<string>(
                name: "ApartmentNumber",
                table: "Chiefs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChiefFullScreenImage",
                table: "Chiefs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Chiefs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Chiefs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FloorNumber",
                table: "Chiefs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HealthCertImage",
                table: "Chiefs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealOptionIngredients",
                table: "MealOptionIngredients",
                columns: new[] { "FoodIngredient", "ChiefID", "MealOptionID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiefIngredients",
                table: "ChiefIngredients",
                columns: new[] { "ChiefID", "FoodIngredient" });

            migrationBuilder.AddForeignKey(
                name: "FK_ChiefIngredients_Chiefs_ChiefID",
                table: "ChiefIngredients",
                column: "ChiefID",
                principalTable: "Chiefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealOptionIngredients_ChiefIngredients_ChiefID_FoodIngredient",
                table: "MealOptionIngredients",
                columns: new[] { "ChiefID", "FoodIngredient" },
                principalTable: "ChiefIngredients",
                principalColumns: new[] { "ChiefID", "FoodIngredient" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealOptionIngredients_MealOptions_MealOptionID",
                table: "MealOptionIngredients",
                column: "MealOptionID",
                principalTable: "MealOptions",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiefIngredients_Chiefs_ChiefID",
                table: "ChiefIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_MealOptionIngredients_ChiefIngredients_ChiefID_FoodIngredient",
                table: "MealOptionIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_MealOptionIngredients_MealOptions_MealOptionID",
                table: "MealOptionIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealOptionIngredients",
                table: "MealOptionIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiefIngredients",
                table: "ChiefIngredients");

            migrationBuilder.DropColumn(
                name: "ApartmentNumber",
                table: "Chiefs");

            migrationBuilder.DropColumn(
                name: "ChiefFullScreenImage",
                table: "Chiefs");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Chiefs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Chiefs");

            migrationBuilder.DropColumn(
                name: "FloorNumber",
                table: "Chiefs");

            migrationBuilder.DropColumn(
                name: "HealthCertImage",
                table: "Chiefs");

            migrationBuilder.RenameTable(
                name: "MealOptionIngredients",
                newName: "MealOptionIngredient");

            migrationBuilder.RenameTable(
                name: "ChiefIngredients",
                newName: "ChiefIngredient");

            migrationBuilder.RenameIndex(
                name: "IX_MealOptionIngredients_MealOptionID",
                table: "MealOptionIngredient",
                newName: "IX_MealOptionIngredient_MealOptionID");

            migrationBuilder.RenameIndex(
                name: "IX_MealOptionIngredients_ChiefID_FoodIngredient",
                table: "MealOptionIngredient",
                newName: "IX_MealOptionIngredient_ChiefID_FoodIngredient");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealOptionIngredient",
                table: "MealOptionIngredient",
                columns: new[] { "FoodIngredient", "ChiefID", "MealOptionID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiefIngredient",
                table: "ChiefIngredient",
                columns: new[] { "ChiefID", "FoodIngredient" });

            migrationBuilder.AddForeignKey(
                name: "FK_ChiefIngredient_Chiefs_ChiefID",
                table: "ChiefIngredient",
                column: "ChiefID",
                principalTable: "Chiefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealOptionIngredient_ChiefIngredient_ChiefID_FoodIngredient",
                table: "MealOptionIngredient",
                columns: new[] { "ChiefID", "FoodIngredient" },
                principalTable: "ChiefIngredient",
                principalColumns: new[] { "ChiefID", "FoodIngredient" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealOptionIngredient_MealOptions_MealOptionID",
                table: "MealOptionIngredient",
                column: "MealOptionID",
                principalTable: "MealOptions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
