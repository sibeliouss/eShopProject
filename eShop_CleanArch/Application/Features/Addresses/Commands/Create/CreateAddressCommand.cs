using Application.Features.Addresses.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
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

        public CreateAddressCommandHandler(IAddressRepository addressRepository, IMapper mapper, AddressBusinessRules addressBusinessRules)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _addressBusinessRules = addressBusinessRules;
        }
        
        public async Task<CreatedAddressResponse> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            await _addressBusinessRules.AddressShouldNotExistForCustomer(request.CustomerId);

            /*var addressExists = await _addressRepository.AnyAsync(a => a.CustomerId == request.CustomerId);

            if (addressExists)
            {
                throw new Exception("Kullanıcıya ait adres kaydı zaten var.");
            }*/
            
            Address address = _mapper.Map<Address>(request);
            
            await _addressRepository.AddAsync(address);
            
            return _mapper.Map<CreatedAddressResponse>(address);
            
        }
        
    }
    
}