using Application.Services.Auth;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Login;

public class LoginCommand : IRequest<LoginResponse>
{
    public string UserNameOrEmail { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
    
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtService _jwtService;

    public LoginCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var appUserByUsername = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.UserNameOrEmail);
        var appUserByEmail = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.UserNameOrEmail);

        var appUser = appUserByUsername ?? appUserByEmail;

        if (appUser is null)
        {
            throw new Exception("Kullanıcı bulunamadı!");
        }

        var passwordHasher = new PasswordHasher<User>();
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash!, request.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new Exception("Şifreniz yanlış!");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(appUser, request.Password, true);

        if (result.IsLockedOut)
        {
            var timeSpan = appUser.LockoutEnd - DateTime.UtcNow;
            if (timeSpan is not null)
            {
                throw new Exception($"Kullanıcı art arda 3 kere yanlış şifre girişinden dolayı sistem {Math.Ceiling(timeSpan.Value.TotalMinutes)} dakika kilitlenmiştir.");
            }
        }

        var token = _jwtService.CreateToken(appUser, null, request.RememberMe);

        return new LoginResponse
        {
            Token = token,
            UserName = appUser.UserName,
            UserId = appUser.Id,
            Expiration = DateTime.UtcNow.AddHours(1)  
        };
    }
}

}