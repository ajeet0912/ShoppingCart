using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothBazar.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedPriceInCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ShoppingCarts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 1,
                column: "ExpiryDate",
                value: new DateTime(2024, 11, 15, 13, 27, 5, 245, DateTimeKind.Local).AddTicks(1778));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 2,
                column: "ExpiryDate",
                value: new DateTime(2024, 11, 15, 13, 27, 5, 245, DateTimeKind.Local).AddTicks(1850));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShoppingCarts");

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 1,
                column: "ExpiryDate",
                value: new DateTime(2024, 11, 15, 9, 2, 57, 79, DateTimeKind.Local).AddTicks(2375));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 2,
                column: "ExpiryDate",
                value: new DateTime(2024, 11, 15, 9, 2, 57, 79, DateTimeKind.Local).AddTicks(2396));
        }
    }
}
