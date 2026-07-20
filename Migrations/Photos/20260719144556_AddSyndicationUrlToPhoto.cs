using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations.Photos
{
    /// <inheritdoc />
    public partial class AddSyndicationUrlToPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SyndicationUrl",
                table: "Photos",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SyndicationUrl",
                table: "Photos");
        }
    }
}
