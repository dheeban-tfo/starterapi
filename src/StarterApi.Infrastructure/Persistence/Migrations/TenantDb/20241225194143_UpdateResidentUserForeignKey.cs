using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarterApi.Infrastructure.Persistence.Migrations.TenantDb
{
    /// <inheritdoc />
    public partial class UpdateResidentUserForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Residents_User_UserId",
                table: "Residents");

            migrationBuilder.AddForeignKey(
                name: "FK_Residents_Users_UserId",
                table: "Residents",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Residents_Users_UserId",
                table: "Residents");

            migrationBuilder.AddForeignKey(
                name: "FK_Residents_User_UserId",
                table: "Residents",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
