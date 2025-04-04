using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equilog_backend.Migrations
{
    /// <inheritdoc />
    public partial class StablePostModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StablePost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdFk = table.Column<int>(type: "int", nullable: false),
                    StableIdFk = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(510)", maxLength: 510, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 4094, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pinned = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StablePost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StablePost_Stables_StableIdFk",
                        column: x => x.StableIdFk,
                        principalTable: "Stables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StablePost_Users_UserIdFk",
                        column: x => x.UserIdFk,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StablePost_StableIdFk",
                table: "StablePost",
                column: "StableIdFk");

            migrationBuilder.CreateIndex(
                name: "IX_StablePost_UserIdFk",
                table: "StablePost",
                column: "UserIdFk");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StablePost");
        }
    }
}
