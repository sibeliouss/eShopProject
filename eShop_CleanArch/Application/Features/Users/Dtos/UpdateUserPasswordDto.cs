namespace Application.Features.Users.Dtos;

public class UpdateUserPasswordDto
{
    public Guid Id { get; set; }
    public string CurrentPassword { get; set; } 
    public string NewPassword { get; set; } 
    public string ConfirmedPassword { get; set; } 
} 
