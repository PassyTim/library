namespace Library.Application.Exceptions;

[Serializable]
public class LoginException : AuthenticationException
{
    public LoginException() { }

    public LoginException(string message) : base(message) { }

    public LoginException(string message, Exception innerException) : base(message, innerException) { }
}