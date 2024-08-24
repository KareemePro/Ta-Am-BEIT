using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemOptionFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SignupDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemOptions_SideDishID_SideDishSizeOption",
                table: "OrderItemOptions",
                columns: new[] { "SideDishID", "SideDishSizeOption" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemOptions_SideDishOptions_SideDishID_SideDishSizeOption",
                table: "OrderItemOptions",
                columns: new[] { "SideDishID", "SideDishSizeOption" },
                principalTable: "SideDishOptions",
                principalColumns: new[] { "SideDishID", "SideDishSizeOption" },
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemOptions_SideDishOptions_SideDishID_SideDishSizeOption",
                table: "OrderItemOptions");

            migrationBuilder.DropIndex(
                name: "IX_OrderItemOptions_SideDishID_SideDishSizeOption",
                table: "OrderItemOptions");

            migrationBuilder.DropColumn(
                name: "SignupDate",
                table: "AspNetUsers");
        }
    }
}
