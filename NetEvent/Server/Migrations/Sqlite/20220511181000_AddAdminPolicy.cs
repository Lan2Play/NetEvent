using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Sqlite
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
                value: "b5f6bbcc-0a37-4167-a612-08ca51505f21");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "orga",
                column: "ConcurrencyStamp",
                value: "05198904-5f30-4dd7-b7f1-1681e0ce17c0");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "f6bb9019-a729-4313-9c12-6a655c14325b");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0c226eee-9a34-4b24-846b-f80bc0855298", "AQAAAAEAACcQAAAAEClkoAvvGx1jxgBfupOfKkoKPDN2BzapGJ4no+nUoHijElYgx/hCK58F9CTFVDTkwA==", "3cd2bacd-a415-40aa-be24-06880b0f5762" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "c8d19ab7-141e-4752-aa38-75f40f182e7d");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "orga",
                column: "ConcurrencyStamp",
                value: "042e2e62-72ac-4338-86ab-354019140c9f");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "8d895ddf-ad09-43b4-88d3-6121a85cb854");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "149876cd-faea-46e0-a934-b6a8c94b8f94", "AQAAAAEAACcQAAAAEJqhUGq6d4egVrmS0nsa3dbBnh4cuBQFdi13FKia1sJk5pYX7SRLNcKc0ogcMlcoKQ==", "09c1cfb5-17e6-4346-8d21-d6b532279267" });
        }
    }
}
