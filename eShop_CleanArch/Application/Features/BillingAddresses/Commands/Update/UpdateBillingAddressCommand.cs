using Application.Features.Addresses.Commands.Update;
using Application.Features.BillingAddresses.Dtos;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.BillingAddresses.Commands.Update;

public class UpdateBillingAddressCommand : IRequest<UpdatedBillingAddressResponse>
{
    public UpdateBillingAddressDto UpdateBillingAddressDto { get; set; }

    public class UpdateBillingAddressCommandHandler : IRequestHandler<UpdateBillingAddressCommand, UpdatedBillingAddressResponse>
    {
        private readonly IBillingAddressRepository _billingAddressRepository;
        private readonly IMapper _mapper;

        public UpdateBillingAddressCommandHandler(IBillingAddressRepository billingAddressRepository, IMapper mapper)
        {
            _billingAddressRepository = billingAddressRepository;
            _mapper = mapper;
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
            _mapper.Map(billingAddressDto, billingAddress);
            
            await _billingAddressRepository.UpdateAsync(billingAddress);
            
            var response = _mapper.Map<UpdatedBillingAddressResponse>(billingAddress);

            return response;
            
            /*billingAddress.ContactName = billingAddressDto.ContactName;
            billingAddress.Description = billingAddressDto.Description;
            billingAddress.City = billingAddressDto.City;
            billingAddress.ZipCode = billingAddressDto.ZipCode;
            billingAddress.Country = billingAddressDto.Country;*/
            
            /*return new UpdatedBillingAddressResponse()
            {
                Id= billingAddress.Id,
                CustomerId = billingAddress.CustomerId,
                Country = billingAddress.Country,
                City = billingAddress.City,
                ZipCode = billingAddress.ZipCode,
                ContactName = billingAddress.ContactName,
                Description = billingAddress.Description
            };*/


        }
    }
}