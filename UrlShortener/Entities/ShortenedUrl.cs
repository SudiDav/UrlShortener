namespace UrlShortener.Entities;

public class ShortenedUrl
{
    public Guid Id { get; set; }
    public string OriginalUrl { get; set; }
    public string ShortUrlId { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int AccessCount { get; set; }
}