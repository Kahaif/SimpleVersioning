using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleVersioning.Data.MySQL.Migrations
{
    public partial class RenamedResourceNameFileModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RessourceName",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "Files",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "RessourceName",
                table: "Files",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "");
        }
    }
}
