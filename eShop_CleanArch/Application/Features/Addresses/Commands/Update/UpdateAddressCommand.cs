using Application.Features.Addresses.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.Addresses.Commands.Update;

public class UpdateAddressCommand : IRequest<UpdatedAddressResponse>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string ContactName { get; set; }
    public string Description { get; set; }  
    
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, UpdatedAddressResponse>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly AddressBusinessRules _addressBusinessRules;
        private readonly IValidator<UpdateAddressCommand> _validator;

        public UpdateAddressCommandHandler(IAddressRepository addressRepository, IMapper mapper, AddressBusinessRules addressBusinessRules, IValidator<UpdateAddressCommand> validator)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _addressBusinessRules = addressBusinessRules;
            _validator = validator;
        }
        public async Task<UpdatedAddressResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var address = await _addressRepository.GetByIdAsync(request.Id);
            await _addressBusinessRules.AddressShouldExistWhenSelected(address);

            address = _mapper.Map(request, address);

            if (address != null) await _addressRepository.UpdateAsync(address);

            return _mapper.Map<UpdatedAddressResponse>(address);
            
        }
    }
}