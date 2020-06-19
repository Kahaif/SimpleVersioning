using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleVersioning.Data.Sql.Migrations
{
    public partial class FileVersionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "LastUpdatedTime",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Files");

            migrationBuilder.AddColumn<int>(
                name: "FileVersionId",
                table: "FileProperties",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hash = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastUpdatedTime = table.Column<DateTime>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Path = table.Column<string>(nullable: false),
                    Content = table.Column<byte[]>(nullable: true),
                    FileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileVersions_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileProperties_FileVersionId",
                table: "FileProperties",
                column: "FileVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FileVersions_FileId",
                table: "FileVersions",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileProperties_FileVersions_FileVersionId",
                table: "FileProperties",
                column: "FileVersionId",
                principalTable: "FileVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileProperties_FileVersions_FileVersionId",
                table: "FileProperties");

            migrationBuilder.DropTable(
                name: "FileVersions");

            migrationBuilder.DropIndex(
                name: "IX_FileProperties_FileVersionId",
                table: "FileProperties");

            migrationBuilder.DropColumn(
                name: "FileVersionId",
                table: "FileProperties");

            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "Files",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Files",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedTime",
                table: "Files",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
