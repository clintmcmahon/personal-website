---
title: "Manage Azure AD B2C users dashboard"
description: ""
date: "2023-06-19"
draft: false
slug: "manage-azure-ad-b2c-users-dashboard-2"
tags:
---

<p><em>I've built an </em><a href="https://github.com/clintmcmahon/azure-ad-b2c-user-manager" rel="noreferrer"><em>entire Azure AD B2C User Management solution</em></a><em> that is available for free on Github. The code samples below are part of this larger solution. Feel free to use this application as you see fit. </em><a href="/contact" rel="noreferrer"><em>Contact me</em></a><em> if you run into any issues or need help consulting on your Azure AD B2C project.</em></p>
<!--kg-card-begin: html-->
<p>I&#8217;ve gotten a few emails from <a href="http://clintmcmahon.com/add-role-claims-to-an-azure-b2c-user-flow-access-token">my blog post</a> on using Postman to query Azure B2C users extension properties using Microsoft&#8217;s GraphAPI. The documentation is light but I&#8217;ve implemented this strategy over and over again so I wrote that blog post to help other people out. </p>
<p>Now, I want to create a dashboard to allows administrative users to ability to manage their B2C properties of other users in their tenants. Currently, user properties need to be managed through the Azure Portal. The Azure Portal feels clunky and not very user friendly. There is also no way for administrators of a tenant to manage the extension properties of a user through the Azure Portal. All extension properties of a <a href="http://clintmcmahon.com/add-role-claims-to-an-azure-b2c-user-flow-access-token/">user must be managed via the Microsoft Graph API</a>.</p>
<h2 id="problems-this-application-will-solve">Problems this application will solve</h2>
<p>There are a couple problems the initial release of the application will aim to solve.</p>
<ol>
<li>There is currently no way to add/remove/update extensions of B2C users without <a href="http://clintmcmahon.com/how-to-use-graph-api-to-query-azure-ad-b2c-users-using-postman/">using the Graph API</a>. This application will be a wrapper around the Graph API integration to give administrative users a nice and user friendly interface into Azure B2C users management.</li>
<li>No way to add roles and features to users without logging directly into Azure. </li>
<li>Cannot add users easily. </li>
</ol>
<h2 id="hard-to-fix-problems">Hard to fix problems</h2>
<p>Some of the problems I might run into while developing this software:</p>
<ol>
<li>The Graph API can be set up as a separate App Registration in the Azure Portal. Then using the Graph API I would connect using the generated client id and client secret. How do you save the client id and client secret securely? And will customers/users trust my application with their client id and secret &#8211; this application will need to have Read/Write access to their user store.</li>
<li>Can I create an OAuth connection to the existing Azure B2C environment where the user already has an account. Instead of connecting as the application, can I simply connect the authenticated user via Azure B2C OAuth?</li>
<li>What type of application is this going to be? How do you distribute this application? Can this be developed as a SaaS application or do I sell the compiled application along with a license?</li>
</ol>

<!--kg-card-end: html-->
