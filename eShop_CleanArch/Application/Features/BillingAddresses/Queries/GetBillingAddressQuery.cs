using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.BillingAddresses.Queries;

public class GetBillingAddressQuery  : IRequest<GetBillingAddressQueryResponse>
{
    public Guid CustomerId { get; set; }

    public class GetAddressQueryHandler : IRequestHandler<GetBillingAddressQuery, GetBillingAddressQueryResponse>
    {
        private readonly IBillingAddressRepository _billingAddressRepository;
        private readonly IMapper _mapper;

        public GetAddressQueryHandler(IBillingAddressRepository billingAddressRepository, IMapper mapper)
        {
            _billingAddressRepository = billingAddressRepository;
            _mapper = mapper;
        }

        public async Task<GetBillingAddressQueryResponse> Handle(GetBillingAddressQuery request, CancellationToken cancellationToken)
        {
            var address = await _billingAddressRepository.Query().Where(a => a.CustomerId == request.CustomerId)
                .FirstOrDefaultAsync(cancellationToken);
            if (address == null)
            {
                throw new Exception("Fatura adresi bulunamadı.");
            }
            
            return _mapper.Map<GetBillingAddressQueryResponse>(address);
        }
    }
}