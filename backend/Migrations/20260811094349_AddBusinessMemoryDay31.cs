using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessMemoryDay31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessMemories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemoryType = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    StructuredData = table.Column<string>(type: "jsonb", nullable: true),
                    SourceModule = table.Column<string>(type: "text", nullable: false),
                    SourceEntityId = table.Column<string>(type: "text", nullable: false),
                    Importance = table.Column<int>(type: "integer", nullable: false),
                    Confidence = table.Column<int>(type: "integer", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_BusinessMemories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessMemories_ExpiresAt",
                table: "BusinessMemories",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessMemories_OrganizationId",
                table: "BusinessMemories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessMemories_OrganizationId_MemoryType_IsActive",
                table: "BusinessMemories",
                columns: new[] { "OrganizationId", "MemoryType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessMemories_OrganizationId_SourceModule_SourceEntityId",
                table: "BusinessMemories",
                columns: new[] { "OrganizationId", "SourceModule", "SourceEntityId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessMemories");
        }
    }
}
