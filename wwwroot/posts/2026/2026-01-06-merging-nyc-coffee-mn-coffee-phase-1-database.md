---
title: "Merging NYC Coffee + MN Coffee Phase 1: Database"
description: "The first phase of combining NYC Coffee and MN Coffee into a unified platform, starting with database consolidation and schema design."
date: "2026-01-06"
draft: false
slug: "merging-nyc-coffee-mn-coffee-phase-1-database"
tags: coffee, nyc coffee, mn coffee
---

In my <a href="/blog/new-york-city-coffee-map">last post about NYC Coffee</a>, I mentioned that maintaining separate codebases for MN Coffee and NYC Coffee was becoming unsustainable. I'm now starting to consolidate everything, beginning with the database layer.

## Single Database, Multiple States

I've merged the MN Coffee and NYC Coffee data sources into a single MySQL database. Both apps were already using Outscraper to pull coffee shop data from Google Maps, so the schema was nearly identical except for the `IsChain` property in NYC Coffee. I don't have that feature in MN Coffee yet but hope to get that done soon. Now instead of two separate databases, I have one database with a state column to differentiate between Minnesota and New York shops.

The Outscraper tasks still run separately for each state. I haven't consolidated those yet, but the data now flows into the same place.

<div class="d-flex">
<img src="/images/2026/nycsubway.png" class="w-50 p-4" />

<img src="/images/2026/nycsubway_results.png" class="w-50 p-4" />
</div>

## New .NET 10 API

To access the data, I built a new .NET 10 API. Right now, each app queries by state. The NYC Coffee app has a query that returns coffee shops where `state = 'ny'`. I'll update MN Coffee to do the same with `state = 'mn'`. 

And eventually I will remove the hard coding and implement `OData` so the queries can be really flexible.

This API will become the single integration point for all coffee apps going forward. No more embedded JSON files in each application. Well after I update the MN Coffee app.

## What's Next: Unified React Native Build

The next phase is consolidating the React Native codebases. I want to use a single codebase for both apps with config-based builds so I can publish multiple projects to EAS under the same repository. I'm not entirely sure how I'm going to structure that yet. Probably some combination of environment variables and app.config.js switching. More on that once I figure it out.

## NYC Subway Overlay

In the meantime, I published a new version of NYC Coffee with a Mapbox overlay for the NYC Subway system. The overlay is courtesy of <a href="https://github.com/chriswhong/mapboxgl-nyc-subway" target="_blank">Chris Whong's Subway Map</a>.

Right now the subway lines and stops are display-only. In a future update, I'd like to add some interactivity to each stop—something like "best espresso near this station" or "closest shop with wifi." That's further down the road, but it's on my list.

