using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestApiServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb13012025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportRequests",
                columns: table => new
                {
                    SupportRequestId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedByUserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SupportRequestTitle = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SupportRequestContent = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssignedToUserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResolvedByUserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastUpdatedByUserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsResolved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DateResolved = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TriageStatus = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TriageType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportRequests", x => x.SupportRequestId);
                    table.ForeignKey(
                        name: "FK_SupportRequests_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_SupportRequests_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupportRequests_Users_LastUpdatedByUserId",
                        column: x => x.LastUpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_SupportRequests_Users_ResolvedByUserId",
                        column: x => x.ResolvedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_AssignedToUserId",
                table: "SupportRequests",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_CreatedByUserId",
                table: "SupportRequests",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_LastUpdatedByUserId",
                table: "SupportRequests",
                column: "LastUpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_ResolvedByUserId",
                table: "SupportRequests",
                column: "ResolvedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_SupportRequestId",
                table: "SupportRequests",
                column: "SupportRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportRequests");
        }
    }
}
