---
title: "AWS Amplify removed video sources from HTML"
description: ""
date: "2021-03-02"
draft: false
slug: "aws-amplify-removed-video-sources-from-html"
tags:
---

<!--kg-card-begin: html--><p>A good amount the websites I&#8217;ve pushed to production in the past couple of years have all been hosted on-prem within my client&#8217;s data centers. However, not all of my clients have test environments. Up until recently I&#8217;ve been hosting my test sites with <a href="https://www.server4you.com/" target="_blank" rel="noopener">Server4You</a>, a fantastic non AWS/Azure/GCP hosting company/provider. But with the rise of AWS and Azure I want to start venturing over that way to gain experience and play around with those providers some more.</p>
<p>I&#8217;m starting with a front end only React app that I&#8217;ve developed for a client which will ultimately live in their data center when the site goes live, but to test while we&#8217;re building the site I decided to give <a href="https://aws.amazon.com/amplify/" target="_blank" rel="noopener">AWS Amplify</a> a try. This service is awesome and hooks right into Gitlab for automated CI/CD with a couple button clicks. Awesome as it is, I did run into one problem: My video src references were stripped out during the CI/CD process.</p>
<p>The site has three HTML5 video references to mp4s that are hosted within the build folder at the root of the web project. When the AWS Amplify CI/CD process migrated my code over to the hosting web server all of my src references were blank &#8211; <code class="EnlighterJSRAW" data-enlighter-language="html">&lt;video src="" /&gt;</code>.. I tried referencing the videos in a couple different ways from the React app but nothing seemed to work.</p>
<p>To get the video links to work in AWS Amplify, I ended up creating a S3 storage account, uploading my videos to a <a href="https://aws.amazon.com/getting-started/hands-on/backup-files-to-amazon-s3/" target="_blank" rel="noopener">S3 bucket</a> , <a href="https://docs.aws.amazon.com/AmazonS3/latest/dev/WebsiteAccessPermissionsReqd.html" target="_blank" rel="noopener">assigned the appropriate permissions</a> then referencing the videos in code to pull from the S3 bucket which ended up fixing the problem.</p>
<p>I haven&#8217;t been able to find any documentation that explicitly says that video sources are removed but hosting the videos in S3 fixed the issue for me.</p>
<!--kg-card-end: html-->
