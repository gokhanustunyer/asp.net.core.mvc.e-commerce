using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class AddImageColToPrCommnenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentImageProductComment",
                columns: table => new
                {
                    ImagesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProductCommentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentImageProductComment", x => new { x.ImagesId, x.ProductCommentId });
                    table.ForeignKey(
                        name: "FK_CommentImageProductComment_Files_ImagesId",
                        column: x => x.ImagesId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentImageProductComment_ProductComments_ProductCommentId",
                        column: x => x.ProductCommentId,
                        principalTable: "ProductComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CommentImageProductComment_ProductCommentId",
                table: "CommentImageProductComment",
                column: "ProductCommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentImageProductComment");
        }
    }
}
