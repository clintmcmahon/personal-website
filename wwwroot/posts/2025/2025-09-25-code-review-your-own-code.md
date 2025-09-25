---
title: "Code Review Your Own Code"
description: "LLMs speed us up, but they don’t replace the basics. Always open a PR and review your own code, even if the bulk of it came from an agent."
date: "2025-09-25"
draft: false
slug: "code-review-your-own-code"
tags: [development]
---

<section>
    <p>
        Whenever I'm developing something alongside a LLM in agent mode, I always commit code frequently. When the code does what I intended it to do, I commit the change to my feature branch. This ensures that in the future - when the agent inevitably destroys what we've done together - I can easily roll back. This week I was reminded of how important it is to apply the same rigorous code review process to AI-assisted development that we use for human-written code - creating a PR and doing a code review of everything before merging back into the trunk.
    </p>
    <p>
        <img src="/images/2025/volodymyr-dobrovolskyy-KrYbarbAx5s-unsplash.jpg" class="w-100 m-4" alt="Cat looking at a monitor with code">
    </p>
    <p>
        The reason for this is simple - to make sure what I think I changed is actually what was changed. This week I was working on an update to a class that was getting a little spaghetti-like. I had Copilot create a simple C# class based on a list of input parameters of a method. Instead of 10 separate strings, make these strings a class and accept the new object as a single input parameter. Easy and simple.
    </p>
    <p>
        Copilot created the class like I wanted and updated the method. But it also removed an entire if statement in an unrelated part of the code. Sure, it did what I asked, but for whatever reason, it thought that this random if statement wasn't needed, so it deleted it. I caught this error later when I was doing a pull request of the feature branch back into the main branch of my repository.
    </p>
    <p>
       LLMs speed us up for sure (sometimes), but they don’t replace the basics and the need to double check our work. I recommend to anyone developing along side an agent to always open a PR and review your own code, especially if the bulk of it came from an agent. AI will happily generate something that compiles and runs and solves your problem, but that doesn’t mean something else was updated as well. It can slip in subtle bugs, remove functionality, create security holes or introduce patterns that don’t line up with how we structure things. If you just drop it straight into main, you’re skipping the one step that gives you a second look at what’s really happening.
    </p>
    <p>
       At times code reviews feel like overhead, but they're a necessary safeguard against unwanted changes making it into your codebase. Doing code reviews like this makes you slow down, re-read the code, and verify it matches your standards before it becomes part of the system.
    </p>
    <p>
        In the end, AI isn’t accountable for this code, we are. So treat your agent like a junior developer and check their work. Create PRs and code reviews to look over the code that's written. Doing this is great for a whole bunch of reasons, here are my two biggest ones: <ul><li>1) You get to read the code and understand what's happening leading to less AI slop. </li><li>2) This double check is great for finding changes to the code that were not intended. Allowing you the time to roll back or reconfigure what was changed before something worse happens down the line.</li></ul> 
    </p>
    <p>
        Let’s keep our process tight and the bar high, kids.
    </p>

    
</section>