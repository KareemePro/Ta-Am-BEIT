using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AddPromoCodeToSubscriptionAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPromoCode_Orders_OrderID",
                table: "CustomerPromoCode");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPromoCode_Subscriptions_SubscriptionID",
                table: "CustomerPromoCode");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPromoCode_OrderID",
                table: "CustomerPromoCode");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPromoCode_SubscriptionID",
                table: "CustomerPromoCode");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "CustomerPromoCode");

            migrationBuilder.DropColumn(
                name: "SubscriptionID",
                table: "CustomerPromoCode");

            migrationBuilder.AddColumn<string>(
                name: "PromoCodeID",
                table: "Subscriptions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromoCodeID",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUsedByOrder",
                table: "CustomerPromoCode",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PromoCodeID_CustomerID",
                table: "Subscriptions",
                columns: new[] { "PromoCodeID", "CustomerID" },
                unique: true,
                filter: "[PromoCodeID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PromoCodeID_CustomerID",
                table: "Orders",
                columns: new[] { "PromoCodeID", "CustomerID" },
                unique: true,
                filter: "[PromoCodeID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CustomerPromoCode_PromoCodeID_CustomerID",
                table: "Orders",
                columns: new[] { "PromoCodeID", "CustomerID" },
                principalTable: "CustomerPromoCode",
                principalColumns: new[] { "PromoCodeID", "CustomerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_CustomerPromoCode_PromoCodeID_CustomerID",
                table: "Subscriptions",
                columns: new[] { "PromoCodeID", "CustomerID" },
                principalTable: "CustomerPromoCode",
                principalColumns: new[] { "PromoCodeID", "CustomerID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CustomerPromoCode_PromoCodeID_CustomerID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_CustomerPromoCode_PromoCodeID_CustomerID",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_PromoCodeID_CustomerID",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PromoCodeID_CustomerID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PromoCodeID",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "PromoCodeID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsUsedByOrder",
                table: "CustomerPromoCode");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderID",
                table: "CustomerPromoCode",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionID",
                table: "CustomerPromoCode",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPromoCode_OrderID",
                table: "CustomerPromoCode",
                column: "OrderID",
                unique: true,
                filter: "[OrderID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPromoCode_SubscriptionID",
                table: "CustomerPromoCode",
                column: "SubscriptionID",
                unique: true,
                filter: "[SubscriptionID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPromoCode_Orders_OrderID",
                table: "CustomerPromoCode",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPromoCode_Subscriptions_SubscriptionID",
                table: "CustomerPromoCode",
                column: "SubscriptionID",
                principalTable: "Subscriptions",
                principalColumn: "ID");
        }
    }
}
