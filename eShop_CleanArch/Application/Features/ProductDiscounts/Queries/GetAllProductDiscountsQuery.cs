using Application.Features.ProductDiscounts.Dtos;
using Application.Services.Repositories;
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
                .Where(p => p.DiscountPercentage > 0 && p.EndDate > DateTime.Now &&
                            p.StartDate <= DateTime.Now) 
                .ToListAsync(cancellationToken);

            if (productDiscounts == null || !productDiscounts.Any())
                return new List<ProductDiscountDto>(); 

      
            return productDiscounts.Select(productDiscount => new ProductDiscountDto
            (
                productDiscount.Id,
                productDiscount.ProductId,
                productDiscount.DiscountPercentage,
                productDiscount.StartDate,
                productDiscount.EndDate,
                productDiscount.DiscountedPrice
            )).ToList();
        }
    }
}