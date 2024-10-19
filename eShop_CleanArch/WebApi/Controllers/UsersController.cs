using Application.Features.Users.Commands.Delete;
using Application.Features.Users.Commands.Update.UpdateUserInformation;
using Application.Features.Users.Commands.Update.UpdateUserPassword;
using Application.Features.Users.Dtos;
using Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }  
    
    [HttpPut]
    public async Task<IActionResult> UpdateUserInformation([FromBody] UpdateUserInformationDto updateUserInformationDto)
    { 
        var command = new UpdateUserInformationCommand
        {
            UpdateUserInformationDto = updateUserInformationDto
        };
        
        try
        { 
            await _mediator.Send(command);
            return Ok("Müşteri bilgileri başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordDto updateUserPasswordDto)
    {
         
        var command = new UpdateUserPasswordCommand
        {
            UpdateUserPasswordDto = updateUserPasswordDto
        };
       
        await _mediator.Send(command);
        return Ok("Şifre başarıyla değiştirildi.");
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var query = new GetAllUsersQuery();
        var users = await _mediator.Send(query);

        if (!users.Any())
        {
            return NotFound("Müşteri bulunmadı.");
        }

        return Ok(users);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        
        var command = new DeleteUserCommand
        {
                
            UserId = id,
         
        };

        await _mediator.Send(command);

        return NoContent();
    }

}