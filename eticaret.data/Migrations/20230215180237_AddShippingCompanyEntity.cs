using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class AddShippingCompanyEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ShippingCompanyId",
                table: "Orders",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "ShippingCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<double>(type: "double", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingCompanies", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ShippingShippingImage",
                columns: table => new
                {
                    ShippingImagesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ShippingsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingShippingImage", x => new { x.ShippingImagesId, x.ShippingsId });
                    table.ForeignKey(
                        name: "FK_ShippingShippingImage_Files_ShippingImagesId",
                        column: x => x.ShippingImagesId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShippingShippingImage_ShippingCompanies_ShippingsId",
                        column: x => x.ShippingsId,
                        principalTable: "ShippingCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingCompanyId",
                table: "Orders",
                column: "ShippingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingShippingImage_ShippingsId",
                table: "ShippingShippingImage",
                column: "ShippingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingCompanies_ShippingCompanyId",
                table: "Orders",
                column: "ShippingCompanyId",
                principalTable: "ShippingCompanies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingCompanies_ShippingCompanyId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ShippingShippingImage");

            migrationBuilder.DropTable(
                name: "ShippingCompanies");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShippingCompanyId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingCompanyId",
                table: "Orders");
        }
    }
}
