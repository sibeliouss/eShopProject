namespace Application.Features.Auth.Login;

public class LoginResponse 
{
    public string Token { get; set; }
    public string? UserName { get; set; }
    public Guid UserId { get; set; }
    public DateTime Expiration { get; set; }
}