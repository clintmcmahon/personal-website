---
title: "How I post photos to my website from my phone without opening the project"
date: "2026-06-12"
slug: "photo-uploader-post-from-your-phone"
description: "I built an admin uploader that commits photos directly to GitHub from the browser — no local dev environment, no terminal, works from any device."
keywords: "post photos to website from phone, publish to github from web form, personal website photo uploader no dev environment, github api commit from browser"
draft: true
---

<!--
WRITING PROMPT

WHAT THIS POST IS ABOUT:
A developer post about a small quality-of-life tool you built for your own site. The reader is someone who runs a personal website and finds posting content to it too painful to bother with regularly. The post should make the problem concrete and the solution reproducible.

THE ANGLE THAT MAKES THIS YOURS:
You built it because the friction was killing the habit — photos sat in your camera roll instead of on the site. The Instagram comparison is the right frame: Instagram works because the upload experience is frictionless. Most personal sites never bother getting that right.

QUESTIONS TO WORK THROUGH:
1. What did the old process actually look like step by step — be specific about the exact friction points that made you not bother?
2. What does the new upload page look like and how does it feel to use vs. the old way?
3. What is the technical core of how it works — specifically the GitHub Git Data API commit flow (the part that makes it work without a local environment)?
4. What does the ImageProcessingService do that you were previously doing manually?
5. What tradeoffs did you make — what does the uploader NOT handle well that you still do locally?
6. What would someone need to replicate this on their own site?

STRUCTURE SUGGESTION:
- Open with the old process: the exact number of steps, the Mac dependency, the dead end on mobile
- The result: what the new flow looks like (drop image, fill two fields, publish — time it)
- How it works technically: the service chain from drop zone through resize through GitHub API commit
- The GitHub commit detail: why committing via the Git Data API is the key piece that removes the local dependency
- Tradeoffs: what it handles vs. what still requires the project open locally
- End with a practical note for anyone who wants to build the same thing

LENGTH: 600–750 words
AVOID: overselling it as Instagram-grade, implying it replaces a CMS — it's a focused admin tool for one specific use case
-->
