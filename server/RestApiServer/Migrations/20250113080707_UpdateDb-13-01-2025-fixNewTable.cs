using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestApiServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb13012025fixNewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportRequests_Users_AssignedToUserId",
                table: "SupportRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportRequests_Users_CreatedByUserId",
                table: "SupportRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportRequests_Users_LastUpdatedByUserId",
                table: "SupportRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportRequests_Users_ResolvedByUserId",
                table: "SupportRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportRequests_Users_AssignedToUserId",
                table: "SupportRequests",
                column: "AssignedToUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportRequests_Users_CreatedByUserId",
                table: "SupportRequests",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportRequests_Users_LastUpdatedByUserId",
                table: "SupportRequests",
                column: "LastUpdatedByUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportRequests_Users_ResolvedByUserId",
                table: "SupportRequests",
                column: "ResolvedByUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportRequests_Users_AssignedToUserId",
                table: "SupportRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportRequests_Users_CreatedByUserId",
                table: "SupportRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportRequests_Users_LastUpdatedByUserId",
                table: "SupportRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportRequests_Users_ResolvedByUserId",
                table: "SupportRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportRequests_Users_AssignedToUserId",
                table: "SupportRequests",
                column: "AssignedToUserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportRequests_Users_CreatedByUserId",
                table: "SupportRequests",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportRequests_Users_LastUpdatedByUserId",
                table: "SupportRequests",
                column: "LastUpdatedByUserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportRequests_Users_ResolvedByUserId",
                table: "SupportRequests",
                column: "ResolvedByUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
