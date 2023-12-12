using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetEvent.Server.Migrations.Psql
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Slug = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    AvailableTickets = table.Column<long>(type: "bigint", nullable: false),
                    SellStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SellEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsGiftable = table.Column<bool>(type: "boolean", nullable: false),
                    EventId = table.Column<long>(type: "bigint", nullable: true)
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
                values: new object[] { "f04ce91e-9530-4345-8cb6-ca7064b18f9e", "AQAAAAIAAYagAAAAEBQZqmlKIsoqJXLPAEONrg4qS/LiDqczL05LikcpplHSrajOE+VxOsnzvK2Xq5bxig==", "9529d94c-3a1d-4616-b977-af03e35856e8" });

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
                values: new object[] { "9b5238c9-c5a4-4790-8eb0-b78510682b41", "AQAAAAIAAYagAAAAEN5rDB2K6wk0FXO84PGiPx3h+VPc4QgpcSbFjQMFGAKW4SU+0nIM5Ee+5MXgDpZs1w==", "34e28843-865a-4bed-a77e-0d90d60b7547" });
        }
    }
}
