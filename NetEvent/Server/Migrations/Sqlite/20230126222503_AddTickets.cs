using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NetEvent.Server.Migrations.Sqlite
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PurchaseTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TicketId = table.Column<long>(type: "INTEGER", nullable: true),
                    PurchaseId = table.Column<long>(type: "INTEGER", nullable: true),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    Currency = table.Column<int>(type: "INTEGER", nullable: false)
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
                values: new object[] { "5d10c057-f0c5-4439-9ea5-52da9de25db4", "AQAAAAIAAYagAAAAEHl8xBgwOlrJfSfDpoRyQLa1TdrX7zMysaC62jvZEjAyngzzdjD+zyfPkY2zk66ZGQ==", "83f864b3-fb09-44e4-9e5c-748beadc4af3" });

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
                values: new object[] { "26cbb106-629e-4b0f-b528-03e4fbaf148e", "AQAAAAIAAYagAAAAEMVeCrEoGoqMFSa9mWv07gyORfXVq9Zash81NSc3Z4QmFWadfMKOABkphkaem7Q7nQ==", "56b690d3-1bb2-4ef1-8fde-0397861a166b" });
        }
    }
}
