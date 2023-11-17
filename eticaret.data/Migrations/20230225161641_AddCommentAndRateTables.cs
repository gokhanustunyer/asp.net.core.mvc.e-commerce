using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eticaret.data.Migrations
{
    public partial class AddCommentAndRateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_AspNetUsers_UserId",
                table: "ProductComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_ProductRate_RateId",
                table: "ProductComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_Products_ProductId",
                table: "ProductComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRate_AspNetUsers_UserId",
                table: "ProductRate");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRate_Products_ProductId",
                table: "ProductRate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRate",
                table: "ProductRate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductComment",
                table: "ProductComment");

            migrationBuilder.RenameTable(
                name: "ProductRate",
                newName: "ProductRates");

            migrationBuilder.RenameTable(
                name: "ProductComment",
                newName: "ProductComments");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRate_UserId",
                table: "ProductRates",
                newName: "IX_ProductRates_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRate_ProductId",
                table: "ProductRates",
                newName: "IX_ProductRates_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComment_UserId",
                table: "ProductComments",
                newName: "IX_ProductComments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComment_RateId",
                table: "ProductComments",
                newName: "IX_ProductComments_RateId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComment_ProductId",
                table: "ProductComments",
                newName: "IX_ProductComments_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProductRates",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRates",
                table: "ProductRates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductComments",
                table: "ProductComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_AspNetUsers_UserId",
                table: "ProductComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_ProductRates_RateId",
                table: "ProductComments",
                column: "RateId",
                principalTable: "ProductRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_Products_ProductId",
                table: "ProductComments",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRates_AspNetUsers_UserId",
                table: "ProductRates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRates_Products_ProductId",
                table: "ProductRates",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_AspNetUsers_UserId",
                table: "ProductComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_ProductRates_RateId",
                table: "ProductComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_Products_ProductId",
                table: "ProductComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRates_AspNetUsers_UserId",
                table: "ProductRates");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRates_Products_ProductId",
                table: "ProductRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRates",
                table: "ProductRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductComments",
                table: "ProductComments");

            migrationBuilder.RenameTable(
                name: "ProductRates",
                newName: "ProductRate");

            migrationBuilder.RenameTable(
                name: "ProductComments",
                newName: "ProductComment");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRates_UserId",
                table: "ProductRate",
                newName: "IX_ProductRate_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRates_ProductId",
                table: "ProductRate",
                newName: "IX_ProductRate_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComments_UserId",
                table: "ProductComment",
                newName: "IX_ProductComment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComments_RateId",
                table: "ProductComment",
                newName: "IX_ProductComment_RateId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComments_ProductId",
                table: "ProductComment",
                newName: "IX_ProductComment_ProductId");

            migrationBuilder.UpdateData(
                table: "ProductRate",
                keyColumn: "UserId",
                keyValue: null,
                column: "UserId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProductRate",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRate",
                table: "ProductRate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductComment",
                table: "ProductComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_AspNetUsers_UserId",
                table: "ProductComment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_ProductRate_RateId",
                table: "ProductComment",
                column: "RateId",
                principalTable: "ProductRate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_Products_ProductId",
                table: "ProductComment",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRate_AspNetUsers_UserId",
                table: "ProductRate",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRate_Products_ProductId",
                table: "ProductRate",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
