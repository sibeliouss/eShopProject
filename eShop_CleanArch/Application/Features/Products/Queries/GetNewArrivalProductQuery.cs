using Application.Features.Products.Dtos;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries;

public class GetNewArrivalProductQuery : IRequest<List<ProductDto>> 
{
    public class GetNewArrivalProductHandler : IRequestHandler<GetNewArrivalProductQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetNewArrivalProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> Handle(GetNewArrivalProductQuery request, CancellationToken cancellationToken)
        {
            var response = await _productRepository.Query()
                .OrderByDescending(p => p.CreateAt) 
                .Take(10) 
                .ToListAsync(cancellationToken);
            
            var products = response.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand,
                Price = p.Price,
                Img = p.Img,
                Quantity = p.Quantity,
                IsFeatured = p.IsFeatured,
                CreateAt = p.CreateAt,
            }).ToList();

            return products;
        }
    }
}