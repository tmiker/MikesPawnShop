using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Products.Write.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPublishedToOutboxRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                schema: "dbo",
                table: "Outbox Records",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                schema: "dbo",
                table: "Outbox Records");
        }
    }
}
