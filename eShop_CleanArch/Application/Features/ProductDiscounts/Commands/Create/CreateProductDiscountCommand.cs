using Application.Features.ProductDiscounts.Dtos;
using Application.Services.Products;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProductDiscounts.Commands.Create;

public class CreateProductDiscountCommand : IRequest<CreatedProductDiscountResponse>
{
    public CreateProductDiscountDto CreateProductDiscountDto { get; set; }
    
    public class CreateProductDiscountCommandHandler : IRequestHandler<CreateProductDiscountCommand, CreatedProductDiscountResponse>
    {
        private readonly IProductDiscountRepository _productDiscountRepository;
        private readonly IProductService _productService;

        public CreateProductDiscountCommandHandler(IProductDiscountRepository productDiscountRepository, IProductService productService)
        {
            _productDiscountRepository = productDiscountRepository;
            _productService = productService;
        }
        
        public async Task<CreatedProductDiscountResponse> Handle(CreateProductDiscountCommand request, CancellationToken cancellationToken)
        {
            var existingDiscount = await _productDiscountRepository.Query()
                .Where(pd => pd.ProductId == request.CreateProductDiscountDto.ProductId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (existingDiscount is not null)
            {
                throw new Exception("Bu ürün için zaten indirim yapılmış.");
            }

            var productPrice = await _productService.Query()
                .Where(p => p.Id == request.CreateProductDiscountDto.ProductId)
                .AsNoTracking()
                .Select(p => p.Price)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            if (productPrice is null)
                throw new Exception("Ürün fiyatı bulunamadı.");
            
            var discountPrice = request.CreateProductDiscountDto.DiscountPercentage == 0
                ? productPrice.Value
                : productPrice.Value - (productPrice.Value * request.CreateProductDiscountDto.DiscountPercentage / 100);

            var productDiscount = new ProductDiscount
            {
                ProductId = request.CreateProductDiscountDto.ProductId,
                DiscountPercentage = request.CreateProductDiscountDto.DiscountPercentage,
                StartDate = request.CreateProductDiscountDto.StartDate,
                EndDate = request.CreateProductDiscountDto.EndDate,
                DiscountedPrice = discountPrice,
            };

            await _productDiscountRepository.AddAsync(productDiscount);
              

            return new CreatedProductDiscountResponse()
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