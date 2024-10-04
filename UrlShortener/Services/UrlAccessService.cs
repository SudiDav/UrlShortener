using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;

namespace UrlShortener.Services;

public class UrlAccessService(AppDbContext context)
{
    public async Task RecordAccess(string shortId)
    {
        var url = await context.ShortenedUrls.FirstOrDefaultAsync(u => u.ShortUrlId == shortId);
        if (url != null)
        {
            url.AccessCount++;
            await context.SaveChangesAsync();
        }
    }
}