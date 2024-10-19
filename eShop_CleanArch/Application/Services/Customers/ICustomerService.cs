/*using Application.Features.Customers.Dtos;
using Domain.Entities;

namespace Application.Services.Customers;

public interface ICustomerService
{
    Task<Customer> CreateCustomerAsync(CreateCustomerDto customerDto);
    Task UpdateCustomerInformationAsync(UpdateCustomerInformationDto request);
    Task UpdateCustomerPasswordAsync(UpdateCustomerPasswordDto request);
    Task<Customer?> GetCustomerByIdAsync(Guid customerId);
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task DeleteAccountAsync(Guid customerId, string password);
}*/