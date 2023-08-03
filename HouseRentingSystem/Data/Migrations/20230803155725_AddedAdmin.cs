using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "00e441e7-8f28-4485-af43-7fdf6d39ffe3", "AQAAAAIAAYagAAAAEDbdFSBQw7lgcFtozRIrWDt2AXnbkpEDYcHa+uaa6CM947uAkCV/Qu+PnSqbS7uSHw==", "6a9a3136-787b-4e64-b3f4-579d1b7b03d9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "deal2856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "845938bf-4e6d-4817-b8b2-cd338c8fb3c5", "AQAAAAIAAYagAAAAEDDVggsoUsQ4dNcb+810GPovkXvAE2Vn3ukJXtRt2BMRKQKNEU3QvSqySAgHXofZ5Q==", "5693cab1-d5b8-4150-96d4-5512ec92c80d" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "bcb4f072-ecca-43c9-ab26-c060c6f364e4", 0, "9b629e95-0e8b-4fdc-b987-5685e5e16ac0", "admin@mail.bg", false, "Great", "Admin", false, null, "admin@mail.bg", "admin@mail.bg", "AQAAAAIAAYagAAAAEPKFrsgh3mNOr7d3sBIc3oA+mN0S7u/sSQqLe5KJ9C5EVsIB26qQtaBRYmUqI+6PdA==", null, false, "ed1c3b5a-5e5e-41b0-83bc-e761002de106", false, "admin@mail.bg" });

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "PhoneNumber", "UserId" },
                values: new object[] { 5, "+359123456789", "bcb4f072-ecca-43c9-ab26-c060c6f364e4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bcb4f072-ecca-43c9-ab26-c060c6f364e4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b84bda2a-2e42-4116-879e-07b9e546e0a2", "AQAAAAEAACcQAAAAENlJ3waLXzc9r5xH7z/w0zweGC3suBCOHPHdYmSteMrFozmbAxStoeEwer/Jq6sFTQ==", "267d68b4-72e1-4fa1-9825-b7ed629fd7c6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "deal2856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b7fd2ecd-0060-4ac4-87b9-a7da12b6519b", "AQAAAAEAACcQAAAAECU6Nzx2xWJTg70RrULHBkisKYzQW4JnyTqPauuGtsIgnWvhvjHeQr3K5ywMugVXfg==", "c4945039-20cd-4433-b69e-706039697cb2" });
        }
    }
}
