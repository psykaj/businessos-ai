using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessOutcomes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessOutcomes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uuid", nullable: false),
                    OutcomeType = table.Column<int>(type: "integer", nullable: false),
                    SourceType = table.Column<int>(type: "integer", nullable: false),
                    SourceId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    RelatedEntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RelatedEntityId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BeforeValue = table.Column<decimal>(type: "numeric", nullable: true),
                    AfterValue = table.Column<decimal>(type: "numeric", nullable: true),
                    ChangeValue = table.Column<decimal>(type: "numeric", nullable: true),
                    ChangePercentage = table.Column<decimal>(type: "numeric", nullable: true),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TimeSavedMinutes = table.Column<int>(type: "integer", nullable: true),
                    RevenueImpact = table.Column<decimal>(type: "numeric", nullable: true),
                    CostImpact = table.Column<decimal>(type: "numeric", nullable: true),
                    CustomerImpact = table.Column<int>(type: "integer", nullable: true),
                    Confidence = table.Column<int>(type: "integer", nullable: false),
                    AttributionLevel = table.Column<int>(type: "integer", nullable: false),
                    MeasurementMethod = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    OccurredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Metadata = table.Column<string>(type: "text", nullable: true),
                    Explanation = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessOutcomes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessOutcomes_BusinessId",
                table: "BusinessOutcomes",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessOutcomes_BusinessId_OutcomeType",
                table: "BusinessOutcomes",
                columns: new[] { "BusinessId", "OutcomeType" });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessOutcomes_BusinessId_SourceType_SourceId",
                table: "BusinessOutcomes",
                columns: new[] { "BusinessId", "SourceType", "SourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessOutcomes_BusinessId_SourceType_SourceId_OutcomeType",
                table: "BusinessOutcomes",
                columns: new[] { "BusinessId", "SourceType", "SourceId", "OutcomeType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessOutcomes_Confidence",
                table: "BusinessOutcomes",
                column: "Confidence");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessOutcomes_OccurredAt",
                table: "BusinessOutcomes",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessOutcomes_Status",
                table: "BusinessOutcomes",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessOutcomes");
        }
    }
}
