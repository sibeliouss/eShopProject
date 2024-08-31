using Application.Features.Auth.Rules;
using Application.Features.Customers.Dtos;
using Application.Services.Customers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Register;

public class RegisterCommand : IRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConfirmedPassword { get; set; }
    

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
        {
            private readonly UserManager<User> _userManager;
            private readonly ICustomerService _customerService;
            private readonly AuthBusinessRules _authBusinessRules;
            private readonly IMapper _mapper;
            public RegisterCommandHandler(AuthBusinessRules authBusinessRules, UserManager<User> userManager, ICustomerService customerService, IMapper mapper)
            {
                _authBusinessRules = authBusinessRules;
                _userManager = userManager;
                _customerService = customerService;
                _mapper = mapper;
            }
            public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                var userNameExist = await _userManager.Users.AnyAsync(p => p.UserName == request.UserName, cancellationToken: cancellationToken);
                var emailExist = await _userManager.Users.AnyAsync(p => p.Email == request.Email, cancellationToken: cancellationToken);

                if (userNameExist || emailExist)
                {
                    throw new Exception("Kullanıcı adı veya email zaten mevcut!");
                }
                //Rule 1
                await _authBusinessRules.PasswordsShouldMatch(request.Password, request.ConfirmedPassword);

                var passwordHasher = new PasswordHasher<User>();
                var hashedPassword = passwordHasher.HashPassword(null, request.Password);

                var user = _mapper.Map<User>(request);
                /*var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    EmailConfirmed = true,
                    UserName = request.UserName,
                    PasswordHash = hashedPassword
                };*/

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    /*var customerDto = new CreateCustomerDto(
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.Id,
                        user.UserName,
                        user.PasswordHash
                    );*/
                    var customerDto = _mapper.Map<CreateCustomerDto>(user);
                    await _customerService.CreateCustomerAsync(customerDto);
                }
                else
                {
                    throw new Exception("Kullanıcı oluşturulurken bir hata oluştu.");
                }
            }
        }
    

}