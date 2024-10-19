using Application.Services.Repositories;
using MediatR;

namespace Application.Features.Users.Commands.Delete;

public class DeleteUserCommand : IRequest
{
   public Guid UserId { get; set; } 
   
   public class DeleteUserCommandHandler: IRequestHandler<DeleteUserCommand>
   {
      private readonly IUserRepository _userRepository;

      public DeleteUserCommandHandler(IUserRepository userRepository)
      {
         _userRepository = userRepository;
      }
      public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
      {
         var user = await _userRepository.GetByIdAsync(request.UserId);
         if (user is null)
         {
            throw new Exception("Silinecek bir kayıt bulunmadı!");
            
         }

         await _userRepository.DeleteAsync(user);

      }
   }
}