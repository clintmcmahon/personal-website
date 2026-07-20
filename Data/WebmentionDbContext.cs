using Microsoft.EntityFrameworkCore;
using Website.Models;

namespace Website.Data;

public class WebmentionDbContext : DbContext
{
    public WebmentionDbContext(DbContextOptions<WebmentionDbContext> options) : base(options) { }

    public DbSet<PendingWebmention> PendingWebmentions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PendingWebmention>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => new { p.Sent, p.ScheduledFor });
        });
    }
}
