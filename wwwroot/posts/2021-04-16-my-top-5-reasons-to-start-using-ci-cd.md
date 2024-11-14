---
title: "My Top 5 Reasons To Start Using CI/CD"
description: ""
date: "2021-04-16"
draft: false
slug: "my-top-5-reasons-to-start-using-ci-cd"
tags:
---

<!--kg-card-begin: html--><p>For the first half of my career we moved code from our local machines -&gt; dev -&gt; test -&gt; production the old cowboy way, just by copy &amp; paste from one environment to the next. It&#8217;s quick and easy but also riddled with holes. Now we&#8217;ve got better ways of doing things with processes like <a href="https://en.wikipedia.org/wiki/CI/CD" target="_blank" rel="noopener">CI/CD</a> and source control systems like <a href="https://docs.gitlab.com/ee/ci/" target="_blank" rel="noopener">Gitlab</a> and <a href="https://docs.github.com/en/actions/guides/about-continuous-integration" target="_blank" rel="noopener">Github</a>. Here are my top five reasons why you should have a CI/CD process or pipeline set up into your development workflow:</p>
<ol>
<li>Each commit can be traced to a specific environment allowing developers to see what code is in production or test at any given time.</li>
<li>If you&#8217;re pasting files around, you can easily blow away or overwrite something you didn&#8217;t mean to.</li>
<li>CI/CD pipelines let you can roll back commits and changes with the click of a button if you break something. Thus keeping you from scrambling to find that backup folder you created (or didn&#8217;t) before moving the code to production.</li>
<li>Check in your code and forget. CI/CD pipelines will automatically move your changes to your environments for you.</li>
<li>Pipelines will run unit and integration tests before deploying code. If the tests fail the migration is halted so breaking changes never make it to production.</li>
</ol>
<p>This <a href="https://docs.gitlab.com/ee/ci/quick_start/" target="_blank" rel="noopener">getting started guide for Gitlab</a> is a good place to start with CI/CD.</p>
<!--kg-card-end: html-->
