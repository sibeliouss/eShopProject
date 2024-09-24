using Application.Features.Products.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Commands.Update;

public class UpdateProductCommand :IRequest<UpdatedProductResponse>
{
    public UpdateProductDto UpdateProductDto { get; set; }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdatedProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }
        public async Task<UpdatedProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var updateProductDto = request.UpdateProductDto;
            var product = await _productRepository.Query().Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.Id == updateProductDto.Id, cancellationToken: cancellationToken);

            if (product is null)
            {
                throw new Exception("ürün bulunamadı!");
            }
            product.Name = updateProductDto.Name;
            product.Brand = updateProductDto.Brand;
            product.ProductDetailId = updateProductDto.ProductDetailId;
            product.Img = updateProductDto.Img;
            product.Price = updateProductDto.Price;
            product.IsFeatured = updateProductDto.IsFeatured;
            
            //Kategoriyi gümcelleme
            if (updateProductDto.CategoryIds is not null)
            {
                //mevcut kategorileri sil
                if (product.ProductCategories != null)
                    foreach (var existingCategory in product.ProductCategories.ToList())
                    {
                        await _productCategoryRepository.DeleteAsync(existingCategory);
                    }

                //yeni kategori
                foreach (var categoryId in updateProductDto.CategoryIds)
                {
                    var productCategory = new ProductCategory()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        CategoryId = categoryId
                    };
                    await _productCategoryRepository.AddAsync(productCategory);
                }
            }
            await _productRepository.UpdateAsync(product);
            
            return new UpdatedProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Img = product.Img,
                Price = product.Price,
                IsFeatured = product.IsFeatured,
              
            };

        }
    }
}