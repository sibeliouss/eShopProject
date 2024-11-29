using Application.Features.Carts.Commands.Create;
using Application.Features.Carts.Dtos;
using Application.Features.Products.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Carts.Profiles;

public class CartProfile :Profile
{
    public CartProfile()
    {
        CreateMap<ShoppingCartDto, Cart>().ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => 1)).ReverseMap();

        CreateMap<Cart, CreatedShoppingCartResponse>().ReverseMap();
        //CreateMap<ProductDetail, ProductDetailDto>().ReverseMap();
        //CreateMap<ProductDiscount, ProductDiscountDto>().ReverseMap();

    }
}