using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations.Blog
{
    /// <inheritdoc />
    public partial class StaticPagesToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeaderImageUrl",
                table: "Pages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    IconClass = table.Column<string>(type: "TEXT", nullable: true),
                    Tech = table.Column<string>(type: "TEXT", nullable: true),
                    Section = table.Column<string>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            // Content carried over from the old About, Feeds, Contact and Privacy views
            // and the Projects view's hardcoded cards.
            migrationBuilder.InsertData(
                table: "Pages",
                columns: new[] { "Slug", "Title", "MetaTitle", "Description", "Tagline", "HeaderImageUrl", "Content", "Published", "ShowLastUpdated", "CreatedAt", "UpdatedAt" },
                values: new object[] { "about", "About", "About | Clint McMahon", "I'm Clint McMahon, a software engineer in Minneapolis who writes about code, coffee, side projects, and whatever else is on my mind.", null, "/images/icon/me.jpg", AboutContent, true, false, new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Pages",
                columns: new[] { "Slug", "Title", "MetaTitle", "Description", "Tagline", "HeaderImageUrl", "Content", "Published", "ShowLastUpdated", "CreatedAt", "UpdatedAt" },
                values: new object[] { "feeds", "My Blogroll Feeds", "Feeds | Clint McMahon", "The blogs I follow via RSS.", null, null, FeedsContent, true, false, new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Pages",
                columns: new[] { "Slug", "Title", "MetaTitle", "Description", "Tagline", "HeaderImageUrl", "Content", "Published", "ShowLastUpdated", "CreatedAt", "UpdatedAt" },
                values: new object[] { "contact", "Let's Talk", "Contact a Freelance Developer in Minneapolis | Clint McMahon", "Get in touch about a stalled project, a new build, or a technology initiative that needs rescuing. Freelance developer based in Minneapolis, MN. I read and respond to every message myself.", null, null, ContactContent, true, false, new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Pages",
                columns: new[] { "Slug", "Title", "MetaTitle", "Description", "Tagline", "HeaderImageUrl", "Content", "Published", "ShowLastUpdated", "CreatedAt", "UpdatedAt" },
                values: new object[] { "privacy", "Privacy Policy", "Privacy Policy | Clint McMahon", null, null, null, PrivacyContent, true, true, new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Name", "Url", "Description", "ImageUrl", "IconClass", "Tech", "Section", "SortOrder" },
                values: new object[] { "clintmcmahon.com", "https://clintmcmahon.com", "This site. A database-backed blog and portfolio with an admin interface for drafting and publishing posts, IndieWeb support, and a live weather widget for the Twin Cities.", null, "bi-globe2", "C#, .NET, SQLite", "Live", 10 });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Name", "Url", "Description", "ImageUrl", "IconClass", "Tech", "Section", "SortOrder" },
                values: new object[] { "Photoblog", "https://photos.clintmcmahon.com", "A photoblog running as its own ASP.NET Core app on a subdomain, with Mastodon integration that auto-posts new photos and pulls engagement metrics back in.", null, "bi-camera", "C#, .NET, Mastodon API", "Live", 20 });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Name", "Url", "Description", "ImageUrl", "IconClass", "Tech", "Section", "SortOrder" },
                values: new object[] { "The Undercurrent.FM", "https://theundercurrent.fm", "A searchable archive of every song played on 89.3 The Current since 2005 — over 2 million plays. Trending charts, year-by-year breakdowns, and full-text search across the station's history.", "/images/2026/the_undercurrent.png", null, null, "Live", 30 });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Name", "Url", "Description", "ImageUrl", "IconClass", "Tech", "Section", "SortOrder" },
                values: new object[] { "MN Coffee Map", "https://mncoffeemap.com", "A React Native app for discovering specialty coffee shops across Minnesota — independent roasters only, chains filtered out. Live Google Places data, AI-generated summaries, and Work From MSP scores for Wi-Fi, outlets, and noise.", "/images/2025/mpls_coffee.png", null, "React Native, Expo, Google Places", "Live", 40 });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Name", "Url", "Description", "ImageUrl", "IconClass", "Tech", "Section", "SortOrder" },
                values: new object[] { "Today's Record High", "https://todaysrecordhigh.com", "A data visualization tool comparing today's temperatures against historical record highs across U.S. cities.", "/images/2025/weather_web.png", null, ".NET Core, HighCharts", "Live", 50 });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Name", "Url", "Description", "ImageUrl", "IconClass", "Tech", "Section", "SortOrder" },
                values: new object[] { "KEXP Browser Extension", "https://clintmcmahon.github.io/kexp-now-playing-extension/", "A browser extension that shows the track, artist, and album currently playing on 90.3 KEXP.", "/images/2025/kexp.png", null, "JavaScript", "Live", 60 });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Name", "Url", "Description", "ImageUrl", "IconClass", "Tech", "Section", "SortOrder" },
                values: new object[] { "NYC Coffee Map", "https://nyccoffeemap.com", "The NYC edition of the coffee map series, with a subway overlay showing live transit lines so you can find a great cup on your route.", "/images/2026/nyc_coffee_icon.png", null, "React Native, Expo", "On Hold", 70 });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Name", "Url", "Description", "ImageUrl", "IconClass", "Tech", "Section", "SortOrder" },
                values: new object[] { "Chicago Coffee Map", "https://chicagocoffee.io", "The Chicago edition of the coffee map series — same codebase, same filter for chains, a new city of independent roasters to explore.", "/images/2026/chicago_coffee_icon.png", null, "React Native, Expo", "On Hold", 80 });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Name", "Url", "Description", "ImageUrl", "IconClass", "Tech", "Section", "SortOrder" },
                values: new object[] { "Attribute Editor", "https://github.com/clintmcmahon/azure-ad-b2c-user-manager", "A UI for managing Azure AD B2C custom user attributes without hand-rolling Graph API calls.", "/images/2025/attribute_editor.png", null, "Azure AD B2C, Graph API", "On Hold", 90 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Pages WHERE Slug IN ('about', 'feeds', 'contact', 'privacy');");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropColumn(
                name: "HeaderImageUrl",
                table: "Pages");
        }
        private const string AboutContent = """
Hello, my name is Clint. I'm a software engineer, photographer and dad in Minneapolis, Minnesota. I run [Parka Software](https://parkasoftware.com), a boutique software consulting company.

This is the place where I write about technology, coffee and a handful of side projects that I've got going on.

Aside from my day job, I brew Chemex pour overs and take pictures of the city with my Ricoh GRIIIx, which end up at [photos.clintmcmahon.com](https://photos.clintmcmahon.com). I'm still trying to figure out a perfect espresso.

Drop me a line at [clintmcmahon@pm.me](mailto:clintmcmahon@pm.me) if you want to talk about coffee, photography or just to say hello. I love it when people email me.

## Elsewhere

<ul class="elsewhere">
  <li>
    <a href="https://instagram.com/cwmcmhn" rel="me">
      <i class="bi bi-instagram" aria-hidden="true"></i>
      <span><strong>Instagram</strong><small>@cwmcmhn</small></span>
      <span class="elsewhere-arrow" aria-hidden="true">&nearr;</span>
    </a>
  </li>
  <li>
    <a href="https://linkedin.com/in/clintmcmahon" rel="me">
      <i class="bi bi-linkedin" aria-hidden="true"></i>
      <span><strong>LinkedIn</strong><small>in/clintmcmahon</small></span>
      <span class="elsewhere-arrow" aria-hidden="true">&nearr;</span>
    </a>
  </li>
  <li>
    <a href="https://github.com/clintmcmahon" rel="me">
      <i class="bi bi-github" aria-hidden="true"></i>
      <span><strong>GitHub</strong><small>clintmcmahon</small></span>
      <span class="elsewhere-arrow" aria-hidden="true">&nearr;</span>
    </a>
  </li>
  <li>
    <a href="https://indieweb.social/@clint" rel="me">
      <i class="bi bi-mastodon" aria-hidden="true"></i>
      <span><strong>Mastodon</strong><small>@clint@indieweb.social</small></span>
      <span class="elsewhere-arrow" aria-hidden="true">&nearr;</span>
    </a>
  </li>
</ul>
""";

        private const string ContactContent = """
Drop me a line if you have something to say, comment or just want to see what's going on. Happy to just say hi, or take a good coffee recommendation.

<script data-letterbirduser="clintmcmahon" src="https://letterbird.co/embed/v1.js" data-width="1000px"></script>

## Direct

Email: [clintmcmahon@pm.me](mailto:clintmcmahon@pm.me?subject=Hello)<br>
Location: Minneapolis, MN

## Elsewhere

- [LinkedIn](https://www.linkedin.com/in/clintmcmahon/)
- [GitHub](https://github.com/clintmcmahon)
- [Instagram](https://www.instagram.com/cwmcmhn/)
- [Reddit](https://www.reddit.com/user/mplsdev)
""";

        private const string PrivacyContent = """
This Privacy Policy describes how your personal information is collected, used, and shared when you visit clintmcmahon.com (the "Site").

## Information I Collect

### Contact Form

When you submit the contact form on this Site, I collect the information you provide, including your name, email address, and message content. This information is used solely to respond to your inquiry.

### Newsletter

If you subscribe to my email newsletter, I collect your email address. Your email is stored locally and is used only to send you updates, insights, and occasional information about my services. You can unsubscribe at any time using the link at the bottom of any email.

## Analytics

This Site uses [Plausible Analytics](https://plausible.io), a privacy-focused analytics tool. Plausible does not use cookies, does not collect personal data, and does not track you across websites. All data collected is aggregated and anonymous. You can read more about [Plausible's data practices](https://plausible.io/data-policy).

## Cookies

This Site does not use cookies.

## How I Use Your Information

I use the information I collect to:

- Respond to your inquiries submitted through the contact form
- Send you my email newsletter (if you've subscribed)
- Understand general, anonymous trends in how visitors use this Site

I do not sell, trade, or share your personal information with third parties for marketing purposes.

## Data Retention

- **Contact form submissions:** I retain your information for as long as necessary to respond to your inquiry and maintain records of our communication.
- **Newsletter subscribers:** Your email address is retained until you unsubscribe.

## Your Rights

You have the right to:

- Request access to the personal information I hold about you
- Request correction of any inaccurate information
- Request deletion of your information
- Unsubscribe from my newsletter at any time

To exercise any of these rights, please contact me using the information below.

## Third-Party Services

Your information may be processed by the following third-party services:

- **My newsletter** – for email newsletter delivery
- **Plausible Analytics** – for anonymous website analytics

Each of these services has their own privacy policy governing how they handle data.

## Changes to This Policy

I may update this Privacy Policy from time to time to reflect changes in my practices or for other operational, legal, or regulatory reasons. The updated policy will be posted on this page with a revised "Last Updated" date.

## Contact Me

If you have questions about this Privacy Policy or want to exercise your data rights, you can contact me at:

**Clint McMahon**<br>
[clintmcmahon AT pm dot me](mailto:clintmcmahon@pm.me)<br>
Minneapolis, Minnesota

This site is operated from Minnesota, United States.
""";

        private const string FeedsContent = """
This page lists the websites I follow via RSS feeds. These are some of my favorite blogs and sources for inspiration, learning, and community.

- [Adactio: Journal](https://adactio.com/journal/)<br><small>RSS: <http://adactio.com/journal/rss></small>
- [Caffeinspiration](https://alexanderell.is/)<br><small>RSS: <https://alexanderell.is/index.xml></small>
- [The website of Alice Bartlett](https://alicebartlett.co.uk/)<br><small>RSS: <http://alicebartlett.co.uk/feed.xml></small>
- [Tooth of the Weasel](https://angryweasel.com/blog/)<br><small>RSS: <https://angryweasel.com/blog/feed/></small>
- [Bacardi55's Web Cave](https://bacardi55.io/)<br><small>RSS: <https://bacardi55.io/index.xml></small>
- [Baty.net](https://baty.net/)<br><small>RSS: <http://baty.net/rss.xml></small>
- [Herding Lions](https://benmccormick.org/)<br><small>RSS: <https://benmccormick.org/rss/></small>
- [Robert Birming](https://birming.com/)<br><small>RSS: <https://birming.com/feed/></small>
- [Happiness Machines](https://blog.ignaciobrasca.com/)<br><small>RSS: <https://blog.ignaciobrasca.com/feed.xml></small>
- [Mark's Dev Blog](https://blog.isquaredsoftware.com/index.xml)<br><small>RSS: <https://blog.isquaredsoftware.com/index.xml></small>
- [Jim Nielsen’s Blog](https://blog.jim-nielsen.com/)<br><small>RSS: <https://blog.jim-nielsen.com/feed.xml></small>
- [Numeric Citizen](https://blog.numericcitizen.me/feeds/)<br><small>RSS: <https://blog.numericcitizen.me/feed.xml></small>
- [The Web Witch's Blog](https://blog.stephaniestimac.com/)<br><small>RSS: <https://blog.stephaniestimac.com/feed/feed.xml></small>
- [Brad Frost](https://bradfrost.com/)<br><small>RSS: <http://feeds.feedburner.com/brad-frosts-blog></small>
- [Brain Baking](https://brainbaking.com/)<br><small>RSS: <https://brainbaking.com/index.xml></small>
- [brr](https://brr.fyi/)<br><small>RSS: <https://brr.fyi/feed.xml></small>
- [bt RSS Feed](https://bt.ht/index.rss)<br><small>RSS: <https://bt.ht/index.rss></small>
- [Cassidy Williams](https://cassidoo.co/)<br><small>RSS: <https://blog.cassidoo.co/rss.xml></small>
- [cannonalexander](https://cannonalexander.writeas.com/)<br><small>RSS: <https://cannonalexander.writeas.com/feed/></small>
- [Chris Coyier](https://chriscoyier.net/)<br><small>RSS: <https://chriscoyier.net/feed></small>
- [Chris Hannah](https://chrishannah.me/)<br><small>RSS: <http://blog.chrishannah.me/feed/></small>
- [Ciraltos](https://ciraltos.com/)<br><small>RSS: <https://ciraltos.com/feed/></small>
- [Cole's Blog](https://coleb.blog)<br><small>RSS: <https://coleb.blog/posts_feed></small>
- [Consultant's Mind](https://consultantsmind.com/)<br><small>RSS: <http://consultantsmind.com/feed/></small>
- [Cybersect](https://cybersect.substack.com/)<br><small>RSS: <https://cybersect.substack.com/feed></small>
- [Software Engineering](https://damienbod.com/)<br><small>RSS: <http://damienbod.wordpress.com/feed/></small>
- [daverupert.com](https://daverupert.com/)<br><small>RSS: <http://daverupert.com/atom.xml></small>
- [&#64;desunit (Sergey Bogdanov)](https://desunit.com/blog/feed/)<br><small>RSS: <https://desunit.com/blog/feed/></small>
- [The Digital Nomad](https://digitalnomadder.micro.blog/)<br><small>RSS: <https://digitalnomadder.micro.blog/feed.xml></small>
- [dotdev](https://dotdev.co)<br><small>RSS: <http://feed.dotdev.co/></small>
- [ᓚᘏᗢdostoynikov](https://dostoynikov.bearblog.dev/)<br><small>RSS: <https://dostoynikov.bearblog.dev/feed/?type=rss></small>
- [Email Is Good](https://email-is-good.com/)<br><small>RSS: <https://email-is-good.com/feed></small>
- [Pablo Enoc](https://enocc.com/)<br><small>RSS: <https://enocc.com/feed.xml></small>
- [Eric Bailey](https://ericwbailey.website/)<br><small>RSS: <https://ericwbailey.website/feed/feed.xml></small>
- [Escaping Flatland](https://escapingflatland.substack.com/)<br><small>RSS: <https://escapingflatland.substack.com/feed></small>
- [Fabian's public notepad](https://fabianholzer.online/)<br><small>RSS: <https://fabianholzer.online/feed.xml></small>
- [dotdev](https://feed.dotdev.co/)<br><small>RSS: <http://feed.dotdev.co/></small>
- [Barbarian Meets Coding](https://feeds.feedburner.com/BarbarianMeetsCoding)<br><small>RSS: <http://feeds.feedburner.com/BarbarianMeetsCoding></small>
- [David Walsh Blog](https://feeds.feedburner.com/Bludice)<br><small>RSS: <http://feeds.feedburner.com/Bludice></small>
- [Brad Frost](https://feeds.feedburner.com/brad-frosts-blog)<br><small>RSS: <http://feeds.feedburner.com/brad-frosts-blog></small>
- [Phil Gyford’s website: Everything](https://feeds.feedburner.com/PhilGyford-Everything)<br><small>RSS: <http://feeds.feedburner.com/PhilGyford-Everything></small>
- [remy sharp's b:log](https://feeds.feedburner.com/remysharp)<br><small>RSS: <http://feeds.feedburner.com/remysharp></small>
- [Scott Hanselman's Blog](https://feeds.feedburner.com/ScottHanselman)<br><small>RSS: <http://feeds.feedburner.com/ScottHanselman></small>
- [Tynan | Life Outside the Box](https://feeds.feedburner.com/tynan)<br><small>RSS: <http://feeds.feedburner.com/tynan></small>
- [gilest.org](https://gilest.org)<br><small>RSS: <https://gilest.org/feed/index.xml></small>
- [A Very Good Blog by Keenan](https://gkeenan.co/avgb/)<br><small>RSS: <https://gkeenan.co/avgb/feed.xml></small>
- [Gosha Spark](https://goshacmd.com)<br><small>RSS: <http://feeds.feedburner.com/GoshaArinich></small>
- [Writings - Henrique Dias](https://hacdias.com/writings/)<br><small>RSS: <https://hacdias.com/articles/feed.xml></small>
- [Hassan Habib](https://hassanhabib.com/)<br><small>RSS: <https://hassanhabib.com/feed/></small>
- [Herbert Lui](https://herbertlui.net/)<br><small>RSS: <https://herbertlui.net/feed/></small>
- [Herman Martinus](https://herman.bearblog.dev/)<br><small>RSS: <https://herman.bearblog.dev/feed/?type=rss></small>
- [High Scalability](https://highscalability.com/blog/)<br><small>RSS: <http://feeds.feedburner.com/HighScalability></small>
- [Fabian's public notepad](https://holzer.online/)<br><small>RSS: <https://holzer.online/feed.xml></small>
- [Jack Vanlightly](https://jack-vanlightly.com/)<br><small>RSS: <https://jack-vanlightly.com/blog?format=RSS></small>
- [Jason Crawford](https://jasoncrawford.org/)<br><small>RSS: <https://jasoncrawford.org/feed.xml></small>
- [Jan-Lukas Else](https://jlelse.blog)<br><small>RSS: <https://jlelse.blog/index.xml></small>
- [up & to the right](https://jon.bo/)<br><small>RSS: <https://jon.bo/index.xml></small>
- [Jonas Hietala](https://jonashietala.se/)<br><small>RSS: <https://jonashietala.se/feed.xml></small>
- [people, ideas, machines](https://joshs.bearblog.dev/)<br><small>RSS: <https://joshs.bearblog.dev/feed/></small>
- [Developer Musings](https://joshghent.com/)<br><small>RSS: <https://joshghent.com/rss.xml></small>
- [kupajo](https://kupajo.com/)<br><small>RSS: <https://kupajo.com/feed/></small>
- [Ludicity](https://lucidity.mataroa.blog/)<br><small>RSS: <https://lucidity.mataroa.blog/rss/></small>
- [Manuel Moreale - Everything Feed](https://manuelmoreale.com/)<br><small>RSS: <https://manuelmoreale.com/feed/rss></small>
- [Marc Littlemore](https://marclittlemore.com/)<br><small>RSS: <https://marclittlemore.com/feed.xml></small>
- [Mattia Compagnucci's Journal](https://mattiacompagnucci.com/)<br><small>RSS: <https://mattiacompagnucci.com/feed/rss></small>
- [Max Rozen](https://maxrozen.com/)<br><small>RSS: <https://maxrozen.com/rss.xml></small>
- [Mert Bulan](https://mertbulan.com/)<br><small>RSS: <https://mertbulan.com/feed.xml></small>
- [Minutes to Midnight](https://minutestomidnight.co.uk/)<br><small>RSS: <https://minutestomidnight.co.uk/feed.xml></small>
- [Max Böck](https://mxb.at/feed.xml)<br><small>RSS: <https://mxb.at/feed.xml></small>
- [Mzansi Bytes](https://mzansibytes.com/)<br><small>RSS: <https://mzansibytes.com/feed/></small>
- [Naz Hamid](https://nazhamid.com/)<br><small>RSS: <https://nazhamid.com/feed.xml></small>
- [Nela Dunato Art & Design](https://neladunato.com)<br><small>RSS: <https://neladunato.com/feed/></small>
- [niqwithq](https://niqwithq.com)<br><small>RSS: <https://niqwithq.com/feed.xml></small>
- [Notes by JCProbably](https://notes.jeddacp.com/)<br><small>RSS: <https://notes.jeddacp.com/feed/></small>
- [Numeric Citizen](https://numericcitizen.me/)<br><small>RSS: <https://numericcitizen.me/feed.xml></small>
- [Ana Rodrigues](https://ohhelloana.blog/)<br><small>RSS: <https://ohhelloana.blog/feed.xml></small>
- [Own Your Web](https://ownyourweb.site)<br><small>RSS: <https://buttondown.email/ownyourweb/rss></small>
- [Pablo Enoc](https://pabloenoc.com/)<br><small>RSS: <https://pabloenoc.com/feed.xml></small>
- [Blog - petitvisual](https://petitvisual.jp/blog/)<br><small>RSS: <https://petitvisual.jp/blog?format=rss></small>
- [Phil Gyford’s website: Everything](https://philgyford.com/)<br><small>RSS: <http://feeds.feedburner.com/PhilGyford-Everything></small>
- [Slice of pi](https://pimoore.ca/)<br><small>RSS: <https://pimoore.ca/feed.xml></small>
- [Piper Haywood](https://piperhaywood.com/)<br><small>RSS: <https://piperhaywood.com/feed/></small>
- [podviaznikov.com](https://podviaznikov.com/)<br><small>RSS: <https://podviaznikov.com/feed.xml></small>
- [Popagandhi](https://popagandhi.com/)<br><small>RSS: <http://popagandhi.com/feed/></small>
- [Prashant Sengar](https://prashants.in/)<br><small>RSS: <https://prashants.in/feed/></small>
- [Rach Smith's digital garden](https://rachsmith.com/)<br><small>RSS: <https://rachsmith.com/feed.xml></small>
- [Robb Knight • Posts • RSS Feed](https://rknight.me/)<br><small>RSS: <https://rknight.me/subscribe/posts/rss.xml></small>
- [remy sharp's b:log](https://remysharp.com/)<br><small>RSS: <http://feeds.feedburner.com/remysharp></small>
- [Robert Birming](https://robert.bearblog.dev)<br><small>RSS: <https://robert.bearblog.dev/feed/?type=rss></small>
- [Robin Rendle](https://robinrendle.com/)<br><small>RSS: <https://robinrendle.com/feed.xml></small>
- [Rob Hope &#64;robhope](https://robhope.com/)<br><small>RSS: <https://robhope.com/feed/></small>
- [Rubenerd](https://rubenerd.com/)<br><small>RSS: <http://rubenerd.com/feed/></small>
- [Sara Joy](https://sarajoy.dev/)<br><small>RSS: <https://sarajoy.dev/rss.xml></small>
- [Scott Smitelli](https://scottsmitelli.com/)<br><small>RSS: <https://scottsmitelli.com/index.xml></small>
- [Andrew's Blog](https://sheep.horse/)<br><small>RSS: <https://sheep.horse/rss.xml></small>
- [Shashank's Thoughts](https://shashanksthoughts.blogspot.com/)<br><small>RSS: <https://shashanksthoughts.blogspot.com/feeds/posts/default></small>
- [shilin typing...](https://shilin.ca/)<br><small>RSS: <https://shilin.ca/feed/></small>
- [Simon Safar](https://simonsafar.com)<br><small>RSS: <https://simonsafar.com/index.xml></small>
- [Simple Programmer](https://simpleprogrammer.com/)<br><small>RSS: <http://simpleprogrammer.com/feed/></small>
- [Slice of pi](https://sliceofpi.com/)<br><small>RSS: <https://sliceofpi.com/feed.xml></small>
- [small cypress](https://smallcypress.bearblog.dev/)<br><small>RSS: <https://smallcypress.bearblog.dev/feed/?type=rss></small>
- [Steven Giesel](https://steven-giesel.com/)<br><small>RSS: <https://steven-giesel.com/feed.rss></small>
- [Terrible Software](https://terriblesoftware.org/)<br><small>RSS: <https://terriblesoftware.org/feed/></small>
- [The Jolly Teapot](https://thejollyteapot.com/)<br><small>RSS: <https://thejollyteapot.com/feed.rss></small>
- [Vladimir Klepov as a Coder](https://thoughtspile.github.io/)<br><small>RSS: <https://thoughtspile.github.io/atom.xml></small>
- [Tim Hårek](https://timharek.no/)<br><small>RSS: <https://timharek.no/feed.xml></small>
- [Tomáš Kohl](https://tomaskohl.com/)<br><small>RSS: <https://tomaskohl.com/code/atom.xml></small>
- [tomcritchlow.com](https://tomcritchlow.com/)<br><small>RSS: <https://tomcritchlow.com/feed.xml></small>
- [Tangible Life](https://tangiblelife.net)<br><small>RSS: <https://tangiblelife.net/feed.rss></small>
- [Tyler Cipriani: blog](https://tylercipriani.com/blog/)<br><small>RSS: <https://tylercipriani.com/blog/index.rss></small>
- [up & to the right](https://upandright.com/)<br><small>RSS: <https://upandright.com/index.xml></small>
- [Useful Fictions](https://usefulfictions.substack.com/)<br><small>RSS: <https://usefulfictions.substack.com/feed></small>
- [Vadim Kravcenko](https://vadimkravcenko.com/)<br><small>RSS: <https://vadimkravcenko.com/feed/></small>
- [Vladimir Klepov as a Coder](https://vladimirklepov.com/)<br><small>RSS: <https://vladimirklepov.com/atom.xml></small>
- [Wandering Aimfully](https://wanderingaimfully.com/articles/)<br><small>RSS: <https://wanderingaimfully.com/articles/feed/></small>
- [Westenberg.](https://westenberg.com/)<br><small>RSS: <https://westenberg.com/rss/></small>
- [Winnie Lim](https://winnielim.org/)<br><small>RSS: <http://journal.winnielim.org/rss.xml></small>
- [Writings - Henrique Dias](https://writings.henriquedias.com/)<br><small>RSS: <https://writings.henriquedias.com/feed.xml></small>
- [zerokspot.com](https://zerokspot.com/)<br><small>RSS: <http://zerokspot.com/weblog/feed/></small>
""";

    }
}
