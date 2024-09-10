using Application.Features.Customers.Dtos;
using Application.Services.Customers;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.Customers.Commands.Create;

public class CreateCustomerCommand : IRequest<CreatedCustomerResponse>
{
    public CreateCustomerDto CreateCustomerDto { get; set; }
    
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreatedCustomerResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper, ICustomerService customerService)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _customerService = customerService;
        }

        public async Task<CreatedCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerDto = request.CreateCustomerDto;
            await _customerService.CreateCustomerAsync(customerDto);

            var createdCustomer = await _customerRepository.AnyAsync(c => c.UserId == customerDto.UserId);

            if (createdCustomer is false)
            {
                throw new Exception("Müşteri oluşturulamadı.");
            }

            var response = _mapper.Map<CreatedCustomerResponse>(createdCustomer);
            return response;
        }
    }

}