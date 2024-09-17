using Application.Features.Categories.Commands.Create;
using Application.Features.Categories.Commands.Update;
using Application.Features.Categories.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Categories.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, GetListCategoryDto>().ReverseMap();
        CreateMap<Category, CreatedCategoryResponse>().ReverseMap();
        CreateMap<Category, CreateCategoryCommand>().ReverseMap();
        CreateMap<UpdatedCategoryResponse, Category>().ReverseMap();
        CreateMap<UpdateCategoryCommand, Category>().ReverseMap();
    }
}