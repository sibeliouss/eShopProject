using Application.Features.Auth.Login;
using Application.Features.Auth.Register;
using Application.Features.Customers.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Auth.Profiles;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterCommand, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()).ReverseMap(); 

        CreateMap<User, CreateCustomerDto>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash)).ReverseMap();

    }
}