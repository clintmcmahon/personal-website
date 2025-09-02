namespace Website.Middleware;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Website.Repositories;

public class RedirectMiddleware
{
    private readonly RequestDelegate _next;

    public RedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path.Value?.Trim('/') ?? string.Empty;

        // Define paths that should not be redirected (controller routes)
        var excludedPaths = new[] { "photos", "projects", "about", "contact", "home", "blog", "now", "portfolio", "services", "rss", "sitemap" };

        // Only execute if the path has no slashes (indicating it's at the root) and is not an excluded path
        if (!string.IsNullOrEmpty(path) && !path.Contains("/") && !excludedPaths.Contains(path, StringComparer.OrdinalIgnoreCase))
        {
            // Resolve IPostRepository within the request scope
            var postRepository = context.RequestServices.GetService<IPostRepository>();

            if (postRepository != null && postRepository.GetPostBySlug(path) != null)
            {
                // Redirect to /blog/{slug} if post exists
                context.Response.Redirect($"/blog/{path}", true);
                return;
            }
        }

        // Continue to the next middleware if no redirect is needed
        await _next(context);
    }
}
