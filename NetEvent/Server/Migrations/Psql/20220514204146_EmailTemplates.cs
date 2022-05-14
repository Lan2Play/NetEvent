using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Psql
{
    public partial class EmailTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    TemplateId = table.Column<string>(type: "text", nullable: false),
                    SubjectTemplate = table.Column<string>(type: "text", nullable: false),
                    ContentTemplate = table.Column<string>(type: "text", nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "b3c89c17-d348-465e-9fdc-12d88317227e");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "orga",
                column: "ConcurrencyStamp",
                value: "bc99dcc1-fd85-4d32-b831-f1f3c07dec0c");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "f28e0660-d780-4f90-96b2-2e326d52036c");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "97f54e37-a2e6-48cb-8f33-10fd6e4f2520", "AQAAAAEAACcQAAAAEGNt16UExWD8wiNsYj5znRbQqDzL9OFdW1XsHjxLYmEZmzptSkpIxbA7rsQT8aUe3w==", "940ba0de-d13a-4958-bd4f-3596455e06d7" });
        }
    }
}
