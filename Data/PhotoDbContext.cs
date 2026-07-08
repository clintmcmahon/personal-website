using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Website.Models;

namespace Website.Data;

public class PhotoDbContext : DbContext
{
    private static readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

    public PhotoDbContext(DbContextOptions<PhotoDbContext> options) : base(options) { }

    public DbSet<PhotoEntry> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var tagsConverter = new ValueConverter<List<string>, string>(
            v => string.Join(",", v),
            v => string.IsNullOrWhiteSpace(v)
                ? new List<string>()
                : v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList()
        );

        var rowsConverter = new ValueConverter<List<List<PhotoImage>>, string>(
            v => JsonSerializer.Serialize(v, _jsonOpts),
            v => string.IsNullOrWhiteSpace(v)
                ? new List<List<PhotoImage>>()
                : JsonSerializer.Deserialize<List<List<PhotoImage>>>(v, _jsonOpts) ?? new()
        );

        modelBuilder.Entity<PhotoEntry>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.Slug).IsUnique();
            entity.HasIndex(p => p.Date);
            entity.Property(p => p.Tags).HasConversion(tagsConverter);
            entity.Property(p => p.Rows).HasConversion(rowsConverter);
        });
    }
}
