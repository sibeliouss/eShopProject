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

        public CreateAddressCommandHandler(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }
        
        public async Task<CreatedAddressResponse> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var addressExists = await _addressRepository.AnyAsync(a => a.CustomerId == request.CustomerId);

            if (addressExists)
            {
                throw new Exception("Kullanıcıya ait adres kaydı zaten var.");
            }
            
            Address address = _mapper.Map<Address>(request);
            
            /*Address address = new Address()
            {
                CustomerId = request.CustomerId,
                Country = request.Country,
                City = request.City,
                ZipCode = request.ZipCode,
                ContactName = request.ContactName,
                Description = request.Description

            };*/
            await _addressRepository.AddAsync(address);
            
            return _mapper.Map<CreatedAddressResponse>(address);
            /*return new CreatedAddressResponse()
            {
             Id= address.Id,
             CustomerId = address.CustomerId,
             Country = address.Country,
             City = address.City,
             ZipCode = address.ZipCode,
             ContactName = address.ContactName,
             Description = address.Description
            };*/
        }
        
    }
    
}