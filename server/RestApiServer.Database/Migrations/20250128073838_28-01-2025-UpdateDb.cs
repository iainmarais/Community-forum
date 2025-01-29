using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestApiServer.Database.Migrations
{
    /// <inheritdoc />
    public partial class _28012025UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.RenameColumn(
                name: "SupportRequestTitle",
                table: "Requests",
                newName: "ServiceRequestTitle");

            migrationBuilder.RenameColumn(
                name: "SupportRequestContent",
                table: "Requests",
                newName: "ServiceRequestContent");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryDescription", "CategoryName", "CreatedByUserId", "DateMarkedForDelete", "IsImportant", "IsMarkedForDelete" },
                values: new object[,]
                {
                    { "a2368d0d-49fc-4c3a-bf97-06f241a2f7fe", "Everything pertaining to software development can be discussed here.", "Software development", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "f8d259a1-4a62-4041-9f1d-e753c5f84b64", "General discussions for the forum.", "General", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "fed5577d-c0fb-4675-b67b-eececfd5e3cc", "Everything pertaining to computer and IT support can be discussed here.", "Computer and IT Support", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "a2368d0d-49fc-4c3a-bf97-06f241a2f7fe");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "f8d259a1-4a62-4041-9f1d-e753c5f84b64");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "fed5577d-c0fb-4675-b67b-eececfd5e3cc");

            migrationBuilder.RenameColumn(
                name: "ServiceRequestTitle",
                table: "Requests",
                newName: "SupportRequestTitle");

            migrationBuilder.RenameColumn(
                name: "ServiceRequestContent",
                table: "Requests",
                newName: "SupportRequestContent");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryDescription", "CategoryName", "CreatedByUserId", "DateMarkedForDelete", "IsImportant", "IsMarkedForDelete" },
                values: new object[,]
                {
                    { "15ecfd4d-17ef-41f6-8af0-9b0d31aa1fa6", "Everything pertaining to software development can be discussed here.", "Software development", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "28574860-0202-439e-8b92-ae1b6a49145d", "Everything pertaining to computer and IT support can be discussed here.", "Computer and IT Support", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { "356d0f1d-0cfd-44e2-8443-319316d3ae1f", "General discussions for the forum.", "General", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false }
                });
        }
    }
}
