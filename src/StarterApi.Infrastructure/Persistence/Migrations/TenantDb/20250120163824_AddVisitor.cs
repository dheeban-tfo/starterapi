using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarterApi.Infrastructure.Persistence.Migrations.TenantDb
{
    /// <inheritdoc />
    public partial class AddVisitor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitors_Residents_RegisteredById",
                table: "Visitors");

            migrationBuilder.DropForeignKey(
                name: "FK_Visitors_Units_VisitedUnitId",
                table: "Visitors");

            migrationBuilder.DropTable(
                name: "OwnerDocuments");

            migrationBuilder.DropIndex(
                name: "IX_Visitors_RegisteredById",
                table: "Visitors");

            migrationBuilder.DropIndex(
                name: "IX_Visitors_VisitedUnitId",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "CheckOutTime",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "RegisteredById",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "VisitedUnitId",
                table: "Visitors");

            migrationBuilder.RenameColumn(
                name: "VehicleRegistrationNumber",
                table: "Visitors",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "Visitors",
                newName: "PurposeOfVisit");

            migrationBuilder.RenameColumn(
                name: "Purpose",
                table: "Visitors",
                newName: "VisitorName");

            migrationBuilder.RenameColumn(
                name: "CheckInTime",
                table: "Visitors",
                newName: "ExpectedVisitDate");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpectedVisitEndTime",
                table: "Visitors",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpectedVisitStartTime",
                table: "Visitors",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "IsParking",
                table: "Visitors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ResidentId",
                table: "Visitors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_ResidentId",
                table: "Visitors",
                column: "ResidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitors_Individuals_ResidentId",
                table: "Visitors",
                column: "ResidentId",
                principalTable: "Individuals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitors_Individuals_ResidentId",
                table: "Visitors");

            migrationBuilder.DropIndex(
                name: "IX_Visitors_ResidentId",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "ExpectedVisitEndTime",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "ExpectedVisitStartTime",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "IsParking",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "ResidentId",
                table: "Visitors");

            migrationBuilder.RenameColumn(
                name: "VisitorName",
                table: "Visitors",
                newName: "Purpose");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Visitors",
                newName: "VehicleRegistrationNumber");

            migrationBuilder.RenameColumn(
                name: "PurposeOfVisit",
                table: "Visitors",
                newName: "Remarks");

            migrationBuilder.RenameColumn(
                name: "ExpectedVisitDate",
                table: "Visitors",
                newName: "CheckInTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOutTime",
                table: "Visitors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Visitors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Visitors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "RegisteredById",
                table: "Visitors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VisitedUnitId",
                table: "Visitors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_RegisteredById",
                table: "Visitors",
                column: "RegisteredById");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_VisitedUnitId",
                table: "Visitors",
                column: "VisitedUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDocuments_OwnerId",
                table: "OwnerDocuments",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitors_Residents_RegisteredById",
                table: "Visitors",
                column: "RegisteredById",
                principalTable: "Residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visitors_Units_VisitedUnitId",
                table: "Visitors",
                column: "VisitedUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
