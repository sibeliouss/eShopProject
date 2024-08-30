using Application.Features.Auth.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Rules;

public class AuthBusinessRules
{
    private static Task ThrowBusinessException(string message)
    {
        throw new BusinessException(message);
    }
    
    public async Task UserShouldBeExistsWhenSelected(User? user)
    {
        if (user == null)
            await ThrowBusinessException(AuthMessages.UserDontExists);
    }
    
    public async Task UserShouldHaveCorrectPassword(User appUser, string enteredPassword)
    {
        var passwordHasher = new PasswordHasher<User>();
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash!, enteredPassword);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            await ThrowBusinessException(AuthMessages.PasswordIncorrect); 
        }
    }
    
    

    
}

public class BusinessException(string message) : Exception(message);