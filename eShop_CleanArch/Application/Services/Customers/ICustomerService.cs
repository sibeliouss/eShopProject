using Application.Features.Customers.Dtos;
using Domain.Entities;

namespace Application.Services.Customers;

public interface ICustomerService
{
    Task CreateCustomerAsync(CreateCustomerDto createCustomerDto);
    Task UpdateCustomerInformationAsync(UpdateCustomerInformationDto request);
    Task UpdateCustomerPasswordAsync(UpdateCustomerPasswordDto request);
    Task<Customer?> GetCustomerByIdAsync(Guid customerId);
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task DeleteAccountAsync(Guid customerId, string password);
}