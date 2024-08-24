using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AddPromoCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PromoCode",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(2,2)", nullable: false),
                    MaxDiscount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCode", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPromoCode",
                columns: table => new
                {
                    PromoCodeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubscriptionID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    UsedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPromoCode", x => new { x.PromoCodeID, x.CustomerID });
                    table.ForeignKey(
                        name: "FK_CustomerPromoCode_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerPromoCode_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CustomerPromoCode_PromoCode_PromoCodeID",
                        column: x => x.PromoCodeID,
                        principalTable: "PromoCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerPromoCode_Subscriptions_SubscriptionID",
                        column: x => x.SubscriptionID,
                        principalTable: "Subscriptions",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPromoCode_CustomerID",
                table: "CustomerPromoCode",
                column: "CustomerID");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPromoCode");

            migrationBuilder.DropTable(
                name: "PromoCode");
        }
    }
}
