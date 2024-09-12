using Application.Features.Addresses.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Addresses.Queries;

public class GetAddressQuery : IRequest<GetAddressQueryResponse>
{
    public Guid CustomerId { get; set; }

    public class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, GetAddressQueryResponse>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly AddressBusinessRules _addressBusinessRules;
        private readonly IMapper _mapper;

        public GetAddressQueryHandler(IAddressRepository addressRepository, AddressBusinessRules addressBusinessRules, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _addressBusinessRules = addressBusinessRules;
            _mapper = mapper;
        }

        public async Task<GetAddressQueryResponse> Handle(GetAddressQuery request, CancellationToken cancellationToken)
        {
            var address = await _addressRepository
                .Query()
                .Where(a => a.CustomerId == request.CustomerId)
                .FirstOrDefaultAsync(cancellationToken);

            await _addressBusinessRules.AddressShouldExistWhenSelected(address);
            /*if (address == null)
            {
                throw new Exception("Müşteri için adres bulunamadı.");
            }*/
            return _mapper.Map<GetAddressQueryResponse>(address);
            /*return new GetAddressQueryResponse()
            {
                Id = address.Id,
                CustomerId = address.CustomerId,
                Description = address.Description,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City,
                ContactName = address.ContactName,
            };*/
        }
    }
}