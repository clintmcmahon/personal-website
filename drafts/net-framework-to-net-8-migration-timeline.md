---
title: ".NET Framework to .NET 8 Migration: How Long It Takes"
date: "2026-08-06"
slug: "net-framework-to-net-8-migration-timeline"
description: "A realistic timeline for migrating a .NET Framework application to .NET 8, what drives the schedule and where these projects slip."
keywords: "net framework to net 8 migration timeline, migrate net framework to net core, net framework end of life, aspnet webforms migration, dotnet modernization"
tags: "dotnet, migration, architecture"
draft: true
---

<!--
WRITING PROMPT

WHAT THIS POST IS ABOUT:
An IT manager or lead developer has a .NET Framework application and is being
asked "how long would it take to modernize this." They need a defensible answer.
They are searching for a timeline, not a tutorial.

THE ANGLE THAT MAKES THIS YOURS:
You have done this repeatedly, including on systems that could not go down. The
value here is the sequencing and the honest account of where schedules slip,
which is the part nobody writes about.

QUESTIONS TO WORK THROUGH:
1. What are the real timeline brackets by application size and shape? A small
   internal tool versus a public-facing app with integrations.
2. What are the top three schedule killers? Candidates from your experience:
   System.Web dependencies, WCF, third-party libraries with no Core version,
   anything using HttpContext deep in business logic, WebForms.
3. Which parts can run in parallel and which are strictly sequential?
4. Is there a case for the incremental strategy (YARP side-by-side, strangler
   fig) versus a clean cut? When do you pick each?
5. What do you do about the code that has no test coverage, which is most of it?
6. What is the honest answer when the right call is "do not migrate this, replace it"?

STRUCTURE SUGGESTION:
- Timeline brackets up front.
- What determines which bracket you are in.
- The three schedule killers, with what each one costs.
- Incremental versus clean cut, and how to choose.
- The testing problem.
- When to replace instead of migrate.
- Next step, linking /services/legacy-systems.

LENGTH: 700-750 words
AVOID:
- Rewriting Microsoft's upgrade-assistant docs. Assume they found those already.
- Promising a number without saying what it depends on.
- Any suggestion that this is easy if you just know what you are doing.
-->
