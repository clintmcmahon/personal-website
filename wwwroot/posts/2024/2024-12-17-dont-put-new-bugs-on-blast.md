---
title: "Don't Put New Bugs On Blast"
description: "Bugs and problems in software are inevitable, but how we communicate them to the team determines the outcome and speed they are resolved."
date: "2024-12-17"
draft: false
slug: "dont-put-new-bugs-on-blast"
tags:
---

 <section>
   <p>
      Bugs and system issues are inevitable in software, but how you communicate these problems can determine whether the team resolves the issue efficiently or gets thrown into unnecessary chaos. Blasting a bug to the entire team Slack group causes panic, disrupts the workflow on fixing the issue, and slows down progress. Instead of tossing a grenade into an email thread or Slack channel, a focused, calm, and systematic approach can contain the situation, address the problem effectively, and reduce stress for everyone.
   </p>
   <p>
      Here is an example of the best way to communicate and handle new bugs in a production environment.    
   </p>
</section>
<section>
   <h2>1. Don't Put the Issue on Blast</h2>
   <p>If you discover a bug in a system or website, the first thing to do is stop and take a moment to think about:</p>
   <ul>
      <li>What the issue is.</li>
      <li>What you're observing.</li>
      <li>Write down a clear and concise summary of the problem and the steps taken to reproduce the bug.</li>
   </ul>
   <p>Avoid immediately alerting everyone in the Slack channel or sending off a mass email. Doing so floods communication channels, creates panic, and makes it harder for the develoers who are in charge of fixing the bug to focus on the fix.</p>
</section>
<section>
   <h2>2. Find the Right Person to Alert</h2>
   <p>Once you've gathered a clear understanding of the issue and provided the steps to reproduce:</p>
   <ul>
      <li>Report the problem to the most experienced technical team member or senior developer.</li>
      <li>
         Be ready to explain:
         <ul>
            <li>How you discovered the issue.</li>
            <li>Steps to replicate the problem.</li>
            <li>A little summary or any other information that you think can be helpful in solving the problem.</li>
         </ul>
      </li>
   </ul>
   <p>This gives the developer or tech lead the ability to assess the situation in a more peaceful enviornment. They're already going to be a little stressed knowing there's a bug in production, but having the calm of not being bombarded with messages only helps the situation. Now they may be able to stop the issue from escalating further - stop the bleeding, as some might say or create a more informed summary for stakeholders before it becomes a panic situation.</p>
</section>
<section>
   <h2>3. Why This Approach Works</h2>
   <p>If you blast the problem to the entire team immediately, here's what happens:</p>
   <ul>
      <li>Flooded communication: The people responsible for fixing the issue will be inundated with messages asking, "What's happening?" or "How bad is it?" or "Why did this happen?". This disrupts focus and slows down resolution. Instead of fixing the problem or at least understanding the problem, you are now in communication mode vs fixing mode.</li>
      <li>Confusion and panic: Sometimes people overreact if they don't understand a problem, escalate unnecessarily, or duplicate efforts.</li>
   </ul>
   <p>By alerting the senior developer or tech staff member first, you're give them a chance to understand the issue and allows them to communicate clearly and calmly with the broader team after they've fully understood the issue. This in turn reduces unnecessary distractions and stress for the entire team.</p>
</section>
<section>
   <h2>4. The Fixing Process Becomes Smoother</h2>
   <p>By following this method, fixing issues is a lot eaiser because the developers don't waste time answering repeated messages about what's wrong. Because the developers can focus on the issue, they're able to effectivaly and clearly explain the situation to the stakeholders once the problem is understood.</p>
   <p>Fixing a bug in production is much easier when developers don't have to "get up to speed" amidst a flood of panicked messages. 
</section>
<section>
   <h2>5. Communicate Calmly When Escalating</h2>
   <p>As soon as there's an understanding of the problem, choose the right moment to inform the broader team or stakeholders. The communication should:</p>
   <ul>
      <li>Be clear and calm.</li>
      <li>Include the impact and a plan for resolution.</li>
      <li>Avoid saying things like "Everything is broken!!!". That will not help the situation.</li>
   </ul>
   <p>Here's an example:</p>
   <blockquote>
      <p>"We've identified an issue with user authentication affecting Service X. Gilfoil is working on it, and we'll provide an update in 15 minutes. Please avoid making related changes until the issue is resolved."</p>
   </blockquote>
   <p>This structured approach builds confidence, minimizes disruption, and allows developers to fix the problem efficiently without juggling questions or panic.</p>
</section>
<section>
   <h2>6. Lessons Learned</h2>
   <p>After the issue is resolved, conduct a quick retrospective:</p>
   <ul>
      <li>What went well in how the issue was communicated and handled?</li>
      <li>What could be improved?</li>
      <li>Can we implement tools or processes to catch this issue earlier next time?</li>
   </ul>

   <p>You are never going to be able to have a bug free system because problems are inevitable, but how they are communicated determines the outcome. The next time you find a bug or issue:</p>
   <ol>
      <li>Pause and assess the problem.</li>
      <li>Alert a senior developer first before hitting up the all Slack channel.</li>
      <li>Allow them to get a handle on the situation before broadcasting to the entire team.</li>
   </ol>
   <p>By taking this more chill and thoughtful approach, you'll reduce panic and support faster problem resolution which helps build a more efficient and confident team.</p>
</section>

