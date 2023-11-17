using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class AddCampaignsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CampaignId",
                table: "Products",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "CampaignId",
                table: "Categories",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DiscountRate = table.Column<int>(type: "int", nullable: false),
                    CodeStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CodeEndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CampaignId",
                table: "Products",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CampaignId",
                table: "Categories",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Campaigns_CampaignId",
                table: "Categories",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Campaigns_CampaignId",
                table: "Products",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Campaigns_CampaignId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Campaigns_CampaignId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropIndex(
                name: "IX_Products_CampaignId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CampaignId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "Categories");
        }
    }
}
