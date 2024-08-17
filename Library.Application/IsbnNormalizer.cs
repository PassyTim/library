namespace Library.Application;

public static class IsbnNormalizer
{
    public static string NormalizeIsbn(string isbn)
    {
        var normalizedIsbn = new string(isbn.Where(char.IsDigit).ToArray());

        return normalizedIsbn;
    }
}