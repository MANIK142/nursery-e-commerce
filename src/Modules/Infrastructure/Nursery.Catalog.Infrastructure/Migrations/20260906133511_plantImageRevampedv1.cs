using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nursery.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class plantImageRevampedv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantImages",
                table: "PlantImages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlantImages_PlantId",
                table: "PlantImages",
                column: "PlantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantImages",
                table: "PlantImages");

            migrationBuilder.DropIndex(
                name: "IX_PlantImages_PlantId",
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
        }
    }
}
