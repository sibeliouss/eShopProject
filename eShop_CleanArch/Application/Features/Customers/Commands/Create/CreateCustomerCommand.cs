using Application.Features.Customers.Dtos;
using Application.Services.Customers;
using AutoMapper;
using MediatR;

namespace Application.Features.Customers.Commands.Create;

public class CreateCustomerCommand : IRequest<CreatedCustomerResponse>
{
    public CreateCustomerDto CreateCustomerDto { get; set; }
    
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreatedCustomerResponse>
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public async Task<CreatedCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerDto = request.CreateCustomerDto;
            await _customerService.CreateCustomerAsync(customerDto);
            
            var createdCustomer = await _customerService.GetCustomerByIdAsync(customerDto.UserId);

            if (createdCustomer == null)
            {
                throw new Exception("Müşteri oluşturulamadı.");
            }

            var response = _mapper.Map<CreatedCustomerResponse>(createdCustomer);
            return response;
        }
    }

}