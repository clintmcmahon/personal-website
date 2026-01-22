---
title: "My local news site redirects you to a third party website for ad blockers"
description: "A local news broadcaster redirects users with ad blockers to a third-party error page with oddly personal messaging."
date: "2026-01-20"
draft: false
slug: "ad-blocker-redirect-kstp"
tags: web, advertising, local-news
---

I was trying to read an article on a <a href="https://kstp.com" target="_blank">local Minnesota news org's website</a> earlier and instead of seeing the news story, I got redirected to this template ad blocker notification page:

<div class="text-center pb-4">
    <img src="/images/2026/ad_block.png" alt="Ad blocker message from error-report.com" class="img-fluid" />
</div>

The message reads: "Ads help me cover the costs of running this site, and I work hard to keep them limited and respectful. Turning off your ad blocker just for this site should fix the issue. Thank you for your support." I've never seen this redirect before so I looked it up online. Turns out lots of other people have <a href="https://www.google.com/search?q=error-report.com" target="_blank">already reported on this site</a>.

I thought redirecting the user to a third-party website that uses a generic template to ask the user to turn off their ad blocker was an interesting approach for a large news organization. There are a few reasons this stood out to me.

First, KSTP is a full television broadcaster owned by a big broadcasting company that's been around forever. There's probably a full web development team supporting this site, the redirect feels out of place for a big organization. A subtle modal popup while keeping the user on the web page would make the experience much more pleasant while still getting the message across. I would be more inclined to turn the ad blocker off if I were still on the page. But by pushing me to another domain makes me more inclined to move on to another site instead of staying on their page.

Second, the error message uses "Me" instead of "We". This makes sense for a template used by small sites or personal blogs, but that's not the case here. 

## What I Would Do Differently
I get why someone would use this redirect, it's a lot easier than building something within their site. This option is more attractive if their website is hosted via off the shelf platform and not home grown. Their ability to generate revenue is important and ad revenue matters for publishers. Without it they have no way to pay their employees. And running a news operation costs money. From a technical standpoint, this approach has a few problems.

First, the redirect completely blocks access to the content. There's no option to continue reading with limited functionality or even see a preview. It's a hard redirect somewhere else. I would create a modal that disables the rest of the site but still keeps you on the main page. This would allow users to see that content exists and, with a simple click, turn off their ad blocker and enjoy the full experience within a few seconds.

Second, the third-party redirect means KSTP's user experience is now controlled by error-report.com. Whatever generic messaging that service uses is what KSTP users see. The broadcaster apparently didn't customize it to reflect that they're an organization, not an individual.

Third, it creates a weird trust situation. When you click a link to kstp.com and end up on report.error-report.com, that feels off. Most users would probably assume they hit a scam page. That's exactly how I felt before reading the content and realizing that this was in response to my ad blocker.

## What Would Be Better

An upfront message explaining the problem and how to resolve it—all within the same domain, and ideally on the same landing page. The message should be customized to reflect the website, not the out-of-the-box text from the provider.

Something like: "KSTP is committed to providing free local news coverage. Advertising helps fund our newsroom and keeps our content accessible to everyone. Please consider adding us to your ad blocker's allowlist."

That's honest, professional, and actually sounds like it came from a the organization.

If anyone at KSTP someday happens to read this post, I would be happy to implement this feature for you. Just <a href="/contact">reach out</a> and let's get that ad blocker messaging taken care of.