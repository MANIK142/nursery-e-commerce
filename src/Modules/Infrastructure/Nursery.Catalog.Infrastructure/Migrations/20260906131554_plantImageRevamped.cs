using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nursery.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class plantImageRevamped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantImages_PlantVariants_PlantVariantId",
                table: "PlantImages");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantImages_Plants_PlantId",
                table: "PlantImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantImages",
                table: "PlantImages");

            migrationBuilder.DropIndex(
                name: "IX_PlantImages_OwnerType_OwnerId",
                table: "PlantImages");

            migrationBuilder.DropIndex(
                name: "IX_PlantImages_PlantId",
                table: "PlantImages");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "PlantImages");

            migrationBuilder.DropColumn(
                name: "OwnerType",
                table: "PlantImages");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlantId",
                table: "PlantImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantImages",
                table: "PlantImages",
                column: "PlantId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantImages_PlantVariants_PlantVariantId",
                table: "PlantImages",
                column: "PlantVariantId",
                principalTable: "PlantVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantImages_Plants_PlantId",
                table: "PlantImages",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantImages",
                table: "PlantImages");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlantId",
                table: "PlantImages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "PlantImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "OwnerType",
                table: "PlantImages",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantImages",
                table: "PlantImages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlantImages_OwnerType_OwnerId",
                table: "PlantImages",
                columns: new[] { "OwnerType", "OwnerId" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantImages_PlantId",
                table: "PlantImages",
                column: "PlantId");

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
    }
}
