using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Psql
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
                value: "13b97bfd-c815-4fe1-a6fb-6f28373e95ac");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "24486221-c4eb-48e8-b0b0-9196f334b92a");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "orga", "1ed8ef04-c162-48cc-a5e0-1a2ca2fe7873", "Orga", "ORGA" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ea5c919d-66f2-4d14-8651-d0337f9b0c6a", "AQAAAAEAACcQAAAAEKNVAD+29wLUstjAjgpc/d/w+NH8f9ptjHoX8GhZlz2GryX4Yoh6l0dM/+gyxCZuQw==", "e4271917-1769-4993-81ac-a342eb99f717" });
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
                value: "c5719bd7-ae7e-4a73-9ab4-749ccd170ece");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "40d745a6-bf89-451e-b041-d4f631dcade2");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0e150be1-623e-4fe6-8f14-35451efec037", "AQAAAAEAACcQAAAAEHbIiyBeJvGwJA6MfKoNVUAa7QzsLRa/lh1RxYZYbK5IcmxgT9vGct4aiS0rFJJy4g==", "737e3334-e483-4ef3-9366-2c31e098182e" });
        }
    }
}
