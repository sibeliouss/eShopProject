using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.BillingAddresses.Queries;

public class GetListBillingAddressQuery  : IRequest<GetListBillingAddressQueryResponse>
{
    public Guid CustomerId { get; set; }

    public class GetAddressQueryHandler : IRequestHandler<GetListBillingAddressQuery, GetListBillingAddressQueryResponse>
    {
        private readonly IBillingAddressRepository _billingAddressRepository;
        private readonly IMapper _mapper;

        public GetAddressQueryHandler(IBillingAddressRepository billingAddressRepository, IMapper mapper)
        {
            _billingAddressRepository = billingAddressRepository;
            _mapper = mapper;
        }

        public async Task<GetListBillingAddressQueryResponse> Handle(GetListBillingAddressQuery request, CancellationToken cancellationToken)
        {
            var address = await _billingAddressRepository.Query().Where(a => a.CustomerId == request.CustomerId)
                .FirstOrDefaultAsync(cancellationToken);
            if (address == null)
            {
                throw new Exception("Fatura adresi bulunamadı.");
            }
            
            return _mapper.Map<GetListBillingAddressQueryResponse>(address);
        }
    }
}