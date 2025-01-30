---
title: "Find the best coffee in Minneapolis"
description: "An overview of how my coffee shop finder, MPLS Coffee, is build. The app that helps you find the best coffee in Minneapolis, MN."
date: "2025-01-06"
draft: false
slug: "find-best-coffee-minneapolis"
tags:
---

 <section>
    <p>
        I've been developing an app called MPLS Coffee to help people find <a href="https://mplscoffee.com" target="_blank">the best coffee in Minneapolis</a>. The idea started when my wife showed me the list of cozy coffee shops in Minneapolis, that got me thinking that it would be cool to just have an app that I could open when I wanted to find a good coffee shop around the city. The app was released at the end of October and has slowely grown over the past two months. 
    </p>
    <p>
        I'm trying to keep up with a monthly release schedule, but with my <a href="/services">freelance development work</a> as well as other obligations in life, it's been more difficult than I imagined to keep this schedule. 
    </p>
    <p>
        When I had the idea to start the app, I used Chat GPT to develop the MVP. My prompts were simple, create a React Native application that will display a list of coffee shops in Minneapolis, MN. Each coffee shop should list the open hours, address and there should be a button for directions to the coffee shop that integrates with the users device. This was actually pretty helpful as the LLM created a very bare bones application with integration with Apple Maps for iOS and Google Maps for Android. The list of coffee shops was a simple javascript array with latitude and longitude included with name, address and open hours. 
    </p>
    <p>
        But soon over time the application got more and more complex. I realized that I needed a database to hold the coffee shop data. Then I needed a way to get the data from the database to the mobile app. And in order to get the data in the first place, I was going to need to write a data loader/harvester to integrate with the Google Maps API.
    </p>
    <p>
        What started as a simple mobile app - basically just to see if I could do it - turned into a larger project that required multiple dependant projects. So, the first iteration of the application consisted of the following parts:
        <ul>
            <li>MySQL database with two tables: CoffeeShops and CoffeeShopHours</li>
            <li>C# console application that gets all the coffee shops in Minnesota from the Google Places API</li>
            <li>C# WebAPI application written with OData to return coffee shops from the MySQL database</li>
            <li>React Native mobile application as the user app</li>
            <li>Next.js landing page/marketing website for <a href="https://mplscoffee" target="_blank">MPLS Coffee</a></li>
        </ul>
    </p>
    <p>
        I published the first version of the app using this architecture. Soon I realized that I didn't actually need the WebAPI for the mobile app and I could improve performance if I bundled the coffee shop list in a .json file within the app package. This way when users load the application there isn't a brief wait time while the app gets the list of coffee shops from the API, the data is already right there on their device. 
    </p>
    <p>
        Last night I released version 1.2 that removed the API from the mobile app and now data is loaded directly from the application. In total the app build is only 23MB, which isn't tiny but compared to other applications in the app store it's actually pretty decent. I've got more updates to the app that I will work on in January as time allows. 
    </p>
    <p>
        <i>Are you looking for the <a href="https://mplscoffee.com">best coffee shops in Minneapolis</a>? Download <a href="https://mplscoffee.com">MPLS Coffee</a> for <a href="https://apps.apple.com/us/app/mpls-coffee/id6736864166?platform=iphone">iOS</a> and <a href="https://play.google.com/store/apps/details?id=com.parkasoftware.mplscoffee">Android</a> now to start finding good coffee.</i>
    </p>
</section>
