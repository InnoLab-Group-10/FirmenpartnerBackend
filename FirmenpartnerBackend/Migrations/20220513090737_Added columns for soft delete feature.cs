using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirmenpartnerBackend.Migrations
{
    public partial class Addedcolumnsforsoftdeletefeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTimestamp",
                table: "TimelineEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TimelineEntries",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTimestamp",
                table: "Programs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Programs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTimestamp",
                table: "People",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "People",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTimestamp",
                table: "FileEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FileEntries",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTimestamp",
                table: "CompanyLocations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CompanyLocations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTimestamp",
                table: "CompanyAssignments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CompanyAssignments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTimestamp",
                table: "Companies",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Companies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedTimestamp",
                table: "TimelineEntries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TimelineEntries");

            migrationBuilder.DropColumn(
                name: "DeletedTimestamp",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "DeletedTimestamp",
                table: "People");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "People");

            migrationBuilder.DropColumn(
                name: "DeletedTimestamp",
                table: "FileEntries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FileEntries");

            migrationBuilder.DropColumn(
                name: "DeletedTimestamp",
                table: "CompanyLocations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CompanyLocations");

            migrationBuilder.DropColumn(
                name: "DeletedTimestamp",
                table: "CompanyAssignments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CompanyAssignments");

            migrationBuilder.DropColumn(
                name: "DeletedTimestamp",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Companies");
        }
    }
}
