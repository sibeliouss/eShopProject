using Application.Features.BillingAddresses.Constants;
using Application.Services.Repositories;

namespace Application.Features.BillingAddresses.Rules;

public class BillingAddressBusinessRules
{
    private readonly IBillingAddressRepository _billingAddressRepository;

    public BillingAddressBusinessRules(IBillingAddressRepository billingAddressRepository)
    {
        _billingAddressRepository = billingAddressRepository;
    }
    
    public async Task BillingAddressShouldNotExistForCustomer(Guid customerId)
    {
        var addressExists = await _billingAddressRepository.AnyAsync(a => a.CustomerId == customerId);
        if (addressExists)
        {
            throw new Exception(BillingAddressMessages.AddressAlreadyExistsForCustomer);
        }
    }
}