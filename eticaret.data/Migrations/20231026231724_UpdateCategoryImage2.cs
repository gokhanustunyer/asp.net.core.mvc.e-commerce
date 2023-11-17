using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class UpdateCategoryImage2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Files_CategoryImagesId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "CategoryImagesId",
                table: "Categories",
                newName: "CategoryImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_CategoryImagesId",
                table: "Categories",
                newName: "IX_Categories_CategoryImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Files_CategoryImageId",
                table: "Categories",
                column: "CategoryImageId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Files_CategoryImageId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "CategoryImageId",
                table: "Categories",
                newName: "CategoryImagesId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_CategoryImageId",
                table: "Categories",
                newName: "IX_Categories_CategoryImagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Files_CategoryImagesId",
                table: "Categories",
                column: "CategoryImagesId",
                principalTable: "Files",
                principalColumn: "Id");
        }
    }
}
