using Application.Features.ProductDetails.Dtos;
using Application.Features.ProductDiscounts.Constants;
using Application.Features.ProductDiscounts.Dtos;
using Application.Services.Products;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
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
        private readonly IMapper _mapper;
        private readonly IValidator<CreateProductDiscountDto> _validator;

        public CreateProductDiscountCommandHandler(IProductDiscountRepository productDiscountRepository, IProductService productService, IMapper mapper, IValidator<CreateProductDiscountDto> validator)
        {
            _productDiscountRepository = productDiscountRepository;
            _productService = productService;
            _mapper = mapper;
            _validator = validator;
        }
        
        public async Task<CreatedProductDiscountResponse> Handle(CreateProductDiscountCommand request, CancellationToken cancellationToken)
        {
            var productDiscountDto = request.CreateProductDiscountDto;
            
            var validationResult = await _validator.ValidateAsync(productDiscountDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var existingDiscount = await _productDiscountRepository.Query()
                .Where(pd => pd.ProductId == productDiscountDto.ProductId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (existingDiscount is not null)
            {
                throw new Exception(ProductDiscountMessages.DiscountAlreadyExists);
            }

            var productPrice = await _productService.Query()
                .Where(p => p.Id == request.CreateProductDiscountDto.ProductId)
                .AsNoTracking()
                .Select(p => p.Price)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            if (productPrice is null)
                throw new Exception(ProductDiscountMessages.PriceNotFound);
            
            var discountPrice = request.CreateProductDiscountDto.DiscountPercentage == 0
                ? productPrice.Value
                : productPrice.Value - (productPrice.Value * request.CreateProductDiscountDto.DiscountPercentage / 100);

            var productDiscount = _mapper.Map<ProductDiscount>(productDiscountDto);
            productDiscount.DiscountedPrice = discountPrice;

            await _productDiscountRepository.AddAsync(productDiscount);

            return _mapper.Map<CreatedProductDiscountResponse>(productDiscount);
            

        }

    }
}