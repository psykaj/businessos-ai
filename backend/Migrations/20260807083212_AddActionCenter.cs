using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddActionCenter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropTable(
                name: "Insights");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_KPIs_OrganizationId_Name",
                table: "KPIs");

            migrationBuilder.DropIndex(
                name: "IX_Forecasts_OrganizationId_ForecastType_ForecastDate",
                table: "Forecasts");

            migrationBuilder.DropColumn(
                name: "Trend",
                table: "KPIs");

            migrationBuilder.DropColumn(
                name: "ConfidenceScore",
                table: "Forecasts");

            migrationBuilder.RenameColumn(
                name: "ForecastType",
                table: "Forecasts",
                newName: "MetricName");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "KPIs",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "TargetValue",
                table: "KPIs",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "KPIs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentValue",
                table: "KPIs",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "KPIs",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "KPIs",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Frequency",
                table: "KPIs",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsHigherBetter",
                table: "KPIs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousValue",
                table: "KPIs",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "PredictedValue",
                table: "Forecasts",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<decimal>(
                name: "ConfidenceLevel",
                table: "Forecasts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Factors",
                table: "Forecasts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "GeneratedAt",
                table: "Forecasts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "LowerBound",
                table: "Forecasts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ModelUsed",
                table: "Forecasts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "UpperBound",
                table: "Forecasts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "AiActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    BusinessImpact = table.Column<string>(type: "text", nullable: false),
                    EstimatedRevenueIncrease = table.Column<decimal>(type: "numeric", nullable: false),
                    EstimatedCostSaving = table.Column<decimal>(type: "numeric", nullable: false),
                    RiskLevel = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExecutedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ExecutionResult = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_AiActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AiRecommendations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EstimatedImpact = table.Column<decimal>(type: "numeric", nullable: false),
                    ConfidenceLevel = table.Column<decimal>(type: "numeric", nullable: false),
                    SuggestedAction = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsApplied = table.Column<bool>(type: "boolean", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_AiRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiRecommendations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutomationWorkflows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_AutomationWorkflows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutomationWorkflows_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BenchmarkMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    MetricName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ComparisonType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DimensionName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CurrentPeriod = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CurrentValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PreviousPeriod = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    PreviousValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PercentageVariance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IndustryAverage = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OrganizationValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BenchmarkMedian = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BenchmarkTopQuartile = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PercentileRank = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_BenchmarkMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenchmarkMetrics_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Benchmarks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    MetricName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IndustryAverage = table.Column<decimal>(type: "numeric", nullable: false),
                    TopQuartile = table.Column<decimal>(type: "numeric", nullable: false),
                    Industry = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Benchmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Benchmarks_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ContactEmail = table.Column<string>(type: "text", nullable: true),
                    ContactPhone = table.Column<string>(type: "text", nullable: true),
                    CostCenterCode = table.Column<string>(type: "text", nullable: true),
                    WorkingHoursJson = table.Column<string>(type: "text", nullable: false),
                    OperationalSettingsJson = table.Column<string>(type: "text", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BranchManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManagerName = table.Column<string>(type: "text", nullable: false),
                    ManagerEmail = table.Column<string>(type: "text", nullable: false),
                    ManagerPhone = table.Column<string>(type: "text", nullable: true),
                    AssignedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CanApproveTransfers = table.Column<bool>(type: "boolean", nullable: false),
                    MaxTransferApprovalLimit = table.Column<decimal>(type: "numeric", nullable: false),
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
                    table.PrimaryKey("PK_BranchManagers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BranchPerformances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchName = table.Column<string>(type: "text", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalExpenses = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalProfit = table.Column<decimal>(type: "numeric", nullable: false),
                    ProfitMarginPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    CustomerCount = table.Column<int>(type: "integer", nullable: false),
                    EmployeeCount = table.Column<int>(type: "integer", nullable: false),
                    InventoryUtilizationPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    PerformanceScore = table.Column<decimal>(type: "numeric", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    IsBestPerformer = table.Column<bool>(type: "boolean", nullable: false),
                    IsLowestPerformer = table.Column<bool>(type: "boolean", nullable: false),
                    MoMRevenueGrowthPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    MoMProfitGrowthPercentage = table.Column<decimal>(type: "numeric", nullable: false),
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
                    table.PrimaryKey("PK_BranchPerformances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BranchWarehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    StorageCapacitySqFt = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrentUtilizationPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalStockItemsCount = table.Column<int>(type: "integer", nullable: false),
                    EstimatedStockValue = table.Column<decimal>(type: "numeric", nullable: false),
                    ContactPerson = table.Column<string>(type: "text", nullable: true),
                    ContactPhone = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_BranchWarehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessGoals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Department = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KpiId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetValue = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ProgressPercentage = table.Column<decimal>(type: "numeric", nullable: false),
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
                    table.PrimaryKey("PK_BusinessGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessGoals_KPIs_KpiId",
                        column: x => x.KpiId,
                        principalTable: "KPIs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessGoals_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessHealthScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    OverallScore = table.Column<decimal>(type: "numeric", nullable: false),
                    FinancialHealth = table.Column<decimal>(type: "numeric", nullable: false),
                    OperationalHealth = table.Column<decimal>(type: "numeric", nullable: false),
                    CustomerHealth = table.Column<decimal>(type: "numeric", nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_BusinessHealthScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessHealthScores_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    MetricType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PreviousValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TargetValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PercentageChange = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Period = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MetadataJson = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_BusinessMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessMetrics_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedToUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedToUserName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AssignedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    AssignedByUserName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    AssignmentReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("PK_CommAssignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommConversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChannelId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChannelType = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CustomerEmail = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CustomerPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    AssignedToUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    AssignedToUserName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    UnreadMessagesCount = table.Column<int>(type: "integer", nullable: false),
                    LastMessageAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastMessagePreview = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SlaDueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsSlaBreached = table.Column<bool>(type: "boolean", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CsatRating = table.Column<int>(type: "integer", nullable: true),
                    CsatFeedback = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Tags = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    MetadataJson = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_CommConversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Direction = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    AttachmentsJson = table.Column<string>(type: "text", nullable: false),
                    SenderName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExternalMessageId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    QuickReplyMetadataJson = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_CommMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommMessageTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ChannelType = table.Column<int>(type: "integer", nullable: true),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ShortcutCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ParametersJson = table.Column<string>(type: "text", nullable: false),
                    UsageCount = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_CommMessageTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChannelType = table.Column<int>(type: "integer", nullable: true),
                    NotificationType = table.Column<int>(type: "integer", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_CommNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationChannels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ChannelType = table.Column<int>(type: "integer", nullable: false),
                    ProviderIdentifier = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ConfigurationJson = table.Column<string>(type: "text", nullable: false),
                    WebhookUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    TotalInboundCount = table.Column<int>(type: "integer", nullable: false),
                    TotalOutboundCount = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_CommunicationChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    MetricDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChannelType = table.Column<int>(type: "integer", nullable: true),
                    TotalConversations = table.Column<int>(type: "integer", nullable: false),
                    ActiveConversations = table.Column<int>(type: "integer", nullable: false),
                    ResolvedConversations = table.Column<int>(type: "integer", nullable: false),
                    AverageResponseTimeMinutes = table.Column<decimal>(type: "numeric", nullable: false),
                    AverageResolutionTimeMinutes = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalInboundMessages = table.Column<int>(type: "integer", nullable: false),
                    TotalOutboundMessages = table.Column<int>(type: "integer", nullable: false),
                    AverageCsatScore = table.Column<decimal>(type: "numeric", nullable: false),
                    CsatResponseCount = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_CommunicationMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ColorCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    SlaThresholdMinutes = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
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
                    table.PrimaryKey("PK_ConversationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ColorCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UsageCount = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_ConversationTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPerformances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CustomerSegment = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LifetimeValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AcquisitionCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    LtvToCacRatio = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalOrders = table.Column<int>(type: "integer", nullable: false),
                    AverageOrderValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    RepeatPurchaseRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    FirstPurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastPurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DaysInactive = table.Column<int>(type: "integer", nullable: false),
                    ChurnRiskScore = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_CustomerPerformances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPerformances_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSatisfactionScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CustomerEmail = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CurrentCsat = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    CurrentNps = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    AverageRating = table.Column<decimal>(type: "numeric(4,2)", nullable: false),
                    TotalFeedbacks = table.Column<int>(type: "integer", nullable: false),
                    TotalComplaints = table.Column<int>(type: "integer", nullable: false),
                    RepeatComplaintRate = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    ResolutionSatisfaction = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    ChurnRiskScore = table.Column<int>(type: "integer", nullable: false),
                    LastCalculatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("PK_CustomerSatisfactionScores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExecutiveInsights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BusinessImpact = table.Column<decimal>(type: "numeric", nullable: false),
                    ConfidenceLevel = table.Column<decimal>(type: "numeric", nullable: false),
                    SuggestedAction = table.Column<string>(type: "text", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    IsActioned = table.Column<bool>(type: "boolean", nullable: false),
                    ActionedById = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_ExecutiveInsights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutiveInsights_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CustomerEmail = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Source = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    RatingValue = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AssignedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsUrgent = table.Column<bool>(type: "boolean", nullable: false),
                    Tags = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    HasBeenAnalyzed = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GrowthRecommendations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    RecommendationType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TargetEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    TargetEntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BusinessImpact = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    EstimatedFinancialImpact = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Priority = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ConfidenceScore = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SuggestedAction = table.Column<string>(type: "text", nullable: false),
                    ActionUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ActionedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActionedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CurrentState = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_GrowthRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrowthRecommendations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImprovementRecommendations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ActionPlan = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    BusinessImpact = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EstimatedCustomerImpactCount = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TargetCustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    TargetEntityId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_ImprovementRecommendations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KPIHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    KpiId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_KPIHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KPIHistories_KPIs_KpiId",
                        column: x => x.KpiId,
                        principalTable: "KPIs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    TimeZone = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
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
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketingPerformances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uuid", nullable: true),
                    CampaignName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Channel = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Period = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Spend = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Impressions = table.Column<int>(type: "integer", nullable: false),
                    Clicks = table.Column<int>(type: "integer", nullable: false),
                    Conversions = table.Column<int>(type: "integer", nullable: false),
                    CustomersAcquired = table.Column<int>(type: "integer", nullable: false),
                    CustomerAcquisitionCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    RevenueGenerated = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ReturnOnAdSpend = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MarketingRoiPercentage = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsProfitable = table.Column<bool>(type: "boolean", nullable: false),
                    TotalSpend = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    RevenueAttributed = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    LeadsGenerated = table.Column<int>(type: "integer", nullable: false),
                    ConversionRatePercentage = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Recommendation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
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
                    table.PrimaryKey("PK_MarketingPerformances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketingPerformances_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPerformances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductSku = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Period = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    UnitsSold = table.Column<int>(type: "integer", nullable: false),
                    RevenueGenerated = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalProfit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    GrossMarginPercentage = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ReturnRatePercentage = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    InventoryTurnoverRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsTopPerformer = table.Column<bool>(type: "boolean", nullable: false),
                    IsLeastPerformer = table.Column<bool>(type: "boolean", nullable: false),
                    RecommendationNotes = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_ProductPerformances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPerformances_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfitSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SnapshotDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Period = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    GrossRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CostOfGoodsSold = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    GrossProfit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    GrossMarginPercentage = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OperatingExpenses = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    NetProfit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    NetMarginPercentage = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Ebitda = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ProfitCenterBreakdownJson = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_ProfitSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfitSnapshots_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityType = table.Column<int>(type: "integer", nullable: false),
                    EntityId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerEmail = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    RatingScore = table.Column<decimal>(type: "numeric(4,2)", nullable: false),
                    MaxScore = table.Column<int>(type: "integer", nullable: false),
                    ReviewText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    VerifiedPurchase = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionalSummaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    TotalBranches = table.Column<int>(type: "integer", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalProfit = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalInventoryValue = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalCustomers = table.Column<int>(type: "integer", nullable: false),
                    TotalEmployees = table.Column<int>(type: "integer", nullable: false),
                    TopPerformingBranchName = table.Column<string>(type: "text", nullable: false),
                    TopPerformingBranchId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_RegionalSummaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RevenueSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SnapshotDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MonthlyRecurringRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AnnualRecurringRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    NewRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ExpansionRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ContractionRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ChurnedRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    NetNewRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_RevenueSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevenueSnapshots_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scorecards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Department = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OverallScore = table.Column<decimal>(type: "numeric", nullable: false),
                    KpiSummaryJson = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Scorecards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scorecards_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SentimentAnalyses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetEntityType = table.Column<int>(type: "integer", nullable: false),
                    TargetEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sentiment = table.Column<int>(type: "integer", nullable: false),
                    ConfidenceScore = table.Column<decimal>(type: "numeric(4,3)", nullable: false),
                    UrgencyScore = table.Column<decimal>(type: "numeric(4,2)", nullable: false),
                    MainComplaint = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SuggestedImprovement = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RawModelOutputJson = table.Column<string>(type: "text", nullable: false),
                    EngineProvider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_SentimentAnalyses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    MetricDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AverageFirstResponseTimeMinutes = table.Column<decimal>(type: "numeric(8,2)", nullable: false),
                    AverageResolutionTimeMinutes = table.Column<decimal>(type: "numeric(8,2)", nullable: false),
                    FirstContactResolutionRate = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    CsatAverage = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    TicketReopenRate = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    OpenHighUrgencyCount = table.Column<int>(type: "integer", nullable: false),
                    TotalResolvedTickets = table.Column<int>(type: "integer", nullable: false),
                    TotalNewComplaints = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_ServiceMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionText = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    QuestionType = table.Column<int>(type: "integer", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    OptionsJson = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_SurveyQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerEmail = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Score = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    AnswersJson = table.Column<string>(type: "text", nullable: false),
                    SentimentStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_SurveyResponses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TotalSent = table.Column<int>(type: "integer", nullable: false),
                    TotalCompleted = table.Column<int>(type: "integer", nullable: false),
                    CompletionRate = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    AverageScore = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
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
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceWarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinationWarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransferNumber = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "text", nullable: false),
                    RequestedById = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedByName = table.Column<string>(type: "text", nullable: false),
                    ApprovedById = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedByName = table.Column<string>(type: "text", nullable: true),
                    TransferredItemsJson = table.Column<string>(type: "text", nullable: false),
                    TotalItemsCount = table.Column<int>(type: "integer", nullable: false),
                    TotalTransferValue = table.Column<decimal>(type: "numeric", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShippedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TrackingNotes = table.Column<string>(type: "text", nullable: true),
                    RejectionReason = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_WarehouseTransfers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Configuration = table.Column<string>(type: "jsonb", nullable: false),
                    IsSystemTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_WorkflowTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AutomationActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Configuration = table.Column<string>(type: "jsonb", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_AutomationActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutomationActions_AutomationWorkflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "AutomationWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutomationActions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutomationConditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentConditionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Configuration = table.Column<string>(type: "jsonb", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_AutomationConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutomationConditions_AutomationConditions_ParentConditionId",
                        column: x => x.ParentConditionId,
                        principalTable: "AutomationConditions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AutomationConditions_AutomationWorkflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "AutomationWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutomationConditions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutomationTriggers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Configuration = table.Column<string>(type: "jsonb", nullable: false),
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
                    table.PrimaryKey("PK_AutomationTriggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutomationTriggers_AutomationWorkflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "AutomationWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutomationTriggers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uuid", nullable: false),
                    CronExpression = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NextRunAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastRunAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_AutomationWorkflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "AutomationWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudioWorkflowExecutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ContextData = table.Column<string>(type: "jsonb", nullable: false),
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
                    table.PrimaryKey("PK_StudioWorkflowExecutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudioWorkflowExecutions_AutomationWorkflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "AutomationWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudioWorkflowExecutions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uuid", nullable: false),
                    VersionNumber = table.Column<int>(type: "integer", nullable: false),
                    ConfigurationSnapshot = table.Column<string>(type: "jsonb", nullable: false),
                    ChangeLog = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_WorkflowVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowVersions_AutomationWorkflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "AutomationWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowVersions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DecisionLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DecisionTitle = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    RelatedInsightId = table.Column<Guid>(type: "uuid", nullable: true),
                    DecisionMakerId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExpectedOutcome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DecisionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("PK_DecisionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecisionLogs_ExecutiveInsights_RelatedInsightId",
                        column: x => x.RelatedInsightId,
                        principalTable: "ExecutiveInsights",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DecisionLogs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudioExecutionLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowExecutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    StepType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StepId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "jsonb", nullable: false),
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
                    table.PrimaryKey("PK_StudioExecutionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudioExecutionLogs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudioExecutionLogs_StudioWorkflowExecutions_WorkflowExecut~",
                        column: x => x.WorkflowExecutionId,
                        principalTable: "StudioWorkflowExecutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AiRecommendations_OrganizationId",
                table: "AiRecommendations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomationActions_OrganizationId",
                table: "AutomationActions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomationActions_WorkflowId",
                table: "AutomationActions",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomationConditions_OrganizationId",
                table: "AutomationConditions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomationConditions_ParentConditionId",
                table: "AutomationConditions",
                column: "ParentConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomationConditions_WorkflowId",
                table: "AutomationConditions",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomationTriggers_OrganizationId",
                table: "AutomationTriggers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomationTriggers_WorkflowId",
                table: "AutomationTriggers",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomationWorkflows_OrganizationId",
                table: "AutomationWorkflows",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkMetrics_OrganizationId_ComparisonType_MetricName",
                table: "BenchmarkMetrics",
                columns: new[] { "OrganizationId", "ComparisonType", "MetricName" });

            migrationBuilder.CreateIndex(
                name: "IX_Benchmarks_OrganizationId",
                table: "Benchmarks",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_OrganizationId_IsDeleted_Code",
                table: "Branches",
                columns: new[] { "OrganizationId", "IsDeleted", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_BranchManagers_OrganizationId_BranchId_UserId",
                table: "BranchManagers",
                columns: new[] { "OrganizationId", "BranchId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_BranchPerformances_OrganizationId_BranchId_Year_Month",
                table: "BranchPerformances",
                columns: new[] { "OrganizationId", "BranchId", "Year", "Month" });

            migrationBuilder.CreateIndex(
                name: "IX_BranchWarehouses_OrganizationId_BranchId_IsDeleted",
                table: "BranchWarehouses",
                columns: new[] { "OrganizationId", "BranchId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessGoals_KpiId",
                table: "BusinessGoals",
                column: "KpiId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessGoals_OrganizationId",
                table: "BusinessGoals",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessHealthScores_OrganizationId",
                table: "BusinessHealthScores",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessMetrics_OrganizationId_MetricType_Period",
                table: "BusinessMetrics",
                columns: new[] { "OrganizationId", "MetricType", "Period" });

            migrationBuilder.CreateIndex(
                name: "IX_CommAssignments_ConversationId",
                table: "CommAssignments",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommConversations_OrganizationId",
                table: "CommConversations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommConversations_OrganizationId_Status",
                table: "CommConversations",
                columns: new[] { "OrganizationId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_CommMessages_ConversationId",
                table: "CommMessages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommMessageTemplates_OrganizationId",
                table: "CommMessageTemplates",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommNotifications_OrganizationId_TargetUserId",
                table: "CommNotifications",
                columns: new[] { "OrganizationId", "TargetUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationChannels_OrganizationId",
                table: "CommunicationChannels",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMetrics_OrganizationId",
                table: "CommunicationMetrics",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationStatuses_OrganizationId",
                table: "ConversationStatuses",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationTags_OrganizationId",
                table: "ConversationTags",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPerformances_OrganizationId_CustomerSegment_ChurnRi~",
                table: "CustomerPerformances",
                columns: new[] { "OrganizationId", "CustomerSegment", "ChurnRiskScore" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSatisfactionScores_OrganizationId_CustomerId",
                table: "CustomerSatisfactionScores",
                columns: new[] { "OrganizationId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_DecisionLogs_OrganizationId",
                table: "DecisionLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionLogs_RelatedInsightId",
                table: "DecisionLogs",
                column: "RelatedInsightId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutiveInsights_OrganizationId",
                table: "ExecutiveInsights",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_OrganizationId_IsDeleted_Status",
                table: "Feedbacks",
                columns: new[] { "OrganizationId", "IsDeleted", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_GrowthRecommendations_OrganizationId_Priority_Status",
                table: "GrowthRecommendations",
                columns: new[] { "OrganizationId", "Priority", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ImprovementRecommendations_OrganizationId_Priority_Status",
                table: "ImprovementRecommendations",
                columns: new[] { "OrganizationId", "Priority", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_KPIHistories_KpiId",
                table: "KPIHistories",
                column: "KpiId");

            migrationBuilder.CreateIndex(
                name: "IX_KPIHistories_OrganizationId",
                table: "KPIHistories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_OrganizationId_IsDeleted",
                table: "Locations",
                columns: new[] { "OrganizationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_MarketingPerformances_OrganizationId_Channel_Period",
                table: "MarketingPerformances",
                columns: new[] { "OrganizationId", "Channel", "Period" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPerformances_OrganizationId_Period_IsTopPerformer",
                table: "ProductPerformances",
                columns: new[] { "OrganizationId", "Period", "IsTopPerformer" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfitSnapshots_OrganizationId_SnapshotDate",
                table: "ProfitSnapshots",
                columns: new[] { "OrganizationId", "SnapshotDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_OrganizationId_EntityType_EntityId",
                table: "Ratings",
                columns: new[] { "OrganizationId", "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_RegionalSummaries_OrganizationId_Region_Year_Month",
                table: "RegionalSummaries",
                columns: new[] { "OrganizationId", "Region", "Year", "Month" });

            migrationBuilder.CreateIndex(
                name: "IX_RevenueSnapshots_OrganizationId_SnapshotDate",
                table: "RevenueSnapshots",
                columns: new[] { "OrganizationId", "SnapshotDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_OrganizationId",
                table: "Schedules",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_WorkflowId",
                table: "Schedules",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Scorecards_OrganizationId",
                table: "Scorecards",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SentimentAnalyses_OrganizationId_TargetEntityType_TargetEnt~",
                table: "SentimentAnalyses",
                columns: new[] { "OrganizationId", "TargetEntityType", "TargetEntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceMetrics_OrganizationId_MetricDate",
                table: "ServiceMetrics",
                columns: new[] { "OrganizationId", "MetricDate" });

            migrationBuilder.CreateIndex(
                name: "IX_StudioExecutionLogs_OrganizationId",
                table: "StudioExecutionLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_StudioExecutionLogs_WorkflowExecutionId",
                table: "StudioExecutionLogs",
                column: "WorkflowExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudioWorkflowExecutions_OrganizationId",
                table: "StudioWorkflowExecutions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_StudioWorkflowExecutions_WorkflowId",
                table: "StudioWorkflowExecutions",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestions_SurveyId",
                table: "SurveyQuestions",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_OrganizationId_SurveyId",
                table: "SurveyResponses",
                columns: new[] { "OrganizationId", "SurveyId" });

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_OrganizationId_IsActive",
                table: "Surveys",
                columns: new[] { "OrganizationId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransfers_OrganizationId_SourceWarehouseId_Destina~",
                table: "WarehouseTransfers",
                columns: new[] { "OrganizationId", "SourceWarehouseId", "DestinationWarehouseId" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowVersions_OrganizationId",
                table: "WorkflowVersions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowVersions_WorkflowId",
                table: "WorkflowVersions",
                column: "WorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_Organizations_OrganizationId",
                table: "Forecasts",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KPIs_Organizations_OrganizationId",
                table: "KPIs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecasts_Organizations_OrganizationId",
                table: "Forecasts");

            migrationBuilder.DropForeignKey(
                name: "FK_KPIs_Organizations_OrganizationId",
                table: "KPIs");

            migrationBuilder.DropTable(
                name: "AiActions");

            migrationBuilder.DropTable(
                name: "AiRecommendations");

            migrationBuilder.DropTable(
                name: "AutomationActions");

            migrationBuilder.DropTable(
                name: "AutomationConditions");

            migrationBuilder.DropTable(
                name: "AutomationTriggers");

            migrationBuilder.DropTable(
                name: "BenchmarkMetrics");

            migrationBuilder.DropTable(
                name: "Benchmarks");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "BranchManagers");

            migrationBuilder.DropTable(
                name: "BranchPerformances");

            migrationBuilder.DropTable(
                name: "BranchWarehouses");

            migrationBuilder.DropTable(
                name: "BusinessGoals");

            migrationBuilder.DropTable(
                name: "BusinessHealthScores");

            migrationBuilder.DropTable(
                name: "BusinessMetrics");

            migrationBuilder.DropTable(
                name: "CommAssignments");

            migrationBuilder.DropTable(
                name: "CommConversations");

            migrationBuilder.DropTable(
                name: "CommMessages");

            migrationBuilder.DropTable(
                name: "CommMessageTemplates");

            migrationBuilder.DropTable(
                name: "CommNotifications");

            migrationBuilder.DropTable(
                name: "CommunicationChannels");

            migrationBuilder.DropTable(
                name: "CommunicationMetrics");

            migrationBuilder.DropTable(
                name: "ConversationStatuses");

            migrationBuilder.DropTable(
                name: "ConversationTags");

            migrationBuilder.DropTable(
                name: "CustomerPerformances");

            migrationBuilder.DropTable(
                name: "CustomerSatisfactionScores");

            migrationBuilder.DropTable(
                name: "DecisionLogs");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "GrowthRecommendations");

            migrationBuilder.DropTable(
                name: "ImprovementRecommendations");

            migrationBuilder.DropTable(
                name: "KPIHistories");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "MarketingPerformances");

            migrationBuilder.DropTable(
                name: "ProductPerformances");

            migrationBuilder.DropTable(
                name: "ProfitSnapshots");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "RegionalSummaries");

            migrationBuilder.DropTable(
                name: "RevenueSnapshots");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Scorecards");

            migrationBuilder.DropTable(
                name: "SentimentAnalyses");

            migrationBuilder.DropTable(
                name: "ServiceMetrics");

            migrationBuilder.DropTable(
                name: "StudioExecutionLogs");

            migrationBuilder.DropTable(
                name: "SurveyQuestions");

            migrationBuilder.DropTable(
                name: "SurveyResponses");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "WarehouseTransfers");

            migrationBuilder.DropTable(
                name: "WorkflowTemplates");

            migrationBuilder.DropTable(
                name: "WorkflowVersions");

            migrationBuilder.DropTable(
                name: "ExecutiveInsights");

            migrationBuilder.DropTable(
                name: "StudioWorkflowExecutions");

            migrationBuilder.DropTable(
                name: "AutomationWorkflows");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "KPIs");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "KPIs");

            migrationBuilder.DropColumn(
                name: "IsHigherBetter",
                table: "KPIs");

            migrationBuilder.DropColumn(
                name: "PreviousValue",
                table: "KPIs");

            migrationBuilder.DropColumn(
                name: "ConfidenceLevel",
                table: "Forecasts");

            migrationBuilder.DropColumn(
                name: "Factors",
                table: "Forecasts");

            migrationBuilder.DropColumn(
                name: "GeneratedAt",
                table: "Forecasts");

            migrationBuilder.DropColumn(
                name: "LowerBound",
                table: "Forecasts");

            migrationBuilder.DropColumn(
                name: "ModelUsed",
                table: "Forecasts");

            migrationBuilder.DropColumn(
                name: "UpperBound",
                table: "Forecasts");

            migrationBuilder.RenameColumn(
                name: "MetricName",
                table: "Forecasts",
                newName: "ForecastType");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "KPIs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<decimal>(
                name: "TargetValue",
                table: "KPIs",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "KPIs",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentValue",
                table: "KPIs",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "KPIs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Trend",
                table: "KPIs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "PredictedValue",
                table: "Forecasts",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<double>(
                name: "ConfidenceScore",
                table: "Forecasts",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CurrentValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TargetValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Insights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BusinessImpact = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Priority = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Recommendation = table.Column<string>(type: "text", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    SuggestedAction = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FileUrl = table.Column<string>(type: "text", nullable: true),
                    Filters = table.Column<string>(type: "text", nullable: false),
                    Format = table.Column<string>(type: "text", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GeneratedBy = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KPIs_OrganizationId_Name",
                table: "KPIs",
                columns: new[] { "OrganizationId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Forecasts_OrganizationId_ForecastType_ForecastDate",
                table: "Forecasts",
                columns: new[] { "OrganizationId", "ForecastType", "ForecastDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Goals_OrganizationId",
                table: "Goals",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Insights_OrganizationId",
                table: "Insights",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_OrganizationId",
                table: "Reports",
                column: "OrganizationId");
        }
    }
}
