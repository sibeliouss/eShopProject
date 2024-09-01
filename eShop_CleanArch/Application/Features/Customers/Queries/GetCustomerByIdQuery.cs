using Application.Services.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Customers.Queries;

public class GetCustomerByIdQuery : IRequest<Customer?>
{
    public Guid CustomerId { get; set; }
}

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer?>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        return await _customerRepository.GetByIdAsync(request.CustomerId);
    }
}