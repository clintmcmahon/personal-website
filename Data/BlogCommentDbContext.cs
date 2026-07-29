using Microsoft.EntityFrameworkCore;
using Website.Models;

namespace Website.Data;

public class BlogCommentDbContext : DbContext
{
    public BlogCommentDbContext(DbContextOptions<BlogCommentDbContext> options) : base(options) { }

    public DbSet<BlogComment> BlogComments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogComment>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Slug);
        });
    }
}
