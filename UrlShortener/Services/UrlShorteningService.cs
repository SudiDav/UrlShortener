using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Entities;

namespace UrlShortener.Services;

public class UrlShorteningService(AppDbContext context, Base62IdGenerator idGenerator)
{
    public async Task<ShortenedUrl> CreateShortenedUrl(string originalUrl, string customShortId = null, int? ttl = null)
    {
        var shortUrlId = customShortId ?? idGenerator.GenerateShortId();

        // Ensure the generated or provided ID is unique
        while (context.ShortenedUrls.Any(u => u.ShortUrlId == shortUrlId))
        {
            if (customShortId != null)
            {
                throw new ArgumentException("Custom short ID already in use.");
            }
            shortUrlId = idGenerator.GenerateShortId(); // Regenerate if there's a collision
        }

        var shortenedUrl = new ShortenedUrl
        {
            OriginalUrl = originalUrl,
            ShortUrlId = shortUrlId,
            ExpiryDate = ttl.HasValue ? DateTime.UtcNow.AddMinutes(ttl.Value) : null
        };

        context.ShortenedUrls.Add(shortenedUrl);
        await context.SaveChangesAsync();

        return shortenedUrl;
    }

    public async Task<ShortenedUrl> GetUrl(string shortId)
    {
        return await context.ShortenedUrls
            .Where(u => u.ShortUrlId == shortId)
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<ShortenedUrl>> GetAllShortenedUrls()
    {
        return await context.ShortenedUrls.ToListAsync();
    }
    public async Task<bool> DeleteShortenedUrl(string id)
    {
        var url = await context.ShortenedUrls.FirstOrDefaultAsync(u => u.ShortUrlId == id);
        if (url == null)
        {
            return false;
        }

        context.ShortenedUrls.Remove(url);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ShortenedUrl>> GetPaginatedShortenedUrls(int page, int pageSize)
    {
        return await context.ShortenedUrls
            .OrderByDescending(u => u.AccessCount)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalUrlCount()
    {
        return await context.ShortenedUrls.CountAsync();
    }
}