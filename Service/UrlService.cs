using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Contracts;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Service
{
    public class UrlService
    {
        private readonly AppDbContext context;
        public UrlService(AppDbContext context)
        {
            this.context = context;
        }
        //public Task<IActionResult> Shorten(string originalUrl)
        //{
        //    if (string.IsNullOrWhiteSpace(originalUrl) || !Uri.IsWellFormedUriString(originalUrl, UriKind.Absolute))
        //    {
        //        ModelState.AddModelError("", "Please enter a valid URL.");
        //        return View("Index");
        //    }

        //    string shortCode = GenerateShortCode(8);

        //    string secretCode = GenerateShortCode(16);

        //    var shortenedUrl = new ShortenedUrl
        //    {
        //        OriginalUrl = originalUrl,
        //        ShortCode = shortCode,
        //    };

        //    context.ShortenedUrls.Add(shortenedUrl);
        //    context.SaveChanges();

        //    string baseUrl = $"{Request.Scheme}://{Request.Host}";
        //    string shortenedUrlLink = $"{baseUrl}/Url/Redirect/{shortCode}";
        //    string secretStatsUrl = $"{baseUrl}/Url/Stats/{shortenedUrl.Id}?secret={secretCode}";

        //    ViewBag.ShortenedUrl = shortenedUrlLink;
        //    ViewBag.SecretStatsUrl = secretStatsUrl;

        //    return View("Index");
        //}
    };
}
