using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class AddSubjectColToSupportFormTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "SupportMails",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "SupportMails");
        }
    }
}
