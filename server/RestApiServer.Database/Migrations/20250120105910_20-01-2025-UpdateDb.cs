using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestApiServer.Database.Migrations
{
    /// <inheritdoc />
    public partial class _20012025UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "0da80bb0-7169-4984-be86-24a060637641");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "5a255d76-04af-4938-9e0c-52bac5e753f4");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "a2e9616e-9689-4877-a41d-fb7bd60c48ca");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateMarkedForDelete",
                table: "Requests",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForDelete",
                table: "Requests",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryDescription", "CategoryName", "CreatedByUserId", "DateMarkedForDelete", "IsImportant", "IsMarkedForDelete" },
                values: new object[,]
                {
                    { "5b74c7fd-dac3-45a3-ab3c-de0f877b6170", "Everything pertaining to software development can be discussed here.", "Software development", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "70f6d2a8-f55b-474f-af88-7fcca957e7dd", "Everything pertaining to computer and IT support can be discussed here.", "Computer and IT Support", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "729eea77-884d-4ece-a20e-0e205ca5e229", "General discussions for the forum.", "General", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "DateMarkedForDelete",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsMarkedForDelete",
                table: "Requests");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryDescription", "CategoryName", "CreatedByUserId", "DateMarkedForDelete", "IsImportant", "IsMarkedForDelete" },
                values: new object[,]
                {
                    { "0da80bb0-7169-4984-be86-24a060637641", "Everything pertaining to computer and IT support can be discussed here.", "Computer and IT Support", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "5a255d76-04af-4938-9e0c-52bac5e753f4", "Everything pertaining to software development can be discussed here.", "Software development", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "a2e9616e-9689-4877-a41d-fb7bd60c48ca", "General discussions for the forum.", "General", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false }
                });
        }
    }
}
