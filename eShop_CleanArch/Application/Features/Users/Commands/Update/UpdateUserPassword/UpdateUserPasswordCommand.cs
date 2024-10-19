using Application.Features.Users.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Commands.Update.UpdateUserPassword;

public class UpdateUserPasswordCommand : IRequest
{
    public UpdateUserPasswordDto UpdateUserPasswordDto { get; set; }
    
     public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<UpdateUserPasswordDto> _validator;

        public UpdateUserPasswordCommandHandler(IUserRepository userRepository, UserManager<User> userManager, IValidator<UpdateUserPasswordDto> validator)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _validator = validator;
        }

        public async Task Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var userDto = request.UpdateUserPasswordDto;
            
            var validationResult = await _validator.ValidateAsync(userDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByIdAsync(userDto.Id);
            if (user == null)
            {
                throw new Exception("Kayıt Bulunamadı!");
            }
            
            
            var passwordVerificationResult = await _userManager.CheckPasswordAsync(user, request.UpdateUserPasswordDto.CurrentPassword);
            if (!passwordVerificationResult)
            {
                throw new Exception("Mevcut şifre yanlış!");
            }
            
            if (request.UpdateUserPasswordDto.NewPassword != request.UpdateUserPasswordDto.ConfirmedPassword)
            {
                throw new Exception("Yeni şifreler uyuşmuyor!");
            }
            
            var result = await _userManager.ChangePasswordAsync(user, request.UpdateUserPasswordDto.CurrentPassword, request.UpdateUserPasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                throw new Exception("Şifre güncellenirken bir hata oluştu.");
            }
            
            // user.PasswordHash = user.PasswordHash;
            await _userRepository.UpdateAsync(user);
        }
    }
}