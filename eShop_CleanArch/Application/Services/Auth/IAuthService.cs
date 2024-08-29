using Application.Features.Auth.Dtos;

namespace Application.Services.Auth;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto request);
    Task RegisterAsync(RegisterDto request);
}