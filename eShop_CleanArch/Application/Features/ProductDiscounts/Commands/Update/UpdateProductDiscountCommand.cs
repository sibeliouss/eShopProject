using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.ProductDiscounts.Constants;
using Application.Features.ProductDiscounts.Dtos;
using Application.Services.Products;
using Application.Services.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProductDiscounts.Commands.Update;

public class UpdateProductDiscountCommand : IRequest<UpdatedProductDiscountResponse>
{
    public UpdateProductDiscountDto UpdateProductDiscountDto { get; set; }

    public class UpdateProductDiscountCommandHandler : IRequestHandler<UpdateProductDiscountCommand, UpdatedProductDiscountResponse>
    {
        private readonly IProductDiscountRepository _productDiscountRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateProductDiscountDto> _validator;

        public UpdateProductDiscountCommandHandler(IProductDiscountRepository productDiscountRepository, IProductService productService, IMapper mapper, IValidator<UpdateProductDiscountDto> validator)
        {
            _productDiscountRepository = productDiscountRepository;
            _productService = productService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<UpdatedProductDiscountResponse> Handle(UpdateProductDiscountCommand request, CancellationToken cancellationToken)
        {
            var updateProductDiscountDto = request.UpdateProductDiscountDto;
            
            var validationResult = await _validator.ValidateAsync(updateProductDiscountDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var productDiscount =await _productDiscountRepository.GetByIdAsync(updateProductDiscountDto.Id);
            
            if (productDiscount is null)
            {
                throw new Exception(ProductDiscountMessages.DiscountRecordNotFound);
            }

            _mapper.Map(updateProductDiscountDto, productDiscount);

            var productPrice = await _productService.Query()
                .Where(p => p.Id == productDiscount.ProductId)
                .AsNoTracking()
                .Select(p => p.Price)
                .FirstOrDefaultAsync(cancellationToken);

            if (productPrice is null)
                throw new Exception(ProductDiscountMessages.PriceNotFound);

            productDiscount.DiscountedPrice = productDiscount.DiscountPercentage == 0
                ? productPrice.Value
                : productPrice.Value - (productPrice.Value * productDiscount.DiscountPercentage / 100);

            await _productDiscountRepository.UpdateAsync(productDiscount);

            return _mapper.Map<UpdatedProductDiscountResponse>(productDiscount);
            
        }

    }
}