namespace CoTuongBackend.Domain.Helpers;

public static class RandomStringGenerator
{
    private static readonly Random _random = new();
    public static string Generate(string chars, int length)
        => new(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
}
