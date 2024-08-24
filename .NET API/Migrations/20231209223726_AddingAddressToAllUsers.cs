using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AddingAddressToAllUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chiefs_Buildings_BuildingID",
                table: "Chiefs");

            migrationBuilder.DropIndex(
                name: "IX_Chiefs_BuildingID",
                table: "Chiefs");

            migrationBuilder.DropColumn(
                name: "BuildingID",
                table: "Chiefs");

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingID",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BuildingID",
                table: "AspNetUsers",
                column: "BuildingID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Buildings_BuildingID",
                table: "AspNetUsers",
                column: "BuildingID",
                principalTable: "Buildings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Buildings_BuildingID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BuildingID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BuildingID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingID",
                table: "Chiefs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Chiefs_BuildingID",
                table: "Chiefs",
                column: "BuildingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Chiefs_Buildings_BuildingID",
                table: "Chiefs",
                column: "BuildingID",
                principalTable: "Buildings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
