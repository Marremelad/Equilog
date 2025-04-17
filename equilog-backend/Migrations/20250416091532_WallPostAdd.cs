using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equilog_backend.Migrations
{
    /// <inheritdoc />
    public partial class WallPostAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WallPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Body = table.Column<string>(type: "nvarchar(280)", maxLength: 280, nullable: true),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastEdited = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StableIdFk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WallPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WallPosts_Stables_StableIdFk",
                        column: x => x.StableIdFk,
                        principalTable: "Stables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WallPosts_StableIdFk",
                table: "WallPosts",
                column: "StableIdFk",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WallPosts");
        }
    }
}
