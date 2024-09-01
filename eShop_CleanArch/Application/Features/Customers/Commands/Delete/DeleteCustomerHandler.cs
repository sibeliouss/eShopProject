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

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository, UserManager<User> userManager)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
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
                var passwordVerificationResult =
                    passwordHasher.VerifyHashedPassword(customer, customer.PasswordHash, request.Password);
                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    throw new Exception("Şifre yanlış!");
                }
            }

            await _customerRepository.DeleteAsync(customer);

            // Kullanıcıyı da sil
            var user = await _userManager.FindByIdAsync(customer.Id.ToString());
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

