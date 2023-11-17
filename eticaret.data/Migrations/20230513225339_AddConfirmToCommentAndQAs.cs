using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class AddConfirmToCommentAndQAs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isConfirmed",
                table: "ProductQAs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isConfirmed",
                table: "ProductComments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isConfirmed",
                table: "ProductQAs");

            migrationBuilder.DropColumn(
                name: "isConfirmed",
                table: "ProductComments");
        }
    }
}
