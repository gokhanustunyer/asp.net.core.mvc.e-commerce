using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class AddProductQa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isHavePhoto",
                table: "ProductComments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ProductQAs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Message = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpperQAId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductQAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductQAs_ProductQAs_UpperQAId",
                        column: x => x.UpperQAId,
                        principalTable: "ProductQAs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductQAs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQAs_ProductId",
                table: "ProductQAs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQAs_UpperQAId",
                table: "ProductQAs",
                column: "UpperQAId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductQAs");

            migrationBuilder.DropColumn(
                name: "isHavePhoto",
                table: "ProductComments");
        }
    }
}
