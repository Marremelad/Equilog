using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equilog_backend.Migrations
{
    /// <inheritdoc />
    public partial class HorseProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoreInformation",
                table: "Horses",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentBox",
                table: "Horses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Horses",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Horses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Horses",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoreInformation",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "CurrentBox",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Horses");
        }
    }
}
