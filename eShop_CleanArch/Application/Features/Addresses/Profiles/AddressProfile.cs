using Application.Features.Addresses.Commands.Create;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Addresses.Profiles;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Address, CreatedAddressResponse>().ReverseMap();

        CreateMap<CreateAddressCommand, Address>().ReverseMap();
    }
}