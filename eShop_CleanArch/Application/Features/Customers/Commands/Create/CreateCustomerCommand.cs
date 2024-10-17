using Application.Features.Customers.Dtos;
using Application.Services.Customers;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Application.Features.Customers.Rules;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.Commands.Create
{
    public class CreateCustomerCommand : IRequest<CreatedCustomerResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreatedCustomerResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerBusinessRules _customerBusinessRules;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<CreateCustomerCommand> _validator;

        public CreateCustomerCommandHandler(
            ICustomerRepository customerRepository, 
            UserManager<User> userManager, 
            IMapper mapper, 
            CustomerBusinessRules customerBusinessRules,
            ICustomerService customerService, IValidator<CreateCustomerCommand> validator)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _customerBusinessRules = customerBusinessRules;
            _userManager = userManager;
            _validator = validator;
        }

        public async Task<CreatedCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
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
            await _customerBusinessRules.PasswordsShouldMatch(request.Password, request.ConfirmedPassword); 
            
            // Kullanıcıyı oluşturma
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
               
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new Exception("Kullanıcı oluşturulurken hata oluştu: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            var customer = _mapper.Map<Customer>(request);
            customer.UserId = user.Id;

            await _customerRepository.AddAsync(customer);

            var response = _mapper.Map<CreatedCustomerResponse>(customer);
            return response;
        }

    }
}
