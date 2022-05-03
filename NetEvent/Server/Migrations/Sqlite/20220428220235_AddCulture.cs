using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Sqlite
{
    public partial class AddCulture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "OrganizationData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "OrganizationData",
                columns: new[] { "Key", "Value" },
                values: new object[] { "Culture", "en-US" });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "b28356af-7c77-43b5-9ae6-94d13a920993");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "7d5abe61-7b08-476a-a045-b3f9ec1db046");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c4f79989-e421-4916-9111-c755b5ff3f7c", "AQAAAAEAACcQAAAAEOYecvBveTxk0Uak/0OlhtvUSmeGR0l671OBLIQA+WLpfW1dGPiqPuKUIVOhRYV8lQ==", "d7bce49e-fef8-44bc-9eaa-f3baaeb7d32e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrganizationData",
                keyColumn: "Key",
                keyValue: "Culture");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "OrganizationData",
                type: "TEXT",
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "8cba3bb0-7bd3-451a-98c9-b5410db49d2c");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "cf7db04d-68f7-468e-9983-fe6e94f0082f");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c65b7069-d295-4dc0-bc16-893a0709919c", "AQAAAAEAACcQAAAAENtabIRe2VSwWrntrDz2wxI1TokRrpCsatjk3EJAkUpvMnyD/iqXU+hK3U9/GNpFuw==", "b0e612a1-6cf0-4559-81cb-af8288ff72cf" });
        }
    }
}
