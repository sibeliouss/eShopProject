using Application.Features.Auth.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Queries;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public Guid UserId { get; set; }

    public GetUserByIdQuery(Guid userId)
    {
        UserId = userId;
    }


    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly UserManager<User> _userManager;

        public GetUserByIdQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var getUserById = await _userManager.Users.Where(u => u.Id == request.UserId).Select(u =>
                    new UserDto // Kullanıcıyı DTO olarak dönüştür
                    {
                        UserId = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        UserName = u.UserName

                    })
                .FirstOrDefaultAsync(cancellationToken);
           // if (getUserById is null) throw new Exception("Kullanıcı mevcut değil.");

            return getUserById;
            /*return new UserDto()
            {
                UserId = getUserById.Id,
                FirstName = getUserById.FirstName,
                LastName = getUserById.LastName,
                Email = getUserById.Email,
                UserName = getUserById.UserName

            };
        }*/
        }
    }
}