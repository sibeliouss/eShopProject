using Application.Features.Addresses.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.Addresses.Commands.Create;

public class CreateAddressCommand : IRequest<CreatedAddressResponse>
{
    public Guid CustomerId { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string ContactName { get; set; }
    public string Description { get; set; } //istenirse dto içinde de tanımlanabilir.

    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, CreatedAddressResponse>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly AddressBusinessRules _addressBusinessRules;
        private readonly IValidator<CreateAddressCommand> _validator;

        public CreateAddressCommandHandler(IAddressRepository addressRepository, IMapper mapper, AddressBusinessRules addressBusinessRules, IValidator<CreateAddressCommand> validator)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _addressBusinessRules = addressBusinessRules;
            _validator = validator;
        }
        
        public async Task<CreatedAddressResponse> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _addressBusinessRules.AddressShouldNotExistForCustomer(request.CustomerId);
            
            var address = _mapper.Map<Address>(request);
            
            await _addressRepository.AddAsync(address);
            
            return _mapper.Map<CreatedAddressResponse>(address);
            
        }
        
    }
    
}