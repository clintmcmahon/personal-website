---
title: "That email address contains five or more consonants in a row."
description: "I'm starting a newsletter for this blog and heard good things about Buttondown. But then I got this error and couldn't sign up."
date: "2025-11-04"
draft: false
slug: "email-address-contains-five-or-more-consonants"
tags: programming
---

Looking around for an email newsletter provider, I came across <a href="https://buttondown.com/" target="_blank">Buttondown</a>. It looks great, and people online seem to like it, too. I decided to create an account and give it a spin. However, when I signed up for an account, I got an error saying that my <strong>email address contains five or more consonants in a row</strong>. <pre><code class="language-text">Email address contains five or more consonants in a row.</pre></code>

<img src="/images/2025/buttondown_error.png" class="w-100 pb-4" />

This is a new one. I can see why this works to stop random spam accounts that are just a jumble of letters together, but I haven't seen it used anywhere on the web before. I assume it's using some type of regex pattern to identify the string sequence. <a href="https://blog.codinghorror.com/regular-expressions-now-you-have-two-problems/" target="_blank">Now they've got two problems</a>. 

The feature's built to stop spammers from signing up for the service, but in this case, it's also stopping valid users from signing up. I've used this version of my email address for my entire online life and have never had this happen before. Googling this error didn't return any specifics that I could find, so it's not a widely used anti-spam pattern.

I really want to try Buttondown so I reached out to their support team to see what they can do. I'll update this post when I hear back from them.

## What does ChatGPT think

As a side note, when I first got this error, I asked ChatGPT about it. ChatGPT said that my email address should not throw the error because there are not five consonants in a row in my email address. 

Here is the original response:
<img src="/images/2025/chatgpt_consonants.png" class="w-100 border p-2 m-3" />

That response is wrong, as there are five consonants in a row in my email address cli<strong>ntmcm</strong>ahon@pm.me. I've seen cases where LLMs have skipped over a second instance of a letter that previously showed up in a string. Maybe that is what's happening here. 

I ran this by ChatGPT to get the following (correct) response:
<img src="/images/2025/chatgpt_consonants2.png" class="w-100 border p-2 m-3" />
Nothing changed except my challenging the LLM's answer. LLMs are not perfect, and they are great at a lot of things. But you can't trust everything they say, and that's a huge drawback for me when using them in production environments.

Here lies one of the major reasons I don't think LLMs are ready for straight blind production usage like so many people are out there pitching to clients. They are "smart" and definitely useful, but I don't think they are ready to be allowed to make their own decisions without a human in the loop - not yet anyway. The human still has to verify that the responses are correct and adjust when necessary.


