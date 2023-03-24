using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectDetectionAPI.Migrations
{
    public partial class userid_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0bc4e807-f9c3-49de-87b9-93bc571cda56");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c86796a1-ba83-4e93-b260-213cccdfd725");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FileStores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7ad7787e-945d-45d0-9598-5f4dd90fe169", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a8e6c40b-68be-4601-8ca5-285470684e32", "1", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ad7787e-945d-45d0-9598-5f4dd90fe169");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8e6c40b-68be-4601-8ca5-285470684e32");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FileStores");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0bc4e807-f9c3-49de-87b9-93bc571cda56", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c86796a1-ba83-4e93-b260-213cccdfd725", "2", "User", "User" });
        }
    }
}
