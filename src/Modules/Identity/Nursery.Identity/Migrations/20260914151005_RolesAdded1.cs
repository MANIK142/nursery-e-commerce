using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nursery.Identity.Migrations
{
    /// <inheritdoc />
    public partial class RolesAdded1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3ca074b1-81cf-470e-99d3-b652a54dafad", "3ca074b1-81cf-470e-99d3-b652a54dafad", "WarehouseStaff", "WAREHOUSESTAFF" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ca074b1-81cf-470e-99d3-b652a54dafad");
        }
    }
}
