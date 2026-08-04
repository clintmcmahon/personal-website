---
title: "Umbraco 7 to 17 Migration: Cost and Timeline"
date: "2026-08-04"
slug: "umbraco-7-to-17-migration-cost-and-timeline"
description: "What an Umbraco 7 migration costs, how long it takes and which version to target now that Umbraco 13 reaches end of life in December 2026."
keywords: "umbraco 7 to 17 migration cost, umbraco 7 to 13 migration, umbraco migration timeline, umbraco 7 upgrade cost, umbraco 13 end of life"
tags: "umbraco, dotnet, migration"
draft: true
---

<!--
BEFORE PUBLISHING, REPLACE THESE WITH YOUR NUMBERS:

1. The $18,000 to $45,000 range. Not from your invoices. Replace with what you
   have quoted and delivered.
2. Every week count in the Timeline section. Your Secretary of State project has
   a real duration. Use it.
3. "Verification is where the schedule goes" is true of migrations generally.
   Confirm it was true for you before saying it in first person.

VERSION FACTS ARE CURRENT AS OF AUG 2026 AND WERE CHECKED AGAINST UMBRACO:
- Umbraco 13 LTS, .NET 8, AngularJS backoffice, security support ends 14 Dec 2026
- Umbraco 17 LTS, .NET 10, Bellissima backoffice, supported to Nov 2028
- Umbraco 14, 15 and 16 are already end of life
Re-check these before publishing if any time has passed. Getting a version fact
wrong on this post costs more than getting the price wrong.
-->

Most Umbraco 7 migrations land between $18,000 and $45,000. The version numbers are not what moves that number. How much of your site was custom is what moves it.

## Target 17, not 13

This is the first decision and most of the advice online is now stale.

Umbraco 13 was the long-term support release for two years, so it is what people searched for and what agencies quoted. Its security support ends on 14 December 2026. Migrating from 7 to 13 today buys you a few months before you are unsupported again.

Umbraco 17 is the current LTS. It runs on .NET 10 and is supported into late 2028. Versions 14, 15 and 16 are already end of life.

One consequence worth knowing before you scope anything: 13 still runs the old AngularJS backoffice. The rewritten backoffice arrived in 14. If you target 17, custom backoffice extensions and property editors are rebuilt in TypeScript against a different API. That is real work. It is also the difference between a 13 quote and a 17 quote. Take it anyway. The alternative is doing this again next year.

## Why this is a rebuild

There is no in-place path from 7. Version 8 dropped the version 7 API and Umbraco never shipped a content upgrade from 7 to 8. Version 9 moved the platform onto ASP.NET Core, so the .NET Framework application underneath you has nowhere to land. You are building a new application and migrating content into it.

## What drives the cost

In order of how much they add:

**Custom property editors and backoffice extensions.** Rebuilt in TypeScript against the new backoffice. Usually the largest line on the quote.

**Legacy grid and macro content.** Version 7 composed flexible pages with the grid, macros and packages like Archetype and Nested Content. Current Umbraco uses Block List and Block Grid. Someone decides, page pattern by page pattern, what the new shape is. This is the piece that gets underquoted most often.

**Abandoned packages.** A site running since 2015 collects packages that were never ported. Each one is either replaced, rebuilt or dropped, and sorting that out belongs at the start.

**Integrations.** Search, forms, authentication, anything talking to another system. Individually small. Collectively not.

**Content volume.** Last. Once the migration is written, ten thousand nodes is not much harder than one thousand. Volume affects verification, not build.

## Timeline

A small site with light customization runs six to eight weeks. A mid-sized site with custom editors and grid content runs ten to fourteen. A large site with integrations and accessibility obligations runs sixteen or more.

Verification is where the schedule goes. Writing the migration is predictable. Confirming that every page arrived with its media, its relationships and its URLs intact is the part that fills the calendar. A quote with no verification time in it is a quote that gets revised later.

## What you can do before calling anyone

- Prune content. Every page you delete is a page nobody migrates or verifies.
- List your custom property editors. Even a rough list changes a quote.
- List your installed packages and mark the ones you still use.
- Decide what to drop. Removing features nobody has opened in three years is cheaper to agree before the quote than after.

## When migrating is the wrong call

If the site is mostly content, the customizations are already abandoned and the design is dated, a fresh build on 17 can cost about the same and leaves you somewhere better. Ask for both numbers before committing to the migration path.

## Next step

Get an inventory before you get a quote. What migrates cleanly, what gets rebuilt, what gets dropped. It is a small piece of work that turns a wide range into a real number. You can take it to any vendor you want to compare against.

More on the [Umbraco migration page](/services/umbraco-consulting). The [Minnesota Secretary of State case study](/portfolio/minnesota-secretary-of-state) covers a version 7 migration on a public system.
