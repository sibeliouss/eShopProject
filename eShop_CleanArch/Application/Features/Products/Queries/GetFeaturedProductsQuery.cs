using Application.Features.Products.Dtos;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries;

public class GetFeaturedProductsQuery: IRequest<List<ProductDto>>
{
    public class GetFeaturedProductQueryHandler: IRequestHandler<GetFeaturedProductsQuery, List<ProductDto> >
    {
        private readonly IProductRepository _productRepository;

        public GetFeaturedProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<List<ProductDto>> Handle(GetFeaturedProductsQuery request, CancellationToken cancellationToken)
        {
            var response = await _productRepository.Query()
                .Where(p => p.IsActive && p.IsFeatured && p.IsDeleted == false).ToListAsync(cancellationToken);

            var products = response.Select(p => new ProductDto()
            {
                Id= p.Id,
                Name=p.Name,
                Brand = p.Brand,
                Price = p.Price
            }).ToList();

            return products;
        }
    }
}