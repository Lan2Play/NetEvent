using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Sqlite
{
    public partial class EmailTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    TemplateId = table.Column<string>(type: "TEXT", nullable: false),
                    SubjectTemplate = table.Column<string>(type: "TEXT", nullable: false),
                    ContentTemplate = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.TemplateId);
                });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "TemplateId", "ContentTemplate", "SubjectTemplate" },
                values: new object[] { "UserEmailConfirmEmailTemplate", "<h1>Welcome to NetEvent.</h1>\n<p> Please confirm your E-Mail by clicking on the following link:</p><a href=\"@Model.TemplateVariables[\"confirmUrl\"]\">@Model.TemplateVariables[\"confirmUrl\"]</a>   ", "Please confirm your E-Mail address." });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "84632355-974a-4ede-8ad2-10fd2ea7205b");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "orga",
                column: "ConcurrencyStamp",
                value: "277a4365-bb0c-4f4f-914b-0dc964fcfc43");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "0abf641a-efe4-44c5-be5e-66d470552fff");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "57bd9dd0-2bf9-46bb-a5d7-344c635fd9e6", "AQAAAAEAACcQAAAAEJsjQn0D+D9bxeePXTyg8t69rtC5NS32qd8SRPx3g1/kuAthsqT0MM2t0Kq9t3Hizw==", "87f068aa-8fd8-4fde-99bc-830be17ffb33" });
        }
    }
}
