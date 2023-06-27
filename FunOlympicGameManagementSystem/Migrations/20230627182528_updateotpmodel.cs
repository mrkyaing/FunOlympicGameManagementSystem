using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FunOlympicGameManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class updateotpmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "OTP",
                newName: "EmailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailId",
                table: "OTP",
                newName: "UserId");
        }
    }
}
