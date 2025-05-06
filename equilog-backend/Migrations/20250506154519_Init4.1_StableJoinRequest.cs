using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equilog_backend.Migrations
{
    /// <inheritdoc />
    public partial class Init41_StableJoinRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StableJoinRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdFk = table.Column<int>(type: "int", nullable: false),
                    StableIdFk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StableJoinRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StableJoinRequests_Stables_StableIdFk",
                        column: x => x.StableIdFk,
                        principalTable: "Stables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StableJoinRequests_Users_UserIdFk",
                        column: x => x.UserIdFk,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StableJoinRequests_StableIdFk",
                table: "StableJoinRequests",
                column: "StableIdFk");

            migrationBuilder.CreateIndex(
                name: "IX_StableJoinRequests_UserIdFk",
                table: "StableJoinRequests",
                column: "UserIdFk");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StableJoinRequests");
        }
    }
}
