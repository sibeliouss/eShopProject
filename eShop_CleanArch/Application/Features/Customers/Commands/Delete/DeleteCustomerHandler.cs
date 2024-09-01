using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Customers.Commands.Delete;

public class DeleteCustomerCommand : IRequest
{
    public Guid CustomerId { get; set; }
    public string Password { get; set; }
}

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null)
        {
            throw new Exception("Kullanıcı bulunamadı.");
        }

        var passwordHasher = new PasswordHasher<Customer>();
        if (customer.PasswordHash != null)
        {
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(customer, customer.PasswordHash, request.Password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                throw new Exception("Şifre yanlış!");
            }
        }

        await _customerRepository.DeleteAsync(customer);
    }
}