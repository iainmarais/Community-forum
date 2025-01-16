using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestApiServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb16012025UpdateDbSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateMarkedForDeletion",
                table: "Posts",
                newName: "DateMarkedForDelete");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateMarkedForDelete",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsImportant",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDelete",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateMarkedForDelete",
                table: "Topics",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsImportant",
                table: "Topics",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDelete",
                table: "Topics",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateMarkedForDelete",
                table: "Threads",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsImportant",
                table: "Threads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDelete",
                table: "Threads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "Requests",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsImportant",
                table: "Posts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateMarkedForDelete",
                table: "GalleryItems",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsImportant",
                table: "GalleryItems",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDelete",
                table: "GalleryItems",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateMarkedForDelete",
                table: "Categories",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsImportant",
                table: "Categories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDelete",
                table: "Categories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateMarkedForDelete",
                table: "Boards",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsImportant",
                table: "Boards",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDelete",
                table: "Boards",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryDescription", "CategoryName", "CreatedByUserId", "DateMarkedForDelete", "IsImportant", "IsMarkedForDelete" },
                values: new object[,]
                {
                    { "1b6f98b0-69be-4a14-947e-ff7a14b6ff3d", "Everything pertaining to computer and IT support can be discussed here.", "Computer and IT Support", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "2344880b-3e84-4e0d-888f-704bd61da294", "Everything pertaining to software development can be discussed here.", "Software development", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "8c14ea71-883b-4e9a-b65a-60d47372fdc3", "General discussions for the forum.", "General", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Description", "RoleName", "RoleType" },
                values: new object[,]
                {
                    { "CommunityManager", "Community managers are trusted community members of the forum.", "Community Manager", "CommunityManager" },
                    { "ContentCreator", "Community members who create high-quality content.", "Content creator", "ContentCreator" }
                });

            migrationBuilder.InsertData(
                table: "SystemPermissions",
                columns: new[] { "SystemPermissionId", "Description", "SystemPermissionName", "SystemPermissionType" },
                values: new object[,]
                {
                    { "cnt_delete_items", "Allows users to delete content", "Content: Delete items", "Content" },
                    { "cnt_post_audio_clips", "Allows users to post audio clips", "Content: Post audio clips", "Content" },
                    { "cnt_post_images", "Allows users to post images", "Content: Post images", "Content" },
                    { "cnt_post_videos", "Allows users to post videos", "Content: Post videos", "Content" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "1b6f98b0-69be-4a14-947e-ff7a14b6ff3d");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "2344880b-3e84-4e0d-888f-704bd61da294");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "8c14ea71-883b-4e9a-b65a-60d47372fdc3");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "CommunityManager");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "ContentCreator");

            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "cnt_delete_items");

            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "cnt_post_audio_clips");

            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "cnt_post_images");

            migrationBuilder.DeleteData(
                table: "SystemPermissions",
                keyColumn: "SystemPermissionId",
                keyValue: "cnt_post_videos");

            migrationBuilder.DropColumn(
                name: "DateMarkedForDelete",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsImportant",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsMarkedForDelete",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DateMarkedForDelete",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "IsImportant",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "IsMarkedForDelete",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "DateMarkedForDelete",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "IsImportant",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "IsMarkedForDelete",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsImportant",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DateMarkedForDelete",
                table: "GalleryItems");

            migrationBuilder.DropColumn(
                name: "IsImportant",
                table: "GalleryItems");

            migrationBuilder.DropColumn(
                name: "IsMarkedForDelete",
                table: "GalleryItems");

            migrationBuilder.DropColumn(
                name: "DateMarkedForDelete",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsImportant",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsMarkedForDelete",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DateMarkedForDelete",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "IsImportant",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "IsMarkedForDelete",
                table: "Boards");

            migrationBuilder.RenameColumn(
                name: "DateMarkedForDelete",
                table: "Posts",
                newName: "DateMarkedForDeletion");
        }
    }
}
