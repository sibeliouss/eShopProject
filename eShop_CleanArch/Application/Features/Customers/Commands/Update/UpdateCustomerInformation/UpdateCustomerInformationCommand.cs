using Application.Features.Customers.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Customers.Commands.Update.UpdateCustomerInformation;

public class UpdateCustomerInformationCommand : IRequest<UpdateCustomerInformationResponse>
{
    public UpdateCustomerInformationDto UpdateCustomerInformationDto { get; set; }
}

public class UpdateCustomerInformationCommandHandler : IRequestHandler<UpdateCustomerInformationCommand, UpdateCustomerInformationResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly UserManager<User> _userManager;

    public UpdateCustomerInformationCommandHandler(ICustomerRepository customerRepository, UserManager<User> userManager)
    {
   
        _customerRepository = customerRepository;
        _userManager = userManager;
    }

    public async Task<UpdateCustomerInformationResponse> Handle(UpdateCustomerInformationCommand request,
        CancellationToken cancellationToken)
    {
        // Customer'ı güncelle
        var customer = await _customerRepository.GetByIdAsync(request.UpdateCustomerInformationDto.Id);
        if (customer == null)
        {
            throw new Exception("Müşteri kaydı bulunamadı!");
        }

        customer.FirstName = request.UpdateCustomerInformationDto.FirstName;
        customer.LastName = request.UpdateCustomerInformationDto.LastName;
        customer.UserName = request.UpdateCustomerInformationDto.UserName;
        customer.Email = request.UpdateCustomerInformationDto.Email;

        await _customerRepository.UpdateAsync(customer);

        // User'ı güncelle
        var user = await _userManager.FindByIdAsync(customer.UserId.ToString());
        if (user == null)
        {
            throw new Exception("Kullanıcı kaydı bulunamadı!");
        }

        user.FirstName = request.UpdateCustomerInformationDto.FirstName;
        user.LastName = request.UpdateCustomerInformationDto.LastName;
        user.UserName = request.UpdateCustomerInformationDto.UserName;
        user.Email = request.UpdateCustomerInformationDto.Email;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception("Kullanıcı güncellenirken bir hata oluştu.");
        }

        // Güncellenen bilgileri geri döndür
        return new UpdateCustomerInformationResponse
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            UserName = customer.UserName,
            Email = customer.Email
        };
    }
}
