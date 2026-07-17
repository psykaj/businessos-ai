using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalyticsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QRScans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    QRCodeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScanDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IPAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DeviceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Browser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BrowserVersion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    OperatingSystem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OperatingSystemVersion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TimeZone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Referrer = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    UTMSource = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UTMMedium = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UTMCampaign = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UTMTerm = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UTMContent = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Language = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRScans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QRScans_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QRScans_QRCodes_QRCodeId",
                        column: x => x.QRCodeId,
                        principalTable: "QRCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QRScans_Browser",
                table: "QRScans",
                column: "Browser");

            migrationBuilder.CreateIndex(
                name: "IX_QRScans_Country",
                table: "QRScans",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_QRScans_DeviceType",
                table: "QRScans",
                column: "DeviceType");

            migrationBuilder.CreateIndex(
                name: "IX_QRScans_OrganizationId",
                table: "QRScans",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_QRScans_QRCodeId",
                table: "QRScans",
                column: "QRCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_QRScans_ScanDateTime",
                table: "QRScans",
                column: "ScanDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QRScans");
        }
    }
}
