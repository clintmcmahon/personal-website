---
title: "I Rebuilt My Website In .Net 8"
description: "A breakdown of my personal site rebuild using .NET 8, Bootstrap, and markdown-powered blogging."
date: "2025-07-24"
draft: false
slug: "i-built-my-website-in-dot-net-8"
tags: [development]
---

<section>
    <p>
        After years of bouncing between WordPress, Tumblr and various static site tools like Jekyll, I decided to build my personal website from scratch using the latest version of .Net at the time - .NET 8. I’ve always wanted full control over my website and have never truly been happy with any of the popular CMS products on the market. So, after a while I just decided to write my own blogging platform / personal website. After all, I believe that <a href="https://www.hanselman.com/" target="_blank">Scott Hanselman</a> wrote his own personal website using .Net as well. 
        </br></br>
        This post outlines how the site is structured, how the blog engine works and a few practical things I’ve done to make managing it simple(er).
    </p>
    <h2>
        Home page and layout
    </h2>
    <p>
        The home page and all the content pages are built with Bootstap 5 and .Net's Razor Views. No frontend frameworks other than good old fashioned jQuery. It's solid and easy to use modern CSS via Bootstrap 5 and a utilizing the .Net shared layout works well on both desktop and mobile. The only real drawback is the site is obviously a Bootstrap site. I'm a developer, not a designer so this is actually OK with me.
    </p>
    <h2>
    Blog engine
</h2>
<p>
    I didn’t want to rely on a CMS or database for my blog. Instead, I wrote a lightweight system that reads markdown (<code>.md</code>) files from disk, parses them into HTML, and generates both the blog list and individual post pages. There’s no admin dashboard or anything like that. I really liked the idea of creating a single file per post so for each blog post I create the file in <code>wwwroot/posts/{year}/{month}</code> and deploy. The blog service reads that entire posts folder to parse each title for the list. Each post uses YAML frontmatter for metadata like title, date, slug, and tags.
    <div>
        <img src="/images/2025/posts_list.png"  />
    </div>
</p>
<p>
    The BlogController handles all the routing and rendering. It loads posts at startup, ignores drafts, and serves up clean, SEO-friendly URLs. Pagination is built in, and the structure is ready for tag filtering if I want to add it later. This approach keeps things simple, fast, and version-controlled. If you’re comfortable with markdown and git, it’s a frictionless way to run a personal blog with no database dependency. Just code and content.
</p>
<h2>
    Page structure
</h2>
<p>
    The site includes the standard set of pages you’d expect on a personal or consulting site:
    <ul>
        <li><strong>About page</strong> – overview of who I am and what I do</li>
        <li><strong>Photos page</strong> – displays photos in a masonry-style layout</li>
        <li><strong>Contact page</strong> – basic form or link to email</li>
        <li><strong>Projects page</strong> – writeups of work I’ve done</li>
        <li><strong>Portfolio page</strong> – similar to projects, but focused on visuals or client deliverables</li>
    </ul>
    Everything runs server-side in .NET. No JavaScript frameworks except for jQuery. The rest is just Razor views, Bootstrap, and a bit of vanilla JS or jQuery where needed.
</p>
<h2>
    Photo blog
</h2>
<p>
    I really enjoy taking pictures, especially now that I have the Richoh GR IIIx. Instagram is good to an extent but I'm getting tired of all the ads and distracting reels that are pushed into my feed. I really just want to have a photography app where I can look at pictures from the people I follow along with a good <strong>photography</strong> recommendation engine. Instagram is OK with the photograph recommendation, however it's not great with all the extra ads and obscure content that gets pushed my way. 
</p>
<p>
    I started posting pictures to my personal site in addition to Instagram. So instead of having the images go top to bottom in a single column, I installed the glightbox jQuery plugin to display the images in a masonry grid. All the images go inside a parent container div tag with the appropriate class and the plugin arranges them in a pretty grid. My recent trip to the <a href="/blog/grand-view-lodge">Grand View Lodge in northern Minnesota</a> is a good example of the plugin at work.
</p>
<h2>
    Why I built it from scratch
</h2>
<p>
    I’ve wanted to build something minimal and personal for a while but couldn't find any existing themes out there that were simple enough for me and actually looked good to me. What finally pushed me over the edge was pure frustration over searching and searching wihtout finding anything I liked.
</p>
<p>
    So I wrote my own. I started using Copilot to scaffold out the basic template and .Net MVC website architecture. I then started creating controllers with list and detail views, then iterated from there. The result is a site that does exactly what I want, with no extra bloat or features I don’t need. I think 50% of this site has been developed using CoPilot or ChatGPT. There have been a bunch of times where I needed to step in and correct the robot to write the correct code. 
</p>

<h2>
    Hosting and deployment
</h2>
<p>
    I host the site in Azure for about $60/month. Azure handles deployment via GitHub actions every time I push to the main branch. That out-of-the-box workflow made it worth paying instead of trying to cobble together a static deployment. I have a dedicated Windows server that I manage that I will someday migrate off of Azure and onto this hardware - but that's going to require an entire new CI/CD pipeline process that I just don't have time for right now.
</p>

<h2>
    Sharing the code
</h2>
<p>
    I really like the .NET ecosystem for projects like this. Styling is handled with Bootstrap 5, and I’ve shared the full source on GitHub for anyone who wants to build their own blog or site like this. If you have questions or want to show off what you’ve built, feel free to reach out.<br /><br />
    <a href="https://github.com/clintmcmahon/personal-website" target="_blank">Full .Net MVC Personal and Blog website</a>.
</p>
</section>