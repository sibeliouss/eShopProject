namespace Application.Features.Auth.Dtos;

public class UserDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
}