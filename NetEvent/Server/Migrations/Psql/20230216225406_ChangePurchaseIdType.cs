using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetEvent.Server.Migrations.Psql
{
    /// <inheritdoc />
    public partial class ChangePurchaseIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PurchaseId",
                table: "TicketPurchases",
                type: "text",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Purchases",
                type: "text",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b379aa23-f095-4145-baba-e76f442552c0", "AQAAAAIAAYagAAAAELfWRDoThz05sToVRCMx7OHbxw1L91Pm2RW5UN3xXun3e2YpIX0NJ1s8fmgFZnB+Eg==", "0c5c2bf4-2389-49dd-bbe2-8c7b3962ef2b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PurchaseId",
                table: "TicketPurchases",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Purchases",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eef6f288-f1da-41c2-81fa-95eae2e69410", "AQAAAAIAAYagAAAAEK8gPOw/LRS1KOMtItdcM8BTgCAcUcbU/Vdonq1PDSC5X/SPhWNjh6eRCVI/eqRktw==", "3806018a-661d-427c-8601-3ee04c4cc6a3" });
        }
    }
}
