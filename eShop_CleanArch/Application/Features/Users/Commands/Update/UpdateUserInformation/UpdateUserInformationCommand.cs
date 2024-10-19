using Application.Features.Users.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Commands.Update.UpdateUserInformation;

public class UpdateUserInformationCommand : IRequest<UpdatedUserInformationResponse>
{
    public UpdateUserInformationDto UpdateUserInformationDto { get; set; }
    
    public class UpdateUserInformationCommandHandler : IRequestHandler<UpdateUserInformationCommand, UpdatedUserInformationResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UpdateUserInformationDto> _validator;

        public UpdateUserInformationCommandHandler(IUserRepository userRepository, IValidator<UpdateUserInformationDto> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }
        public async Task<UpdatedUserInformationResponse> Handle(UpdateUserInformationCommand request, CancellationToken cancellationToken)
        {
            var userDto = request.UpdateUserInformationDto;
            
            var validationResult = await _validator.ValidateAsync(userDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByIdAsync(userDto.Id);
            if (user is null)
            {
                throw new Exception("Kullanıcı bulunamadı!");
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.UserName = userDto.UserName;
            user.Email = userDto.Email;

            await _userRepository.UpdateAsync(user);

            return new UpdatedUserInformationResponse()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

        }
    }
}