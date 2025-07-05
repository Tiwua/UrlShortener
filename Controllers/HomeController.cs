using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UrlShortener.Data;
using UrlShortener.Models;
using System.Net;

namespace UrlShortener.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly AppDbContext context;

        public HomeController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Shorten(string originalUrl)
        {
            if (string.IsNullOrWhiteSpace(originalUrl) || !Uri.IsWellFormedUriString(originalUrl, UriKind.Absolute))
            {
                ModelState.AddModelError("", "Please enter a valid URL.");
                return View("Index");
            }

            string existingShortCode = await this.context.ShortenedUrls
                .Where(u => u.OriginalUrl == originalUrl)
                .Select(u => u.ShortCode)
                .FirstOrDefaultAsync();

            string secretId = await this.context.ShortenedUrls
                .Where(u => u.OriginalUrl == originalUrl)
                .Select(u => u.Id.ToString())
                .FirstOrDefaultAsync();

            if (existingShortCode != null)
            {

                string code = $"{originalUrl}/Url/Redirect/{existingShortCode}";
                string id = $"{originalUrl}/Url/Stats/{secretId}?secret={existingShortCode}";

                ViewBag.ShortenedUrl = code;
                ViewBag.SecretStatsUrl = id;

                return View("Index");
            }

            string shortCode = GenerateShortCode(4);
            string secretCode = GenerateShortCode(8);


            string baseUrl;
            string shortenedUrlLink;

             baseUrl = $"{Request.Scheme}://{Request.Host}";
                shortenedUrlLink = $"{baseUrl}/Url/Redirect/{shortCode}";
                string secretStatsUrl = $"{baseUrl}/Url/Stats/?secret={secretCode}";

            var shortenedUrl = new ShortenedUrl
            {
                OriginalUrl = originalUrl,
                ShortCode = shortCode,
                SecretUrl = $"{baseUrl}/Url/Stats/?secret={secretCode}",
                CreatedAt = DateTime.UtcNow
            };
                await context.ShortenedUrls.AddAsync(shortenedUrl);
                await context.SaveChangesAsync();
                ViewBag.ShortenedUrl = shortenedUrlLink;
                ViewBag.SecretStatsUrl = secretStatsUrl;

                return View("Index");

        }

        private string GenerateShortCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpGet("/Url/Redirect/{shortCode}")]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            if (string.IsNullOrWhiteSpace(shortCode))
            {
                return NotFound();
            }

            var shortenedUrl = await this.context.ShortenedUrls
                .FirstOrDefaultAsync(u => u.ShortCode == shortCode);

            if (shortenedUrl == null)
            {
                return NotFound();
            }

            string ipAddress = Request.HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR") ??
                               Request.HttpContext.Connection.RemoteIpAddress?.ToString() ??
                               "Unknown";

            if (string.IsNullOrWhiteSpace(ipAddress) || ipAddress == "::1")
            {
                ipAddress = string.Empty;

            }

            string name = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(name);

            System.Net.IPAddress[] addresses = hostEntry.AddressList;
            try
            {
                foreach(IPAddress address in addresses)
                {
                    if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipAddress = address.ToString();
                    }
                }

                if (string.IsNullOrEmpty(ipAddress))
                {
                  ipAddress = addresses[addresses.Length - 1].ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }

            var accessLog = new UrlAccessLog
            {
                ShortenedUrlId = shortenedUrl.Id,
                IpAddress = ipAddress,
                AccessedAt = DateTime.UtcNow,
                SecretUrl = shortenedUrl.SecretUrl
            };

            await this.context.UrlAccessLogs.AddAsync(accessLog);
            await this.context.SaveChangesAsync();

            return Redirect(shortenedUrl.OriginalUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Stats(string secretId)
        {
            var shortenedUrl = await this.context.ShortenedUrls
                .Include(u => u.AccessLogs)
                .FirstOrDefaultAsync(u => u.SecretUrl == secretId);

            if (shortenedUrl == null)
            {
                return NotFound();
            }

            var uniqueVisitsPerDay = shortenedUrl.AccessLogs
                .GroupBy(a => a.AccessedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    UniqueVisitors = g.Select(a => a.IpAddress).Distinct().Count()
                })
                .OrderBy(x => x.Date)
                .ToList();

            var topIps = shortenedUrl.AccessLogs
                .GroupBy(a => a.IpAddress)
                .Select(g => new
                {
                    IpAddress = g.Key,
                    VisitCount = g.Count()
                })
                .OrderByDescending(x => x.VisitCount)
                .Take(10)
                .ToList();

            ViewBag.UniqueVisitsPerDay = uniqueVisitsPerDay;
            ViewBag.TopIps = topIps;
            ViewBag.OriginalUrl = shortenedUrl.OriginalUrl;

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
