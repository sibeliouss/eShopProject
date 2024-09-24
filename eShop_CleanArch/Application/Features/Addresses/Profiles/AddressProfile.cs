using Application.Features.Addresses.Commands.Create;
using Application.Features.Addresses.Commands.Update;
using Application.Features.Addresses.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Addresses.Profiles;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Address, CreatedAddressResponse>().ReverseMap();
        CreateMap<CreateAddressCommand, Address>().ReverseMap();
        CreateMap<UpdateAddressCommand, Address>().ReverseMap();
        CreateMap<UpdatedAddressResponse, Address>().ReverseMap();
        CreateMap<GetListAddressQueryResponse, Address>().ReverseMap();
    }
}