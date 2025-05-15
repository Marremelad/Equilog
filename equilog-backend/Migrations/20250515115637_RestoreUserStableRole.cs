using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equilog_backend.Migrations
{
    /// <inheritdoc />
    public partial class RestoreUserStableRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "UserStables",
                newName: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "UserStables",
                newName: "UserRole");
        }
    }
}
