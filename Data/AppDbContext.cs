using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<ShortenedUrl> ShortenedUrls { get; set; } = null!;
        public DbSet<UrlAccessLog> UrlAccessLogs { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortenedUrl>()
                .HasMany(s => s.AccessLogs)
                .WithOne(a => a.ShortenedUrl)
                .HasForeignKey(a => a.ShortenedUrlId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
