using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nursery.Identity.Migrations
{
    /// <inheritdoc />
    public partial class RolesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a71a55d6-99d7-4123-b4e0-1218ecb90e3e", "a71a55d6-99d7-4123-b4e0-1218ecb90e3e", "Customer", "CUSTOMER" },
                    { "c309fa92-2123-47be-b397-a1c77adb502c", "c309fa92-2123-47be-b397-a1c77adb502c", "Admin", "ADMIN" },
                    { "e6902b62-bd46-4561-950c-8c8eb63db8bc", "e6902b62-bd46-4561-950c-8c8eb63db8bc", "Employee", "EMPLOYEE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a71a55d6-99d7-4123-b4e0-1218ecb90e3e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c309fa92-2123-47be-b397-a1c77adb502c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6902b62-bd46-4561-950c-8c8eb63db8bc");
        }
    }
}
