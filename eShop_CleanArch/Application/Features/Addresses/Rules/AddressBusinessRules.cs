using Application.Features.Addresses.Constants;
using Application.Services.Repositories;
using Domain.Entities;

namespace Application.Features.Addresses.Rules;

public class AddressBusinessRules
{
    private readonly IAddressRepository _addressRepository;

    public AddressBusinessRules(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    private static void ThrowBusinessException(string message)
    {
        throw new BusinessException(message);
    }

    public async Task AddressShouldNotExistForCustomer(Guid userId)
    {
        var addressExists = await _addressRepository.AnyAsync(a => a.UserId == userId);
        if (addressExists)
        {
            ThrowBusinessException(AddressMessages.AddressAlreadyExistsForCustomer);
        }
    }
    public async Task AddressShouldExistWhenSelected(Address? address)
    {
        if (address == null)
        {
            ThrowBusinessException(AddressMessages.AddressNotFound);
        }
    }
    
}

public class BusinessException(string message) : Exception(message);