using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestApiServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseCreateNewForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "GalleryItems",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryItems_CreatedByUserId",
                table: "GalleryItems",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryItems_Users_CreatedByUserId",
                table: "GalleryItems",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GalleryItems_Users_CreatedByUserId",
                table: "GalleryItems");

            migrationBuilder.DropIndex(
                name: "IX_GalleryItems_CreatedByUserId",
                table: "GalleryItems");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "GalleryItems",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
