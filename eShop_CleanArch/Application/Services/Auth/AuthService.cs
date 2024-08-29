using Application.Features.Auth.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtService _jwtService;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }
    public async Task<string> LoginAsync(LoginDto request)
    {
        var appUserByUsername = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.UserNameOrEmail);
        var appUserByEmail = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.UserNameOrEmail);

        var appUser = appUserByUsername ?? appUserByEmail;

        if (appUser is null)
        {
            throw new Exception( "Kullanıcı bulunamadı!" );
        }

        var passwordHasher = new PasswordHasher<User>();
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash!, request.Password);

        if(passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new Exception("Şifreniz yanlış!");
        }
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, request.Password, true);

        if (result.IsLockedOut)
        {
            var timeSpan = appUser.LockoutEnd - DateTime.UtcNow;
            if(timeSpan is not null)
                throw new Exception($"Kullanıcı art arda 3 kere yanlış şifre girişinden dolayı sistem {Math.Ceiling(timeSpan.Value.TotalMinutes)} dakika kilitlenmiştir." );
        }

        return _jwtService.CreateToken(appUser, null, request.RememberMe);
    }
    public async Task RegisterAsync(RegisterDto request)
    {
        bool userNameExist = await _userManager.Users.AnyAsync(p => p.UserName == request.UserName);
        bool emailExist = await _userManager.Users.AnyAsync(p => p.Email == request.Email);

        if (!userNameExist && !emailExist)
        {
            if (request.Password != request.ConfirmedPassword)
            {
                throw new Exception("Başarısız kayıt işlemi. Şifreler uyuşmuyor!");
            }
        }

        var passwordHasher = new PasswordHasher<User>();
        var hashedPassword = passwordHasher.HashPassword(null!, request.Password);

        User user = new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            EmailConfirmed = true,
            UserName = request.UserName,
            PasswordHash = hashedPassword,
        };

       await _userManager.CreateAsync(user, request.Password);

        
    }
}