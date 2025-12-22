---
title: "NYC Coffee - A New York City Coffee Map"
description: "After launching MN Coffee I decided to create the New York City version of the app to display all the coffee shops around New York City."
date: "2025-12-21"
draft: false
slug: "new-york-city-coffee-map"
tags: programming, coffee
---

Over the past year, I've been working on an <a href="/blog/mpls-coffee-mn-coffee">app that displays all the coffee shops in Minnesota</a> called <a href="https://mplscoffee.com" title="MN Coffee" target="_blank">MN Coffee</a>. The app has been really helpful for finding different coffee shops around Minnesota. A few weeks ago I was listening to an episode of <a href="https://www.youtube.com/watch?v=a3R7fonafqQ">Dan Loves To Chat</a> where Dan Geneen is interviewing <a href="https://cultureespresso.com">Johnny Norton of Culture Espresso</a>. In the podcast they talk about different New York Coffee shops which got me thinking about when we lived in New York and all the great coffee shops that exist in the city. That got me thinking about how MN Coffee could be applied to NYC. With a much larger user base and more coffee options, building the NYC version seemed like a good idea.

<img src="/images/2025/nyc_coffee_map.png" alt="NYC Coffee Shop Map" class="w-100 mb-4"/>

That's where <a href="https://nyccoffeemap.com" title="NY Coffee" target="_blank">NYC Coffee</a> comes in. Built on the same foundation as MN Coffee, this app displays coffee shops across New York City, highlighting those closest to the user. Using the `expo-location` package, the app shows users their location on the map along with all nearby coffee shops. When a user clicks on a coffee shop, a toast-style popup appears with a few details about the shop. Clicking the popup opens a full detail page with more in depth information.

The shop details are sourced from Google Maps via Outscraper, and I cross-reference each coffee shop with Google Places to retrieve the Google AI Summary.

<div class="row">
    <div class="col-md-4">
        <img class="w-100" src="/images/2025/screenshot1_device.png" alt="NYC Coffee map view showing coffee shops in Manhattan">
    </div>
    <div class="col-md-4">
        <img class="w-100" src="/images/2025/screenshot2_device.png" alt="NYC Coffee shop detail popup with quick information">
    </div>
    <div class="col-md-4">
        <img class="w-100" src="/images/2025/screenshot3_device.png" alt="NYC Coffee full detail page with shop information">
    </div>
</div>
<div class="row my-3">
    <div class="col-md-4">
        <img class="w-100" src="/images/2025/screenshot4_device.png" alt="NYC Coffee list view of nearby shops">
    </div>
     <div class="col-md-4">
        <img class="w-100" src="/images/2025/screenshot5_device.png" alt="NYC Coffee search and filter options">
    </div>
     <div class="col-md-4">
        <img class="w-100" src="/images/2025/screenshot6_device.png" alt="NYC Coffee user location and navigation features">
    </div>
</div>

The NYC Coffee codebase is essentially identical to MN Coffee. I simply copied the web and mobile projects into their own folders and made the necessary updates for the New York version. While I initially considered building a multi-city app, I really wanted to publish quickly and opted to duplicate the codebase instead. However, now that both apps are gaining traction and I'm maintaining updates across multiple platforms, a consolidated repository is becoming necessary. As of today, NYC Coffee is closing in on 100 downloads in the three weeks it's been live in the app store. 

<img src="/images/2025/coffeeshopsfiles.png" alt="Coffee shop files" class="w-100"/>

With versions for data harvester, web, iOS, and Android, I'm currently maintaining four separate codebases - that's becoming unsustainable, especially when factoring in the data harvester, database, and other supporting utilities. Currently, data for each app is stored at the database level and then exported to JSON files that are embedded in each application. To streamline this, I plan to consolidate everything into a single database with a C# API as the integration point. By connecting OData to the API, I can use it as the single access point for all apps. This will be the first step in my consolidation plan.

Consolidating the codebases will streamline the publishing process, allowing me to spend more time building new features rather than managing deployments across multiple platforms. 

NYC Coffee is stable and running so now my next step is going to be putting the code for both apps into a single project. More on all of that later, in the mean-time, take a look at NYC Coffee at the links below:

Web - <a href="https://nyccoffeemap.com/">NYC Coffee Web</a> <br />
iOS - <a href="https://apps.apple.com/us/app/nyc-coffee-map/id6755573635">NYC Coffee iOS App</a><br />
Android - <a href="https://play.google.com/store/apps/details?id=com.parkasoftware.nyccoffee">NYC Coffee Android App</a>


