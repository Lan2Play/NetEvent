using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Psql
{
    public partial class AddAdminPolicy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "b301f818-14bf-44b4-962b-707dff1c8fbf");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "orga",
                column: "ConcurrencyStamp",
                value: "0c833faf-3f02-4330-8163-c1e8bde3ce13");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "0d3d268a-f688-4de9-9314-2851b6b3dab5");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3ce54de9-0c49-4be8-9b62-0058a31c6038", "AQAAAAEAACcQAAAAEDL0ayrTnRU9HdtM5cKAg3pq7HEmW9nL84atXLj7/RJrvgsTXSbLP9asIjVE9MfAoQ==", "58483af1-b677-4bb2-8c17-5227842f80f7" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "34e86cbe-c55c-4e50-a483-b6c2d6a2cfe0");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "orga",
                column: "ConcurrencyStamp",
                value: "8f4a0188-1f34-4330-9367-4db4c2ba157c");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "6074c073-80ff-4c2b-9fb3-6dc1c5e22937");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1f590c30-4259-47f5-8ca1-93af30ab20ed", "AQAAAAEAACcQAAAAEDKgcYsr1GV1xfWmPNlZIVxKVk/ZnTV0HHlLQRy2NFRzaSW2b24J9CG9E9lnhrbhAw==", "b44c7126-4442-4b16-bd42-a382152fad2e" });
        }
    }
}
