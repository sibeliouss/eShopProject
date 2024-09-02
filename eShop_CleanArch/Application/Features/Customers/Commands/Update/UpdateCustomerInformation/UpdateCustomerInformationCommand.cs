using Application.Features.Customers.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace Application.Features.Customers.Commands.Update.UpdateCustomerInformation
{
    public class UpdateCustomerInformationCommand : IRequest<UpdateCustomerInformationResponse>
    {
        public UpdateCustomerInformationDto UpdateCustomerInformationDto { get; set; }
    }

    public class UpdateCustomerInformationCommandHandler : IRequestHandler<UpdateCustomerInformationCommand, UpdateCustomerInformationResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UpdateCustomerInformationCommandHandler(ICustomerRepository customerRepository, UserManager<User> userManager, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UpdateCustomerInformationResponse> Handle(UpdateCustomerInformationCommand request,
            CancellationToken cancellationToken)
        {
            // Customer
            var customer = await _customerRepository.GetByIdAsync(request.UpdateCustomerInformationDto.Id);
            if (customer == null)
            {
                throw new Exception("Müşteri kaydı bulunamadı!");
            }

            _mapper.Map(request.UpdateCustomerInformationDto, customer);

            await _customerRepository.UpdateAsync(customer);

            // User'ı da güncelle
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
            
            var response = _mapper.Map<UpdateCustomerInformationResponse>(customer);
            return response;
        }
    }
}
