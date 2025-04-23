using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equilog_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdFkToCalendarEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "CalendarEvents",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserIdFk",
                table: "CalendarEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_UserIdFk",
                table: "CalendarEvents",
                column: "UserIdFk");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_Users_UserIdFk",
                table: "CalendarEvents",
                column: "UserIdFk",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_Users_UserIdFk",
                table: "CalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEvents_UserIdFk",
                table: "CalendarEvents");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CalendarEvents");

            migrationBuilder.DropColumn(
                name: "UserIdFk",
                table: "CalendarEvents");
        }
    }
}
