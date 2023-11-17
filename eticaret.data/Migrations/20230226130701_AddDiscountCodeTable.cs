using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class AddDiscountCodeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DiscountCodeId",
                table: "Products",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "DiscountCodeId",
                table: "Categories",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "DiscountCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodeLimitNumber = table.Column<int>(type: "int", nullable: false),
                    DiscountRate = table.Column<int>(type: "int", nullable: false),
                    DiscountNumber = table.Column<double>(type: "double", nullable: false),
                    CodeStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CodeEndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DiscountCodeId",
                table: "Products",
                column: "DiscountCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DiscountCodeId",
                table: "Categories",
                column: "DiscountCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_DiscountCodes_DiscountCodeId",
                table: "Categories",
                column: "DiscountCodeId",
                principalTable: "DiscountCodes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_DiscountCodes_DiscountCodeId",
                table: "Products",
                column: "DiscountCodeId",
                principalTable: "DiscountCodes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_DiscountCodes_DiscountCodeId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_DiscountCodes_DiscountCodeId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "DiscountCodes");

            migrationBuilder.DropIndex(
                name: "IX_Products_DiscountCodeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_DiscountCodeId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "Categories");
        }
    }
}
