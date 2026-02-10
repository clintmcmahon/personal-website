---
title: "How do delete all local git branches except main and develop"
description: "Over time my git branches on my local machine get out of control. Here's the command to delete all your local git branches except main and develop."
date: "2026-02-10"
draft: false
slug: "delete-all-local-branches"
tags: git
---

<section>

Over time my git branches start to get out of control on my local machine. I'm using Azure DevOps primarily right now for work and each pull request has a `delete branch` option after the merge is successful. This helps keep our remote repos clean, but not so locally. There isn't an automatic branch delete or clean up that runs locally on my machine so I end up with a lot of orphaned branches using this flow. To clean up all the left over git feature branches while leaving `main` and `develop` alone, I run this git command:

<pre><code class="language-git">
git branch | grep -vE "^\*|main|develop" | xargs git branch -d
</code></pre>

To add more branches to this list just add `|branchName` after `develop`. To force a delete on an unmerged branch, just replace -d for -D at the end of teh above command.
</section>