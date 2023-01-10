using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Psql
{
    /// <inheritdoc />
    public partial class AddEventFormat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "eventFormat",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ad7c3278-025a-4e2d-88a9-552e00f40e13", "AQAAAAIAAYagAAAAEAP/L7N9GLytqJf80BBxBLQ5UWeehMBsWdxgyzw5ml6bCGaRSEnO5AOFcVd3xfZCDA==", "5ea9e243-c62f-4766-a4a7-897d6fc87352" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "eventFormat",
                table: "Events");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "52c12867-bd72-4aab-bea4-8c28a3eb2ab4", "AQAAAAIAAYagAAAAEBFWYpReeWM5orL5YUESF1QxaL/Jii8og1m7UPYa5RlfF9Fy8THluDm4JOSsa+99Gw==", "8a198d21-af03-4a7d-a921-ec5aa3da97f2" });
        }
    }
}
