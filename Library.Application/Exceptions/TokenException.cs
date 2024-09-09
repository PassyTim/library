namespace Library.Application.Exceptions;

public class TokenException : Exception
{
    public TokenException() { }

    public TokenException(string message) : base(message) { }
}