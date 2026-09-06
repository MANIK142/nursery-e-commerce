using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nursery.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CareInstructionAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Plants");

            migrationBuilder.CreateTable(
                name: "CareInstructions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WateringFrequency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SunlightRequirement = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SoilType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MinTemperatureCelsius = table.Column<int>(type: "int", nullable: false),
                    MaxTemperatureCelsius = table.Column<int>(type: "int", nullable: false),
                    HumidityLevel = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FertilizingFrequency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DifficultyLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsToxicToPets = table.Column<bool>(type: "bit", nullable: false),
                    PruningNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareInstructions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CareInstructions_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CareInstructions_PlantId",
                table: "CareInstructions",
                column: "PlantId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CareInstructions");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Plants",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
