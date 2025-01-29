using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestApiServer.Database.Migrations
{
    /// <inheritdoc />
    public partial class _27012025UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "AssignedToUserId",
                table: "Requests",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Requests",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ResolvedByUserId",
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
                    { "15ecfd4d-17ef-41f6-8af0-9b0d31aa1fa6", "Everything pertaining to software development can be discussed here.", "Software development", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "28574860-0202-439e-8b92-ae1b6a49145d", "Everything pertaining to computer and IT support can be discussed here.", "Computer and IT Support", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "356d0f1d-0cfd-44e2-8443-319316d3ae1f", "General discussions for the forum.", "General", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AssignedToUserId",
                table: "Requests",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CreatedByUserId",
                table: "Requests",
                column: "CreatedByUserId");

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
                name: "FK_Requests_Users_CreatedByUserId",
                table: "Requests",
                column: "CreatedByUserId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_AssignedToUserId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_CreatedByUserId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_ResolvedByUserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_AssignedToUserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_CreatedByUserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ResolvedByUserId",
                table: "Requests");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "15ecfd4d-17ef-41f6-8af0-9b0d31aa1fa6");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "28574860-0202-439e-8b92-ae1b6a49145d");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "356d0f1d-0cfd-44e2-8443-319316d3ae1f");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ResolvedByUserId",
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
    }
}
