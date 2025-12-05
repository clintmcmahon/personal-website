---
title: "Creating Custom Serilog Enrichers in C#"
description: "A practical guide to building and registering a custom Serilog enricher that automatically logs the authenticated user ID in ASP.NET Core."
date: "2025-11-12"
draft: true
slug: "custom-serilog-enricher-csharp"
tags: consulting
---

<section>

### What This Post Is About

Serilog is one of the most popular structured logging libraries for .NET. It helps developers move beyond plain text logs by capturing structured key-value pairs that tools like Seq, Kibana, or Application Insights can filter and analyze.

**Enrichers** are a key part of this system. They automatically attach extra information — like the current username, thread ID, or machine name — to every log entry, without you having to add it manually in every log statement.

In this post, we'll look at:
- what enrichers do,
- which ones are available out of the box,
- why you’d want to create your own,
- and how to build a custom enricher that logs the authenticated user’s ID in ASP.NET Core.

---

### Built-in Enrichers

Serilog ships with a number of useful enrichers and has an active ecosystem of community add-ons.  
Here are a few of the most common built-in ones:

| Enricher | What it adds |
|-----------|--------------|
| `WithMachineName` | The machine or container name |
| `WithThreadId` / `WithProcessId` | Thread or process information |
| `WithEnvironmentUserName` | The operating system user account |
| `FromLogContext` | Any properties pushed into the current `LogContext` |
| `WithExceptionDetails` (via `Serilog.Exceptions`) | Full exception data, including nested inner exceptions |

These cover a lot of environment-level context, but your application often needs its *own* context — things like `UserId`, `TenantId`, or `CorrelationId`.

---

### Why Use Enrichers

Enrichers help keep your logs consistent and useful. Instead of adding the same context manually everywhere, you let Serilog do it automatically.

Common use cases:
- **Consistency:** Every log line includes critical context like user ID or tenant.
- **Searchability:** Easily filter logs in your monitoring system by `UserId` or `RequestId`.
- **Traceability:** Identify which user triggered which action.
- **Multi-tenancy:** Automatically include `TenantId` for every log event.
- **Cross-cutting context:** Keep your code clean — no need to pass IDs through every layer.

> *Example:*  
> If you have hundreds of logs per second, but each includes the same user or tenant identifier, troubleshooting becomes dramatically easier.

---

### Example: A Custom `RequestUserIdEnricher`

Let’s build a custom enricher that adds the current authenticated user’s ID to every log event.

#### The Enricher Class

```csharp
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

public class RequestUserIdEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestUserIdEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        string userId = "anonymous";

        if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? httpContext.User.Identity.Name
                  ?? "unknown";
        }

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestUserId", userId));
    }
}
```

This class checks whether the current request has an authenticated user and adds a `RequestUserId` property to the log.

---

#### Registering the Enricher

Add the HTTP context accessor and configure Serilog to use your enricher.  
The important part is using the **DI-aware** overload of `UseSerilog`:

```csharp
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.With(new RequestUserIdEnricher(
            services.GetRequiredService<IHttpContextAccessor>()))
        .WriteTo.Console(outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} (User={RequestUserId}){NewLine}{Exception}");
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();
```

Because this configuration resolves the enricher from the real dependency injection container, the enricher has access to the actual `HttpContext` for each request.

---

#### Test It

Add a simple log statement in a controller:

```csharp
_logger.LogInformation("Inside controller log");
```

Now when you make an authenticated request, your console output should look like this:

```
[14:22:51 INF] HTTP GET /api/test responded 200 in 15.6 ms (User=abc123)
[14:22:51 INF] Inside controller log (User=abc123)
```

Unauthenticated users show up as:

```
[14:23:10 INF] HTTP GET /api/test responded 200 in 12.4 ms (User=anonymous)
```

---

### Wrapping Up

Enrichers are one of Serilog’s most powerful features.  
They keep your logs structured, searchable, and consistent — without scattering boilerplate through your codebase.

We used a `RequestUserIdEnricher` here, but the same pattern can be applied for:
- `TenantId`
- `RequestOrigin`
- `FeatureFlag`
- `CorrelationId`

When you centralize context with enrichers, your logs become self-describing — and that makes debugging, auditing, and observability dramatically easier.

</section>
