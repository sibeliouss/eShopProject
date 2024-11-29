using Application.Features.Products.Constants;
using Application.Features.Products.Dtos;
using Application.Features.Products.Rules;
using Application.Services.ProductCategories;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Commands.Update;

public class UpdateProductCommand :IRequest<UpdatedProductResponse>
{
    public UpdateProductDto UpdateProductDto { get; set; }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdatedProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;
        private readonly ProductBusinessRules _productBusinessRules;
        private readonly IValidator<UpdateProductDto> _validator;

        public UpdateProductCommandHandler(IProductRepository productRepository, IProductCategoryService productCategoryService, IMapper mapper, ProductBusinessRules productBusinessRules, IValidator<UpdateProductDto> validator)
        {
            _productRepository = productRepository;
            _productCategoryService = productCategoryService;
            _mapper = mapper;
            _productBusinessRules = productBusinessRules;
            _validator = validator;
        }
        public async Task<UpdatedProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            // Validation işlemi
            var validationResult = await _validator.ValidateAsync(request.UpdateProductDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            
            var updateProductDto = request.UpdateProductDto;
            var product = await _productRepository.Query().Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.Id == updateProductDto.Id, cancellationToken: cancellationToken);

            if (product is null)
            {
                throw new Exception(ProductMessages.ProductNotFound);
            }

            _mapper.Map(updateProductDto, product);
            
            //Kategoriyi gümcelleme
            if (updateProductDto.CategoryIds is not null)
            {
                await _productCategoryService.RemoveAllCategoriesFromProductAsync(product.Id);
                
                await _productCategoryService.AddCategoriesToProductAsync(product.Id, updateProductDto.CategoryIds);

            }
            await _productRepository.UpdateAsync(product);

            return _mapper.Map<UpdatedProductResponse>(product);

            

        }
    }
}