using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equilog_backend.Migrations
{
    /// <inheritdoc />
    public partial class StablePropAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Stables",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BoxCount",
                table: "Stables",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "Stables",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PostCode",
                table: "Stables",
                type: "int",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Stables",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Stables");

            migrationBuilder.DropColumn(
                name: "BoxCount",
                table: "Stables");

            migrationBuilder.DropColumn(
                name: "County",
                table: "Stables");

            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "Stables");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Stables");
        }
    }
}
