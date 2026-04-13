namespace Website.Middleware;

public class SubdomainMiddleware
{
    private readonly RequestDelegate _next;

    public SubdomainMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var host = context.Request.Host.Host;

        if (host.StartsWith("photos.", StringComparison.OrdinalIgnoreCase))
        {
            var path = context.Request.Path.Value ?? "/";

            // Rewrite subdomain paths to their /photos/* equivalents:
            //   /              → /photos
            //   /2025-04-13   → /photos/2025-04-13
            //   /about        → /photos/about
            //   /rss          → /photos/rss
            // Paths already under /photos/* pass through unchanged.
            if (!path.StartsWith("/photos", StringComparison.OrdinalIgnoreCase))
            {
                context.Request.Path = path == "/" ? "/photos" : "/photos" + path;
            }
        }

        await _next(context);
    }
}
