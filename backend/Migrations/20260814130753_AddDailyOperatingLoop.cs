using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyOperatingLoop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyBusinessBriefings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    BriefingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false),
                    BusinessHealth = table.Column<int>(type: "integer", nullable: false),
                    PriorityCount = table.Column<int>(type: "integer", nullable: false),
                    OpportunityCount = table.Column<int>(type: "integer", nullable: false),
                    RiskCount = table.Column<int>(type: "integer", nullable: false),
                    CompletedPriorityCount = table.Column<int>(type: "integer", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_DailyBusinessBriefings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyPriorities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    BriefingId = table.Column<Guid>(type: "uuid", nullable: false),
                    PriorityType = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    Severity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PriorityScore = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    RelatedEntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RelatedEntityId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RelatedGoalId = table.Column<Guid>(type: "uuid", nullable: true),
                    RelatedKpiId = table.Column<Guid>(type: "uuid", nullable: true),
                    SuggestedAction = table.Column<string>(type: "text", nullable: false),
                    ExpectedImpact = table.Column<string>(type: "text", nullable: false),
                    ImpactType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Confidence = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DueAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_DailyPriorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyPriorities_DailyBusinessBriefings_BriefingId",
                        column: x => x.BriefingId,
                        principalTable: "DailyBusinessBriefings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyBusinessBriefings_OrganizationId_BriefingDate",
                table: "DailyBusinessBriefings",
                columns: new[] { "OrganizationId", "BriefingDate" });

            migrationBuilder.CreateIndex(
                name: "IX_DailyBusinessBriefings_Status",
                table: "DailyBusinessBriefings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPriorities_BriefingId",
                table: "DailyPriorities",
                column: "BriefingId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPriorities_OrganizationId_BriefingId",
                table: "DailyPriorities",
                columns: new[] { "OrganizationId", "BriefingId" });

            migrationBuilder.CreateIndex(
                name: "IX_DailyPriorities_OrganizationId_PriorityType",
                table: "DailyPriorities",
                columns: new[] { "OrganizationId", "PriorityType" });

            migrationBuilder.CreateIndex(
                name: "IX_DailyPriorities_PriorityScore",
                table: "DailyPriorities",
                column: "PriorityScore");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPriorities_RelatedEntityId",
                table: "DailyPriorities",
                column: "RelatedEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPriorities_Status",
                table: "DailyPriorities",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyPriorities");

            migrationBuilder.DropTable(
                name: "DailyBusinessBriefings");
        }
    }
}
