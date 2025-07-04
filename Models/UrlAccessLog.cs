using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class UrlAccessLog
    {
        public UrlAccessLog()
        {
            this.Id = Guid.NewGuid();
            this.AccessedAt = DateTime.UtcNow;
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string ShortenedUrlId { get; set; } = null!;
        public ShortenedUrl ShortenedUrl { get; set; } = null!;

        [Required]
        public string IpAddress { get; set; } = null!;

        [Required]
        public DateTime AccessedAt { get; set; }

    }
}
