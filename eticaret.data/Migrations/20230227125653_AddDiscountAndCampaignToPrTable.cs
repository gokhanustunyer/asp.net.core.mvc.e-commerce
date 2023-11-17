using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class AddDiscountAndCampaignToPrTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Campaigns_CampaignId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_DiscountCodes_DiscountCodeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CampaignId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_DiscountCodeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "CampaignProduct",
                columns: table => new
                {
                    CampaignsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProductsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignProduct", x => new { x.CampaignsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_CampaignProduct_Campaigns_CampaignsId",
                        column: x => x.CampaignsId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DiscountCodeProduct",
                columns: table => new
                {
                    DiscountCodesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProductsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodeProduct", x => new { x.DiscountCodesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_DiscountCodeProduct_DiscountCodes_DiscountCodesId",
                        column: x => x.DiscountCodesId,
                        principalTable: "DiscountCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountCodeProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignProduct_ProductsId",
                table: "CampaignProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeProduct_ProductsId",
                table: "DiscountCodeProduct",
                column: "ProductsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignProduct");

            migrationBuilder.DropTable(
                name: "DiscountCodeProduct");

            migrationBuilder.AddColumn<Guid>(
                name: "CampaignId",
                table: "Products",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "DiscountCodeId",
                table: "Products",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CampaignId",
                table: "Products",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DiscountCodeId",
                table: "Products",
                column: "DiscountCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Campaigns_CampaignId",
                table: "Products",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_DiscountCodes_DiscountCodeId",
                table: "Products",
                column: "DiscountCodeId",
                principalTable: "DiscountCodes",
                principalColumn: "Id");
        }
    }
}
