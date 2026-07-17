using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class QRCodeModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QRCodes_Organizations_OrganizationId",
                table: "QRCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QRCodes",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "QRCodes",
                type: "character varying(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "QRCodes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorCorrectionLevel",
                table: "QRCodes",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "QRCodes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Folder",
                table: "QRCodes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ForegroundColor",
                table: "QRCodes",
                type: "character varying(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "QRCodes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Margin",
                table: "QRCodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OriginalValue",
                table: "QRCodes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "QRCodes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PasswordProtected",
                table: "QRCodes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QRType",
                table: "QRCodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "QrImageUrl",
                table: "QRCodes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScanCount",
                table: "QRCodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ShortCode",
                table: "QRCodes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "QRCodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "QRCodes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<string>>(
                name: "Tags",
                table: "QRCodes",
                type: "text[]",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_QRCodes_ShortCode",
                table: "QRCodes",
                column: "ShortCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QRCodes_Organizations_OrganizationId",
                table: "QRCodes",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QRCodes_Organizations_OrganizationId",
                table: "QRCodes");

            migrationBuilder.DropIndex(
                name: "IX_QRCodes_ShortCode",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "ErrorCorrectionLevel",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "Folder",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "ForegroundColor",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "Margin",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "OriginalValue",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "PasswordProtected",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "QRType",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "QrImageUrl",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "ScanCount",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "ShortCode",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "QRCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QRCodes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AddForeignKey(
                name: "FK_QRCodes_Organizations_OrganizationId",
                table: "QRCodes",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
