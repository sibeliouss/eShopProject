using Application.Features.Customers.Rules;
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
        private readonly UserManager<User> _userManager;
        private readonly CustomerBusinessRules _customerBusinessRules;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository, UserManager<User> userManager, CustomerBusinessRules customerBusinessRules)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
            _customerBusinessRules = customerBusinessRules;
        }

        public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            
            await _customerBusinessRules.CustomerShouldBeExistsWhenSelected(customer);
            _customerBusinessRules.CustomerPasswordShouldBeVerified(customer, request.Password);
            await _customerRepository.DeleteAsync(customer);

                // Kullanıcıyı da sil
                var user = await _userManager.FindByIdAsync(customer.UserId.ToString());
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        throw new Exception($"Kullanıcı silinirken bir hata oluştu: {errors}");
                    }
                }
            
        }

    }

