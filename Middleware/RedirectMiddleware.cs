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
        var host = context.Request.Host.Host;

        // Skip redirect logic for photos.* subdomain
        if (host.StartsWith("photos."))
        {
            await _next(context);
            return;
        }

        // Routing is case-insensitive, so "/Blog/my-post" and "/blog/my-post" both serve
        // the same page. The feeds emitted the "/Blog/" form for a long time, so send
        // those to the canonical lowercase URL instead of leaving two live spellings.
        // GET and HEAD only: a 301 on a POST would drop the body and turn it into a GET.
        var requestPath = context.Request.Path.Value ?? string.Empty;
        var isSafeMethod = HttpMethods.IsGet(context.Request.Method) || HttpMethods.IsHead(context.Request.Method);
        if (isSafeMethod && requestPath.Any(char.IsUpper))
        {
            context.Response.Redirect(requestPath.ToLowerInvariant() + context.Request.QueryString, true);
            return;
        }

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
