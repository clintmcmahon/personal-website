---
title: "Duplicating Code"
description: "A reflection on why I'm not creating a reusable component for a new feature."
date: "2025-04-08"
draft: false
slug: "copying-code-pragmatically"
tags: consulting
---

I'm adding a new feature to a website that is a duplicate of the same functionality but on a new page. Instead of creating a reusable component that can be referenced from both pages, I'm copying the existing working code for the new page.

I don't like doing this, as it's not clean and it leads to redundant code. However, the way the original code was written, it will require a good effort to create a reusable component out of the existing code. This would take about a day's worth of work and testing.

I've decided to copy the code to create the other component now. This creates duplicate code. If we were to change the functionality down the road, both places need to be updated. This, again, is not the best practice. But with the current implementation, it seems to be the most straightforward and the most structurally sound.

By copying the existing code, this saves time and resources now. The code is two simple models that are pretty straightforward CRUD operations. Because the change is new functionality—versus new and updated, as would be the case for a new reusable component—we only need to test new code and not do a regression test for a new reusable component.

By copying the existing code and not introducing a new feature plus a change I end up:

**✔ Save time & resources ($$)**  
**✔ Limit scope of testing**  
**✔ Reduce risk of introducing new bugs**

If sometime later the business decides to create another component that uses these same features, then it would be time to write a new component that can be reused in all three locations. But for now this is the most straightforward, quick and cost effective solution which gets the job done. 
