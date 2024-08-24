using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AddingSideDishes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FreeSideDishes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MealOptionID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeSideDishes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FreeSideDishes_MealOptions_MealOptionID",
                        column: x => x.MealOptionID,
                        principalTable: "MealOptions",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "FreeSideDishOption",
                columns: table => new
                {
                    FreeSideDishID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MealOptionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeSideDishOption", x => new { x.FreeSideDishID, x.MealOptionID });
                    table.ForeignKey(
                        name: "FK_FreeSideDishOption_FreeSideDishes_FreeSideDishID",
                        column: x => x.FreeSideDishID,
                        principalTable: "FreeSideDishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FreeSideDishOption_MealOptions_MealOptionID",
                        column: x => x.MealOptionID,
                        principalTable: "MealOptions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FreeSideDishes_MealOptionID",
                table: "FreeSideDishes",
                column: "MealOptionID");

            migrationBuilder.CreateIndex(
                name: "IX_FreeSideDishOption_MealOptionID",
                table: "FreeSideDishOption",
                column: "MealOptionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FreeSideDishOption");

            migrationBuilder.DropTable(
                name: "FreeSideDishes");
        }
    }
}
