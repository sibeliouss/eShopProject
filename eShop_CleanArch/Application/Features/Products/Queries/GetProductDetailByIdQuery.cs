using Application.Features.Products.Constants;
using Application.Features.Products.Dtos;
using Application.Services.ProductCategories;
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
        private readonly IProductCategoryService _productCategoryService;

        public GetProductDetailByIdQueryHandler(IProductRepository productRepository, IProductCategoryService productCategoryService)
        {
            _productRepository = productRepository;
            _productCategoryService = productCategoryService;

        }
        public async Task<ProductDto> Handle(GetProductDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow; 
            var product = await _productRepository.Query().Include(p => p.ProductDetail).Include(p=>p.ProductDiscount)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);
            if (product is null)
            {
                throw new Exception(ProductMessages.ProductNotFound);
            }

            var productDto = new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Quantity = product.Quantity,
                ProductDetail = new ProductDetailDto()
                {
                    Id = product.ProductDetail!.Id,
                    ProductId = product.Id,
                    Barcode = product.ProductDetail.Barcode,
                    Description = product.ProductDetail.Description,
                    Material = product.ProductDetail.Material,
                    Fit = product.ProductDetail.Fit,
                    Size = product.ProductDetail.Size,
                    Color = product.ProductDetail.Color
                },
                ProductDiscount = product.ProductDiscount != null &&
                                  product.ProductDiscount.StartDate <= currentDate &&
                                  product.ProductDiscount.EndDate >= currentDate
                    ? new ProductDiscountDto()
                    {
                        Id = product.ProductDiscount.Id,
                        ProductId = product.Id,
                        DiscountedPrice = product.ProductDiscount.DiscountedPrice,
                        DiscountPercentage = product.ProductDiscount.DiscountPercentage,
                        StartDate = product.ProductDiscount.StartDate,
                        EndDate = product.ProductDiscount.EndDate
                    }
                    : null,
                Img = product.Img,
                Price = product.Price,
                CreateAt = product.CreateAt,
                ProductCategories = await _productCategoryService.Query().Where(pc=>pc.ProductId==product.Id).Select(pc=>new ProductCategoryDto()
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