using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equilog_backend.Migrations
{
    /// <inheritdoc />
    public partial class NewModelNamesCalendarEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StablePost_Stables_StableIdFk",
                table: "StablePost");

            migrationBuilder.DropForeignKey(
                name: "FK_StablePost_Users_UserIdFk",
                table: "StablePost");

            migrationBuilder.DropTable(
                name: "UserEvents");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StablePost",
                table: "StablePost");

            migrationBuilder.RenameTable(
                name: "StablePost",
                newName: "StablePosts");

            migrationBuilder.RenameIndex(
                name: "IX_StablePost_UserIdFk",
                table: "StablePosts",
                newName: "IX_StablePosts_UserIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_StablePost_StableIdFk",
                table: "StablePosts",
                newName: "IX_StablePosts_StableIdFk");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StablePosts",
                table: "StablePosts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CalendarEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StableIdFk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarEvents_Stables_StableIdFk",
                        column: x => x.StableIdFk,
                        principalTable: "Stables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCalendarEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdFk = table.Column<int>(type: "int", nullable: false),
                    EventIdFk = table.Column<int>(type: "int", nullable: false),
                    CalendarEventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCalendarEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCalendarEvents_CalendarEvents_CalendarEventId",
                        column: x => x.CalendarEventId,
                        principalTable: "CalendarEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCalendarEvents_Users_UserIdFk",
                        column: x => x.UserIdFk,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_StableIdFk",
                table: "CalendarEvents",
                column: "StableIdFk");

            migrationBuilder.CreateIndex(
                name: "IX_UserCalendarEvents_CalendarEventId",
                table: "UserCalendarEvents",
                column: "CalendarEventId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCalendarEvents_UserIdFk",
                table: "UserCalendarEvents",
                column: "UserIdFk");

            migrationBuilder.AddForeignKey(
                name: "FK_StablePosts_Stables_StableIdFk",
                table: "StablePosts",
                column: "StableIdFk",
                principalTable: "Stables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StablePosts_Users_UserIdFk",
                table: "StablePosts",
                column: "UserIdFk",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StablePosts_Stables_StableIdFk",
                table: "StablePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_StablePosts_Users_UserIdFk",
                table: "StablePosts");

            migrationBuilder.DropTable(
                name: "UserCalendarEvents");

            migrationBuilder.DropTable(
                name: "CalendarEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StablePosts",
                table: "StablePosts");

            migrationBuilder.RenameTable(
                name: "StablePosts",
                newName: "StablePost");

            migrationBuilder.RenameIndex(
                name: "IX_StablePosts_UserIdFk",
                table: "StablePost",
                newName: "IX_StablePost_UserIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_StablePosts_StableIdFk",
                table: "StablePost",
                newName: "IX_StablePost_StableIdFk");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StablePost",
                table: "StablePost",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StableIdFk = table.Column<int>(type: "int", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Stables_StableIdFk",
                        column: x => x.StableIdFk,
                        principalTable: "Stables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventIdFk = table.Column<int>(type: "int", nullable: false),
                    UserIdFk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEvents_Events_EventIdFk",
                        column: x => x.EventIdFk,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEvents_Users_UserIdFk",
                        column: x => x.UserIdFk,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_StableIdFk",
                table: "Events",
                column: "StableIdFk");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_EventIdFk",
                table: "UserEvents",
                column: "EventIdFk");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_UserIdFk",
                table: "UserEvents",
                column: "UserIdFk");

            migrationBuilder.AddForeignKey(
                name: "FK_StablePost_Stables_StableIdFk",
                table: "StablePost",
                column: "StableIdFk",
                principalTable: "Stables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StablePost_Users_UserIdFk",
                table: "StablePost",
                column: "UserIdFk",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
