using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nursery.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPlantImagesCascadeConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantImages_PlantVariants_PlantVariantId",
                table: "PlantImages");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantImages_PlantVariants_PlantVariantId",
                table: "PlantImages",
                column: "PlantVariantId",
                principalTable: "PlantVariants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantImages_PlantVariants_PlantVariantId",
                table: "PlantImages");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantImages_PlantVariants_PlantVariantId",
                table: "PlantImages",
                column: "PlantVariantId",
                principalTable: "PlantVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
