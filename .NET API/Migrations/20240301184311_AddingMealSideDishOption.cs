using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AddingMealSideDishOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SideDishOptions_MealSideDishes_MealSideDishID",
                table: "SideDishOptions");

            migrationBuilder.DropIndex(
                name: "IX_SideDishOptions_MealSideDishID",
                table: "SideDishOptions");

            migrationBuilder.DropColumn(
                name: "MealSideDishID",
                table: "SideDishOptions");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "SideDishes",
                newName: "ThumbnailImage");

            migrationBuilder.AddColumn<string>(
                name: "FullScreenImage",
                table: "SideDishes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MealSideDishOption",
                columns: table => new
                {
                    MealSideDishID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SideDishID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SideDishSizeOption = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealSideDishOption", x => new { x.SideDishID, x.MealSideDishID, x.SideDishSizeOption });
                    table.ForeignKey(
                        name: "FK_MealSideDishOption_MealSideDishes_MealSideDishID",
                        column: x => x.MealSideDishID,
                        principalTable: "MealSideDishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealSideDishOption_SideDishes_SideDishID",
                        column: x => x.SideDishID,
                        principalTable: "SideDishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealSideDishOption_MealSideDishID",
                table: "MealSideDishOption",
                column: "MealSideDishID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealSideDishOption");

            migrationBuilder.DropColumn(
                name: "FullScreenImage",
                table: "SideDishes");

            migrationBuilder.RenameColumn(
                name: "ThumbnailImage",
                table: "SideDishes",
                newName: "Image");

            migrationBuilder.AddColumn<Guid>(
                name: "MealSideDishID",
                table: "SideDishOptions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SideDishOptions_MealSideDishID",
                table: "SideDishOptions",
                column: "MealSideDishID");

            migrationBuilder.AddForeignKey(
                name: "FK_SideDishOptions_MealSideDishes_MealSideDishID",
                table: "SideDishOptions",
                column: "MealSideDishID",
                principalTable: "MealSideDishes",
                principalColumn: "ID");
        }
    }
}
