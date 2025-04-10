---
title: "How to manually deploy a .Net WebApi project"
description: "This blog post is about how to manually deploy a .Net WebApi project to a live server without using an CI/CD."
date: "2025-04-10"
draft: false
slug: "manually-deploy-dotnet-webapi"
tags: dotnet
---

I don't always have access to a <a href="/blog/my-top-5-reasons-to-start-using-ci-cd">CI/CD pipelines when deploying new code changes</a>. In those cases I deploy code changes the old cowboy way - via copy and paste. This blog post shows how I publish .Net WebApi projects to a web server without using any CI/CD tools. 

1. Execute the dotnet publish command using the dotnet CLI
    - <pre lang="shell" class="language-shell" tabindex="0"><code class="language-shell">dotnet publish YourApiProject.csproj -c Release</code></pre>
2. This will publish the runtime files to ```bin/release/publish```
3. Navigate to the newly published folder (```bin/release/publish```) and copy the entire contents of the folder. If the code has already been deployed and this is not the first time you are publishing the application, only copy the compiled code files leaving the web.config and appsettings.json files alone. Since the web.config and appSettings files usually contain sensative and/or environment specific settings, leave those and make any necessary updates to those files manually on the web server. 
4. Open the web server file location and paste the copied files into the directory. If this is the first time you are copying the files then you are good. If this is an update then you will most likely need to stop IIS or reset the application in order to overwrite the existing files. Trying to paste the modified files without resetting the website will result in a ```Access Denied``` error due to the files being locked. 
    - "Touch" the web.config file by opening it in a text editor and make a small change. I usually just create a new line at the bottom of the page then click save. This will reset the web application and unlock the application files. 
    - Now you are able delete all the compiled files from this directory. Leave the web.config and appsettings.json files alone as well as any other folder/files that you did not copy from your publish folder.
5. If there are any appSettings changes I make those now. After making any appSettings you need to touch the web.config file again. After resetting the application through an ```iisreset``` or changing the web.config any update to the appSettings will be loaded into the application.

That's it. Having a working <a href="/blog/lessons-learned-in-cid-permissions">CI/CD pipeline is the best (and safest) way to deploy changes</a> but that's not always possible. Therefore whenever I have to manually deploy changes this is what I do. The same process can be done for other applications like Angular and React apps. 