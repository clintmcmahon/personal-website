---
title: "AI Engineering in 2026: How We Build Software with AI"
date: "January 7, 2026"
draft: true
slug: "ai-engineering-2026"
---

2025 wasn't the year of AI agents like a lot of the big tech giants would have liked it to be. Instead, it was the year AI became a pretty good engineering tool for software teams. Here's how we use AI to build software in 2026 and how that allows us to build, debug and ship faster for our clients.

## The Shift from Hype to Engineering

The software industry is super obsessed with autonomous agents that will replace developers. It's been the talk of AI company founders for years.  That didn't happen in 2025 and doesn't appear to be something that is going to happen in 2026. However, what did happen was that AI became a reliable collaborator in the software development process. We moved past the [AI hype cycle](/Blog/ai-integration-reality-check) and into productive daily use. It's become a tool that we as software developers can use along side us to improve the development process.

The difference between 2024 and 2026 isn't that LLMs got dramatically smarter, but we learned how to work with it more effectively. Instead of looking at LLMs and AI, in general, as something that is going to run on it's own and run software teams out of the room - it's something that can make us better at doing our jobs.

## Starting with a Plan

When we work AI to create software, this is how we do it. It starts with having a plan. Every project starts with a general plan or a basic level of requirements. Before we write any code, we write up the plan and documentation. We take that documentation and provide it to Claude Code and ask it to analyze our approach. The goal isn't to have AI validate our thinking, but to bring in the context of what we are building. This step is good for a couple of reasons; the first is what I just said. It creates a context around what we are trying to build and the second, This process exposes any gaps in our thinking that we might have missed. 

The key technique is to have Claude ask you questions where it finds gaps in the plan or where it doesn't understand the plan. I learned from <a href="https://harper.blog/2025/02/16/my-llm-codegen-workflow-atm/" target="_blank">Harper Reed's blog post</a> on their codegen workflow. I ask Claude to ask me questions, one at a time, building on each previous answer. This continues until all unknowns are accounted for. These questions fill requirements in ways we might have overlooked or assumed were already answered.

## Machines Don't Infer

Working with AI is fundamentally different from working with humans in a lot of ways. For one, form the start they have no idea what you are talking about. When you talk to another developer, they infer meaning from context, tone, and shared experience. They know your usual patterns and what project we are talking about without naming the project.

LLMs don't have this memory or skill. Every new feature requires covering basic details again and again. That's where `claude.md` files come in handy. Generally, the machine has no memory of your architecture decisions, your team's conventions, or what worked on the last project. Having AI ask questions about your software plan forces you to see the project from a different perspective. Details that seemed obvious get examined. Assumptions get surfaced. Parts of the project that might have been overlooked get attention.

This mirrors what we've seen with [technical debt](/Blog/technical-debt-is-a-business-decision)—the assumptions you don't examine are the ones that cost you later.

## The Summary Loop

After the questions are answered, we have Claude repeat back a summary and project overview. This is rarely right on the first try.

The back-and-forth continues until the project description matches our actual requirements. It's a validation step that catches misunderstandings before they become code.

This iterative refinement is similar to the [build vs. buy analysis process](/Blog/build-vs-buy-2025)—the first answer is never the final answer. Requirements need pressure testing.

## The Blueprint: claude.md

Once the requirements are solid, we ask Claude to create a `claude.md` file. This breaks down the application features into consumable chunks—essentially a technical specification that Claude will use as its blueprint.

The file becomes the contract between us and the AI. It defines scope, features, and acceptance criteria in a format that both humans and AI can reference throughout the build.

This step matters because it creates accountability. Without a defined scope, AI will generate code that technically works but misses the point. With a clear blueprint, every generated component can be checked against explicit requirements.

## Human in the Loop

After signing off on the checklist, we have Claude build the software line item by line item. Each step gets reviewed before moving forward.

This isn't about distrust. It's about catching drift early. AI can be confident and wrong. Small misunderstandings in early steps compound into major rework later—the same pattern we see in [projects that need rescue](/Blog/five-signs-project-needs-rescue).

The human review at each step ensures what gets built matches what we asked for. When it doesn't, we correct course immediately rather than discovering the problem at integration.

## What This Means for Clients

AI engineering delivers faster iterations with better requirements coverage. The questioning process surfaces issues that would otherwise become expensive change requests. The blueprint creates clarity that traditional specification documents often lack.

But the core value hasn't changed. Software still needs experienced engineers who understand business problems, make architectural decisions, and [manage technical debt](/Blog/managing-technical-debt) appropriately. AI accelerates the work. It doesn't replace the judgment.

The clients who benefit most are those with complex requirements and tight timelines—projects where traditional development struggles to move fast without cutting corners.

---