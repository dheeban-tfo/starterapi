using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarterApi.Infrastructure.Persistence.Migrations.TenantDb
{
    /// <inheritdoc />
    public partial class UpdateDocumentStorageToAzureBlob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentId",
                table: "DocumentVersions");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "DocumentVersions",
                newName: "BlobUrl");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Documents",
                newName: "BlobUrl");

            migrationBuilder.AddColumn<string>(
                name: "BlobPath",
                table: "DocumentVersions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "DocumentVersions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "DocumentVersions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "BlobPath",
                table: "Documents",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Documents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentId",
                table: "DocumentVersions",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentId",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "BlobPath",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "BlobPath",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "BlobUrl",
                table: "DocumentVersions",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "BlobUrl",
                table: "Documents",
                newName: "FilePath");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentId",
                table: "DocumentVersions",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
