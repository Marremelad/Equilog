using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equilog_backend.Migrations
{
    /// <inheritdoc />
    public partial class PasswordResetRequest_20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordResetRequests_Users_UserIdFk",
                table: "PasswordResetRequests");

            migrationBuilder.DropIndex(
                name: "IX_PasswordResetRequests_UserIdFk",
                table: "PasswordResetRequests");

            migrationBuilder.DropColumn(
                name: "UserIdFk",
                table: "PasswordResetRequests");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "PasswordResetRequests",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "PasswordResetRequests");

            migrationBuilder.AddColumn<int>(
                name: "UserIdFk",
                table: "PasswordResetRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetRequests_UserIdFk",
                table: "PasswordResetRequests",
                column: "UserIdFk");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResetRequests_Users_UserIdFk",
                table: "PasswordResetRequests",
                column: "UserIdFk",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
