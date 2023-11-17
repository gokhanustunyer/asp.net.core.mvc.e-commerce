using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class UpdateCategoryImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCategoryImage");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryImagesId",
                table: "Categories",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryImagesId",
                table: "Categories",
                column: "CategoryImagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Files_CategoryImagesId",
                table: "Categories",
                column: "CategoryImagesId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Files_CategoryImagesId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CategoryImagesId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryImagesId",
                table: "Categories");

            migrationBuilder.CreateTable(
                name: "CategoryCategoryImage",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CategoryImagesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCategoryImage", x => new { x.CategoryId, x.CategoryImagesId });
                    table.ForeignKey(
                        name: "FK_CategoryCategoryImage_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryCategoryImage_Files_CategoryImagesId",
                        column: x => x.CategoryImagesId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCategoryImage_CategoryImagesId",
                table: "CategoryCategoryImage",
                column: "CategoryImagesId");
        }
    }
}
