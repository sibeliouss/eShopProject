
using Application.Features.Products.Dtos;
using Application.Services.ProductCategories;
using Application.Services.Repositories;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductDto = Application.Features.Products.Queries.ResponseDtos.ProductDto;

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
            ResponseDto<List<ProductDto>> response = new();
            var replaceSearch = request.RequestDto.Search?.ToLower() ?? "";

            var query = _productRepository.Query().Include(p => p.ProductDetail).Where(p => p.IsActive && !p.IsDeleted);

            if (request.RequestDto.CategoryId != null) 
            {
                query = query.Include(p => p.ProductCategories)
                             .Where(p => p.ProductCategories!.Any(pc => pc.CategoryId == request.RequestDto.CategoryId));
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
                    CreateAt = product.CreateAt,
                    Img = product.Img,
                    Price = product.Price,
                    Quantity = product.Quantity,
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

