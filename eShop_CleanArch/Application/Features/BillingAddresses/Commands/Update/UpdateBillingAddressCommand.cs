using Application.Features.Addresses.Commands.Update;
using Application.Features.BillingAddresses.Dtos;
using Application.Services.Repositories;
using MediatR;

namespace Application.Features.BillingAddresses.Commands.Update;

public class UpdateBillingAddressCommand : IRequest<UpdatedBillingAddressResponse>
{
    public UpdateBillingAddressDto UpdateBillingAddressDto { get; set; }

    public class UpdateBillingAddressCommandHandler : IRequestHandler<UpdateBillingAddressCommand, UpdatedBillingAddressResponse>
    {
        private readonly IBillingAddressRepository _billingAddressRepository;

        public UpdateBillingAddressCommandHandler(IBillingAddressRepository billingAddressRepository)
        {
            _billingAddressRepository = billingAddressRepository;
        }

        public async Task<UpdatedBillingAddressResponse> Handle(UpdateBillingAddressCommand request,
            CancellationToken cancellationToken)
        {
            var billingAddressDto = request.UpdateBillingAddressDto;
            var billingAddress = await _billingAddressRepository.GetByIdAsync(billingAddressDto.Id);
            if (billingAddress == null)
            {
                throw new Exception("Fatura adresi bulunamadı.");
            }
            
            billingAddress.ContactName = billingAddressDto.ContactName;
            billingAddress.Description = billingAddressDto.Description;
            billingAddress.City = billingAddressDto.City;
            billingAddress.ZipCode = billingAddressDto.ZipCode;
            billingAddress.Country = billingAddressDto.Country;

            await _billingAddressRepository.UpdateAsync(billingAddress);

            return new UpdatedBillingAddressResponse()
            {
                Id= billingAddress.Id,
                CustomerId = billingAddress.CustomerId,
                Country = billingAddress.Country,
                City = billingAddress.City,
                ZipCode = billingAddress.ZipCode,
                ContactName = billingAddress.ContactName,
                Description = billingAddress.Description
            };


        }
    }
}