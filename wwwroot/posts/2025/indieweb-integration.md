---
title: "Adding IndieAuth and IndieWeb Features to My ASP.NET Core Website"
date: 2025-06-06
description: "How I made clintmcmahon.com IndieWeb-friendly with IndieAuth, microformats, and the IndieWeb Webring."
tags: [indieweb, indieauth, aspnetcore, microformats, webring, personal-site]
slug: indieweb-integration
---

Over the past few days, I set out to make my personal website (clintmcmahon.com) more IndieWeb-friendly. This post documents the steps I took to add IndieAuth authentication, microformats, and IndieWeb webring support to my ASP.NET Core MVC site.

## Why IndieWeb?

The IndieWeb is a movement to take back ownership of your online identity and content. By using open standards like IndieAuth and microformats, you can make your site interoperable with other independent sites and tools, and even log in to other services using your own domain.

## Step 1: IndieAuth Setup

I wanted to use IndieAuth for personal authentication. IndieAuth lets you log in to your own site (and others) using your domain as your identity. I chose the simplest route: using indieauth.com as my provider.

### 1.1. Add a rel="me" Link
I added a rel="me" link to my GitHub profile in my site's `<head>`:

```html
<link rel="me" href="https://github.com/clintmcmahon" />
```

### 1.2. Verify with IndieAuth.com
I went to [indieauth.com](https://indieauth.com), entered my domain, and followed the prompts to verify ownership using my GitHub account.

## Step 2: IndieAuth Integration in ASP.NET Core

I created an `AuthController` with endpoints for login, callback, and logout. The login endpoint redirects to indieauth.com, the callback handles the token exchange, and the session is managed using ASP.NET Core's built-in session middleware.

I also added a login/logout link to the footer (and later removed it for a cleaner look).

## Step 3: IndieWeb Webring

To join the IndieWeb Webring, I added these links to my site's footer, centered between my copyright and (optionally) the login:

```html
<a href="https://xn--sr8hvo.ws/previous">&larr;</a>
An <a href="https://xn--sr8hvo.ws">IndieWeb Webring</a> 🕸💍
<a href="https://xn--sr8hvo.ws/next">&rarr;</a>
```

## Step 4: Add a Representative h-card

A representative h-card is a microformats2 way to mark up your identity for IndieWeb tools. I added a hidden h-card to my layout:

```html
<div class="h-card d-none">
  <a class="p-name u-url" rel="me" href="https://clintmcmahon.com/">Clint McMahon</a>
  <img class="u-photo" src="/images/me.jpg" alt="Clint McMahon" />
</div>
```

## Final Thoughts

With these changes, my site is now much more IndieWeb-friendly. I can log in with IndieAuth, my identity is discoverable via microformats, and I’m part of the IndieWeb Webring. If you want to make your own site IndieWeb-compatible, I hope this post helps you get started!

---

*Questions or want to connect? Find me on [GitHub](https://github.com/clintmcmahon) or via my [contact page](/contact).*
