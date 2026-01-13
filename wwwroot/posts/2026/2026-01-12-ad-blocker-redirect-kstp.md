---
title: "This News Site's Ad Blocker Redirect is an Interesting Choice"
description: "A local news broadcaster redirects users with ad blockers to a third-party error page with oddly personal messaging."
date: "2026-01-12"
draft: true
slug: "ad-blocker-redirect-kstp"
tags: web, advertising, local-news
---

I was trying to read an article on <a href="https://kstp.com" target="_blank">KSTP's website</a> earlier today and instead of seeing the news story, I got redirected to this:

<div class="text-center pb-2">
    <img src="/images/2026/ad_block.png" alt="Ad blocker message from error-report.com" class="img-fluid" />
</div>

The message reads: "Ads help me cover the costs of running this site, and I work hard to keep them limited and respectful. Turning off your ad blocker just for this site should fix the issue. Thank you for your support."

To put this as a nice Midwesterner, this is an interesting approach. 

There are a few reasons I think it's an interesting choice to redirect the entire page to a generic ad blocker page. One is that KSTP is a full television broadcaster owned by a big broadcasting company that's been around forever. They most likely have a full web developent team, so the choice to redirect to another site that's just text feels out of place for big organization. A subtle modal pop up while keeping the user on the web page would make the experience much more pleasant while still getting the message across.

Another is that the ad blocker references "Me" instead of "We". Makes sense to use the template if it's a small site or personal blog. 

The messaging feels like it was written for a completely different context—probably a small indie website or personal project. When a major broadcaster uses first-person singular pronouns like this, it comes across as disconnected. We're not talking about an individual creator here. We're talking about a company with broadcast towers and a newsroom.

## The Technical Side

Looking at the URL I got redirected to, it's even stranger:

```
https://report.error-report.com/modal?eventId=&error=aWZyYW1lIG1zZyBlcnI6IHhocl9mYWlsZWQ=&domain=fb.html-load.com&url=...
```

That Base64 encoded error parameter decodes to `iframe msg err: xhr_failed`. So KSTP isn't hosting this page themselves. They're using a third-party service called error-report.com to handle ad blocker detection and the redirect.

The redirect URL also includes the full original article URL encoded as a parameter, presumably so the service knows which page the user was trying to access. This setup means KSTP outsourced their ad blocker detection to a company that provides a generic one-size-fits-all message—complete with messaging written for solo creators.

## The Real Issue

I get it. Ad revenue matters for publishers. Running a news operation costs money. But this approach has a few problems.

First, the redirect completely blocks access to the content. There's no option to continue reading with limited functionality or even see a preview. It's a hard wall.

Second, the third-party redirect means KSTP's user experience is now controlled by error-report.com. Whatever generic messaging that service uses is what KSTP users see. The broadcaster apparently didn't customize it to reflect that they're an organization, not an individual.

Third, it creates a weird trust situation. When you click a link to kstp.com and end up on report.error-report.com, that feels off. Most users would probably assume they hit a scam page.

## What Would Be Better

If you're going to ask users to disable their ad blocker, at least own the messaging. Host the page yourself. Write copy that matches your brand. Acknowledge that you're a professional news organization and explain why advertising supports your journalism.

Something like: "KSTP is committed to providing free local news coverage. Advertising helps fund our newsroom and keeps our content accessible to everyone. Please consider adding us to your ad blocker's allowlist."

That's honest, professional, and actually sounds like it came from a news organization.

Instead, KSTP users get a page that sounds like it was written by a blogger with a WordPress site asking for help covering hosting costs. It's a strange choice for a broadcaster that's been on the air for nearly 80 years.
