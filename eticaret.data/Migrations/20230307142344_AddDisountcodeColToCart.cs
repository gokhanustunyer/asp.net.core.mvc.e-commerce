using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class AddDisountcodeColToCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DiscountCodeId",
                table: "Carts",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_DiscountCodeId",
                table: "Carts",
                column: "DiscountCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_DiscountCodes_DiscountCodeId",
                table: "Carts",
                column: "DiscountCodeId",
                principalTable: "DiscountCodes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_DiscountCodes_DiscountCodeId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_DiscountCodeId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "Carts");
        }
    }
}
