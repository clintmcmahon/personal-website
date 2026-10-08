using Website.Data;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Website.Middleware;
using Website.Repositories;
using Website.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

// Add SQLite for blog posts
var blogDbPath = Path.Combine(builder.Environment.ContentRootPath, "blog.db");
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlite($"Data Source={blogDbPath}"));

// Add SQLite for the delayed webmention send queue
var webmentionDbPath = Path.Combine(builder.Environment.ContentRootPath, "webmentions.db");
builder.Services.AddDbContext<WebmentionDbContext>(options =>
    options.UseSqlite($"Data Source={webmentionDbPath}"));

// Add SQLite for the guestbook
var guestbookDbPath = Path.Combine(builder.Environment.ContentRootPath, "guestbook.db");
builder.Services.AddDbContext<GuestbookDbContext>(options =>
    options.UseSqlite($"Data Source={guestbookDbPath}"));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Url.Action otherwise emits the controller name as declared ("/Blog/my-post"),
// which does not match the lowercase canonical tags.
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddScoped<IPostRepository, DatabasePostRepository>();

builder.Services.AddSingleton<OgImageService>();
builder.Services.AddHttpClient("Mastodon");
builder.Services.AddScoped<MastodonService>();

builder.Services.AddHttpClient("Webmention", c =>
{
    c.Timeout = TimeSpan.FromSeconds(8);
    c.DefaultRequestHeaders.UserAgent.ParseAdd("clintmcmahon.com-webmention/1.0");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });
builder.Services.AddScoped<WebmentionService>();
builder.Services.AddHostedService<WebmentionDispatcherService>();

// Login attempts: 5 per minute per client IP, since the admin password is a single
// shared secret with no other brute-force defense (no per-account lockout to bypass).
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("login", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 5,
                QueueLimit = 0
            }));
});

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient("MastodonPublic", c =>
{
    c.Timeout = TimeSpan.FromSeconds(8);
    c.DefaultRequestHeaders.UserAgent.ParseAdd("clintmcmahon.com-engagement/1.0");
});
builder.Services.AddScoped<MastodonEngagementService>();

builder.Services.AddHttpClient("Weather", c =>
{
    c.Timeout = TimeSpan.FromSeconds(6);
    c.DefaultRequestHeaders.UserAgent.ParseAdd("clintmcmahon.com-weather/1.0");
});
builder.Services.AddScoped<WeatherService>();

// Persist the data protection key ring to disk. Without this, the keys used to encrypt/decrypt
// the auth cookie live only in memory and get regenerated every process restart — so every
// deploy (which stops/starts the systemd service) silently invalidates every existing login,
// regardless of the 30-day sliding expiration below. The "keys" folder is excluded from the
// rsync --delete in deploy.yml so it survives deploys.
//
// The path is configuration rather than a constant because the photoblog at
// photos.clintmcmahon.com shares this login: both apps must persist to the SAME directory
// and use the SAME application name, or a cookie issued by one is undecryptable by the other.
// Set DataProtection:KeyPath to the shared directory in appsettings.local.json on the server.
var keyPath = builder.Configuration["DataProtection:KeyPath"] ?? "keys";
if (!Path.IsPathRooted(keyPath))
    keyPath = Path.Combine(builder.Environment.ContentRootPath, keyPath);

builder.Services.AddDataProtection()
    .SetApplicationName(builder.Configuration["DataProtection:ApplicationName"] ?? "clintmcmahon-website")
    .PersistKeysToFileSystem(new DirectoryInfo(keyPath));

// Persistent, signed auth cookie — not server-side session state. This is what actually
// survives (a) being away from the keyboard for a long stretch, via sliding expiration,
// and (b) deploys, since every push recycles the app pool and would otherwise wipe any
// in-memory session instantly regardless of idle timeout.
var cookieDomain = builder.Configuration["DataProtection:CookieDomain"];
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";
        options.Cookie.Name = "clintmcmahon_admin";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;

        // Empty in development (localhost rejects a dotted domain); set to ".clintmcmahon.com"
        // in production so the same cookie also authenticates against the photoblog.
        if (!string.IsNullOrWhiteSpace(cookieDomain))
            options.Cookie.Domain = cookieDomain;

        // The EasyMDE image-upload endpoint is called via fetch/XHR from the post editor,
        // not navigated to directly. If the session expires mid-edit, redirecting it to the
        // login HTML page (the default challenge behavior) just leaves EasyMDE trying to
        // parse a login page as JSON. Return a plain 401 for that one endpoint instead.
        options.Events.OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/admin/blog/upload-image"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }

            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var blogDb = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
    blogDb.Database.Migrate();

    var webmentionDb = scope.ServiceProvider.GetRequiredService<WebmentionDbContext>();
    webmentionDb.Database.Migrate();

    var guestbookDb = scope.ServiceProvider.GetRequiredService<GuestbookDbContext>();
    guestbookDb.Database.Migrate();
}

// The app runs behind a reverse proxy in production. Without this, Request.Scheme
// is "http" for every request, which leaks into anything that generates an absolute
// URL. Must come before any middleware that reads the scheme.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;
    headers["X-Content-Type-Options"] = "nosniff";
    headers["X-Frame-Options"] = "DENY";
    headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    // script-src needs 'unsafe-inline' and a handful of third-party hosts because the site
    // ships inline <script> blocks and loads EasyMDE/Masonry (unpkg), Font Awesome Kit,
    // Plausible analytics, Prism (cdnjs) and the Letterbird contact embed from CDNs rather
    // than bundling them locally. Still blocks arbitrary attacker-supplied script hosts.
    headers["Content-Security-Policy"] =
        "default-src 'self'; " +
        "img-src 'self' data: https:; " +
        "style-src 'self' 'unsafe-inline' https://cdnjs.cloudflare.com https://unpkg.com https://fonts.googleapis.com https://cdn.jsdelivr.net; " +
        "font-src 'self' data: https://ka-f.fontawesome.com https://fonts.gstatic.com https://cdn.jsdelivr.net; " +
        "script-src 'self' 'unsafe-inline' https://unpkg.com https://letterbird.co https://plausible.io https://cdnjs.cloudflare.com https://kit.fontawesome.com; " +
        "connect-src 'self' https://ka-f.fontawesome.com https://plausible.io; " +
        "frame-ancestors 'none'; " +
        "base-uri 'self'; " +
        "form-action 'self'";
    await next();
});

app.UseMiddleware<RedirectMiddleware>();

app.UseRouting();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
