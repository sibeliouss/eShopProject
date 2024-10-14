using Application.Features.Customers.Dtos;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.Queries;

public class GetCustomerByUserIdQuery  : IRequest<CustomerDto>
{
    public Guid UserId { get; set; }

    public GetCustomerByUserIdQuery(Guid userId)
    {
        UserId = userId;
    } 
    
    public class GetCustomerByUserIdHandler : IRequestHandler<GetCustomerByUserIdQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByUserIdHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDto> Handle(GetCustomerByUserIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.Query()
                .Where(c => c.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (customer == null)
            {
                return null;
            }

          
            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                UserId = customer.UserId,
                UserName = customer.UserName,
                Email = customer.Email
            };
        }
    }

}