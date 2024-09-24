using Application.Features.ProductDetails.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.ProductDetails.Commands.Create;

public class CreateProductDetailCommand : IRequest<CreatedProductDetailResponse>
{
    public CreateProductDetailDto CreateProductDetailDto { get; set; }
    

    public class CreateProductDetailCommandHandler : IRequestHandler<CreateProductDetailCommand, CreatedProductDetailResponse>
    {
        private readonly IProductDetailRepository _productDetailRepository;
        private readonly CreateProductDetailCommandValidator _validator;
        

        public CreateProductDetailCommandHandler(IProductDetailRepository productDetailRepository, CreateProductDetailCommandValidator validator)
        {
            _productDetailRepository = productDetailRepository;
            _validator = validator;
        }
        public async Task<CreatedProductDetailResponse> Handle(CreateProductDetailCommand request, CancellationToken cancellationToken)
        {
            //Validasyon
            var validationResult = await _validator.ValidateAsync(request.CreateProductDetailDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var productDetailDto = request.CreateProductDetailDto;
            var productDetail = await _productDetailRepository.AnyAsync(pd => pd.ProductId == productDetailDto.ProductId);
            if (productDetail)
            {
                throw new Exception("Bu ürüne ait bilgi zaten bulunuyor!");
            }
 
            var detail = new ProductDetail
            {
                ProductId = productDetailDto.ProductId,
                Barcode =  productDetailDto.Barcode,
                Color = productDetailDto.Color,
                Fit = productDetailDto.Fit,
                Stock = productDetailDto.Stock,
                Size = productDetailDto.Size,
                Material = productDetailDto.Material,
                Description = productDetailDto.Description,
                Brand = productDetailDto.Brand,
                
            };
            await _productDetailRepository.AddAsync(detail);

            return new CreatedProductDetailResponse()
            {
                Id = detail.Id,
                ProductId = detail.ProductId,
                Barcode = detail.Barcode,
                Color = detail.Color,
                Material = detail.Material,
                Fit = detail.Fit,
                Size = detail.Size,
                Description = detail.Description,
                Stock = detail.Stock,
                Brand = detail.Brand
            };
        } 
    }
}