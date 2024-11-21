using Application.Features.ProductDiscounts.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProductDiscounts.Queries;

public class GetProductDiscountsQuery : IRequest<List<ProductDiscountDto>>
{


    public class GetProductDiscountsQueryHandler : IRequestHandler<GetProductDiscountsQuery, List<ProductDiscountDto>>
    {
        private readonly IProductDiscountRepository _productDiscountRepository;

        public GetProductDiscountsQueryHandler(IProductDiscountRepository productDiscountRepository)
        {
            _productDiscountRepository = productDiscountRepository;
        }

        public async Task<List<ProductDiscountDto>> Handle(GetProductDiscountsQuery request,
            CancellationToken cancellationToken)
        {

            var productDiscounts = await _productDiscountRepository.Query()
                .Include(pd => pd.Product) 
                .Where(pd => pd.DiscountPercentage > 0 && pd.EndDate > DateTime.Now && pd.StartDate <= DateTime.Now)
                .ToListAsync(cancellationToken);

            if (productDiscounts == null || !productDiscounts.Any())
                return new List<ProductDiscountDto>();

            // DTO'ya gerekli bilgileri aktar
            return productDiscounts.Select(productDiscount => new ProductDiscountDto
            (
                productDiscount.Id,
                productDiscount.ProductId,
                productDiscount.Product.Name, 
                productDiscount.Product.Brand , 
                productDiscount.Product.Img, 
                productDiscount.Product.Price, 
                productDiscount.DiscountPercentage,
                productDiscount.StartDate,
                productDiscount.EndDate,
                productDiscount.DiscountedPrice,
                productDiscount.Product.Quantity
            )).ToList();
        }
    }
}