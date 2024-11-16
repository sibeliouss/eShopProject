using Application.Features.Users.Dtos;
using Domain.Entities;

namespace Application.Services.Users;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<IEnumerable<User>> GetAllUsersAsync();
    
    Task<User?> FindAsync(Guid id);
    
}