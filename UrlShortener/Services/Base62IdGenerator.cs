namespace UrlShortener.Services;

public class Base62IdGenerator
{
    private static readonly string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private readonly Random _random = new Random();

    public string GenerateShortId(int length = 8)
        => new string(Enumerable.Repeat(Base62Chars, length).Select(s 
            => s[_random.Next(s.Length)]).ToArray());
}