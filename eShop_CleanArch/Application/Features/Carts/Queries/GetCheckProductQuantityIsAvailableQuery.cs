using Application.Features.Carts.Queries.Responses;
using Application.Services.Products;
using MediatR;

namespace Application.Features.Carts.Queries;

public class GetCheckProductQuantityIsAvailableQuery : IRequest<CheckProductQuantityIsAvailableResponse>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    
    public class GetCheckProductQuantityIsAvailableQueryHandler : IRequestHandler<GetCheckProductQuantityIsAvailableQuery, CheckProductQuantityIsAvailableResponse>
    {
        private readonly IProductService _productService;

        public GetCheckProductQuantityIsAvailableQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<CheckProductQuantityIsAvailableResponse> Handle(GetCheckProductQuantityIsAvailableQuery request, CancellationToken cancellationToken)
        {
            //ürün stok kontrolü
            var product = await _productService.FindAsync(request.ProductId);
            if (product is null)
            {
                return new CheckProductQuantityIsAvailableResponse
                {
                    IsAvailable = false,
                    Message = "Ürün bulunamadı"
                };
            }
            
            if (product.Quantity < request.Quantity)
            {
                return new CheckProductQuantityIsAvailableResponse
                {
                    IsAvailable = false,
                    Message = "Ürün stoğu yeterli değil"
                };
            }
            
            return new CheckProductQuantityIsAvailableResponse
            {
                IsAvailable = true,
                Message = "Ürün stoğu yeterli"
            };
        }
    }
}