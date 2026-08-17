using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations.Blog
{
    /// <summary>
    /// The 2026 coffee journal, transcribed from the paper notebook (26 bags, Dec 30 2025
    /// through Jul 28 2026) plus the notebook's back-page brewing notes.
    ///
    /// Content rather than schema, but it ships as a migration on purpose: blog.db lives on
    /// the server and is excluded from the deploy rsync, so Database.Migrate() at startup is
    /// the only path that puts these on the site without re-typing them into /admin.
    /// Down() removes exactly these slugs, so it is reversible.
    /// </summary>
    public partial class AddCoffeeJournal2026 : Migration
    {
        private static readonly string[] Slugs = new[]
        {
                @"sey-bishan-wate",
                @"verve-juan-benitez",
                @"northern-coffeeworks-ladys-slipper",
                @"dogwood-bear-hug",
                @"verve-street-level",
                @"verve-granitos-de-ortiz",
                @"tandem-west-end-blues-2026-01-29",
                @"heart-gerba-wogo-sodu",
                @"dak-poppy-soda",
                @"verve-sermon",
                @"sey-jhonoton-pinto",
                @"heart-ramon-hernandez",
                @"heart-tagel-alemayehu",
                @"sey-eduardo-quispe",
                @"archers-ethiopian-bito",
                @"northern-coffeeworks-evergreen-espresso",
                @"onyx-geometry",
                @"onyx-monarch",
                @"heart-david-munoz-las-flores",
                @"port-2050-carlos-cadena",
                @"heart-vilma-miranda",
                @"sey-ibonia-estate",
                @"heart-daysi-munoz",
                @"coffee-collective-bombona",
                @"sey-juan-jimenez",
                @"heart-byron-hernandez",
                @"coffee-roastings-notes-august-2026"
        };

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Title", "Description", "Date", "Draft", "Slug", "Tags", "Content", "CreatedAt", "UpdatedAt" },
                values: new object[,]
                {
                { @"Bishan Wate from Sey", @"Asked an AI how to brew Ethiopian beans and finally got one I liked", new DateTime(2025, 12, 30), false, @"sey-bishan-wate", @"coffee,coffee-journal,sey,ethiopia", @"**Sey** · Ethiopian · ★★★★☆

<!-- IMAGE: sey-bishan-wate.png -->

**Grind:** #20-22 on Baratza · **Water:** 210-212°F (just off boil)

I consulted Claude AI on how to brew Ethiopian beans. I've had a lot of bad coffee come from good beans in the past. There are a bunch of new things I learned that have helped me. This was one of the first Ethiopians I really liked.

Things to remember about Africans:

- Higher heat. These do well between 210°-212°F. Just off boil
- Grind around 20-22
- Slow and low pour with little agitation", new DateTime(2025, 12, 30), new DateTime(2025, 12, 30) },
                { @"Juan Benitez from Verve", @"A New Year's Eve bag brewed on AI instructions that told me to go way coarser than I would have", new DateTime(2025, 12, 31), false, @"verve-juan-benitez", @"coffee,coffee-journal,verve,honduras,latin-america", @"**Verve** · Single Origin - Honduras - Washed · ★★★☆☆

<!-- IMAGE: verve-juan-benitez.png -->

**Grind:** #24-26 pour over, #13 espresso · **Water:** 205°F

Pineapple, custard and candied orange.

Had this bag on New Year's Eve. Morning brewed for pour over using ChatGPT instructions. They instructed me to go way coarse, which I thought was crazy. Turns out it brewed nicely. Does AI know how, or the best way, to brew coffee? I took a pic of the bag and asked it how to brew. More data is needed.

Just did milk and espresso drinks and they are good. Only three star good though. Not bad at all. Other beans are better. Dipped a biscotti cookie in my flat white. It's a beautiful sunny winter day out today. Looking forward to 2026. Happy New Year.", new DateTime(2025, 12, 31), new DateTime(2025, 12, 31) },
                { @"Lady's Slipper from Northern Coffeeworks", @"The best roaster still open on a Sunday afternoon, and a barista who actually helped", new DateTime(2026, 1, 11), false, @"northern-coffeeworks-ladys-slipper", @"coffee,coffee-journal,northern-coffeeworks,ethiopia,minneapolis", @"**Northern Coffeeworks** · Ethiopian · ★★★★☆

<!-- IMAGE: northern-coffeeworks-ladys-slipper.png -->

**Grind:** #23 on Encore · **Water:** 207°F · no espresso

Jasmine, peach, cashew brittle, oolong.

First bag of Northern Coffeeworks I've bought in a few years. Plus for being open till four on a Sunday. Was in need of a bag for the week and they were the best roaster still open.

The barista was excellent in helping me find a good bean.

It tasted like a heavy medium roast. Grinded coarse and turned up the temp to better the taste.

I'm feeling much better about brewing Ethiopian beans now. Maybe I will write a how-to post soon.", new DateTime(2026, 1, 11), new DateTime(2026, 1, 11) },
                { @"Bear Hug from Dogwood", @"A short entry on a chocolatey Dogwood espresso, two weeks off roast", new DateTime(2026, 1, 15), false, @"dogwood-bear-hug", @"coffee,coffee-journal,dogwood,minneapolis", @"**Dogwood**

<!-- IMAGE: dogwood-bear-hug.png -->

**Grind:** #13 espresso

Chocolate and caramely. Two weeks off roast for the first pull.

First pull: 13, 25 seconds.

- A little sour, think I can go finer for this.
- Good crema.", new DateTime(2026, 1, 15), new DateTime(2026, 1, 15) },
                { @"Street Level from Verve", @"Chased a too-dark pour from 22 all the way out to 26 and still could not fully shake it", new DateTime(2026, 1, 19), false, @"verve-street-level", @"coffee,coffee-journal,verve", @"**Verve** · Medium Roast · ★★★★☆

<!-- IMAGE: verve-street-level.png -->

**Grind:** #26 pour over · **Water:** 200°F · **Dose:** 18g, 25-27s, 29-30.5g yield

Clementine, red apple, honeycomb. Roasted Jan 8 2026, two weeks out.

First pour was 22 grind that was much too dark. The pour finished at five minutes.

Today we grind at 26 to see how it comes. Temp is 200° and pour time was 4:14. Still tastes a little too dark.

Espresso pull at 16 was 30 secs. Tasted kind of sour. Next pull will be 15 to see how that plays out. Another 16 with 19 grams is 25 secs.

15 pulled well. Still a bit sour. Output 40-45g.

14g to 25 secs to 40g. Good.", new DateTime(2026, 1, 19), new DateTime(2026, 1, 19) },
                { @"Granitos De Ortiz from Verve", @"A light roast that tasted dark no matter what I did to it", new DateTime(2026, 1, 29), false, @"verve-granitos-de-ortiz", @"coffee,coffee-journal,verve,costa-rica,latin-america", @"**Verve** · Tarrazu, Costa Rica - White Honey · ★★☆☆☆

<!-- IMAGE: verve-granitos-de-ortiz.png -->

**Grind:** #18-20 pour over

Nectarine, simple syrup. Roasted Jan 14, two weeks off.

First two grinds were at 20. Pour came through around 4ish. Tasted too dark for this light roast. Couldn't get any flavors from it.

18 grind size this time.

Not liking this coffee very much. It has a dark roast flavor that I can't shake no matter how I brew it. Maybe try this as an espresso?", new DateTime(2026, 1, 29), new DateTime(2026, 1, 29) },
                { @"West End Blues from Tandem Roasters", @"A second bag of West End Blues, and preheating the portafilter changed every number I had", new DateTime(2026, 1, 29), false, @"tandem-west-end-blues-2026-01-29", @"coffee,coffee-journal,tandem,portland", @"**Tandem Roasters**

<!-- IMAGE: tandem-west-end-blues-2026-01-29.png -->

**Grind:** #17-19 espresso

Preheating the portafilter now. This is changing how my shots are pulled.

18g in 38g out for 37 seconds. Grind was 15. Very dry puck. Result was very bitter.

New grind is 16 but the shot pulled at 46 seconds. Came out bitter as expected. The preheated porta is really changing how I pull shots for myself. Next will be 18. Never thought I would go that high.

17 grind pulled 30 seconds at 48g.

19 grind size pulled 36 grams at 25 seconds. Good.", new DateTime(2026, 1, 29), new DateTime(2026, 1, 29) },
                { @"Gerba Wogo Sodu from Heart", @"First bag from Heart, and two brews spent learning this bean wants a much coarser grind", new DateTime(2026, 2, 1), false, @"heart-gerba-wogo-sodu", @"coffee,coffee-journal,heart,ethiopia", @"**Heart** · Ethiopian - Guji - Washed · ★★★★☆

<!-- IMAGE: heart-gerba-wogo-sodu.png -->

**Grind:** #26 pour over · **Water:** 205°F

Strawberry, brown sugar, hibiscus. Roasted 01/21.

First attempt with Heart roasters. According to the internet these are the best roaster in the country. They are well regarded online. First grind was 20 at 205 degrees. Pour took 6+ minutes. Need to grind much coarser than this. 6 minutes was far too much extraction.

Second brew at 205° at 26 grind setting. This poured much faster at 4:30. However, the taste was watery and there weren't any notes. I think with Ethiopians I don't care about draw down time.", new DateTime(2026, 2, 1), new DateTime(2026, 2, 1) },
                { @"Poppy Soda from Dak", @"An Amsterdam roaster that arrived with a magazine subscription and made one of our best brews", new DateTime(2026, 2, 10), false, @"dak-poppy-soda", @"coffee,coffee-journal,dak,colombia", @"**Dak** · Colombia · ★★★★☆

<!-- IMAGE: dak-poppy-soda.png -->

**Grind:** #20 · **Water:** 208°F · **Dose:** 50g, Hoffmann 2-3 pour recipe

Lemonade, jasmine tea, peach.

Dak Coffee Roasters from Amsterdam. Came with my subscription to Standart magazine. A magazine about coffee, art and life. It's very film photographer-esque.

Great brew. Did 50g at hot water. Used Claude to guide me on brew parameters.

Three pours, swirl at bloom, shake and swirl after the first pour. Uneven bed at finish, should do a better swirl at the end to flatten.

Tasted lemonade after cool down.

Katie liked this a lot. One of the better brews to come out of our house lately.", new DateTime(2026, 2, 10), new DateTime(2026, 2, 10) },
                { @"Sermon from Verve", @"A medium roast blend that finally got me thinking about temperature as a dial, not a constant", new DateTime(2026, 2, 14), false, @"verve-sermon", @"coffee,coffee-journal,verve,colombia,ethiopia", @"**Verve** · Colombia Washed / Ethiopia Natural - Medium · ★★★★☆

<!-- IMAGE: verve-sermon.png -->

**Grind:** #21 pour over, #15 espresso · **Water:** 201-203°F · **Ratio:** 1:16, 40g to 640g

Roast is medium so temp is cooler. 205-207 would be lighter roast. 195 for a dark roast.

Bloom 80g with gentle swirl. Medium won't bubble as much as a light.

- First pour to 380g
- 1:45 pour to 640g
- Give it a swirl to flatten the bed
- Target 4:15 to 4:45

Too thin then grind finer. Too muddy go to 22.

15 espresso pulled 38g in 28 seconds. I could go finer to bring the seconds up and the ratio in grams.", new DateTime(2026, 2, 14), new DateTime(2026, 2, 14) },
                { @"Jhonoton Pinto from Sey", @"A dense pink bourbon that needed a finer grind and hotter water than anything else this year", new DateTime(2026, 2, 17), false, @"sey-jhonoton-pinto", @"coffee,coffee-journal,sey,colombia", @"**Sey** · Colombian Pink Bourbon - Washed · ★★★★☆

<!-- IMAGE: sey-jhonoton-pinto.png -->

**Grind:** #18 · **Water:** 210°F · four pours, 5 minute draw down

Tangerine, honey, blueberry.

Sey roast very light coffee. When brewing a light roast the temp needs to be higher. I went 212 off the boil the first time.

Pink bourbon is a dense bean. Harder to extract.

Grind fine and hot water to pull extractions. Grind one more finer on this one.

Went to grind 18 with temp at 210. 5 minute draw down with four pours. Turned out nice.

I could stretch the ratio or grind back to 19 or 20.

Rich and sweet. Nothing to note but it's good.", new DateTime(2026, 2, 17), new DateTime(2026, 2, 17) },
                { @"Ramon Hernandez from Heart", @"The whole entry is one sentence, and it says everything it needs to", new DateTime(2026, 2, 28), false, @"heart-ramon-hernandez", @"coffee,coffee-journal,heart,honduras", @"**Heart** · Honduras · ★★★★☆

<!-- IMAGE: heart-ramon-hernandez.png -->

**Grind:** #22 · **Water:** 212°F

Didn't have time to write about this coffee but I liked it and that's good enough.", new DateTime(2026, 2, 28), new DateTime(2026, 2, 28) },
                { @"Tagel Alemayehu from Heart", @"Opened this one a week off roast, which was too soon, and week two proved it", new DateTime(2026, 3, 14), false, @"heart-tagel-alemayehu", @"coffee,coffee-journal,heart,ethiopia", @"**Heart** · Ethiopian - Fully Washed · ★★★★☆

<!-- IMAGE: heart-tagel-alemayehu.png -->

**Grind:** #18 pour over · **Water:** 210°F · three pours, 5:30 draw down

Watermelon, peach, honeysuckle. Roasted 3/3/26.

Opened only after a week off the roast date. It wasn't bitter so I call that a win. One week off was too soon and it lacked any real flavor.

Week two it opened up so much more. A really great coffee. A fully washed and clean drink. There's a hint of bitter on the end but nothing bad. A solid brew that's only going to get better this week.", new DateTime(2026, 3, 14), new DateTime(2026, 3, 14) },
                { @"Eduardo Quispe from Sey", @"Left this bag for three weeks while we were in Belize and it was worth coming home to", new DateTime(2026, 4, 1), false, @"sey-eduardo-quispe", @"coffee,coffee-journal,sey", @"**Sey** · ★★★★☆

<!-- IMAGE: sey-eduardo-quispe.png -->

**Grind:** #18 pour over · **Water:** 211°F · 5 minute pour

Really good Sey coffee. Let it sit for three weeks off roast before brewing. We went to Belize for a week after I opened this bag. When we came back this bag was so good to come home to.

There were a few good coffee shops in San Pedro. Nothing roasted like we have here. Lots of espresso options. Bitter and not sure how fresh the coffee was. Drank mostly small lattes.

Katie and Thea had some great sugar coffee drinks.", new DateTime(2026, 4, 1), new DateTime(2026, 4, 1) },
                { @"Ethiopian Bito from Archers ⭐", @"Best cup of coffee of the year, from a UAE roaster that came with a magazine", new DateTime(2026, 4, 3), false, @"archers-ethiopian-bito", @"coffee,coffee-journal,archers,ethiopia", @"**Archers** · Ethiopian - Sama Washed - Guji - 2,350 masl · ★★★★★

<!-- IMAGE: archers-ethiopian-bito.png -->

**Grind:** #16-18 pour over · **Water:** 211°F · **Dose:** 50g, 6-7 minute pour

Jasmine, lemongrass, pear, earl grey.

Second batch of coffee with my second edition of Standart.

A long brew time at 6-7 mins. Poured the whole sample size of 50g which resulted in a longer pour time. This is a really good coffee. It's fruity and smooth. Not a hint of being bitter.

UAE based Archers Coffee is a top tier roaster. They are a direct trade roaster focusing on quality and being transparent in their process.

Best cup of coffee of the year.", new DateTime(2026, 4, 3), new DateTime(2026, 4, 3) },
                { @"Evergreen Espresso from Northern Coffeeworks", @"Looking for one house espresso, because there are already too many variables to track", new DateTime(2026, 4, 4), false, @"northern-coffeeworks-evergreen-espresso", @"coffee,coffee-journal,northern-coffeeworks,colombia,brazil,ethiopia,minneapolis", @"**Northern Coffeeworks** · Colombia + Brazil + Ethiopia - Natural and Washed · ★★★☆☆

<!-- IMAGE: northern-coffeeworks-evergreen-espresso.png -->

**Grind:** #17 espresso · 18g in to 36g out at 28 seconds

Raspberry, cocoa nib, trail mix. Roasted 3-19.

A pretty dark roast for what I normally want from my espresso. Makes a good and rich milk drink. I'm trying to find one espresso for home. Between photography, editing and pour overs there are too many variables to learn and keep track of. There are deep hints of raspberry in the cup. This espresso pulled so good. 18 in 36 out at the perfect 28 second mark. Coming just about two weeks off roast I think this is prime.

Evergreen is Northern's go to espresso. They rotate a single origin as another option at the cafe. Pink Lady Slipper is another one I had and liked better.", new DateTime(2026, 4, 4), new DateTime(2026, 4, 4) },
                { @"Geometry from Onyx", @"Three sour pulls before reading the bag and finding out Onyx wants a much longer ratio", new DateTime(2026, 4, 11), false, @"onyx-geometry", @"coffee,coffee-journal,onyx,colombia,ethiopia", @"**Onyx** · Colombia + Ethiopia - Washed

<!-- IMAGE: onyx-geometry.png -->

**Grind:** #10-14 espresso · **Water:** 205°F · Onyx recommends 20:45 at 24-26 seconds

Fruit, tea like.

First pull of Onyx coffee. Expectations are high. High profile coffee roaster from Arkansas.

- First pull at 14 poured through at 18 seconds. This was a sour pull I tossed. 36g
- Second was 11, came out at 25 seconds. 36g
- Third was 10 at 30 seconds. 36g

Still sour. See, Onyx recommends a 20:45 ratio which is much longer than I was pulling.", new DateTime(2026, 4, 11), new DateTime(2026, 4, 11) },
                { @"Monarch from Onyx", @"A 23 second pull that seems extreme on paper and tastes rich and bold in the cup", new DateTime(2026, 4, 23), false, @"onyx-monarch", @"coffee,coffee-journal,onyx,ethiopia,colombia", @"**Onyx** · Ethiopia + Colombia · ★★★★☆

<!-- IMAGE: onyx-monarch.png -->

**Grind:** #16 espresso · 19g in to 47g out at 23 seconds

First pull on Monarch. Grind was 11. Way too fine. Pull took 44 seconds to get to 47g. First sip at this ratio is darker but not bitter like pure Italian roast have been. Moving grind size up should produce better results.

I've tightened this brew up now. Grind size is 16 and I'm pulling at 23 seconds. Which seems extreme but the shots are pretty rich and bold. This coffee is surprising!

Katie doesn't love this espresso. On to the next.", new DateTime(2026, 4, 23), new DateTime(2026, 4, 23) },
                { @"David Munoz Las Flores from Heart", @"Never found the setting for this one, and I am not sure the bean was the problem", new DateTime(2026, 4, 26), false, @"heart-david-munoz-las-flores", @"coffee,coffee-journal,heart,honduras", @"**Heart** · Honduras - Santa Barbara - Washed - 1,620 masl · ★★★☆☆

<!-- IMAGE: heart-david-munoz-las-flores.png -->

**Grind:** #21 pour over · 640g total

Orange blossom, praline, chocolate taffy.

First pour was flat and watery. Started at 21 grind with 640 total. Draw down was 5 minutes. I need to tighten my grind and ratio to get the full flavor.

I never got this coffee to taste that good. It'd come out bitter and too dark. Maybe the bean was just too dark for us.", new DateTime(2026, 4, 26), new DateTime(2026, 4, 26) },
                { @"Carlos Cadena from Port 2050", @"A well roasted bag that is just not my flavor, picked out by someone paying attention", new DateTime(2026, 5, 7), false, @"port-2050-carlos-cadena", @"coffee,coffee-journal,port-2050,mexico", @"**Port 2050** · Mexico - Natural - 1,880 masl · ★★★☆☆

<!-- IMAGE: port-2050-carlos-cadena.png -->

**Grind:** #10 espresso · 18g in to 37g out

Cranberry, pomegranate, dark chocolate.

Not good for me. First Port 2050 I tried. Great roast, you can tell by the taste. It's way too fruit forward for me. Can really taste the chocolate and sour fruits.

Will try this roaster again. Katie picked this bag up for me. Bless her, but those notes are not what I like. Love that she knew I wanted to try this roaster.", new DateTime(2026, 5, 7), new DateTime(2026, 5, 7) },
                { @"Vilma Miranda from Heart", @"Opened a week early at three weeks off roast and it held up fine", new DateTime(2026, 6, 13), false, @"heart-vilma-miranda", @"coffee,coffee-journal,heart,honduras", @"**Heart** · Honduras - Santa Barbara - Washed - 1,700 masl · ★★★★☆

<!-- IMAGE: heart-vilma-miranda.png -->

**Grind:** #19 pour over · 4:00 to 5:30 pour

Yellow peach, maple candy, bergamot.

Opened this bag a week early at 3 weeks. It's good so far.

Rich and fruity. Took a couple grinds to get going. Stands up nice at single cup too.

Started at 1:15, ended up 1:16 and happy with that.", new DateTime(2026, 6, 13), new DateTime(2026, 6, 13) },
                { @"Ibonia Estate from Sey", @"Too much of a fruit bowl for me, though it found its audience in the house", new DateTime(2026, 6, 24), false, @"sey-ibonia-estate", @"coffee,coffee-journal,sey,kenya", @"**Sey** · Kenya - Kiambu · ★★☆☆☆

<!-- IMAGE: sey-ibonia-estate.png -->

**Grind:** #17-20 pour over · **Water:** 209-210°F

Blackberry, hibiscus. Roasted June 10 2026. More balanced and wine like.

First pour was at 17 with 624g out, 209F. This was a hot washed pour that turned out muddy, heavy and a tad bitter. 17 was a bold move. The pour was close to 7 minutes. Way too long. Washes like it hot. Tastes fruity.

Next try will be 20 grams hoping it all the same. Came out way too watery and flat. Pour time was at about 5 mins. So the timing was good, but I can't get into this coffee. It's too much of a fruit bowl for me.

Matty liked this one a lot. Not for me.

Guests coming this weekend so I went and got three bags of Dogwood. Not amazing, but it will do.", new DateTime(2026, 6, 24), new DateTime(2026, 6, 24) },
                { @"Daysi Munoz from Heart", @"Days of dialing without finding the sweet spot, and a thought about what that costs", new DateTime(2026, 7, 4), false, @"heart-daysi-munoz", @"coffee,coffee-journal,heart,honduras", @"**Heart** · Honduras - Santa Barbara - Pecas - Washed - 1,820 masl · ★★★☆☆

<!-- IMAGE: heart-daysi-munoz.png -->

**Grind:** #20 pour over · **Water:** 204°F

Mango, cherry blossom, panela. Roasted 6/16.

Been playing with this coffee for a few days. Can't quite figure out its sweet spot.

I like Honduras more. It's a good coffee that's 3 weeks off roasting.

Not sure I want to keep spending $30 a bag to dial in a coffee.", new DateTime(2026, 7, 4), new DateTime(2026, 7, 4) },
                { @"Bombona from Coffee Collective ⭐", @"Carried this one home from a Nordic cafe in Pioneer Square and it is the best espresso of the year", new DateTime(2026, 7, 17), false, @"coffee-collective-bombona", @"coffee,coffee-journal,coffee-collective,colombia,seattle", @"**Coffee Collective** · Colombia - Espresso · ★★★★★

<!-- IMAGE: coffee-collective-bombona.png -->

**Grind:** #11 · 27 second draw down

Roasted 6-26, three weeks out.

Picked up this bag in Seattle at Day Made Kaffe Bar in Pioneer Square. It's a scandanavian and Nordic cafe. We sat and had cold coffees in the minimal white space. Katie had a cold brew which was good. I had this espresso as an iced cortado. Thea had soft serve. Great coffee shop in Seattle.

I love this espresso in milk. It's smooth, not bitter at all. I taste the notes of chocolate. Blends so good, man.", new DateTime(2026, 7, 17), new DateTime(2026, 7, 17) },
                { @"Juan Jimenez from Sey", @"Flat and watery on the first pour, but the potential is right there under it", new DateTime(2026, 7, 20), false, @"sey-juan-jimenez", @"coffee,coffee-journal,sey,colombia", @"**Sey** · Colombia - El Porvenir - Pink Bourbon - Washed - 1,700 masl · ★★★★☆

<!-- IMAGE: sey-juan-jimenez.png -->

**Grind:** #20 pour over · **Water:** 209°F

Key lime, lilac.

Pink bourbon is a new variety for Huila, Colombia. Genetically from Ethiopia.

First pour over was light and watery. Started at 20 on the Baratza with a total time of 3:30. Temp was 209°. Going to drop to 18 grind with 207° next time. Came out flat and watery but I can taste the potential.", new DateTime(2026, 7, 20), new DateTime(2026, 7, 20) },
                { @"Byron Hernandez from Heart", @"Hotter water for a high elevation washed bean, and a cold brew batch that worked", new DateTime(2026, 7, 28), false, @"heart-byron-hernandez", @"coffee,coffee-journal,heart,honduras", @"**Heart** · Honduras - Washed - 1,820 masl · ★★★★☆

<!-- IMAGE: heart-byron-hernandez.png -->

**Grind:** #18 pour over · **Water:** 205°F · cold brew 24 hours

Cantaloupe, raspberry, honeysuckle.

First grind is 18 with a temp of 208F for this washed bean, so we go hotter for a little bit of a high elevation bean. Draw down was 4:14, so looking good there.

First test is good. Can really taste that raspberry in that one.

Made a decent batch of cold brew from this roast as well. 24 hours, could be less.", new DateTime(2026, 7, 28), new DateTime(2026, 7, 28) },
                { @"Coffee roastings notes - August 2026", @"The brewing cheat sheet from the back of the 2026 journal, pulled out on its own", new DateTime(2026, 8, 17), false, @"coffee-roastings-notes-august-2026", @"coffee,coffee-journal", @"Notes from the back page of this year's journal. Not about one bag, just the things I stopped having to look up.

**Espresso, in order.** Lock in the three parameters first. 18g of coffee that makes 36 grams of espresso around 28-30 seconds. Once that is locked in you can move on to ratios. The dial in is first.

**When it's off.**

- Sour? Go to 18/40 or 18/44
- Bitter? Go to 18/32

**Pour over temperature.** High elevation washed wants hotter, right off boil. Bring the temp down before going coarser.", new DateTime(2026, 8, 17), new DateTime(2026, 8, 17) },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var slug in Slugs)
            {
                migrationBuilder.DeleteData(table: "Posts", keyColumn: "Slug", keyValue: slug);
            }
        }
    }
}
