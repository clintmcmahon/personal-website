using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Website.Models;

namespace Website.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var tagsConverter = new ValueConverter<List<string>, string>(
            v => string.Join(",", v),
            v => string.IsNullOrWhiteSpace(v)
                ? new List<string>()
                : v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                   .Select(t => t.Trim())
                   .ToList()
        );

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.Slug).IsUnique();
            entity.Property(p => p.Tags).HasConversion(tagsConverter);
            entity.Ignore(p => p.TagsRaw);
        });
    }
}
