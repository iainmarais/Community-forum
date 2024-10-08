using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestApiServer.Migrations
{
    /// <inheritdoc />
    public partial class updateuserentryurtentry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRefreshTokens_Users_UserId",
                table: "UserRefreshTokens");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserRefreshTokens",
                newName: "AssignedToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRefreshTokens_UserId",
                table: "UserRefreshTokens",
                newName: "IX_UserRefreshTokens_AssignedToUserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserRefreshTokens_Users_AssignedToUserId",
                table: "UserRefreshTokens",
                column: "AssignedToUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRefreshTokens_SystemPermissions_SystemPermissionEntrySys~",
                table: "UserRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRefreshTokens_Users_AssignedToUserId",
                table: "UserRefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserRefreshTokens_SystemPermissionEntrySystemPermissionId",
                table: "UserRefreshTokens");

            migrationBuilder.DropColumn(
                name: "SystemPermissionEntrySystemPermissionId",
                table: "UserRefreshTokens");

            migrationBuilder.RenameColumn(
                name: "AssignedToUserId",
                table: "UserRefreshTokens",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRefreshTokens_AssignedToUserId",
                table: "UserRefreshTokens",
                newName: "IX_UserRefreshTokens_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRefreshTokens_Users_UserId",
                table: "UserRefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
