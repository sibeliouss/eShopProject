using Application.Features.ProductDetails.Constants;
using Application.Features.ProductDetails.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.ProductDetails.Commands.Create;

public class CreateProductDetailCommand : IRequest<CreatedProductDetailResponse>
{
    public CreateProductDetailDto CreateProductDetailDto { get; set; }
}

public class CreateProductDetailCommandHandler : IRequestHandler<CreateProductDetailCommand, CreatedProductDetailResponse>
{
    private readonly IProductDetailRepository _productDetailRepository;

    public CreateProductDetailCommandHandler(IProductDetailRepository productDetailRepository)
    {
        _productDetailRepository = productDetailRepository;
    }

    public async Task<CreatedProductDetailResponse> Handle(CreateProductDetailCommand request, CancellationToken cancellationToken)
    {
        var detailDto = request.CreateProductDetailDto;

        var findProductDetail = await _productDetailRepository.AnyAsync(pd => pd.ProductId == detailDto.ProductId);
        if (findProductDetail)
        {
            throw new Exception(ProductDetailMessages.ProductDetailAlreadyExists);
        }
        
        var productDetail = new ProductDetail
        {
            ProductId = detailDto.ProductId,
            Description = detailDto.Description,
            Stock = detailDto.Stock,
            Barcode = detailDto.Barcode,
            Material = detailDto.Material,
            Size = detailDto.Size,
            Color = detailDto.Color,
            Fit = detailDto.Fit,
            Brand = detailDto.Brand
        };

        
        await _productDetailRepository.AddAsync(productDetail);

        return new CreatedProductDetailResponse
        {
            Id = productDetail.Id,
            ProductId = productDetail.ProductId,
            Description = productDetail.Description,
            Stock = productDetail.Stock,
            Barcode = productDetail.Barcode,
            Material = productDetail.Material,
            Size = productDetail.Size,
            Color = productDetail.Color,
            Fit = productDetail.Fit,
            Brand = productDetail.Brand
        };
    }
}