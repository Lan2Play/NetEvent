using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class AddEventTicketType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "eventFormat",
                table: "Events",
                newName: "EventFormat");

            migrationBuilder.CreateTable(
                name: "EventTicketTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Slug = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    Currency = table.Column<int>(type: "INTEGER", nullable: false),
                    AvailableTickets = table.Column<long>(type: "INTEGER", nullable: false),
                    SellStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SellEndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsGiftable = table.Column<bool>(type: "INTEGER", nullable: false),
                    EventId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTicketTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTicketTypes_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "ClaimType",
                value: "Admin.Images.Write");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 10,
                column: "ClaimType",
                value: "Admin.Events.Write");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 12,
                column: "ClaimType",
                value: "Admin.Venues.Write");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26cbb106-629e-4b0f-b528-03e4fbaf148e", "AQAAAAIAAYagAAAAEMVeCrEoGoqMFSa9mWv07gyORfXVq9Zash81NSc3Z4QmFWadfMKOABkphkaem7Q7nQ==", "56b690d3-1bb2-4ef1-8fde-0397861a166b" });

            migrationBuilder.CreateIndex(
                name: "IX_EventTicketTypes_EventId",
                table: "EventTicketTypes",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTicketTypes");

            migrationBuilder.RenameColumn(
                name: "EventFormat",
                table: "Events",
                newName: "eventFormat");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "ClaimType",
                value: "Admin.Images.Edit");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 10,
                column: "ClaimType",
                value: "Admin.Events.Edit");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 12,
                column: "ClaimType",
                value: "Admin.Venues.Edit");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f205a4d6-cbab-4163-9427-32802498bc59", "AQAAAAIAAYagAAAAEF+OwzkcGgbWzhEmVd7Xy0iRHQU842tguftE2WfkQvSDLy8ZHNXzDqg57vVgrCdioA==", "a9023bf7-6259-40e8-a944-8aae466e9709" });
        }
    }
}
