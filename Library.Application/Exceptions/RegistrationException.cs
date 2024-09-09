namespace Library.Application.Exceptions;

[Serializable]
public class RegistrationException : AuthenticationException
{
    public RegistrationException() { }

    public RegistrationException(string message) : base(message) { }

    public RegistrationException(string message, Exception innerException) : base(message, innerException) { }
}