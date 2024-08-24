using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class EditingSideDishAndMealTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FreeSideDishOption");

            migrationBuilder.DropTable(
                name: "MealTag");

            migrationBuilder.DropTable(
                name: "FreeSideDishes");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.CreateTable(
                name: "MealSideDishes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsFree = table.Column<bool>(type: "bit", nullable: false),
                    IsTopping = table.Column<bool>(type: "bit", nullable: false),
                    MealOptionID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealSideDishes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MealSideDishes_MealOptions_MealOptionID",
                        column: x => x.MealOptionID,
                        principalTable: "MealOptions",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "MealTags",
                columns: table => new
                {
                    MealID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tag = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTags", x => new { x.MealID, x.Tag });
                    table.ForeignKey(
                        name: "FK_MealTags_Meals_MealID",
                        column: x => x.MealID,
                        principalTable: "Meals",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SideDishes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChiefID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SideDishes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SideDishes_Chiefs_ChiefID",
                        column: x => x.ChiefID,
                        principalTable: "Chiefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SideDishOptions",
                columns: table => new
                {
                    SideDishID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SideDishSizeOption = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MealSideDishID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SideDishOptions", x => new { x.SideDishID, x.SideDishSizeOption });
                    table.ForeignKey(
                        name: "FK_SideDishOptions_MealSideDishes_MealSideDishID",
                        column: x => x.MealSideDishID,
                        principalTable: "MealSideDishes",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_SideDishOptions_SideDishes_SideDishID",
                        column: x => x.SideDishID,
                        principalTable: "SideDishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectedSideDish",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MealOptionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SideDishID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SideDishSizeOption = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedSideDish", x => new { x.UserID, x.MealOptionID, x.SideDishID });
                    table.ForeignKey(
                        name: "FK_SelectedSideDish_Cart_UserID_MealOptionID",
                        columns: x => new { x.UserID, x.MealOptionID },
                        principalTable: "Cart",
                        principalColumns: new[] { "UserID", "MealOptionID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedSideDish_SideDishOptions_SideDishID_SideDishSizeOption",
                        columns: x => new { x.SideDishID, x.SideDishSizeOption },
                        principalTable: "SideDishOptions",
                        principalColumns: new[] { "SideDishID", "SideDishSizeOption" },
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealSideDishes_MealOptionID",
                table: "MealSideDishes",
                column: "MealOptionID");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedSideDish_SideDishID_SideDishSizeOption",
                table: "SelectedSideDish",
                columns: new[] { "SideDishID", "SideDishSizeOption" });

            migrationBuilder.CreateIndex(
                name: "IX_SideDishes_ChiefID",
                table: "SideDishes",
                column: "ChiefID");

            migrationBuilder.CreateIndex(
                name: "IX_SideDishOptions_MealSideDishID",
                table: "SideDishOptions",
                column: "MealSideDishID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealTags");

            migrationBuilder.DropTable(
                name: "SelectedSideDish");

            migrationBuilder.DropTable(
                name: "SideDishOptions");

            migrationBuilder.DropTable(
                name: "MealSideDishes");

            migrationBuilder.DropTable(
                name: "SideDishes");

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
                name: "Tags",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.ID);
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

            migrationBuilder.CreateTable(
                name: "MealTag",
                columns: table => new
                {
                    TagID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MealID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTag", x => new { x.TagID, x.MealID });
                    table.ForeignKey(
                        name: "FK_MealTag_Meals_MealID",
                        column: x => x.MealID,
                        principalTable: "Meals",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealTag_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
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

            migrationBuilder.CreateIndex(
                name: "IX_MealTag_MealID",
                table: "MealTag",
                column: "MealID");
        }
    }
}
