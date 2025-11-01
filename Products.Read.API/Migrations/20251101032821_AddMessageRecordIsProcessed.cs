using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Products.Read.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageRecordIsProcessed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "MessageRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "MessageRecords");
        }
    }
}
