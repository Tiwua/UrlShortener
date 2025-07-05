using System;
using UrlShortener.Models;
using static System.Net.WebRequestMethods;

namespace UrlShortener.Data.Configuration
{
    public class UrlAccessLogsConfig
    {
        public static ShortenedUrl GenerateMockData()
        {
            var shortenedUrl = new ShortenedUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = "https://example.com/very-long-url",
                ShortCode = "abc123",
                SecretUrl = "https://example.com/very-long-urladvlsmvpsjafo0i=w[vtwev8254vsd62f1",
                CreatedAt = DateTime.UtcNow
            };

            var random = new Random();
            var ipAddresses = new[]
            {
                "192.168.0.1", "192.168.0.2", "192.168.0.3", "192.168.0.4", "192.168.0.5",
                "192.168.0.6", "192.168.0.7", "192.168.0.8", "192.168.0.9", "192.168.0.10"
            };

            for (int i = 0; i < 50; i++)
            {
                shortenedUrl.AccessLogs.Add(new UrlAccessLog
                {
                    Id = Guid.NewGuid(),
                    ShortenedUrlId = shortenedUrl.Id,
                    IpAddress = ipAddresses[random.Next(ipAddresses.Length)],
                    AccessedAt = DateTime.UtcNow.AddDays(-random.Next(0, 5)),
                    SecretUrl = shortenedUrl.SecretUrl
                });
            }

            return shortenedUrl;
        }
    }
}
