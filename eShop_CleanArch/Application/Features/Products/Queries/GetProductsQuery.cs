using Application.Features.Products.Dtos;
using Application.Features.Products.Queries.ResponseDtos;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Products.Queries;

    
    public class GetProductsQuery : IRequest<List<GetProductResponse>>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<GetProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<GetProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.Query().AsNoTracking().ToListAsync(cancellationToken);

            return _mapper.Map<List<GetProductResponse>>(products);
            
        }
    }
