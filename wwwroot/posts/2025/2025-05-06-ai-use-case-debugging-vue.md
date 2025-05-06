---
title: "AI Use Case: Debugging Vue + Quasar UI Code with ChatGPT"
description: "I use AI in my day-to-day life as a developer and human. One way that I use AI as a developer is to understand code bases that I'm not familiar with."
date: "2025-05-06"
draft: false
slug: "ai-use-case-debugging-vue"
tags: [consulting, ai]
---

ChatGPT is a great tool to analyze code that I don't understand. I'm working on a backend .Net Core WebAPI that intergrates with a Vue component that is written with Quasar UI. I wanted to know why the ```costModifier``` variable of the ```Question``` object wasn't being set by the UI before the object payload was passed to the API I'm working on. Instead of Googleing how to set up and run a Vue/Quasar debugger - because I don't know anything about those two frameworks - I decided to shortcut the process and use ChatGPT. I copied the whole Vue file and posted it to ChatGPT with the simple prompt: **"Do you understand Vue?”**

ChatGPT analyzed the file then provided me with the summary, core features and an exmplanation of the main data structure from the file. In a minute I fully understood what this component was doing and capable of. 

My next prompt was: **"Is there a way to update costModifier for the question object?"**. ChatGPT responded by saying **Yes — you can update the costModifier for a question object via the questionForm.costModifier field inside the Question modal. Here's how it works:"** along with a javascript and Quasar code blocks showing exaclty how this is done in the component. In it's response was one special line that caught my attention. It was **"Inside the modal, if questionForm.questionType === 'bool', the Cost Modifier field is shown"**. This caught my attention because the bug I'm working with had a qustionType of 'select', not 'bool'.

Right here I can see why my variable was not being set and could return to the front end folks with some information. I wanted to double check so I asked ChatGPT: **"If the question type is something other than bool, will the costModifier variable be set?"**. The response was the same **"No — the costModifier input field is only displayed if the question type is 'bool'."**. Along with the correct answer, ChatGPT showed me exaclty where in the code this was being set and how I could update the code to allow for 'select' to allow for the variable to be modified.

Double-check the logic.

From there I was able to articulate to the team that handles this code as to the issue and provide a path to the solution.

Without AI assistance, this bug could have taken a while to troubleshoot. Not that I don’t want to learn Vue or Quasar, but I would rather have this fixed sooner than later for the client’s sake. This is just one of the many benefits I see from ChatGPT as a developer and consultant. You can use its computing ability to summarize a code base so you can quickly get up to speed. Saves me time and <a href="/portfolio">my clients</a> money.