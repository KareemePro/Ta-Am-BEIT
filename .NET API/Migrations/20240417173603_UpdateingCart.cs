using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class UpdateingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_MealOptions_MealOptionID",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_SelectedSideDish_Cart_UserID_MealOptionID",
                table: "SelectedSideDish");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cart",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_MealOptionID",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "MealOptionID",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Cart");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeOfDelivery",
                table: "Cart",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AddColumn<bool>(
                name: "DeliverNow",
                table: "Cart",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cart",
                table: "Cart",
                column: "UserID");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MealOptionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CartItems_Cart_UserID",
                        column: x => x.UserID,
                        principalTable: "Cart",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_MealOptions_MealOptionID",
                        column: x => x.MealOptionID,
                        principalTable: "MealOptions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItemOption",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartItemID = table.Column<int>(type: "int", nullable: false),
                    MealSideDishID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MealSideDishOptionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SideDishSizeOption = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItemOption", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CartItemOption_CartItems_CartItemID",
                        column: x => x.CartItemID,
                        principalTable: "CartItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItemOption_MealSideDishes_MealSideDishID",
                        column: x => x.MealSideDishID,
                        principalTable: "MealSideDishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItemOption_CartItemID_MealSideDishID",
                table: "CartItemOption",
                columns: new[] { "CartItemID", "MealSideDishID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItemOption_MealSideDishID",
                table: "CartItemOption",
                column: "MealSideDishID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_MealOptionID",
                table: "CartItems",
                column: "MealOptionID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserID",
                table: "CartItems",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedSideDish_Cart_UserID",
                table: "SelectedSideDish",
                column: "UserID",
                principalTable: "Cart",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectedSideDish_Cart_UserID",
                table: "SelectedSideDish");

            migrationBuilder.DropTable(
                name: "CartItemOption");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cart",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "DeliverNow",
                table: "Cart");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "TimeOfDelivery",
                table: "Cart",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MealOptionID",
                table: "Cart",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cart",
                table: "Cart",
                columns: new[] { "UserID", "MealOptionID" });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_MealOptionID",
                table: "Cart",
                column: "MealOptionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_MealOptions_MealOptionID",
                table: "Cart",
                column: "MealOptionID",
                principalTable: "MealOptions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedSideDish_Cart_UserID_MealOptionID",
                table: "SelectedSideDish",
                columns: new[] { "UserID", "MealOptionID" },
                principalTable: "Cart",
                principalColumns: new[] { "UserID", "MealOptionID" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
