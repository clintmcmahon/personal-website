using Website.Data;
using Microsoft.EntityFrameworkCore;
using Website.Middleware;
using Website.Repositories;
using Website.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

// Add SQLite for photo comments
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "photocomments.db");
builder.Services.AddDbContext<PhotoCommentDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// Add SQLite for blog posts
var blogDbPath = Path.Combine(builder.Environment.ContentRootPath, "blog.db");
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlite($"Data Source={blogDbPath}"));

// Add SQLite for photo posts
var photoDbPath = Path.Combine(builder.Environment.ContentRootPath, "photos.db");
builder.Services.AddDbContext<PhotoDbContext>(options =>
    options.UseSqlite($"Data Source={photoDbPath}"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPostRepository, DatabasePostRepository>();
builder.Services.AddScoped<IPhotoRepository, DatabasePhotoRepository>();

// Keep the filesystem-based repo registered as a concrete type for the one-time migration route
builder.Services.AddSingleton<PhotoRepository>(provider =>
    new PhotoRepository(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "photos"),
        builder.Environment.IsDevelopment()));

builder.Services.AddScoped<PhotoService>();
builder.Services.AddScoped<ImageProcessingService>();
builder.Services.AddHttpClient("Mastodon");
builder.Services.AddScoped<MastodonService>();

builder.Services.AddHttpClient("Webmention", c =>
{
    c.Timeout = TimeSpan.FromSeconds(8);
    c.DefaultRequestHeaders.UserAgent.ParseAdd("clintmcmahon.com-webmention/1.0");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });
builder.Services.AddScoped<WebmentionService>();

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient("MastodonPublic", c =>
{
    c.Timeout = TimeSpan.FromSeconds(8);
    c.DefaultRequestHeaders.UserAgent.ParseAdd("clintmcmahon.com-engagement/1.0");
});
builder.Services.AddScoped<MastodonEngagementService>();

builder.Services.AddSession();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PhotoCommentDbContext>();
    db.Database.Migrate();

    var blogDb = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
    blogDb.Database.Migrate();

    var photoDb = scope.ServiceProvider.GetRequiredService<PhotoDbContext>();
    photoDb.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<RedirectMiddleware>();
app.UseMiddleware<SubdomainMiddleware>();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
