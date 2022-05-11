using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Sqlite
{
    public partial class UpdateDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "e0b8deb6-b85d-4204-8576-a2b2235996d9");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "e958d9c1-d9c8-4179-a919-2fd6ef0ca718");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "orga", "b33353bf-0392-48ce-b6bc-ea9a18f6f66a", "Orga", "ORGA" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3bc8e70a-51bf-4931-9573-bfddf81c6364", "AQAAAAEAACcQAAAAEIcuuLroSU0p/LKsh6Rxus0pgOOWi8KcRQ6AIKd3D4FK8075cMpRfKQeUYuNZwSqYQ==", "df445398-6b45-455f-baea-c9e2a8034b48" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "orga");

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
    }
}
