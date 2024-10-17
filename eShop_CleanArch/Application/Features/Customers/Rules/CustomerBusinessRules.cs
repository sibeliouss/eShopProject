using Application.Features.Customers.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Customers.Rules;

public class CustomerBusinessRules
{
    private static Task ThrowBusinessException(string message)
    {
        throw new BusinessException(message);
    }
    
    public async Task CustomerShouldBeExistsWhenSelected(Customer? customer)
    {
        if (customer == null)
            await ThrowBusinessException(CustomerMessages.CustomerDontExists);
    }

    public void CustomerPasswordShouldBeVerified(Customer customer, string password)
    {
        var passwordHasher = new PasswordHasher<Customer>();

        if (customer.PasswordHash != null)
        {
            var passwordVerificationResult =
                passwordHasher.VerifyHashedPassword(customer, customer.PasswordHash, password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                throw new BusinessException(CustomerMessages.PasswordIncorrect);
            }
        }
    }
    
    public async Task PasswordsShouldMatch(string password, string confirmedPassword)
    {
        if (password != confirmedPassword)
        {
            await ThrowBusinessException(CustomerMessages.PasswordsDoNotMatch);
        }
    }
}

public class BusinessException(string message) : Exception(message);