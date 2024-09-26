using Application.Features.ProductDetails.Constants;
using Application.Features.ProductDetails.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.ProductDetails.Commands.Update;

public class UpdateProductDetailCommand :IRequest<UpdatedProductDetailResponse>
{
    public UpdateProductDetailDto UpdateProductDetailDto { get; set; }

    public class UpdateProductDetailCommandHandler: IRequestHandler<UpdateProductDetailCommand, UpdatedProductDetailResponse>
    {
        private readonly IProductDetailRepository _productDetailRepository;

        public UpdateProductDetailCommandHandler(IProductDetailRepository productDetailRepository)
        {
            _productDetailRepository = productDetailRepository;
        }
        
        public async Task<UpdatedProductDetailResponse> Handle(UpdateProductDetailCommand request, CancellationToken cancellationToken)
        {
            var productDetailDto = request.UpdateProductDetailDto;
            var productDetail = await _productDetailRepository.GetByIdAsync(productDetailDto.Id);
            if (productDetail is null)
            {
                throw new Exception(ProductDetailMessages.ProductDetailNotFound);
            }


            productDetail.Brand = productDetailDto.Brand;
            productDetail.Barcode = productDetailDto.Barcode;
            productDetail.Color = productDetailDto.Color;
            productDetail.Description = productDetailDto.Description;
            productDetail.Fit = productDetailDto.Fit;
            productDetail.Material = productDetailDto.Material;
            productDetail.Size = productDetailDto.Size;
            productDetail.Stock = productDetailDto.Stock;
           

            await _productDetailRepository.UpdateAsync(productDetail);

            return new UpdatedProductDetailResponse()
            {
             Id= productDetail.Id,
             Brand = productDetail.Brand,
             Barcode = productDetail.Barcode,
             Color=productDetail.Color,
             Description = productDetail.Description,
             Fit = productDetail.Fit,
             Material = productDetail.Material,
             Stock = productDetail.Stock,
             Size = productDetail.Size
            };

        }
    }
}