using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Sqlite
{
    public partial class ModifyEmailTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "TemplateId",
                keyValue: "UserEmailConfirmEmailTemplate",
                columns: new[] { "ContentTemplate", "SubjectTemplate" },
                values: new object[] { "<h1>@Model.TemplateVariables[\"firstName\"], welcome to NetEvent.</h1>\n<p> Please confirm your E-Mail by clicking on the following link:</p><a href=\"@Model.TemplateVariables[\"confirmUrl\"]\">@Model.TemplateVariables[\"confirmUrl\"]</a>   ", "@Model.TemplateVariables[\"firstName\"], please confirm your E-Mail address." });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "b7b30950-199a-4a7e-b775-75d093c8b97f");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "orga",
                column: "ConcurrencyStamp",
                value: "d519f414-8044-4d2d-aab8-b7116ce26d2a");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "45e24a20-7c3d-4fb8-b794-61b68b8f83d7");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0805bcc0-5ef4-4e2f-bb4c-f9fa62026b43", "AQAAAAEAACcQAAAAEK9djjvzIvs8zYvjmzdj4OxG14H0pe9ZtLCZcPkb0mDfupn+nzaEC+Lecor1udpnWQ==", "36011a58-a5ee-4c8d-ab5e-71c2d46a0fa0" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "TemplateId",
                keyValue: "UserEmailConfirmEmailTemplate",
                columns: new[] { "ContentTemplate", "SubjectTemplate" },
                values: new object[] { "<h1>Welcome to NetEvent.</h1>\n<p> Please confirm your E-Mail by clicking on the following link:</p><a href=\"@Model.TemplateVariables[\"confirmUrl\"]\">@Model.TemplateVariables[\"confirmUrl\"]</a>   ", "Please confirm your E-Mail address." });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "a9e950cd-a0b4-48c9-8329-724c6b3d69d0");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "orga",
                column: "ConcurrencyStamp",
                value: "a380c13c-a121-4f97-bb9d-885afaaeb08d");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "f7e61d71-8e65-4791-908e-4563efba1efe");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2dcf5f91-a56b-467a-82e5-dd927e51ab4d", "AQAAAAEAACcQAAAAEDS5p8JIep3tOgH2QGUNDy/CrdYXB0iduqDWi52faobvGV+bd02ymX6dCbi2l1vxkw==", "0ff48cc5-ed35-47fe-8379-a60dd474ce31" });
        }
    }
}
