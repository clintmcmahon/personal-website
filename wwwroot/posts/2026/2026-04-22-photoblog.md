---
title: "Building a Photoblog with AI"
date: "2026-04-22"
description: "After years of posting photos only to Instagram I decided to build a dedicated photoblog at photos.clintmcmahon.com. This is the story of why I built it, how I used AI to migrate 250 old Tumblr posts and why the photos — not the code — are the product."
draft: false
slug: "photoblog"
---

After seeing <a href="https://florian.photo/" target="_blank">Florian's photoblog</a> and a handful of others like it, I decided to create my own dedicated space for photos. For a while I was posting photos to my regular blog as normal posts, but I wanted a separate space dedicated to just posting photos. <a href="https://www.instagram.com/cwmcmhn/" target="_blank">Instagram</a> is kind of that place already, but what I don't like about Instagram is the small screen constraint and all the noise. With ads, recommended posts and reels it's much too distracting. It's also a battle to display multiple photos of different dimensions within the same post.

In the mid-2000s there were a lot of great photoblogs. I'm nostalgic for that era of the internet of personal sites, blogrolls, photoblogs and RSS feeds. Others seem to be nostalgic for it too. Photography on the indie web has been gaining more traction year after year. Photoblogs are making a comeback, for me at least. Maybe the hard-core folks never left.

The photoblog code lives in this site's codebase. I created a new photo folder structure, along with its own set of views and a controller that serves the site at [photos.clintmcmahon.com](https://photos.clintmcmahon.com). The routing is handled through IIS and a subdomain that points back to the same application. The controller and web.config handle routing to serve up the different shared `_layout.cshtml` so that when you hit the subdomain you see a different site design that fits photoblogs better than a personal website. So if you are on [photos.clintmcmahon.com](https://photos.clintmcmahon.com) you are on the same site as [clintmcmahon.com/photos](https://clintmcmahon.com/photos) but served up an entirly different design.

I used to run a Tumblr with all my photos from around Minneapolis. That Tumblr eventually died. Now that I have more free time, photoblogging is back. I had about 250 posts on that old Tumblr that I wanted to migrate over to the new codebase. I was able to use Claude to help me create a migration plan by laying out the spec, the schema differences between the two platforms and the steps to bring everything over. The AI executed what I had envisioned perfectly. What could have taken days of manual work was done in about 15 minutes.

## AI Built Most of This

AI was used in the majority of building this photoblog. I didn't want coding up a photoblog website to be a completely new project to work on. Since I have plenty of those already, I wanted the photos to be the project here. So, I created a plan with Claude to develop a minimal photoblog that I can drop photos into a folder and have multiple photos of different dimensions. I created a `.md` file structure that allows me to be explicit of how each blog post is laid out. Then worked through the bugs and guided the project the rest of the way to finish. I always kept the architecture and vision role for myself. What AI is really good at is taking a well-thought-out plan and executing the actual code and repetitive parts quickly so I'm able to use this new product for what I want: posting photos.

In the end I enjoy desinging and architecting this site with AI. Going from zero to working project on my own would have taken the entire weekend. That's if I had time to build it out for an entire weekend. Using AI changes that timeline significantly. I still love building things and coding, but for applications where I just want the product and want to be part of building the product — but don't want to actually code the thing — AI coding is really great. Now that I have a functioning photoblog website I don't need to think about the code that runs the site, I can use AI to add the features that I want going forward. This makes it so much easier to focus on photos and getting them posted. 
