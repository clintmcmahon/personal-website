---
title: "How to create a custom bound OData function"
description: "Create a custom OData function. Specifically a bound function which is a function that is associated with a specific entity type or entity set."
date: "2024-06-06"
draft: false
slug: "how-to-create-a-custom-bound-odata-function"
tags:
---

<p>Sometimes you have to create a custom function in your OData WebAPI project to return custom logic or call a stored procedure to return a result set that doesn't come out of the box with OData. In order to do this, create a custom OData function. Specifically a bound function which is a function that is associated with a specific entity type or entity set.</p><h2 id="define-function">Define function</h2><p>Start by defining the bound function in <code>Program.cs</code> where the other OData entities are defined. In this example and the rest of the example I'm going to call my bound class or entity <code>TEntity</code>. This could be any of your entity types. The function <code>GetRelatedEntities</code> is the name of my custom function. It's bound to the TEntity type by the <code>ReturnsCollectionFromEntitySet&lt;TEntity&gt;("TEntities")</code>. This tells OData that the return type of the function is going to be of type <code>TEntities</code>. </p><pre><code class="language-csharp">var entityType = builder.EntityType&lt;TEntity&gt;();
        entityType.Collection.Function("GetRelatedEntities").ReturnsCollectionFromEntitySet&lt;TEntity&gt;("TEntities");
</code></pre><h2 id="implement-logic-in-the-controller">Implement logic in the controller</h2><p>Create a new method for the custom function in your controller for <code>TEntity</code>. This logic executes a custom stored procedure that returns data in the same format as the <code>TEntity</code> model. In order to execute a stored procedure the results must match the type of entity the SQL is called from. In this case, <code>TEntities</code>.</p><pre><code class="language-csharp">public class TEntitiesController : ODataController
{
    private readonly YourDbContext _context;

    public TEntitiesController(YourDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [EnableQuery]
    public IQueryable&lt;TEntity&gt; GetRelatedEntities()
    {
        var entities = _context.TEntities
                       .FromSqlRaw("Exec GetEntities")
                       .AsQueryable();

        return entities;
    }

}
</code></pre><p>That's all that is needed to create a custom bound OData function. To view the results of this function you'd call it at the end of the url like:<br><code>https://localhost:5001/odata/TEntities/GetRelatedEntities()</code></p>
