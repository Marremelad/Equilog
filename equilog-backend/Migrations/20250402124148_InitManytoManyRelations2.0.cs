using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equilog_backend.Migrations
{
    /// <inheritdoc />
    public partial class InitManytoManyRelations20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Stable_StableIdFk",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_StableHorse_Horses_HorseIdFk",
                table: "StableHorse");

            migrationBuilder.DropForeignKey(
                name: "FK_StableHorse_Stable_StableIdFk",
                table: "StableHorse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHorse_Horses_HorseIdFk",
                table: "UserHorse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHorse_User_UserIdFk",
                table: "UserHorse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStable_Stable_StableIdFk",
                table: "UserStable");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStable_User_UserIdFk",
                table: "UserStable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStable",
                table: "UserStable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHorse",
                table: "UserHorse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StableHorse",
                table: "StableHorse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stable",
                table: "Stable");

            migrationBuilder.RenameTable(
                name: "UserStable",
                newName: "UserStables");

            migrationBuilder.RenameTable(
                name: "UserHorse",
                newName: "UserHorses");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "StableHorse",
                newName: "StableHorses");

            migrationBuilder.RenameTable(
                name: "Stable",
                newName: "Stables");

            migrationBuilder.RenameIndex(
                name: "IX_UserStable_UserIdFk",
                table: "UserStables",
                newName: "IX_UserStables_UserIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_UserStable_StableIdFk",
                table: "UserStables",
                newName: "IX_UserStables_StableIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_UserHorse_UserIdFk",
                table: "UserHorses",
                newName: "IX_UserHorses_UserIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_UserHorse_HorseIdFk",
                table: "UserHorses",
                newName: "IX_UserHorses_HorseIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_StableHorse_StableIdFk",
                table: "StableHorses",
                newName: "IX_StableHorses_StableIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_StableHorse_HorseIdFk",
                table: "StableHorses",
                newName: "IX_StableHorses_HorseIdFk");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStables",
                table: "UserStables",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHorses",
                table: "UserHorses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StableHorses",
                table: "StableHorses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stables",
                table: "Stables",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdFk = table.Column<int>(type: "int", nullable: false),
                    EventIdFk = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_UserEvents_EventIdFk",
                table: "UserEvents",
                column: "EventIdFk");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_UserIdFk",
                table: "UserEvents",
                column: "UserIdFk");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Stables_StableIdFk",
                table: "Events",
                column: "StableIdFk",
                principalTable: "Stables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StableHorses_Horses_HorseIdFk",
                table: "StableHorses",
                column: "HorseIdFk",
                principalTable: "Horses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StableHorses_Stables_StableIdFk",
                table: "StableHorses",
                column: "StableIdFk",
                principalTable: "Stables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHorses_Horses_HorseIdFk",
                table: "UserHorses",
                column: "HorseIdFk",
                principalTable: "Horses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHorses_Users_UserIdFk",
                table: "UserHorses",
                column: "UserIdFk",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStables_Stables_StableIdFk",
                table: "UserStables",
                column: "StableIdFk",
                principalTable: "Stables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStables_Users_UserIdFk",
                table: "UserStables",
                column: "UserIdFk",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Stables_StableIdFk",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_StableHorses_Horses_HorseIdFk",
                table: "StableHorses");

            migrationBuilder.DropForeignKey(
                name: "FK_StableHorses_Stables_StableIdFk",
                table: "StableHorses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHorses_Horses_HorseIdFk",
                table: "UserHorses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHorses_Users_UserIdFk",
                table: "UserHorses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStables_Stables_StableIdFk",
                table: "UserStables");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStables_Users_UserIdFk",
                table: "UserStables");

            migrationBuilder.DropTable(
                name: "UserEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStables",
                table: "UserStables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHorses",
                table: "UserHorses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stables",
                table: "Stables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StableHorses",
                table: "StableHorses");

            migrationBuilder.RenameTable(
                name: "UserStables",
                newName: "UserStable");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "UserHorses",
                newName: "UserHorse");

            migrationBuilder.RenameTable(
                name: "Stables",
                newName: "Stable");

            migrationBuilder.RenameTable(
                name: "StableHorses",
                newName: "StableHorse");

            migrationBuilder.RenameIndex(
                name: "IX_UserStables_UserIdFk",
                table: "UserStable",
                newName: "IX_UserStable_UserIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_UserStables_StableIdFk",
                table: "UserStable",
                newName: "IX_UserStable_StableIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_UserHorses_UserIdFk",
                table: "UserHorse",
                newName: "IX_UserHorse_UserIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_UserHorses_HorseIdFk",
                table: "UserHorse",
                newName: "IX_UserHorse_HorseIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_StableHorses_StableIdFk",
                table: "StableHorse",
                newName: "IX_StableHorse_StableIdFk");

            migrationBuilder.RenameIndex(
                name: "IX_StableHorses_HorseIdFk",
                table: "StableHorse",
                newName: "IX_StableHorse_HorseIdFk");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStable",
                table: "UserStable",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHorse",
                table: "UserHorse",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stable",
                table: "Stable",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StableHorse",
                table: "StableHorse",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Stable_StableIdFk",
                table: "Events",
                column: "StableIdFk",
                principalTable: "Stable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StableHorse_Horses_HorseIdFk",
                table: "StableHorse",
                column: "HorseIdFk",
                principalTable: "Horses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StableHorse_Stable_StableIdFk",
                table: "StableHorse",
                column: "StableIdFk",
                principalTable: "Stable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHorse_Horses_HorseIdFk",
                table: "UserHorse",
                column: "HorseIdFk",
                principalTable: "Horses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHorse_User_UserIdFk",
                table: "UserHorse",
                column: "UserIdFk",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStable_Stable_StableIdFk",
                table: "UserStable",
                column: "StableIdFk",
                principalTable: "Stable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStable_User_UserIdFk",
                table: "UserStable",
                column: "UserIdFk",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
