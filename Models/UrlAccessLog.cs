using System.ComponentModel.DataAnnotations;

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
    public Guid ShortenedUrlId { get; set; }
    public ShortenedUrl ShortenedUrl { get; set; } = null!;

    [Required]
    public string IpAddress { get; set; } = null!;

    [Required]
    public DateTime AccessedAt { get; set; }

    [Required]
    public string SecretUrl { get; set; } = null!;  
}
