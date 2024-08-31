using Application.Features.Customers.Dtos;
using Application.Services.Customers;
using MediatR;

namespace Application.Features.Customers.Commands.Create;

public class CreateCustomerCommand : IRequest<CreatedCustomerResponse>
{
    public CreateCustomerDto CreateCustomerDto { get; set; }
    
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreatedCustomerResponse>
    {
        private readonly ICustomerService _customerService;

        public CreateCustomerCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
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

            return new CreatedCustomerResponse
            {
                Id = createdCustomer.Id,
                FirstName = createdCustomer.FirstName,
                LastName = createdCustomer.LastName,
                Email = createdCustomer.Email,
                UserName = createdCustomer.UserName,
                UserId = createdCustomer.UserId,
                PasswordHash = createdCustomer.PasswordHash
            };
        }
    }

}