using Application.Features.Products.Dtos;
using Application.Features.Products.Queries.ResponseDtos;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Products.Queries;

    
    public class GetProductsQuery : IRequest<List<GetProductResponse>>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<GetProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<GetProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.Query().AsNoTracking()
                .Select(product => new GetProductResponse()
                {
                    Id = product.Id,
                    DetailId = product.ProductDetailId,
                    Name = product.Name,
                    Brand = product.Brand,
                    Price = product.Price,
                    CreateAt = product.CreateAt,
                    Img = product.Img,
                })
                .ToListAsync(cancellationToken);

            return products;
        }
    }
