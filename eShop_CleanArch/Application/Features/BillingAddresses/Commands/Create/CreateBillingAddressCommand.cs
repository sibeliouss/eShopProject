using Application.Features.BillingAddresses.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.BillingAddresses.Rules;
using AutoMapper;
using FluentValidation;

namespace Application.Features.BillingAddresses.Commands.Create
{
    public class CreateBillingAddressCommand : IRequest<CreatedBillingAddressResponse>
    {
        public CreateBillingAddressDto CreateBillingAddressDto { get; set; }
        

        public class CreateBillingAddressCommandHandler : IRequestHandler<CreateBillingAddressCommand, CreatedBillingAddressResponse>
        {
            private readonly IBillingAddressRepository _billingAddressRepository;
            private readonly IMapper _mapper;
            private readonly IValidator<CreateBillingAddressDto> _validator;
            private readonly BillingAddressBusinessRules _billingAddressBusinessRules;

            public CreateBillingAddressCommandHandler(IBillingAddressRepository billingAddressRepository, IMapper mapper, IValidator<CreateBillingAddressDto> validator, BillingAddressBusinessRules billingAddressBusinessRules)
            {
                _billingAddressRepository = billingAddressRepository;
                _mapper = mapper;
                _validator = validator;
                _billingAddressBusinessRules = billingAddressBusinessRules;
            }

            public async Task<CreatedBillingAddressResponse> Handle(CreateBillingAddressCommand request, CancellationToken cancellationToken)
            {
                var billingAddressDto = request.CreateBillingAddressDto;
                
                var validationResult = await _validator.ValidateAsync(billingAddressDto, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                await _billingAddressBusinessRules.BillingAddressShouldNotExistForCustomer(
                    billingAddressDto.UserId);
                
                var billingAddress = _mapper.Map<BillingAddress>(billingAddressDto);

                await _billingAddressRepository.AddAsync(billingAddress);
                
                var response = _mapper.Map<CreatedBillingAddressResponse>(billingAddress);
                
                return response;
            }
        }
    }
}
