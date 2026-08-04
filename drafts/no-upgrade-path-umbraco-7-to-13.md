---
title: "Why There Is No Upgrade Path from Umbraco 7 to Umbraco 13"
date: "2026-08-05"
slug: "no-upgrade-path-umbraco-7-to-13"
description: "Umbraco 7 to 13 is a rebuild with a content migration attached, not an upgrade. Here is what breaks and why there is no in-place path."
keywords: "umbraco 7 to 13 upgrade path, upgrade umbraco 7, umbraco 7 end of life, umbraco 8 breaking changes, umbraco migration"
tags: "umbraco, dotnet, migration"
draft: true
---

<!--
WRITING PROMPT

WHAT THIS POST IS ABOUT:
A developer or IT manager has just discovered that "upgrade Umbraco" is not a
NuGet command. They are trying to understand why, so they can explain it to
someone who controls the budget. This post is the technical explanation they
can forward to their boss.

This is the companion to the cost post. That one answers "how much," this one
answers "why so much." Link them to each other.

THE ANGLE THAT MAKES THIS YOURS:
You have been through every one of these breaks on a real system. The post
should read like a field report, not a changelog summary.

QUESTIONS TO WORK THROUGH:
1. What is the actual reason there is no in-place path? Walk the version chain:
   7 to 8 dropped the old API, 8 to 9 moved to ASP.NET Core, 14 replaced the
   backoffice. Which of those is the hardest wall and why?
2. What specifically did you have to rebuild rather than port? Name the
   categories: property editors, grid content, macros, packages, anything that
   touched the request pipeline.
3. What surprised you? The thing you budgeted a day for that took a week.
4. What tooling actually helps? uSync, Umbraco Deploy, custom scripts. Be
   specific about what each one does and does not solve.
5. What is the one decision that determines whether this goes well or badly?
6. How much of the old site should be carried forward at all?

STRUCTURE SUGGESTION:
- State it plainly in the first two sentences. It is a rebuild.
- The version chain and where each wall sits.
- What has to be rebuilt, category by category. This is the body of the post.
- What the tooling covers and what it does not.
- The one decision that matters most.
- Practical next step: the inventory, and link the cost post.

LENGTH: 700-750 words
AVOID:
- Turning it into a changelog. The reader wants consequences, not a version history.
- Any framing that makes the reader feel stupid for being on Umbraco 7. Most of
  them inherited it.
- Speculating about future Umbraco versions.
-->
