using Website.Data;
using Microsoft.EntityFrameworkCore;
using Website.Middleware;
using Website.Repositories;
using Website.Services;

var builder = WebApplication.CreateBuilder(args);

// Add SQLite for photo comments
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "photocomments.db");
builder.Services.AddDbContext<PhotoCommentDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IPostRepository, PostRepository>();
builder.Services.AddSingleton<PhotoRepository>(provider =>
    new PhotoRepository(Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "photos")));
builder.Services.AddScoped<PhotoService>();
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
