using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UrlsController : ControllerBase
    {
        private readonly UrlShorteningService _shorteningService;
        private readonly UrlCacheService _cacheService;
        private readonly UrlAccessService _accessService;

        private const int DefaultPage = 1;
        private const int DefaultPageSize = 6;

        public UrlsController(
            UrlShorteningService shorteningService,
            UrlCacheService cacheService,
            UrlAccessService accessService)
        {
            _shorteningService = shorteningService;
            _cacheService = cacheService;
            _accessService = accessService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateShortUrlRequest request)
        {
            if (string.IsNullOrEmpty(request.OriginalUrl))
            {
                return BadRequest("Original URL is required.");
            }

            try
            {
                var shortUrl = await _shorteningService.CreateShortenedUrl(request.OriginalUrl, request.CustomShortId, request.TTL);
                _cacheService.CacheUrl(shortUrl.ShortUrlId, shortUrl.OriginalUrl);
                return CreatedAtAction(nameof(GetById), new { shortUrlId = shortUrl.ShortUrlId }, shortUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the short URL: {ex.Message}");
            }
        }

        [HttpGet("{shortUrlId}")]
        public async Task<IActionResult> GetById(string shortUrlId)
        {
            if (string.IsNullOrEmpty(shortUrlId))
            {
                return BadRequest("Short URL ID is required.");
            }

            try
            {
                var originalUrl = _cacheService.GetCachedUrl(shortUrlId);
                if (originalUrl == null)
                {
                    var url = await _shorteningService.GetUrl(shortUrlId);
                    if (url == null) return NotFound("Short URL not found.");
                    originalUrl = url.OriginalUrl;
                    _cacheService.CacheUrl(shortUrlId, originalUrl);
                }

                await _accessService.RecordAccess(shortUrlId);
                return Redirect(originalUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the original URL: {ex.Message}");
            }
        }
    
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = DefaultPage, [FromQuery] int pageSize = DefaultPageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page and page size must be greater than zero.");
            }

            try
            {
                var urls = await _shorteningService.GetPaginatedShortenedUrls(page, pageSize);
                var totalCount = await _shorteningService.GetTotalUrlCount();
                var response = new
                {
                    Urls = urls,
                    TotalCount = totalCount,
                    CurrentPage = page,
                    PageSize = pageSize
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the URLs: {ex.Message}");
            }
        }
    
        [HttpDelete("{shortUrlId}")]
        public async Task<IActionResult> Delete(string shortUrlId)
        {
            if (string.IsNullOrEmpty(shortUrlId))
            {
                return BadRequest("Short URL ID is required.");
            }

            try
            {
                bool deleted = await _shorteningService.DeleteShortenedUrl(shortUrlId);
                if (!deleted)
                {
                    return NotFound("URL not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the URL: {ex.Message}");
            }
        }
    }
}