using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations.Blog
{
    /// <inheritdoc />
    public partial class AddPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    MetaTitle = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Tagline = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Published = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShowLastUpdated = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pages_Slug",
                table: "Pages",
                column: "Slug",
                unique: true);

            // Carries the old Views/Now/Index.cshtml over so /now keeps its content
            // the moment this migration runs. UpdatedAt matches the "Last updated:
            // August 2026" line the static page had.
            migrationBuilder.InsertData(
                table: "Pages",
                columns: new[] { "Slug", "Title", "MetaTitle", "Description", "Tagline", "Content", "Published", "ShowLastUpdated", "CreatedAt", "UpdatedAt" },
                values: new object[]
                {
                    "now",
                    "Now",
                    "This is my Now page | Clint McMahon",
                    null,
                    "Taking pictures, drinking coffee & sunshine.",
                    NowContent,
                    true,
                    true,
                    new DateTime(2026, 8, 15, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(2026, 8, 15, 0, 0, 0, DateTimeKind.Utc)
                });
        }

        private const string NowContent = """
This is my [Now page](https://nownownow.com/about), inspired by [Derek Sivers](https://sive.rs/now).

## Work

- Currently embedded with a client team building Angular and .NET tools that pull data together from a lot of disconnected systems and make it legible to the people who have to act on it. That's most of what I do lately, in one form or another. We've been using AI-assisted development with Claude Code to work at the architecture level by designing systems and data flows rather than hand-writing every line. It's new and has a strange feeling at first, but I'm starting to love how this new type of development lets me move faster and produce more features.
- Over the past six months I've really shifted away from writing 100% of the code by hand. So much of the code that is produced is produced by AI. I'm prompting and telling the system the design, requirements and rules - but it's wild to see how powerful these AI systems actually are. I've got some conflicted feelings about AI coding right now. Its powerful but is it going to take our jobs? So much of software development is problem solving, working through requirements and understanding what brings value to the users. I don't think AI can do those things. I much prefer to be an architect and AI coding systems are turning coders into architects now.

## Side Projects

- I've published a bunch of new updates to a few different apps this summer. [Today's Record High](https://todaysrecordhigh.com) has a fully updated look and feel. There is a brand new dashboard that shows off the major cities across the country and what their record highs are. I also add a little blurb to indicate if that city is getting close to a record high that day. Along with the new website updates, I've updated the mobile app for iOS and Android to match the new information and updates available on the website.
- Every month the MN Coffee Map gets a data update with the latest data from Google Maps. I haven't done very much app work on it for a few months. I'm going to leave it for now as it does exaclty what I want it to do: show me good coffee that's nearby.
- My photoblog is in full swing. It went from markdown file static site to a full DB backed CMS system now. I can post images to it just as easliy as I can to social media. I'm thinking that eventually I will just post to the photoblog and Mastodon in the future. It feels less addicting than Instagram and I can tailor my photos however I want and not have to worry about the algorythm.

## Life

- We just got back from a nice vacation to the San Jaun Islands off the coast of Washington. [There are](https://photos.clintmcmahon.com/2026-07-28) a few [different](https://photos.clintmcmahon.com/2026-07-22) sets of [photos](https://photos.clintmcmahon.com/2026-07-17) on my photoblog from the trip.
- Been carrying around my Ricoh GRIIIx everywhere I go. I love this camera so much. It takes such great photos but it's so simple to carry around with me. I beat it up pretty bad, so hopefully it lasts me a while because I don't want to shoot with anything else.
- Out in Seattle I tried [Bombona Espresso from Coffee Collective](https://coffeecollective.dk/products/bombona-espresso-250-g) and it's the best espresso I've ever had.
- I'd like to blog and write more. I've been reading a lot of [Simon Willison](https://simonwillison.net/) and [Robert Birming](https://robertbirming.com/) - both are excellent bloggers and have inspired me to write more. I still have that writers block feeling of not knowing what I'm suppose to blog about. I think training my brain to be a better writer is going to make me a better technologist in the future. As AI spreads and coding as I once knew it goes away, it's going to be so important to be able to write clearly.
""";

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pages");
        }
    }
}
