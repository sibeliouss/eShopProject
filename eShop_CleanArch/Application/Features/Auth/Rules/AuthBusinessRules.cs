using Application.Features.Auth.Constants;
using Domain.Entities;

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
    
}

public class BusinessException(string message) : Exception(message);