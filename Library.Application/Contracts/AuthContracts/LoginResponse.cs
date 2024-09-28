namespace Library.Application.Contracts.AuthContracts;

public class LoginResponse
{
    public ResponseUser User { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}