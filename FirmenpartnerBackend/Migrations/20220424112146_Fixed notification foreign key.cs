using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirmenpartnerBackend.Migrations
{
    public partial class Fixednotificationforeignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_People_RecipientId",
                table: "Notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_RecipientId",
                table: "Notifications",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_RecipientId",
                table: "Notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_People_RecipientId",
                table: "Notifications",
                column: "RecipientId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
