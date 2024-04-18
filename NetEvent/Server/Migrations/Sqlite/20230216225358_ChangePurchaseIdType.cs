using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Sqlite
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
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Purchases",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d07882b0-10fa-4ee6-8bf2-231f80fb4656", "AQAAAAIAAYagAAAAEJoG+/hCI6v6EHNg+X2s0QzmdcRbSeNwg8rNju5+pXfaezhvxch9oAXvljZaNVeJbg==", "dce345b2-0622-4092-9182-afc2c76cc50a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PurchaseId",
                table: "TicketPurchases",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Purchases",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5d10c057-f0c5-4439-9ea5-52da9de25db4", "AQAAAAIAAYagAAAAEHl8xBgwOlrJfSfDpoRyQLa1TdrX7zMysaC62jvZEjAyngzzdjD+zyfPkY2zk66ZGQ==", "83f864b3-fb09-44e4-9e5c-748beadc4af3" });
        }
    }
}
