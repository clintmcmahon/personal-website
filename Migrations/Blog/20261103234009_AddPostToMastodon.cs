using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations.Blog
{
    /// <inheritdoc />
    public partial class AddPostToMastodon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PostToMastodon",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostToMastodon",
                table: "Posts");
        }
    }
}
