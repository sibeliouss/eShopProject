using Application.Features.Auth.Rules;
using Application.Features.Customers.Dtos;
using Application.Services.Customers;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
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
    private readonly IValidator<RegisterCommand> _validator;

    public RegisterCommandHandler(
        AuthBusinessRules authBusinessRules, 
        UserManager<User> userManager, 
        ICustomerService customerService, 
        IMapper mapper, 
        IValidator<RegisterCommand> validator)
    {
        _authBusinessRules = authBusinessRules;
        _userManager = userManager;
        _customerService = customerService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var userNameExist = await _userManager.Users.AnyAsync(p => p.UserName == request.UserName, cancellationToken);
        var emailExist = await _userManager.Users.AnyAsync(p => p.Email == request.Email, cancellationToken);

        if (userNameExist || emailExist)
        {
            throw new Exception("Kullanıcı adı veya email zaten mevcut!");
        }

        // Şifrelerin eşleştiğinden emin olun
        await _authBusinessRules.PasswordsShouldMatch(request.Password, request.ConfirmedPassword);

        // Kullanıcı oluşturma
        var user = _mapper.Map<User>(request);
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var customerDto = new CreateCustomerDto(
                user.FirstName,
                user.LastName,
                user.Email,
                user.Id,
                user.UserName,
                user.PasswordHash // Şifre hash'lenmiş olarak otomatik gelir
            );
            await _customerService.CreateCustomerAsync(customerDto);
        }
        else
        {
            await _userManager.DeleteAsync(user);
            throw new Exception("Kullanıcı oluşturulurken bir hata oluştu.");
        }
    }
}

    

}