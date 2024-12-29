using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarterApi.Infrastructure.Persistence.Migrations.TenantDb
{
    /// <inheritdoc />
    public partial class AddDocumentIdToFacilityImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "FacilityImages");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FacilityImages");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "FacilityImages");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "FacilityImages");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "FacilityImages");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FacilityImages",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId",
                table: "FacilityImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FacilityImages_DocumentId",
                table: "FacilityImages",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityImages_Documents_DocumentId",
                table: "FacilityImages",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacilityImages_Documents_DocumentId",
                table: "FacilityImages");

            migrationBuilder.DropIndex(
                name: "IX_FacilityImages_DocumentId",
                table: "FacilityImages");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "FacilityImages");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FacilityImages",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "FacilityImages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FacilityImages",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "FacilityImages",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "FacilityImages",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "FacilityImages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
