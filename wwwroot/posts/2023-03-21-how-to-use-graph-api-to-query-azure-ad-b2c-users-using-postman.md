---
title: "How to use Graph API to query Azure AD B2C users using Postman"
description: "In this blog post I show you how to use Graph API to query Azure AD B2C users using Postman and return a list of users in your B2C tenant."
date: "2023-03-21"
draft: false
slug: "how-to-use-graph-api-to-query-azure-ad-b2c-users-using-postman"
tags:
---

<p><em>I've built an </em><a href="https://github.com/clintmcmahon/azure-ad-b2c-user-manager" rel="noreferrer"><em>entire Azure AD B2C User Management solution</em></a><em> that is available for free on Github. The code samples below are part of this larger solution. Feel free to use this application as you see fit. </em><a href="/contact" rel="noreferrer"><em>Contact me</em></a><em> if you run into any issues or need help consulting on your Azure AD B2C project.</em></p>
<!--kg-card-begin: html-->

<p>
As more and more companies move their applications to the cloud, managing user identities and access is a critical task. Azure Active Directory B2C is a cloud-based identity and access management solution that helps developers add identity and access management capabilities to their applications. However, querying and managing user data in Azure AD B2C can be a challenge. That's where the Microsoft Graph API comes in. In this blog post I&#8217;ll show you how to use Graph API to query Azure AD B2C users using <a href="https://www.postman.com/" target="_blank" rel="noreferrer noopener">Postman</a> and return a list of users in your B2C tenant. By the end of this post, you'll have a good understanding of how to get started with Graph API to manage user data in Azure AD B2C.

</p>

<h2>Register new Graph API app</h2>

<p>The first thing you need to do is sign into your Azure B2C tenant and create a new app registration for the Graph API app. Follow these steps to create a new Graph API app in your Azure environment.</p>

<ol>
<li>Switch to your B2C Tenant directory</li>

<li>Go to Azure AD B2C</li>

<li>Select <strong>App registrations</strong> in the Manage section</li>

<li>Create a <strong>New registration</strong></li>

<li>Give your app a name. I named mine <strong>B2C Graph API</strong>. Click save and make a copy of the newly created <strong>Client Id</strong>.</li>

<li>Click <strong>Certificates &amp; secrets</strong> and create a <strong>New client secret</strong>. Give it a descriptive name and set the expiration date. I usually set mine to 6 months.</li>

<li>Copy the Value of the newly created secret and hold on to it. Store the value in a secret vault or other safe place along with the client id from step 5.</li>

<li>Click <strong>API Permissions</strong>. This is where you define what resources/access your Graph API will have access to. For this post I&#8217;m going to all the application to read and write all users. Click <strong>Add a permission</strong>, then Microsoft Graph, Application permissions and search for <strong>User.ReadWrite.All</strong>. Select the checkbox and click <strong>Add permission</strong>.</li>
</ol>

<h2>Get an access token</h2>

<p>The next stop is to obtain an access token using the client credentials OAuth flow. Once an access token is created you&#8217;ll use that to authenticate your Graph API requests to return the user information.</p>

<figure class="wp-block-image size-full"><img decoding="async" loading="lazy" src="/images/wordpress/2023/03/Screenshot-2023-03-21-at-10.21.20-AM.png" alt="" class="wp-image-24791" sizes="(max-width: 775px) 100vw, 775px" / sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<p>Open Postman and create a new POST request with the following parameters:<br><br><code><strong>Action</strong>: POST<br><strong>Url</strong>: https://login.microsoftonline.com/{{TennantId}}/oauth2/v2.0/token where {{TenantId}} is the tenant id of your B2C Tennant<br><strong>scope</strong>: https://graph.microsoft.com/.default<br><strong>grant_type</strong>: client_credentials<br><strong>client_secret</strong>: YOUR_CLIENT_SECRET<br><strong>client_id</strong>: YOUR_GRAPHAPI_CLIENT_ID</code><br></p>

<p>You should get a response that contains an <strong>access_token</strong> property. Copy the access token and open a new Postman tab. In the new tab enter the GET request URL equal to <strong>https://graph.microsoft.com/v1.0/users</strong>. Click the <strong>Authorization</strong> tab and select <strong>Bearer Token</strong> from the type drop down. Enter the access token you copied form the <strong>/token</strong> request and paste it into the text box for the <strong>Token</strong>. Your request should look like this:</p>

<figure class="wp-block-image size-large"><img decoding="async" loading="lazy" src="/images/wordpress/2023/03/Screenshot-2023-03-21-at-12.06.38-PM.png" alt="" class="wp-image-24792" sizes="(max-width: 1024px) 100vw, 1024px" / sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<p>Assuming it&#8217;s all set up correctly, after clicking <strong>Send</strong> you&#8217;ll get a response with a list of users who are part of your B2C tenant. From here you can append the <strong>Id</strong> to the URL and return the specific user details.</p>

<figure class="wp-block-image size-large"><img decoding="async" loading="lazy" src="/images/wordpress/2023/03/Screenshot-2023-03-21-at-12.14.10-PM.png" alt="" class="wp-image-24793" sizes="(max-width: 1024px) 100vw, 1024px" / sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<p>Related: <a href="https://clintmcmahon.com/add-role-claims-to-an-azure-b2c-user-flow-access-token/" data-type="URL" data-id="https://clintmcmahon.com/add-role-claims-to-an-azure-b2c-user-flow-access-token/">Add role claims to an Azure B2C user flow access token</a><br><br></p>

<!--kg-card-end: html-->
