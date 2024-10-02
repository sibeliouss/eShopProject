using Application.Features.ProductDetails.Constants;
using Application.Features.ProductDetails.Dtos;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
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
    private readonly IValidator<CreateProductDetailDto> _validator;

    public CreateProductDetailCommandHandler(IProductDetailRepository productDetailRepository, IMapper mapper, IValidator<CreateProductDetailDto> validator)
    {
        _productDetailRepository = productDetailRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<CreatedProductDetailResponse> Handle(CreateProductDetailCommand request, CancellationToken cancellationToken)
    {
        var detailDto = request.CreateProductDetailDto;
        
        var validationResult = await _validator.ValidateAsync(detailDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

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