using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    public partial class EditingChiefAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.Sql("DELETE FROM [Chiefs]", true);

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingID",
                table: "Chiefs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "UserImage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "UserImage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingID",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
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
    }
}
