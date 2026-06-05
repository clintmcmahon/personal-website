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

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IPostRepository, PostRepository>();
builder.Services.AddSingleton<PhotoRepository>(provider =>
    new PhotoRepository(Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "photos"), builder.Environment.IsDevelopment()));
builder.Services.AddScoped<PhotoService>();
builder.Services.AddScoped<ImageProcessingService>();

var ghConfig = builder.Configuration.GetSection("GitHub");
var ghPat = ghConfig["PersonalAccessToken"] ?? string.Empty;
var ghOwner = ghConfig["Owner"] ?? "clintmcmahon";
var ghRepo = ghConfig["Repo"] ?? "personal-website";
builder.Services.AddScoped<GitHubService>(_ => new GitHubService(ghPat, ghOwner, ghRepo));
builder.Services.AddScoped<PhotoPostService>();

builder.Services.AddSession();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PhotoCommentDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
