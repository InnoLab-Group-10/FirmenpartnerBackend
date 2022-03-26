using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirmenpartnerBackend.Migrations
{
    public partial class Addedstudentcontactandprogramtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "People",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ProgramId",
                table: "People",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Semester",
                table: "People",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "People",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_People_ProgramId",
                table: "People",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Programs_ProgramId",
                table: "People",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Programs_ProgramId",
                table: "People");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_People_ProgramId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "People");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "People");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "People");
        }
    }
}
