---
title: "How to execute multiple SQL Server views in C#"
description: "This blog post shows how to return data from multiple SQL Server views in a single database trip with C# WebAPI. "
date: "2024-07-18"
draft: false
slug: "how-to-execute-multiple-sql-server-views-from-entity-framework"
tags:
---

<p>I've had this problem come up over that past couple years so I decided to write a blog post about how to make this work. When I'm working with large datasets from multiple data sources I often use a SQL view to combine the data so it can be returned in a single query. There are times when I want my API to return the results from multiple views without having to call the database n-number of times. This blog post shows how to return data from multiple SQL Server views in a single database trip. </p><p>The three major sections of this process are as follows:</p><ol><li><strong>Use a Single Database Connection</strong>: Execute all views in a single query using a single database connection.</li><li><strong>Use a Single Command</strong>: Combine the SQL commands into a single command.</li><li><strong>Read Multiple Result Sets</strong>: Use <code>DbDataReader</code> to read multiple result sets efficiently.</li></ol><h3 id="step-by-step-guide">Step-by-Step Guide</h3><h3 id="step-1-define-your-entities">Step 1: Define Your Entities</h3><p>Define the entities that correspond to the views. These results can be any POCO class or view model. They should represent the dataset that is returned from each of the views. </p><pre><code class="language-csharp">public class View1Result
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class View2Result
{
public int Id { get; set; }
public string Description { get; set; }
}

public class View3Result
{
public int Id { get; set; }
public DateTime Date { get; set; }
}

public class View4Result
{
public int Id { get; set; }
public decimal Amount { get; set; }
}
</code></pre><h3 id="step-2-configure-the-method">Step 2: Configure the method</h3><p>Configure the method for fetching data from the views. In the method below you combine your select statements from each view together in single DbCommand so they execute within a single database connection - therefore bypassing the need to make multiple trips to the database. </p><p>The results are returned in sequential order to the DbReader with multiple result sets. After reading your result set, a call to .NextResultSetAsync() will move the reader to the next result set in order of execution and iterate through the results adding to your POCO collection. It's important to have your query order in the same order so you are mapping the correct data.</p><p><strong>GetMyData.cs:</strong></p><pre><code class="language-csharp">using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

public class DataService
{
private readonly ApplicationDbContext \_context;

    public GetMyData(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task&lt;(List&lt;View1Result&gt;, List&lt;View2Result&gt;, List&lt;View3Result&gt;, List&lt;View4Result&gt;)&gt; GetViews()
    {
        var view1Results = new List&lt;View1Result&gt;();
        var view2Results = new List&lt;View2Result&gt;();
        var view3Results = new List&lt;View3Result&gt;();
        var view4Results = new List&lt;View4Result&gt;();

        using (var command = Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = @"
                SELECT * FROM View1;
                SELECT * FROM View2;
                SELECT * FROM View3;
                SELECT * FROM View4;";
            command.CommandType = CommandType.Text;

            await Database.OpenConnectionAsync();

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    view1Results.Add(new View1Result
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }

                await reader.NextResultAsync();

                while (await reader.ReadAsync())
                {
                    view2Results.Add(new View2Result
                    {
                        Id = reader.GetInt32(0),
                        Description = reader.GetString(1)
                    });
                }

                await reader.NextResultAsync();

                while (await reader.ReadAsync())
                {
                    view3Results.Add(new View3Result
                    {
                        Id = reader.GetInt32(0),
                        Date = reader.GetDateTime(1)
                    });
                }

                await reader.NextResultAsync();

                while (await reader.ReadAsync())
                {
                    view4Results.Add(new View4Result
                    {
                        Id = reader.GetInt32(0),
                        Amount = reader.GetDecimal(1)
                    });
                }

                //....and so on and so on
            }
        }

        return (view1Results, view2Results, view3Results, view4Results);
    }

}

</code></pre><h3 id="step-3-call-the-method-from-the-controller">Step 3: Call the Method from the Controller</h3><p>Use the method in your controller to fetch the data and return it in the API response.</p><p><strong>ViewsController.cs:</strong></p><pre><code class="language-csharp">using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ViewsController : ControllerBase
{
private readonly IDataService \_dataService;

    public ViewsController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet("GetViewResults)]
    public async Task&lt;IActionResult&gt; GetViewResults()
    {
        var (view1Results, view2Results, view3Results, view4Results) = await _dataService.GetViewResults();
        return Ok(new
        {
            View1Results = view1Results,
            View2Results = view2Results,
            View3Results = view3Results,
            View4Results = view4Results
        });
    }

}

</code></pre><h3 id="summary">Summary</h3><ul><li><strong>Single Database Connection</strong>: Use a single database connection to execute all views in a single query.</li><li><strong>Efficient Reading</strong>: Use <code>DbDataReader</code> to efficiently read multiple result sets.</li><li><strong>Combine Queries</strong>: Combine the SQL queries for all views into a single command to minimize database round trips.</li></ul><p>By following this approach, you can ensure that all views are executed efficiently and the results are returned to C# collections.</p>
