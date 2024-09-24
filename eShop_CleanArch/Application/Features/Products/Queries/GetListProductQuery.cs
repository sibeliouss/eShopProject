using Application.Features.Products.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries;

public class GetListProductQuery: IRequest<ResponseDto<List<ProductDto>>>
{
    public RequestDto RequestDto { get; set; }
    
    public GetListProductQuery(RequestDto requestDto)
    {
        RequestDto = requestDto;
    }

    public class GetListProductQueryHandler : IRequestHandler<GetListProductQuery, ResponseDto<List<ProductDto>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public GetListProductQueryHandler(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task<ResponseDto<List<ProductDto>>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
    {
        ResponseDto<List<ProductDto>> response = new();
        var replaceSearch = request.RequestDto.Search?.ToLower() ?? "";

        
        var query = _productRepository.Query().Include(p=>p.ProductDetail).Where(p => p.IsActive && !p.IsDeleted);

        
        if (request.RequestDto.CategoryId != Guid.Empty) 
        {
            query = query.Include(p => p.ProductCategories)
                         .Where(p => p.ProductCategories != null && p.ProductCategories.Any(pc => pc.CategoryId == request.RequestDto.CategoryId));
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
                ProductCategories = _productCategoryRepository.Query()
                                        .Where(p => p.ProductId == product.Id)
                                        .Select(s => new ProductCategoryDto
                                        {
                                            CategoryId = s.CategoryId,
                                            CategoryName = s.Category.Name
                                        })
                                        .ToList() // async çağrılar kullanılabilir
            }).ToListAsync(cancellationToken); // ToListAsync ile async desteklenir.

       
        response.Data = productsDto;
        response.PageNumber = request.RequestDto.PageNumber;
        response.PageSize = request.RequestDto.PageSize;
        response.OrderBy = request.RequestDto.OrderBy;
        response.TotalPageCount = (int)Math.Ceiling(await query.CountAsync(cancellationToken) / (double)request.RequestDto.PageSize); // async count
        response.IsFirstPage = request.RequestDto.PageNumber == 1;
        response.IsLastPage = request.RequestDto.PageNumber == response.TotalPageCount;

        return response;
    }
}



}