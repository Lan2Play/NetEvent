using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class AddHideOrganizationNameInNavBarSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "Key", "SerializedValue" },
                values: new object[] { "HideOrganizationNameInNavBar", "False" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f205a4d6-cbab-4163-9427-32802498bc59", "AQAAAAIAAYagAAAAEF+OwzkcGgbWzhEmVd7Xy0iRHQU842tguftE2WfkQvSDLy8ZHNXzDqg57vVgrCdioA==", "a9023bf7-6259-40e8-a944-8aae466e9709" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemSettings",
                keyColumn: "Key",
                keyValue: "HideOrganizationNameInNavBar");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5065bc9a-bc05-4b7c-9af3-ae2b8c357b5e", "AQAAAAIAAYagAAAAEIl//AZ2fNXWd9KtdIpih7g5tnmcglDs61MDvlTphozHjweLVUb5dSMPuu0O2FDjtA==", "4f945210-5300-41d7-95f5-05ec1edb4070" });
        }
    }
}
