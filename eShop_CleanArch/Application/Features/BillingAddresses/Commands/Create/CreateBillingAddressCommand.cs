using Application.Features.BillingAddresses.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace Application.Features.BillingAddresses.Commands.Create
{
    public class CreateBillingAddressCommand : IRequest<CreatedBillingAddressResponse>
    {
        public CreateBillingAddressDto CreateBillingAddressDto { get; set; }
        

        public class CreateBillingAddressCommandHandler : IRequestHandler<CreateBillingAddressCommand, CreatedBillingAddressResponse>
        {
            private readonly IBillingAddressRepository _billingAddressRepository;
            private readonly IMapper _mapper;

            public CreateBillingAddressCommandHandler(IBillingAddressRepository billingAddressRepository, IMapper mapper)
            {
                _billingAddressRepository = billingAddressRepository;
                _mapper = mapper;
            }

            public async Task<CreatedBillingAddressResponse> Handle(CreateBillingAddressCommand request, CancellationToken cancellationToken)
            {
                var billingAddressDto = request.CreateBillingAddressDto;
                var addressExists = await _billingAddressRepository.AnyAsync(a => a.CustomerId == billingAddressDto.CustomerId);
                if (addressExists)
                {
                    throw new Exception("Fatura adresi zaten mevcut");
                }
                var billingAddress = _mapper.Map<BillingAddress>(billingAddressDto);

                await _billingAddressRepository.AddAsync(billingAddress);
                
                var response = _mapper.Map<CreatedBillingAddressResponse>(billingAddress);
                
                return response;
            }
        }
    }
}
