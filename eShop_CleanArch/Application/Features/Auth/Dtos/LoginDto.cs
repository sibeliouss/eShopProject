namespace Application.Features.Auth.Dtos;

public record LoginDto(
    string UserNameOrEmail,
    string Password,
    bool RememberMe = false);