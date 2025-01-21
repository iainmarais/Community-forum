using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestApiServer.Database.Migrations
{
    /// <inheritdoc />
    public partial class _21012025UpdateDbv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryDescription", "CategoryName", "CreatedByUserId", "DateMarkedForDelete", "IsImportant", "IsMarkedForDelete" },
                values: new object[,]
                {
                    { "22b80a4f-9234-4f45-8802-5f7ca24a4e21", "Everything pertaining to software development can be discussed here.", "Software development", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "641787e5-d953-4b0c-97e7-5911e1b058b3", "Everything pertaining to computer and IT support can be discussed here.", "Computer and IT Support", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "fc0bc756-87b7-43ed-8015-5f03ec737b9b", "General discussions for the forum.", "General", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "22b80a4f-9234-4f45-8802-5f7ca24a4e21");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "641787e5-d953-4b0c-97e7-5911e1b058b3");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "fc0bc756-87b7-43ed-8015-5f03ec737b9b");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryDescription", "CategoryName", "CreatedByUserId", "DateMarkedForDelete", "IsImportant", "IsMarkedForDelete" },
                values: new object[,]
                {
                    { "506dde4b-8d11-4e1e-a1ba-63e455911c0e", "Everything pertaining to computer and IT support can be discussed here.", "Computer and IT Support", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "de645ee8-b0b1-4102-9fb0-26e709a0fe2c", "Everything pertaining to software development can be discussed here.", "Software development", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "e8c8cfb6-0f34-4549-b57e-dec72584bc50", "General discussions for the forum.", "General", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false }
                });
        }
    }
}
