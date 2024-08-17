using System.Text.RegularExpressions;

namespace Library.Application;

public static class IsbnValidator
{
    public static bool Validate(string isbn)
    {
        string pattern =
            "^(?=(?:\\D*\\d){10}(?:(?:\\D*\\d){3})?$)[\\d-]+$";
        
        Regex regex = new Regex(pattern);
        var matches = regex.Matches(isbn);
            
        if (matches.Count > 0) return true;
        return false;
    }
}