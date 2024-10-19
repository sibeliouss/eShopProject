using Application.Features.Users.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Users.Profiles;

public class UserProfile : Profile
{

    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}