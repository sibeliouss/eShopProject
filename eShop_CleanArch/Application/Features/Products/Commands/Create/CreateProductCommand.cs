using Application.Features.Products.Constants;
using Application.Features.Products.Dtos;
using Application.Features.Products.Rules;
using Application.Services.ProductCategories;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.ValueObjects;
using FluentValidation;
using MediatR;


namespace Application.Features.Products.Commands.Create;

    public class CreateProductCommand : IRequest<CreatedProductResponse>
    {
        public CreateProductDto CreateProductDto { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreatedProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;
        private readonly ProductBusinessRules _productBusinessRules;
        private readonly IValidator<CreateProductDto> _validator;

        public CreateProductCommandHandler(IProductRepository productRepository, IProductCategoryService productCategoryService, IMapper mapper, ProductBusinessRules productBusinessRules, IValidator<CreateProductDto> validator)
        {
            _productRepository = productRepository;
            _productCategoryService = productCategoryService;
            _mapper = mapper;
            _productBusinessRules = productBusinessRules;
            _validator = validator;
        } 
        public async Task<CreatedProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
       {
         var productDto = request.CreateProductDto;
         
         var validationResult = await _validator.ValidateAsync(productDto, cancellationToken);
         if (!validationResult.IsValid)
         {
             throw new ValidationException(validationResult.Errors);
         }
         
         await _productBusinessRules.ProductNameAlreadyExists(productDto.Name);

        // Transaction
        await _productRepository.BeginTransactionAsync(cancellationToken);

        try
        {
            var product = _mapper.Map<Product>(productDto);

            await _productRepository.AddAsync(product);
            
            if (productDto.CategoryIds is not null && productDto.CategoryIds.Any())
            {
                await _productCategoryService.AddCategoriesToProductAsync(product.Id, productDto.CategoryIds);
            }
            
            await _productRepository.CommitTransactionAsync(cancellationToken);

            return _mapper.Map<CreatedProductResponse>(product);
        }
        catch (Exception ex)
        {
            await _productRepository.RollbackTransactionAsync(cancellationToken);
            throw new Exception(ProductMessages.ProductCreationError, ex);
        }
       }

    }

