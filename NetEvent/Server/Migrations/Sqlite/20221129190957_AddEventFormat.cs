using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetEvent.Server.Migrations.Sqlite
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
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5065bc9a-bc05-4b7c-9af3-ae2b8c357b5e", "AQAAAAIAAYagAAAAEIl//AZ2fNXWd9KtdIpih7g5tnmcglDs61MDvlTphozHjweLVUb5dSMPuu0O2FDjtA==", "4f945210-5300-41d7-95f5-05ec1edb4070" });
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
                values: new object[] { "0c974d53-f5ba-458b-8b29-c4b59699505a", "AQAAAAIAAYagAAAAEGW9UdB9bSniD6sgL5khtBIgD6AZPN0wINdTfQpLsC/s3Nh32vI6FjuFDOHNw5qyzQ==", "c952ac1c-570a-48bf-b073-2d227fb38d4e" });
        }
    }
}
