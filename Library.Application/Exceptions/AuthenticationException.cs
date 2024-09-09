namespace Library.Application.Exceptions;

public class AuthenticationException : Exception
{
    protected AuthenticationException() { }

    protected AuthenticationException(string message) : base(message) { }

    protected AuthenticationException(string message, Exception innerException) : base(message, innerException) { }
}