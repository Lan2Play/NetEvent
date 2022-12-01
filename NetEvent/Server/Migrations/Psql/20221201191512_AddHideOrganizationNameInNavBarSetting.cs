using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Psql
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
                values: new object[] { "9b5238c9-c5a4-4790-8eb0-b78510682b41", "AQAAAAIAAYagAAAAEN5rDB2K6wk0FXO84PGiPx3h+VPc4QgpcSbFjQMFGAKW4SU+0nIM5Ee+5MXgDpZs1w==", "34e28843-865a-4bed-a77e-0d90d60b7547" });
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
                values: new object[] { "ad7c3278-025a-4e2d-88a9-552e00f40e13", "AQAAAAIAAYagAAAAEAP/L7N9GLytqJf80BBxBLQ5UWeehMBsWdxgyzw5ml6bCGaRSEnO5AOFcVd3xfZCDA==", "5ea9e243-c62f-4766-a4a7-897d6fc87352" });
        }
    }
}
