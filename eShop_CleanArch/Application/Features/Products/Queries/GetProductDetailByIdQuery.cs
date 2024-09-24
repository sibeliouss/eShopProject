using Application.Features.Products.Dtos;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries;

public class GetProductDetailByIdQuery:IRequest<ProductDto>
{
    public Guid Id { get; set; }
    
    public class GetProductDetailByIdQueryHandler: IRequestHandler<GetProductDetailByIdQuery,ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public GetProductDetailByIdQueryHandler(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }
        public async Task<ProductDto> Handle(GetProductDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.Query().Include(p => p.ProductDetail)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);
            if (product is null)
            {
                throw new Exception("Ürün bulunmadı!");
            }

            var productDto = new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                ProductDetail = new ProductDetailDto()
                {
                    Id = product.ProductDetail!.Id,
                    ProductId = product.Id,
                    Brand = product.ProductDetail.Brand,
                    Barcode = product.ProductDetail.Barcode,
                    Stock = product.ProductDetail.Stock,
                    Description = product.ProductDetail.Description,
                    Material = product.ProductDetail.Material,
                    Fit = product.ProductDetail.Fit,
                    Size = product.ProductDetail.Size,
                    Color = product.ProductDetail.Color
                },
                Img = product.Img,
                Price = product.Price,
                CreateAt = product.CreateAt,
                ProductCategories = await _productCategoryRepository.Query().Where(pc=>pc.ProductId==product.Id).Select(pc=>new ProductCategoryDto()
                    {
                        CategoryId = pc.CategoryId,
                        CategoryName = pc.Category.Name
                    }
              ).ToListAsync(cancellationToken),
            };

            return productDto;
        }
    }
}