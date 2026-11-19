using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations.Blog
{
    /// <inheritdoc />
    public partial class AddBlogroll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogrollFeeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    SiteUrl = table.Column<string>(type: "TEXT", nullable: true),
                    FeedUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ImportedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogrollFeeds", x => x.Id);
                });

            // The old blogroll was a hand-written markdown list in the /feeds page's
            // Content. The list now comes from this table, so keep only the intro text
            // above the first "- [" list item.
            migrationBuilder.Sql("""
                UPDATE Pages
                SET Content = rtrim(substr(Content, 1, instr(Content, char(10) || '- [') - 1), char(10) || char(13) || ' ')
                WHERE Slug = 'feeds' AND instr(Content, char(10) || '- [') > 0;
                """);

            // Initial blogroll: the Blogs and Photoblogs folders from NetNewsWire on
            // 2026-11-19. Later changes come from OPML uploads at /admin/blogroll.
            migrationBuilder.Sql("""
                INSERT INTO BlogrollFeeds (Category, Title, SiteUrl, FeedUrl, ImportedAt) VALUES
                ('Blogs', 'Adam Wood', 'https://adamwood.blog/', 'https://adamwood.blog/posts_feed', '2026-11-19 02:26:00'),
                ('Blogs', 'After Babel', 'https://www.afterbabel.com/', 'https://jonathanhaidt.substack.com/feed', '2026-11-19 02:26:00'),
                ('Blogs', 'Allen Pike', 'https://www.allenpike.com/', 'https://allenpike.com/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Ana Rodrigues', 'https://ohhelloana.blog/', 'https://ohhelloana.blog/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Andrew''s Blog', 'https://sheep.horse/', 'https://sheep.horse/rss.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Andy Bell', 'https://bell.bz/', 'https://andy-bell.co.uk/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'bacon roll', 'https://roll.bybacon.com/', 'https://roll.bybacon.com/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Brad Frost', 'https://bradfrost.com/', 'http://feeds.feedburner.com/brad-frosts-blog', '2026-11-19 02:26:00'),
                ('Blogs', 'Bradley Taunt', 'https://btxx.org/', 'https://btxx.org/index.rss', '2026-11-19 02:26:00'),
                ('Blogs', 'brennan.day', 'https://brennan.day/', 'https://brennan.day/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Building My Way Out', 'https://rcrdo.com/', 'https://rcrdo.com/rss/', '2026-11-19 02:26:00'),
                ('Blogs', 'Cassidy Williams', 'https://cassidoo.co/', 'https://blog.cassidoo.co/rss.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Chris Coyier', 'https://chriscoyier.net/', 'https://chriscoyier.net/feed', '2026-11-19 02:26:00'),
                ('Blogs', 'Ciraltos', 'https://www.ciraltos.com/', 'https://www.ciraltos.com/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Cliff538', 'https://cliff538.com/', 'https://cliff538.com/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Colin Walker', 'https://colinwalker.blog/', 'https://colinwalker.blog/dailyfeed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Daniel Bourke', 'https://www.mrdbourke.com/', 'https://www.mrdbourke.com/rss/', '2026-11-19 02:26:00'),
                ('Blogs', 'daverupert.com', 'https://daverupert.com/', 'http://daverupert.com/atom.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Developer Musings', 'https://joshghent.com/', 'https://joshghent.com/rss.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'dostoynikov', 'https://dostoynikov.com/', 'https://dostoynikov.bearblog.dev/feed/?type=rss', '2026-11-19 02:26:00'),
                ('Blogs', 'dylanbeattie.net', 'https://dylanbeattie.net/', 'https://dylanbeattie.net/rss.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Eddie Dale', 'https://www.eddiedale.com/', 'https://www.eddiedale.com/blog-feed', '2026-11-19 02:26:00'),
                ('Blogs', 'Email Is Good', 'https://email-is-good.com/', 'https://email-is-good.com/feed', '2026-11-19 02:26:00'),
                ('Blogs', 'Eric Bailey', 'https://ericwbailey.website/', 'https://ericwbailey.website/feed/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Fabian''s public notepad', 'https://holzer.online/', 'https://holzer.online/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Field Notes', 'https://www.wmprkr.xyz/', 'https://www.wmprkr.xyz/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Field notes for life – kupajo', 'https://kupajo.com/', 'https://kupajo.com/category/field-notes-for-life/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Founding Mode', 'https://aimode.substack.com/', 'https://aimode.substack.com/feed', '2026-11-19 02:26:00'),
                ('Blogs', 'Francisco Trindade', 'https://franciscotrindade.me/', 'https://franciscotrindade.me/rss.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'garrit.xyz', 'https://garrit.xyz/', 'https://garrit.xyz/rss.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'gilest.org', 'https://gilest.org/', 'https://gilest.org/feed/index.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Go With The (RSS) Flow', 'https://contino.com/', 'https://gwtf.it/feed', '2026-11-19 02:26:00'),
                ('Blogs', 'Happiness Machines', 'https://blog.ignaciobrasca.com/', 'https://blog.ignaciobrasca.com/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Herbert Lui', 'https://herbertlui.net/', 'https://herbertlui.net/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Herman Martinus', 'https://herman.bearblog.dev/', 'https://herman.bearblog.dev/feed/?type=rss', '2026-11-19 02:26:00'),
                ('Blogs', 'Inverted Passion', 'https://invertedpassion.com/', 'https://invertedpassion.com/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Jack Franklin', 'http://www.jackfranklin.co.uk/', 'https://www.jackfranklin.co.uk/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Jack Vanlightly', 'https://jack-vanlightly.com/', 'https://jack-vanlightly.com/blog?format=RSS', '2026-11-19 02:26:00'),
                ('Blogs', 'James Baker', 'https://jamesbaker.uk/', 'https://jamesbaker.uk/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Jamie Lord', 'https://lord.technology/', 'https://lord.technology/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Jan-Lukas Else', 'https://janlukas.blog/', 'https://jlelse.blog/index.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Jeff Kaufman''s Writing', 'https://www.jefftk.com/p', 'https://www.jefftk.com/news.rss', '2026-11-19 02:26:00'),
                ('Blogs', 'Jim Nielsen’s Blog', 'https://blog.jim-nielsen.com/', 'https://blog.jim-nielsen.com/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Jonas Hietala', 'https://www.jonashietala.se/', 'https://www.jonashietala.se/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Josh Beckman’s Organization', 'https://www.joshbeckman.org/', 'https://www.joshbeckman.org/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Josh Swords', 'https://joshs.bearblog.dev/', 'https://joshs.bearblog.dev/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Keenan', 'https://gkeenan.co/avgb', 'https://gkeenan.co/avgb/?format=rss', '2026-11-19 02:26:00'),
                ('Blogs', 'kupajo', 'https://kupajo.com/', 'https://kupajo.com/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Kyle Shevlin''s Blog RSS Feed', 'https://kyleshevlin.com/', 'https://kyleshevlin.com/rss.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'laze.net', 'https://laze.net/', 'https://laze.net/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Living Out Loud', 'https://louplummer.lol/', 'https://louplummer.lol/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Liza Shulyayeva', 'https://liza.io/', 'http://liza.io/index.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Ludicity', 'https://ludic.mataroa.blog/', 'https://ludic.mataroa.blog/rss/', '2026-11-19 02:26:00'),
                ('Blogs', 'Luke Harris', 'https://www.lkhrs.com/blog/', 'https://www.lkhrs.com/blog/index.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Lyteforce Writes', 'https://lyteforcewrites.fyi/', 'https://lyteforcewrites.fyi/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Mark''s Dev Blog', 'https://blog.isquaredsoftware.com/index.xml', 'https://blog.isquaredsoftware.com/index.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Marko Denic', 'https://markodenic.com/', 'https://markodenic.com/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Matt Blodgett', 'http://www.mattblodgett.com/', 'http://feeds.feedburner.com/mattblodgett', '2026-11-19 02:26:00'),
                ('Blogs', 'Mattia Compagnucci''s Journal', 'https://mattiacompagnucci.com/', 'https://mattiacompagnucci.com/feed/rss', '2026-11-19 02:26:00'),
                ('Blogs', 'Max Böck', 'https://mxb.dev/', 'https://mxb.at/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Melanie Richards', 'https://melanie-richards.com/', 'https://melanie-richards.com/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Mert Bulan - Blog & Portfolio', 'https://mertbulan.com/', 'https://mertbulan.com/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'michaelheap.com', 'https://michaelheap.com/', 'https://michaelheap.com/rss', '2026-11-19 02:26:00'),
                ('Blogs', 'mikehindle.uk', 'https://mikehindle.uk/', 'https://mikehindle.uk/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Minutes to Midnight', 'https://minutestomidnight.co.uk/', 'https://minutestomidnight.co.uk/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'moonwriting', 'https://moonwriting.blog/', 'https://moonwriting.blog/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Nate.spot', 'https://nate.spot/', 'https://nate.spot/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Naz Hamid • Journal', 'https://nazhamid.com/', 'https://nazhamid.com/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Nela Dunato Art & Design', 'https://neladunato.com/', 'https://neladunato.com/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'nik.art', 'https://nik.art/', 'http://niklasgoeke.com/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'niqwithq', 'https://niqwithq.com/', 'https://niqwithq.com/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Notes by JCProbably', 'https://notes.jeddacp.com/', 'https://notes.jeddacp.com/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'ntietz.com blog - technically a blog', 'https://ntietz.com/blog/', 'https://ntietz.com/atom.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Own Your Web', 'https://newsletter.ownyourweb.site/', 'https://buttondown.email/ownyourweb/rss', '2026-11-19 02:26:00'),
                ('Blogs', 'Pablo Enoc', 'https://enocc.com/', 'https://enocc.com/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Philip Ilic Thomas', 'https://www.philipithomas.com/', 'https://www.philipithomas.com/posts.rss', '2026-11-19 02:26:00'),
                ('Blogs', 'Piper Haywood', 'https://piperhaywood.com/', 'https://piperhaywood.com/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'remy sharp''s b:log', 'https://remysharp.com/', 'http://feeds.feedburner.com/remysharp', '2026-11-19 02:26:00'),
                ('Blogs', 'Rhoneisms', 'https://www.patrickrhone.net/', 'http://therandompost.com/rss', '2026-11-19 02:26:00'),
                ('Blogs', 'Robb Knight • Posts • RSS Feed', 'https://rknight.me/', 'https://rknight.me/subscribe/posts/rss.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Robert Birming', 'https://robertbirming.com/', 'https://robert.bearblog.dev/feed/?type=rss', '2026-11-19 02:26:00'),
                ('Blogs', 'Robin Rendle', 'https://robinrendle.com/', 'https://robinrendle.com/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Sal''s Blog', 'https://sals.place/', 'https://sals.place/index.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Sara Joy', 'https://sarajoy.dev/', 'https://sarajoy.dev/rss.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Sasha''s ''Newsletter''', 'https://sashachapin.substack.com/', 'https://sashachapin.substack.com/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Scott Smitelli', 'https://www.scottsmitelli.com/', 'https://www.scottsmitelli.com/index.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'seangoedecke.com RSS feed', 'https://seangoedecke.com/', 'https://www.seangoedecke.com/rss.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Shashank''s Thoughts', 'https://shashanksthoughts.blogspot.com/', 'https://shashanksthoughts.blogspot.com/feeds/posts/default', '2026-11-19 02:26:00'),
                ('Blogs', 'Shilin typing', 'https://shilin.ca/', 'https://shilin.ca/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Simon Safar', 'https://simonsafar.com/', 'https://simonsafar.com/index.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Simple Programmer', 'https://simpleprogrammer.com/', 'http://simpleprogrammer.com/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Slice of pi', 'https://pimoore.ca/', 'https://pimoore.ca/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'small cypress', 'https://smallcypress.bearblog.dev/', 'https://smallcypress.bearblog.dev/feed/?type=rss', '2026-11-19 02:26:00'),
                ('Blogs', 'Software and Tech stories from an Insider - iDiallo.com', 'https://idiallo.com/', 'http://idiallo.com/feed.rss', '2026-11-19 02:26:00'),
                ('Blogs', 'Software Engineering', 'https://damienbod.com/', 'http://damienbod.wordpress.com/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Steven Giesel', 'https://steven-giesel.com/', 'https://steven-giesel.com/feed.rss', '2026-11-19 02:26:00'),
                ('Blogs', 'Strategy, Architecture, Continuous Delivery, and DDD', 'https://medium.com/nick-tune-tech-strategy-blog?source=rss----522d82c208e7---4', 'https://medium.com/feed/nick-tune-tech-strategy-blog', '2026-11-19 02:26:00'),
                ('Blogs', 'Tangible Life', 'https://tangiblelife.net/', 'https://tangiblelife.net/feed.rss', '2026-11-19 02:26:00'),
                ('Blogs', 'Terrible Software', 'https://terriblesoftware.org/', 'https://terriblesoftware.org/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'The Courage to Live It', 'https://shawnsmucker.substack.com/', 'https://shawnsmucker.substack.com/feed', '2026-11-19 02:26:00'),
                ('Blogs', 'The Digital Nomad', 'https://digitalnomadder.micro.blog/', 'https://digitalnomadder.micro.blog/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'The Jolly Teapot', 'https://thejollyteapot.com/', 'https://thejollyteapot.com/feed.rss', '2026-11-19 02:26:00'),
                ('Blogs', 'The Web Witch''s Blog', 'https://blog.stephaniestimac.com/', 'https://blog.stephaniestimac.com/feed/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'The website of Alice Bartlett', 'https://alicebartlett.co.uk/', 'http://alicebartlett.co.uk/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Thomas Rigby', 'https://thomasrigby.com/', 'https://thomasrigby.com/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Tim Hårek', 'https://timharek.no/', 'https://timharek.no/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Tomáš Kohl', 'https://tomaskohl.com/', 'https://tomaskohl.com/code/atom.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'tomcritchlow.com', 'https://tomcritchlow.com/', 'https://tomcritchlow.com/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Tore Nestenius', 'https://nestenius.se/', 'https://nestenius.se/feed/', '2026-11-19 02:26:00'),
                ('Blogs', 'Tyler Cipriani: blog', 'https://tylercipriani.com/blog/', 'https://tylercipriani.com/blog/index.rss', '2026-11-19 02:26:00'),
                ('Blogs', 'Tynan | Life Outside the Box', 'https://tynan.com/', 'http://feeds.feedburner.com/tynan', '2026-11-19 02:26:00'),
                ('Blogs', 'up & to the right', 'https://jon.bo/', 'https://jon.bo/index.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Useful Fictions', 'https://usefulfictions.substack.com/', 'https://usefulfictions.substack.com/feed', '2026-11-19 02:26:00'),
                ('Blogs', 'Victoria Lo - Tech Blog', 'https://lo-victoria.com/', 'https://lo-victoria.com/rss.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Vladimir Klepov as a Coder', 'https://thoughtspile.github.io/', 'https://thoughtspile.github.io/atom.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Weblog on marginalia.nu', 'https://www.marginalia.nu/log/', 'https://www.marginalia.nu/log/index.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Winnie Lim', 'https://winnielim.org/', 'http://journal.winnielim.org/rss.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'Writings - Henrique Dias', 'https://hacdias.com/writings/', 'https://hacdias.com/articles/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'www.marginalia.nu on marginalia.nu', 'https://www.marginalia.nu/', 'https://memex.marginalia.nu/log/feed.xml', '2026-11-19 02:26:00'),
                ('Blogs', 'yours, tiramisu', 'https://tiramisu.bearblog.dev/', 'https://tiramisu.bearblog.dev/feed/?type=rss', '2026-11-19 02:26:00'),
                ('Blogs', 'Z1NZ0L1N', 'https://z1nz0l1n.com/', 'https://z1nz0l1n.com/rss/', '2026-11-19 02:26:00'),
                ('Blogs', 'zerokspot.com', 'https://zerokspot.comhttps://zerokspot.com/', 'http://zerokspot.com/weblog/feed/', '2026-11-19 02:26:00'),
                ('Photoblogs', '75 Central Photography', 'https://www.75centralphotography.com/', 'https://www.75centralphotography.com/feed/', '2026-11-19 02:26:00'),
                ('Photoblogs', '365Pixels', 'https://365pixels.xyz/', 'https://365pixels.xyz/feed.json', '2026-11-19 02:26:00'),
                ('Photoblogs', 'aows', 'https://aows.co/blog/', 'https://aows.co/blog?format=RSS', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Bits by bino posts tagged photo', 'https://bits.gobino.be/tag/photo', 'https://bits.gobino.be/tag/photo/feed', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Blog - Roman Fox', 'https://www.snapsbyfox.com/blog/', 'https://www.snapsbyfox.com/blog?format=rss', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Charity Shot Blog', 'https://charityshot.co.uk/', 'https://charityshot.co.uk/rss.xml', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Chris Glass', 'https://chrisglass.com/', 'https://chrisglass.com/photos/feed/', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Clint McMahon — Photos', 'https://skywayhours.com/', 'https://photos.clintmcmahon.com/rss', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Daniel Pietzsch’s Photo Journal', 'https://photojournal.danielpietzsch.com/', 'https://photojournal.danielpietzsch.com/feed.xml', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Dave Lawrence', 'https://davelawrence.photo/', 'https://davelawrence.photo/feed/', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Florian', 'https://florian.photo/', 'https://florian.photo/feed', '2026-11-19 02:26:00'),
                ('Photoblogs', 'FUJI X WEEKLY', 'https://fujixweekly.com/', 'https://fujixweekly.com/blog/feed/', '2026-11-19 02:26:00'),
                ('Photoblogs', 'In Photos', 'https://inphotos.org/', 'http://inphotos.org/feed/', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Jasper Tandy', 'https://jasper.tandy.is/', 'https://jasper.tandy.is/syndicated', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Joe''s nyc', 'https://blog.josephholmes.io/', 'http://joesnyc.streetnine.com/index.xml', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Landscape and Nature Photography Photoblog • Michael Russell Photography', 'https://www.mrussellphotography.com/blog/', 'http://photoblog.mrussellphotography.com/feed/', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Light, Logged!', 'https://lightlogged.blog/', 'https://lightlogged.blog/feed/', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Long Delays Possible', 'https://www.longdelayspossible.com/', 'https://www.longdelayspossible.com/blog/feed/', '2026-11-19 02:26:00'),
                ('Photoblogs', 'Luis Natera', 'https://www.luisnatera.photo/', 'https://www.luisnatera.photo/rss/', '2026-11-19 02:26:00'),
                ('Photoblogs', 'maique', 'https://maique.eu/tag/photography', 'https://maique.eu/tag/photography/feed', '2026-11-19 02:26:00');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogrollFeeds");
        }
    }
}
