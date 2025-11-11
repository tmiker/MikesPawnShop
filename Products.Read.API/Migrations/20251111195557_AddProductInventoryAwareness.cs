using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Products.Read.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProductInventoryAwareness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LowStockThreshold",
                schema: "dbo",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityAvailable",
                schema: "dbo",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityOnHand",
                schema: "dbo",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UOM",
                schema: "dbo",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LowStockThreshold",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "QuantityAvailable",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "QuantityOnHand",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UOM",
                schema: "dbo",
                table: "Products");
        }
    }
}
