using Application.Features.Customers.Dtos;
using Application.Services.Repositories;
using MediatR;

namespace Application.Features.Customers.Commands.Update;

public class UpdateCustomerInformationCommand : IRequest<UpdateCustomerInformationResponse>
{
    public UpdateCustomerInformationDto UpdateCustomerInformationDto { get; set; }
}

public class
    UpdateCustomerInformationCommandHandler : IRequestHandler<UpdateCustomerInformationCommand,
    UpdateCustomerInformationResponse>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerInformationCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<UpdateCustomerInformationResponse> Handle(UpdateCustomerInformationCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.UpdateCustomerInformationDto.Id);
        if (customer == null)
        {
            throw new Exception("Kayıt Bulunamadı!");
        }

        customer.FirstName = request.UpdateCustomerInformationDto.FirstName;
        customer.LastName = request.UpdateCustomerInformationDto.LastName;
        customer.UserName = request.UpdateCustomerInformationDto.UserName;
        customer.Email = request.UpdateCustomerInformationDto.Email;

        await _customerRepository.UpdateAsync(customer);

        // Güncellenmiş bilgileri geri döndüren response nesnesi oluşturuluyor
        var response = new UpdateCustomerInformationResponse
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            UserName = customer.UserName,
            Email = customer.Email
        };

        return response;
    }
}