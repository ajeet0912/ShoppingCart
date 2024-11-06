using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothBazar.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCouponProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FreeItems",
                table: "Coupons",
                newName: "UsageLimit");

            migrationBuilder.RenameColumn(
                name: "DiscountPercentage",
                table: "Coupons",
                newName: "DiscountValue");

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "Coupons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesUsed",
                table: "Coupons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DiscountType", "ExpiryDate", "TimesUsed", "UsageLimit" },
                values: new object[] { 1, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 1000 });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DiscountType", "DiscountValue", "ExpiryDate", "TimesUsed", "UsageLimit" },
                values: new object[] { 2, 0m, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 500 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "TimesUsed",
                table: "Coupons");

            migrationBuilder.RenameColumn(
                name: "UsageLimit",
                table: "Coupons",
                newName: "FreeItems");

            migrationBuilder.RenameColumn(
                name: "DiscountValue",
                table: "Coupons",
                newName: "DiscountPercentage");

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ExpiryDate", "FreeItems" },
                values: new object[] { new DateTime(2024, 11, 15, 13, 27, 5, 245, DateTimeKind.Local).AddTicks(1778), 0 });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DiscountPercentage", "ExpiryDate", "FreeItems" },
                values: new object[] { 10m, new DateTime(2024, 11, 15, 13, 27, 5, 245, DateTimeKind.Local).AddTicks(1850), 0 });
        }
    }
}
