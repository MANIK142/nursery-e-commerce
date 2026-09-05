using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nursery.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlantVariantAndSalePriceAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Plants_SkuCode",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "RetailPrice",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "SkuCode",
                table: "Plants");

            migrationBuilder.CreateTable(
                name: "PlantVariants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VariantName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RetailPrice_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RetailPrice_Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    WholesalePrice_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WholesalePrice_Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantVariants_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VariantSalePrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlantVariantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalePrice_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalePrice_Currency = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    StartsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariantSalePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariantSalePrices_PlantVariants_PlantVariantId",
                        column: x => x.PlantVariantId,
                        principalTable: "PlantVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlantVariants_PlantId",
                table: "PlantVariants",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_VariantSalePrices_PlantVariantId",
                table: "VariantSalePrices",
                column: "PlantVariantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VariantSalePrices");

            migrationBuilder.DropTable(
                name: "PlantVariants");

            migrationBuilder.AddColumn<decimal>(
                name: "RetailPrice",
                table: "Plants",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SkuCode",
                table: "Plants",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_SkuCode",
                table: "Plants",
                column: "SkuCode",
                unique: true);
        }
    }
}
