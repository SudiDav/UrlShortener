namespace UrlShortener.Models;

public class CreateShortUrlRequest
{
    public string OriginalUrl { get; set; }
    public string CustomShortId { get; set; }
    public int TTL { get; set; }
}