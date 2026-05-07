---
title: "I Still Can't Trust AI"
date: "2026-04-28"
description: "AI gave me two different brew recipes for the same coffee in two separate chats. When I pushed back it changed its answer — not because I was right, but because I pushed. That's why I can't trust AI."
draft: false
slug: "i-still-cant-trust-ai"
---

This is a post about why I still can't trust AI or LLMs specifically. I use it every day for various things like brewing coffee and software engineering. But for this post I'm going to use coffee as the example. 

I have a coffee subscription where I get a new coffee every week. To minimize the dialing in of new coffees, I started using AI to help. I have a Claude project with my fixed parameters that give context to what I'm working with: a Chemex pour over, Baratza grinder and a Fellow gooseneck kettle. All I do is take a picture of the bag and ask Claude how I should brew it. It spits out the grind size, water temperature and pour timing relative to my fixed parameters. I then record the results for that roast in a coffee journal. This works reasonably well. When a cup comes out bitter, I'll make a note of the settings, ask Claude what needs to change and try again. Usually the adjustments make the coffee better.

Sometimes I'll ask Claude how I should brew a coffee that I've already brewed just to see if the suggestion matches what I have in my notebook. Sometimes what the machine says matches what I've written down. Other times, it's completely different. When I point this out to Claude, it responds that I'm right(!) and we should go with what I said first. So which version am I supposed to trust here? The first version or the version it gave me after I raised the question? How do I know which is the right answer?

Here's another example. I have an app called <a href="https://mncoffeemap.com" target="_blank">MN Coffee Map</a>. The app uses a list of coffee shops in Minnesota to display in a Mapbox map. I exported the list of shops, uploaded it to Claude and told it to create a list of the top ten coffee shops based on their Google rating (which was part of the data). It did the work and spit out a list of ten coffee shops. But, I noticed that there was a coffee shop that I have never heard of in the list named "Brewbird". Brewbird wasn't in my list of coffee shops that I uploaded. When I asked Claude why there was a coffee shop named Brewbird it said "You're right — I added it and I filled in a #10 that wasn't in your data. I'll remove it now." This is a great example of LLM hallucination.

This is why I can't trust LLMs. If the answer is not correct or is made up, there's no way you can trust the results for any real work without fact-checking it. If the answer was correct and I question it, the system will give me a different answer that isn't correct (or maybe it is?). All to make me happy. If the answer is wrong and I question it, hopefully AI will tell me the correct answer. Then at other times it will just generate random data without telling me that it generated the random data.

There are a lot of people using AI who aren't subject matter experts in what they're asking about. Doctors and lawyers are using AI because they don't know the answer and want AI to tell them the correct answer. Those are the people taking the first answer. And the system sounds confident either way.

AI systems (LLMs specifically) are a work in progress and they are getting better. But they are not yet 100% accurate and maybe they never will be. Right now, there has to be someone who already knows enough to catch the mistake and question the results. Otherwise we're heading in the wrong direction if we blindly accept LLM responses. I'm ready to go all in with some AI projects, but because I can't trust the output and don't have time to comb over the results to make sure they are correct - I can't go all in on the technology. I still use it for coding but I'm a pretty good developer so I can perform code reviews to make sure the results are what I want.

But if I don't know the subject well or at all - how am I supposed to trust that AI gave me the correct answer. Especially if my weak pushback is enough to change AI's original answer? My examples above are just some basic coffee use cases. Should we be blindly trusting LLMs to analyze legal documents or summarize patient data without oversight? No, we shouldn't. If the gains we get from the LLM are blown away by the time used to fact check the results, are they really saving all that much time in situations where the data can't be wrong?
