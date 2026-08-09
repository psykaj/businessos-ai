using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Day29_BusinessBriefingAlerts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessBriefings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false),
                    BusinessHealthScore = table.Column<int>(type: "integer", nullable: false),
                    RevenueSummary = table.Column<string>(type: "text", nullable: false),
                    CustomerSummary = table.Column<string>(type: "text", nullable: false),
                    FinanceSummary = table.Column<string>(type: "text", nullable: false),
                    InventorySummary = table.Column<string>(type: "text", nullable: false),
                    RiskCount = table.Column<int>(type: "integer", nullable: false),
                    OpportunityCount = table.Column<int>(type: "integer", nullable: false),
                    ActionCount = table.Column<int>(type: "integer", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_BusinessBriefings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProactiveAlerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    SourceModule = table.Column<string>(type: "text", nullable: false),
                    SourceEntityId = table.Column<string>(type: "text", nullable: false),
                    RecommendedAction = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeduplicationKey = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_ProactiveAlerts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BriefingItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BriefingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    SourceModule = table.Column<string>(type: "text", nullable: false),
                    SourceEntityId = table.Column<string>(type: "text", nullable: false),
                    ConfidenceScore = table.Column<decimal>(type: "numeric", nullable: false),
                    ExpectedBusinessImpact = table.Column<string>(type: "text", nullable: false),
                    RecommendedAction = table.Column<string>(type: "text", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    IsDismissed = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_BriefingItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BriefingItems_BusinessBriefings_BriefingId",
                        column: x => x.BriefingId,
                        principalTable: "BusinessBriefings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BriefingItems_BriefingId",
                table: "BriefingItems",
                column: "BriefingId");

            migrationBuilder.CreateIndex(
                name: "IX_BriefingItems_Priority",
                table: "BriefingItems",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessBriefings_OrganizationId",
                table: "BusinessBriefings",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessBriefings_OrganizationId_Date",
                table: "BusinessBriefings",
                columns: new[] { "OrganizationId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProactiveAlerts_DeduplicationKey",
                table: "ProactiveAlerts",
                column: "DeduplicationKey");

            migrationBuilder.CreateIndex(
                name: "IX_ProactiveAlerts_OrganizationId",
                table: "ProactiveAlerts",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProactiveAlerts_OrganizationId_Status",
                table: "ProactiveAlerts",
                columns: new[] { "OrganizationId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BriefingItems");

            migrationBuilder.DropTable(
                name: "ProactiveAlerts");

            migrationBuilder.DropTable(
                name: "BusinessBriefings");
        }
    }
}
