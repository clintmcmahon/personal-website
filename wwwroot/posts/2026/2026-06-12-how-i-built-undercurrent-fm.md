---
title: "How I built a 20-year playlist analytics site for 89.3 The Current"
date: "2026-06-12"
slug: "how-i-built-undercurrent-fm"
description: "I built Undercurrent.fm to display playlist analytics for 89.3 The Current going back to 2005 — using React, C#, MySQL and Claude Code."
keywords: "building playlist analytics site, vibe coding with claude code, react c# mysql personal project, 89.3 the current playlist data"
draft: true
---

<!--
WRITING PROMPT

WHAT THIS POST IS ABOUT:
A developer post about building Undercurrent.fm — a real product you shipped using agentic coding. The reader is a developer curious about what vibe coding looks like on a real project (not a tutorial), or someone who wants to know what the site actually does.

THE ANGLE THAT MAKES THIS YOURS:
You have wanted to build this for 10+ years. Claude Code made it possible to carve out the time. But you still had to do the architecture thinking — Claude handled the implementation. That distinction matters and is the honest version of the vibe coding story.

QUESTIONS TO WORK THROUGH:
1. What specific problem were you solving — what data did you want to see that didn't exist anywhere?
2. What was the data situation: how did you get 20 years of plays into a database, what rate limiting decisions did you have to make, what surprised you about the backfill?
3. What did you actually architect vs. what did Claude Code write? Where did you have to think hard, and where did you just describe what you wanted?
4. What broke or went sideways during the build — a specific bug or wrong turn that illustrates what the actual workflow looked like?
5. What does the site show now and what is still missing?
6. What would you do differently if you started today?

STRUCTURE SUGGESTION:
- Open with the problem: the data existed, no one was surfacing it the way you wanted
- The data acquisition: The Current's public API, the backfill strategy, rate limiting, the final row count
- The stack and why: React, C# ASP.NET Core, MySQL, Hangfire — what each piece does
- The build: what the Claude Code workflow actually looked like in practice, where you had to take the wheel
- What it shows now: the specific dashboard views and what they're useful for
- End with a link to the site and one honest thing that still needs work

LENGTH: 600–750 words
AVOID: overselling the vibe coding angle, making it sound effortless, generic "AI is amazing" framing — be specific about where it helped and where it didn't
-->
