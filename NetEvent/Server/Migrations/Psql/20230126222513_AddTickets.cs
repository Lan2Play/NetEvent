using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NetEvent.Server.Migrations.Psql
{
    /// <inheritdoc />
    public partial class AddTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TicketPurchases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseId = table.Column<long>(type: "bigint", nullable: true),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketPurchases_EventTicketTypes_TicketId",
                        column: x => x.TicketId,
                        principalTable: "EventTicketTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketPurchases_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "Key", "SerializedValue" },
                values: new object[,]
                {
                    { "AdyenApiKey ", "" },
                    { "AdyenClientKey ", "" },
                    { "AdyenMerchantAccount ", "" }
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eef6f288-f1da-41c2-81fa-95eae2e69410", "AQAAAAIAAYagAAAAEK8gPOw/LRS1KOMtItdcM8BTgCAcUcbU/Vdonq1PDSC5X/SPhWNjh6eRCVI/eqRktw==", "3806018a-661d-427c-8601-3ee04c4cc6a3" });

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_UserId",
                table: "Purchases",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPurchases_PurchaseId",
                table: "TicketPurchases",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPurchases_TicketId",
                table: "TicketPurchases",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketPurchases");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DeleteData(
                table: "SystemSettings",
                keyColumn: "Key",
                keyValue: "AdyenApiKey ");

            migrationBuilder.DeleteData(
                table: "SystemSettings",
                keyColumn: "Key",
                keyValue: "AdyenClientKey ");

            migrationBuilder.DeleteData(
                table: "SystemSettings",
                keyColumn: "Key",
                keyValue: "AdyenMerchantAccount ");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f04ce91e-9530-4345-8cb6-ca7064b18f9e", "AQAAAAIAAYagAAAAEBQZqmlKIsoqJXLPAEONrg4qS/LiDqczL05LikcpplHSrajOE+VxOsnzvK2Xq5bxig==", "9529d94c-3a1d-4616-b977-af03e35856e8" });
        }
    }
}
