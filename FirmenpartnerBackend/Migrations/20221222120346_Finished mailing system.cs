using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirmenpartnerBackend.Migrations
{
    public partial class Finishedmailingsystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Note",
                table: "MailingListsEntries",
                newName: "Suffix");

            migrationBuilder.RenameColumn(
                name: "Mail",
                table: "MailingListsEntries",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "MailingListsEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "MailingListsEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "MailingListsEntries",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "MailingListsEntries");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "MailingListsEntries");

            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "MailingListsEntries");

            migrationBuilder.RenameColumn(
                name: "Suffix",
                table: "MailingListsEntries",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "MailingListsEntries",
                newName: "Mail");
        }
    }
}
