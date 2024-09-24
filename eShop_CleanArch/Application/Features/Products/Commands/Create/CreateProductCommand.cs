using Application.Features.Products.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Entities.ValueObjects;
using MediatR;


namespace Application.Features.Products.Commands.Create;

    public class CreateProductCommand : IRequest<CreatedProductResponse>
    {
        public CreateProductDto CreateProductDto { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreatedProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public CreateProductCommandHandler(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<CreatedProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productDto = request.CreateProductDto;
            
            var productExists = await _productRepository.AnyAsync(p => p.Name == productDto.Name);
            if (productExists)
            {
                throw new Exception("Bu ürün zaten eklenmiş.");
            }

            // Transaction
            await _productRepository.BeginTransactionAsync(cancellationToken);

            try
            {
                var product = new Product
                {
                    Name = productDto.Name,
                    ProductDetailId = productDto.ProductDetailId,
                    Brand = productDto.Brand,
                    Img = productDto.Img,
                    Price = new Money(productDto.Price.Value, productDto.Price.Currency),
                    IsFeatured = productDto.IsFeatured
                };

                await _productRepository.AddAsync(product);

                // Kategori ekleme
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
                        await _productCategoryRepository.AddAsync(productCategory);
                    }
                }

                // Veritabanına kaydet ve transaction'ı commit et
                await _productRepository.CommitTransactionAsync(cancellationToken);

                return new CreatedProductResponse
                {
                    Id = product.Id,
                    DetailId = product.ProductDetailId,
                    Name = product.Name,
                    Brand = product.Brand,
                    Price = product.Price,
                    Img = productDto.Img,
                    IsFeatured = product.IsFeatured
                    
                };
            }
            catch (Exception ex)
            {
                // Eğer bir hata olursa rollback yap
                await _productRepository.RollbackTransactionAsync(cancellationToken);
                throw new Exception("Ürün oluşturulurken bir hata meydana geldi.", ex);
            }
        }
    }

