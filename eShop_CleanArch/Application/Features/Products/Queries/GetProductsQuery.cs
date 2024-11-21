using Application.Features.Products.Dtos;
using Application.Features.Products.Queries.ResponseDtos;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Products.Queries;

    
public class GetProductsQuery : IRequest<List<GetProductResponse>>
{
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<GetProductResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<GetProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var currentDate = DateTime.Now; 
        var productsDto = await _productRepository.Query()
            .AsNoTracking()
            .Select(product => new GetProductResponse()
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
                    : null,
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
            }).ToListAsync(cancellationToken);

        return productsDto;
    }

}