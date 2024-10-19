namespace Application.Features.Users.Dtos;

public class UpdateUserInformationDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string UserName { get; set; } 
    public string Email { get; set; } 
    
}