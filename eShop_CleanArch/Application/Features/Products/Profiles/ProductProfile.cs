using Application.Features.Products.Commands.Create;
using Application.Features.Products.Commands.Update;
using Application.Features.Products.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Products.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>().ReverseMap();
        CreateMap<CreatedProductResponse, Product>().ReverseMap();
        CreateMap<UpdateProductDto, Product>().ReverseMap();
        CreateMap<UpdatedProductResponse, Product>().ReverseMap();
    }
}