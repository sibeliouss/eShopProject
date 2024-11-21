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
            var currentDate = DateTime.UtcNow; 
            var response = await _productRepository.Query().Include(p=>p.ProductDiscount)
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