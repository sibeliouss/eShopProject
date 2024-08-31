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
    
    public static async Task UserShouldHaveCorrectPassword(User user, string enteredPassword)
    {
        var passwordHasher = new PasswordHasher<User>();
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, enteredPassword);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            await ThrowBusinessException(AuthMessages.PasswordIncorrect); 
        }
    }
   
    public async Task PasswordsShouldMatch(string password, string confirmedPassword)
    {
        if (password != confirmedPassword)
        {
            await ThrowBusinessException(AuthMessages.PasswordsDoNotMatch);
        }
    }
    
}

public class BusinessException(string message) : Exception(message);