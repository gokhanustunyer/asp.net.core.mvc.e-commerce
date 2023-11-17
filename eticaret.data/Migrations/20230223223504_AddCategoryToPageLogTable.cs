using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class AddCategoryToPageLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "PageLogs",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_PageLogs_CategoryId",
                table: "PageLogs",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PageLogs_Categories_CategoryId",
                table: "PageLogs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageLogs_Categories_CategoryId",
                table: "PageLogs");

            migrationBuilder.DropIndex(
                name: "IX_PageLogs_CategoryId",
                table: "PageLogs");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "PageLogs");
        }
    }
}
