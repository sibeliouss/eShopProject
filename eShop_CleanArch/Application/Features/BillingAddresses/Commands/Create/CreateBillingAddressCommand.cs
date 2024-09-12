using Application.Features.BillingAddresses.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.BillingAddresses.Commands.Create
{
    public class CreateBillingAddressCommand : IRequest<CreatedBillingAddressResponse>
    {
        public CreateBillingAddressDto CreateBillingAddressDto { get; set; }

        public class CreateBillingAddressCommandHandler : IRequestHandler<CreateBillingAddressCommand, CreatedBillingAddressResponse>
        {
            private readonly IBillingAddressRepository _billingAddressRepository;

            public CreateBillingAddressCommandHandler(IBillingAddressRepository billingAddressRepository)
            {
                _billingAddressRepository = billingAddressRepository;
            }

            public async Task<CreatedBillingAddressResponse> Handle(CreateBillingAddressCommand request, CancellationToken cancellationToken)
            {
                var billingAddressDto = request.CreateBillingAddressDto;
                var billingAddress = new BillingAddress()
                {
                    CustomerId = billingAddressDto.CustomerId,
                    City = billingAddressDto.City,
                    Country = billingAddressDto.Country,
                    ContactName = billingAddressDto.ContactName,
                    Description = billingAddressDto.Description,
                    ZipCode = billingAddressDto.ZipCode
                };

                await _billingAddressRepository.AddAsync(billingAddress);

                var response = new CreatedBillingAddressResponse
                {
                    Id = billingAddress.Id,
                    CustomerId = billingAddress.CustomerId,
                    Country = billingAddress.Country,
                    City = billingAddress.City,
                    ZipCode = billingAddress.ZipCode,
                    ContactName = billingAddress.ContactName,
                    Description = billingAddress.Description
                };
                
                return response;
            }
        }
    }
}
