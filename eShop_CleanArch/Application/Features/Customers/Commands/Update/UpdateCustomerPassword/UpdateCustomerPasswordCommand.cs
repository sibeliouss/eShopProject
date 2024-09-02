using Application.Features.Customers.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Customers.Commands.Update.UpdateCustomerPassword
{
    public class UpdateCustomerPasswordCommand : IRequest
    {
        public UpdateCustomerPasswordDto UpdateCustomerPasswordDto { get; set; }
    }

    public class UpdateCustomerPasswordCommandHandler : IRequestHandler<UpdateCustomerPasswordCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<User> _userManager; 

        public UpdateCustomerPasswordCommandHandler(ICustomerRepository customerRepository, UserManager<User> userManager)
        {
            _customerRepository = customerRepository;
            _userManager = userManager; 
        }

        public async Task Handle(UpdateCustomerPasswordCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.UpdateCustomerPasswordDto.Id);
            if (customer == null)
            {
                throw new Exception("Kayıt Bulunamadı!");
            }
            
            var user = await _userManager.FindByIdAsync(customer.UserId.ToString());
            if (user == null)
            {
                throw new Exception("Kullanıcı Bulunamadı!");
            }
            
            var passwordVerificationResult = await _userManager.CheckPasswordAsync(user, request.UpdateCustomerPasswordDto.CurrentPassword);
            if (!passwordVerificationResult)
            {
                throw new Exception("Mevcut şifre yanlış!");
            }
            
            if (request.UpdateCustomerPasswordDto.NewPassword != request.UpdateCustomerPasswordDto.ConfirmedPassword)
            {
                throw new Exception("Yeni şifreler uyuşmuyor!");
            }
            
            var result = await _userManager.ChangePasswordAsync(user, request.UpdateCustomerPasswordDto.CurrentPassword, request.UpdateCustomerPasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                throw new Exception("Şifre güncellenirken bir hata oluştu.");
            }
            
            customer.PasswordHash = user.PasswordHash;
            await _customerRepository.UpdateAsync(customer);
        }
    }
}
