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
    
            // Validation işlemi
            var validationResult = await _validator.ValidateAsync(productDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            

            // Transaction başlat
            await _productRepository.BeginTransactionAsync(cancellationToken);

            try
            {
               
                await _productBusinessRules.ProductNameAlreadyExists(productDto.Name);
                
                
                var product = _mapper.Map<Product>(productDto);
                await _productRepository.AddAsync(product);
        
                // CategoryIds kontrol et ve ekle
                if (productDto.CategoryIds is not null && productDto.CategoryIds.Any())
                {
                    foreach (var categoryId in productDto.CategoryIds)
                    {
                        var productCategory = new ProductCategory
                        {  
                            Id = Guid.NewGuid(),
                            ProductId = product.Id,
                            CategoryId = categoryId
                        };
                        await _productCategoryService.AddAsync(productCategory);
                    }
                }

                await _productRepository.CommitTransactionAsync(cancellationToken);

              
                var response = _mapper.Map<CreatedProductResponse>(product);
                response.CategoryIds = productDto.CategoryIds; 

                return response;
            }
            catch (Exception ex)
            {
                await _productRepository.RollbackTransactionAsync(cancellationToken);
                throw new Exception(ProductMessages.ProductCreationError, ex);
            }
        }

    }


