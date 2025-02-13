---
title: "Move from React Native Maps to Mapbox"
description: "As I added more and more coffee shops to MPLS Coffee I noticed a huge performance hit with React Native Maps. I updated the entire app to use Mapbox and it's so much faster."
date: "2025-02-15"
draft: false
slug: "move-from-react-native-maps-to-mapbox"
tags: react-native
---

 <section>
    <p>
        I just released a major update to my <a href="https://mplscoffee.com" target="_blank">Minneapolis coffee shop finder app</a>, MPLS Coffee. The biggest change on this version of the app was the switch from React Native Maps to Mapbox for the map portion. This was that only major update I made to the public facing app, but some behind the scenes updates were including all coffee shops in Minnesota and changing the way I was collecting the coffee shop data.
    </p>
        <h2>React Native Maps --> Mapbox</h2>
        <p>
            For the <a href="/blog/find-best-coffee-minneapolis">first iteration of MPLS Coffee</a> I was using React Native Maps as the mapping framework for the app. React Native Maps is nice that it can use Apple Maps on iOS and Google Maps for Android. Out of the box it performed great and I didn't have any problems. But over time as I added more coffee shops to the display the app performance took a nosedive. When I crossed the line to 50-100 coffee shops listed on the map there was a significant lag when navigating around the map. The final straw was that when I would toggle between boolean properties, for instance coffee shops that were open now, the redraw of shops took a while. And in some cases the map wouldn't redraw with the correct coffee shops. It was only when I moved the map slightly that all the new coffee shops would be drawn. My initial suspicion was this was related to the onRegionChange method but instead of spending more time debugging the issue I went looking for an alternative.
        <p>
            I use <a href="https://parkasoftware.com/portfolio/organ-procurement-organization-interactive-report/" target="_blank">Mapbox on the web</a>. It's solid and I've never had a performance issue with it. I found that there is a <a href="https://github.com/rnmapbox/maps">Mapbox React Native module</a> that has been around for a while and decided to give it a shot. The implementation wasn't as easy as using React Native Maps, but in the end it's super fast and can handle the 1,000 coffee shops I'm throwing at it without a problem. There were a couple of bumps I ran into setting up the module, but with a little help from StackOverflow (Chat GPT was no help) I was able to get passed any blockers that came up.
        </p>
        <div class="row pb-3">
            <div class="col">
                <img src="/images/2025/iphone.jpg" class="w-100" alt="MPLS Coffee - Minnesota coffee shop locator"/>
            </div>
        </div>
        <h2>All Minnesota Coffee Shops</h2>
        <p>
           I removed the Good Coffee filter because it was too much manual editing. That and trying to define what "good coffee" is became too much. When I decided to remove the Good Coffee filter, I decided to open up the coffee shops to the entire state of Minnesota. I was running my own Google Places C# console application that I ran once a month to harvest the coffee shops in Minneapolis and St. Paul. As soon as I decided to include the entire state of Minnesota things got a lot more complicated. The Google Places API only allows for 60 results per search and it's very difficult to narrow down returning JUST coffee shops in a zip code. It was just too complicated for me to run trial and error over and over again. I'm sure if I were to focus more and really dig into the Places API I would have come away with a nice working solution. However, I have limited time to dedicate to this project as is, so I need to get the data as fast and easy as possible to start. 
        </p>
        <p>
            After burning through my trial for the Places API I started looking for more accurate Google Places API integrations that other people had done. I came across a Reddit post talking about <a href="https://outscraper.com/" target="_blank">Outscraper</a> - the web scraper that can get you just about anything from Google Maps. The person in the post used the service to get a list of local businesses, not needing all local businesses I decided to sign up and see if I could get just coffee shops in Minnesota. I could and did for free. What was costing me $300/month in fees from Google I was able to download all the coffee shops in Minnesota for free in a matter of minutes. And it's good data, too. The only thing that was missing from this dataset is the Generative AI Overview and AI Descriptions for each place. Google has started added these summaries based on their user reviews and have been including them in their search results. Sine Outscraper provides me with the Google Places Id, I can loop through their dataset to hit the Google Places API to return these two fields. 
        </p>
        <hr />
        <p>
            Now that this iteration is out the door I might start playing around with blog posts for the <a href="https://mplscoffee.com" target="_blank">MPLS Coffee website</a>. Something witty and possible AI generated. I was thinking using a Tom Hanks tone or perspective to add some spice to coffee shop overviews. I don't know yet but will definitely be thinking about it some more.
        </p>
        
</section>
