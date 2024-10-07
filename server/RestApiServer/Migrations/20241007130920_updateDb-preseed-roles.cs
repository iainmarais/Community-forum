using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestApiServer.Migrations
{
    /// <inheritdoc />
    public partial class updateDbpreseedroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Description", "RoleName", "RoleType" },
                values: new object[,]
                {
                    { "Admin", "Administrators have unrestricted access to administrate the forum and chat services.", "Administrator", "Admin" },
                    { "JuniorModerator", "Moderators are trusted community members of the forum.", "Junior Moderator", "JuniorModerator" },
                    { "Moderator", "Moderators are trusted community members of the forum.", "Moderator", "Moderator" },
                    { "User", "Users have limited rights to the forum, but can create posts and upload content, and edit their own posts.", "Regular User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "Admin");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "JuniorModerator");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "Moderator");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "User");
        }
    }
}
