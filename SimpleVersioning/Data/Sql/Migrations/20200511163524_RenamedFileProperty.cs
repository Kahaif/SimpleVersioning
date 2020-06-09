using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleVersioning.Data.Sql.Migrations
{
    public partial class RenamedFileProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Files_FileId",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Properties",
                table: "Properties");

            migrationBuilder.RenameTable(
                name: "Properties",
                newName: "FileProperties");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_FileId",
                table: "FileProperties",
                newName: "IX_FileProperties_FileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileProperties",
                table: "FileProperties",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileProperties_Files_FileId",
                table: "FileProperties",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileProperties_Files_FileId",
                table: "FileProperties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileProperties",
                table: "FileProperties");

            migrationBuilder.RenameTable(
                name: "FileProperties",
                newName: "Properties");

            migrationBuilder.RenameIndex(
                name: "IX_FileProperties_FileId",
                table: "Properties",
                newName: "IX_Properties_FileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Properties",
                table: "Properties",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Files_FileId",
                table: "Properties",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
