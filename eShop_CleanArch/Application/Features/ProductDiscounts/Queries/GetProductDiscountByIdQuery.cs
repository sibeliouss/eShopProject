using Application.Features.ProductDiscounts.Dtos;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProductDiscounts.Queries;

public class GetProductDiscountByIdQuery : IRequest<ProductDiscountDto>
{
    public Guid ProductId { get; set; }
    
    public GetProductDiscountByIdQuery(Guid productId)
    {
        ProductId = productId;
    }
}

public class GetProductDiscountByIdQueryHandler : IRequestHandler<GetProductDiscountByIdQuery, ProductDiscountDto>
{
    private readonly IProductDiscountRepository _productDiscountRepository;

    public GetProductDiscountByIdQueryHandler(IProductDiscountRepository productDiscountRepository)
    {
        _productDiscountRepository = productDiscountRepository;
    }

    public async Task<ProductDiscountDto> Handle(GetProductDiscountByIdQuery request, CancellationToken cancellationToken)
    {
        
        var productDiscount = await _productDiscountRepository.Query()
            .Where(p => p.ProductId == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);

        if (productDiscount == null)
            return null;  

       
        return new ProductDiscountDto
        (
            productDiscount.Id,
            productDiscount.ProductId,
            productDiscount.DiscountPercentage,
            productDiscount.StartDate,
            productDiscount.EndDate,
            productDiscount.DiscountedPrice
        );
    }
}
