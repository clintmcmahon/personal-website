using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations.Guestbook
{
    /// <inheritdoc />
    public partial class AddGuestbookApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "GuestbookEntries",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "GuestbookEntries");
        }
    }
}
