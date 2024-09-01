using Application.Features.Customers.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Customers.Queries
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public Guid CustomerId { get; set; }
    }

    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            return _mapper.Map<CustomerDto>(customer); 
        }
    }
}