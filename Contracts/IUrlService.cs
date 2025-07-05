using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Contracts
{
    public interface IUrlService
    {
        public Task<IActionResult> Shorten(string originalUrl);
    }
}
