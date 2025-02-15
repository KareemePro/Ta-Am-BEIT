﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class EditingCheifGovernmentID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GovernmentID",
                table: "Chiefs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GovernmentID",
                table: "Chiefs",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
