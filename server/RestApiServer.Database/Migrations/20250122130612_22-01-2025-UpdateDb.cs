using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestApiServer.Database.Migrations
{
    /// <inheritdoc />
    public partial class _22012025UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_AssignedToUserUserId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_CreatedByUserUserId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_ResolvedByUserUserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_AssignedToUserUserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_CreatedByUserUserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ResolvedByUserUserId",
                table: "Requests");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "8163d986-5eb0-4612-a5f1-3c9856312c47");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "818e95de-e578-4358-8beb-5ce1e42e5c53");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "c2cb1632-5e63-42cb-b9ee-13195854762a");

            migrationBuilder.DropColumn(
                name: "AssignedToUserUserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CreatedByUserUserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ResolvedByUserUserId",
                table: "Requests");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryDescription", "CategoryName", "CreatedByUserId", "DateMarkedForDelete", "IsImportant", "IsMarkedForDelete" },
                values: new object[,]
                {
                    { "29ba1fad-96eb-480f-83ac-4358cd25bbad", "Everything pertaining to computer and IT support can be discussed here.", "Computer and IT Support", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "3c740ffe-3da8-4d93-9b26-de1604199228", "General discussions for the forum.", "General", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "a301d288-a813-421a-8f79-4ff1651f7725", "Everything pertaining to software development can be discussed here.", "Software development", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "29ba1fad-96eb-480f-83ac-4358cd25bbad");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "3c740ffe-3da8-4d93-9b26-de1604199228");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "a301d288-a813-421a-8f79-4ff1651f7725");

            migrationBuilder.AddColumn<string>(
                name: "AssignedToUserUserId",
                table: "Requests",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserUserId",
                table: "Requests",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ResolvedByUserUserId",
                table: "Requests",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryDescription", "CategoryName", "CreatedByUserId", "DateMarkedForDelete", "IsImportant", "IsMarkedForDelete" },
                values: new object[,]
                {
                    { "8163d986-5eb0-4612-a5f1-3c9856312c47", "Everything pertaining to computer and IT support can be discussed here.", "Computer and IT Support", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "818e95de-e578-4358-8beb-5ce1e42e5c53", "Everything pertaining to software development can be discussed here.", "Software development", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "c2cb1632-5e63-42cb-b9ee-13195854762a", "General discussions for the forum.", "General", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AssignedToUserUserId",
                table: "Requests",
                column: "AssignedToUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CreatedByUserUserId",
                table: "Requests",
                column: "CreatedByUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ResolvedByUserUserId",
                table: "Requests",
                column: "ResolvedByUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_AssignedToUserUserId",
                table: "Requests",
                column: "AssignedToUserUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_CreatedByUserUserId",
                table: "Requests",
                column: "CreatedByUserUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_ResolvedByUserUserId",
                table: "Requests",
                column: "ResolvedByUserUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
