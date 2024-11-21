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
            var currentDate = DateTime.UtcNow;

            var response = await _productRepository.Query().Include(p=>p.ProductDiscount)
                .Where(p => p.IsActive == true && p.IsFeatured == true && p.IsDeleted == false)
                .ToListAsync(cancellationToken);

            var products = response.Select(p => new ProductDto()
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand,
                Img = p.Img,
                Price = p.Price,
                ProductDiscount = p.ProductDiscount != null && 
                                  p.ProductDiscount.StartDate <= currentDate &&
                                  p.ProductDiscount.EndDate >= currentDate 
                    ? new ProductDiscountDto()
                    {
                        Id = p.ProductDiscount.Id,
                        ProductId = p.Id,
                        DiscountedPrice = p.ProductDiscount.DiscountedPrice,
                        DiscountPercentage = p.ProductDiscount.DiscountPercentage,
                        StartDate = p.ProductDiscount.StartDate,
                        EndDate = p.ProductDiscount.EndDate
                    }
                    : null,
            }).ToList();

            return products;
        }

    }
}