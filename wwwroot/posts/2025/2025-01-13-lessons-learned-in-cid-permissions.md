---
title: "I Learned a Lesson in CI/CD Permissions"
description: "I completely obliterated user privileges on a client folder while trying to implement CI/CD using Azure DevOps. My goal was to set up an Angular front-end build and deploy pipeline but instead I bricked the site."
date: "2025-01-13"
draft: false
slug: "lessons-learned-in-cid-permissions"
tags:
---

 <section>
    <p>
        Recently, I completely obliterated user privileges on a client folder while trying to implement CI/CD using <a href="/services">Azure DevOps</a>. My goal was to set up an Angular front-end build and deploy pipeline using an existing agent, so it's a pretty simple change since there wasn’t a need to set up a new agent.
    </p>
    <p>
        The build process worked just about out of the box so I just needed to configure the deploy process. I configured the deployment to target a new test folder that was inside the same parent folder as the existing website folder. Then I created a share of the existing parent folder which allows the agent to directly access the deployment folder without requiring access to the entire directory chain.
    </p>
    <p>
        What I failed to consider was that by creating the share on the parent folder, all existing permissions for the subfolders were blown away, despite my explicit instruction to Windows not to overwrite them. Unfortunately, when I clicked 'Create Share' all the existing privileges were wiped out anyway.
    </p>
    <p>
       This misstep created a chain reaction of 500 errors across the entire site, leaving users unable to access the website and thus sending <a href="/blog/dont-put-new-bugs-on-blast">messages to me</a>. Now the deployment agent’s service account was now the only account with access to the subfolders in the newly shared directory.
    </p>
    <p>
        As soon as the site went down, I started receiving messages about the issue so I quickly removed the share and restored the previous permissions. The website went back online for everyone right away.
    </p>
    <p>
      To avoid a repeat of this incident, I’ve created a new test folder entirely outside of the active website's directory structure - something I should have done from the start. I will continue testing within this isolated folder. Once everything is verified to work as expected, I plan to make the share changes to the live website folder over the weekend when any disruption will have minimal impact on users.
    </p>
    <h2>The Takeaway</h2>
     <p>
      This just reinforced the importance of testing in an isolated environment which is what I'll be doing going forward - like a professional.
    </p>
</section>
