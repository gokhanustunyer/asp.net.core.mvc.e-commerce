using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class TrySomething : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_AspNetUsers_UserId",
                table: "ProductComment");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProductComment",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_AspNetUsers_UserId",
                table: "ProductComment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_AspNetUsers_UserId",
                table: "ProductComment");

            migrationBuilder.UpdateData(
                table: "ProductComment",
                keyColumn: "UserId",
                keyValue: null,
                column: "UserId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProductComment",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_AspNetUsers_UserId",
                table: "ProductComment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
