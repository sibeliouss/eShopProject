using Application.Features.Addresses.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Addresses.Queries;

public class GetListAddressQuery : IRequest<GetListAddressQueryResponse>
{
    public Guid CustomerId { get; set; }

    public class GetAddressQueryHandler : IRequestHandler<GetListAddressQuery, GetListAddressQueryResponse>
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

        public async Task<GetListAddressQueryResponse> Handle(GetListAddressQuery request, CancellationToken cancellationToken)
        {
            var address = await _addressRepository.Query().Where(a => a.CustomerId == request.CustomerId)
                .FirstOrDefaultAsync(cancellationToken);

            await _addressBusinessRules.AddressShouldExistWhenSelected(address);
            return _mapper.Map<GetListAddressQueryResponse>(address);
           
        }
    }
}