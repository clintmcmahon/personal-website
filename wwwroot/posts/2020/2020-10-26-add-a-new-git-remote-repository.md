---
title: "Add A New Git Remote Repository"
description: ""
date: "2020-10-26"
draft: false
slug: "add-a-new-git-remote-repository"
tags:
---

<!--kg-card-begin: html--><p>Working with different clients I often end up with multiple Git remote repositories. When I start a new project with a client I create the initial repository in my <a href="https://gitlab.com" target="_blank" rel="noopener noreferrer">Gitlab</a> called <code>origin</code> and if the time comes I will add another remote repository in the client environment with the name of the client <code>clientABC.</code> This is how to add a new Git remote repository in addition to your default Git repository.</p>
<p>Add the new remote repository using the git remote add command.<br />
<code>git remote add clientABC https://gitlab.com/clientABC/exampl-repo.git</code></p>
<p>Then when pushing or pulling from the different remote repositories make sure to include the appropriate remote name when running the commands. For example, when pulling from origin dev branch use <code>git pull origin dev</code>. And if you want to push your changes up to the new client remote use <code>git push clientABC dev</code>.</p>
<!--kg-card-end: html-->
