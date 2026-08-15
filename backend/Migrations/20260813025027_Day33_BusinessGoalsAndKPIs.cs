using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Day33_BusinessGoalsAndKPIs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessGoals_KPIs_KpiId",
                table: "BusinessGoals");

            migrationBuilder.DropIndex(
                name: "IX_BusinessGoals_KpiId",
                table: "BusinessGoals");

            migrationBuilder.DropColumn(
                name: "KpiId",
                table: "BusinessGoals");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "BusinessGoals",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "BusinessGoals",
                newName: "TargetDate");

            migrationBuilder.RenameColumn(
                name: "Department",
                table: "BusinessGoals",
                newName: "GoalType");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "BusinessGoals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "BusinessGoals",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentValue",
                table: "BusinessGoals",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BusinessGoals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "BusinessGoals",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerUserId",
                table: "BusinessGoals",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "BusinessGoals",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "BusinessGoals",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BusinessKPIs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    BusinessGoalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    KPIType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MetricKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CurrentValue = table.Column<decimal>(type: "numeric", nullable: false),
                    TargetValue = table.Column<decimal>(type: "numeric", nullable: false),
                    PreviousValue = table.Column<decimal>(type: "numeric", nullable: false),
                    Unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Direction = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LastCalculatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_BusinessKPIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessKPIs_BusinessGoals_BusinessGoalId",
                        column: x => x.BusinessGoalId,
                        principalTable: "BusinessGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessKPIs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessGoals_OrganizationId_Status",
                table: "BusinessGoals",
                columns: new[] { "OrganizationId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessKPIs_BusinessGoalId",
                table: "BusinessKPIs",
                column: "BusinessGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessKPIs_OrganizationId",
                table: "BusinessKPIs",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessKPIs");

            migrationBuilder.DropIndex(
                name: "IX_BusinessGoals_OrganizationId_Status",
                table: "BusinessGoals");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "BusinessGoals");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "BusinessGoals");

            migrationBuilder.DropColumn(
                name: "CurrentValue",
                table: "BusinessGoals");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BusinessGoals");

            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "BusinessGoals");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "BusinessGoals");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "BusinessGoals");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "BusinessGoals");

            migrationBuilder.RenameColumn(
                name: "TargetDate",
                table: "BusinessGoals",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "BusinessGoals",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "GoalType",
                table: "BusinessGoals",
                newName: "Department");

            migrationBuilder.AddColumn<Guid>(
                name: "KpiId",
                table: "BusinessGoals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BusinessGoals_KpiId",
                table: "BusinessGoals",
                column: "KpiId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessGoals_KPIs_KpiId",
                table: "BusinessGoals",
                column: "KpiId",
                principalTable: "KPIs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
