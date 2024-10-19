using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Auth;

public class JwtService
{
    public string CreateToken(User user, List<string?>? roles, bool rememberMe)
    {
        var token = string.Empty;

        List<Claim> claims = new();
        claims.Add(new("UserId", user.Id.ToString()));
        claims.Add(new("Name", user.GetName()));
        claims.Add(new("Email", user.Email ?? string.Empty));
        claims.Add(new("UserName", user.UserName ?? string.Empty));
        if (roles is not null) claims.Add(new("Roles", string.Join(",", roles)));

        DateTime expires = rememberMe ? DateTime.Now.AddMonths(1) : DateTime.Now.AddDays(1);

        JwtSecurityToken jwtSecurityToken = new(
            issuer: "Sibel Öztürk",
            audience: "eShop Project",
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(3), //şu andan itibaren kullan. 3 saat ile sınırla
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("StrongAndSecretKeyStrongAndSecretKeyStrongAndSecretKeyStrongAndSecretKey")), SecurityAlgorithms.HmacSha512));

        JwtSecurityTokenHandler handler = new();
        token = handler.WriteToken(jwtSecurityToken);

        return token;
    }
}