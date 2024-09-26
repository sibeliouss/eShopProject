using Application.Features.Addresses.Commands.Update;
using Application.Features.BillingAddresses.Constants;
using Application.Features.BillingAddresses.Dtos;
using Application.Services.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Features.BillingAddresses.Commands.Update;

public class UpdateBillingAddressCommand : IRequest<UpdatedBillingAddressResponse>
{
    public UpdateBillingAddressDto UpdateBillingAddressDto { get; set; }

    public class UpdateBillingAddressCommandHandler : IRequestHandler<UpdateBillingAddressCommand, UpdatedBillingAddressResponse>
    {
        private readonly IBillingAddressRepository _billingAddressRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateBillingAddressDto> _validator;

        public UpdateBillingAddressCommandHandler(IBillingAddressRepository billingAddressRepository, IMapper mapper, IValidator<UpdateBillingAddressDto> validator)
        {
            _billingAddressRepository = billingAddressRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<UpdatedBillingAddressResponse> Handle(UpdateBillingAddressCommand request, CancellationToken cancellationToken)
        {
            var billingAddressDto = request.UpdateBillingAddressDto;
            
            var validationResult = await _validator.ValidateAsync(billingAddressDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var billingAddress = await _billingAddressRepository.GetByIdAsync(billingAddressDto.Id);
            if (billingAddress == null)
            {
                throw new Exception(BillingAddressMessages.BillingAddressNotFound);
            }
            _mapper.Map(billingAddressDto, billingAddress);
            
            await _billingAddressRepository.UpdateAsync(billingAddress);
            
            var response = _mapper.Map<UpdatedBillingAddressResponse>(billingAddress);

            return response;
            
        }
    }
}