using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equilog_backend.Migrations
{
    /// <inheritdoc />
    public partial class InitManytoManyRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Horses",
                newName: "Age");

            migrationBuilder.RenameColumn(
                name: "HorseId",
                table: "Horses",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Horses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Horses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Breed",
                table: "Horses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StableIdFk",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Stable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StableHorse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StableIdFk = table.Column<int>(type: "int", nullable: false),
                    HorseIdFk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StableHorse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StableHorse_Horses_HorseIdFk",
                        column: x => x.HorseIdFk,
                        principalTable: "Horses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StableHorse_Stable_StableIdFk",
                        column: x => x.StableIdFk,
                        principalTable: "Stable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHorse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdFk = table.Column<int>(type: "int", nullable: false),
                    HorseIdFk = table.Column<int>(type: "int", nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHorse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserHorse_Horses_HorseIdFk",
                        column: x => x.HorseIdFk,
                        principalTable: "Horses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHorse_User_UserIdFk",
                        column: x => x.UserIdFk,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdFk = table.Column<int>(type: "int", nullable: false),
                    StableIdFk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStable_Stable_StableIdFk",
                        column: x => x.StableIdFk,
                        principalTable: "Stable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStable_User_UserIdFk",
                        column: x => x.UserIdFk,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_StableIdFk",
                table: "Events",
                column: "StableIdFk");

            migrationBuilder.CreateIndex(
                name: "IX_StableHorse_HorseIdFk",
                table: "StableHorse",
                column: "HorseIdFk");

            migrationBuilder.CreateIndex(
                name: "IX_StableHorse_StableIdFk",
                table: "StableHorse",
                column: "StableIdFk");

            migrationBuilder.CreateIndex(
                name: "IX_UserHorse_HorseIdFk",
                table: "UserHorse",
                column: "HorseIdFk");

            migrationBuilder.CreateIndex(
                name: "IX_UserHorse_UserIdFk",
                table: "UserHorse",
                column: "UserIdFk");

            migrationBuilder.CreateIndex(
                name: "IX_UserStable_StableIdFk",
                table: "UserStable",
                column: "StableIdFk");

            migrationBuilder.CreateIndex(
                name: "IX_UserStable_UserIdFk",
                table: "UserStable",
                column: "UserIdFk");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Stable_StableIdFk",
                table: "Events",
                column: "StableIdFk",
                principalTable: "Stable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Stable_StableIdFk",
                table: "Events");

            migrationBuilder.DropTable(
                name: "StableHorse");

            migrationBuilder.DropTable(
                name: "UserHorse");

            migrationBuilder.DropTable(
                name: "UserStable");

            migrationBuilder.DropTable(
                name: "Stable");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Events_StableIdFk",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "StableIdFk",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Horses",
                newName: "DateOfBirth");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Horses",
                newName: "HorseId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Horses",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Horses",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Breed",
                table: "Horses",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
