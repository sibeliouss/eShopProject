using Application.Features.ProductDetails.Commands.Create;
using Application.Features.ProductDetails.Commands.Update;
using Application.Features.ProductDetails.Dtos;
using Application.Features.ProductDetails.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.ProductDetails.Profiles;

public class ProductDetailProfile : Profile
{
    public ProductDetailProfile()
    {
        CreateMap<ProductDetail, CreateProductDetailDto>().ReverseMap();
        CreateMap<ProductDetail, CreatedProductDetailResponse>().ReverseMap();
        CreateMap<ProductDetail, UpdateProductDetailDto>().ReverseMap();
        CreateMap<ProductDetail, UpdatedProductDetailResponse>().ReverseMap();
        CreateMap<ProductDetail, GetAllProductDetailResponse>().ReverseMap();
    }
}