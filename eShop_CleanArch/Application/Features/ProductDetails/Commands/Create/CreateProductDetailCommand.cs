using Application.Features.ProductDetails.Constants;
using Application.Features.ProductDetails.Dtos;
using Application.Services.Repositories;
using AutoMapper;
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
    private readonly IMapper _mapper;

    public CreateProductDetailCommandHandler(IProductDetailRepository productDetailRepository, IMapper mapper)
    {
        _productDetailRepository = productDetailRepository;
        _mapper = mapper;
    }

    public async Task<CreatedProductDetailResponse> Handle(CreateProductDetailCommand request, CancellationToken cancellationToken)
    {
        var detailDto = request.CreateProductDetailDto;

        var findProductDetail = await _productDetailRepository.AnyAsync(pd => pd.ProductId == detailDto.ProductId);
        if (findProductDetail)
        {
            throw new Exception(ProductDetailMessages.ProductDetailAlreadyExists);
        }

        var productDetail = _mapper.Map<ProductDetail>(detailDto);

        await _productDetailRepository.AddAsync(productDetail);

        return _mapper.Map<CreatedProductDetailResponse>(productDetail);
        
    }
}