using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestApiServer.Migrations
{
    /// <inheritdoc />
    public partial class updateseedData09102024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateMarkedForDeletion",
                table: "Posts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDelete",
                table: "Posts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "interactive_edit_posts",
                column: "Description",
                value: "Allows a user to edit existing posts. All registered users have such permission, aside from guests, who may only post in authorised areas.");

            migrationBuilder.InsertData(
                table: "SystemPermissions",
                columns: new[] { "SystemPermissionId", "Description", "SystemPermissionName", "SystemPermissionType" },
                values: new object[,]
                {
                    { "interactive_create_boards", "Allows a user to create new boards. All registered users have such permission, aside from guests, who may only post in authorised areas.", "Interactive: Create boards", "Interactivity" },
                    { "interactive_create_threads", "Allows a user to create new threads. All registered users have such permission, aside from guests, who may only post in authorised areas.", "Interactive: Create threads", "Interactivity" },
                    { "interactive_delete_boards", "Allows a user to delete existing boards. All registered users have such permission, aside from guests, who may only post in authorised areas.", "Interactive: Delete boards", "Interactivity" },
                    { "interactive_delete_posts", "Allows a user to delete existing posts. All registered users have such permission, aside from guests, who may only post in authorised areas.", "Interactive: Delete posts", "Interactivity" },
                    { "interactive_delete_threads", "Allows a user to delete existing threads. All registered users have such permission, aside from guests, who may only post in authorised areas.", "Interactive: Delete threads", "Interactivity" },
                    { "interactive_edit_boards", "Allows a user to edit existing boards. All registered users have such permission, aside from guests, who may only post in authorised areas.", "Interactive: Edit boards", "Interactivity" },
                    { "interactive_edit_own_posts", "Allows a user to edit their own posts.", "Interactive: Edit own posts", "Interactivity" },
                    { "interactive_edit_threads", "Allows a user to edit existing threads. All registered users have such permission, aside from guests, who may only post in authorised areas.", "Interactive: Edit threads", "Interactivity" },
                    { "interactive_vote_in_polls", "Allows a user to vote in polls.", "Interactive: Vote in polls", "Interactivity" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "interactive_create_boards");

            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "interactive_create_threads");

            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "interactive_delete_boards");

            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "interactive_delete_posts");

            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "interactive_delete_threads");

            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "interactive_edit_boards");

            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "interactive_edit_own_posts");

            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "interactive_edit_threads");

            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "interactive_vote_in_polls");

            migrationBuilder.DropColumn(
                name: "DateMarkedForDeletion",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsMarkedForDelete",
                table: "Posts");

            migrationBuilder.UpdateData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "interactive_edit_posts",
                column: "Description",
                value: "Allows a user to edit their own posts.");
        }
    }
}
