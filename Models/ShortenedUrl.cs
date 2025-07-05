using System.ComponentModel.DataAnnotations;
using UrlShortener.Models;

public class ShortenedUrl
{
    public ShortenedUrl()
    {
        this.Id = Guid.NewGuid();
        this.CreatedAt = DateTime.UtcNow;
        this.AccessLogs = new List<UrlAccessLog>();
    }

    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(2048)]
    public string OriginalUrl { get; set; } = null!;

    [Required, MaxLength(2048)]
    public string SecretUrl { get; set; } = null!;

    [Required, MaxLength(10)]
    public string ShortCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual List<UrlAccessLog> AccessLogs { get; set; }
}
