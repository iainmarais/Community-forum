using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestApiServer.Database.Migrations
{
    /// <inheritdoc />
    public partial class _21012025UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_AssignedToUserId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_LastUpdatedByUserId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_ResolvedByUserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_AssignedToUserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_LastUpdatedByUserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ResolvedByUserId",
                table: "Requests");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "5b74c7fd-dac3-45a3-ab3c-de0f877b6170");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "70f6d2a8-f55b-474f-af88-7fcca957e7dd");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "729eea77-884d-4ece-a20e-0e205ca5e229");

            migrationBuilder.AlterColumn<string>(
                name: "ResolvedByUserId",
                table: "Requests",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastUpdatedByUserId",
                table: "Requests",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedToUserId",
                table: "Requests",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UserEntryUserId",
                table: "Requests",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UserEntryUserId1",
                table: "Requests",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UserEntryUserId2",
                table: "Requests",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryDescription", "CategoryName", "CreatedByUserId", "DateMarkedForDelete", "IsImportant", "IsMarkedForDelete" },
                values: new object[,]
                {
                    { "506dde4b-8d11-4e1e-a1ba-63e455911c0e", "Everything pertaining to computer and IT support can be discussed here.", "Computer and IT Support", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "de645ee8-b0b1-4102-9fb0-26e709a0fe2c", "Everything pertaining to software development can be discussed here.", "Software development", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "e8c8cfb6-0f34-4549-b57e-dec72584bc50", "General discussions for the forum.", "General", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UserEntryUserId",
                table: "Requests",
                column: "UserEntryUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UserEntryUserId1",
                table: "Requests",
                column: "UserEntryUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UserEntryUserId2",
                table: "Requests",
                column: "UserEntryUserId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_UserEntryUserId",
                table: "Requests",
                column: "UserEntryUserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_UserEntryUserId1",
                table: "Requests",
                column: "UserEntryUserId1",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_UserEntryUserId2",
                table: "Requests",
                column: "UserEntryUserId2",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_UserEntryUserId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_UserEntryUserId1",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_UserEntryUserId2",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_UserEntryUserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_UserEntryUserId1",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_UserEntryUserId2",
                table: "Requests");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "506dde4b-8d11-4e1e-a1ba-63e455911c0e");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "de645ee8-b0b1-4102-9fb0-26e709a0fe2c");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "e8c8cfb6-0f34-4549-b57e-dec72584bc50");

            migrationBuilder.DropColumn(
                name: "UserEntryUserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "UserEntryUserId1",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "UserEntryUserId2",
                table: "Requests");

            migrationBuilder.AlterColumn<string>(
                name: "ResolvedByUserId",
                table: "Requests",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastUpdatedByUserId",
                table: "Requests",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedToUserId",
                table: "Requests",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryDescription", "CategoryName", "CreatedByUserId", "DateMarkedForDelete", "IsImportant", "IsMarkedForDelete" },
                values: new object[,]
                {
                    { "5b74c7fd-dac3-45a3-ab3c-de0f877b6170", "Everything pertaining to software development can be discussed here.", "Software development", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "70f6d2a8-f55b-474f-af88-7fcca957e7dd", "Everything pertaining to computer and IT support can be discussed here.", "Computer and IT Support", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "729eea77-884d-4ece-a20e-0e205ca5e229", "General discussions for the forum.", "General", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AssignedToUserId",
                table: "Requests",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_LastUpdatedByUserId",
                table: "Requests",
                column: "LastUpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ResolvedByUserId",
                table: "Requests",
                column: "ResolvedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_AssignedToUserId",
                table: "Requests",
                column: "AssignedToUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_LastUpdatedByUserId",
                table: "Requests",
                column: "LastUpdatedByUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_ResolvedByUserId",
                table: "Requests",
                column: "ResolvedByUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
