using Microsoft.EntityFrameworkCore;
using Website.Models;

namespace Website.Data
{
    public class PhotoCommentDbContext : DbContext
    {
        public PhotoCommentDbContext(DbContextOptions<PhotoCommentDbContext> options) : base(options) { }

        public DbSet<PhotoComment> PhotoComments { get; set; }
    }
}
