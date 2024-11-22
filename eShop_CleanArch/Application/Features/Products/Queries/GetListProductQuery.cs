
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Products.Dtos;
using Application.Services.ProductCategories;
using Application.Services.Repositories;
using Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Products.Queries;

public class GetListProductQuery : IRequest<ResponseDto<List<ProductDto>>>
{
    public RequestDto RequestDto { get; set; }
    
    public GetListProductQuery(RequestDto requestDto)
    {
        RequestDto = requestDto;
    }

    public class GetListProductQueryHandler : IRequestHandler<GetListProductQuery, ResponseDto<List<ProductDto>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryService _productCategoryService;

        public GetListProductQueryHandler(IProductRepository productRepository, IProductCategoryService productCategoryService)
        {
            _productRepository = productRepository;
            _productCategoryService = productCategoryService;
        }

        public async Task<ResponseDto<List<ProductDto>>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.Now; 
            ResponseDto<List<ProductDto>> response = new();
            var replaceSearch = request.RequestDto.Search?.ToLower() ?? "";

            var query = _productRepository.Query().Include(p => p.ProductDetail).Where(p => p.IsActive && !p.IsDeleted);

            if (request.RequestDto.CategoryId != null) 
            {
                query = query.Include(p => p.ProductCategories)
                             .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == request.RequestDto.CategoryId));
            }

            if (!string.IsNullOrEmpty(request.RequestDto.OrderBy))
            {
                switch (request.RequestDto.OrderBy.ToLower())
                {
                    case "date":
                        query = query.OrderByDescending(p => p.CreateAt);
                        break;
                    case "price":
                        query = query.OrderBy(p => p.Price.Value);
                        break;
                    case "price-desc":
                        query = query.OrderByDescending(p => p.Price.Value);
                        break;
                    case "discounted-price":
                        query = query.OrderBy(p =>
                            p.ProductDiscount != null && p.ProductDiscount.StartDate <= currentDate && p.ProductDiscount.EndDate >= currentDate
                                ? p.ProductDiscount.DiscountedPrice
                                : p.Price.Value);
                        break;
                    case "discounted-price-desc":
                        query = query.OrderByDescending(p => p.ProductDiscount != null && p.ProductDiscount.StartDate <= currentDate && p.ProductDiscount.EndDate >= currentDate
                            ? p.ProductDiscount.DiscountedPrice
                            : p.Price.Value);
                        break;
                    default:
                        query = query.OrderBy(p => p.Id);
                        break;
                }
            }

            // Arama kriteri
            query = query.Where(p => p.Name.ToLower().Contains(replaceSearch) ||
                                     p.Brand.ToLower().Contains(replaceSearch));
            
            var productsDto = await query.AsNoTracking()
                .Skip((request.RequestDto.PageNumber - 1) * request.RequestDto.PageSize)
                .Take(request.RequestDto.PageSize)
                .Select(product => new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Brand = product.Brand,
                    Img = product.Img,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    ProductDetail = product.ProductDetail != null
                        ? new ProductDetailDto
                        {
                            Id = product.ProductDetail.Id,
                            ProductId = product.Id,
                            Barcode = product.ProductDetail.Barcode,
                            Description = product.ProductDetail.Description,
                            Material = product.ProductDetail.Material,
                            Fit = product.ProductDetail.Fit,
                            Size = product.ProductDetail.Size,
                            Color = product.ProductDetail.Color
                        }
                        : null, // ProductDetail null ise null olarak işleyin
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
                    ProductCategories = _productCategoryService.Query()
                        .Where(p => p.ProductId == product.Id)
                        .Select(s => new ProductCategoryDto
                        {
                            CategoryId = s.CategoryId,
                            CategoryName = s.Category.Name
                        })
                        .ToList()
                }).ToListAsync(cancellationToken);
 

            response.Data = productsDto;
            response.PageNumber = request.RequestDto.PageNumber;
            response.PageSize = request.RequestDto.PageSize;
            response.OrderBy = request.RequestDto.OrderBy;
            response.TotalPageCount = (int)Math.Ceiling(await query.CountAsync(cancellationToken) / (double)request.RequestDto.PageSize);
            response.IsFirstPage = request.RequestDto.PageNumber == 1;
            response.IsLastPage = request.RequestDto.PageNumber == response.TotalPageCount;

            return response;
        }
    }
}

