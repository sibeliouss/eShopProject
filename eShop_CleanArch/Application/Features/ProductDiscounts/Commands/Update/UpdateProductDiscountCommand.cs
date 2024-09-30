using Application.Features.ProductDiscounts.Dtos;
using Application.Services.Products;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProductDiscounts.Commands.Update;

public class UpdateProductDiscountCommand : IRequest<UpdatedProductDiscountResponse>
{
    public UpdateProductDiscountDto UpdateProductDiscountDto { get; set; }

    public class
        UpdateProductDiscountCommandHandler : IRequestHandler<UpdateProductDiscountCommand,
        UpdatedProductDiscountResponse>
    {
        private readonly IProductDiscountRepository _productDiscountRepository;
        private readonly IProductService _productService;

        public UpdateProductDiscountCommandHandler(IProductDiscountRepository productDiscountRepository,
            IProductService productService)
        {
            _productDiscountRepository = productDiscountRepository;
            _productService = productService;
        }

        public async Task<UpdatedProductDiscountResponse> Handle(UpdateProductDiscountCommand request,
            CancellationToken cancellationToken)
        {
            var productDiscount =await _productDiscountRepository.GetByIdAsync(request.UpdateProductDiscountDto.Id);
            
            if (productDiscount is null)
            {
                throw new Exception("İndirim kaydı bulunamadı.");
            }
            
            productDiscount.DiscountPercentage = request.UpdateProductDiscountDto.DiscountPercentage;
            productDiscount.StartDate = request.UpdateProductDiscountDto.StartDate;
            productDiscount.EndDate = request.UpdateProductDiscountDto.EndDate;

            var productPrice = await _productService.Query()
                .Where(p => p.Id == productDiscount.ProductId)
                .AsNoTracking()
                .Select(p => p.Price)
                .FirstOrDefaultAsync(cancellationToken);

            if (productPrice is null)
                throw new Exception("Ürün fiyatı bulunamadı.");

            productDiscount.DiscountedPrice = productDiscount.DiscountPercentage == 0
                ? productPrice.Value
                : productPrice.Value - (productPrice.Value * productDiscount.DiscountPercentage / 100);

            await _productDiscountRepository.UpdateAsync(productDiscount);

            return new UpdatedProductDiscountResponse
            {
                Id = productDiscount.Id,
                ProductId = productDiscount.ProductId,
                DiscountPercentage = productDiscount.DiscountPercentage,
                StartDate = productDiscount.StartDate,
                EndDate = productDiscount.EndDate,
                DiscountedPrice = productDiscount.DiscountedPrice
            };
        }

    }
}