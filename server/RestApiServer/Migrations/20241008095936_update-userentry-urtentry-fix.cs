using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestApiServer.Migrations
{
    /// <inheritdoc />
    public partial class updateuserentryurtentryfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRefreshTokens_SystemPermissions_SystemPermissionEntrySys~",
                table: "UserRefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserRefreshTokens_SystemPermissionEntrySystemPermissionId",
                table: "UserRefreshTokens");

            migrationBuilder.DropColumn(
                name: "SystemPermissionEntrySystemPermissionId",
                table: "UserRefreshTokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SystemPermissionEntrySystemPermissionId",
                table: "UserRefreshTokens",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshTokens_SystemPermissionEntrySystemPermissionId",
                table: "UserRefreshTokens",
                column: "SystemPermissionEntrySystemPermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRefreshTokens_SystemPermissions_SystemPermissionEntrySys~",
                table: "UserRefreshTokens",
                column: "SystemPermissionEntrySystemPermissionId",
                principalTable: "SystemPermissions",
                principalColumn: "SystemPermissionId");
        }
    }
}
