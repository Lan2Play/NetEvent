using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Psql
{
    public partial class AddCulture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "OrganizationData",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "OrganizationData",
                columns: new[] { "Key", "Value" },
                values: new object[] { "Culture", "en-US" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrganizationData",
                keyColumn: "Key",
                keyValue: "Culture");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "OrganizationData",
                type: "text",
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "43c133fb-9d03-4384-926f-0cc0bdf8ed76");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "0f0d9301-e452-45e5-9b8b-f2352f7766cc");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d23693f6-f4a2-4225-8b9a-0d704b0f6b36", "AQAAAAEAACcQAAAAEJ1Hma7SypiJ0f8Fpew1kKBIvqOnfIeNuwJqBqwQB6ytPaou838eJEO3au2h7J0MxA==", "f7cb9a70-b856-442a-a110-a71d9d81c485" });
        }
    }
}
