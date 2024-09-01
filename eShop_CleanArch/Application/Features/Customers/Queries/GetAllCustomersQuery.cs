using Application.Services.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Customers.Queries;

public class GetAllCustomersQuery : IRequest<IEnumerable<Customer>>
{
}

public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<Customer>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetAllCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        return await _customerRepository.GetAllAsync();
    }
}