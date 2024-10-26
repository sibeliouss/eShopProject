using Application.Features.WishLists.Commands.Create;
using Application.Features.WishLists.Commands.Delete;
using Application.Features.WishLists.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class WishListsController : ControllerBase
{
  
    private readonly IMediator _mediator;
    
    public WishListsController(IMediator mediator)
    {
        _mediator = mediator;
    } 
    
    [HttpPost]
    public async Task<IActionResult> AddToWishList([FromBody] CreateWishListCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteWishListCommand() { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetAllWishLists(Guid userId)
    {
        var query = new GetAllWishListQuery { UserId = userId };
        var result = await _mediator.Send(query);

        return Ok(result);
    }
    
    
}