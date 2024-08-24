using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class RemovingMealSizeOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealOptions_SizeOptions_SizeOptionID",
                table: "MealOptions");

            migrationBuilder.DropTable(
                name: "SizeOptions");

            migrationBuilder.DropIndex(
                name: "IX_MealOptions_SizeOptionID",
                table: "MealOptions");

            migrationBuilder.DropColumn(
                name: "SizeOptionID",
                table: "MealOptions");

            migrationBuilder.AddColumn<int>(
                name: "MealSizeOption",
                table: "MealOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MealSizeOption",
                table: "MealOptions");

            migrationBuilder.AddColumn<Guid>(
                name: "SizeOptionID",
                table: "MealOptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SizeOptions",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SizeOptions", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealOptions_SizeOptionID",
                table: "MealOptions",
                column: "SizeOptionID");

            migrationBuilder.AddForeignKey(
                name: "FK_MealOptions_SizeOptions_SizeOptionID",
                table: "MealOptions",
                column: "SizeOptionID",
                principalTable: "SizeOptions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
