using Application.Features.Customers.Dtos;
using Application.Services.Customers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Register;

public class RegisterCommand : IRequest
{
    public RegisterDto RegisterDto { get; set; }
    
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly ICustomerService _customerService;

        public RegisterCommandHandler(UserManager<User> userManager, ICustomerService customerService)
        {
            _userManager = userManager;
            _customerService = customerService;
        }

        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RegisterDto;

            bool userNameExist = await _userManager.Users.AnyAsync(p => p.UserName == dto.UserName);
            bool emailExist = await _userManager.Users.AnyAsync(p => p.Email == dto.Email);

            if (userNameExist || emailExist)
            {
                throw new Exception("Kullanıcı adı veya email zaten mevcut!");
            }

            if (dto.Password != dto.ConfirmedPassword)
            {
                throw new Exception("Şifreler uyuşmuyor!");
            }

            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(null, dto.Password);

            User user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                EmailConfirmed = true,
                UserName = dto.UserName,
                PasswordHash = hashedPassword
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                var customerDto = new CreateCustomerDto(
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Id,
                    user.UserName,
                    user.PasswordHash
                );

                await _customerService.CreateCustomerAsync(customerDto);
            }
            else
            {
                throw new Exception("Kullanıcı oluşturulurken bir hata oluştu.");
            }
            
        }
    }
}