---
title: "OData function is not returning data"
description: "This is a blog post about why a custom OData function is not returning any data despite returning a 200 code. "
date: "2024-03-14"
draft: false
slug: "odata-function"
tags:
---

<p>This is a blog post about why a custom OData function is not returning any data despite returning a 200 code. Or why your function or action is not returning the custom navigation properties you created on a main entity. This can happen when fetching complex objects without using the <code>$expand</code> query parameter. The <code>$expand</code> parameter will expand the specific objects in the request. If you don't use expand query parameter your complex objects will not be returned in the response. </p><p>In this post I'm going to break down my scenario and the steps that I took to fix the issue. In the example below my function is called <code>GetUsersInRoles()</code> that returns a custom view model with two properties, Administrators and Users. </p><h4 id="the-scenario-getusersinroles-function">The Scenario: <code>GetUsersInRoles()</code> Function</h4><p>My situation called for returning a custom view model with two complex objects for roles, Administrators and Users, both of type <code>List&lt;User&gt;</code>. Each role contains a list of users with properties like Name, Email, and IsActive.</p><p>Here's a simplified version of the custom view model:</p><pre><code class="language-csharp">public class UsersInRolesViewModel
{
    public List&lt;User&gt; Administrators { get; set; }
    public List&lt;User&gt; Editors { get; set; }
}

public class User
{
public string Name { get; set; }
public string Email { get; set; }
public bool IsActive { get; set; }
}
</code></pre><p>And the function in your OData controller might look something like this:</p><pre><code class="language-csharp">public class UsersController : ODataController
{
[HttpGet]
public IHttpActionResult GetUsersInRoles()
{
var usersInRoles = new UsersInRolesViewModel
{
Administrators = \_userService.GetAdministrators(),
Editors = \_userService.GetEditors()
};

        return Ok(usersInRoles);
    }

}
</code></pre><p>You would expect to access this data by navigating to <code>/odata/users/GetUsersInRoles()</code>. However, if you've not expanded the complex objects you're met with no data returned or missing complex data from your response. If you have other properties in your response object, those will show up, but not the complex objects as they need to be expanded.</p><h4 id="understanding-the-issue">Understanding the Issue</h4><p>The problem here is not with your model or the controller; it's with how OData handles the serialization of nested custom classes. By default, OData does not expand and serialize nested objects unless explicitly told to do so. This behavior aims to optimize performance and avoid inadvertently sending unnecessary large amounts of nested data over the network.</p><h4 id="the-solution-using-the-expand-query-option">The Solution: Using the <code>$expand</code> Query Option</h4><p>To instruct OData to include the nested classes (<code>Administrators</code> and <code>Editors</code> in this case), you need to use the <code>$expand</code> query option in your request URL. The <code>$expand</code> option tells OData to "expand" and include the specified nested properties in the response.</p><p>The corrected URL to access your data would be:</p><pre><code class="language-bash">/odata/users/GetUsersInRoles()?$expand=Administrators,Editors
</code></pre><p>To make this work, ensure your OData configuration in Program.cs is set up for the custom function like so:</p><pre><code class="language-csharp">var usersEntitySet = builder.EntitySet&lt;User&gt;("Users");
usersEntitySet.Function("GetUsersInRoles").Returns&lt;UsersInRolesViewModel&gt;();</code></pre><ol start="2"><li><strong>Adjust the Model or Controller if Necessary</strong>: In some cases, you might need to adjust your model or controller to ensure that the <code>$expand</code> option is properly applied. However, in most scenarios involving simple property expansion, no additional changes are required beyond enabling the query option in your configuration.</li></ol><h4 id="conclusion">Conclusion</h4><p>The <code>$expand</code> query option is a powerful tool in your arsenal for controlling data serialization, especially when dealing with complex nested models. By correctly applying this option, you can ensure that your application delivers exactly the data it needs, no more and no less. </p>
