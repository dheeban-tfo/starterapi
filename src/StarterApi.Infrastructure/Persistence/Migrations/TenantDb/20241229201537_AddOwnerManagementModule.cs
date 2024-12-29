using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarterApi.Infrastructure.Persistence.Migrations.TenantDb
{
    /// <inheritdoc />
    public partial class AddOwnerManagementModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Blocks_BlockId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Units_UnitId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilities_Societies_SocietyId",
                table: "Facilities");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSlots_Societies_SocietyId",
                table: "ParkingSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalContracts_Residents_TenantId",
                table: "RentalContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalContracts_Units_UnitId",
                table: "RentalContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Owners_CurrentOwnerId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Owners_OwnerId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_OwnerId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Units");

            migrationBuilder.AlterColumn<string>(
                name: "Terms",
                table: "RentalContracts",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMode",
                table: "RentalContracts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentFrequency",
                table: "RentalContracts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ParkingSlots",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ParkingSlots",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SlotNumber",
                table: "ParkingSlots",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "ParkingSlots",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "OwnershipDocumentNumber",
                table: "Owners",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "OwnershipPercentage",
                table: "Owners",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Owners",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Facilities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Facilities",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Facilities",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Announcements",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "Announcements",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Announcements",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Audience",
                table: "Announcements",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "OwnerDocuments",
                columns: table => new
                {
                    DocumentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerDocuments", x => new { x.DocumentsId, x.OwnerId });
                    table.ForeignKey(
                        name: "FK_OwnerDocuments_Documents_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OwnerDocuments_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OwnershipHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransferType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransferReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PreviousOwnerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TransferDocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnershipHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OwnershipHistories_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OwnershipHistories_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OwnershipTransferRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransferType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnershipTransferRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OwnershipTransferRequests_Owners_CurrentOwnerId",
                        column: x => x.CurrentOwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OwnershipTransferRequests_Owners_NewOwnerId",
                        column: x => x.NewOwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OwnershipTransferRequests_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OwnershipTransferRequests_Users_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OwnershipHistoryDocuments",
                columns: table => new
                {
                    DocumentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnershipHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnershipHistoryDocuments", x => new { x.DocumentsId, x.OwnershipHistoryId });
                    table.ForeignKey(
                        name: "FK_OwnershipHistoryDocuments_Documents_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OwnershipHistoryDocuments_OwnershipHistories_OwnershipHistoryId",
                        column: x => x.OwnershipHistoryId,
                        principalTable: "OwnershipHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OwnershipTransferDocuments",
                columns: table => new
                {
                    OwnershipTransferRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportingDocumentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnershipTransferDocuments", x => new { x.OwnershipTransferRequestId, x.SupportingDocumentsId });
                    table.ForeignKey(
                        name: "FK_OwnershipTransferDocuments_Documents_SupportingDocumentsId",
                        column: x => x.SupportingDocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OwnershipTransferDocuments_OwnershipTransferRequests_OwnershipTransferRequestId",
                        column: x => x.OwnershipTransferRequestId,
                        principalTable: "OwnershipTransferRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDocuments_OwnerId",
                table: "OwnerDocuments",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipHistories_OwnerId",
                table: "OwnershipHistories",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipHistories_UnitId",
                table: "OwnershipHistories",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipHistoryDocuments_OwnershipHistoryId",
                table: "OwnershipHistoryDocuments",
                column: "OwnershipHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipTransferDocuments_SupportingDocumentsId",
                table: "OwnershipTransferDocuments",
                column: "SupportingDocumentsId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipTransferRequests_ApprovedBy",
                table: "OwnershipTransferRequests",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipTransferRequests_CurrentOwnerId",
                table: "OwnershipTransferRequests",
                column: "CurrentOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipTransferRequests_NewOwnerId",
                table: "OwnershipTransferRequests",
                column: "NewOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipTransferRequests_UnitId",
                table: "OwnershipTransferRequests",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Blocks_BlockId",
                table: "Announcements",
                column: "BlockId",
                principalTable: "Blocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Units_UnitId",
                table: "Announcements",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Facilities_Societies_SocietyId",
                table: "Facilities",
                column: "SocietyId",
                principalTable: "Societies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSlots_Societies_SocietyId",
                table: "ParkingSlots",
                column: "SocietyId",
                principalTable: "Societies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalContracts_Residents_TenantId",
                table: "RentalContracts",
                column: "TenantId",
                principalTable: "Residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalContracts_Units_UnitId",
                table: "RentalContracts",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Owners_CurrentOwnerId",
                table: "Units",
                column: "CurrentOwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Blocks_BlockId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Units_UnitId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilities_Societies_SocietyId",
                table: "Facilities");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSlots_Societies_SocietyId",
                table: "ParkingSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalContracts_Residents_TenantId",
                table: "RentalContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalContracts_Units_UnitId",
                table: "RentalContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Owners_CurrentOwnerId",
                table: "Units");

            migrationBuilder.DropTable(
                name: "OwnerDocuments");

            migrationBuilder.DropTable(
                name: "OwnershipHistoryDocuments");

            migrationBuilder.DropTable(
                name: "OwnershipTransferDocuments");

            migrationBuilder.DropTable(
                name: "OwnershipHistories");

            migrationBuilder.DropTable(
                name: "OwnershipTransferRequests");

            migrationBuilder.DropColumn(
                name: "OwnershipDocumentNumber",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "OwnershipPercentage",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Owners");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Units",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Terms",
                table: "RentalContracts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMode",
                table: "RentalContracts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentFrequency",
                table: "RentalContracts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ParkingSlots",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ParkingSlots",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "SlotNumber",
                table: "ParkingSlots",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "ParkingSlots",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Facilities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Facilities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Facilities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "Audience",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Units_OwnerId",
                table: "Units",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Blocks_BlockId",
                table: "Announcements",
                column: "BlockId",
                principalTable: "Blocks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Units_UnitId",
                table: "Announcements",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Facilities_Societies_SocietyId",
                table: "Facilities",
                column: "SocietyId",
                principalTable: "Societies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSlots_Societies_SocietyId",
                table: "ParkingSlots",
                column: "SocietyId",
                principalTable: "Societies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalContracts_Residents_TenantId",
                table: "RentalContracts",
                column: "TenantId",
                principalTable: "Residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalContracts_Units_UnitId",
                table: "RentalContracts",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Owners_CurrentOwnerId",
                table: "Units",
                column: "CurrentOwnerId",
                principalTable: "Owners",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Owners_OwnerId",
                table: "Units",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id");
        }
    }
}
