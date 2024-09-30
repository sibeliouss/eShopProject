using Application.Features.ProductDiscounts.Commands.Create;
using Application.Features.ProductDiscounts.Commands.Update;
using Application.Features.ProductDiscounts.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.ProductDiscounts.Profiles;

public class ProductDiscountProfile : Profile
{
   public ProductDiscountProfile()
   {
      CreateMap<ProductDiscount, CreateProductDiscountDto>().ReverseMap();
      CreateMap<ProductDiscount, UpdateProductDiscountDto>().ReverseMap();
      CreateMap<ProductDiscount, CreatedProductDiscountResponse>().ReverseMap();
      CreateMap<ProductDiscount, UpdatedProductDiscountResponse>().ReverseMap();
   }
}