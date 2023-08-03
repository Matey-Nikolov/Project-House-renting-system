using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Services.Data.Migrations
{
    public partial class AddedUserColums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b84bda2a-2e42-4116-879e-07b9e546e0a2", "Teodor", "Lesly", "AQAAAAEAACcQAAAAENlJ3waLXzc9r5xH7z/w0zweGC3suBCOHPHdYmSteMrFozmbAxStoeEwer/Jq6sFTQ==", "267d68b4-72e1-4fa1-9825-b7ed629fd7c6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "deal2856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b7fd2ecd-0060-4ac4-87b9-a7da12b6519b", "Linda", "Michaels", "AQAAAAEAACcQAAAAECU6Nzx2xWJTg70RrULHBkisKYzQW4JnyTqPauuGtsIgnWvhvjHeQr3K5ywMugVXfg==", "c4945039-20cd-4433-b69e-706039697cb2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9a6e3c15-dda3-41c8-93e1-39f9928e2d0b", "AQAAAAEAACcQAAAAEJenBli52nHe2Xc8nqjQsNfmU6VyEbztDPihlHme+Vq09+6j9eMNfnDaZrdPmaGK7w==", "66e2acfc-97fe-48dc-86b7-abaced079340" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "deal2856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3b11b29a-48aa-4dac-991a-d4b690f3953c", "AQAAAAEAACcQAAAAEMfvHzlMFX4iU2iBi7/GVOINFPoq7m8EEnX4qtR1uZ/iYHCQKjXptzwDC/BdyPO9gg==", "eb105f6d-e5d2-48f1-8eb1-cb4155349803" });
        }
    }
}
