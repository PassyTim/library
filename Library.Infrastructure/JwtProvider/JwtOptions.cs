namespace Library.Infrastructure.JwtProvider;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresMin { get; set; }
}