using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Psql
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
                value: "c5bee0d2-6312-4a3f-b501-dc6001b35603");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "orga",
                column: "ConcurrencyStamp",
                value: "3b51e485-2609-43e0-adb0-49ac0e238bd5");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "6547e48a-4e21-48be-9670-b1d8a019a820");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "02ab97c5-1d17-4091-86d0-90a4e543528f", "AQAAAAEAACcQAAAAENsJgZLpNMsHlYIa6U13ES8HrzYCSVLStFH0JpKehnO+9rkYz7coFQHqJ2PO/jSKtw==", "6db79444-ab20-4d81-8a81-0f893229528a" });
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
                value: "96e012c4-e3dd-4443-a2ce-9031dafeb83c");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "orga",
                column: "ConcurrencyStamp",
                value: "60e60ecc-36eb-4ab3-9056-639239ecfaae");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "8d4a545f-a52c-4da7-b6a2-84a6b74b4598");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c6de3dce-0e88-49b1-ba42-65f604cda396", "AQAAAAEAACcQAAAAEEqFQbDflodnoBrfOkS/UR+FEd8KBGGchtzYAWy/sFhCMy/U7dDCQ0MO7mT+9SX8Ng==", "92d206a4-6b7e-44df-9051-0e781cc7e1e8" });
        }
    }
}
