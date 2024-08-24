using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AddingChiefShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Buildings_BuildingID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Meals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AvailableQuantity",
                table: "MealOptions",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ClosingTime",
                table: "Chiefs",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "OpeningTime",
                table: "Chiefs",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AlterColumn<Guid>(
                name: "BuildingID",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Buildings_BuildingID",
                table: "AspNetUsers",
                column: "BuildingID",
                principalTable: "Buildings",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Buildings_BuildingID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "AvailableQuantity",
                table: "MealOptions");

            migrationBuilder.DropColumn(
                name: "ClosingTime",
                table: "Chiefs");

            migrationBuilder.DropColumn(
                name: "OpeningTime",
                table: "Chiefs");

            migrationBuilder.AlterColumn<Guid>(
                name: "BuildingID",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Buildings_BuildingID",
                table: "AspNetUsers",
                column: "BuildingID",
                principalTable: "Buildings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
