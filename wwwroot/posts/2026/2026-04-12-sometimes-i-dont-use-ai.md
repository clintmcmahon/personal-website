---
title: "Sometimes I Don't Use AI At All"
date: "2026-04-13"
description: "Some clients don't allow AI in their codebase. Here's what working without it looks like in 2026, and why I think the trade-off is worth it."
draft: false
slug: "sometimes-i-dont-use-ai"
---

There are a few reasons I still work without AI. Sometimes I miss the flow of coding. But the main one is that some clients don't allow it in their codebase. I genuinely enjoy working on these projects. They are objectively slower to develop than a <a href="Blog/ai-engineering-2026">full AI agent-assisted project</a>, but in this environment things are like they were three years ago — work on the feature and code everything by hand.

Working without AI is harder than it sounds right now, and not because I've become dependent on it to think for me. The bigger issue is search. Before Google started surfacing AI summaries, you'd search a problem you were stuck on, find the Stack Overflow post or blog that closely matched your situation, and adapt that partial fix to your context. That workflow still exists, but Google now leads with the AI-generated answer and most of the time it's good enough that you never click through to the original source.

On these projects, all architecture decisions fall on me and the team. We talk through problems the old-fashioned way. What I get in return is full ownership of the code — I understand everything the application is doing, with no assumptions baked in from something I half-reviewed. That matters for security. If I wrote it, I can reason about it. A concern I share with a lot of developers right now is how easy it's become to outsource the hard thinking to AI and <a href="https://softwaredoug.com/blog/2025/03/28/ai-brainrot-opportunity" target="_blank">let the underlying skill atrophy</a>. Working through problems manually keeps that from happening.

The obvious cost is speed. Over the past three years I've seen firsthand how much faster AI makes the actual building. I can scaffold an entire .NET OData Web API with Claude in roughly the same time it takes me to wire up Serilog by hand — that's a real and significant gain. AI-assisted development lets me deliver more features faster and at a lower cost to clients than ever before. The trade-off is that I can't trust the output without reviewing it. Everything generated needs a code review to confirm that what I planned is actually what got built and deployed. In the coming years we're going to see vulnerabilities and bugs introduced because engineers failed to properly review the code their AI agents produced. It's the main reason I've added a code review step into my AI agent development work flow. This is good practice going forward whether I wrote the code or Claude did.