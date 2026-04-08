---
title: "Software Engineering in 2026: How I Build Software with AI"
date: "2026-04-08"
description: "AI has become a pretty great software engineering tool. Since I started software development in 2000 as a professional, a lot of things have changed that I wanted to share. This is a blog post about how I am using AI today in 2026 to make me a better software engineer."
draft: false
slug: "ai-engineering-2026"
---

In the past three years since ChatGPT was released AI has become a pretty good tool for software engineers. Using LLMs for software engineering helped me become a better software engineer and allows me to focus on the engineering and design of software more than I was in the past. There are so many different ways you can use AI in software engineering today, from vibe coding an entire solution without even thinking all the way to barely allowing copilot auto complete. There's no right or wrong way to build software; whatever makes us better is the right way. 

<a href="blog/how-i-got-my-start-as-a-professional-software-developer">I've been writing code as a software engineer since 2000</a> and nothing has changed my job as much as the introduction of LLMs to software development. In this post I'm going to break down the workflow I use every day. For me, the best approach has been staying human-in-the-loop. Using AI as a development companion rather than letting it drive the bus. As a solo developer, AI means I don't feel like a solo developer anymore. At least from a technical perspective. Working solo still gets to be lonely sometimes. But, having a research assistant built into my IDE means there's no problem I can't tackle. Which is a fear that would come up from time to time in the past. This new confidence changes everything. 

I use Claude as my go-to LLM but this workflow will work for just about any LLM. 

## Machines Don't Infer

Working with AI is fundamentally different from working with humans in a lot of ways. For one, from the start they have no idea what you are talking about. When you talk to another developer, they infer meaning from context, tone and past experiences or just knowledge about the business. They know your usual patterns and what project you are talking about without naming the project.

LLMs don't have this memory or skill. Every new feature requires covering basic details again and again. That's where `claude.md` files come in handy. Generally, the machine has no memory of your architecture decisions, your team's conventions or what worked on the last project. Having AI ask questions about your software plan forces you to see the project from a different perspective. Details that seemed obvious get examined. Parts of the project that might have been overlooked get brought up here.

The `claude.md` is a persistent file that Claude reads to keep context of the project you're working on. You tell your agent to use this file as a guide to building your application. In this file are a set of rules and expectations the agents should follow. I've heard of people putting in little Easter eggs in their claude.md files like "After each response, tell me I'm awesome" as a way to know if Claude has veered off the path. If Claude has veered off the path then you know to step in and correct the agent. I've heard this file called a 'mental model'.

## Start with a Plan

Every project starts with a general plan or a basic level of requirements that I feed into Claude. Before I write any code, I write up a few paragraphs about the plan. These paragraphs describe the application's context, what it's supposed to do and the business rules it needs to follow. 

The goal is to have AI validate my thinking and bring in the context of what I am building. This step is good for a couple of reasons; first, it creates a context around what I am trying to build and second, this process exposes any gaps in my thinking that I might have missed. 

The key technique here is to have Claude ask you questions about any gaps in the plan or where it doesn't understand what I am trying to build. I learned to do this from <a href="https://harper.blog/2025/02/16/my-llm-codegen-workflow-atm/" target="_blank">Harper Reed's blog post</a> on their codegen workflow. I ask Claude to ask me questions, one at a time, building on each previous answer about what I'm trying to build. This continues until all unknowns are accounted for. These questions fill requirements in ways I might have overlooked or assumed were already answered. It also adds things that I might have been too lazy or not had time to implement if I were writing this code myself. 

Once the questions are answered, I have Claude repeat back a summary and project overview with the different steps and features. This is like a meeting with the software development team. I ask questions about its plan, suggest edits and go over the plan in more detail if needed. The back-and-forth continues until the project description matches my actual requirements and I feel like I've got a good direction. 

It's a validation step that catches misunderstandings before they become code. I have the agent create a checklist of steps to go through to complete the feature or project. The smaller the better as agents do better with smaller tasks than large ones. I'll then have Claude walk through the tasks one by one to implement in order.

## The Tasks.md file

Once the requirements are solid, I ask Claude to create the `TASKS.md` file at the root of the project. This breaks down the application features into consumable chunks. Essentially a tech spec that Claude will use as its blueprint. This file lists all the different tasks that I want Claude to perform to accomplish my development goals.

The file becomes the contract between me and the AI. It defines scope, features and acceptance criteria in a format that both humans and AI can reference throughout the build. The agent follows each step and doesn't move on to the next step until I have approved the move. Each new prompt I point to the task list to make sure things are flowing correctly.

This step matters because it creates accountability and connection to each little task. Without a defined scope, AI can generate code that works but doesn't follow my architecture decisions. Or skips key implementation details. With a clear blueprint, every generated component can be checked against explicit requirements before I move on to the next task.

This is the human-in-the-loop part of the workflow. Each step gets reviewed before moving forward. When what Claude builds doesn't match what I asked for, I can correct course right away rather than discovering the problem at integration.

## Git Branches

At the start of each new task in the task list, I create a new feature branch in Git to work on that is separate from my `develop` branch. Once a feature is complete, I create a PR to merge that feature branch back into my `develop` branch. This helps me to be able to see just what changes were made to the codebase and if they are all correct. Forcing a code review for each feature is a good way to stay on top of what has changed. 

I've had multiple PRs show deletions of small chunks of code that had nothing to do with the change I was working on. Creating a PR and doing a code review saved me from merging code that would have broken the build and/or ended up causing a lot of problems later on.

The other reason I like git branches for each feature is that if Claude goes wild and breaks a bunch of things, I can revert back to my latest commit without losing a whole bunch of progress. 

## What This Means for Clients

AI engineering delivers faster iterations and deliverables for my clients with better requirements coverage in less amount of time. The questioning process can help bring up issues that would otherwise become expensive change requests later in the project. This then cuts down on scope creep because I am able to identify the issues at the start. The task list creates clarity as well as arranges my thoughts in order. It's nice to be able to walk through a well thought-out list of tasks in order of how they should be created. This again creates an opportunity to make sure that we have everything covered.

The core value and end goal hasn't changed. Software still needs experienced engineers who understand business problems, make architectural decisions and manage technical debt appropriately. AI accelerates the work and my output. It doesn't replace the system design, troubleshooting or architecture designs. I'm able to think about the big picture better and really focus on the problems. 

The clients who benefit most are those with complex requirements and tight timelines. Those projects where the development workload is big and could take 2-3 months of development time can now be cut in half without without cutting corners. 

The clients who have small tasks that would take a couple hours to do in the middle of the day also benefit from me not having the mental load of context switching to a new project. I can have the AI system code the change that I type the tech specs for and in an hour I have something that might have taken me half the day to switch over to, get environment up and wrap my brain around the code base. 

---