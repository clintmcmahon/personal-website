using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations.Blog
{
    /// <inheritdoc />
    public partial class AddNowFieldsToPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brewing",
                table: "Pages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListeningTo",
                table: "Pages",
                type: "TEXT",
                nullable: true);

            // Carry over the value that was hardcoded in Services/NowStatus.cs.
            migrationBuilder.Sql("UPDATE Pages SET Brewing = 'Verve Santa Rosa 1900' WHERE Slug = 'now';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brewing",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "ListeningTo",
                table: "Pages");
        }
    }
}
