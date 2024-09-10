using Application.Features.Auth.Rules;
using Application.Services.Auth;
using AutoMapper;
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
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtService _jwtService;
       

        public LoginCommandHandler(AuthBusinessRules authBusinessRules, UserManager<User> userManager,
            SignInManager<User> signInManager,
            JwtService jwtService)
        {
            _authBusinessRules = authBusinessRules;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }
        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var appUserByUsername =
                await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.UserNameOrEmail, cancellationToken: cancellationToken);
            var appUserByEmail = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.UserNameOrEmail, cancellationToken: cancellationToken);
            var appUser = appUserByUsername ?? appUserByEmail;
            //Rule 1
            await _authBusinessRules.UserShouldBeExistsWhenSelected(appUser);
            //Rule 2
            if (appUser != null)
                await AuthBusinessRules.UserShouldHaveCorrectPassword(appUser, request.Password);

            if (appUser != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(appUser, request.Password, true);

                if (result.IsLockedOut)
                {
                    var timeSpan = appUser.LockoutEnd - DateTime.UtcNow;
                    if (timeSpan is not null)
                    {
                        throw new Exception(
                            $"Kullanıcı art arda 3 kere yanlış şifre girişinden dolayı sistem {Math.Ceiling(timeSpan.Value.TotalMinutes)} dakika kilitlenmiştir.");
                    }
                }
            } 
            var token = _jwtService.CreateToken(appUser, null, request.RememberMe);
                return new LoginResponse
                {
                    Token = token,
                    UserName = appUser.UserName,
                    UserId = appUser.Id,
                    Expiration = DateTime.UtcNow
                };
                
        }
    } 
}
