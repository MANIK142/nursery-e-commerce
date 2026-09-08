using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nursery.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFKatPlantAndPlantVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantImages_PlantVariants_OwnerId",
                table: "PlantImages");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantImages_Plants_OwnerId",
                table: "PlantImages");

            migrationBuilder.DropIndex(
                name: "IX_PlantImages_OwnerId",
                table: "PlantImages");

            migrationBuilder.AddColumn<Guid>(
                name: "PlantId",
                table: "PlantImages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlantVariantId",
                table: "PlantImages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlantImages_PlantId",
                table: "PlantImages",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantImages_PlantVariantId",
                table: "PlantImages",
                column: "PlantVariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantImages_PlantVariants_PlantVariantId",
                table: "PlantImages",
                column: "PlantVariantId",
                principalTable: "PlantVariants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantImages_Plants_PlantId",
                table: "PlantImages",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantImages_PlantVariants_PlantVariantId",
                table: "PlantImages");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantImages_Plants_PlantId",
                table: "PlantImages");

            migrationBuilder.DropIndex(
                name: "IX_PlantImages_PlantId",
                table: "PlantImages");

            migrationBuilder.DropIndex(
                name: "IX_PlantImages_PlantVariantId",
                table: "PlantImages");

            migrationBuilder.DropColumn(
                name: "PlantId",
                table: "PlantImages");

            migrationBuilder.DropColumn(
                name: "PlantVariantId",
                table: "PlantImages");

            migrationBuilder.CreateIndex(
                name: "IX_PlantImages_OwnerId",
                table: "PlantImages",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantImages_PlantVariants_OwnerId",
                table: "PlantImages",
                column: "OwnerId",
                principalTable: "PlantVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantImages_Plants_OwnerId",
                table: "PlantImages",
                column: "OwnerId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
