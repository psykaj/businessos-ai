using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddQRCodeLabelAndFont : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LabelFont",
                table: "QRCodes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabelText",
                table: "QRCodes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LabelFont",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "LabelText",
                table: "QRCodes");
        }
    }
}
