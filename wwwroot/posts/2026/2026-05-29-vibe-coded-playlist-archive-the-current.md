---
title: "The Undercurrent.FM - vibe coded 20 years of playlist data from  for 89.3 The Current"
date: "2026-05-29"
description: "I built a website that's completely vibe coded using Claude to display analytics, track plays and artist information for 89.3 The Current. Built in React, C# and MySQL."
draft: true
slug: "vibe-coded-playlist-archive-the-current"
---

I have been listening to <a href="https://www.thecurrent.org" target="_blank">89.3 The Current</a> since my friend excitedly told me about it one night in 2005. His hook was "What would you say if there was a radio station that played Mos Def and The Hold Steady?". Or something like that. That was my introduction to The Current. I still remember that night at Big E's like it was yesterday. The DJs played what they wanted and there were no commercials. Up until that point I had only heard of <a href="https://kexp.org" target="_blank">KEXP in Seattle</a> doing this, now we were doing this awesome thing in Minnesota.

There was a website for KEXP that would track the songs and artists that were played on the station. Users could see how often a certain track was played and what the top artists & tracks were for the month or year. The website is no longer, but I always liked to see that information. For the past ten years I've always wanted to create a similar site for The Current. With Claude Code and having access to agentic engineering I was finally able to carve out a little bit of time to write the first draft of the website. Today <a href="https://theundercurrent.fm" target="_blank">The Undercurrent</a> was born.


<a href="https://theundercurrent.fm" target="_blank"><img src="/images/2026/the_undercurrent.png" class="w-100" alt="The Undercurrent - 89.3 The Current analytics" /></a>

### What does this thing do?

The Undercurrent is an analytics site that I vibe coded to show 89.3 The Current's full playlist history going back to December 22, 2005. The station launched in January of 2005 but the playlist API endpoint only goes back to December 22, 2005 so it's not truly 100% of the tracks they've ever played. 

I created an ongoing database of every single play since then that adds up to roughly 2.2 million plays. On the dashboard, you can see most played songs, top artists, trends over time and even browse playlists by date.

### How it was built — the vibe coding angle

I didn't do a traditional question and answer planning session with Claude to start building this app. Normally that's how I start agentic coding sessions, but this time I described what I wanted to Claude and iterated from there. I used the KEXPlorer as an example of what I wanted to accomplish. I asked Claude to harvest the data and store it in a way that made it easy to display the following data points:

- Top Songs This Week (Most played in the last seven days)
- New to Rotation (Songs appearing for the first time in thirty days)
- Number of unique tracks played year over year
- Total plays
- Trends of songs rising and falling on the playlists
- Artist and song page to display play history over time

I explicitly guided the agent to use a React front end to pull data from a C# API that returned the data from a MySQL database. The API is set up with hangfire to execute a new pull from The Current's own public API endpoint on a timed frequency so there's no scraping required. 

The API also was able to backfill the last 20 years of data in the background with rate-limits to be polite and not hammer The Current's own public API with my own requests.

<a href="https://theundercurrent.fm/browse" target="_blank"><img src="/images/2026/most_played_today.png" class="w-100" alt="The Undercurrent - 89.3 The Current analytics" /></a>

If you do not care about code quality and have a lot of patience, it's amazing just how far you can get with describing the features in conversations with Claude. I was able to architect the solution from a very high level while referencing data points that I wanted to make and Claude delivered them. 

I ran into bugs and build issues along the way, but they really weren't a big deal. Claude is really good at identifying build errors and bugs. The more context you put around the issue, the better the system is at fixing those bugs. I've noticed around the Internet that you can easily spot a website that was vibe coded — or at least dashboards — because they all kind of look like The Undercurrent. I've seen a few dashboards pop up online in the last couple of weeks that all looked very similar to this one. 

Even though this site was vibe coded, I still had to think about how it should be architected and what features needed to be included. Software engineering is definitely changing to be a mix of high level architecture and solution development with a mix of product development. 

### What's next

This is the first draft of the website. I will let it run for a while to see what other information people throw at me. There's already been a feature request to include a link to Youtube along side the Spotify and Apple Music links. Some ideas I have are: decade comparisons, artist deep-dives and a whole bunch of performance improvements. 
