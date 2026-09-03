using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nursery.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlantCategoryCongigurationadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantCategory",
                table: "PlantCategory");

            migrationBuilder.DropIndex(
                name: "IX_PlantCategory_PlantId",
                table: "PlantCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantCategory",
                table: "PlantCategory",
                columns: new[] { "PlantId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantCategory_CategoryId",
                table: "PlantCategory",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantCategory",
                table: "PlantCategory");

            migrationBuilder.DropIndex(
                name: "IX_PlantCategory_CategoryId",
                table: "PlantCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantCategory",
                table: "PlantCategory",
                columns: new[] { "CategoryId", "PlantId" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantCategory_PlantId",
                table: "PlantCategory",
                column: "PlantId");
        }
    }
}
