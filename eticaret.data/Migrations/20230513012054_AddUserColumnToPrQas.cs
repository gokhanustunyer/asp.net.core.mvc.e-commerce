using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class AddUserColumnToPrQas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProductQAs",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQAs_UserId",
                table: "ProductQAs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductQAs_AspNetUsers_UserId",
                table: "ProductQAs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductQAs_AspNetUsers_UserId",
                table: "ProductQAs");

            migrationBuilder.DropIndex(
                name: "IX_ProductQAs_UserId",
                table: "ProductQAs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductQAs");
        }
    }
}
