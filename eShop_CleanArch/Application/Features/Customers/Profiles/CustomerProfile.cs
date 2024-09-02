using Application.Features.Customers.Commands.Create;
using Application.Features.Customers.Commands.Update.UpdateCustomerInformation;
using Application.Features.Customers.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Customers.Profiles;

public class CustomerProfile : Profile
{
  public CustomerProfile()
  {
    CreateMap<Customer, CustomerDto>().ReverseMap();
    CreateMap<Customer, CreatedCustomerResponse>().ReverseMap();
    CreateMap<Customer, UpdateCustomerInformationDto>().ReverseMap();
    CreateMap<User, UpdateCustomerInformationDto>().ReverseMap();
    CreateMap<Customer, UpdateCustomerInformationResponse >().ReverseMap();
  }  
}