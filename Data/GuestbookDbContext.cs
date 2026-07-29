using Microsoft.EntityFrameworkCore;
using Website.Models;

namespace Website.Data;

public class GuestbookDbContext : DbContext
{
    public GuestbookDbContext(DbContextOptions<GuestbookDbContext> options) : base(options) { }

    public DbSet<GuestbookEntry> GuestbookEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GuestbookEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}
