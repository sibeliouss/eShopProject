using Application.Features.Reviews.Commands.Create;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Reviews.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<CreateReviewCommand, Review>()
            .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.CreateAt)).ReverseMap();
    } 
}