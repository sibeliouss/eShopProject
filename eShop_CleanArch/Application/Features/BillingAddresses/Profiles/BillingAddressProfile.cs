using Application.Features.Addresses.Queries;
using Application.Features.BillingAddresses.Commands.Create;
using Application.Features.BillingAddresses.Commands.Update;
using Application.Features.BillingAddresses.Dtos;
using Application.Features.BillingAddresses.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.BillingAddresses.Profiles;

public class BillingAddressProfile : Profile
{
   public BillingAddressProfile()
   {
      CreateMap<BillingAddress, CreateBillingAddressDto>().ReverseMap();
      CreateMap<BillingAddress, CreatedBillingAddressResponse>().ReverseMap();
      CreateMap<BillingAddress, UpdateBillingAddressDto>().ReverseMap();
      CreateMap<BillingAddress, UpdatedBillingAddressResponse>().ReverseMap();
      CreateMap<BillingAddress, GetListBillingAddressQueryResponse>().ReverseMap();
   } 
}