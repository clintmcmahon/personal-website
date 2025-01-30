---
title: "How to view all Azure AD B2C users programmatically"
description: "This post shows how to get a list of Azure AD B2C users programmatically by using the Graph API client for .Net in a .Net Core (8) MVC web app. "
date: "2024-05-22"
draft: false
slug: "how-to-view-all-azure-ad-b2c-users-programatically"
tags:
---

<p>This post shows how to get a list of Azure AD B2C users programmatically by using the <a href="https://github.com/microsoftgraph/msgraph-sdk-dotnet" rel="noreferrer">Graph API client for .Net</a> in a .Net Core (8) MVC web app. </p><p>I've built an <a href="https://github.com/clintmcmahon/azure-ad-b2c-user-manager" rel="noreferrer">entire Azure AD B2C User Management solution</a> that is available for free on Github. The code samples below are part of this larger solution. Feel free to use this application as you see fit. <a href="/contact" rel="noreferrer">Contact me</a> if you run into any issues or need help consulting on your Azure AD B2C project.</p><h2 id="prerequisites">Prerequisites</h2><p>You will need the following to get started:</p><ol><li><strong>Azure AD B2C Tenant</strong>: You need an Azure AD B2C tenant set up.</li><li><strong>App Registration</strong>: Register an application in your B2C tenant and configure API permissions.</li><li><strong>Client Credentials</strong>: Note down your client ID, client secret, and tenant ID.</li></ol><h2 id="setting-up-the-b2cusersservice">Setting Up the B2CUsersService</h2><p>I've created a service class called B2CUsersService that handles the interaction with Microsoft Graph API. We'll use the <code>ClientSecretCredential</code> from the <code>Azure.Identity</code> library to authenticate.</p><p>The B2CUsersService constructor utilizes the appsettings.json file to get the tenantId, clientId and clientSecret values. Open the appsettings.json file from the root of the web project. There you will enter the client id, client secret and tenant id for your B2C tenant.</p><h3 id="step-1-initialize-the-graph-service-client">Step 1: Initialize the Graph Service Client</h3><p>Create the <code>B2CUsersService</code> and initialize the <code>GraphServiceClient</code> in the constructor using the client credentials flow. Make sure to request the <code>/.default</code> scope, which includes all the permissions granted to the app in the Azure portal.</p><pre><code class="language-csharp">using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Reflection;
namespace B2CUserAdmin.Web.Services;

public class B2CUsersService
{
private readonly GraphServiceClient \_graphServiceClient;
private readonly IConfiguration \_configuration;

    public B2CUsersService(IConfiguration configuration)
    {
        // The client credentials flow requires that you request the
        // /.default scope, and pre-configure your permissions on the
        // app registration in Azure. An administrator must grant consent
        // to those permissions beforehand.
        var scopes = new[] { "https://graph.microsoft.com/.default" };

        // using Azure.Identity;
        var options = new ClientSecretCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
        };

        var tenantId = _configuration["AzureAdB2C:TenantId"];
        var clientId = _configuration["AzureAdB2C:ClientId"];
        var clientSecret = _configuration["AzureAdB2C:ClientSecret"];

        // https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret, options);

        _graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);

    }

    //More methods

}</code></pre><h3 id="step-2-retrieve-the-list-of-users">Step 2: Retrieve the List of Users</h3><p>Next, create a method to fetch users from Azure AD B2C. This method uses the <code>GraphServiceClient</code> to send a request to Microsoft Graph and retrieve the users. Use the <code>.Select</code> method to identify what fields to return that might fall outside of the basic list of user properties to return. Do not pass QueryParameters if you are OK with returning the default properties of the user object.</p><pre><code class="language-csharp">public async Task&lt;IEnumerable&lt;User&gt;&gt; GetUsersAsync()
{
var users = await \_graphServiceClient.Users
.GetAsync(requestConfiguration =&gt;
{
requestConfiguration.QueryParameters.Select = ["displayName", "id", "identities", "otherMails"];

            });

        return users.Value;
    }</code></pre><h3 id="full-implementation">Full Implementation</h3><p>Here's the complete implementation of the <code>B2CUsersService</code>:</p><pre><code class="language-csharp">using Azure.Identity;

using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Reflection;
namespace B2CUserAdmin.Web.Services;

public class B2CUsersService
{
private readonly GraphServiceClient \_graphServiceClient;
private readonly IConfiguration \_configuration;

    public B2CUsersService(IConfiguration configuration)
    {
        // The client credentials flow requires that you request the
        // /.default scope, and pre-configure your permissions on the
        // app registration in Azure. An administrator must grant consent
        // to those permissions beforehand.
        var scopes = new[] { "https://graph.microsoft.com/.default" };

        // using Azure.Identity;
        var options = new ClientSecretCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
        };

        var tenantId = _configuration["AzureAdB2C:TenantId"];
        var clientId = _configuration["AzureAdB2C:ClientId"];
        var clientSecret = _configuration["AzureAdB2C:ClientSecret"];

        // https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret, options);

        _graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);

    }

    public async Task&lt;IEnumerable&lt;User&gt;&gt; GetUsersAsync()
    {
        var users = await _graphServiceClient.Users
            .GetAsync(requestConfiguration =&gt;
            {
                requestConfiguration.QueryParameters.Select = ["displayName", "id", "identities", "otherMails"];

            });

        return users.Value;
    }</code></pre><h2 id="display-the-list-of-users">Display the list of users</h2><p>This example is using .Net 8 and the MVC pattern. The service code above will work in other implementations than MVC. This is how to view the list of users in a simple MVC Razor view.</p><h3 id="indexcshtml">Index.cshtml</h3><pre><code class="language-razor">@model IEnumerable&lt;Microsoft.Graph.Models.User&gt;

@{
ViewData["Title"] = "User List";
}

&lt;h2&gt;@ViewData["Title"]&lt;/h2&gt;

&lt;table class="table table-striped"&gt;
&lt;thead&gt;
&lt;tr&gt;
&lt;th&gt;Name&lt;/th&gt;
&lt;th&gt;Identity/User Name&lt;/th&gt;
&lt;th&gt;Actions&lt;/th&gt;
&lt;/tr&gt;
&lt;/thead&gt;
&lt;tbody&gt;
@foreach (var user in Model)
{
&lt;tr&gt;
&lt;td&gt;@user.DisplayName&lt;/td&gt;
&lt;td&gt;
@{
var displayIdentities = user.Identities?
.Where(identity =&gt; identity.SignInType != "userPrincipalName")
.Select(identity =&gt; $"{identity.SignInType}: {identity.IssuerAssignedId} ({identity.Issuer})");

    var displayOtherMails = user.OtherMails;

}

@if (displayIdentities != null &amp;&amp; displayIdentities.Any())
{
&lt;p&gt;Identities: @string.Join(", ", displayIdentities)&lt;/p&gt;
}

@if (displayOtherMails != null &amp;&amp; displayOtherMails.Any())
{
&lt;p&gt;Other Emails: @string.Join(", ", displayOtherMails)&lt;/p&gt;
}
&lt;/td&gt;
&lt;td&gt;&lt;a asp-action="Details" asp-route-id="@user.Id"&gt;View&lt;/a&gt; | &lt;a asp-action="Edit" asp-route-id="@user.Id"&gt;Edit&lt;/a&gt;&lt;/td&gt;
&lt;/tr&gt;
}
&lt;/tbody&gt;
&lt;/table&gt;
</code></pre><h3 id="userscontrollercs">UsersController.cs</h3><pre><code class="language-csharp">using Microsoft.AspNetCore.Mvc;
using B2CUserAdmin.Web.Services;
using B2CUserAdmin.Models;
namespace B2CUserAdmin.Web.Controllers;

public class UsersController : Controller
{
private B2CUsersService \_graphAPIService;
private readonly IConfiguration \_configuration;
private readonly ILogger&lt;UsersController&gt; \_logger;

    public UsersController(IConfiguration configuration,
    ILogger&lt;UsersController&gt; logger, B2CUsersService graphAPIService)
    {
        _configuration = configuration;
        _logger = logger;
        _graphAPIService = graphAPIService;
    }

    public async Task&lt;IActionResult&gt; Index()
    {
        var users = await _graphAPIService.GetUsersAsync();
        return View(users);
    }

}</code></pre><h2 id="running-the-code">Running the Code</h2><p>To run this code, make sure you've added the necessary NuGet packages:</p><ul><li><code>Microsoft.Graph</code></li><li><code>Azure.Identity</code></li></ul><p>You can install these packages using the following commands:</p><pre><code class="language-sh">dotnet add package Microsoft.Graph
dotnet add package Azure.Identity
</code></pre><p>Now run <code>dotnet run</code> and navigate to <code>/users</code> where you should see a list of your Azure AD B2C users for the tenant you created the credential for.</p><figure class="kg-card kg-image-card kg-card-hascaption"><img src="/images/2024/05/azure-ad-b2c-users.png" class="kg-image" alt="" loading="lazy" srcset="/images/2024/05/azure-ad-b2c-users.png 600w, /images/2024/05/azure-ad-b2c-users.png 1000w, /images/2024/05/azure-ad-b2c-users.png 1237w" sizes="(min-width: 720px) 720px" sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"><figcaption><span style="white-space: pre-wrap;">A list of Azure AD B2C Users</span></figcaption></figure><h2 id="conclusion">Conclusion</h2><p>With this setup, you can now retrieve a list of users from your Azure AD B2C tenant programmatically. This approach is scalable and can be integrated into various applications to manage your B2C users efficiently. If you encounter any issues or have questions, <a href="/contact" rel="noreferrer">feel free to reach out</a>. Happy coding!</p>
